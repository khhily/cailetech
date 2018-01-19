using CL.Tools.RedisBase;
using CL.View.Entity.Redis;
using System.Collections.Generic;
using CL.Enum.Common;
using CL.Json.Entity.WebAPI;
using CL.Game.Entity;
using CL.View.Entity.Other;

namespace CL.Redis.BLL
{
    public class UserRedis
    {
        #region ---------- 身份认证 银行卡 ----------

        /// <summary>
        /// 插入：银行卡
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Insert_BankCardRedis(udv_BankCard Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.BankCard, Entity.UserCode);
            return RedisHelper.Hash_Set(Key, Entity.BankCode.ToString(), Entity);
        }
        /// <summary>
        /// 查询：银行卡数据集
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public List<udv_BankCard> Query_BankCardRedis(long UserCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.BankCard, UserCode);
            return RedisHelper.Hash_GetAll<udv_BankCard>(Key);
        }

        /// <summary>
        /// 删除：银行卡
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="BankCode"></param>
        /// <returns></returns>
        public bool Remove_BankCardRedis(long UserCode, long BankCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.BankCard, UserCode);
            return RedisHelper.Hash_Remove(Key, BankCode.ToString());
        }
        #endregion

        #region 用户分享限制
        /// <summary>
        /// 保存当此分享
        /// (次数累计)
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool SaveUserSharing(udv_Sharing Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Sharing, Entity.UserCode);
            return RedisHelper.Set_Entity(Key, Entity);
        }
        /// <summary>
        /// 查询当前分享已注册次数
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public udv_Sharing QueryUserSharing(long UserCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Sharing, UserCode);
            return RedisHelper.Get_Entity<udv_Sharing>(Key);
        }
        #endregion
    }
}
