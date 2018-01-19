using CL.Dapper.Repository;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL.View
{
    public class udv_UsersWithdrawDAL : DataRepositoryBase<udv_UsersWithdraw>
    {
        public udv_UsersWithdrawDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="whereStr">查询条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="recordCount">共多少条</param>
        /// <returns></returns>
        public List<udv_UsersWithdraw> QueryListByPage(int pageIndex, int pageSize, string Where, object Paramters, string orderby, ref int recordCount)
        {
            recordCount = base.GetIntSingle(string.Format("select count(1) from udv_UsersWithdraw where {0}", Where), Paramters);
            return base.GetListPaged(pageIndex, pageSize, Where.ToString(), orderby, Paramters).ToList();
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="whereStr">查询条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="recordCount">共多少条</param>
        /// <returns></returns>
        public List<udv_UsersWithdraw> QueryListByPage(int pageIndex, int pageSize, string Where, string orderby, ref int recordCount)
        {
            recordCount = base.GetIntSingle(string.Format("select count(1) from udv_UsersWithdraw where {0}", Where));
            return base.GetListPaged(pageIndex, pageSize, Where.ToString(), orderby).ToList();
        }
    }
}
