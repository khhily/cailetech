using CL.Dapper.Repository;
using CL.Coupons.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Enum.Common;
using System.Data;
using Dapper;
using CL.View.Entity.Coupons;

namespace CL.Coupons.DAL
{
    public class CouponsDAL : DataRepositoryBase<CouponsEntity>
    {
        public CouponsDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {

        }

        /// <summary>
        /// 查询即将过期彩券推送给用户
        /// </summary>
        /// <returns></returns>
        public List<udv_CouponsExpireTimeList> QueryCouponsExpireTimeList()
        {
            StringBuilder SQLStr = new StringBuilder();
            SQLStr.Append(" SELECT r.CouponsID,r.ExpireTime,u.UserName,up.PushIdentify,r.FaceValue FROM CaileGame.dbo.CT_Users AS u ");
            SQLStr.Append(" INNER JOIN CaileGame.dbo.CT_UsersPush AS up ON u.UserID=up.UserId ");
            SQLStr.Append(" INNER JOIN CT_Coupons AS r ON r.UserID=u.UserID ");
            SQLStr.Append(" WHERE CONVERT(DATE,r.ExpireTime) = CONVERT(DATE,GETDATE()) AND r.FaceValue = r.Balance ; ");
            return new DataRepositoryBase<udv_CouponsExpireTimeList>(DbConnectionEnum.CaileCoupons).QueryList(SQLStr.ToString()).ToList();
        }
        /// <summary>
        /// 查询可用彩券
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OrderMoney"></param>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<CouponsEntity> QueryCouponsPayment(long UserID, long OrderMoney, int LotteryCode)
        {
            var para = new DynamicParameters();
            para.Add("@UserID", UserID);
            para.Add("@OrderMoney", OrderMoney);
            para.Add("@LotteryCode", LotteryCode);
            return base.QueryList("udp_QueryCouponsPayment", para, CommandType.StoredProcedure).ToList();
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
            var para = new DynamicParameters();
            para.Add("@UserID", UserID);
            para.Add("@IsCoupons", IsCoupons);
            para.Add("@PageIndex", PageIndex);
            para.Add("@PageSize", PageSize);
            para.Add("@Counts", PageSize, DbType.Int32, ParameterDirection.Output);
            List<CouponsEntity> Entitys = null;
            var RecEntity = base.QueryList("udp_QueryCoupons", para, CommandType.StoredProcedure).ToList();
            if (RecEntity != null && RecEntity.Count > 0)
                Entitys = RecEntity;
            if (IsCoupons)
                Counts = para.Get<int>("@Counts");
            return Entitys;
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
        public List<udv_CouponsList> QueryCouponsList(string UserName, int LotteryCode, int CouponsStatus, int CouponsType, int CouponsSource, DateTime StartTime, DateTime EndTime, int PageIndex, int PageSize, ref int RecordCount,ref long RecordFaceValue,ref long RecordEmploy)
        {
            var para = new DynamicParameters();
            para.Add("@UserName", UserName);
            para.Add("@LotteryCode", LotteryCode);
            para.Add("@CouponsStatus", CouponsStatus);
            para.Add("@CouponsType", CouponsType);
            para.Add("@CouponsSource", CouponsSource);
            para.Add("@StartTime", StartTime);
            para.Add("@EndTime", EndTime);
            para.Add("@PageIndex", PageIndex);
            para.Add("@PageSize", PageSize);
            para.Add("@RecordCount", RecordCount, DbType.Int32, ParameterDirection.Output);
            para.Add("@RecordFaceValue", RecordFaceValue, DbType.Int64, ParameterDirection.Output);
            para.Add("@RecordEmploy", RecordEmploy, DbType.Int64, ParameterDirection.Output);
            List<udv_CouponsList> Entitys = new DataRepositoryBase<udv_CouponsList>(DbConnectionEnum.CaileCoupons).QueryList("udp_QueryCouponsList", para, CommandType.StoredProcedure).ToList();
            RecordCount = para.Get<int>("@RecordCount");
            RecordFaceValue = para.Get<long>("@RecordFaceValue");
            RecordEmploy = para.Get<long>("@RecordEmploy");
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
        public List<udv_ReportCouponsList> QueryReportCoupons(int TimeType, DateTime StartTime, DateTime EndTime)
        {
            var para = new DynamicParameters();
            para.Add("@TimeType", TimeType);
            para.Add("@StartTime", StartTime);
            para.Add("@EndTime", EndTime);
            return new DataRepositoryBase<udv_ReportCouponsList>(DbConnectionEnum.CaileCoupons).QueryList("udp_ReportCoupons", para, CommandType.StoredProcedure).ToList();

        }

        /// <summary>
        /// 生成彩券兑换码
        /// </summary>
        /// <param name="CDKeyEntitys"></param>
        /// <param name="CouponsEntitys"></param>
        /// <returns></returns>
        public bool GenerateCoupons(List<CouponsCDKeyEntity> CDKeyEntitys, List<CouponsEntity> CouponsEntitys)
        {
            using (IDbTransaction tran = base.db.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < CouponsEntitys.Count; i++)
                    {
                        var Entity = CouponsEntitys[i];
                        var KetEntity = CDKeyEntitys[i];
                        long CouponsID = this.Insert_Long(Entity, tran) ?? 0;
                        if (CouponsID > 0)
                        {
                            KetEntity.CouponsID = CouponsID;
                            new CouponsCDKeyDAL(DbConnectionEnum.CaileCoupons).Insert_Long(KetEntity);
                        }
                    }
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
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
        public bool RegisterGiveCoupons(long UserID, int ActivityID, long Amount, int LotteryCode, int Day)
        {
            var para = new DynamicParameters();
            para.Add("@UserID", UserID);
            para.Add("@ActivityID", ActivityID);
            para.Add("@Amount", Amount);
            para.Add("@LotteryCode", LotteryCode);
            para.Add("@Day", Day);
            int rec = base.Execute("udp_RegisterGiveCoupons", para);
            return rec > 0;
        }
    }
}
