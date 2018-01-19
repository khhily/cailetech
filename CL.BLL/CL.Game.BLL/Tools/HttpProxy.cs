using CL.Redis.BLL;
using CL.Tools.Common;
using CL.View.Entity.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.BLL.Tools
{
    /// <summary>
    /// http代理
    /// </summary>
    public static class HttpProxy
    {
        static Log log = new Log("HttpProxy");
        public static string HttpGetProxy(string url, Encoding EnCodeing = null)
        {
            try
            {
                string rec = string.Empty;
                var Entitys = new BusinessRedis().QueryHttpProxy();
                if (Entitys == null || Entitys.Count == 0)
                {
                    //接口获取最新代理地址
                    Entitys = GetProxyHost();
                    if (Entitys != null && Entitys.Count > 0)
                        new BusinessRedis().InsertHttpProxy(Entitys);
                }
                if (Entitys == null || Entitys.Count == 0) //没有代理IP时 不使用代理
                    rec = Utils.HttpGet(url);
                else
                {
                    //随机获取单个代理对象
                    Random rd = new Random(Utils.GenerateRandomSeed());
                    int i = rd.Next(Entitys.Count);
                    var Entity = Entitys[i];
                    if (EnCodeing == null)
                        EnCodeing = Encoding.UTF8;
                    rec = Utils.HttpGetProxy(url, Entity.host, Entity.port, EnCodeing);
                }
                return rec;
            }
            catch (Exception ex)
            {
                log.Write("HTTP代理请求错误：" + ex.Message, true);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取代理数据
        /// </summary>
        /// <returns></returns>
        public static List<udv_HttpProxy> GetProxyHost()
        {
            try
            {
                //num:条  longlife:在线时间   format:回调数据格式
                string Param = "&num=30&longlife=20&format=json";
                string Host = System.Configuration.ConfigurationManager.AppSettings["PROXYURL"] ?? "";
                string Url = string.Format("{0}{1}", Host, Param);
                string Result = Utils.HttpGet(Url);
                if (string.IsNullOrEmpty(Result.Trim()))
                    return null;
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<udv_HttpProxy>>(Result);
            }
            catch (Exception ex)
            {
                log.Write("获取HTTP代理地址错误：" + ex.Message, true);
                return null;
            }
        }
    }
}
