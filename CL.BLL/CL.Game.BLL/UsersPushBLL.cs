using CL.Enum.Common;
using CL.Game.DAL;
using CL.Game.Entity;
using CL.Json.Entity;
using CL.Redis.BLL;
using CL.Tools.Common;
using CL.Tools.JiGuangPush;
using cn.jpush.api.push;
using System;
using System.Collections.Generic;

namespace CL.Game.BLL
{
    public class UsersPushBLL
    {
        UsersPushDAL dal = new UsersPushDAL(DbConnectionEnum.CaileGame);
        Log log = new Log("UsersPushBLL");
        /// <summary>
        /// 查询单个对象
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public UsersPushEntity QueryEntity(long UserCode)
        {
            return dal.QueryEntity(UserCode);
        }
        /// <summary>
        /// 插入单个对象
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool InsertEntity(UsersPushEntity Entity)
        {
            return dal.InsertEntity(Entity);
        }
        /// <summary>
        /// 推送查询
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public List<udv_UserPushList> QueryPushList(string UserName)
        {
            return dal.QueryPushList(UserName);
        }



        #region 自定义方法 
        /// <summary>
        /// 查询百度推送标识
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public string QueryBaiduIdentify(long UserCode)
        {
            try
            {
                var Redis_Entity = new SystemRedis().SignInByUserCodeRedis(UserCode);
                if (Redis_Entity == null)
                {
                    var Db_Entity = this.QueryEntity(UserCode);
                    return Db_Entity.PushIdentify;
                }
                else
                {
                    if (string.IsNullOrEmpty(Redis_Entity.PushIdentify))
                    {
                        var Db_Entity = this.QueryEntity(UserCode);
                        if (Db_Entity != null)
                        {
                            Redis_Entity.PushIdentify = Db_Entity.PushIdentify;
                            new SystemRedis().SignInSessionRedis(Redis_Entity);
                        }
                    }
                    return Redis_Entity.PushIdentify;
                }
            }
            catch (Exception ex)
            {
                log.Write("查询百度推送标识：" + ex.StackTrace, true);
                return string.Empty;
            }
        }
        /// <summary>
        /// 保存推送标识
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        /// <param name="PushIdentify"></param>
        /// <returns></returns>
        public JsonResult SavePushIdentify(string Token, long UserCode, string PushIdentify)
        {
            JsonResult result = null;
            try
            {
                int RecCode = (int)ResultCode.Success;
                bool rec = this.InsertEntity(new UsersPushEntity()
                {
                    UserId = UserCode,
                    PushIdentify = PushIdentify
                });
                if (rec)
                {
                    var Redis_Entity = new SystemRedis().SignInByUserCodeRedis(UserCode);
                    if (Redis_Entity != null)
                    {
                        Redis_Entity.PushIdentify = PushIdentify;
                        new SystemRedis().SignInSessionRedis(Redis_Entity);
                    }
                }
                else
                    RecCode = (int)ResultCode.SavePushFailure;
                result = new JsonResult()
                {
                    Code = RecCode,
                    Msg = Common.GetDescription((ResultCode)RecCode)
                };
            }
            catch (Exception ex)
            {
                log.Write("保存推送标识：" + ex.StackTrace, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return result;
        }
        #endregion

        #region 推送
        /// <summary>
        /// 推送消息_广播
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public bool PushMessager_Android_All(string Title, string Messager)
        {
            try
            {
                MessageResult resultJson = null;
                new PushHelper().BroadcastNotice(Title, Messager, ref resultJson);
                log.Write("推送消息_广播回调数据：" + resultJson);
                return true;
            }
            catch (Exception ex)
            {
                log.Write("推送消息_广播：" + ex.StackTrace, true);
                return false;
            }
        }
        /// <summary>
        /// 推送消息_单播
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="Title"></param>
        /// <param name="Messager"></param>
        /// <returns></returns>
        public bool PushMessager_Android_Single(long UserCode, string Title, string Content, string Msg)
        {
            try
            {
                string PushRegId = this.QueryBaiduIdentify(UserCode);
                if (string.IsNullOrEmpty(PushRegId))
                    return false;
                MessageResult resultJson = null;
                new PushHelper().PushPersonal(new string[] { PushRegId }, Title, Content, Msg, ref resultJson);
                log.Write("推送消息_单播回调数据：" + Newtonsoft.Json.JsonConvert.SerializeObject(resultJson));
                return true;
            }
            catch (Exception ex)
            {
                log.Write("推送消息_单播：" + ex.StackTrace, true);
                return false;
            }
        }
        /// <summary>
        /// 推送消息_多播
        /// </summary>
        /// <param name="ChannelIDs"></param>
        /// <param name="Title"></param>
        /// <param name="Messager"></param>
        /// <returns></returns>

        #endregion
    }
}
