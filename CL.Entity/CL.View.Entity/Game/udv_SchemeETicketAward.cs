using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    /// <summary>
    /// 
    /// </summary>
    public class udv_SchemeETicketAward
    {
        /// <summary>
        /// 电子票标识
        /// </summary>
        public long SchemeETicketsID { set; get; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public long UserID { set; get; }
        /// <summary>
        /// 电子票状态
        /// </summary>
        public int TicketStatus { set; get; }
        /// <summary>
        /// 彩种
        /// </summary>
        public int LotteryCode { set; get; }
        /// <summary>
        /// 期号
        /// </summary>
        public string IsuseName { set; get; }
        /// <summary>
        /// 方案编号
        /// </summary>
        public string SchemeNumber { set; get; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 用户绑定的手机号码
        /// </summary>
        public string UserMobile { set; get; }
        /// <summary>
        /// 电子票金额
        /// </summary>
        public long TicketMoney { set; get; }
        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { set; get; }
        /// <summary>
        /// 投注内容
        /// </summary>
        public string Number { set; get; }
        /// <summary>
        /// 接口电子票表示
        /// </summary>
        public string Ticket { set; get; }
        /// <summary>
        /// 落地号
        /// </summary>
        public string Identifiers { set; get; }
    }

    public class udv_InterfaceAward : udv_SchemeETicketAward
    {

        public int InterfaceStatus { set; get; }

        public long WinMoney { set; get; }

        public long WinMoneyNoWithTax { set; get; }
    }
    public class udv_Award
    {
        /// <summary>
        /// 电子票标识
        /// </summary>
        public long tid { set; get; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public long uuid { set; get; }
        /// <summary>
        /// 税前奖金
        /// </summary>
        public long wm { set; get; }
        /// <summary>
        /// 税后奖金
        /// </summary>
        public long nwm { set; get; }
    }
}
