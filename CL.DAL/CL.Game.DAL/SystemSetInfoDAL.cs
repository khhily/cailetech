using CL.Dapper.Repository;
using CL.Game.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL
{
    public class SystemSetInfoDAL : DataRepositoryBase<SystemSetInfoEntity>
    {
        public SystemSetInfoDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SetKey"></param>
        /// <returns></returns>
        public SystemSetInfoEntity QueryEntity(string SetKey)
        {
            return base.Get(new { SetKey = SetKey }, "SetID asc");
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(SystemSetInfoEntity entity)
        {
            return base.Update(entity);
        }
        /// <summary>
        /// 获取实体集合
        /// </summary>
        public List<SystemSetInfoEntity> QueryEntitys()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from CT_SystemSetInfo ");
            return base.QueryList(sql.ToString()).ToList();
        }
    }
}
