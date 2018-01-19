using Dapper;
using System;

namespace CL.View.Entity.Game
{
    [Table("udv_UserAccountReport")]
    public class udv_UserAccountReport
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key]
        public long UserID { get; set; }
        /// <summary>
        /// 会员名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户手机号码
        /// </summary>
        public string UserMobile { get; set; }
        /// <summary>
        /// 是否是机器人
        /// </summary>
        public bool IsRobot { get; set; }

        /// <summary>
        /// 账户是否允许登陆
        /// </summary>
        public bool IsCanLogin { get; set; }
        /// <summary>
        /// 是否认证通过
        /// </summary>
        public bool? IsVerify { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 用户余额
        /// </summary>
        public long Balance { get; set; }
        /// <summary>
        /// 购彩余额汇总
        /// </summary>
        public long Buycp { get; set; }
        /// <summary>
        /// 充值余额汇总
        /// </summary>
        public long Recharge { get; set; }
        /// <summary>
        /// 中奖余额汇总
        /// </summary>
        public long Winning { get; set; }
        /// <summary>
        /// 成功提款余额汇总
        /// </summary>
        public long Withdraw { get; set; }
    }
}
