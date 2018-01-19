using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    /// <summary>
    /// 提现次数限制
    /// </summary>
    public class udv_WithdrawCount
    {
        /// <summary>
        /// 用户
        /// </summary>
        public long UserID { set; get; }
        /// <summary>
        /// 体现次数
        /// </summary>
        public int Count { set; get; }
        /// <summary>
        /// 提现时间
        /// </summary>
        public DateTime Time { set; get; }
        /// <summary>
        /// 日允许提现金额
        /// </summary>
        public long AllowMoney { set; get; }
    }
}
