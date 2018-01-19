using CL.Dapper.Repository;
using CL.Game.Entity;
using CL.Enum.Common;
using System.Data;
using CL.View.Entity.Game;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using System;

namespace CL.Game.DAL
{
    public class UsersPayDetailDAL : DataRepositoryBase<UsersPayDetailEntity>
    {
        public UsersPayDetailDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long InsertEntity(UsersPayDetailEntity entity)
        {
            return base.Insert_Long(entity) ?? 0;
        }
        /// <summary>
        /// 退款单号是否存在
        /// </summary>
        /// <param name="IsOutRefundNo"></param>
        /// <returns></returns>
        public bool IsOutRefundNo(string RefundNo)
        {
            return base.RecordCount(new { RefundNo = RefundNo }) > 0 ? false : true;
        }
        /// <summary>
        /// 订单号是否存在
        /// 存在返回false
        /// </summary>
        /// <param name="tbout_trade_no"></param>
        /// <returns></returns>
        public bool IsTboutTradeNo(string tbout_trade_no)
        {
            return base.RecordCount(new { OrderNo = tbout_trade_no }) > 0 ? false : true;
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<udv_UserPay> QueryListByPage(string keywords, int iType, int pageSize, int pageIndex, ref int recordCount)
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" IsDel = @IsDel ");
            var Parms = new DynamicParameters();
            Parms.Add("@IsDel", 0, DbType.Boolean, null, 1);

            if (keywords.Trim() != "")
            {
                Where.Append(" and (UserName like @keywords or UserMobile like @keywords or OrderNo like @keywords ) ");
                Parms.Add("@keywords", string.Format("%{0}%", keywords), DbType.String, null, 64);
            }
            if (iType != -1)
            {
                Where.Append(" and Result = @Result ");
                Parms.Add("@Result", iType, DbType.Int16, null, 1);
            }

            recordCount = base.GetIntSingle(string.Format("select count(1) from udv_UserPay where {0}", Where.ToString()), Parms);
            return new DataRepositoryBase<udv_UserPay>(DbConnectionEnum.CaileGame).GetListPaged(pageIndex, pageSize, Where.ToString(), "PayID desc", Parms).ToList();
        }
        /// <summary>
        /// 充值补单
        /// </summary>
        /// <param name="PayID"></param>
        /// <param name="RechargeNo">接口交易号</param>
        /// <returns></returns>
        public int Replenishment(long PayID, string RechargeNo, string OutRechargeNo)
        {
            var para = new DynamicParameters();
            para.Add("@PayID", PayID, DbType.Int64);
            para.Add("@RechargeNo", RechargeNo, DbType.String);
            para.Add("@OutRechargeNo", OutRechargeNo, DbType.String);
            para.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            base.Execute("udp_Replenishment", para);
            return para.Get<int>("@ReturnValue");
        }

        /// <summary>
        /// 报表:充值查询统计
        /// </summary>
        /// <param name="StartTime">起止时间</param>
        /// <param name="EndTime">起止时间</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="OrderNo">平台订单编号</param>
        /// <param name="RechargeNo">第三方订单编号</param>
        /// <param name="PayType">充值方式</param>
        /// <param name="Result">状态</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="RecordPayAmount">充值总额</param>
        /// <param name="RecordCount">查询总记录</param>
        /// <returns></returns>
        public List<udv_ReportPayDetail> QuertPayDetailReport(DateTime StartTime, DateTime EndTime, long UserID, string OrderNo, string RechargeNo, string PayType, int Result, int PageIndex, int PageSize, ref long RecordPayAmount, ref int RecordCount)
        {
            var para = new DynamicParameters();
            para.Add("@StartTime", StartTime);
            para.Add("@EndTime", EndTime);
            para.Add("@UserID", UserID);
            para.Add("@OrderNo", OrderNo);
            para.Add("@RechargeNo", RechargeNo);
            para.Add("@PayType", PayType);
            para.Add("@Result", Result);
            para.Add("@PageIndex", PageIndex);
            para.Add("@PageSize", PageSize);
            para.Add("@RecordPayAmount", RecordPayAmount, DbType.Int64, ParameterDirection.Output);
            para.Add("@RecordCount", RecordCount, DbType.Int32, ParameterDirection.Output);
            var Entitys = new DataRepositoryBase<udv_ReportPayDetail>(DbConnectionEnum.CaileGame).QueryList("udp_QuertPayDetailReport", para, CommandType.StoredProcedure).ToList();
            RecordPayAmount = para.Get<long>("RecordPayAmount");
            RecordCount = para.Get<int>("RecordCount");
            return Entitys;
        }




    }
}
