using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    public class cv_Ticket
    {
        /// <summary>
        /// 追号详情编号
        /// </summary>
        public long ChaseTaskDetailsID { set; get; }

        /// <summary>
        /// 中奖金额
        /// </summary>
        public long WinMoney { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        public int TicketStatus { set; get; }
    }
}
