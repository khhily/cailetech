using CL.Dapper.Repository;
using CL.View.Entity.Game;
using CL.Enum.Common;
using System.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CL.Tools.Common;
using System.Linq;

namespace CL.Game.DAL.View
{
    public class udv_SchemeChaseTaskDetailDAL : DataRepositoryBase<udv_SchemeChaseTaskDetail>
    {
        public udv_SchemeChaseTaskDetailDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID">方案号</param>
        /// <returns></returns>
        public udv_SchemeChaseTaskDetail QueryEntitysBySchemeID(long SchemeID)
        {
            return base.Get(SchemeID);
        }
        /// <summary>
        /// 查询对象集合
        /// </summary>
        /// <param name="SchemeID">方案号</param>
        /// <param name="PageIndex">当期页</param>
        /// <param name="PageSize">每页大小</param>
        /// <param name="RecordCount">共多少条记录</param>
        /// <returns></returns>
        public List<udv_SchemeChaseTaskDetail> QueryListByPage(long schemeID, string orderby, int PageIndex, int PageSize, ref int RecordCount)
        {
            StringBuilder whereSql = new StringBuilder();
            whereSql.AppendFormat(" SchemeID = '{0}' ", schemeID);
            RecordCount = base.GetIntSingle(PagingHelper.CreateCountingSql(new udv_SchemeChaseTaskDetail().GetType().Name, whereSql.ToString()));
            return base.GetListPaged(PageIndex, PageSize, whereSql.ToString(), "StartTime desc").ToList();
        }
    }
}
