using CL.Dapper.Repository;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Enum.Common;
using System.Data;

/// <summary>
/// 加奖记录
/// </summary>
namespace CL.Game.DAL
{
    public class SchemesAwardsDAL : DataRepositoryBase<SchemesAwardsEntity>
    {
        public SchemesAwardsDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
    }
}
