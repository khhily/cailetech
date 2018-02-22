using CL.Tools.Common;
using System.Xml;

namespace CL.Tools.RedisBase
{
    public class RedisConfigInfo
    {
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
