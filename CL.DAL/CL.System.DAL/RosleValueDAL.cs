using CL.Dapper.Repository;
using CL.SystemInfo.Entity;
using CL.Enum.Common;
using System.Data;
using System.Collections.Generic;
using System.Linq;
namespace CL.SystemInfo.DAL
{
    public class RosleValueDAL : DataRepositoryBase<RosleValueEntity>
    {
        public RosleValueDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="Entitys"></param>
        /// <returns></returns>
        public bool ModifyRoles(int RoleID, List<RosleValueEntity> Entitys)
        {

            using (IDbTransaction tran = base.db.BeginTransaction())
            {
                try
                {
                    base.DeleteList(new { RoleID = RoleID }, tran);
                    Entitys.ForEach((Entity) =>
                    {
                        base.Insert(Entity, tran);
                    });
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }

        }
    }
}
