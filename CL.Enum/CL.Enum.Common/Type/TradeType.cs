using System.ComponentModel;

namespace CL.Enum.Common.Type
{
    public enum TradeType
    {
        [Description("充值")]
        Pay = 0,

        [Description("购彩消费")]
        BuyLottery = 1,

        [Description("提现")]
        PayOut = 2,

        [Description("提现失败退回")]
        PatOutFailure = 3,

        [Description("金豆兑换")]
        Conversion = 4,

        [Description("中奖")]
        Winning = 5,

        [Description("用户撤单")]
        UserRevoke = 11,

        [Description("系统撤单")]
        SystemRevoke = 12,

        [Description("追号撤单")]
        ChaseRevoke = 13,

        [Description("投注失败退款")]
        LotteryFailure = 14,

        [Description("出票失败退款")]
        TicketRefund = 15,

        [Description("申请退款冻结")]
        ApplyForRefundFreeze = 16,

        [Description("退款失败返回金额")]
        RefundFailedAmountReturned = 17,

    }
}
