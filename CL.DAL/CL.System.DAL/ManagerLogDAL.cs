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
    public class ManagerLogDAL : DataRepositoryBase<ManagerLogEntity>
    {
        public ManagerLogDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(ManagerLogEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }

        /// <summary>
        /// 根据用户名返回上一次登录记录
        /// </summary>
        public ManagerLogEntity QueryEntity(string UserName, int top_num, string ActionType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" declare @id int, @Count int
                select @Count = count(1) FROM CT_ManagerLog where UserName=@UserName
                if @TopNum = 1 
                begin
                    set @Count = 2
                end
                if @Count > 1
                begin 
                    select top 1 @id = id from (select top(@TopNum) id from CT_ManagerLog 
                        where UserName=@UserName and ActionType=@ActionType order by id desc) as T  
                    order by id asc
                    select  top 1 id,UserID,UserName,ActionType,Remark,UserIP,AddTime from CT_ManagerLog where id=@id
                end ");

            var para = new DynamicParameters();
            para.Add("@TopNum", top_num);
            para.Add("@UserName", UserName);
            para.Add("@ActionType", ActionType);

            ManagerLogEntity model = base.QuerySingleOrDefaultT(strSql.ToString(), para);
            return model;
        }
        /// <summary>
        /// 删除N天前的日志数据
        /// </summary>
        public int DeletedDay(int dayCount)
        {
            var para = new DynamicParameters();
            para.Add("@dayCount", dayCount, DbType.Int32, null, 4);
            para.Add("@ReturnDelCount", null, DbType.String, ParameterDirection.Output, 100);
            base.Execute("udp_DelManagerLog", para);
            int iResult = para.Get<int>("@ReturnDelCount");
            return iResult;

        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<ManagerLogEntity> QueryListByPage(string strName, string orderby, int pageSize, int pageIndex, ref int recordCount)
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" 1 = @Val ");
            object Paramters = new { Val = 1 };
            if (!string.IsNullOrEmpty(strName.Trim()))
            {
                Where.Append(" and (UserName like  @UserName or ActionType like @ActionType) ");
                Paramters = new { Val = 1, IsDel = false, UserName = string.Format("%{0}%", strName), ActionType = string.Format("%{0}%", strName) };
            }
            recordCount = base.GetIntSingle(string.Format("select count(1) from CT_ManagerLog where {0}", Where.ToString()), Paramters);
            return base.GetListPaged(pageIndex, pageSize, Where.ToString(), orderby, Paramters).ToList();
        }
    }
}
