using CL.Dapper.Repository;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;
using Dapper;
using CL.View.Entity.Game;
using CL.Tools.Common;
using CL.Json.Entity.WebAPI;
namespace CL.Game.DAL
{
    public class SalePointFileDAL : DataRepositoryBase<SalePointFileEntity>
    {
        public SalePointFileDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(SalePointFileEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
        
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<SalePointFileEntity> QueryEntitys(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select SalePointFileID,FileUrl,FileName,FileEXT ");
            strSql.Append(" FROM CT_SalePointFile ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlMapper.GridReader grid = base.QueryMultiple(strSql.ToString());
            List<SalePointFileEntity> list = grid.Read<SalePointFileEntity>().ToList();
            grid.Dispose();
            return list;
        }
    }
}
