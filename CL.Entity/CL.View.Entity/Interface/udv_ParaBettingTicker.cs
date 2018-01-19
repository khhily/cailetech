
namespace CL.View.Entity.Interface
{
    public class udv_ParaBettingTicker
    {
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
        /// 电子票ID
        /// </summary>
        public long SchemeETicketID { get; set; }
        /// <summary>
        /// 彩种代码
        /// </summary>
        public int LotteryCode { get; set; }
        /// <summary>
        /// 期号
        /// </summary>
        public string Issue { get; set; }
        /// <summary>
        /// 玩法代码
        /// </summary>
        public int PlayType { get; set; }
        /// <summary>
        /// 投注号码
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 倍数
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
    }

    public class udv_ParaFootballBettingTicker
    {
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
        /// 电子票ID
        /// </summary>
        public long SchemeETicketID { get; set; }
        /// <summary>
        /// 彩种代码
        /// </summary>
        public int LotteryCode { get; set; }
        /// <summary>
        /// 期号
        /// </summary>
        public string Issue { get; set; }
        /// <summary>
        /// 玩法代码
        /// </summary>
        public int PlayType { get; set; }
        /// <summary>
        /// 投注号码
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 倍数
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
        /// 方案投注最小场次^方案投注最大场次
        /// </summary>
        public string BetLotteryMode { set; get; }
    }
}
