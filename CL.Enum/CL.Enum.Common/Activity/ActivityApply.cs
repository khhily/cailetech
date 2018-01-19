using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Activity
{
    /// <summary>
    /// 活动申请状态：0 申请中，1 审核通过，2 审核拒绝，3 变更申请，4 变更审核通过，5 变更审核拒绝
    /// </summary>
    public enum ActivityApply
    {
        [Description("申请中")]
        Apply = 0,

        [Description("审核通过")]
        AuditingAdopt = 1,

        [Description("审核拒绝")]
        AuditingRefuse = 2,

        [Description("变更申请")]
        UpdateApply = 3,

        [Description("变更审核通过")]
        UpdateAuditingAdopt = 4,

        [Description("变更审核拒绝")]
        UpdateAuditingRefuse = 5

    }
}
