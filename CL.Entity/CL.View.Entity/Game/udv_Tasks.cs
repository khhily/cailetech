using System.Collections.Generic;

namespace CL.View.Entity.Game
{
    public class udv_Tasks
    {


        /// <summary>
        /// 停止条件(0表示中奖停止;大于0表示累计中奖达到停止)
        /// </summary>
        public long Stops { set; get; }

        /// <summary>
        /// 追号期数
        /// </summary>
        public int IsuseCount { set; get; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime { set; get; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { set; get; }

        /// <summary>
        /// 追号列表
        /// </summary>
        public List<udv_TasksDetails> Data { set; get; }


    }
    public class udv_TasksDetails
    {
        /// <summary>
        /// 期号ID
        /// </summary>
        public long IsuseID { set; get; }

        /// <summary>
        /// 当前期的追号金额
        /// </summary>
        public long Amount { set; get; }

        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { set; get; }
    }
}
