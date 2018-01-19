
namespace CL.View.Entity.Game
{
    public class udv_SchemesDetailsWin
    {
        /// <summary>
        /// 编号ID
        /// </summary>
        public long SchemesDetailsWinID { get; set; }

        /// <summary>
        /// 追号
        /// </summary>
        public long ChaseTaskDetailsID { set; get; }

        /// <summary>
        /// 电子票ID
        /// </summary>
        public long SchemeETicketsID { get; set; }

        /// <summary>
        /// 方案ID
        /// </summary>
        public long SchemeID { get; set; }

        /// <summary>
        /// 方案发起者ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 奖等编号
        /// </summary>
        public int WinCode { get; set; }

        /// <summary>
        /// 中奖金额
        /// </summary>
        public long WinMoney { get; set; }

        /// <summary>
        /// 中奖税后金额
        /// </summary>
        public long WinMoneyNoWithTax { get; set; }
    }
}
