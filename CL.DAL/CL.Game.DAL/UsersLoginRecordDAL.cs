using CL.Dapper.Repository;
using CL.Game.Entity;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL
{
    public class UsersLoginRecordDAL : DataRepositoryBase<UsersLoginRecordEntity>
    {
        public UsersLoginRecordDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        
    }
}
