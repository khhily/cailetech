using CL.Enum.Common;
using CL.Game.DAL;
using CL.Json.Entity;
using CL.Redis.BLL;
using CL.Coupons.DAL;
using CL.Coupons.Entity;
using CL.Tools.Common;
using CL.View.Entity.Coupons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CL.Coupons.BLL
{
    public class CouponsCDKeyBLL
    {
        Log log = new Log("CouponsCDKeyBLL");
        CouponsCDKeyDAL dal = new CouponsCDKeyDAL(Enum.Common.DbConnectionEnum.CaileCoupons);

        /// <summary>
        /// 根据CDKey查询兑换码实体
        /// </summary>
        /// <param name="CDKey"></param>
        /// <returns></returns>
        public CouponsCDKeyEntity QueryEntity(string CDKey)
        {
            return dal.QueryEntity(CDKey);
        }
        /// <summary>
        /// 查询可用彩券
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OrderMoney"></param>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public int ExchangeCoupons(long UserID, long CouponsID)
        {
            return dal.ExchangeCoupons(UserID, CouponsID);
        }
        /// <summary>
        /// 查询彩券兑换码列表
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RecordCount"></param>
        /// <returns></returns>
        public List<udv_CouponsCDKeyList> QueryCouponsList(string UserName, DateTime StartTime, DateTime EndTime, int PageIndex, int PageSize, ref int RecordCount)
        {
            return dal.QueryCouponsList(UserName, StartTime, EndTime, PageIndex, PageSize, ref RecordCount);
        }
        /// <summary>
        /// 查询兑换码报表
        /// </summary>
        /// <param name="PartnerCode"></param>
        /// <param name="TimeType"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<udv_ReportCDKeyList> QueryReportCDKey(string PartnerCode, int TimeType, DateTime StartTime, DateTime EndTime)
        {
            return dal.QueryReportCDKey(PartnerCode, TimeType, StartTime, EndTime);
        }

        #region 自定义方法
        /// <summary>
        /// 兑换码兑换
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        /// <param name="CDKey"></param>
        /// <returns></returns>
        public JsonResult VerifyCDKey(long UserCode, string CDKey)
        {
            JsonResult result = null;
            try
            {
                var Entity = this.QueryEntity(CDKey.Trim());
                if (Entity == null)
                    result = new JsonResult()
                    {
                        Code = (int)ResultCode.CDKeyFailure,
                        Msg = Common.GetDescription(ResultCode.CDKeyFailure)
                    };
                else
                {
                    string EncCDKey = EncryptAlgorithm.CustomEncryptKey(Entity.CDKey);
                    if (CDKey.Trim() != EncCDKey.Trim())
                        result = new JsonResult()
                        {
                            Code = (int)ResultCode.CDKeyFailure,
                            Msg = Common.GetDescription(ResultCode.CDKeyFailure)
                        };
                    else
                    {
                        int Rec = this.ExchangeCoupons(UserCode, Entity.CouponsID);
                        if (Rec >= 0)
                        {
                            result = new JsonResult()
                            {
                                Code = (int)ResultCode.Success,
                                Msg = Common.GetDescription(ResultCode.Success)
                            };
                            Task.Factory.StartNew(() =>
                            {
                                var CouponsEntity = new CouponsBLL().QueryEntity(Entity.CouponsID);
                                new UsersDAL(DbConnectionEnum.CaileGame).SetSystemStaticdata(DateTime.Now.ToString("yyyy-MM-dd"), 6, CouponsEntity.FaceValue, 0, 0);
                            });
                        }
                        else if (Rec == -2)
                            result = new JsonResult()
                            {
                                Code = (int)ResultCode.ExchangerAbate,
                                Msg = Common.GetDescription(ResultCode.ExchangerAbate)
                            };
                        else if (Rec == -3)
                            result = new JsonResult()
                            {
                                Code = (int)ResultCode.ExchangerExpire,
                                Msg = Common.GetDescription(ResultCode.ExchangerExpire)
                            };
                        else
                            result = new JsonResult()
                            {
                                Code = (int)ResultCode.ExchangerAbate,
                                Msg = Common.GetDescription(ResultCode.ExchangerAbate)
                            };
                    }
                }
            }
            catch (Exception ex)
            {
                log.Write(string.Format("兑换码兑换错误：{0}", ex.Message), true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return result;
        }
        #endregion
    }
}
