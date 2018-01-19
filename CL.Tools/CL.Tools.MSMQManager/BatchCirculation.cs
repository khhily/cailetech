using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Tools.MSMQManager
{
    public class BatchCirculation
    {
        /// <summary>
        /// 批次
        /// </summary>
        public int BatchID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long CirculationID { set; get; }
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string Num { set; get; }
        /// <summary>
        /// 开奖彩豆
        /// </summary>
        public long Amount { set; get; }
        /// <summary>
        /// 是否远程队列
        /// </summary>
        public bool isRemote = false;
        /// <summary>
        /// 消息队列名称
        /// </summary>
        public string ElectronicTicketQueueName = @".\Private$\BatchCirculation";
    }
}
