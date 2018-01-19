using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    public class udv_ExpireRevokeChase
    {
        /// <summary>
        /// 方案标识
        /// </summary>
        public long SchemeID { set; get; }

        /// <summary>
        /// 追号标识
        /// </summary>
        public long ChaseTaskDetailID { set; get; }

        /// <summary>
        /// 单期追号金额
        /// </summary>
        public long Amount { set; get; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public long UserID { set; get; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { set; get; }

        /// <summary>
        /// 方案号
        /// </summary>
        public string SchemeNumber { set; get; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nick { set; get; }
    }
}
