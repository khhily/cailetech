using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.BLL.PayInterface;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using CL.Entity.Json.WebAPI;
using CL.Tools.Common;
using CL.WebAPI.Filters;
using CL.Json.Entity;
using WebAPI.Models;
using CL.Json.Entity.WebAPI;
using CL.Game.BLL.IM;
using System.Threading.Tasks;

namespace CL.WebAPI.Controllers
{
    public class UserController : BaseController
    {
        #region ---------- 注册 登录 重置密码 查询用户 查询用户余额 ---------- 
        /// <summary>
        /// 手机号注册
        /// </summary>
        /// <param name="Equipment">设备标识</param>
        /// <param name="SourceType">终端0PC 1Android 2IOS 3Ipad</param>
        /// <param name="Mobile">手机号码</param>
        /// <param name="Pwd">密码</param>
        /// <param name="VerifyCode">验证码</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("MobileRegister")]
        public async Task<string> MobileRegister()
        {
            LoginResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                byte SourceType = 0;
                string Equipment = dic["Equipment"].ToString();
                string Mobile = dic["Mobile"].ToString();
                string Pwd = dic["Pwd"].ToString();
                string VerifyCode = dic["VerifyCode"].ToString();
                byte.TryParse(dic["SourceType"].ToString(), out SourceType);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                {
                    result = new UsersBLL().MobileRegister(Equipment, SourceType, Mobile, Pwd, VerifyCode);
                    if (result.Code == (int)ResultCode.Success)
                    {
                        //开始内部IM通讯验证
                        bool Rec = new Communication().Login(result.Data.UserCode, result.Data.Token, true);
                        if (!Rec)
                            result = new LoginResult()
                            {
                                Code = (int)ResultCode.IMLoginFailure,
                                Msg = Common.GetDescription(ResultCode.IMLoginFailure),
                                Data = result.Data
                            };
                    }
                }
                else
                    result = new LoginResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("手机号注册错误:" + ex.Message, true);
                result = new LoginResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 集成登录
        /// </summary>
        /// <param name="Equipment">设备唯一标识</param>
        /// <param name="Code">集成登录编码</param>
        /// <param name="SourceType">终端 0PC 1Android 2IOS 3Ipad</param>
        /// <param name="LoginType">登录类型 1QQ 2微信</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("IntegrateSignIn")]
        public async Task<string> IntegrateSignIn()
        {
            LoginResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                byte SourceType = 0;
                byte LoginType = 0;
                string Equipment = dic["Equipment"].ToString();
                string Code = dic["Code"].ToString();
                byte.TryParse(dic["SourceType"].ToString(), out SourceType);
                byte.TryParse(dic["LoginType"].ToString(), out LoginType);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                {
                    result = new UsersBLL().IntegratedSignIn(Equipment, SourceType, Code, LoginType);
                    if (result.Code == (int)ResultCode.Success)
                    {
                        //开始内部IM通讯验证
                        bool Rec = new Communication().Login(result.Data.UserCode, result.Data.Token, true);
                        if (!Rec)
                            result = new LoginResult()
                            {
                                Code = (int)ResultCode.IMLoginFailure,
                                Msg = Common.GetDescription(ResultCode.IMLoginFailure),
                                Data = result.Data
                            };
                    }
                }
                else
                    result = new LoginResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("集成登录错误:" + ex.Message, true);
                result = new LoginResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 手机号码登录
        /// </summary>
        /// <param name="Equipment">设备唯一标识</param>
        /// <param name="SourceType"></param>
        /// <param name="Mobile"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("MobileSignIn")]
        public async Task<string> MobileSignIn()
        {
            LoginResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                byte SourceType = 0;
                string Equipment = dic["Equipment"].ToString();
                string Pwd = dic["Pwd"].ToString();
                string Mobile = dic["Mobile"].ToString();
                byte.TryParse(dic["SourceType"].ToString(), out SourceType);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                {
                    result = new UsersBLL().MobileSignIn(Equipment, SourceType, Mobile, Pwd);
                    if (result.Code == (int)ResultCode.Success)
                    {
                        //开始内部IM通讯验证
                        bool Rec = new Communication().Login(result.Data.UserCode, result.Data.Token, true);
                        if (!Rec)
                            result = new LoginResult()
                            {
                                Code = (int)ResultCode.IMLoginFailure,
                                Msg = Common.GetDescription(ResultCode.IMLoginFailure),
                                Data = result.Data
                            };
                    }
                }
                else
                    result = new LoginResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("手机号码登陆错误:" + ex.Message, true);
                result = new LoginResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 重置登录密码(未登录)
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="Mobile">手机号码</param>
        /// <param name="Pwd">登录密码</param>
        /// <param name="VerifyCode">验证码</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("ResetSignInPwd")]
        public async Task<string> ResetSignInPwd()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                string Mobile = dic["Mobile"].ToString();
                string Pwd = dic["Pwd"].ToString();
                string VerifyCode = dic["VerifyCode"].ToString();
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersBLL().ResetSignInPwd(Mobile, Pwd, VerifyCode);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("重置登录密码错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 类型 1登录密码 2支付密码
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        /// <param name="PwdType"></param>
        /// <param name="Pwd"></param>
        /// <param name="VerifyCode"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("ResetPwd")]
        public async Task<string> ResetPwd()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                byte PwdType = 0;
                string Pwd = dic["Pwd"].ToString();
                string VerifyCode = dic["VerifyCode"].ToString();
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                byte.TryParse(dic["PwdType"].ToString(), out PwdType);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersBLL().ResetPwd(UserCode, PwdType, Pwd, VerifyCode);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("找回密码错误:" + ex.Message);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编码</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("QueryUserInfo")]
        public async Task<string> QueryUserInfo()
        {
            LoginResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                string Token = dic["Token"].ToString();
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersBLL().QueryUserInfo(Token, UserCode);
                else
                    result = new LoginResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("查询用户信息错误:" + ex.Message, true);
                result = new LoginResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }
        /// <summary>
        /// 查询用户余额
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("QueryUserBalance")]
        public async Task<string> QueryUserBalance()
        {
            UserBalanceResult result = null;
            try
            {
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                {
                    var dic = DataSecret.DicPostPara;
                    string Token = dic["Token"].ToString();
                    long UserCode = 0;
                    long.TryParse(dic["UserCode"].ToString(), out UserCode);
                    result = new UsersBLL().QueryUserBalance(Token, UserCode);
                }
                else
                    result = new UserBalanceResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("查询用户余额错误[QueryUserBalance]：" + ex.Message, true);
                result = new UserBalanceResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }
        /// <summary>
        /// 保存推送标识
        /// </summary>
        /// <returns></returns>
        [HttpPut, DataLog, ErrorLog, VerifySecret, ActionName("PushIdentify")]
        public async Task<string> PushIdentify()
        {
            JsonResult result = null;
            try
            {
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                {
                    var dic = DataSecret.DicPostPara;
                    string Token = dic["Token"].ToString();
                    long UserCode = 0;
                    long.TryParse(dic["UserCode"].ToString(), out UserCode);
                    string PushIdentify = dic["PushIdentify"].ToString();
                    result = new UsersPushBLL().SavePushIdentify(Token, UserCode, PushIdentify);
                }
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("保存推送标识[PushIdentify]：" + ex.Message, true);
                result = new UserBalanceResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="Equipment"></param>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        /// <param name="UnixTime"></param>
        /// <param name="Digest"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("OutLogin")]
        public async Task<string> OutLogin()
        {
            OutLoginResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                string Equipment = dic["Equipment"].ToString();
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                {
                    result = new UsersBLL().OutLogin(Equipment, UserCode);
                    if (result.Code == (int)ResultCode.Success)
                    {
                        //开始内部IM通讯验证
                        bool Rec = new Communication().Login(-1, result.Token, false);
                        if (!Rec)
                            result = new OutLoginResult()
                            {
                                Code = (int)ResultCode.IMLoginFailure,
                                Msg = Common.GetDescription(ResultCode.IMLoginFailure),
                                Token = result.Token
                            };
                    }
                }
                else
                    result = new OutLoginResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("退出登录错误:" + ex.Message, true);
                result = new OutLoginResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }
        /// <summary>
        /// 兑换彩豆
        /// </summary>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("ExchangeBean")]
        public async Task<string> ExchangeBean()
        {
            UserBalanceResult result = null;
            try
            {
                long UserCode = 0;
                long Amount = 0;
                long Gold = 0;
                var dic = DataSecret.DicPostPara;
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                long.TryParse(dic["Amount"].ToString(), out Amount);
                long.TryParse(dic["Gold"].ToString(), out Gold);
                result = new UsersBLL().ExchangeBean(UserCode, Amount, Gold);
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("兑换彩豆错误:" + ex.Message, true);
                result = new UserBalanceResult()
                {
                    Code = (int)ResultCode.RequestHasFailed,
                    Msg = Common.GetDescription(ResultCode.RequestHasFailed)
                };
            }
            return await base.TResultAsync(result);
        }
        #endregion

        #region ---------- 更换手机 更换昵称 ----------

        /// <summary>
        /// 更换手机号码
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编号</param>
        /// <param name="Mobile">手机号码</param>
        /// <param name="VerifyCode">验证码</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("ChangeMobile")]
        public async Task<string> ChangeMobile()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                string Mobile = dic["Mobile"].ToString();
                string VerifyCode = dic["VerifyCode"].ToString();
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersBLL().ChangeMobile(UserCode, Mobile, VerifyCode);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("更换手机号码错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 更换昵称
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        /// <param name="Nick"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("ChangeNick")]
        public async Task<string> ChangeNick()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                string Nick = Utils.UrlDecode(dic["Nick"].ToString());
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersBLL().ChangeNick(UserCode, Nick);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("更换昵称错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }
        #endregion

        #region  ---------- 身份认证 银行卡 提现 充值 ---------- 
        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编号</param>
        /// <param name="FullName">真实姓名</param>
        /// <param name="IDCardNo">身份证号码</param>
        /// <param name="VerifyCode">验证码</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("IDVerification")]
        public async Task<string> IDVerification()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                string Token = dic["Token"].ToString();
                string Mobile = dic["Mobile"].ToString();
                string IDCardNo = dic["IDCardNo"].ToString();
                string FullName = Utils.UrlDecode(dic["FullName"].ToString());
                string VerifyCode = dic["VerifyCode"].ToString();
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersExtendBLL().IDVerification(Token, UserCode, FullName, IDCardNo, VerifyCode, Mobile);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("身份证认证错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 绑定银行卡
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编号</param>
        /// <param name="BankName">银行名称</param>
        /// <param name="CardNum">银行卡号</param>
        /// <param name="Area">开卡地址</param>
        /// <param name="Mobile">银行预留号码</param>
        /// <param name="VerifyCode">验证码</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("BindBankCard")]
        public async Task<string> BindBankCard()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                string BankName = Utils.UrlDecode(dic["BankName"].ToString());
                string Area = Utils.UrlDecode(dic["Area"].ToString());
                string CardNum = dic["CardNum"].ToString();
                string Mobile = dic["Mobile"].ToString();
                string VerifyCode = dic["VerifyCode"].ToString();
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersBanksBLL().BindBankCard(UserCode, BankName, CardNum, Area, Mobile, VerifyCode);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("绑定银行卡错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 解绑银行卡
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编号</param>
        /// <param name="BankCode">银行卡编号</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("UnBindBankCard")]
        public async Task<string> UnBindBankCard()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                long BankCode = 0;
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                long.TryParse(dic["BankCode"].ToString(), out BankCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersBanksBLL().UnBindBankCard(UserCode, BankCode);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("解绑错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 银行卡列表
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编号</param>
        /// <param name="PageNumber">页码</param>
        /// <param name="RowsPerPage">每页多少行</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("BankCards")]
        public async Task<string> BankCards()
        {
            BankCardsResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                int PageNumber = 1;
                int RowsPerPage = 1;
                long.TryParse(dic["UserCode"] != null ? dic["UserCode"].ToString() : "", out UserCode);
                int.TryParse(dic["PageNumber"] != null ? dic["PageNumber"].ToString() : "", out PageNumber);
                int.TryParse(dic["RowsPerPage"] != null ? dic["RowsPerPage"].ToString() : "", out RowsPerPage);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersBanksBLL().BankCards(UserCode, PageNumber, RowsPerPage);
                else
                    result = new BankCardsResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("银行卡列表错误:" + ex.Message, true);
                result = new BankCardsResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编号</param>
        /// <param name="BankCode">银行编号</param>
        /// <param name="Amount">提现金额</param>
        /// <param name="PayPwd">支付密码</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("WithdrawDeposit")]
        public async Task<string> WithdrawDeposit()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                long BankCode = 0;
                long Amount = 0;
                string PayPwd = dic["PayPwd"].ToString();
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                long.TryParse(dic["BankCode"].ToString(), out BankCode);
                long.TryParse(dic["Amount"].ToString(), out Amount);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersWithdrawBLL().WithdrawDeposit(UserCode, BankCode, Amount, PayPwd);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("提现错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 查询当日可提现数据
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("WithdrawCount")]
        public async Task<string> WithdrawCount()
        {
            WithdrawCountResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersWithdrawBLL().QueryWithdrawCount(UserCode);
                else
                    result = new WithdrawCountResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("查询当日可提现数据错误:" + ex.Message, true);
                result = new WithdrawCountResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }
        /// <summary>
        /// 预支付 威富通(第三方支付)
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编码</param>
        /// <param name="Amount">支付金额</param>
        /// <param name="TbBody">商品描述</param>
        /// <param name="TbAttach">附加信息</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("PreparePaymentWFT")]
        public async Task<string> PreparePaymentWFT()
        {
            PreparePaymentResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                long Amount = 0;
                string TbBody = Utils.UrlDecode(dic["TbBody"].ToString());
                string TbAttach = Utils.UrlDecode(dic["TbAttach"].ToString());
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                long.TryParse(dic["Amount"].ToString(), out Amount);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new WFTPay().WFTPrepayment(UserCode, Amount, TbBody, TbAttach);
                else
                    result = new PreparePaymentResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("威付通予支付错误:" + ex.Message, true);
                result = new PreparePaymentResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);

        }
        /// <summary>
        /// 快捷支付验证
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编号</param>
        /// <param name="OrderNo">订单号</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("VerifyPaymentShortcut")]
        public async Task<string> VerifyPaymentShortcut()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                string OrderNo = dic["OrderNo"].ToString();
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersPayDetailBLL().VerifyPaymentShortcut(UserCode, OrderNo);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("快捷支付验证错误:" + ex.Message, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 用户交易记录
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="UserCode">用户编号</param>
        /// <param name="TradingType">交易类型 1充值 2提现</param>
        /// <param name="PageNumber">页码</param>
        /// <param name="RowsPerPage">页行</param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("UserTradingRecord")]
        public async Task<string> UserTradingRecord()
        {
            TradingResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                long UserCode = 0;
                int PageNumber = 1;
                int RowsPerPage = 0;
                long.TryParse(dic["UserCode"].ToString(), out UserCode);
                int.TryParse(dic["PageNumber"].ToString(), out PageNumber);
                int.TryParse(dic["RowsPerPage"].ToString(), out RowsPerPage);
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersRecordBLL().UserTradingRecord(UserCode, PageNumber, RowsPerPage);
                else
                    result = new TradingResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("用户交易记录错误:" + ex.Message, true);
                result = new TradingResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return await base.TResultAsync(result);
        }

        /// <summary>
        /// 注册验证
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        [HttpPost, DataLog, ErrorLog, VerifySecret, ActionName("VerifyMobile")]
        public async Task<string> VerifyMobile()
        {
            JsonResult result = null;
            try
            {
                var dic = DataSecret.DicPostPara;
                string Mobile = dic["Mobile"].ToString();
                if (DataSecret.VerifyRec == (int)ResultCode.Success)
                    result = new UsersBLL().VerifyMobile(Mobile);
                else
                    result = new JsonResult()
                    {
                        Code = DataSecret.VerifyRec,
                        Msg = Common.GetDescription((ResultCode)DataSecret.VerifyRec)
                    };
            }
            catch (Exception ex)
            {
                new Log("UserController").Write("注册验证错误:" + ex.Message, true);
                result = new JsonResult()
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
