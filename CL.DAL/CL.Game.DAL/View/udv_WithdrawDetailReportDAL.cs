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
    public class udv_WithdrawDetailReportDAL : DataRepositoryBase<udv_WithdrawDetailReport>
    {
        public udv_WithdrawDetailReportDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 提现明细列表
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Mobile">手机号码</param>
        /// <param name="ReservedPhone">银行预留手机号码</param>
        /// <param name="PayOutStatus">提现状态
        /// 0.申请，2.处理中，4.处理完成，6.提现失败</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="RecordCount">查询条数</param>
        /// <returns>List<udv_OrderDetailReport></returns>
        public List<udv_WithdrawDetailReport> QueryModeListByPages(int payOutStatus, string userName, string mobile, string reservedPhone, string startTime, string endTime, string orderBy, int pageIndex, int pageSize, ref int recordCount)
        {
            StringBuilder whereSql = new StringBuilder();
            whereSql.Append(" 1 = 1 ");
            if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(userName.Trim()))
                whereSql.AppendFormat(" AND UserName like '%{0}%' ", userName);
            if (!string.IsNullOrEmpty(mobile) || !string.IsNullOrEmpty(mobile.Trim()))
                whereSql.AppendFormat(" AND UserMobile = '{0}' ", mobile);
            if (!string.IsNullOrEmpty(reservedPhone) || !string.IsNullOrEmpty(reservedPhone.Trim()))
                whereSql.AppendFormat(" AND ReservedPhone = '{0}' ", reservedPhone);
            if (payOutStatus >= 0)
                whereSql.AppendFormat(" AND PayOutStatus = {0} ", payOutStatus);
            if (!string.IsNullOrEmpty(startTime) || !string.IsNullOrEmpty(startTime.Trim()))
                whereSql.AppendFormat(" AND CreateTime >= '{0}' ", startTime);
            if (!string.IsNullOrEmpty(endTime) || !string.IsNullOrEmpty(endTime.Trim()))
                whereSql.AppendFormat(" AND CreateTime <= '{0}' ", endTime);

            recordCount = base.GetIntSingle(PagingHelper.CreateCountingSql(new udv_WithdrawDetailReport().GetType().Name, whereSql.ToString()));
            return base.GetListPaged(pageIndex, pageSize, whereSql.ToString(), orderBy).ToList();

        }
    }
}
