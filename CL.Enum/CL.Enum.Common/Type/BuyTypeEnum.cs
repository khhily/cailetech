using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Type
{
    /// <summary>
    /// 购买类型 0.代购 1.追号 2.跟单
    /// </summary>
    public enum BuyTypeEnum
    {
        [Description("代购")]
        BuyReplace = 0,

        [Description("追号")]
        BuyChase = 1,

        [Description("跟单")]
        BuyFollow = 2,

    }
}
