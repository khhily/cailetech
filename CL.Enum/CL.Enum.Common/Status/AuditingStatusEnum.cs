using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Status
{
    /// <summary>
    /// 审核状态：1.待审核 2.审核通过 3.审核未通过
    /// </summary>
    public enum AuditingStatusEnum
    {
        [Description("待审核")]
        Wait = 1,

        [Description("审核通过")]
        Adopt = 2,

        [Description("审核拒绝")]
        Refuse = 3
    }
}
