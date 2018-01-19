using System.ComponentModel;

namespace CL.Enum.Common.Type
{
    public enum StopType
    {
        [Description("中奖后停止追加")]
        AfterWinning = 0,

        [Description("追加期数完成后停止追加")]
        AfterIsuse = -1
    }
}
