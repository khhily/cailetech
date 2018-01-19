using CL.Dapper.Repository;
using CL.View.Entity.Game;
using CL.Enum.Common;
using System.Data;
using System.Collections.Generic;
using System.Text;
using CL.Tools.Common;
using System.Linq;

namespace CL.Game.DAL.View
{
    public class udv_UserAccountReportDAL : DataRepositoryBase<udv_UserAccountReport>
    {
        public udv_UserAccountReportDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 用户账户明细列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">查询条数</param>
        /// <returns>List<udv_OrderDetailReport></returns>
        public List<udv_UserAccountReport> QueryModeListByPages(string userName, string mobile, string startTime, string endTime, string orderBy, int pageIndex, int pageSize, ref int recordCount)
        {
            StringBuilder whereSql = new StringBuilder();
            whereSql.Append(" 1 = 1 ");
            if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(userName.Trim()))
                whereSql.AppendFormat(" AND UserName like '%{0}%' ", userName);
            if (!string.IsNullOrEmpty(mobile) || !string.IsNullOrEmpty(mobile.Trim()))
                whereSql.AppendFormat(" AND UserMobile = '{0}' ", mobile);
            if (!string.IsNullOrEmpty(startTime) || !string.IsNullOrEmpty(startTime.Trim()))
                whereSql.AppendFormat(" AND CreateTime >= '{0}' ", startTime);
            if (!string.IsNullOrEmpty(endTime) || !string.IsNullOrEmpty(endTime.Trim()))
                whereSql.AppendFormat(" AND CreateTime <= '{0}' ", endTime);

            recordCount = base.GetIntSingle(PagingHelper.CreateCountingSql(new udv_UserAccountReport().GetType().Name, whereSql.ToString()));
            return base.GetListPaged(pageIndex, pageSize, whereSql.ToString(), orderBy).ToList();

        }
    }
}
