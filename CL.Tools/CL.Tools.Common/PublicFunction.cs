using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;

namespace CL.Tools.Common
{
    public class PublicFunction
    {
        private static string _StrKeyWord = GetConfigString("SqlInjectionKeyWord");

        /// <summary>
        /// 从QueryString获取整型
        /// </summary>
        /// <param name="requestName">参数名</param>
        /// <returns>为0则失败</returns>
        public static int RequestQueryInt(HttpContext context, string requestName)
        {
            int iData = -1;
            string sData = context.Request.QueryString[requestName];
            int.TryParse(sData, out iData);
            return iData;
        }
        public static long RequestQueryLong(HttpContext context, string requestName)
        {
            long iData = -1;
            string sData = context.Request.QueryString[requestName];
            long.TryParse(sData, out iData);
            return iData;
        }

        /// <summary>
        /// 从QueryString获取字符串型
        /// </summary>
        /// <param name="requestName">参数名</param>
        /// <returns>为0则失败</returns>
        public static string RequestQueryString(HttpContext context, string requestName)
        {
            return GetRequestQuery(context, requestName);
        }

        private static string GetRequestForm(HttpContext context, string requestName)
        {
            string sData = context.Request.Form[requestName];
            //为空、null 或 有注入时
            if (string.IsNullOrEmpty(sData) || CheckSqlInjection(context, sData))
            {
                sData = "";
            }

            return sData;
        }

        private static string GetRequestQuery(HttpContext context, string requestName)
        {
            string sData = context.Request.QueryString[requestName];
            //为空、null 或 有注入时
            if (string.IsNullOrEmpty(sData) || CheckSqlInjection(context, sData))
            {
                sData = "";
            }

            return sData;
        }

        /// <summary>
        /// 从Form 获取整型
        /// </summary>
        /// <param name="requestName">参数名</param>
        /// <returns>为0则失败</returns>
        public static int RequestFormInt(HttpContext context, string requestName)
        {
            int iData = -1;
            string sData = context.Request.Form[requestName];
            int.TryParse(sData, out iData);
            return iData;
        }
        /// <summary>
        /// 从Form 获取长整型
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requestName"></param>
        /// <returns></returns>
        public static long RequestFormLong(HttpContext context, string requestName)
        {
            long iData = -1;
            string sData = context.Request.Form[requestName];
            long.TryParse(sData, out iData);
            return iData;
        }

        /// <summary>
        /// 从Form获取字符串型
        /// </summary>
        /// <param name="requestName">参数名</param>
        /// <returns>为0则失败</returns>
        public static string RequestFormString(HttpContext context, string requestName)
        {
            return GetRequestForm(context, requestName);
        }

        /// <summary>
        /// 获取web.config配置信息
        /// </summary>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string GetConfigString(string sKey)
        {
            string sData = ConfigurationManager.AppSettings[sKey];
            if (!string.IsNullOrEmpty(sData))
                return sData.Trim();
            else
                return "";
        }

        //检测SQL注入   
        public static bool CheckSqlInjection(HttpContext context, string sWord)
        {
            //过滤关键字
            if (Regex.IsMatch(sWord, _StrKeyWord, RegexOptions.IgnoreCase))
            {
                if (GetConfigString("SqlInjectionLog") == "0")
                {
                    Log log = new Log("Safe");
                    log.Write("SqlInjection  clientip: " + context.Request.UserHostAddress + " url: " + context.Request.Url.ToString());
                }
                return true;
            }
            else
                return false;
        }
    }
}
