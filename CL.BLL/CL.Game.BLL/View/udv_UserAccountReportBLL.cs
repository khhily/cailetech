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
    public class udv_UserAccountReportBLL
    {
        udv_UserAccountReportDAL dal = new udv_UserAccountReportDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public List<udv_UserAccountReport> QueryModeListByPages(string userName, string mobile, string startTime, string endTime, string orderBy, int pageIndex, int pageSize, ref int totalCount)
        {
            return dal.QueryModeListByPages(userName, mobile, startTime, endTime, orderBy, pageIndex, pageSize, ref totalCount);
        }
    }
}
