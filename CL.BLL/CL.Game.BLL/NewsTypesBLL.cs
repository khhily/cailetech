//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//     2017-04-26 11:36:00 Created by LEON
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using CL.Enum.Common;
using CL.Game.DAL;
using CL.Game.Entity;
using CL.View.Entity.Game;
using System.Collections.Generic;
using System.Linq;

namespace CL.Game.BLL
{

    /// <summary>
    ///NewsTypes info
    /// </summary>
    public class NewsTypesBLL
    {
        NewsTypesDAL dal = new NewsTypesDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int TypeID)
        {
            return dal.Exists(TypeID);
        }

        /// <summary>
        /// 修改排序号
        /// </summary>
        public bool ModifySortID(int id, string strValue)
        {
            return dal.ModifySortID(id, strValue);
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="TypeID"></param>
        /// <returns></returns>
        public bool DelEntity(int TypeID)
        {
            return DelEntity(TypeID);
        }
        /// <summary>
        /// 获取栏目数据
        /// 待优化
        /// </summary>
        /// <returns></returns>
        public List<udv_NewsTypes> QueryEntitys(int ParentID, int TypeID)
        {
            List<udv_NewsTypes> Entitys = dal.QueryEntitys(ParentID, TypeID);
            List<udv_NewsTypes> newlist = new List<udv_NewsTypes>();
            this.GetChilds(Entitys, newlist, ParentID);
            return newlist;
        }
        /// <summary>
        /// 获取所有栏目
        /// </summary>
        /// <returns></returns>
        public List<NewsTypesEntity> QueryEntitys()
        {
            return dal.GetList(new { IsShow = true }, "TypeID asc").ToList();
        }
        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <param name="TypeID"></param>
        /// <returns></returns>
        public NewsTypesEntity QueryEntity(int TypeID)
        {
            return dal.Get(TypeID);
        }
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(NewsTypesEntity entity)
        {
            return dal.InsertEntity(entity);
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(NewsTypesEntity entity)
        {
            return dal.ModifyEntity(entity);
        }
        /// <summary>
        /// 自身迭代
        /// </summary>
        private void GetChilds(List<udv_NewsTypes> oldData, List<udv_NewsTypes> newData, int parent_id)
        {
            List<udv_NewsTypes> dr = oldData.FindAll(p => p.ParentID == parent_id);

            foreach (udv_NewsTypes nav in dr)
            {
                //添加一行数据
                udv_NewsTypes row = new udv_NewsTypes();
                row.TypeID = nav.TypeID;
                row.ParentID = nav.ParentID;
                row.ClassLayer = nav.ClassLayer;
                row.TypeName = nav.TypeName;
                row.IsShow = nav.IsShow;
                row.IsSystem = nav.IsSystem;
                row.Sort = nav.Sort;
                row.Remarks = nav.Remarks;
                row.IsDel = false;
                newData.Add(row);
                //调用自身迭代
                this.GetChilds(oldData, newData, nav.TypeID);
            }
        }

    }
}
