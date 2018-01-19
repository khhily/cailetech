using System.ComponentModel;

namespace CL.Enum.Common
{
    public enum DbConnectionEnum
    {
        [Description("彩票业务数据库")]
        CaileGame,

        [Description("彩票系统数据库")]
        CaileSystem,

        [Description("彩票彩券数据库")]
        CaileCoupons
    }
}
