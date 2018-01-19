using CL.Tools.Common;
using CL.View.Entity.Integrated;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace CL.Game.BLL.Integrated
{
    public abstract class BaseOAuth
    {
        public HttpRequest Request = HttpContext.Current.Request;
        public HttpResponse Response = HttpContext.Current.Response;
        public HttpSessionState Session = HttpContext.Current.Session;

        public abstract void Login();
        public abstract udv_Integrated Callback();


        /// <summary>
        /// 生成唯一随机串防CSRF攻击
        /// </summary>
        /// <returns></returns>
        protected string GetStateCode()
        {
            Random rand = new Random();
            string data = DateTime.Now.ToString("yyyyMMddHHmmssffff") + rand.Next(1, 0xf423f).ToString();

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] md5byte = md5.ComputeHash(UTF8Encoding.Default.GetBytes(data));

            return BitConverter.ToString(md5byte).Replace("-", "");

        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected string GetRequest(string url)
        {
            HttpWebRequest httpWebRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.Method = "GET";
            httpWebRequest.ServicePoint.Expect100Continue = false;

            StreamReader responseReader = null;
            string responseData;
            try
            {
                responseReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            finally
            {
                httpWebRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
            }

            return responseData;
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        protected string PostRequest(string url, string postData)
        {
            HttpWebRequest httpWebRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.Method = "POST";
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            //写入POST参数
            StreamWriter requestWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            try
            {
                requestWriter.Write(postData);
            }
            finally
            {
                requestWriter.Close();
            }

            //读取请求后的结果
            StreamReader responseReader = null;
            string responseData;
            try
            {
                responseReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            finally
            {
                httpWebRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
            }

            return responseData;
        }

        /// <summary>
        /// 解析JSON
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        protected NameValueCollection ParseJson(string strJson)
        {
            NameValueCollection mc = new NameValueCollection();
            Regex regex = new Regex(@"(\s*\""?([^""]*)\""?\s*\:\s*\""?([^""]*)\""?\,?)");
            strJson = strJson.Trim();
            if (strJson.StartsWith("{"))
            {
                strJson = strJson.Substring(1, strJson.Length - 2);
            }

            foreach (Match m in regex.Matches(strJson))
            {
                mc.Add(m.Groups[2].Value, m.Groups[3].Value);
            }
            return mc;
        }

        /// <summary>
        /// 解析URL
        /// </summary>
        /// <param name="strParams"></param>
        /// <returns></returns>
        protected NameValueCollection ParseUrlParameters(string strParams)
        {
            NameValueCollection nc = new NameValueCollection();
            foreach (string p in strParams.Split('&'))
            {
                string[] ps = p.Split('=');
                nc.Add(ps[0], ps[1]);
            }
            return nc;
        }

        #region 图片解析存储
        /// <summary>
        /// 保存集成登录拉取的头像
        /// </summary>
        /// <param name="OpenId"></param>
        /// <param name="ImgPath"></param>
        /// <param name="IntegratedType"></param>
        /// <returns></returns>
        public string AnalysisImgPath(string OpenId, string ImgPath, string IntegratedType)
        {
            string ResultPath = string.Empty;
            string SiskSite = string.Empty;
            string HttpURL = string.Empty;
            string Path = string.Empty;
            double Percent = 0.5;
            string ImgName = string.Empty;
            ImageClass img = new ImageClass(null);
            SiskSite = ConfigurationManager.AppSettings["ICON"];
            HttpURL = ConfigurationManager.AppSettings["ICONHOSTURL"];
            if (SiskSite.ToLower().IndexOf("http://") >= 0 || SiskSite.ToLower().IndexOf("https://") >= 0)
            {
                //根据URL地址上传到服务器上
                ImgName = OpenId + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_Icon.jpg";
                Path = string.Format("{0}{1}", SiskSite, ImgName);
                var r_save = img.HttpThumbnailImage(SiskSite, ImgPath, Percent);
                if (!r_save)
                    ResultPath = string.Empty;
            }
            else
            {
                ImgName = OpenId + "_Icon.jpg";

                Path = string.Format(@"{0}\", IntegratedType);
                ResultPath = string.Format("{0}{1}{2}", HttpURL, Path.Replace(@"\", "/"), ImgName);


                Path = string.Format("{0}{1}", SiskSite, Path);
                //不存在则创建
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);

                Path = string.Format("{0}{1}", Path, ImgName);
                //存在则删除
                if (File.Exists(Path))
                    File.Delete(Path);

                var r_save = img.ThumbnailImage(Path, ImgPath, Percent);
                if (!r_save)
                    ResultPath = ImgPath;
            }
            return ResultPath;
        }
        protected bool ThumbnailCallback()
        {
            return false;
        }
        #endregion 


    }
}
