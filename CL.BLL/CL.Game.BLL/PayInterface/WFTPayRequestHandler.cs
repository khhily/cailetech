using CL.Tools.Common;
using System;
using System.Collections;
using System.Text;
using System.Web;

namespace CL.Game.BLL.PayInterface
{
    public class WFTPayRequestHandler
    {
        public WFTPayRequestHandler(HttpContext httpContext)
        {
            parameters = new Hashtable();

            this.httpContext = httpContext;
        }

        /// <summary>
        /// 网关url地址
        /// </summary>
        private string gateUrl;

        /// <summary>
        /// 密钥
        /// </summary>
        private string key;

        /// <summary>
        /// 请求的参数
        /// </summary>
        protected Hashtable parameters;

        /// <summary>
        /// debug信息
        /// </summary>
        private string debugInfo;

        protected HttpContext httpContext;

        /// <summary>
        /// 初始化函数
        /// </summary>
        public virtual void init()
        {
            //nothing to do
        }

        /// <summary>
        /// 获取入口地址,不包含参数值
        /// </summary>
        /// <returns></returns>
        public String getGateUrl()
        {
            return gateUrl;
        }

        /// <summary>
        /// 设置入口地址,不包含参数值
        /// </summary>
        /// <param name="gateUrl">入口地址</param>
        public void setGateUrl(String gateUrl)
        {
            this.gateUrl = gateUrl;
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        public String getKey()
        {
            return key;
        }

        /// <summary>
        /// 设置密钥
        /// </summary>
        /// <param name="key">密钥字符串</param>
        public void setKey(string key)
        {
            this.key = key;
        }

        /// <summary>
        /// 获取带参数的请求URL
        /// </summary>
        /// <returns></returns>
        public virtual string getRequestURL()
        {
            this.createSign();

            StringBuilder sb = new StringBuilder();
            ArrayList akeys = new ArrayList(parameters.Keys);
            akeys.Sort();
            foreach (string k in akeys)
            {
                string v = (string)parameters[k];
                if (null != v && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + WFTPayHelper.UrlEncode(v, getCharset()) + "&");
                }
            }

            //去掉最后一个&
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return this.getGateUrl() + "?" + sb.ToString();
        }

        /// <summary>
        ///创建md5摘要,规则是:按参数名称a-z排序,遇到空值的参数不参加签名。
        /// </summary>
        public virtual void createSign()
        {
            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList(parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + this.getKey());

            string sign = WFTPayHelper.GetMD5(sb.ToString(), getCharset()).ToUpper();

            this.setParameter("sign", sign);

            //debug信息
            this.setDebugInfo(sb.ToString() + " => sign:" + sign);
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="parameter">参数名</param>
        /// <returns></returns>
        public string getParameter(string parameter)
        {
            string s = (string)parameters[parameter];
            return (null == s) ? "" : s;
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="parameter">参数名</param>
        /// <param name="parameterValue">参数值</param>
        public void setParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                if (parameters.Contains(parameter))
                {
                    parameters.Remove(parameter);
                }

                parameters.Add(parameter, parameterValue);
            }
        }

        public void doSend()
        {
            this.httpContext.Response.Redirect(this.getRequestURL());
        }

        /// <summary>
        /// 获取debug信息
        /// </summary>
        /// <returns></returns>
        public String getDebugInfo()
        {
            return debugInfo;
        }

        /// <summary>
        /// 设置debug信息
        /// </summary>
        /// <param name="debugInfo"></param>
        public void setDebugInfo(String debugInfo)
        {
            this.debugInfo = debugInfo;
        }

        /// <summary>
        /// 获取所有参数
        /// </summary>
        /// <returns></returns>
        public Hashtable getAllParameters()
        {
            return this.parameters;
        }

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        protected virtual string getCharset()
        {
            //return this.httpContext.Request.ContentEncoding.BodyName;
            return "utf-8";
        }

        /// <summary>
        /// 设置页面提交的请求参数
        /// </summary>
        /// <param name="paramNames">参数名</param>
        public void setReqParameters(string[] paramNames)
        {
            this.parameters.Clear();
            foreach (string pName in paramNames)
            {
                string reqVal = this.httpContext.Request[pName];
                if (String.IsNullOrEmpty(reqVal))
                {
                    continue;
                }
                this.parameters.Add(pName, reqVal);
            }
        }
    }
}
