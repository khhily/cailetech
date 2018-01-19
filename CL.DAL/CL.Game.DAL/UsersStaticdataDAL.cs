using CL.Dapper.Repository;
using CL.Game.Entity;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL
{
    public class UsersStaticdataDAL : DataRepositoryBase<UsersStaticdataEntity>
    {
        public UsersStaticdataDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
         
    }
}
