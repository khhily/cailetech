using CL.Dapper.Repository;
using CL.Game.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;
using CL.Tools.Common;
using Dapper;

namespace CL.Game.DAL
{
    public class NewsDAL : DataRepositoryBase<NewsEntity>
    {
        public NewsDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int NewsID)
        {
            int iResult = base.RecordCount(new { NewsID = NewsID });
            if (iResult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(NewsEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(NewsEntity entity)
        {
            return base.Update(entity);
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public bool DelEntity(int NewsID)
        {
            return base.Delete(NewsID) > 0;
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public NewsEntity QueryEntity(int NewsID)
        {
            return base.Get(NewsID);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<udv_NewsTypeName> QueryListByPage(string strTitle, int TypeID, int pageSize, int pageIndex, ref int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT a.*, b.TypeName FROM CT_News a
                    LEFT JOIN CT_NewsTypes b ON b.TypeID=a.TypeID
                    WHERE 1 = 1 ");

            if (strTitle.Trim() != "")
            {
                strSql.Append(" and a.Title like  '%" + strTitle + "%' ");
            }
            if (TypeID > 0)
            {
                strSql.Append(" and a.TypeID IN(SELECT TypeID FROM dbo.CT_NewsTypes WHERE TypeID=" + TypeID + " OR ParentID=" + TypeID + ") ");
            }

            recordCount = base.GetIntSingle(PagingHelper.CreateCountingSql(strSql.ToString()));
            if (recordCount == 0)
                return null;
            SqlMapper.GridReader grid = base.QueryMultiple(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), " a.PublishTime desc "));
            List<udv_NewsTypeName> list = grid.Read<udv_NewsTypeName>().ToList();
            grid.Dispose();
            return list;
        }
        /// <summary>
        /// 分页获取数据列表(审核)
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<udv_NewsTypeName> QueryListByPage(int pageSize, int pageIndex, ref int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT a.*, b.TypeName FROM CT_News a
                    LEFT JOIN CT_NewsTypes b ON b.TypeID=a.TypeID
                    WHERE a.AuditingStatus = 1 and IsDel = 0  ");
            recordCount = base.GetIntSingle(PagingHelper.CreateCountingSql(strSql.ToString()));
            if (recordCount == 0)
                return null;
            SqlMapper.GridReader grid = base.QueryMultiple(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), " a.PublishTime desc "));
            List<udv_NewsTypeName> list = grid.Read<udv_NewsTypeName>().ToList();
            grid.Dispose();
            return list;
        }

        /// <summary>
        /// 查询最新N条新闻
        /// </summary>
        /// <param name="TypeID">类型ID</param>
        /// <param name="LotteryCode">彩种ID</param>
        /// <param name="Top">查询多少条</param>
        /// <returns></returns>
        public List<NewsEntity> QueryEntitys(int TypeID, int LotteryCode, int Top)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" SELECT TOP {0} NewsID,Title,IsRecommend FROM dbo.CT_News ", Top);
            strSql.AppendFormat(" WHERE TypeID={0} AND AuditingStatus=2 AND IsDel = 0 ", TypeID);
            if (LotteryCode > 0)
                strSql.AppendFormat(" AND LotteryCode = {0} ", LotteryCode);
            strSql.Append(" ORDER BY IsRecommend DESC,PublishTime DESC ");
            return base.QueryList(strSql.ToString()).ToList();
        }
        /// <summary>
        /// 查询最新N条新闻
        /// </summary>
        /// <param name="NewsID"></param>
        /// <param name="IsGreater"></param>
        /// <returns></returns>
        public List<NewsEntity> QueryEntitys(int NewsID, int Top, bool IsGreater)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" SELECT TOP {0} s.NewsID,s.Title,s.IsRecommend,s.PublishTime FROM dbo.CT_News AS s ", Top);
            strSql.AppendFormat(" INNER JOIN (SELECT LotteryCode,TypeID FROM dbo.CT_News WHERE NewsID = {0} ", NewsID);
            strSql.AppendFormat(" ) AS tab ON s.LotteryCode = tab.LotteryCode AND s.TypeID = tab.TypeID AND s.NewsID {1} {0} ", NewsID, IsGreater == true ? ">" : "<");
            strSql.Append(" ORDER BY s.IsRecommend DESC,s.PublishTime DESC ");
            return base.QueryList(strSql.ToString()).ToList();
        }
        /// <summary>
        /// 支持文章
        /// </summary>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public bool SupportNews(int NewsID)
        {
            using (IDbTransaction tran = db.BeginTransaction())
            {
                try
                {
                    var Entity = this.Get(NewsID, tran);
                    Entity.SupportNum += 1;
                    this.Update(Entity, tran);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// 反对文章
        /// </summary>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public bool OpposeNews(int NewsID)
        {
            using (IDbTransaction tran = db.BeginTransaction())
            {
                try
                {
                    var Entity = this.Get(NewsID, tran);
                    Entity.OpposeNum += 1;
                    this.Update(Entity, tran);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }


    }
}
