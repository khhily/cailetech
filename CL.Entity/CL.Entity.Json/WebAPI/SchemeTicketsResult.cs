using CL.Json.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Entity.Json.WebAPI
{
    public class SchemeTicketsResult : JsonResult
    {
        /// <summary>
        /// 电子票集
        /// </summary>
        public List<SchemeTicketsInfo> Data { set; get; }
    }

    public class SchemeTicketsInfo
    {
        /// <summary>
        /// 电子票号
        /// </summary>
        public string Tickets { set; get; }
        /// <summary>
        /// 电子票状态：0.待出票，1.投注成功，2.出票完成，3.投注失败，4.出票失败，8.兑奖中，10中奖，11.不中
        /// </summary>
        public int TicketStatus { set; get; }
        /// <summary>
        /// 电子票信息
        /// </summary>
        public string Number { set; get; }
        /// <summary>
        /// 中奖金额
        /// </summary>
        public long WinMoney { set; get; }
        /// <summary>
        /// 玩法编号
        /// </summary>
        public int PlayCode { set; get; }
        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { set; get; }
    }
}
