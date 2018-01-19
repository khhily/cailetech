
namespace CL.View.Entity.Redis
{
    public class udv_VerifyCode
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 短信类型：
        /// 0注册 1找回密码 3修改登录密码 5修改支付密码 7绑定修改手机号码 9绑定银行卡 11身份验证
        /// </summary>
        public byte VerifyType { set; get; }
    }
}
