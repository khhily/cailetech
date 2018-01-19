
namespace CL.View.Entity.Interface
{
    public class udv_ResultIssue
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// 具体信息
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 彩种编号
        /// </summary>
        public string LotteryCode { get; set; }
        /// <summary>
        /// 期号
        /// </summary>
        public string Issue { get; set; }
        /// <summary>
        /// 开始销售时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 停止销售时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 状态 0.未开启 1.开始 2.暂停 3.截止 4.期结 5.返奖 6.返奖结束
        /// </summary>
        public string Satus { get; set; }
        /// <summary>
        /// 全国销售金额
        /// </summary>
        public string SaleMoney { get; set; }
        /// <summary>
        /// 全国中奖金额
        /// </summary>
        public string BonusMoney { get; set; }
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string OpenNumber { get; set; }
    }
}
