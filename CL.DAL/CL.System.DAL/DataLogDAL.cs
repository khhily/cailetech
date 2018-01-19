using CL.Dapper.Repository;
using CL.Enum.Common;
using CL.SystemInfo.Entity;
using System.Data;

namespace CL.SystemInfo.DAL
{
    /// <summary>
    /// 操作日志
    /// 2017年4月26日
    /// </summary>
    public class DataLogDAL : DataRepositoryBase<DataLogEntity>
    {
        public DataLogDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {

        }
        /// <summary>
        /// 插入操作日志
        /// </summary>
        /// <param name="entity">操作日志对象</param>
        /// <returns></returns>
        public int InertEntity(DataLogEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
    }
}
