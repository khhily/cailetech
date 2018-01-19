using System.ComponentModel;

namespace CL.Enum.Common.Lottery
{
    public enum CQSSCWinCode
    {

        [Description("一星直选单式")]
        ZX_1X = 3011001,

        [Description("二星直选")]
        ZX_2X = 3010601,

        [Description("二星直选和值")]
        ZXHZ_2X = 3010701,

        [Description("二星组选")]
        ZU_2X = 3010801,

        [Description("二星组选和值")]
        ZUHZ_2X = 3010901,

        [Description("三星直选")]
        ZX_3X = 3010301,

        [Description("三星组三")]
        ZU3_3X = 3010401,

        [Description("三星组六")]
        ZU6_3X = 3010501,

        [Description("五星直选")]
        ZX_5X = 3010101,

        [Description("五星通选全中")]
        TX_5X_ALLWin = 3010201,

        [Description("五星通选中前3或后3")]
        TX_5X_Q3HH3 = 3010202,

        [Description("五星通选中前2或后2")]
        TX_5X_Q2HH2 = 3010203,

        [Description("大小单双")]
        DXDS = 3011101
    }
}
