
namespace CL.View.Entity.Redis
{
    public class udv_BankCard
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserCode { set; get; }
        /// <summary>
        /// 银行卡编号
        /// </summary>
        public long BankCode { set; get; }
        /// <summary>
        /// 银行卡名称
        /// </summary>
        public string BankName { set; get; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string CardNum { set; get; }
        /// <summary>
        /// 开卡城市
        /// </summary>
        public string Area { set; get; }
        /// <summary>
        /// 银行预留手机
        /// </summary>
        public string Mobile { set; get; }

    }
}
