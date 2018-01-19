using System.ComponentModel;

namespace CL.Enum.Common.Lottery
{
    public enum JLXK3PlayCode
    {

        [Description("和值")]
        HZ = 10101,

        [Description("三同号通选")]
        STHTX = 10102,

        [Description("三同号单选")]
        STHDX = 10103,

        [Description("三同号单选")]
        SBTH = 10104,

        [Description("三连号通选")]
        SLHTX = 10105,

        [Description("二同号复选")]
        ETHFX = 10106,

        [Description("二同号单选")]
        ETHDX = 10107,

        [Description("二不同号")]
        EBTH = 10108
    }
}
