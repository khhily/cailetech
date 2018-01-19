using System.ComponentModel;

namespace CL.Enum.Common.IM
{
    /// <summary>
    /// 消息发送目标:1为全服   2为房间   3为个人
    /// </summary>
    public enum SendToEnums
    {
        [Description("为全服")]
        All = 1,

        [Description("为房间")]
        Room = 2,

        [Description("为个人")]
        Personal = 3,
    }
}
