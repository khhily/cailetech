using CL.Dapper.Repository;
using CL.Game.Entity;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL
{
    public class SchemesDetailDAL : DataRepositoryBase<SchemesDetailEntity>
    {
        public SchemesDetailDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        
        /// <summary>
        /// 查询单个对象
        /// </summary>
        /// <param name="SDID"></param>
        /// <returns></returns>
        public SchemesDetailEntity QueryEntity(long SDID)
        {
            return base.Get(SDID);
        }
        /// <summary>
        /// 根据方案编号查询方案详情
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public List<SchemesDetailEntity> QueryEntityBySchemeID(long SchemeID)
        {
            return base.GetList(new { SchemeID = SchemeID }, "SDID asc").ToList();
        }
    }

}
