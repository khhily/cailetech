
namespace CL.View.Entity.Interface
{
    public class udv_ParaWinInfo
    {
        public long SchemeETicketID { get; set; }

        /// <summary>
        /// 中奖金额内部算奖用
        /// </summary>
        public long WinMoney { get; set; }

        /// <summary>
        /// 中奖税后金额内部算奖用
        /// </summary>
        public long WinMoneyNoWithTax { get; set; }
    }
}
