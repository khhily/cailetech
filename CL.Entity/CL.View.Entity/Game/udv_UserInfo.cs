using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    [Table("udv_UserInfo")]
    public class udv_UserInfo
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
        /// 用户密码
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// 支付密码(6位数字后的MD5)
        /// </summary>
        public string PayPassword { get; set; }

        /// <summary>
        /// 用户余额
        /// </summary>
        public long Balance { get; set; }

        /// <summary>
        /// 金豆与余额的兑换比例是1:100
        /// </summary>
        public long GoldBean { get; set; }

        /// <summary>
        /// 冻结金额
        /// </summary>
        public long Freeze { get; set; }

        ///// <summary>
        ///// 推荐人ID
        ///// </summary>
        //public int ReCommenderID { get; set; }

        /// <summary>
        /// 是否是机器人
        /// </summary>
        public bool IsRobot { get; set; }

        /// <summary>
        /// 账户是否允许登陆
        /// </summary>
        public bool IsCanLogin { get; set; }

        ///// <summary>
        ///// 云信id
        ///// </summary>
        //public string AccID { get; set; }

        /// <summary>
        /// 云信端唯一token
        /// </summary>
        //public string Token { get; set; }

        /// <summary>
        /// 绑定类型，0.未绑定，1.手机，2.微信，3.QQ，4.支付宝
        /// </summary>
        public byte BindType { get; set; } = 0;

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime? BindTime { get; set; }

        /// <summary>
        /// 第三方关联id
        /// </summary>
        public string RelationID { get; set; }

        /// <summary>
        /// 是否认证通过
        /// </summary>
        public bool? IsVerify { get; set; }

        /// <summary>
        /// 是否绑定手机
        /// </summary>
        public bool? IsBindTel { get; set; }

        /// <summary>
        /// 关注数
        /// </summary>
        public int? Idols { get; set; }

        /// <summary>
        /// 粉丝数
        /// </summary>
        //public int? Fans { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 用户头像地址
        /// </summary>
        public string AvatarAddress { get; set; }
    }
}
