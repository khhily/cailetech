using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Other
{
    public class ReptileHB11X5Info
    {
        /// <summary>
        /// 数据包
        /// </summary>
        public ReptileHB11X5Info_Data data { set; get; }
    }
    public class ReptileHB11X5Info_Data
    {
        /// <summary>
        /// 状态吗
        /// </summary>
        public string result { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public string desc { set; get; }
        /// <summary>
        /// 列表
        /// </summary>
        public List<ReptileHB11X5Info_Data_NumberList> numberList { set; get; }

    }
    public class ReptileHB11X5Info_Data_NumberList
    {
        /// <summary>
        /// 彩种编号
        /// </summary>
        public int lotteryId { set; get; }
        /// <summary>
        /// 期号
        /// </summary>
        public string issueNum { set; get; }
        /// <summary>
        /// 开奖时间
        /// </summary>
        public string bonusTime { set; get; }
        /// <summary>
        /// 领奖时间
        /// </summary>
        public string rewardTime { set; get; }
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string baseCode { set; get; }
    }
}
