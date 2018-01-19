using CL.Game.Entity;

namespace CL.View.Entity.Game
{
    public class udv_IsuseInfo : IsusesEntity
    {

        /// <summary>
        /// 彩种规则 10@分@79@08:30-21:30  间隔-单位-期数-开奖周期
        /// </summary>
        public string IntervalType { get; set; }

        /// <summary>
        /// 期号提前结束时间(分钟为单位)
        /// </summary>
        public int AdvanceEndTime { set; get; }

        /// <summary>
        /// 预售时间(分钟为单位)
        /// </summary>
        public int PresellTime { set; get; }
    }
}
