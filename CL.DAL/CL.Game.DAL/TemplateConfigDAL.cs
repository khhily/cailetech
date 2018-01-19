using CL.Dapper.Repository;
using CL.Game.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;
using Dapper;

namespace CL.Game.DAL
{
    public class TemplateConfigDAL : DataRepositoryBase<TemplateConfigEntity>
    {
        public TemplateConfigDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long ID)
        {
            return RecordCount(new { ID = ID }) == 0 ? false : true;
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(TemplateConfigEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(TemplateConfigEntity entity)
        {
            return base.Update(entity);
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TemplateConfigEntity QueryEntity(int ID)
        {
            return base.Get(ID);
        }
        /// <summary>
        /// 获取所有对象
        /// </summary>
        /// <returns></returns>
        public List<TemplateConfigEntity> QueryEntitys()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ID,Title,TemplateContent,TemplateType,CreateTime,AdminID FROM CT_TemplateConfig ");
            return base.QueryList(sql.ToString()).ToList();
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DelEntity(long ID)
        {
            return base.Delete(ID) > 0;
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<TemplateConfigEntity> QueryListByPage(string strName, int TemplateType, string orderby, int pageSize, int pageIndex, ref int recordCount)
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" 1 = 1");
            var Parms = new DynamicParameters();
            if (TemplateType > 0)
            {
                Where.Append(" AND TemplateType = @TemplateType ");
                Parms.Add("@TemplateType", TemplateType, DbType.Int16, null, 1);
            }
            if (strName.Trim() != "")
            {
                Where.Append(" AND Title like @Title ");
                Parms.Add("@Title", string.Format("%{0}%", strName), DbType.String, null, 64);
            }

            recordCount = GetIntSingle(string.Format("select count(1) from CT_TemplateConfig where {0}", Where.ToString()), Parms);
            return base.GetListPaged(pageIndex, pageSize, Where.ToString(), "ID DESC", Parms).ToList();
        }
    }
}
