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
    public class udv_TradeDetailReportDAL : DataRepositoryBase<udv_TradeDetailReport>
    {
        public udv_TradeDetailReportDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 交易明细列表
        /// 中奖明细列表
        /// </summary>
        /// <param name="tradeType">交易类型 0.充值，1.购彩消费，2.提现冻结，3.提现失败解冻，4.金豆兑换，5.中奖 11.用户撤单 12.系统撤单,13.追号撤单，14.投注失败退款 15.出票失败退款</param>
        /// <param name="userName">用户名</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="endTime">查询结束时间</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">查询条数</param>
        /// <returns>List<udv_OrderDetailReport></returns>
        public List<udv_TradeDetailReport> QueryModeListByPages(int tradeType, string userName, string mobile, string startTime, string endTime, string orderBy, int pageIndex, int pageSize, ref int recordCount)
        {
            StringBuilder whereSql = new StringBuilder();
            whereSql.Append(" 1 = 1 ");
            if (tradeType >= 0)
                whereSql.AppendFormat(" AND TradeType = {0} ", tradeType);
            if (tradeType == 5)
                whereSql.Append(" AND TradeAmount > 0 ");
            if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(userName.Trim()))
                whereSql.AppendFormat(" AND UserName like '%{0}%' ", userName);
            if (!string.IsNullOrEmpty(mobile) || !string.IsNullOrEmpty(mobile.Trim()))
                whereSql.AppendFormat(" AND UserMobile = '{0}' ", mobile);
            if (!string.IsNullOrEmpty(startTime) || !string.IsNullOrEmpty(startTime.Trim()))
                whereSql.AppendFormat(" AND CreateTime >= '{0}' ", startTime);
            if (!string.IsNullOrEmpty(endTime) || !string.IsNullOrEmpty(endTime.Trim()))
                whereSql.AppendFormat(" AND CreateTime <= '{0}' ", endTime);

            recordCount = base.GetIntSingle(PagingHelper.CreateCountingSql(new udv_TradeDetailReport().GetType().Name, whereSql.ToString()));
            return base.GetListPaged(pageIndex, pageSize, whereSql.ToString(), orderBy).ToList();

        }


    }
}
