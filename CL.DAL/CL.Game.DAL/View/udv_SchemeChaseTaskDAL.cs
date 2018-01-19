using CL.Dapper.Repository;
using CL.View.Entity.Game;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL.View
{
    public class udv_SchemeChaseTaskDAL : DataRepositoryBase<udv_SchemeChaseTask>
    {
        public udv_SchemeChaseTaskDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public udv_SchemeChaseTask QueryEntity(long SchemeID)
        {
            return base.Get(SchemeID);
        }
    }
}
