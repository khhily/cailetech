using Dapper;

namespace CL.View.Entity.Game
{
    [Table("udv_OutTickets")]
    public class udv_OutTickets
    {
        [Key]
        public long SchemeETicketsID { get; set; }
        public long SchemeID { get; set; }
        public int LotteryCode { get; set; }
        /// <summary>
        /// 状态,0.待出票，1.投注成功，2.出票完成，3.投注失败，4.出票失败，8.兑奖中，10中奖，11.不中
        /// </summary>
        public byte TicketStatus { get; set; }

        public long ChaseTaskDetailsID { set; get; }
    }
}
