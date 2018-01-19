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
    public class udv_TradeDetailReportBLL
    {
        udv_TradeDetailReportDAL dal = new udv_TradeDetailReportDAL(DbConnectionEnum.CaileGame);

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
        /// <param name="totalCount">查询条数</param>
        /// <returns>List<udv_OrderDetailReport></returns>
        public List<udv_TradeDetailReport> QueryModeListByPages(int tradeType, string userName, string mobile, string startTime, string endTime, string orderBy, int pageIndex, int pageSize, ref int totalCount)
        {
            return dal.QueryModeListByPages(tradeType, userName, mobile, startTime, endTime, orderBy, pageIndex, pageSize, ref totalCount);
        }
    }
}
