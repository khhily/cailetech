using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Type
{
    public enum LoginTypeEnum
    {
        [Description("QQ")]
        QQ = 1,

        [Description("微信")]
        WeChat = 2
    }
}
