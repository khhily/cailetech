using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Other
{
    /// <summary>
    /// 显示设备：1.全部 2.网站 3.客户端
    /// </summary>
    public enum EquipmentEnum
    {
        [Description("全部")]
        All = 1,

        [Description("网站")]
        Web = 2,

        [Description("客户端")]
        Client = 3
    }
}
