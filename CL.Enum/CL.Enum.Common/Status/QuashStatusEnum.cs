using System.ComponentModel;


namespace CL.Enum.Common.Status
{
    public enum QuashStatusEnum
    {
        [Description("未撤销")]
        NoQuash = 0,

        [Description("用户撤销")]
        UserQuash = 1,

        [Description("系统撤销")]
        SysQuash = 2,
    }
}
