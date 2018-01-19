using System.ComponentModel;

namespace CL.Enum.Common.Lottery
{
    public enum HB11X5PlayCode
    {
        [Description("任选2")]
        R2 = 20101,

        [Description("任选3")]
        R3 = 20102,

        [Description("任选4")]
        R4 = 20103,

        [Description("任选5")]
        R5 = 20104,

        [Description("任选6")]
        R6 = 20105,

        [Description("任选7")]
        R7 = 20106,

        [Description("任选8")]
        R8 = 20107,

        [Description("前一")]
        Q1 = 20108,

        [Description("前二直选")]
        Q2_ZX = 20109,

        [Description("前二组选")]
        Q2_ZUX = 20110,

        [Description("前三直选")]
        Q3_ZX = 20111,

        [Description("前三组选")]
        Q3_ZUX = 20112,
    }
}
