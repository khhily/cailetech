using CL.View.Entity.Game;
using System;
using System.Collections.Generic;

namespace CL.Tools.MSMQManager
{
    [Serializable]
    public class ElectronicTicket
    {
        /// <summary>
        /// 是否远程队列
        /// </summary>
        public bool isRemote = false;
        /// <summary>
        /// 消息队列名称
        /// </summary>
        public string ElectronicTicketQueueName = @".\Private$\ElectronicTicket";
        /// <summary>
        /// 彩种编码
        /// </summary>
        public int LotteryCode { get; set; }
        /// <summary>
        /// 方案ID
        /// </summary>
        public long SchemeID { get; set; }
        /// <summary>
        /// 是否机器人
        /// </summary>
        public bool IsRobot { get; set; }
        /// <summary>
        /// 期号名称
        /// </summary>
        public string IsuseName { get; set; }
        /// <summary>
        /// 期号开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 期号结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 方案总金额
        /// </summary>
        public long SchemeMoney { get; set; }
        /// <summary>
        /// 追号ID  大于0为追号订单
        /// </summary>
        public long ChaseTaskDetailsID { get; set; }
        /// <summary>
        /// 投注明细
        /// </summary>
        public List<udv_Parameter> TicketDetails { get; set; }
    }
}
