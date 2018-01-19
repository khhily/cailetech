using CL.Dapper.Repository;
using CL.Game.Entity;
using CL.Enum.Common;
using System.Data;
using Dapper;

namespace CL.Game.DAL
{
    public class UsersPayRefundDAL : DataRepositoryBase<UsersPayRefundEntity>
    {
        public UsersPayRefundDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long InsertEntity(UsersPayRefundEntity entity)
        {
            return base.Insert_Long(entity) ?? 0;
        }


        /// <summary>
        /// 更新退款申请记录
        /// </summary>
        /// <returns></returns>
        public bool ModifyPayRefund(long ReID, short iResult)
        {
            var para = new DynamicParameters();
            para.Add("@ReID", ReID, DbType.Int64, null, 8);
            para.Add("@Result", iResult, DbType.Int16, null, 1);
            para.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            para.Add("@ReturnDescription", null, DbType.String, ParameterDirection.Output, 100);

            Execute("udp_UpUserPayRefund", para);
            return para.Get<int>("@ReturnValue") == 0;
        }
    }
}
