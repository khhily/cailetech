using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Type
{
    /// <summary>
    /// 支付类型：1.余额彩豆购彩券支付 2.余额支付 3.购彩券支付 4.彩豆支付 5.余额和购彩券支付 6.余额和彩豆支付 7.彩豆和购彩券支付 
    /// </summary>
    public enum PaymentTypeEnum
    {
        [Description("余额彩豆购彩券支付")]
        ALL = 1,

        [Description("余额支付")]
        Balance = 2,

        [Description("购彩券支付")]
        Film = 3,

        [Description("彩豆支付")]
        Gold = 4,

        [Description("余额和购彩券支付")]
        BalanceToFilm = 5,

        [Description("余额和彩豆支付")]
        BalanceToGold = 6,

        [Description("彩豆和购彩券支付")]
        FilmToGold = 7
    }
}
