using Dapper;

namespace CL.View.Entity.Game
{
    [Table("udv_ComputeTicketChaseTasks")]
    public class udv_ComputeTicketChaseTasks
    {
        /// <summary>
        /// 电子票标识
        /// </summary>
        [Key]
        public long SchemeETicketsID { get; set; }

        /// <summary>
        /// 彩种编码
        /// </summary>
        public int LotteryCode { get; set; }


        /// <summary>
        /// 玩法编码
        /// </summary>
        public int PlayCode { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { get; set; }

        /// <summary>
        /// 投注号码
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 方案标识
        /// </summary>
        public long SchemeID { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        public string IsuseName { get; set; }

        /// <summary>
        /// 开奖号码
        /// </summary>
        public string OpenNumber { get; set; }

        /// <summary>
        /// 电子票状态 2出票完成
        /// </summary>
        public byte TicketStatus { set; get; }

        /// <summary>
        /// 是否开奖
        /// </summary>
        public bool IsOpened { set; get; }

        /// <summary>
        /// 期号状态  4结期
        /// </summary>
        public byte IsuseState { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int WinCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long SumWinMoney { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long SumWinMoneyNoWithTax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 0 1.适用于双色球大乐透等不能自动计算一等奖和二等奖奖金的彩种
        /// </summary>
        public int IsFirstPrize { get; set; } = 0;

        /// <summary>
        /// 追号详情ID
        /// </summary>
        public long ChaseTaskDetailsID { set; get; }
    }
}
