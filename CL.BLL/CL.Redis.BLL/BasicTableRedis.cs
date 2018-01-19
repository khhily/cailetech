using CL.Tools.RedisBase;
using CL.View.Entity.Redis;
using System.Collections.Generic;
using CL.Enum.Common;
using CL.Json.Entity.WebAPI;
using CL.View.Entity.BasicTable;

namespace CL.Redis.BLL
{
    public class BasicTableRedis
    {
        #region 和值基础表
        /// <summary>
        /// 和值基础表设置
        /// </summary>
        /// <param name="PlayCode"></param>
        /// <param name="Entitys"></param>
        /// <returns></returns>
        public bool ValueBasicTableRedis(int PlayCode, List<ValueTable> Entitys)
        {
            string Key = string.Format("{0}:{1}:{2}", RedisKeysEnum.BasicTable, RedisKeysEnum.Value, PlayCode);
            return RedisHelper.Set_Entity(Key, Entitys);
        }
        /// <summary>
        /// 和值基础表读取
        /// </summary>
        /// <param name="PlayCode"></param>
        /// <returns></returns>
        public List<ValueTable> ValueBasicTableRedis(int PlayCode)
        {
            string Key = string.Format("{0}:{1}:{2}", RedisKeysEnum.BasicTable, RedisKeysEnum.Value, PlayCode);
            return RedisHelper.Get_Entity<List<ValueTable>>(Key);
        }
        #endregion
    }
}
