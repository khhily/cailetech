using CL.Dapper.Repository;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;
using CL.View.Entity.Game;

namespace CL.Game.DAL
{
    public class NewsTypesDAL : DataRepositoryBase<NewsTypesEntity>
    {
        public NewsTypesDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(NewsTypesEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(NewsTypesEntity entity)
        {
            return base.Update(entity);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int TypeID)
        {
            return base.RecordCount(new { TypeID = TypeID }) == 0 ? false : true;
        }
        /// <summary>
        /// 修改排序号
        /// </summary>
        public bool ModifySortID(int id, string strValue)
        {
            using (IDbTransaction tran = base.db.BeginTransaction())
            {
                try
                {
                    var entity = base.Get(id, tran);
                    entity.Sort = Convert.ToInt32(strValue);
                    base.Update(entity, tran);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    base.db.Dispose();
                    base.db.Close();
                }
            }
        }
        /// <summary>
        /// 获取栏目数据
        /// 待优化
        /// </summary>
        /// <returns></returns>
        public List<udv_NewsTypes> QueryEntitys(int ParentID, int TypeID)
        {
            string strWhere = "";
            if (TypeID > 0)
                strWhere = string.Format(" AND TypeID = {0} ", TypeID);

            string strSql = @" WITH Ctree AS
            (
	            SELECT TypeID, ParentID, TypeName, IsShow, IsSystem, Sort, Remarks, 1 AS ClassLayer
	            FROM CT_NewsTypes
	            WHERE ParentID = {0} {1}
	            UNION ALL
	            SELECT a.TypeID, a.ParentID, a.TypeName, a.IsShow, a.IsSystem, a.Sort, a.Remarks, (Ctree.ClassLayer + 1) AS ClassLayer
	            FROM CT_NewsTypes a
	            INNER JOIN Ctree ON Ctree.TypeID = a.ParentID
	            WHERE Ctree.IsShow=1
            )
            SELECT * FROM Ctree order by Sort asc,TypeID desc";
            
            return base.QueryMultiple(string.Format(strSql, ParentID, strWhere)).Read<udv_NewsTypes>().ToList();
        }
        
    }
}
