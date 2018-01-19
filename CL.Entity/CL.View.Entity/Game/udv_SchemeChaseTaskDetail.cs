using Dapper;
using System;

namespace CL.View.Entity.Game
{
    [Table("udv_SchemeChaseTaskDetail")]
    public class udv_SchemeChaseTaskDetail
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long SchemeID { get; set; }
        /// <summary>
        /// 期号名
        /// </summary>
        public string IsuseName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 本期追号金额
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        /// 奖金
        /// </summary>
        public long WinMoney { get; set; }
        /// <summary>
        /// 状态,0.待出票，1.投注成功，2.出票完成，3.投注失败，4.出票失败，8.兑奖中，10中奖，11.不中
        /// </summary>
        public byte TicketStatus { get; set; }
    }
}
