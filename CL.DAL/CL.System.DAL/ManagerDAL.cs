using CL.Dapper.Repository;
using CL.SystemInfo.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;
using CL.View.Entity.SystemInfo;

namespace CL.SystemInfo.DAL
{
    public class ManagerDAL : DataRepositoryBase<ManagerEntity>
    {
        public ManagerDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(ManagerEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(ManagerEntity entity)
        {
            return base.Update(entity);
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public ManagerEntity GetModel(string UserName, string PassWord)
        {
            return base.Get(new { UserName = UserName, PassWord = PassWord }, "ID desc");
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public ManagerEntity GetModel(int id)
        {
            return base.Get(new { id = id }, "ID desc");
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string UserName)
        {
            return base.RecordCount(new { UserName = UserName }) == 0 ? false : true;
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            return base.RecordCount(new { id = id }) == 0 ? false : true;
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
        /// 分页获取数据列表
        /// </summary>
        public List<udv_Manager> QueryListByPage(int RoleType, string strName, string orderby, int pageSize, int pageIndex, ref int recordCount)
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" RoleType>=@RoleType ");
            object Paramters = new { RoleType = RoleType };
            if (!string.IsNullOrEmpty(strName))
            {
                Where.Append(" AND (UserName like  @UserName or NickName like @UserName) ");
                Paramters = new { RoleType = RoleType, UserName = string.Format("%{0}%", strName) };
            }
            recordCount = base.GetIntSingle(string.Format("select count(1) from udv_Manager where {0}", Where.ToString()), Paramters);
            return new DataRepositoryBase<udv_Manager>(DbConnectionEnum.CaileSystem).GetListPaged(pageIndex, pageSize, Where.ToString(), orderby, Paramters).ToList();

        }
    }
}
