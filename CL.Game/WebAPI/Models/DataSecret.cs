using CL.Enum.Common;
using CL.Game.BLL;
using CL.Redis.BLL;
using CL.Tools.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebAPI.Models
{
    public class DataSecret
    {
        public static int VerifyRec = -1;

        Log log = new Log("DataSecret");

        public static Dictionary<string, object> DicPostPara = null;
        /// <summary>
        /// 客户端服务端约定密钥
        /// </summary>
        protected string SecretKey = System.Configuration.ConfigurationManager.AppSettings["SECRET"] ?? "";
        public DataSecret()
        {

        }

        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="unixTime">客户端时间戳</param>
        /// <param name="parameters">参数集</param>
        /// <returns></returns>
        public void VerifySecret(long unixTime, string Secret, Dictionary<string, object> parameters, string methed)
        {
            try
            {
                StringBuilder sb = new StringBuilder("?");
                ArrayList akeys = new ArrayList(parameters.Keys);
                DateTime dt = DateTime.Now;
                var b = this.GetTimeStamp(unixTime, ref dt);
                if (!b)
                    VerifyRec = (int)ResultCode.TimeStampError;  //时间戳验证失败
                else
                {
                    foreach (string k in akeys)
                        sb.AppendFormat("{0}={1}&", k, HttpUtility.UrlDecode((string)parameters[k], Encoding.GetEncoding("utf-8")));
                    sb.AppendFormat("TimeStamp={0}&Secret={1}", unixTime, SecretKey);
                    string ServerSecret = Utils.MD5(sb.ToString().Trim(), true);
                    if (ServerSecret != Secret)
                        VerifyRec = (int)ResultCode.SecretError; //数据签名不一致 签名失败
                    else
                        VerifyRec = (int)ResultCode.Success;
                }
                if (methed == "POST" || methed == "PUT")
                {
                    DicPostPara = parameters;
                    log.Write("POST参数:" + Newtonsoft.Json.JsonConvert.SerializeObject(parameters), true);
                }
            }
            catch (Exception ex)
            {
                log.Write("签名验证错误：" + ex.Message, true);
                VerifyRec = (int)ResultCode.Error;
            }
        }

        /// <summary>
        /// 参数验证
        /// </summary>
        /// <param name="parameters"></param>
        public void VerifyParameters(Dictionary<string, object> dic)
        {
            try
            {
                //令牌
                if (dic.Keys.Contains("Token"))
                {
                    var entity = new BaseRedis().VerifyTokenRedis(Convert.ToString(dic["Token"]));
                    if (entity == null)
                    {
                        VerifyRec = (int)ResultCode.TokenError;
                        return;
                    }
                }
                //令牌and用户编号
                if (dic.Keys.Contains("Token") && dic.Keys.Contains("UserCode"))
                {
                    var entity = new BaseRedis().VerifyTokenRedis(Convert.ToString(dic["Token"]));
                    if (entity == null || entity.UserCode != Convert.ToInt64(dic["UserCode"]))
                    {
                        VerifyRec = (int)ResultCode.TokenError;
                        return;
                    }
                }
                //手机号码
                if (dic.Keys.Contains("Mobile") && !DataCheck.IsMobile(Convert.ToString(dic["Mobile"])))
                {
                    VerifyRec = (int)ResultCode.MobileFormatError;
                    return;
                }
                //来源类型
                if (dic.Keys.Contains("SourceType") && Convert.ToInt16(dic["SourceType"]) < 0)
                {
                    VerifyRec = (int)ResultCode.TerminalNull;
                    return;
                }
                //找回密码验证手机号是否存在
                if (dic.Keys.Contains("VerifyType") && dic.Keys.Contains("Mobile") && Convert.ToInt16(dic["VerifyType"]) == 1)
                {
                    var UserEntity = new UsersBLL().QueryEntityByUserMobile(Convert.ToString(dic["Mobile"]));
                    if (UserEntity == null)
                    {
                        VerifyRec = (int)ResultCode.NoMobile;
                        return;
                    }
                }
                //订单金额
                //if (dic.Keys.Contains("Amount") && Convert.ToInt64(dic["Amount"]) == 0)
                //{
                //    VerifyRec = (int)ResultCode.BettingMoneyOrderMoneyNoEqual;
                //    return;
                //}
                //真实姓名
                if (dic.Keys.Contains("FullName") && !DataCheck.WhetherChinese(Utils.UrlDecode(Convert.ToString(dic["FullName"]))))
                {
                    VerifyRec = (int)ResultCode.FullNameFormatError;
                    return;
                }
                //身份证号码
                if (dic.Keys.Contains("IDCardNo") && !DataCheck.IsIDCard(Convert.ToString(dic["IDCardNo"])))
                {
                    VerifyRec = (int)ResultCode.IdentifyFormatError;
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Write("参数验证错误：" + ex.Message, true);
                VerifyRec = (int)ResultCode.SystemBusy;
            }
        }

        /// <summary>
        /// 验证时间戳
        /// 五分钟有效
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected bool GetTimeStamp(long unixTime, ref DateTime TimeStamp)
        {
            try
            {
                DateTime StartTime = DateTime.Now.AddMinutes(-5);
                DateTime EndTime = DateTime.Now.AddMinutes(5);
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0)); // 当地时区
                TimeStamp = startTime.AddMilliseconds(unixTime);
                if (EndTime < TimeStamp || StartTime > TimeStamp)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                log.Write("时间戳验证错误：" + ex.Message, true);
                return false;
            }
        }
    }
}