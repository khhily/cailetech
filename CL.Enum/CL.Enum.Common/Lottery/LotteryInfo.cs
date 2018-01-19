using System.ComponentModel;

namespace CL.Enum.Common.Lottery
{
    public enum LotteryInfo
    {
        [Description("红快3")]
        JLK3 = 101,

        [Description("赢快3")]
        JXK3 = 102,

        [Description("华11选5")]
        HB11X5 = 201,

        [Description("老11选5")]
        SD11X5 = 202,

        [Description("老时时彩")]
        CQSSC = 301,

        [Description("江西时时彩")]
        JXSSC = 302,

        [Description("双色球")]
        SSQ = 801,

        [Description("超级大乐透")]
        CJDLT = 901,

        [Description("幸运28")]
        XY28 = 40010,

        [Description("加拿大28")]
        JND28 = 40020,
    }
}
