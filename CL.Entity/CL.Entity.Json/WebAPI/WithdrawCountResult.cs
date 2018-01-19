using CL.Json.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Entity.Json.WebAPI
{
    public class WithdrawCountResult : JsonResult
    {
        /// <summary>
        /// 提现次数
        /// </summary>
        public int Count { set; get; }
        /// <summary>
        /// 日允许提现金额
        /// </summary>
        public long AllowMoney { set; get; }
        /// <summary>
        /// 可提现金额
        /// </summary>
        public long WithdrawMoney { set; get; }

    }
}
