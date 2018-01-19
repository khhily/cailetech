using System.ComponentModel;

namespace CL.Enum.Common.Status
{
    public enum SalePointStatus
    {
        [Description("过期")]
        Overdue = -2,

        [Description("审核不通过")]
        AuditNotThrough = -1,

        [Description("待审核")]
        PendingAudit = 1,

        [Description("有效")]
        Effective = 2
    }
}
