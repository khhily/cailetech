using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Push.JiGuang
{
    /// <summary>
    /// 推送平台
    /// </summary>
    public enum PlatformEnum
    {
        [Description("所有平台")]
        all,

        [Description("安卓系统")]
        android,

        [Description("苹果系统")]
        ios
    }
}
