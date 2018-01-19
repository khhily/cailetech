using CL.Enum.Common;
using CL.Enum.Common.Lottery;
using CL.Enum.Common.Type;
using CL.Game.DAL;
using CL.Game.Entity;
using CL.Json.Entity;
using CL.Redis.BLL;
using CL.Tools.Common;
using CL.Tools.MSMQManager;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CL.Game.BLL.View
{
    public class udv_SpecialBLL
    {
        Log log = new Log("udv_SpecialBLL");
        SchemesDAL SchemeDAL = new SchemesDAL(DbConnectionEnum.CaileGame);
        SchemesBLL SchemeBLL = new SchemesBLL();
        /// <summary>
        /// 投注
        /// </summary>
        /// <param name="UserCode">用户编号</param>
        /// <param name="LotteryCode">彩种编号</param>
        /// <param name="IsuseNum">期号</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="BuyType">投注类型</param>
        /// <param name="Amount">投注金额</param>
        /// <param name="RoomCode">房间编号</param>
        /// <param name="BetData">投注内容</param>
        /// <returns></returns>
        public JsonResult SpecialBet(long UserCode, int LotteryCode, string IsuseNum, string BeginTime, string EndTime, int BuyType, long Amount, string RoomCode, string BetData)
        {
            JsonResult result = null;
            try
            {
                log.Write("开始投注：" + UserCode);
                string AvatarUrl = string.Empty;
                string Nick = string.Empty;
                //最小投注金额
                long MinBuyAmount = 1000;
                long.TryParse(ConfigHelper.GetConfigString("MINBUYAMOUNT") ?? "", out MinBuyAmount);
                //最大投注金额
                long MaxBuyAmount = 2000000;
                long.TryParse(ConfigHelper.GetConfigString("MAXBUYAMOUNT") ?? "", out MaxBuyAmount);
                ResultCode RecCode = ResultCode.Success;
                //验证是否停止销售
                var lots = new SystemRedis().ApplyLotteryDataRedis();
                if (lots == null || lots.Count == 0)
                    lots = new LotteriesBLL().SystemLottyData();
                var lotteryEntity = lots.Where(w => w.LotteryCode == LotteryCode).FirstOrDefault();
                if (lotteryEntity != null && lotteryEntity.LotteryStatus == 4)
                    RecCode = ResultCode.LotStop;
                else
                {
                    DateTime BTime = DateTime.Now, ETime = DateTime.Now;
                    bool IsSuccess = this.SetIssueTime(BeginTime, EndTime, ref BTime, ref ETime);
                    if (!IsSuccess)
                        RecCode = ResultCode.SystemBusy;
                    else
                    {
                        SchemesEntity Entity = new SchemesEntity();
                        List<udv_Parameter> RetuList = new List<udv_Parameter>();
                        string NumberNew = (BetData.Replace("\\\"", "\""));
                        List<udv_SchemeBetData> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<udv_SchemeBetData>>(NumberNew);
                        if (list != null && list.Count > 0)
                        {
                            foreach (var item in list)
                                foreach (var Data in item.Data)
                                    RetuList.Add(new udv_Parameter()
                                    {
                                        Amount = Amount,
                                        Bet = 1,
                                        IsNorm = false,
                                        LotteryCode = LotteryCode,
                                        Multiple = Convert.ToInt32(Amount / 100),
                                        Number = Data.Number,
                                        PlayCode = item.PlayCode,
                                        UserCode = UserCode
                                    });
                            if (Amount < MinBuyAmount)
                                RecCode = ResultCode.MinAmount;
                            else if (Amount > MaxBuyAmount)
                                RecCode = ResultCode.MaxAmount;
                            else
                            {
                                Entity.IsSplit = true;
                                Random R = new Random();
                                Entity.SchemeNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff") + UserCode + R.Next(1000, 9999);
                                Entity.LotteryNumber = BetData;
                                Entity.LotteryCode = LotteryCode;
                                Entity.SchemeMoney = Amount;
                                Entity.IsuseName = IsuseNum;
                                Entity.InitiateUserID = UserCode;
                                Entity.SchemeID = 0;
                                Entity.BuyType = (byte)BuyType;
                                Entity.RoomCode = RoomCode;
                                long ChaseTaskDetailsID = 0;
                                long SchemeID = SchemeDAL.SubmitOrder(Entity, RetuList, null, null, (int)PaymentTypeEnum.Balance, 0, ref ChaseTaskDetailsID);
                                if (SchemeID > 0)
                                {
                                    Entity.SchemeID = SchemeID;
                                    // Redis数据扣费
                                    new UsersBLL().ModifyUserBalanceRedis(UserCode, Amount, false);
                                    #region 查询方案详情信息
                                    List<SchemesDetailEntity> SchemesDetailEntits = new SchemesDetailBLL().QueryEntityBySchemeID(SchemeID);
                                    RetuList = SchemesDetailEntits.Select(s => new udv_Parameter()
                                    {
                                        SDID = s.SDID,
                                        Amount = s.BetMoney,
                                        Bet = s.BetNum,
                                        IsNorm = s.IsNorm == 1 ? true : false,
                                        Multiple = s.Multiple,
                                        Number = s.BetNumber,
                                        PlayCode = s.PlayCode,
                                        SchemeID = SchemeID
                                    }).ToList();
                                    #endregion
                                    //生成Redis方案对象
                                    SchemeBLL.GenerateRedisEntity(ChaseTaskDetailsID, Entity, SchemesDetailEntits, ETime);
                                    var UserEntity = new SystemRedis().SignInByUserCodeRedis(UserCode);
                                    if (UserEntity != null)
                                    {
                                        Nick = UserEntity.Nick;
                                        AvatarUrl = UserEntity.AvatarUrl;
                                    }
                                    ElectronicTicketSender ticket = new ElectronicTicketSender();
                                    ticket.SchemeTicket.SchemeID = SchemeID;
                                    ticket.SchemeTicket.IsRobot = false;
                                    ticket.SchemeTicket.LotteryCode = Entity.LotteryCode;
                                    ticket.SchemeTicket.StartTime = BTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    ticket.SchemeTicket.EndTime = ETime.ToString("yyyy-MM-dd HH:mm:ss");
                                    ticket.SchemeTicket.IsuseName = Entity.IsuseName;
                                    ticket.SchemeTicket.SchemeMoney = Amount;
                                    ticket.SchemeTicket.TicketDetails = RetuList;
                                    ticket.SchemeTicket.ChaseTaskDetailsID = ChaseTaskDetailsID;
                                    if (!ticket.Sender())
                                    {
                                        //撤单
                                        SchemeBLL.RevokeRedisSchemeEntity(SchemeID, ChaseTaskDetailsID);
                                        RecCode = ResultCode.SystemBusy;
                                        log.Write(string.Format("投注[订单号为【{0}】,发送拆票系统失败", SchemeID), true);
                                    }
                                    else
                                    {
                                        string errorinfo = string.Empty;
                                        new IM.Communication().Send_Api_Chatrooms_Bet(Nick, RoomCode, IsuseNum, Common.GetDescription((LotteryInfo)LotteryCode), SchemeID, (byte)BuyType, Amount, AvatarUrl, BetData, ref errorinfo);
                                        #region 变更静态数据
                                        Task.Factory.StartNew(() =>
                                        {
                                            new UsersBLL().Staticdata_Buy(Amount, LotteryCode);
                                        });
                                        #endregion
                                    }
                                }
                                else if (SchemeID == -42) //当期投注参数不正确
                                    result = new JsonResult()
                                    {
                                        Code = (int)ResultCode.IssueIncorrectParameter,
                                        Msg = Common.GetDescription(ResultCode.IssueIncorrectParameter)
                                    };
                                else if (SchemeID == -5)  //未付款订单
                                    result = new JsonResult()
                                    {
                                        Code = (int)ResultCode.InsufficientBalance,
                                        Msg = Common.GetDescription(ResultCode.InsufficientBalance)
                                    };
                                else //系统繁忙
                                    result = new JsonResult()
                                    {
                                        Code = (int)ResultCode.SystemBusy,
                                        Msg = Common.GetDescription(ResultCode.SystemBusy)
                                    };
                            }
                        }
                        else
                            RecCode = ResultCode.BettingInfoNull;
                    }
                }
                result = new JsonResult()
                {
                    Code = (int)RecCode,
                    Msg = Common.GetDescription(RecCode)
                };
            }
            catch (Exception ex)
            {
                log.Write(string.Format("投注失败：{0}", ex.Message), true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return result;
        }
        protected bool SetIssueTime(string BeginTime, string EndTime, ref DateTime BTime, ref DateTime ETime)
        {
            try
            {
                BTime = DateTime.ParseExact(BeginTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                ETime = DateTime.ParseExact(EndTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
