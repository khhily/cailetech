using System.ComponentModel;

namespace CL.Enum.Common.Lottery
{
    public enum CQSSCPlayCode
    {

        [Description("一星直选单式")]
        ZX_1X = 30110,

        [Description("二星直选")]
        ZX_2X = 30106,

        [Description("二星直选和值")]
        ZXHZ_2X = 30107,

        [Description("二星组选")]
        ZU_2X = 30108,

        [Description("二星组选和值")]
        ZUHZ_2X = 30109,

        [Description("三星直选")]
        ZX_3X = 30103,

        [Description("三星组三")]
        ZU3_3X = 30104,

        [Description("三星组六")]
        ZU6_3X = 30105,

        [Description("五星直选")]
        ZX_5X = 30101,

        [Description("五星通选")]
        TX_5X = 30102,

        [Description("大小单双")]
        DXDS = 30111
    }
}
