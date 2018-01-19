using CL.Json.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Entity.Json.WebAPI
{
    public class RockDiceResult : JsonResult
    {
        public RockDice Data { set; get; }
    }
    public class RockDice
    {
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string Num { set; get; }

        /// <summary>
        /// 中奖金额
        /// </summary>
        public long Win { set; get; }

        /// <summary>
        /// 剩余彩豆
        /// </summary>
        public long Gold { set; get; }

    }
}
