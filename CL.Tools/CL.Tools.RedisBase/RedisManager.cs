using ServiceStack.Redis;
using System;
using System.Linq;

namespace CL.Tools.RedisBase
{
    public class RedisManager
    {
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfigInfo redisConfigInfo = RedisConfigInfo.GetConfig();

        private static PooledRedisClientManager prcm;
        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }
        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            int i = 0;
            cc: i++; //重连三次
            try
            {
                string[] writeServerList = SplitString(redisConfigInfo.WriteServerList, ",");
                string[] readServerList = SplitString(redisConfigInfo.ReadServerList, ",");
                prcm = new PooledRedisClientManager(readServerList, writeServerList,
                                 new RedisClientManagerConfig
                                 {
                                     MaxWritePoolSize = Convert.ToInt32(redisConfigInfo.MaxWritePoolSize),
                                     MaxReadPoolSize = Convert.ToInt32(redisConfigInfo.MaxReadPoolSize),
                                     AutoStart = redisConfigInfo.AutoStart.ToLower() == "true" ? true : false,
                                 });
                if (prcm == null)
                {
                    if (i <= 3)
                        goto cc;
                }
            }
            catch
            {
                if (i <= 3)
                    goto cc;
            }

        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient()
        {
            if (prcm == null)
                CreateManager();

            return prcm.GetClient();
        }
    }
}
