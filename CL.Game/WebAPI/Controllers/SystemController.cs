using CL.Enum.Common;
using CL.Game.BLL;
using CL.Json.Entity;
using CL.Json.Entity.WebAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using CL.WebAPI.Filters;
using CL.Tools.Common;
using CL.Game.BLL.IM;
using System.Threading.Tasks;
using CL.Redis.BLL;

namespace CL.WebAPI.Controllers
{
    public class SystemController : BaseController
    {
        #region ---------- 启动 初始化 验证码 服务器时间 ----------

        /// <summary>
        /// 应用启动
        /// </summary>
        /// <param name="Equipment">设备唯一标识</param>
        /// <param name="Version">版本号</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("ApplyStart")]
        public async Task<string> ApplyStart()
        {
            ApplyStartResult result = null;
            try
            {
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                {
                    var dic = DataSecret.DicPostPara;
                    string Equipment = dic["Equipment"].ToString();
                    string Version = dic["Version"].ToString();
                    result = ApplyHelp.ApplyStartConfig(Equipment, Version);
                }
                else
                    result = new ApplyStartResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("SystemController").Write("应用启动错误:" + ex.Message, true);
                result = new ApplyStartResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 初始化应用
        /// </summary>
        /// <param name="Equipment">设备唯一标识</param>
        /// <param name="TimeStamp">时间戳</param>
        /// <param name="Token">令牌</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("ApplyInitiate")]
        public async Task<string> ApplyInitiate()
        {
            ApplyInitiateResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                string Equipment = dic["Equipment"].ToString();
                string Token = dic["Token"].ToString();
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                {
                    result = new UsersBLL().SSOSignIn(Equipment, Token);
                    if (result.Code == (int)ResultCode.Success)
                    {
                        int RecCode = (int)ResultCode.Success;
                        //开始内部IM通讯验证
                        if (result.LoginData != null)
                        {
                            if (!new Communication().Login(result.LoginData.UserCode, result.LoginData.Token, true))
                                RecCode = (int)ResultCode.IMLoginFailure;
                        }
                        else
                        {
                            if (!new Communication().Login(-1, result.Token, false))
                                RecCode = (int)ResultCode.IMLoginFailure;
                        }
                        result = new ApplyInitiateResult()
                        {
                            Code = RecCode,
                            Msg = Common.GetDescription((ResultCode)RecCode),
                            IsVerifyToken = result.IsVerifyToken,
                            Token = result.Token,
                            LoginData = result.LoginData,
                            LotteryData = result.LotteryData,
                            SystemTime = result.SystemTime,
                            SystemTotalWin = result.SystemTotalWin,
                            ActivityData = result.ActivityData
                        };

                    }
                }
                else
                    result = new ApplyInitiateResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("SystemController").Write("初始化应用错误:" + ex.Message, true);
                result = new ApplyInitiateResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="Equipment">设备唯一标识</param>
        /// <param name="Token">令牌</param>
        /// <param name="Mobile">手机号码</param>
        /// <param name="VerifyType">
        /// 短信类型：0注册 1找回密码 3修改登录密码 5修改支付密码 7绑定修改手机号码 9绑定银行卡 11身份验证</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("ApplySendVerifyCode")]
        public async Task<string> ApplySendVerifyCode()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                byte VerifyType = 0;
                string Equipment = dic["Equipment"].ToString();
                string Token = dic["Token"].ToString();
                string Mobile = dic["Mobile"].ToString();
                byte.TryParse(dic["VerifyType"].ToString(), out VerifyType);

                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersBLL().SendVerifyCode(Equipment, Token, Mobile, VerifyType);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("SystemController").Write("发送验证码错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 验证码验证
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="Mobile"></param>
        /// <param name="VerifyCode"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("VerifyCode")]
        public async Task<string> VerifyCode()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                byte VerifyType = 0;
                string Mobile = dic["Mobile"].ToString();
                string VerifyCode = dic["VerifyCode"].ToString();
                byte.TryParse(dic["VerifyType"].ToString(), out VerifyType);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersBLL().VerifyCode(Mobile, VerifyCode, VerifyType);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("SystemController").Write("验证码校验错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 服务器时间
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("Get")]
        public async Task<string> Get()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0);
            var result = new
            {
                Code = (int)ResultCode.Success,
                Msg = Common.GetDescription(ResultCode.Success),
                ServerTime = Convert.ToInt64(ts.TotalMilliseconds).ToString()
            };
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 校验身份信息 
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("VerifyAuth")]
        public async Task<string> VerifyAuth()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                string Token = dic["Token"].ToString();
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersBLL().VerifyAuth(Token);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("SystemController").Write("校验身份信息错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 获取游客令牌
        /// </summary>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("SightseerToken")]
        public async Task<string> SightseerToken()
        {
            try
            {
                ResultCode RecCode = ResultCode.Success;
                var dic = DataSecret.DicPostPara;
                string Equipment = dic["Equipment"].ToString();
                var Entity = new BaseRedis().VerifyTokenRedis(string.Empty, Equipment);
                if (Entity != null)
                {
                    //开始内部IM通讯验证
                    bool Rec = new Communication().Login(-1, Entity.Token, false);
                    if (!Rec)
                        RecCode = ResultCode.IMLoginFailure;
                    else
                        RecCode = ResultCode.Success;
                    var result = new
                    {
                        Code = (int)RecCode,
                        Msg = Common.GetDescription(RecCode),
                        Token = Entity.Token
                    };
                    return await base.TResultAsync(result);
                }
                else
                {
                    var result = new
                    {
                        Code = (int)ResultCode.NullData,
                        Msg = Common.GetDescription(ResultCode.NullData),
                    };
                    return await base.TResultAsync(result);
                }
            }
            catch (Exception ex)
            {
                new Log("SystemController").Write("获取游客令牌错误:" + ex.Message, true);
                var result = new
                {
                    Code = (int)ResultCode.NullData,
                    Msg = Common.GetDescription(ResultCode.NullData)
                };
                return await base.TResultAsync(result);
            }
        }
        #endregion

        #region 开奖大厅 
        /// <summary>
        /// 开奖大厅
        /// </summary>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("OpenAwardHome")]
        public async Task<string> OpenAwardHome()
        {
            OfLotteryResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                string Token = dic["Token"].ToString();
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new IsusesBLL().OpenAwardHall(Token);
                else
                    result = new OfLotteryResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("SystemController").Write("开奖大厅错误:" + ex.Message, true);
                result = new OfLotteryResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }
        #endregion
    }
}
