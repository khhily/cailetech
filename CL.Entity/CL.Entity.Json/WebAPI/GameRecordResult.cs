using CL.Json.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Entity.Json.WebAPI
{
    public class GameRecordResult : JsonResult
    {
        public List<GameRecord> Data { set; get; }
    }
    public class GameRecord
    {
        /// <summary>
        /// 交易时间
        /// </summary>
        public string Time { set; get; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public long Gold { set; get; }

        /// <summary>
        /// 交易说明
        /// </summary>
        public string Remarks { set; get; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string RecordType { set; get; }
    }
}
