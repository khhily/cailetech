using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    public class udv_ManualOpenLottery
    {
        /// <summary>
        /// 开奖账号
        /// </summary>
        public int OpenLotUser { set; get; }
        /// <summary>
        /// 开奖彩种
        /// </summary>
        public int LotteryCode { set; get; }
        /// <summary>
        /// 开奖期号
        /// </summary>
        public string IsuseName { set; get; }
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string OpenNumber { set; get; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { set; get; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { set; get; }
    }
}
