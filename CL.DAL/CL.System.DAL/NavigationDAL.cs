using CL.Dapper.Repository;
using CL.SystemInfo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;
using CL.View.Entity.SystemInfo;
using System.Text;
using CL.Tools.Common;
using Dapper;

namespace CL.SystemInfo.DAL
{
    public class NavigationDAL : DataRepositoryBase<NavigationEntity>
    {
        public NavigationDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(NavigationEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(NavigationEntity entity)
        {
            return base.Update(entity);
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DelEntity(int id)
        {
            return base.Delete(id) > 0;
        }
        /// <summary>
        /// 导航名称是否存在
        /// </summary>
        /// <param name="navname"></param>
        /// <returns></returns>
        public bool Exists(string navname)
        {
            return base.RecordCount(new { Name = navname }) == 0 ? false : true;
        }
        /// <summary>
        /// 导航名称是否存在
        /// </summary>
        /// <param name="navname"></param>
        /// <returns></returns>
        public bool Exists(int id)
        {
            return base.RecordCount(new { id = id }) == 0 ? false : true;
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NavigationEntity QueryEntity(int id)
        {
            return base.Get(id);
        }
        /// <summary>
        /// 修改排序号
        /// </summary>
        public bool UpdateSortID(int id, string strValue)
        {
            using (IDbTransaction tran = base.db.BeginTransaction())
            {
                try
                {
                    var entity = base.Get(id, tran);
                    entity.SortID = Convert.ToInt32(strValue);
                    int result = base.Update(entity, tran);
                    tran.Commit();
                    if (result > 0)
                        return true;
                    else
                        return false;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// 获取类别列表
        /// </summary>
        /// <param name="parent_id">父ID</param>
        /// <returns>DataTable</returns>
        public List<udv_Navigation> QueryEntitys(int parent_id)
        {
            List<NavigationEntity> models = base.GetList(new { IsDel = false }, "SortID ASC,id DESC").ToList();

            //重组列表
            if (models == null)
            {
                return null;
            }
            List<udv_Navigation> newData = new List<udv_Navigation>();

            //调用迭代组合成DAGATABLE
            GetChilds(models, newData, parent_id, 0);
            return newData;
        }

        /// <summary>
        /// 从内存中取得所有下级类别列表（自身迭代）
        /// </summary>
        private void GetChilds(List<NavigationEntity> oldData, List<udv_Navigation> newData, int parent_id, int class_layer)
        {
            class_layer++;
            List<NavigationEntity> dr = oldData.FindAll(p => p.ParentID == parent_id);

            foreach (NavigationEntity nav in dr)
            {
                //添加一行数据
                udv_Navigation row = new udv_Navigation();
                row.id = nav.id;
                row.ParentID = nav.ParentID;
                row.class_layer = class_layer;
                row.Name = nav.Name;
                row.Title = nav.Title;
                row.LinkUrl = nav.LinkUrl;
                row.SortID = nav.SortID;
                row.IsLock = nav.IsLock;
                row.Remark = nav.Remark;
                row.ActionType = nav.ActionType;
                row.IsSys = nav.IsSys;
                newData.Add(row);
                //调用自身迭代
                this.GetChilds(oldData, newData, nav.id, class_layer);
            }
        }

              /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<udv_Navigation> QueryListByPage(int pageSize, int pageIndex, ref int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM dbo.CT_Navigation WHERE IsLock=0 ");
            recordCount = base.GetIntSingle(PagingHelper.CreateCountingSql(strSql.ToString()));
            if (recordCount == 0)
                return null;
            SqlMapper.GridReader grid = base.QueryMultiple(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), " id ASC "));
            List<udv_Navigation> list = grid.Read<udv_Navigation>().ToList();
            grid.Dispose();
            return list;
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<NavigationEntity> QueryListByPage()
        {
            return base.GetList(new { IsLock = 0 }, "id asc").ToList();
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<NavigationEntity> GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select id,ParentID,name,Title,LinkUrl,IsSys ");
            strSql.Append(" FROM CT_Navigation ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            SqlMapper.GridReader grid = base.QueryMultiple(strSql.ToString());
            List<NavigationEntity> list = grid.Read<NavigationEntity>().ToList();
            grid.Dispose();
            return list;
        }

    }
}
