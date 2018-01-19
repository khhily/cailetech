namespace CL.View.Entity.Game
{
    public class udv_ChaseDetailRecord
    {
        //,,,,,,,,,
        /// <summary>
        /// 追号详情标识
        /// </summary>
        public long ID { set; get; }
        /// <summary>
        /// 是否开奖
        /// </summary>
        public bool IsOpened { set; get; }
        /// <summary>
        /// 期号
        /// </summary>
        public string IsuseName { set; get; }
        /// <summary>
        /// 购彩金额
        /// </summary>
        public long Amount { set; get; }
        /// <summary>
        /// 是否执行
        /// </summary>
        public bool IsExecuted { set; get; }
        /// <summary>
        /// 是否发送到电子票
        /// </summary>
        public bool IsSendOut { set; get; }
        /// <summary>
        /// 方案编号
        /// </summary>
        public long SchemeID { set; get; }
        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { set; get; }
        /// <summary>
        /// 期号标识
        /// </summary>
        public long IsuseID { set; get; }
        /// <summary>
        /// 中奖金额
        /// </summary>
        public long WinMoney { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public int QuashStatus { set; get; }

    }
}
