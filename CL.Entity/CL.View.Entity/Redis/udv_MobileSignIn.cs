
namespace CL.View.Entity.Redis
{
    public class udv_MobileSignIn
    {
        public string Token { set; get; }
        public long UserCode { set; get; }
        public string Nick { set; get; }
        public string Mobie { set; get; }
        public string Pwd { set; get; }
        public string PayPwd { set; get; }
        public long Balance { set; get; }
        public long Gold { set; get; }
        public bool IsRobot { set; get; }
        public string AvatarUrl { set; get; }
        public int VIP { set; get; }
        public int Level { set; get; }
        public int SpecialGroup { set; get; }
        public bool IsCertification { set; get; }
        public string WechatOpenID { set; get; }
        public string QQOpenID { set; get; }
        public string AliPayOpenID { set; get; }
        public string FullName { set; get; }
        public string PushIdentify { set; get; }
        /// <summary>
        /// 是否允许登录
        /// </summary>
        public bool IsCanLogin { set; get; }
    }
}
