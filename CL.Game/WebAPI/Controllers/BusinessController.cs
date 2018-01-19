using CL.Game.BLL;
using CL.Json.Entity;
using CL.Json.Entity.WebAPI;
using CL.Tools.LotteryTrendChart;
using System;
using System.Web.Http;
using CL.Tools.Common;
using CL.Enum.Common;
using CL.Enum.Common.LotteryTrendChart;
using CL.WebAPI.Filters;
using CL.Entity.Json.WebAPI;
using CL.Coupons.BLL;
using WebAPI.Models;
using System.Threading.Tasks;

namespace CL.WebAPI.Controllers
{
    public class BusinessController : BaseController
    {
        #region ---------- 期号 开奖 方案 追号 投注 电子票---------- 

        /// <summary>
        /// 当前期信息
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="LotteryCode">彩种编号</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("CurrentIsuseInfo")]
        public async Task<string> CurrentIsuseInfo()
        {
            CurrentIsuseInfoResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                int LotteryCode = 0;
                int.TryParse(dic["LotteryCode"].ToString(), out LotteryCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new IsusesBLL().CurrentIsuseInfo(LotteryCode);
                else
                    result = new CurrentIsuseInfoResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("当前期信息错误:" + ex.Message, true);
                result = new CurrentIsuseInfoResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 开奖列表
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="LotteryCode">彩种编号</param>
        /// <param name="PageNumber">页码</param>
        /// <param name="RowsPerPage">页行</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("OfLotteryRecord")]
        public async Task<string> OfLotteryRecord()
        {
            OfLotteryResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                int LotteryCode = 0;
                int PageNumber = 0;
                int RowsPerPage = 0;
                int.TryParse(dic["LotteryCode"].ToString(), out LotteryCode);
                int.TryParse(dic["PageNumber"].ToString(), out PageNumber);
                int.TryParse(dic["RowsPerPage"].ToString(), out RowsPerPage);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new IsusesBLL().OfLotteryRecord(LotteryCode, PageNumber, RowsPerPage);
                else
                    result = new OfLotteryResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("开奖列表错误:" + ex.Message, true);
                result = new OfLotteryResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 方案记录
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编号</param>
        /// <param name="OrderStatus">状态 0全部 1待开奖 2中奖 3追号</param>
        /// <param name="PageNumber">页码</param>
        /// <param name="RowsPerPage">页行</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("SchemeRecord")]
        public async Task<string> SchemeRecord()
        {
            SchemeRecordResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                short OrderStatus = 0;
                int PageNumber = 0;
                int RowsPerPage = 0;
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                short.TryParse(dic["OrderStatus"].ToString(), out OrderStatus);
                int.TryParse(dic["PageNumber"].ToString(), out PageNumber);
                int.TryParse(dic["RowsPerPage"].ToString(), out RowsPerPage);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new SchemesBLL().SchemeRecord(UserCode, OrderStatus, PageNumber, RowsPerPage);
                else
                    result = new SchemeRecordResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("方案记录错误:" + ex.Message, true);
                result = new SchemeRecordResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 方案详情
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编码</param>
        /// <param name="OrderCode">订单编码</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("SchemeDetail")]
        public async Task<string> SchemeDetail()
        {
            SchemeDetailResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                long OrderCode = 0;
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                long.TryParse(dic["OrderCode"].ToString(), out OrderCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new SchemesBLL().SchemeDetail(UserCode, OrderCode);
                else
                    result = new SchemeDetailResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("方案详情错误:" + ex.Message, true);
                result = new SchemeDetailResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 追号查询期数
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="LotteryCode">彩种编号</param>
        /// <param name="Top">多少条记录</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("ChaseIsuseNum")]
        public async Task<string> ChaseIsuseNum()
        {
            ChaseIsuseNumResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                int LotteryCode = 0;
                int Top = 0;
                int.TryParse(dic["LotteryCode"].ToString(), out LotteryCode);
                int.TryParse(dic["Top"].ToString(), out Top);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new IsusesBLL().ChaseIsuseNum(LotteryCode, Top);
                else
                    result = new ChaseIsuseNumResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("追号查询期数错误:" + ex.Message, true);
                result = new ChaseIsuseNumResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 开奖详情
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="LotteryCode">彩种编号</param>
        /// <param name="IsuseNum">期号</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("OfLotteryDetail")]
        public async Task<string> OfLotteryDetail()
        {
            OfLotteryDetailResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                int LotteryCode = 0;
                string IsuseNum = dic["IsuseNum"].ToString();
                int.TryParse(dic["LotteryCode"].ToString(), out LotteryCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new IsusesBLL().OfLotteryDetail(LotteryCode, IsuseNum);
                else
                    result = new OfLotteryDetailResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("开奖详情错误:" + ex.Message, true);
                result = new OfLotteryDetailResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 追号列表
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        /// <param name="OrderCode"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("ChaseRecord")]
        public async Task<string> ChaseRecord()
        {
            ChaseRecordResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                long OrderCode = 0;
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                long.TryParse(dic["OrderCode"].ToString(), out OrderCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new ChaseTaskDetailsBLL().ChaseRecord(UserCode, OrderCode);
                else
                    result = new ChaseRecordResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("追号列表错误:" + ex.Message, true);
                result = new ChaseRecordResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 追号订单详细
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编号</param>
        /// <param name="ChaseDetailCode">追号详情编号</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("ChaseDetail")]
        public async Task<string> ChaseDetail()
        {
            ChaseDetailResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                long ChaseDetailCode = 0;
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                long.TryParse(dic["ChaseDetailCode"].ToString(), out ChaseDetailCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new ChaseTaskDetailsBLL().ChaseDetail(UserCode, ChaseDetailCode);
                else
                    result = new ChaseDetailResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("追号订单详细错误:" + ex.Message, true);
                result = new ChaseDetailResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 投注
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编号</param>
        /// <param name="LotteryCode">彩种编号</param>
        /// <param name="IsuseNum">期号</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="BuyType">购彩类型 0代购 1追号 2跟单</param>
        /// <param name="Amount">投注总金额</param>
        /// <param name="RoomCode">房间编号</param>
        /// <param name="BetData">投注内容串</param>
        /// <param name="ChaseData">追号内容串</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("Bet")]
        public async Task<string> Bet()
        {
            JsonResult result = null;
            try
            {
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                {
                    var dic = DataSecret.DicPostPara;
                    long UserCode = 0;                             //用户编码
                    int LotteryCode = 0;                           //彩种编码
                    string IsuseNum = dic["IsuseNum"].ToString();             //期号
                    string BeginTime = dic["BeginTime"].ToString();           //开始时间
                    string EndTime = dic["EndTime"].ToString();               //结束时间
                    short BuyType = 0;                             //投注类型 0标准投注 1追号投注 2跟单投注
                    long Amount = 0;                               //投注金额
                    string RoomCode = dic["RoomCode"].ToString();             //聊天室编号
                    string BetData = dic["BetData"].ToString();               //投注内容
                    string ChaseData = dic["ChaseData"].ToString();           //追号内容

                    long.TryParse(dic["UserCode"].ToString(), out UserCode);
                    int.TryParse(dic["LotteryCode"].ToString(), out LotteryCode);
                    short.TryParse(dic["BuyType"].ToString(), out BuyType);
                    long.TryParse(dic["Amount"].ToString(), out Amount);
                    #region 新增字段 支持彩券支付
                    int PaymentType = 0;   //支付类型：1.余额彩豆购彩券支付 2.余额支付 3.购彩券支付 4.彩豆支付 5.余额和购彩券支付 6.余额和彩豆支付 7.彩豆和购彩券支付 
                    long Gold = 0;         //彩豆
                    string Coupons = dic["Coupons"].ToString();     //彩券支付编号 多个彩券一起使用请用，逗号隔开(英文字符)  [{\"pid\":11,\"ba\":1000}]

                    int.TryParse(dic["PaymentType"].ToString(), out PaymentType);
                    long.TryParse(dic["Gold"].ToString(), out Gold);
                    #endregion
                    result = new SchemesBLL().Bet(UserCode, LotteryCode, IsuseNum, BeginTime, EndTime, BuyType, Amount, RoomCode, BetData, ChaseData, Coupons, PaymentType, Gold);
                }
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("投注错误[Bet]：" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 跟单投注
        /// </summary>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("FollowBet")]
        public async Task<string> FollowBet()
        {
            JsonResult result = null;
            try
            {
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                {
                    var dic = DataSecret.DicPostPara;
                    string Token = dic["Token"].ToString();                   //令牌
                    long UserCode = 0;                             //用户编码
                    long OrderCode = 0;                            //用户编码
                    string IsuseNum = dic["IsuseNum"].ToString();             //期号
                    string BeginTime = dic["BeginTime"].ToString();           //开始时间
                    string EndTime = dic["EndTime"].ToString();               //结束时间
                    long.TryParse(dic["UserCode"].ToString(), out UserCode);
                    long.TryParse(dic["OrderCode"].ToString(), out OrderCode);
                    result = new SchemesBLL().FollowBet(Token, UserCode, OrderCode, IsuseNum, BeginTime, EndTime);
                }
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("投注错误[Bet]：" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 查询彩种具体加奖玩法
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("AwardPlayCode")]
        public async Task<string> AwardPlayCode()
        {
            AwarPlayCodeResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                int LotteryCode = 0;
                string Token = dic["Token"].ToString();
                int.TryParse(dic["LotteryCode"].ToString(), out LotteryCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new ActivityAwardBLL().QueryAwardPlayCode(Token, LotteryCode);
                else
                    result = new AwarPlayCodeResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("查询当前可支付的彩券错误:" + ex.Message, true);
                result = new AwarPlayCodeResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 查询订单电子票详情
        /// </summary>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("LookTicketInfo")]
        public async Task<string> LookTicketInfo()
        {
            SchemeTicketsResult result = null;
            try
            {
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                {
                    var dic = DataSecret.DicPostPara;
                    string Token = dic["Token"].ToString();                   //令牌
                    long UserCode = 0;                             //用户编码
                    long OrderCode = 0;                            //订单编号
                    long ChaseCode = 0;                            //追号编号
                    int PageIndex = 0;                             //当前页
                    int PageSize = 0;                              //页码
                    long.TryParse(dic["UserCode"].ToString(), out UserCode);
                    long.TryParse(dic["OrderCode"].ToString(), out OrderCode);
                    long.TryParse(dic["ChaseCode"].ToString(), out ChaseCode);
                    int.TryParse(dic["PageIndex"].ToString(), out PageIndex);
                    int.TryParse(dic["PageSize"].ToString(), out PageSize);
                    result = new SchemeETicketsBLL().LookETickets(Token, UserCode, OrderCode, ChaseCode, PageIndex, PageSize);
                }
                else
                    result = new SchemeTicketsResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("查询订单电子票详情错误[LookTicketInfo]：" + ex.Message, true);
                result = new SchemeTicketsResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        #endregion

        #region  ---------- 彩券 ---------- 

        /// <summary>
        /// 查询当前可支付的彩券
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        /// <param name="OrderMoney"></param>
        /// <param name="LotteryCode"></param>
        /// <param name="BuyType"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("CouponsPaymentList")]
        public async Task<string> CouponsPaymentList()
        {
            CouponsListResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                long OrderMoney = 0;
                int LotteryCode = 0;
                int BuyType = 0;
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                long.TryParse(dic["OrderMoney"].ToString(), out OrderMoney);
                int.TryParse(dic["LotteryCode"].ToString(), out LotteryCode);
                int.TryParse(dic["BuyType"].ToString(), out BuyType);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new CouponsBLL().QueryCouponsPaymentList(UserCode, OrderMoney, LotteryCode, BuyType);
                else
                    result = new CouponsListResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("查询当前可支付的彩券错误:" + ex.Message, true);
                result = new CouponsListResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }
        /// <summary>
        /// 查询用户彩券列表
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("CouponsList")]
        public async Task<string> CouponsList()
        {
            CouponsListResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                bool IsCoupons = false;
                int PageIndex = 0;
                int PageSize = 0;
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                int.TryParse(dic["PageIndex"].ToString(), out PageIndex);
                int.TryParse(dic["PageSize"].ToString(), out PageSize);
                bool.TryParse(dic["IsCoupons"].ToString(), out IsCoupons);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new CouponsBLL().QueryCouponsList(UserCode, IsCoupons, PageIndex, PageSize);
                else
                    result = new CouponsListResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("查询用户彩券列表错误:" + ex.Message, true);
                result = new CouponsListResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 兑换彩券
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        /// <param name="CDKey"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("ExchangerCoupons")]
        public async Task<string> ExchangerCoupons()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                string CDKey = dic["CDKey"].ToString();
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new CouponsCDKeyBLL().VerifyCDKey(UserCode, CDKey);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("兑换彩券错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }
        #endregion

        #region   ---------- 走势图分析  ---------- 
        /// <summary>
        /// 标准遗漏
        /// URL:Business/NormOmission
        /// </summary>
        /// <param name="LotteryCode">彩种编号</param>
        /// <param name="IsuseName">期次号</param>
        /// <param name="PlayCode">玩法编号</param>
        /// <param name="TopCount">计算遗漏条数</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("NormOmission")]
        public async Task<string> NormOmission()
        {

            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                int LotteryCode = 0;
                int PlayCode = 0;
                int TopCount = 0;
                string IsuseName = dic["IsuseName"].ToString();
                int.TryParse(dic["LotteryCode"].ToString(), out LotteryCode);
                int.TryParse(dic["PlayCode"].ToString(), out PlayCode);
                int.TryParse(dic["TopCount"].ToString(), out TopCount);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    return new LotBase()[LotteryCode, LotBaseType.BaseOmission].Omission(IsuseName, PlayCode, TopCount);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("标准遗漏错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 走势图
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <param name="PlayCode"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("TrendChart")]
        public async Task<string> TrendChart()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                int LotteryCode = 0;
                int PlayCode = 0;
                string IsuseName = dic["IsuseName"].ToString();
                int.TryParse(dic["LotteryCode"].ToString(), out LotteryCode);
                int.TryParse(dic["PlayCode"].ToString(), out PlayCode);

                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    return new LotBase()[LotteryCode, LotBaseType.BaseOmission].Basics_TrendChart(IsuseName, PlayCode, 100);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("走势图错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }
        #endregion

        #region ---------- 新闻 ---------- 
        /// <summary>
        /// 彩种分析
        /// </summary>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("LotAnalysis")]
        public async Task<string> LotAnalysis()
        {
            NewsTitleResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                int LotteryCode = 0;
                int.TryParse(dic["LotteryCode"].ToString(), out LotteryCode);
                result = new NewsBLL().LotAnalysis(LotteryCode);
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("彩种分析错误[LotAnalysis]：" + ex.Message, true);
                result = new NewsTitleResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }
        /// <summary>
        /// 查看分析资讯
        /// </summary>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("LookNewsAnalysis")]
        public async Task<string> LookNewsAnalysis()
        {
            ClientNewsResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                int NewsID = 0;
                int.TryParse(dic["NewsID"].ToString(), out NewsID);
                result = new NewsBLL().LookNewsAnalysis(NewsID);
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("查看分析资讯错误[LookNewsAnalysis]：" + ex.Message, true);
                result = new ClientNewsResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }
        /// <summary>
        /// 支持反对资讯
        /// </summary>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("EvaluateNews")]
        public async Task<string> EvaluateNews()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                int NewsID = 0;
                int IsSupport = 0;
                int.TryParse(dic["NewsID"].ToString(), out NewsID);
                int.TryParse(dic["IsSupport"].ToString(), out IsSupport);
                result = new NewsBLL().EvaluateNews(NewsID, IsSupport);
            }
            catch (Exception ex)
            {
                new Log("BusinessController").Write("评价文章资讯错误[EvaluateNews]：" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        #endregion
        
        
    }
}
