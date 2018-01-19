using Dapper;

namespace CL.View.Entity.Game
{
    [Table("udv_BettingTickets")]
    public class udv_BettingTickets
    {
        [Key]
        /// <summary>
        /// 电子票ID
        /// </summary>
        public long SchemeETicketsID { get; set; }
        public long SchemeID { set; get; }
        /// <summary>
        /// 彩票持有人真实姓名
        /// </summary>
        public string TicketUser { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string Identify { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { get; set; }
        /// <summary>
        /// 玩法编号
        /// </summary>
        public int PlayCode { get; set; }
        /// <summary>
        /// 期号
        /// </summary>
        public string IsuseName { get; set; }
        /// <summary>
        /// 投注号码
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 注数
        /// </summary>
        public int Multiple { get; set; }
        /// <summary>
        /// 注数
        /// </summary>
        public int Bet { get; set; }
        /// <summary>
        /// 投注金额
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        /// 状态,0.待出票，1.投注成功，2.出票完成，3.投注失败，4.出票失败，8.兑奖中，10中奖，11.不中
        /// </summary>
        public byte TicketStatus { get; set; }
        public long ChaseTaskDetailsID { set; get; }
    }
}
