
namespace CL.View.Entity.Integrated
{
    public class udv_Integrated
    {
        /// <summary>
        /// 微信：用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
        /// </summary>
        public string OpenId { set; get; }

        /// <summary>
        /// 微信：网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        public string Access_Token { set; get; }

        /// <summary>
        /// 微信：用户刷新access_token
        /// </summary>
        public string Refresh_Token { set; get; }

        /// <summary>
        /// 微信：用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string Scope { set; get; }

        /// <summary>
        /// 微信：昵称
        /// </summary>
        public string NickName { set; get; }

        /// <summary>
        /// 微信：头像
        /// </summary>
        public string HeadImgUrl { set; get; }

        /// <summary>
        /// 微信：错误码
        /// </summary>
        public string ErrCode { set; get; } = "0";

        /// <summary>
        /// 微信：错误内容
        /// </summary>
        public string ErrMsg { set; get; }

        public string UnionId { set; get; }
    }
}
