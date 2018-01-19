using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Activity.Regular
{
    /// <summary>
    /// 类型：1 周六加奖，2 周日加奖，3 周六日加奖，4 指定时间段加奖(按活动开始结束时间加奖)
    /// </summary>
    public enum HolidayType
    {
        [Description("周六加奖")]
        Saturday = 1,

        [Description("周日加奖")]
        Sunday = 2,

        [Description("周六日加奖")]
        SaturdaySunday = 3,

        [Description("指定时间段加奖")]
        Time = 3,
    }
}
