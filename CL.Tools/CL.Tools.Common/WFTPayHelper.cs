using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace CL.Tools.Common
{
    /// <summary>
    /// 威富通支付接口
    /// </summary>
    public class WFTPayHelper
    {
        /// <summary>
        /// 密钥
        /// </summary>
        private string key;

        /// <summary>
        /// 应答的参数
        /// </summary>
        protected Hashtable parameters;

        /// <summary>
        /// debug信息
        /// </summary>
        private string debugInfo;

        /// <summary>
        /// 原始内容
        /// </summary>
        protected string content;

        private string charset = "UTF-8";

        /// <summary>
        /// 获取服务器通知数据方式，进行参数获取
        /// </summary>
        public WFTPayHelper()
        {
            parameters = new Hashtable();
        }

        /// <summary>
        /// 获取返回内容
        /// </summary>
        /// <returns></returns>
        public string getContent()
        {
            return this.content;
        }
        /// <summary>
        /// 设置返回内容
        /// </summary>
        /// <param name="content">XML内容</param>
        public virtual void setContent(string content)
        {
            this.content = content;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);
            XmlNode root = xmlDoc.SelectSingleNode("xml");
            XmlNodeList xnl = root.ChildNodes;

            foreach (XmlNode xnf in xnl)
            {
                this.setParameter(xnf.Name, xnf.InnerText);
            }
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
        /// <summary>
        /// 是否威富通签名,规则是:按参数名称a-z排序,遇到空值的参数不参加签名。
        /// </summary>
        /// <returns></returns>
        public virtual Boolean isTenpaySign()
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
            string sign = GetMD5(sb.ToString(), getCharset()).ToLower();

            //debug信息
            this.setDebugInfo(sb.ToString() + " => sign:" + sign);
            return getParameter("sign").ToLower().Equals(sign);
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <returns></returns>
        public string isVerfiySign()
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
            string sign = GetMD5(sb.ToString(), getCharset()).ToLower();

            //debug信息
            this.setDebugInfo(sb.ToString() + " => sign:" + sign);
            return sign;
        }
        /// <summary>
        /// 获取大写的MD5签名结果
        /// </summary>
        /// <param name="encypStr">需要签名的串</param>
        /// <param name="charset">编码</param>
        /// <returns>返回大写的MD5签名结果</returns>
        public static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
                Console.WriteLine(ex);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, String> loadCfg()
        {
            Dictionary<String, String> cfg = new Dictionary<string, string>();
            XmlNode Node = Utils.QueryConfigNode("root/paywft");
            cfg.Add("req_url", Node.SelectSingleNode("requrl").InnerText);
            cfg.Add("key", Node.SelectSingleNode("mchkey").InnerText);
            cfg.Add("mch_id", Node.SelectSingleNode("mchid").InnerText);
            cfg.Add("version", Node.SelectSingleNode("version").InnerText);
            cfg.Add("notify_url", Node.SelectSingleNode("notifyurl").InnerText);
            return cfg;
        }
        /// <summary>
        /// 保存接口返回结果到文件中
        /// </summary>
        /// <param name="_param">接口结果</param>
        public static void writeFile(string title, Hashtable _param)
        {
            string resFilePath = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ApplicationBase)
                                + Path.DirectorySeparatorChar + "result.txt";
            if (!File.Exists(resFilePath))
            {
                using (StreamWriter sw = new StreamWriter(resFilePath))
                {
                    sw.WriteLine("=====================" + title + "=====================");
                    foreach (DictionaryEntry de in _param)
                    {
                        sw.WriteLine("key:" + de.Key.ToString() + " value:" + de.Value.ToString());
                    }
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(resFilePath))
                {
                    sw.WriteLine("=====================" + title + "=====================");
                    foreach (DictionaryEntry de in _param)
                    {
                        sw.WriteLine("key:" + de.Key.ToString() + " value:" + de.Value.ToString());
                    }
                }
            }
        }
        /// <summary>
        /// 获取返回的所有参数
        /// </summary>
        /// <returns></returns>
        public Hashtable getAllParameters()
        {
            return this.parameters;
        }
        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        public string getKey()
        { return key; }

        /// <summary>
        /// 设置密钥
        /// </summary>
        /// <param name="key">密钥</param>
        public void setKey(string key)
        { this.key = key; }
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
        /// 设置debug信息
        /// </summary>
        /// <param name="debugInfo"></param>
        protected void setDebugInfo(String debugInfo)
        { this.debugInfo = debugInfo; }
        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        protected virtual string getCharset()
        {
            return this.charset;
        }

        /// <summary>
        /// 生成16位订单号 by  hyf 2016年2月16日17:48:43
        /// </summary>
        /// <returns></returns>
        public static string Nmrandom()
        {
            string rm = "";
            Random ra = new Random();
            for (int i = 0; i < 16; i++)
            {
                rm += ra.Next(0, 9).ToString();
            }
            return rm;
        }
        /// <summary>
        /// 对字符串进行URL编码
        /// </summary>
        /// <param name="instr">URL字符串</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
        public static string UrlEncode(string instr, string charset)
        {
            //return instr;
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                string res;

                try
                {
                    res = HttpUtility.UrlEncode(instr, Encoding.GetEncoding(charset));

                }
                catch (Exception ex)
                {
                    res = HttpUtility.UrlEncode(instr, Encoding.GetEncoding("GB2312"));
                    Console.WriteLine(ex);
                }


                return res;
            }
        }


        /// <summary>
        /// 对字符串进行URL解码
        /// </summary>
        /// <param name="instr">编码的URL字符串</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
        public static string UrlDecode(string instr, string charset)
        {
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                string res;

                try
                {
                    res = HttpUtility.UrlDecode(instr, Encoding.GetEncoding(charset));

                }
                catch (Exception ex)
                {
                    res = HttpUtility.UrlDecode(instr, Encoding.GetEncoding("GB2312"));
                    Console.WriteLine(ex);
                }


                return res;

            }
        }
        /// <summary>
        /// 将Hashtable参数传为XML
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public static string toXml(Hashtable _params)
        {
            StringBuilder sb = new StringBuilder("<xml>");
            foreach (DictionaryEntry de in _params)
            {
                string key = de.Key.ToString();
                sb.Append("<").Append(key).Append("><![CDATA[").Append(de.Value.ToString()).Append("]]></").Append(key).Append(">");
            }

            return sb.Append("</xml>").ToString();
        }
        /// <summary>
        /// 生成32位随机数
        /// </summary>
        /// <returns></returns>
        public static string random()
        {
            char[] constant = {'0','1','2','3','4','5','6','7','8','9',
                               'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                               'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
            StringBuilder sb = new StringBuilder(32);
            Random rd = new Random();
            for (int i = 0; i < 32; i++)
            {
                sb.Append(constant[rd.Next(62)]);
            }
            return sb.ToString();
        }
    }
}
