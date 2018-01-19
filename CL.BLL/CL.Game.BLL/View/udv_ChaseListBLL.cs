using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;
using System.Collections.Generic;

namespace CL.Game.BLL.View
{
    public class udv_ChaseListBLL
    {
        udv_ChaseListDAL dal = new udv_ChaseListDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="iType"></param>
        /// <param name="IsuseName">期号</param>
        /// <param name="UserName">用户名</param>
        /// <param name="SchemeNumber">方案号</param>
        /// <param name="LotteryCode">彩种</param>
        /// <param name="ChaseStatus">追号状态</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="pageSize">当页大小</param>
        /// <param name="pageIndex">当页</param>
        /// <param name="recordCount">共条</param>
        /// <param name="SumMoney">总金额</param>
        /// <returns></returns>
        public List<udv_ChaseList> QueryListByPage(int iType, string IsuseName, string UserName, string SchemeNumber, int LotteryCode, int ChaseStatus, string StartTime, string EndTime, int pageSize, int pageIndex,
            ref int recordCount, ref long SumMoney)
        {
            return dal.QueryListByPage(iType, IsuseName, UserName, SchemeNumber, LotteryCode, ChaseStatus, StartTime, EndTime, pageSize, pageIndex, ref recordCount, ref SumMoney);
        }
    }
}
