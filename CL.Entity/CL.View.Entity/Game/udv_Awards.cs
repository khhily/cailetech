using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    /// <summary>
    /// 加奖对象
    /// </summary>
    public class udv_Awards
    {
        /// <summary>
        /// 方案电子票
        /// </summary>
        public long tid { set; get; }
        /// <summary>
        /// 方案标识
        /// </summary>
        public long oid { set; get; }
        /// <summary>
        /// 规则标识
        /// </summary>
        public int rid { set; get; }
        /// <summary>
        /// 加奖类型
        /// </summary>
        public int at { set; get; }
        /// <summary>
        /// 加奖金额
        /// </summary>
        public long am { set; get; }
    }
}
