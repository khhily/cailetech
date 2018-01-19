using CL.Dapper.Repository;
using CL.SystemInfo.Entity;
using CL.Enum.Common;
using System.Data;

namespace CL.SystemInfo.DAL
{
    /// <summary>
    /// 错误日志
    /// 2017年4月26日
    /// </summary>
    public class ErrorLogDAL : DataRepositoryBase<ErrorLogEntity>
    {
        public ErrorLogDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 插入错误日志
        /// </summary>
        /// <param name="entity">错误日志对象</param>
        /// <returns></returns>
        public int InsertEntity(ErrorLogEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
    }
}
