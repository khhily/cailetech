using System.ComponentModel;

namespace CL.Enum.Common.Lottery
{
    /// <summary>
    /// 山东11选5(老十一选5:十一云夺金)
    /// 玩法枚举定义
    /// </summary>
    public enum SD11X5PlayCode
    {
        [Description("任选2")]
        R2 = 20201,

        [Description("任选3")]
        R3 = 20202,

        [Description("任选4")]
        R4 = 20203,

        [Description("任选5")]
        R5 = 20204,

        [Description("任选6")]
        R6 = 20205,

        [Description("任选7")]
        R7 = 20206,

        [Description("任选8")]
        R8 = 20207,

        [Description("前一")]
        Q1 = 20208,

        [Description("前二直选")]
        Q2_ZX = 20209,

        [Description("前二组选")]
        Q2_ZUX = 20210,

        [Description("前三直选")]
        Q3_ZX = 20211,

        [Description("前三组选")]
        Q3_ZUX = 20212,

        [Description("乐选3")]
        LX3 = 20213,

        [Description("乐选4")]
        LX4 = 20214,

        [Description("乐选5")]
        LX5 = 20215,
    }
}
