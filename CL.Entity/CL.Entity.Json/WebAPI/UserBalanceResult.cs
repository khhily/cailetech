using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Json.Entity;

namespace CL.Entity.Json.WebAPI
{
    public class UserBalanceResult : JsonResult
    {
        /// <summary>
        /// 余额
        /// </summary>
        public long Balance { set; get; }

        /// <summary>
        /// 彩豆
        /// </summary>
        public long Gold { set; get; }
    }
}
