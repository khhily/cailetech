using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.IM
{
    /// <summary>
    /// 1代表 记录聊天数据，非1不记录
    /// </summary>
    public enum IMRecordEnums
    {
        [Description("记录聊天数据")]
        Yes = 1,

        [Description("不记录")]
        No = -1,

    }
}
