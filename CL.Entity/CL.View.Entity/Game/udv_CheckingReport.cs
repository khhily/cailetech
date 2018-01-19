using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{

    /// <summary>
    /// 对账报表
    /// </summary>
    public class udv_CheckingReport
    {
        /// <summary>
        /// 交易类型
        /// </summary>
        public int TradeType { set; get; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public long TradeAmount { set; get; }

    }

    /// <summary>
    /// 对账报表详情
    /// ,0.充值 1.购彩消费 2.提现冻结 3.提现失败解冻 4.金豆兑换 
    /// 5.中奖 11.用户撤单 12.系统撤单 13.追号撤单 14.投注失败退款 
    /// 15.出票失败退款 16.充值退款冻结 17.退款失败返回金额
    /// </summary>
    public class udv_CheckingReportDetail
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Day { set; get; }
        /// <summary>
        /// 充值：0
        /// </summary>
        public long Pay { set; get; }
        /// <summary>
        /// 购彩：1
        /// </summary>
        public long BuyLot { set; get; }
        /// <summary>
        /// 冻结：2
        /// </summary>
        public long Freeze { set; get; }
        /// <summary>
        /// 解冻：3
        /// </summary>
        public long UnFreeze { set; get; }
        /// <summary>
        /// 金豆兑换：4
        /// </summary>
        public long Imazamox { set; get; }
        /// <summary>
        /// 中奖：5
        /// </summary>
        public long WinMoney { set; get; }
        /// <summary>
        /// 用户撤单：11
        /// </summary>
        public long UserRevoke { set; get; }
        /// <summary>
        /// 系统撤单：12
        /// </summary>
        public long SystemRevoke { set; get; }
        /// <summary>
        /// 追号撤单：13
        /// </summary>
        public long ChaseRevoke { set; get; }
        /// <summary>
        /// 投注失败撤单：14
        /// </summary>
        public long BetRevoke { set; get; }
        /// <summary>
        /// 出票失败撤单：15
        /// </summary>
        public long TicketRevoke { set; get; }
        /// <summary>
        /// 充值退款冻结：16
        /// </summary>
        public long RefundFreeze { set; get; }
        /// <summary>
        /// 退款失败返回金额：17
        /// </summary>
        public long RefundFailure { set; get; }
        /// <summary>
        /// 是否合计数据
        /// 月合计
        /// </summary>
        public bool IsTotal { set; get; }
    }

}
