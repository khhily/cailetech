using CL.Entity.Json.WebAPI;
using CL.Enum.Common;
using CL.Enum.Common.Lottery;
using CL.Game.DAL;
using CL.Redis.BLL;
using CL.Coupons.DAL;
using CL.Coupons.Entity;
using CL.Tools.Common;
using CL.View.Entity.Coupons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Coupons.BLL
{
    public class CouponsBLL
    {
        Log log = new Log("CouponsBLL");
        CouponsDAL dal = new CouponsDAL(DbConnectionEnum.CaileCoupons);

        protected const string Currency = "全场通用";

        /// <summary>
        /// 查询可用彩券
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OrderMoney"></param>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<CouponsEntity> QueryCouponsPayment(long UserID, long OrderMoney, int LotteryCode)
        {
            return dal.QueryCouponsPayment(UserID, OrderMoney, LotteryCode);
        }
        /// <summary>
        /// 查询可用彩券
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OrderMoney"></param>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<CouponsEntity> QueryCoupons(long UserID, bool IsCoupons, int PageIndex, int PageSize, ref int Counts)
        {
            return dal.QueryCoupons(UserID, IsCoupons, PageIndex, PageSize, ref Counts);
        }

        /// <summary>
        /// 查询彩券列表
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="LotteryCode"></param>
        /// <param name="CouponsStatus"></param>
        /// <param name="CouponsType"></param>
        /// <param name="CouponsSource"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RecordCount"></param>
        /// <returns></returns>
        public List<udv_CouponsList> QueryCouponsList(string UserName, int LotteryCode, int CouponsStatus, int CouponsType, int CouponsSource, DateTime StartTime, DateTime EndTime, int PageIndex, int PageSize, ref int RecordCount, ref long RecordFaceValue, ref long RecordEmploy)
        {
            return dal.QueryCouponsList(UserName, LotteryCode, CouponsStatus, CouponsType, CouponsSource, StartTime, EndTime, PageIndex, PageSize, ref RecordCount, ref RecordFaceValue, ref RecordEmploy);
        }
        /// <summary>
        /// 查询兑换码报表
        /// </summary>
        /// <param name="PartnerCode"></param>
        /// <param name="TimeType"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<udv_ReportCouponsList> QueryReportCoupons(int TimeType, DateTime StartTime, DateTime EndTime)
        {
            return dal.QueryReportCoupons(TimeType, StartTime, EndTime);
        }
        /// <summary>
        /// 注册送彩券
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ActivityID"></param>
        /// <param name="Amount"></param>
        /// <param name="LotteryCode"></param>
        /// <param name="Day"></param>
        /// <returns></returns>
        public bool RegisterGiveCoupons(long UserID, long Amount = 0)
        {
            try
            {
                int ActivityID = ConfigHelper.GetConfigInt("KEYACTIVITYID");
                if (Amount == 0)
                    Amount = Convert.ToInt64(ConfigHelper.Get("KEYAMOUNT"));
                int LotteryCode = ConfigHelper.GetConfigInt("KEYLOTTERYCODE");
                int Day = ConfigHelper.GetConfigInt("KEYDAY");
                bool Rec = dal.RegisterGiveCoupons(UserID, ActivityID, Amount, LotteryCode, Day);
                if (Rec)
                    Task.Factory.StartNew(() =>
                    {
                        new UsersDAL(DbConnectionEnum.CaileGame).SetSystemStaticdata(DateTime.Now.ToString("yyyy-MM-dd"), 6, Amount, 0, 0);
                    });

                return Rec;
            }
            catch
            {
                return false;
            }


        }
        /// <summary>
        /// 查询即将过期彩券推送给用户
        /// </summary>
        /// <returns></returns>
        public List<udv_CouponsExpireTimeList> QueryCouponsExpireTimeList()
        {
            return dal.QueryCouponsExpireTimeList();
        }

        public CouponsEntity QueryEntity(long CouponsID)
        {
            return dal.Get(CouponsID);
        }

        #region 自定义方法
        /// <summary>
        /// 查询可支付彩券
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OrderMoney"></param>
        /// <param name="LotteryCode"></param>
        /// <param name="BuyType"></param>
        /// <returns></returns>
        public CouponsListResult QueryCouponsPaymentList(long UserID, long OrderMoney, int LotteryCode, int BuyType)
        {

            CouponsListResult result = null;
            try
            {

                var Entitys = this.QueryCouponsPayment(UserID, OrderMoney, LotteryCode);
                if (Entitys == null || Entitys.Count == 0)
                    result = new CouponsListResult()
                    {
                        Code = (int)ResultCode.NullData,
                        Msg = Common.GetDescription(ResultCode.NullData)
                    };
                else
                {
                    if (BuyType == 1)
                        Entitys = Entitys.Where(w => w.IsChaseTask == true).ToList();
                    if (Entitys == null || Entitys.Count == 0)
                        result = new CouponsListResult()
                        {
                            Code = (int)ResultCode.NullData,
                            Msg = Common.GetDescription(ResultCode.NullData)
                        };
                    else
                        result = new CouponsListResult()
                        {
                            Code = (int)ResultCode.Success,
                            Msg = Common.GetDescription(ResultCode.Success),
                            Data = Entitys.Select(s => new CouponsListData()
                            {
                                Balance = s.Balance,
                                FaceValue = s.FaceValue,
                                IsChaseTask = s.IsChaseTask,
                                IsSuperposition = s.IsSuperposition,
                                LotteryCode = s.LotteryCode,
                                CouponsID = s.CouponsID,
                                StartTime = s.StartTime == null ? "" : Convert.ToDateTime(s.StartTime).ToString("yyyyMMddHHmmss"),
                                ExpireTime = s.ExpireTime == null ? "" : Convert.ToDateTime(s.ExpireTime).ToString("yyyyMMddHHmmss"),
                                SatisfiedMoney = s.SatisfiedMoney,
                                CouponsType = s.CouponsType
                            }).ToList()
                        };
                }
            }
            catch (Exception ex)
            {
                log.Write("查询可支付彩券错误：" + ex.Message, true);
                result = new CouponsListResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return result;
        }
        /// <summary>
        /// 查询彩券列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public CouponsListResult QueryCouponsList(long UserID, bool IsCoupons, int PageIndex, int PageSize)
        {
            CouponsListResult result = null;
            try
            {
                int Count = 0;
                var Entitys = this.QueryCoupons(UserID, IsCoupons, PageIndex, PageSize, ref Count);
                if (Entitys == null || Entitys.Count == 0)
                    result = new CouponsListResult()
                    {
                        Code = (int)ResultCode.NullData,
                        Msg = Common.GetDescription(ResultCode.NullData)
                    };
                else
                    result = new CouponsListResult()
                    {
                        Code = (int)ResultCode.Success,
                        Msg = Common.GetDescription(ResultCode.Success),
                        Data = Entitys.Select(s => new CouponsListData()
                        {
                            Balance = s.Balance,
                            FaceValue = s.FaceValue,
                            IsChaseTask = s.IsChaseTask,
                            IsSuperposition = s.IsSuperposition,
                            LotteryCode = s.LotteryCode,
                            CouponsID = s.CouponsID,
                            StartTime = s.StartTime == null ? "" : Convert.ToDateTime(s.StartTime).ToString("yyyyMMddHHmmss"),
                            ExpireTime = s.ExpireTime == null ? "" : Convert.ToDateTime(s.ExpireTime).ToString("yyyyMMddHHmmss"),
                            SatisfiedMoney = s.SatisfiedMoney,
                            CouponsType = s.CouponsType
                        }).ToList(),
                        Counts = Count
                    };
            }
            catch (Exception ex)
            {
                log.Write("查询彩券列表错误：" + ex.Message, true);
                result = new CouponsListResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }

            return result;
        }

        public bool GenerateCDKeys(int ActivityID, int CouponsProxy, int LotteryCode, int CouponsType, DateTime StartTime, DateTime ExpireTime, DateTime KeyExpireTime, long SatisfiedMoney, long FaceValue, bool IsGive, bool IsChaseTask, bool IsSuperposition, bool IsTimes, bool IsJoinBuy, int CouponsCount)
        {
            try
            {
                DateTime time = DateTime.Now;
                List<CouponsCDKeyEntity> CDKeyEntitys = new List<CouponsCDKeyEntity>();
                List<CouponsEntity> CouponsEntitys = new List<CouponsEntity>();
                CouponsEntity Entity = new CouponsEntity();
                CouponsCDKeyEntity KeyEntity = new CouponsCDKeyEntity();
                for (int i = 0; i < CouponsCount; i++)
                {
                    string cdkey = string.Format("{0}{1}{2}", time.ToString("yyMMdd"), CouponsProxy, GenerateRandom());
                    string encryptcdkey = EncryptAlgorithm.CustomEncryptKey(cdkey);
                    #region 组装
                    KeyEntity = new CouponsCDKeyEntity();
                    KeyEntity.EncryptKey = encryptcdkey;
                    KeyEntity.CDKey = cdkey;
                    KeyEntity.IsExchanger = false;
                    KeyEntity.ExchangerUserID = 0;
                    KeyEntity.ExpireTime = KeyExpireTime;
                    KeyEntity.GenerateTime = time;
                    KeyEntity.PartnerCode = CouponsProxy.ToString();
                    CDKeyEntitys.Add(KeyEntity);
                    Entity = new CouponsEntity();
                    Entity.ActivityID = ActivityID;
                    Entity.Balance = FaceValue * 100;
                    Entity.FaceValue = FaceValue * 100;
                    if (CouponsType != 3)
                        Entity.ExpireTime = ExpireTime;
                    Entity.GenerateTime = time;
                    Entity.IsChaseTask = IsChaseTask;
                    Entity.IsGive = IsGive;
                    Entity.IsJoinBuy = IsJoinBuy;
                    Entity.IsSuperposition = IsSuperposition;
                    Entity.IsTimes = IsTimes;
                    Entity.LotteryCode = LotteryCode;
                    Entity.CouponsStatus = 0;
                    Entity.CouponsType = CouponsType;
                    Entity.CouponsSource = 0;
                    Entity.SatisfiedMoney = CouponsType == 2 ? SatisfiedMoney * 100 : 0;
                    Entity.StartTime = StartTime;
                    Entity.UserID = 0;
                    CouponsEntitys.Add(Entity);
                    #endregion
                }
                return dal.GenerateCoupons(CDKeyEntitys, CouponsEntitys);
            }
            catch
            {
                throw;
            }
        }
        public string GenerateRandom()
        {
            string rec = string.Empty;
            string[] character = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M", "z", "a", "q", "w", "s", "x", "c", "d", "e", "r", "f", "v", "b", "g", "t", "y", "h", "n", "m", "j", "u", "i", "k", "l", "o", "p" };
            Random ran = new Random(Utils.GenerateRandomSeed());
            for (int i = 0; i < 6; i++)
                rec += character[ran.Next(0, 25)];
            return rec;
        }

        #endregion
    }
}
