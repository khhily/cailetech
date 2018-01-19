using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace CL.Tools.Common
{
    public static class SMS
    {
        public static string SendModel(string tel, string para1, string para2, int verify)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            sb.AppendFormat("applyid={0}", System.Configuration.ConfigurationManager.AppSettings["SMSAPPLYID"] ?? "");
            sb.AppendFormat("&secret={0}", System.Configuration.ConfigurationManager.AppSettings["SMSSECRET"] ?? "");
            sb.AppendFormat("&tel={0}", tel);
            sb.AppendFormat("&para1={0}", para1);
            sb.AppendFormat("&para2={0}", para2);
            sb.AppendFormat("&verify={0}", verify);
            sb.AppendFormat("&sign={0}", Encrypt(sb.ToString()));
            string url = (System.Configuration.ConfigurationManager.AppSettings["SMSURL"] ?? "") + "&" + sb.ToString();

            return getContent(url);
        }
        public static string SendModel(string tel, string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            sb.AppendFormat("applyid={0}", System.Configuration.ConfigurationManager.AppSettings["SMSAPPLYID"] ?? "");
            sb.AppendFormat("&secret={0}", System.Configuration.ConfigurationManager.AppSettings["SMSSECRET"] ?? "");
            sb.AppendFormat("&tel={0}", tel);
            sb.AppendFormat("&msg={0}", msg);
            sb.AppendFormat("&sign={0}", Encrypt(sb.ToString()));
            string url = (System.Configuration.ConfigurationManager.AppSettings["SMSURL"] ?? "") + "&" + sb.ToString();

            return getContent(url);
        }

        private static string Encrypt(string password, int? length = null)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            string str = BitConverter.ToString(provider.ComputeHash(Encoding.UTF8.GetBytes(password)));
            if (length != null && length == 16)
            {
                str = BitConverter.ToString(provider.ComputeHash(Encoding.UTF8.GetBytes(password)), 4, 8);
            }
            provider.Clear();
            return str.Replace("-", null);
        }
        private static string getContent(string Url)
        {
            string strResult = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                //声明一个HttpWebRequest请求    
                request.Timeout = 60000;
                //设置连接超时时间    
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                strResult = streamReader.ReadToEnd();
                streamReader.Close();
            }
            catch (Exception ex)
            {
                new Log("SMS").Write(ex.Message + "\r\n  \r  \n <br />" + ex.StackTrace, true);
            }
            return strResult;
        }
        #region 短信类型
        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="smstype"></param>
        /// <param name="verfiycode"></param>
        /// <returns></returns>
        public static string SendSms(string tel, byte smstype, string verfiycode)
        {
            string msg = SetSmsType(smstype, verfiycode);
            return SendModel(tel, msg);
        }
        /// <summary>
        /// 短信类型：0注册 1找回密码 3修改登录密码 5修改支付密码 7绑定修改手机号码 9绑定银行卡 11身份验证
        /// </summary>
        /// <param name="SmsType"></param>
        /// <param name="VerfiyCode"></param>
        /// <returns></returns>
        public static string SetSmsType(byte SmsType, string VerfiyCode)
        {
            string res = string.Empty;
            switch (SmsType)
            {
                case 0: //0注册
                    res = string.Format("注册验证码为{0},请在十分钟内使用。", VerfiyCode);
                    break;
                case 1: //1找回密码
                    res = string.Format("找回密码验证码为{0},请在十分钟内使用。", VerfiyCode);
                    break;
                case 3: //3修改登录密码
                    res = string.Format("修改密码验证码为{0},请在十分钟内使用。", VerfiyCode);
                    break;
                case 5: //5修改支付密码
                    res = string.Format("修改支付密码验证码为{0},请在十分钟内使用。", VerfiyCode);
                    break;
                case 7: //7绑定修改手机号码
                    res = string.Format("修改手机验证码为{0},请在十分钟内使用。", VerfiyCode);
                    break;
                case 9: //9绑定银行卡
                    res = string.Format("绑定银行卡验证码为{0},请在十分钟内使用。", VerfiyCode);
                    break;
                case 11: //11身份验证
                    res = string.Format("身份验证验证码为{0},请在十分钟内使用。", VerfiyCode);
                    break;
            }
            return res;
        }
        #endregion
    }
}
