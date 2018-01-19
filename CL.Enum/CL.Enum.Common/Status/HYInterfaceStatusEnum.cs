using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Status
{
    /// <summary>
    /// 华阳接口票状态
    /// 状态 0.未开奖 1.未中奖 2.中奖 3.可算奖状态
    /// </summary>
    public enum HYInterfaceStatusEnum
    {
        [Description("未开奖")]
        NoOpen = 0,

        [Description("未中奖")]
        NoAward = 1,

        [Description("中奖")]
        Award = 2,

        [Description("可算奖状态")]
        Awarding = 3,
    }
}
