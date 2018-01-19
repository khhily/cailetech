using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Push.JiGuang
{
    public enum StyleEnum
    {
        [Description("默认")]
        AcquiesceIn = 0,

        [Description("大文本通知栏样式")]
        bigText = 1,

        [Description("文本条目通知栏样式")]
        Inbox = 2,

        [Description("大图片通知栏样式")]
        bigPicture = 3
    }
}
