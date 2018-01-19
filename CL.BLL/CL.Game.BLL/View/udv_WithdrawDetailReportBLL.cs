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
    public class udv_WithdrawDetailReportBLL
    {
        udv_WithdrawDetailReportDAL dal = new udv_WithdrawDetailReportDAL(DbConnectionEnum.CaileGame);

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
        public List<udv_WithdrawDetailReport> QueryModeListByPages(int payOutStatus, string userName, string mobile, string reservedPhone, string startTime, string endTime, string orderBy, int pageIndex, int pageSize, ref int totalCount)
        {
            return dal.QueryModeListByPages(payOutStatus, userName, mobile, reservedPhone, startTime, endTime, orderBy, pageIndex, pageSize, ref totalCount);
        }
    }
}
