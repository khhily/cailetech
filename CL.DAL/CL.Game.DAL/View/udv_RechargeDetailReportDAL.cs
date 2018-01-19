using CL.Dapper.Repository;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Enum.Common;
using System.Data;
using CL.Tools.Common;

namespace CL.Game.DAL.View
{
    public class udv_RechargeDetailReportDAL : DataRepositoryBase<udv_RechargeDetailReport>
    {
        public udv_RechargeDetailReportDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 充值明细列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="orderNo">系统订单号</param>
        /// <param name="rechargeNo">借口商订单号</param>
        /// <param name="outRechargeNo">第三方订单号</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="endTime">查询结束时间</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="RecordCount">查询条数</param>
        /// <returns>List<udv_OrderDetailReport></returns>
        public List<udv_RechargeDetailReport> QueryModeListByPages(string userName, string mobile, string orderNo, string rechargeNo, string outRechargeNo, string startTime, string endTime, string orderBy, int pageIndex, int pageSize, ref int RecordCount)
        {
            StringBuilder whereSql = new StringBuilder();
            whereSql.Append(" 1 = 1 ");
            if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(userName.Trim()))
                whereSql.AppendFormat(" AND UserName like '%{0}%' ", userName);
            if (!string.IsNullOrEmpty(mobile) || !string.IsNullOrEmpty(mobile.Trim()))
                whereSql.AppendFormat(" AND UserMobile = '{0}' ", mobile);
            if (!string.IsNullOrEmpty(orderNo) || !string.IsNullOrEmpty(orderNo.Trim()))
                whereSql.AppendFormat(" AND OrderNo = '{0}' ", orderNo);
            if (!string.IsNullOrEmpty(rechargeNo) || !string.IsNullOrEmpty(rechargeNo.Trim()))
                whereSql.AppendFormat(" AND RechargeNo = '{0}' ", rechargeNo);
            if (!string.IsNullOrEmpty(outRechargeNo) || !string.IsNullOrEmpty(outRechargeNo.Trim()))
                whereSql.AppendFormat(" AND OutRechargeNo = '{0}' ", outRechargeNo);
            if (!string.IsNullOrEmpty(startTime) || !string.IsNullOrEmpty(startTime.Trim()))
                whereSql.AppendFormat(" AND CreateTime >= '{0}' ", startTime);
            if (!string.IsNullOrEmpty(endTime) || !string.IsNullOrEmpty(endTime.Trim()))
                whereSql.AppendFormat(" AND CreateTime <= '{0}' ", endTime);

            RecordCount = base.GetIntSingle(PagingHelper.CreateCountingSql(new udv_RechargeDetailReport().GetType().Name, whereSql.ToString()));
            return base.GetListPaged(pageIndex, pageSize, whereSql.ToString(), orderBy).ToList();

        }
    }
}
