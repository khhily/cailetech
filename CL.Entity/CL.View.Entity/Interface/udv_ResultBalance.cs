
namespace CL.View.Entity.Interface
{
    public class udv_ResultBalance
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
        /// 投注金账户金额
        /// </summary>
        public string ActMoney { get; set; }
        /// <summary>
        /// 奖金账户金额
        /// </summary>
        public string WinMoney { get; set; }
    }
}
