
namespace CL.View.Entity.Redis
{
    /// <summary>
    /// 提现记录
    /// </summary>
    public class udv_WithdrawDeposit
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserCode { set; get; }
        /// <summary>
        /// 银行编号
        /// </summary>
        public long BankCode { set; get; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public long Amount { set; get; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public long Balance { set; get; }
        /// <summary>
        /// 提现时间
        /// </summary>
        public string Date { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }

    }
}
