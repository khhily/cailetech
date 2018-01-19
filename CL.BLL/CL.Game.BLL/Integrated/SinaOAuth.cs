using CL.View.Entity.Integrated;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace CL.Game.BLL.Integrated
{
    public class SinaOAuth : BaseOAuth
    {
        public string AppKey = ConfigurationManager.AppSettings["OAuth_Sina_AppKey"];
        public string AppSecret = ConfigurationManager.AppSettings["OAuth_Sina_AppSecret"];
        public string RedirectUrl = ConfigurationManager.AppSettings["OAuth_Sina_RedirectUrl"];

        public const string GET_AUTH_CODE_URL = "https://api.weibo.com/oauth2/authorize";
        public const string GET_ACCESS_TOKEN_URL = "https://api.weibo.com/oauth2/access_token";
        public const string GET_UID_URL = "https://api.weibo.com/2/account/get_uid.json";

        /// <summary>
        /// 新浪微博登录，跳转到登录页面
        /// </summary>
        public override void Login()
        {
            //-------生成唯一随机串防CSRF攻击
            string state = GetStateCode();
            Session["Sina_State"] = state; //state 放入Session

            string parms = "?client_id=" + AppKey + "&redirect_uri=" + Uri.EscapeDataString(RedirectUrl)
              + "&state=" + state;

            string url = GET_AUTH_CODE_URL + parms;
            Response.Redirect(url); //跳转到登录页面
        }

        /// <summary>
        /// 新浪微博回调函数
        /// </summary>
        /// <returns></returns>
        public override udv_Integrated Callback()
        {
            string code = Request.QueryString["code"];
            string state = Request.QueryString["state"];

            //--------验证state防止CSRF攻击
            if (state != (string)Session["Sina_State"])
            {
                ShowError("The state does not match. You may be a victim of CSRF.");
            }

            string parms = "client_id=" + AppKey + "&client_secret=" + AppSecret
              + "&grant_type=authorization_code&code=" + code + "&redirect_uri=" + Uri.EscapeDataString(RedirectUrl);

            string str = PostRequest(GET_ACCESS_TOKEN_URL, parms);

            NameValueCollection user = ParseJson(str);

            Session["Sina_AccessToken"] = user["access_token"]; //access_token 放入Session
            Session["Sina_UId"] = user["uid"]; //uid 放入Session
            return null;
        }


        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="description">错误描述</param>
        private void ShowError(string description = null)
        {
            Response.Write("<h2>" + description + "</h2>");
            Response.End();
        }
    }
}
