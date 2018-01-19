using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    public class cv_UserRebate
    {
        /// <summary>
        /// 回水
        /// </summary>
        public long TradeAmount { set; get; }

        /// <summary>
        /// 回水时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 房间名称
        /// </summary>
        public string RoomName { set; get; }

        /// <summary>
        /// 房间id
        /// </summary>
        public long RoomID { set; get; }
    }
}
