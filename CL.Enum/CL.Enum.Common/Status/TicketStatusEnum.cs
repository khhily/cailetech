using System.ComponentModel;

namespace CL.Enum.Common.Status
{
    public enum TicketStatusEnum
    {
        [Description("待出票")]
        InTicke = 0,

        [Description("投注成功")]
        BettingSuccess = 1,

        [Description("出票完成")]
        InTickeSuccess = 2,

        [Description("投注失败")]
        BettingFailure = 3,

        [Description("出票失败")]
        InTickeFailure = 4,

        [Description("兑奖中")]
        Expiry = 8,

        [Description("中奖")]
        Winning = 10,

        [Description("不中")]
        NoWinning = 11,

    }
}
