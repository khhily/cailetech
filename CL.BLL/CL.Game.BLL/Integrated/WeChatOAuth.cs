using CL.View.Entity.Integrated;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace CL.Game.BLL.Integrated
{
    public class WeChatOAuth : BaseOAuth
    {
        public string AppId = ConfigurationManager.AppSettings["OAUTHWECHATAPPID"];
        public string AppSecret = ConfigurationManager.AppSettings["OAUTH_WECHATAPPSECRET"];
        public string RedirectUrl = ConfigurationManager.AppSettings["OAUTHWECHATREDIRECTURL"];

        public const string GET_AUTH_CODE_URL = "https://open.weixin.qq.com/connect/qrconnect";
        public const string GET_ACCESS_TOKEN_URL = "https://api.weixin.qq.com/sns/oauth2/access_token";
        public const string GET_USERINFO_URL = "https://api.weixin.qq.com/sns/userinfo";

        protected string WeChatCode = string.Empty;
        public WeChatOAuth(string code = null)
        {
            WeChatCode = code;
        }
        /// <summary>
        /// 微信登录，跳转到登录页面
        /// </summary>
        public override void Login()
        {
            //-------生成唯一随机串防CSRF攻击
            string state = GetStateCode();
            Session["Weixin_State"] = state; //state 放入Session

            string parms = "?appid=" + AppId
              + "&redirect_uri=" + Uri.EscapeDataString(RedirectUrl) + "&response_type=code&scope=snsapi_login"
              + "&state=" + state + "#wechat_redirect";

            string url = GET_AUTH_CODE_URL + parms;
            Response.Redirect(url); //跳转到登录页面
        }

        /// <summary>
        /// 微信回调函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override udv_Integrated Callback()
        {
            #region 注释代码
            #endregion
            udv_Integrated entity = new udv_Integrated();
            #region 验证信息
            string parms = "?appid=" + AppId + "&secret=" + AppSecret + "&code=" + WeChatCode + "&grant_type=authorization_code";
            string url = GET_ACCESS_TOKEN_URL + parms;
            string str = GetRequest(url);
            NameValueCollection msg = ParseJson(str);
            if (!string.IsNullOrEmpty(msg["errcode"]))
            {
                entity.ErrCode = msg["errcode"];
                entity.ErrMsg = msg["errmsg"];
            }
            else
            {
                entity.OpenId = msg["openid"];
                entity.Refresh_Token = msg["refresh_token"];
                entity.Access_Token = msg["access_token"];
                entity.Scope = msg["scope"];
                entity.UnionId = msg["unionid"];

                #region 读取个人资料
                try
                {
                    parms = "?access_token=" + entity.Access_Token + "&openid=" + entity.OpenId + "&lang=zh_CN ";
                    url = GET_USERINFO_URL + parms;
                    str = GetRequest(url);
                    msg = ParseJson(str);
                    entity.NickName = msg["nickname"];
                    entity.HeadImgUrl = msg["headimgurl"].Replace(@"\/", "/");
                }
                catch
                {
                    throw;
                }
                #endregion
            }
            #endregion
            return entity;
        }
        public string SaveImg(string OpenId, string ImgUrl)
        {
            return base.AnalysisImgPath(OpenId, ImgUrl, "WeChat");
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="code">错误编号</param>
        /// <param name="description">错误描述</param>
        private void ShowError(string code, string description = null)
        {
            if (description == null)
            {
                switch (code)
                {
                    case "20001":
                        description = "<h2>配置文件损坏或无法读取，请检查web.config</h2>";
                        break;
                    case "30001":
                        description = "<h2>The state does not match. You may be a victim of CSRF.</h2>";
                        break;
                    case "50001":
                        description = "<h2>接口未授权</h2>";
                        break;
                    default:
                        description = "<h2>系统未知错误，请联系我们</h2>";
                        break;
                }
                Response.Write(description);
                Response.End();
            }
            else
            {
                Response.Write("<h3>error:<h3>" + code + "<h3>msg:<h3>" + description);
                Response.End();
            }
        }

    }
}
