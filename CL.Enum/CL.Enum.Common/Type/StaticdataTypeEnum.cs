using System.ComponentModel;

namespace CL.Enum.Common.Type
{
    /// <summary>
    /// 静态数据类型 1.今日充值总额 2.今日线上充值 3.今日线下充值 4.今日提现总额 5.今日新增会员 6.今日赠送金额 7.彩种投注 8.彩种中奖
    /// </summary>
    public enum StaticdataTypeEnum
    {
        [Description("今日充值总额")]
        DayRecharge = 1,

        [Description("今日线上充值")]
        DayOnline = 2,

        [Description("今日线下充值")]
        DayOffline = 3,

        [Description("今日提现总额")]
        DayWithdraw = 4,

        [Description("今日新增会员")]
        DayUsers = 5,

        [Description("今日赠送金额")]
        DayLargess = 6,

        [Description("彩种投注")]
        LotBuy = 7,

        [Description("彩种中奖")]
        LotWin = 8
    }
}
