using CL.Enum.Common;
using CL.Json.Entity.WebAPI;
using CL.Tools.RedisBase;
using CL.View.Entity.Redis;
using System;
using System.Collections.Generic;
using CL.View.Entity.SystemInfo;
using CL.View.Entity.Game;
using CL.View.Entity.Other;

namespace CL.Redis.BLL
{
    public class SystemRedis
    {
        #region ---------- 系统累计中奖 大厅彩种 底部模板 验证码 ----------

        /// <summary>
        /// 读取Redis数据
        /// </summary>
        /// <returns></returns>
        public List<int> ApplyShowModel()
        {
            string Key = string.Format("Apply:{0}", RedisKeysEnum.ApplyShowModel.ToString());
            string showModel = RedisHelper.Get_String<string>(Key);
            List<int> ls = new List<int>();
            if (!string.IsNullOrEmpty(showModel))
            {
                foreach (var item in showModel.Split(','))
                    ls.Add(Convert.ToInt32(item));
            }
            return ls;
        }

        /// <summary>
        /// 数据插入Redis
        /// </summary>
        /// <param name="showModel"></param>
        public void ApplyShowModel(string showModel)
        {
            string Key = string.Format("Apply:{0}", RedisKeysEnum.ApplyShowModel.ToString());
            RedisHelper.Set_String(Key, showModel, 24, 0, 0);
        }

        /// <summary>
        /// 读取Redis数据
        /// </summary>
        /// <returns></returns>
        public long ApplySystemTotalWinRedis()
        {
            string Key = string.Format("Apply:{0}", RedisKeysEnum.HallTotalWin.ToString());
            string showModel = RedisHelper.Get_String<string>(Key);
            return Convert.ToInt64(showModel);
        }

        /// <summary>
        /// 数据插入Redis
        /// </summary>
        /// <param name="SysVal"></param>
        /// <param name="WinVal"></param>
        /// <returns></returns>
        public void ApplySystemTotalWinRedis(long SysVal, long WinVal)
        {
            string Key = string.Format("Apply:{0}", RedisKeysEnum.HallTotalWin.ToString());
            string showModel = Convert.ToString((SysVal + WinVal));
            RedisHelper.Set_String(Key, showModel, 1, 0, 0);
        }

