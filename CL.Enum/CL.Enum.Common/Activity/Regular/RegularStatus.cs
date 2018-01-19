using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Activity.Regular
{
    /// <summary>
    /// 规则状态：0 初始化规则，1 规则作废(活动审核失败)，2 规则开始并生效(活动审核通过)，3 活动截止并开始加奖(这里加奖针对活动期间累计加奖规则)，
    /// 4 活动结束并销毁(所有加奖派发完成后结束和销毁活动，销毁后的活动规则无法直接启用，启用销毁的规则需要重置及走审核流程)
    /// </summary>
    public enum RegularStatus
    {
        [Description("初始化规则")]
        Regular = 0,

        [Description("规则作废")]
        Cancellation = 1,

        [Description("规则开始并生效")]
        Effect = 2,

        [Description("活动截止并开始加奖")]
        Award = 3,

        [Description("活动结束并销毁")]
        Destroy = 4
    }
}
