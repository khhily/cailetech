using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    public class udv_AwardPlayCode
    {
        /// <summary>
        /// 玩法编号
        /// </summary>
        public int PlayCode { set; get; }
        /// <summary>
        /// 加奖金额
        /// 区间或名称加奖为零
        /// </summary>
        public long AwardMoney { set; get; }
    }
}
