using System.ComponentModel;

namespace CL.Enum.Common.Type
{
    public enum BankType
    {

        [Description("工商银行")]
        ICBC = 1,

        [Description("建设银行")]
        CCB = 2,

        [Description("农业银行")]
        ABC = 3,

        [Description("交通银行")]
        BOCOM = 4,

        [Description("招商银行")]
        CMBC = 5,

        [Description("邮政银行")]
        Postbank = 6,

        [Description("微信")]
        WeChat = 100,

        [Description("财付通")]
        Tenpay = 101,

        [Description("支付宝")]
        Alipay = 102,

    }
}