        /// <summary>
        /// 读取Redis数据
        /// </summary>
        /// <returns></returns>
        public List<LotteryData> ApplyLotteryDataRedis()
        {
            string Key = string.Format("Apply:{0}", RedisKeysEnum.HallLottery.ToString());
            return RedisHelper.Get_Entity<List<LotteryData>>(Key);
        }
        /// <summary>
        /// 读取Redis数据
        /// </summary>
        /// <returns></returns>
        public bool RemoveApplyLotteryDataRedis()
        {
            string Key = string.Format("Apply:{0}", RedisKeysEnum.HallLottery.ToString());
            return RedisHelper.Remove_Entity(Key);
        }
        /// <summary>
        /// 数据插入Redis
        /// </summary>
        /// <param name="ls"></param>
        public void ApplyLotteryDataRedis(List<LotteryData> Entitys)
        {
            string Key = string.Format("Apply:{0}", RedisKeysEnum.HallLottery.ToString());
            RedisHelper.Set_Entity(Key, Entitys);
            DateTime dt = Convert.ToDateTime(string.Format("{0} 00:00:00", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
            RedisHelper.SetExpire_List(Key, dt);
        }

        /// <summary>
        /// 存储验证码
        /// 短信类型：0注册 1找回密码 3修改登录密码 5修改支付密码 7绑定修改手机号码 9绑定银行卡 11身份验证
        /// </summary>
        /// <param name="Mobile"></param>
        /// <param name="Code"></param>
        /// <param name="VerifyType"></param>
        public void SendVerifyCodeRedis(string Mobile, string Code, byte VerifyType)
        {
            string Key = string.Format("{0}:{1}:{2}", RedisKeysEnum.VerifyCode.ToString(), VerifyType, Mobile);
            udv_VerifyCode Entity = new udv_VerifyCode()
            {
                Code = Code,
                Mobile = Mobile,
                VerifyType = VerifyType
            };
            RedisHelper.Set_Entity<udv_VerifyCode>(Key, Entity, 0, 10, 0);
        }

        /// <summary>
        /// 验证码验证
        /// 短信类型：0注册 1找回密码 3修改登录密码 5修改支付密码 7绑定修改手机号码 9绑定银行卡 11身份验证
        /// </summary>
        /// <param name="Mobile"></param>
        /// <param name="Code"></param>
        /// <param name="VerifyType"></param>
        /// <returns></returns>
        public int VerifyCodeRedis(string Mobile, string Code, byte VerifyType)
        {
            string Key = string.Format("{0}:{1}:{2}", RedisKeysEnum.VerifyCode.ToString(), VerifyType, Mobile);
            var entity = RedisHelper.Get_Entity<udv_VerifyCode>(Key);
            if (entity == null)
                return 2; //验证码无效
            if (entity.Code.ToLower().Trim() != Code.ToLower().Trim())
                return 1; //验证码错误
            return 0; //验证码正常
        }
        #endregion

        #region ---------- 单点登录 手机登陆 集成登陆 ----------
        /// <summary>
        /// 数据插入Redis
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="Entity"></param>
        /// <param name="ExtendEntity"></param>
        /// <returns></returns>
        public void SignInSessionRedis(udv_MobileSignIn Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, Entity.UserCode);
            RedisHelper.Set_Entity<udv_MobileSignIn>(Key, Entity);
            if (!string.IsNullOrEmpty(Entity.Mobie))
            {
                Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, Entity.Mobie);
                RedisHelper.Set_Entity<udv_MobileSignIn>(Key, Entity);
            }
            if (!string.IsNullOrEmpty(Entity.WechatOpenID))
            {
                Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, Entity.WechatOpenID);
                RedisHelper.Set_Entity<udv_MobileSignIn>(Key, Entity);
            }
            if (!string.IsNullOrEmpty(Entity.QQOpenID))
            {
                Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, Entity.QQOpenID);
                RedisHelper.Set_Entity<udv_MobileSignIn>(Key, Entity);
            }
            if (!string.IsNullOrEmpty(Entity.AliPayOpenID))
            {
                Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, Entity.AliPayOpenID);
                RedisHelper.Set_Entity<udv_MobileSignIn>(Key, Entity);
            }
        }
        /// <summary>
        /// 更新Redis
        /// 待优化
        /// </summary>
        /// <param name="Entity"></param>
        public void ModifySignInSessionRedis(udv_MobileSignIn Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, Entity.UserCode);
            RedisHelper.Set_Entity<udv_MobileSignIn>(Key, Entity);
            if (!string.IsNullOrEmpty(Entity.Mobie))
            {
                Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, Entity.Mobie);
                RedisHelper.Set_Entity<udv_MobileSignIn>(Key, Entity);
            }
            if (!string.IsNullOrEmpty(Entity.WechatOpenID))
            {
                Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, Entity.WechatOpenID);
                RedisHelper.Set_Entity<udv_MobileSignIn>(Key, Entity);
            }
            if (!string.IsNullOrEmpty(Entity.QQOpenID))
            {
                Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, Entity.QQOpenID);
                RedisHelper.Set_Entity<udv_MobileSignIn>(Key, Entity);
            }
            if (!string.IsNullOrEmpty(Entity.AliPayOpenID))
            {
                Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, Entity.AliPayOpenID);
                RedisHelper.Set_Entity<udv_MobileSignIn>(Key, Entity);
            }
        }

        /// <summary>
        /// 读取Redis数据
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public udv_MobileSignIn SignInByUserCodeRedis(long UserCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, UserCode);
            return RedisHelper.Get_Entity<udv_MobileSignIn>(Key);
        }

        /// <summary>
        /// 读取Redis数据
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public udv_MobileSignIn SignInByMobileRedis(string Mobile)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, Mobile);
            return RedisHelper.Get_Entity<udv_MobileSignIn>(Key);
        }

        /// <summary>
        /// 读取Redis数据
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public udv_MobileSignIn SignInByWechatTokenRedis(string WechatOpenID)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, WechatOpenID);
            return RedisHelper.Get_Entity<udv_MobileSignIn>(Key);
        }
        /// <summary>
        /// 读取Redis数据
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public bool RemoveWechatTokenRedis(string WechatOpenID)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, WechatOpenID);
            return RedisHelper.Remove_Entity(Key);
        }
        /// <summary>
        /// 读取Redis数据
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public udv_MobileSignIn SignInByQQTokenRedis(string QQOpenID)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SignInUser, QQOpenID);
            return RedisHelper.Get_Entity<udv_MobileSignIn>(Key);
        }
        
        #endregion

        #region 登录密码错误次数限制

        /// <summary>
        /// 查询登录错误次数
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public udv_LoginCount QueryLoginErrorCount(string LoginName)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.LoginError, LoginName);
            return RedisHelper.Get_Entity<udv_LoginCount>(Key);
        }
        /// <summary>
        /// 保存登录次数
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool LoginErrorCount(udv_LoginCount Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.LoginError, Entity.LoginName);
            return RedisHelper.Set_Entity(Key, Entity, 2, 0, 0);
        }
        #endregion

        #region 日提现次数限制
        /// <summary>
        /// 用户提现限制
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool WithdrawCount(udv_WithdrawCount Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.WithdrawCount, Entity.UserID);
            return RedisHelper.Set_Entity(Key, Entity);
        }
        /// <summary>
        /// 提现限制查询
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public udv_WithdrawCount WithdrawCount(long UserID)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.WithdrawCount, UserID);
            return RedisHelper.Get_Entity<udv_WithdrawCount>(Key);
        }
        #endregion

        #region 系统设置
        /// <summary>
        /// 系统设置
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public bool SetSiteConfig(SiteConfig Entity, string Key)
        {
            return RedisHelper.Set_Entity(Key, Entity);
        }
        /// <summary>
        /// 查询系统设置
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public SiteConfig QuerySiteConfig(string Key)
        {
            return RedisHelper.Get_Entity<SiteConfig>(Key);
        }

        public bool SetScheduler(string jobname)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Scheduler, jobname);
            return RedisHelper.Set_String(Key, jobname, 12, 0, 0);
        }
        public bool SetScheduler_d(string jobname)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Scheduler, jobname);
            return RedisHelper.Set_String(Key, jobname, 80, 0, 0);
        }
        public string GetScheduler(string jobname)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Scheduler, jobname);
            return RedisHelper.Get_String<string>(Key);
        }
        #endregion
    }
}
