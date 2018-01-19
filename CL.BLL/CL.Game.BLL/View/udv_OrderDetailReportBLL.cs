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
    public class udv_OrderDetailReportBLL
    {
        udv_OrderDetailReportDAL dal = new udv_OrderDetailReportDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="lotteryCode">彩票编码</param>
        /// <param name="schemeStatus">方案状态</param>
        /// <param name="schemeNumber">方案号</param>
        /// <param name="userName">用户昵称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <param name="PageIndex">当期页</param>
        /// <param name="PageSize">每页大小</param>
        /// <param name="RecordCount">共多少条记录</param>
        /// <returns></returns>
        public List<udv_OrderDetailReport> QueryModeListByPages(int lotteryCode, int schemeStatus, string schemeNumber, string userName, string mobile, string startTime, string endTime, string orderBy, int pageIndex, int pageSize, ref int recordCount)
        {
            return dal.QueryModeListByPages(lotteryCode, schemeStatus, schemeNumber, userName, mobile, startTime, endTime, orderBy, pageIndex, pageSize, ref recordCount);
        }
    }
}
