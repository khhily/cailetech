using CL.Dapper.Repository;
using CL.View.Entity.Game;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL.View
{
    public class udv_SchemesDetailDAL : DataRepositoryBase<udv_SchemesDetail>
    {
        public udv_SchemesDetailDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 获取方案明细数据
        /// </summary>
        /// <returns></returns>
        public List<udv_SchemesDetail> QueryEntitysBySchemeID(long SchemeID)
        {
            return base.GetList(new { SchemeID = SchemeID }).ToList();
        }
    }
}
