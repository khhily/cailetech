using CL.Dapper.Repository;
using CL.View.Entity.Other;
using CL.Enum.Common;
using System.Data;
using CL.Tools.Common;

namespace CL.SystemInfo.DAL
{
    public class SiteConfigDAL : DataRepositoryBase<SiteConfig>
    {
        public SiteConfigDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        private static object lockHelper = new object();

        /// <summary>
        ///  读取站点配置文件
        /// </summary>
        public SiteConfig loadConfig(string configFilePath)
        {
            return (SiteConfig)SerializationHelper.Load(typeof(SiteConfig), configFilePath);
        }

        /// <summary>
        /// 写入站点配置文件
        /// </summary>
        public SiteConfig saveConifg(SiteConfig model, string configFilePath)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(model, configFilePath);
            }
            return model;
        }
    }
}
