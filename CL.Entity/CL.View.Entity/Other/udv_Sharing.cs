using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Other
{
    /// <summary>
    /// 分享限制
    /// </summary>
    public class udv_Sharing
    {
        /// <summary>
        /// 分享用户
        /// </summary>
        public long UserCode { set; get; }
        /// <summary>
        /// 已分享次数
        /// </summary>
        public int Sharing { set; get; }
        /// <summary>
        /// 最后一次分享时间
        /// </summary>
        public DateTime Time { set; get; }
    }
}
