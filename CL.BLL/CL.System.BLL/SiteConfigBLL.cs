using CL.Enum.Common;
using CL.Redis.BLL;
using CL.SystemInfo.DAL;
using CL.Tools.Common;
using CL.View.Entity.Other;

namespace CL.SystemInfo.BLL
{
    public class SiteConfigBLL
    {
        SiteConfigDAL dal = new SiteConfigDAL(DbConnectionEnum.CaileSystem);
        /// <summary>
        ///  读取配置文件
        /// </summary>
        public SiteConfig loadConfig()
        {
            SiteConfig model = new SystemRedis().QuerySiteConfig(CLKeys.CACHE_SITE_CONFIG);
            if (model == null)
            {
                model = dal.loadConfig(Utils.GetXmlMapPath(CLKeys.FILE_SITE_XML_CONFING));
                new SystemRedis().SetSiteConfig(model, CLKeys.CACHE_SITE_CONFIG);
            }
            return model;
        }

        /// <summary>
        ///  保存配置文件
        /// </summary>
        public SiteConfig saveConifg(SiteConfig model)
        {
            return dal.saveConifg(model, Utils.GetXmlMapPath(CLKeys.FILE_SITE_XML_CONFING));
        }

    }
}
