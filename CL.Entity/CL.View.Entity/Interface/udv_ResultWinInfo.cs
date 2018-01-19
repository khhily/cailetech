using System.Collections.Generic;

namespace CL.View.Entity.Interface
{
    public class udv_ResultWinInfo
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// 具体信息
        /// </summary>
        public string ErrorMsg { get; set; }

        public List<udv_WinInfoEntites> ListWinInfo { get; set; }
    }
    public class udv_WinInfoEntites
    {
        /// <summary>
        /// 电子票ID
        /// </summary>
        public string SchemeETicketID { get; set; }
        /// <summary>
        /// 中奖状态 0.未开奖 1.未中奖 2.中奖 3.可算奖状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 彩票支撑系统票号
        /// </summary>
        public string TicketID { get; set; }
        /// <summary>
        /// 税前奖金金额
        /// </summary>
        public string PrebonusValue { get; set; }
        /// <summary>
        /// 中奖金额
        /// </summary>
        public string BonusValue { get; set; }

        public long SchemeID { set; get; }

        public long ChaseTaskDetailsID { set; get; }
    }
}
