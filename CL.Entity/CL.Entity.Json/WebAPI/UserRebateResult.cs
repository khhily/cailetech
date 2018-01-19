using CL.Json.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Entity.Json.WebAPI
{
    public class UserRebateResult : JsonResult
    {
        public int Count { set; get; }
        public List<UserRebate> Data { set; get; }
    }

    public class UserRebate
    {
        /// <summary>
        /// 回水时间
        /// </summary>
        public string Time { set; get; }

        /// <summary>
        /// 回水
        /// </summary>
        public long Rebate { set; get; }

        /// <summary>
        /// 房间名
        /// </summary>
        public string RoomName { set; get; }
    }
}
