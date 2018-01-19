using CL.Tools.Common;
using System;
using System.IO;

namespace CL.Tools.RedisBase
{
    public class RedisConfigInfo
    {
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <returns></returns>
        public static RedisConfigInfo GetConfig(string FilePath = "Config")
        {
            string CacheKey = "Cache_RedisConfigInfo";
            RedisConfigInfo Entity = CacheHelper.Get<RedisConfigInfo>(CacheKey);
            if (Entity == null)
            {
                string cfgPath = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ApplicationBase)
                                    + Path.DirectorySeparatorChar + FilePath + Path.DirectorySeparatorChar + "config.properties";
                Entity = new RedisConfigInfo();
                var Properties = Entity.GetType().GetProperties();
                using (StreamReader sr = new StreamReader(cfgPath))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        if (line.StartsWith("#"))
                        {
                            continue;
                        }
                        int startInd = line.IndexOf("=");
                        string key = line.Substring(0, startInd);
                        string val = line.Substring(startInd + 1, line.Length - (startInd + 1));
                        foreach (var PropertyInfo in Properties)
                            if (PropertyInfo.Name.Trim().Equals(key))
                                PropertyInfo.SetValue(Entity, val);
                    }
                }
                //本地缓存
                CacheHelper.Insert(CacheKey, Entity, 60);
            }
            return Entity;
        }

        /// <summary>
        /// 可写的Redis链接地址
        /// </summary>
        public string WriteServerList { set; get; }
        /// <summary>
        /// 可读的Redis链接地址
        /// </summary>
        public string ReadServerList { set; get; }
        /// <summary>
        /// 最大写链接数
        /// </summary>
        public string MaxWritePoolSize { set; get; }
        /// <summary>
        /// 最大读链接数
        /// </summary>
        public string MaxReadPoolSize { set; get; }
        /// <summary>
        /// 自动重启
        /// </summary>
        public string AutoStart { set; get; }
        /// <summary>
        /// 本地缓存到期时间，单位:秒
        /// </summary>
        public string LocalCacheTime { set; get; }
        /// <summary>
        /// 是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项
        /// </summary>
        public string RecordeLog { set; get; }
    }
}
