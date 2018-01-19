using CL.Dapper.Repository;
using CL.Coupons.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;
using Dapper;
using CL.View.Entity.Coupons;

namespace CL.Coupons.DAL
{
    public class CouponsCDKeyDAL : DataRepositoryBase<CouponsCDKeyEntity>
    {
        public CouponsCDKeyDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 根据CDKey查询兑换码实体
        /// </summary>
        /// <param name="CDKey"></param>
        /// <returns></returns>
        public CouponsCDKeyEntity QueryEntity(string CDKey)
        {
            return base.Get(new { EncryptKey = CDKey }, " CDKeyID desc ");
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
            var para = new DynamicParameters();
            para.Add("@UserID", UserID);
            para.Add("@CouponsID", CouponsID);
            return base.Execute("udp_ExchangeCoupons", para);
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
            var para = new DynamicParameters();
            para.Add("@UserName", UserName);
            para.Add("@StartTime", StartTime);
            para.Add("@EndTime", EndTime);
            para.Add("@PageIndex", PageIndex);
            para.Add("@PageSize", PageSize);
            para.Add("@RecordCount", RecordCount, DbType.Int32, ParameterDirection.Output);
            List<udv_CouponsCDKeyList> Entitys = new DataRepositoryBase<udv_CouponsCDKeyList>(DbConnectionEnum.CaileCoupons).QueryList("udp_CouponsCDKeyList", para, CommandType.StoredProcedure).ToList();
            RecordCount = para.Get<int>("@RecordCount");
            return Entitys;

        }

        /// <summary>
        /// 查询兑换码报表
        /// </summary>
        /// <param name="PartnerCode"></param>
        /// <param name="TimeType"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<udv_ReportCDKeyList> QueryReportCDKey(string PartnerCode,int TimeType,DateTime StartTime,DateTime EndTime)
        {
            var para = new DynamicParameters();
            para.Add("@PartnerCode", PartnerCode);
            para.Add("@TimeType", TimeType);
            para.Add("@StartTime", StartTime);
            para.Add("@EndTime", EndTime);
            return new DataRepositoryBase<udv_ReportCDKeyList>(DbConnectionEnum.CaileCoupons).QueryList("udp_ReportCDKey", para, CommandType.StoredProcedure).ToList();
            
        }
    }
}
