using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Activity.Regular
{
    /// <summary>
    /// 规则类型：0 标准玩法加奖，1 追号加奖，2 胆拖玩法加奖，3 球队加奖，4 串关加奖，5 投注金额累计区间加奖，6 中奖金额区间加奖，
    /// 7 活动期间投注金额累计名次加奖，8 活动期间中奖金额累计名次加奖，9 数字彩中球加奖，10 节假日加奖
    /// </summary>
    public enum RegularType
    {
        [Description("标准玩法加奖")]
        NormAward = 0,

        [Description("追号加奖")]
        ChaseAward = 1,

        [Description("胆拖玩法加奖")]
        DanTuoAward = 2,

        [Description("球队加奖")]
        BallAward = 3,

        [Description("串关加奖")]
        OnAward = 4,

        [Description("投注累计区间加奖")]
        BetIntervalAward = 5,

        [Description("中奖区间加奖")]
        IntervalAward = 6,

        [Description("投注累计名次加奖")]
        BetRankingAward = 7,

        [Description("中奖累计名次加奖")]
        RankingAward = 8,

        [Description("数字彩中球加奖")]
        IntervalBallAward = 9,

        [Description("节假日加奖")]
        HolidayAward = 10
    }
}
