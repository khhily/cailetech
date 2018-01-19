using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Activity
{
    /// <summary>
    /// 活动类型：0 官方活动，1 彩乐平台活动
    /// </summary>
    public enum ActivityType
    {
        [Description("官方活动")]
        OfficialActivity = 0,

        [Description("彩乐活动")]
        CaileActivity = 1
    }
}
