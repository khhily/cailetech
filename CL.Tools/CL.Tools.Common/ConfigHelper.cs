using System;
using System.Configuration;

namespace CL.Tools.Common
{
    public class ConfigHelper
    {
        /// <summary>
        /// 获取web.config中AppSettings的配置项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            string str = ConfigurationManager.AppSettings[key];
            if (str == null)
            {
                throw new Exception("WebConfigHasNotAddKet:" + key);
            }
            return str;
        }

        /// <summary>
        ///  获取web.config中AppSettings的配置项字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            string CacheKey = "AppSettings-" + key;
            object objModel = CacheHelper.Get(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = Get(key);
                    if (objModel != null)
                    {
                        CacheHelper.Insert(CacheKey, objModel, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                    }
                }
                catch {
                    throw;
                }
            }
            return objModel.ToString();
        }

        /// <summary>
        ///  获取web.config中AppSettings的配置Bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(string key)
        {
            bool result = false;
            string Val = GetConfigString(key);
            if (Val != null && string.Empty != Val)
            {
                try
                {
                    result = bool.Parse(Val);
                }
                catch {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        ///  获取web.config中AppSettings的配置Decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0;
            string Val = GetConfigString(key);
            if (Val != null && string.Empty != Val)
            {
                try
                {
                    result = decimal.Parse(Val);
                }
                catch {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        ///  获取web.config中AppSettings的配置Int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key)
        {
            int result = 0;
            string Val = GetConfigString(key);
            if (Val != null && string.Empty != Val)
            {
                try
                {
                    result = int.Parse(Val);
                }
                catch {
                    throw;
                }
            }
            return result;
        }
    }
}
