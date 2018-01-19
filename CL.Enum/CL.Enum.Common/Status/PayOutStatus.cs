using System.ComponentModel;

namespace CL.Enum.Common.Status
{
    public enum PayOutStatus
    {
        [Description("申请中")]
        ApplyFor = 0,

        [Description("处理中")]
        DealWith = 2,

        [Description("处理完成")]
        OK = 4,

        [Description("提现失败")]
        Failure = 6
    }
}
