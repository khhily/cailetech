
namespace CL.View.Entity.Redis
{
    public class udv_Recharge
    {

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserCode { set; get; }

        /// <summary>
        /// 充值金额
        /// </summary>
        public long Amount { set; get; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public long Balance { set; get; }

        /// <summary>
        /// 充值时间
        /// </summary>
        public string Date { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
    }
}
