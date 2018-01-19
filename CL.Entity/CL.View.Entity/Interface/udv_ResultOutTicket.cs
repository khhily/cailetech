using System.Collections.Generic;

namespace CL.View.Entity.Interface
{
    public class udv_ResultOutTicket
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// 具体信息
        /// </summary>
        public string ErrorMsg { get; set; }

        public List<udv_OutTicketEntites> ListOutTicker { get; set; }
    }
    public class udv_OutTicketEntites
    {
        /// <summary>
        /// 电子票ID
        /// </summary>
        public string SchemeETicketID { get; set; }
        /// <summary>
        /// 状态 0.不可出票 1.可出票状态 2.出票成功 3.出票失败(允许再出票） 4.出票中 5.出票中（体彩中心） 6.出票失败（不允许出票）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 彩票支撑系统票号
        /// </summary>
        public string TicketID { get; set; }
        /// <summary>
        /// 出票时间
        /// </summary>
        public string TicketTime { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string ExtendedValue { get; set; }
    }
}
