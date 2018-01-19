using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.BLL.View
{
    public class udv_RechargeDetailReportBLL
    {
        udv_RechargeDetailReportDAL dal = new udv_RechargeDetailReportDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 充值明细列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="orderNo">系统订单号</param>
        /// <param name="rechargeNo">借口商订单号</param>
        /// <param name="outRechargeNo">第三方订单号</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="endTime">查询结束时间</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">查询条数</param>
        /// <returns>List<udv_OrderDetailReport></returns>
        public List<udv_RechargeDetailReport> QueryModeListByPages(string userName, string mobile, string orderNo, string rechargeNo, string outRechargeNo, string startTime, string endTime, string orderBy, int pageIndex, int pageSize, ref int totalCount)
        {
            return dal.QueryModeListByPages(userName, mobile, orderNo, rechargeNo, outRechargeNo, startTime, endTime, orderBy, pageIndex, pageSize, ref totalCount);
        }
    }
}
