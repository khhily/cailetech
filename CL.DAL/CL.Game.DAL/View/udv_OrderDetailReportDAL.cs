using CL.Dapper.Repository;
using CL.View.Entity.Game;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;
using CL.Tools.Common;

namespace CL.Game.DAL.View
{
    public class udv_OrderDetailReportDAL : DataRepositoryBase<udv_OrderDetailReport>
    {
        public udv_OrderDetailReportDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="UserName">用户昵称</param>
        /// <param name="Mobile">手机号码</param>
        /// <param name="SchemeNumber">方案编号</param>
        /// <param name="BuyType">购彩类型</param>
        /// <param name="PageIndex">当期页</param>
        /// <param name="PageSize">每页大小</param>
        /// <param name="RecordCount">共多少条记录</param>
        /// <returns></returns>
        public List<udv_OrderDetailReport> QueryModeListByPages(int lotteryCode, int schemeStatus, string schemeNumber, string userName, string mobile, string startTime, string endTime, string orderBy, int pageIndex, int pageSize, ref int recordCount)
        {
            StringBuilder whereSql = new StringBuilder();
            whereSql.Append(" 1 = 1 ");
            if (lotteryCode > 0)
                whereSql.AppendFormat(" AND LotteryCode = '{0}' ", lotteryCode);
            if (schemeStatus != -1)
                whereSql.AppendFormat(" AND schemeStatus = '{0}' ", schemeStatus);
            if (!string.IsNullOrEmpty(schemeNumber) || !string.IsNullOrEmpty(schemeNumber.Trim()))
                whereSql.AppendFormat(" AND SchemeNumber like '%{0}%' ", schemeNumber);
            if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(userName.Trim()))
                whereSql.AppendFormat(" AND UserName like '%{0}%' ", userName);
            if (!string.IsNullOrEmpty(mobile) || !string.IsNullOrEmpty(mobile.Trim()))
                whereSql.AppendFormat(" AND UserMobile = '{0}' ", mobile);
            if (!string.IsNullOrEmpty(startTime) || !string.IsNullOrEmpty(startTime.Trim()))
                whereSql.AppendFormat(" AND CreateTime >= '{0}' ", startTime);
            if (!string.IsNullOrEmpty(endTime) || !string.IsNullOrEmpty(endTime.Trim()))
                whereSql.AppendFormat(" AND CreateTime <= '{0}' ", endTime);

            recordCount = base.GetIntSingle(PagingHelper.CreateCountingSql(new udv_OrderDetailReport().GetType().Name, whereSql.ToString()));
            return base.GetListPaged(pageIndex, pageSize, whereSql.ToString(), orderBy).ToList();

        }
    }
}
