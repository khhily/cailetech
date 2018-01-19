using System.Collections.Generic;

namespace CL.View.Entity.Interface
{
    public class udv_ResultBetting
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// 具体信息
        /// </summary>
        public string ErrorMsg { get; set; }

        public List<udv_BettingEntites> ListTicker { get; set; }
    }
    public class udv_BettingEntites
    {
        /// <summary>
        /// 电子票ID
        /// </summary>
        public string SchemeETicketID { get; set; }
        /// <summary>
        /// 彩票支撑系统票号
        /// </summary>
        public string TicketID { get; set; }
        /// <summary>
        /// 账户金额
        /// </summary>
        public string ActMoney { get; set; }
        /// <summary>
        /// 处理状态 0.成功 1.失败
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 具体信息
        /// </summary>
        public string Msg { get; set; }
    }
}
