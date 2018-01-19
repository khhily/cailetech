using CL.Dapper.Repository;
using CL.Coupons.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Enum.Common;
using System.Data;

namespace CL.Coupons.DAL
{
    public class CouponsRecordDAL : DataRepositoryBase<CouponsRecordEntity>
    {
        public CouponsRecordDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

    }
}
