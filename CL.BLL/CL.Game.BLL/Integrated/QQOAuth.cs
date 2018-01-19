using CL.View.Entity.Integrated;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace CL.Game.BLL.Integrated
{
    public class QQOAuth : BaseOAuth
    {
        public string AppId = ConfigurationManager.AppSettings["OAUTHQQAPPID"];
        public string AppKey = ConfigurationManager.AppSettings["OAUTHQQAPPKEY"];
        public string RedirectUrl = ConfigurationManager.AppSettings["OAUTHQQREDIRECTURL"];

        public const string GET_AUTH_CODE_URL = "https://graph.qq.com/oauth2.0/authorize";
        public const string GET_ACCESS_TOKEN_URL = "https://graph.qq.com/oauth2.0/token";
        public const string GET_OPENID_URL = "https://graph.qq.com/oauth2.0/me";
        public const string GET_USERINFO_URL = "https://graph.qq.com/user/get_user_info";

        protected string Access_Token = string.Empty;
        public QQOAuth(string code = null)
        {
            Access_Token = code;
        }

        public override void Login()
        {
            //-------生成唯一随机串防CSRF攻击
            string state = GetStateCode();
            Session["QC_State"] = state; //state 放入Session

            string parms = "?response_type=code&"
              + "client_id=" + AppId + "&redirect_uri=" + Uri.EscapeDataString(RedirectUrl) + "&state=" + state;

            string url = GET_AUTH_CODE_URL + parms;
            Response.Redirect(url); //跳转到登录页面
        }

        /// <summary>
        /// QQ回调函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override udv_Integrated Callback()
        {
            string code = Request.QueryString["code"];
            string state = Request.QueryString["state"];

            //--------验证state防止CSRF攻击
            if (state != (string)Session["QC_State"])
            {
                ShowError("30001");
            }

            string parms = "?grant_type=authorization_code&"
              + "client_id=" + AppId + "&redirect_uri=" + Uri.EscapeDataString(RedirectUrl)
              + "&client_secret=" + AppKey + "&code=" + code;

            string url = GET_ACCESS_TOKEN_URL + parms;
            string str = GetRequest(url);

            if (str.IndexOf("callback") != -1)
            {
                int lpos = str.IndexOf("(");
                int rpos = str.IndexOf(")");
                str = str.Substring(lpos + 1, rpos - lpos - 1);
                NameValueCollection msg = ParseJson(str);
                if (!string.IsNullOrEmpty(msg["error"]))
                {
                    ShowError(msg["error"], msg["error_description"]);
                }

            }

            NameValueCollection token = ParseUrlParameters(str);
            Session["QC_AccessToken"] = token["access_token"]; //access_token 放入Session
            return null;
        }


        /// <summary>
        /// 使用Access Token来获取用户的OpenID
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public udv_Integrated GetOpenID()
        {
            udv_Integrated entity = new udv_Integrated();
            string parms = "?access_token=" + Access_Token;

            string url = GET_OPENID_URL + parms;
            string str = GetRequest(url);

            if (str.IndexOf("callback") != -1)
            {
                int lpos = str.IndexOf("(");
                int rpos = str.IndexOf(")");
                str = str.Substring(lpos + 1, rpos - lpos - 1);
            }

            NameValueCollection user = ParseJson(str);

            if (!string.IsNullOrEmpty(user["error"]))
            {
                ShowError(user["error"], user["error_description"]);
                entity.ErrCode = user["error"];
                entity.ErrMsg = user["error_description"];
            }
            else
            {
                entity.OpenId = user["openid"];
                entity.Access_Token = Access_Token;
                #region 读取个人资料
                try
                {
                    parms = "?access_token=" + entity.Access_Token + "&oauth_consumer_key=" + AppId + "&openid=" + entity.OpenId;
                    url = GET_USERINFO_URL + parms;
                    str = GetRequest(url);
                    if (str.IndexOf("callback") != -1)
                    {
                        int lpos = str.IndexOf("(");
                        int rpos = str.IndexOf(")");
                        str = str.Substring(lpos + 1, rpos - lpos - 1);
                    }
                    user = ParseJson(str);
                    entity.NickName = user["nickname"];
                    entity.HeadImgUrl = user["figureurl_qq_1"].Replace(@"\/", "/");
                }
                catch
                {
                    throw;
                }
                #endregion 

            }

            return entity;
        }
        public string SaveImg(string OpenId, string ImgUrl)
        {
            return base.AnalysisImgPath(OpenId, ImgUrl, "QQ");
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
                        description = "<h2>可能是服务器无法请求https协议</h2>可能未开启curl支持,请尝试开启curl支持，重启web服务器，如果问题仍未解决，请联系我们";
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
