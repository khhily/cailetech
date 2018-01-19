using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    public class udv_UsersRecord
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserID { set; get; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public long TradeAmount { set; get; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public long Balance { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { set; get; }

    }
}
