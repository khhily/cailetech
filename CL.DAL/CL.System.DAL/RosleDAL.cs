using CL.Dapper.Repository;
using CL.SystemInfo.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;
using Dapper;

namespace CL.SystemInfo.DAL
{
    public class RosleDAL : DataRepositoryBase<RosleEntity>
    {
        public RosleDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(RosleEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(RosleEntity entity)
        {
            return base.Update(entity);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RosleEntity GetModel(int RoleID)
        {
            RosleEntity entity = new RosleEntity();
            entity = base.Get(new { RoleID = RoleID }, "RoleID");
            if (entity != null)
                entity.manager_role_values = new RosleValueDAL(DbConnectionEnum.CaileSystem).GetList(new { RoleID = RoleID }, "id desc").ToList();
            return entity;
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int RoleID)
        {
            int iResult = base.RecordCount(new { RoleID = RoleID });

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
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string RoleName)
        {
            int iResult = base.RecordCount(new { RoleName = RoleName });

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
        /// 获得数据列表
        /// </summary>
        public List<RosleEntity> GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select RoleID,RoleName,RoleType ");
            strSql.Append(" FROM CT_Rosle ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlMapper.GridReader grid = base.QueryMultiple(strSql.ToString());
            List<RosleEntity> list = grid.Read<RosleEntity>().ToList();
            grid.Dispose();
            return list;
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
    }
}
