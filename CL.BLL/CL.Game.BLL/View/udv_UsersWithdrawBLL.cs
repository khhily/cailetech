using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.BLL.View
{
    public class udv_UsersWithdrawBLL
    {
        udv_UsersWithdrawDAL dal = new udv_UsersWithdrawDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="whereStr">查询条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="recordCount">共多少条</param>
        /// <returns></returns>
        public List<udv_UsersWithdraw> QueryListPageByFullName(int pageIndex, int pageSize, int PayOutStatus, string FullName, ref int recordCount)
        {
            StringBuilder Where = new StringBuilder();
            StringBuilder OrderBy = new StringBuilder();
            Where.Append(" PayOutStatus=@PayOutStatus ");
            object Paramters = new { PayOutStatus = PayOutStatus };
            if (!string.IsNullOrEmpty(FullName.Trim()))
            {
                Where.Append(" AND FullName like @FullName");
                Paramters = new { PayOutStatus = PayOutStatus, FullName = string.Format("%{0}%", FullName) };
            }
            OrderBy.Append(" CreateTime desc,PayOutID desc ");
            return dal.QueryListByPage(pageIndex, pageSize, Where.ToString(), Paramters, OrderBy.ToString(), ref recordCount);
        }
    }
}
