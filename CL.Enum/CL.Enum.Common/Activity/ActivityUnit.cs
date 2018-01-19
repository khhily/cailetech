using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Activity
{
    /// <summary>
    /// 币种单位(彩乐平台币种)：0 人民币(余额)，1 元宝，2 游戏币，3 彩券
    /// </summary>
    public enum ActivityUnit
    {
        [Description("余额")]
        Balance = 0,

        [Description("元宝")]
        Ingot = 1,

        [Description("游戏币")]
        Currency = 2,

        [Description("彩券")]
        Coupons = 3

    }
}
