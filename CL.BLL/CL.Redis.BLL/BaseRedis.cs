using CL.Enum.Common;
using CL.Tools.RedisBase;
using CL.View.Entity.Redis;
using System;
using System.Configuration;

namespace CL.Redis.BLL
{
    public class BaseRedis
    {
        public BaseRedis()
        {
        }

        #region ---------- 验证令牌 生成游客令牌 生成会员令牌 ----------
        /// <summary>
        /// 验证令牌
        /// 验证失败则返回新的游客令牌
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="Equipment"></param>
        /// <returns></returns>
        public udv_TokenInfo VerifyTokenRedis(string Token, string Equipment)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Token.ToString(), Token);
            udv_TokenInfo Entity = RedisHelper.Get_Entity<udv_TokenInfo>(Key);
            if (Entity == null)
                Entity = GenerateSightseerTokenRedis(Equipment);
            else
            {
                if (Entity.TokenType == 0)
                    RedisHelper.Hash_SetExpire(Key, DateTime.Now.AddHours(1));
            }
            return Entity;
        }

        /// <summary>
        /// 生成游客Token
        /// </summary>
        /// <param name="Equipment"></param>
        /// <returns></returns>
        protected udv_TokenInfo GenerateSightseerTokenRedis(string Equipment)
        {
            string Token = Guid.NewGuid().ToString().Replace("-", "");
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Token.ToString(), Token);
            udv_TokenInfo Entity = RedisHelper.Get_Entity<udv_TokenInfo>(Key);
            if (Entity != null)
                return GenerateSightseerTokenRedis(Equipment);
            else
            {
                Entity = new udv_TokenInfo()
                {
                    Equipment = Equipment,
                    TokenType = 0,
                    UserCode = 0,
                    Token = Token
                };
                //缓存一小时
                bool r = RedisHelper.Set_Entity(Key, Entity, 1, 0, 0);
                if (!r)
                    throw new Exception("生成游客令牌错误(Redis写入失败)。");
                else
                    return Entity;
            }
        }
        /// <summary>
        /// 创建会员令牌
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        public udv_TokenInfo GenerateUserTokenRedis(string Equipment, string Token, long UserCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Token.ToString(), Token);
            RedisHelper.Remove_Entity(Key);
            udv_TokenInfo Entity = new udv_TokenInfo()
            {
                Equipment = Equipment,
                TokenType = 1,
                UserCode = UserCode,
                Token = Token
            };
            //缓存一小时
            bool r = RedisHelper.Set_Entity(Key, Entity);
            if (!r)
                throw new Exception("生成会员令牌错误(Redis写入失败)。");
            return Entity;
        }
        /// <summary>
        /// 验证令牌是否有效
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public udv_TokenInfo VerifyTokenRedis(string Token)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Token.ToString(), Token);
            var Entity = RedisHelper.Get_Entity<udv_TokenInfo>(Key);
            return Entity;
        }
        /// <summary>
        /// 解除令牌
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public bool UnToken(string Token)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Token.ToString(), Token);
            return RedisHelper.Remove_Entity(Key);
        }
        #endregion

    }
}
