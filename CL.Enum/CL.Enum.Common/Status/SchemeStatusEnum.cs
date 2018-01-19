using System.ComponentModel;

namespace CL.Enum.Common.Status
{
    public enum SchemeStatusEnum
    {
        [Description("待付款")]
        InPayment = 0,

        [Description("订单过期")]
        OrderOver = 2,

        [Description("下单成功")]
        OrderSuccess = 4,

        [Description("出票成功")]
        TicketSuccess = 6,

        [Description("部分出票成功")]
        PartTicketSuccess = 8,

        [Description("下单失败（限号）")]
        OrderFailure = 10,

        [Description("订单撤销")]
        OrderRevoke = 12,

        [Description("中奖")]
        Winning = 14,

        [Description("派奖中")]
        Sending = 15,

        [Description("派奖完成")]
        SendSuccess = 16,

        [Description("未中奖")]
        NoWinningSuccess = 18,

        [Description("追号进行中")]
        Chaseing = 19,

        [Description("追号完成")]
        ChaseSuccess = 20,
    }
}
