using System;
using System.Collections.Generic;

namespace CL.Game.Entity
{
    public class udv_UserPushList
    {
         /// <summary>
         /// 用户标识
         /// </summary>
        public long UserID { set; get; }
        
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// 推送标识
        /// </summary>
        public string PushIdentify { set; get; }
    }
}
