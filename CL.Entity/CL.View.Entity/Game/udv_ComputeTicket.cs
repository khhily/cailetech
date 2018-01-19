using Dapper;

namespace CL.View.Entity.Game
{
    [Table("udv_ComputeTicket")]
    public class udv_ComputeTicket
    {
        [Key]
        public long SchemeETicketsID { get; set; }
        public int LotteryCode { get; set; }
        public int PlayCode { get; set; }
        public int WinCode { get; set; }
        public int Multiple { get; set; }
        public string Number { get; set; }
        public long SchemeID { get; set; }
        public string IsuseName { get; set; }
        public string OpenNumber { get; set; }
        public long SumWinMoney { get; set; }
        public long SumWinMoneyNoWithTax { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 0 1.适用于双色球大乐透等不能自动计算一等奖和二等奖奖金的彩种
        /// </summary>
        public int IsFirstPrize { get; set; } = 0;

        public long TicketMoney { set; get; }
    }
}
