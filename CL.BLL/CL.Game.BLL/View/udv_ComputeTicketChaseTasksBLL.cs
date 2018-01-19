using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;
using System.Collections.Generic;

namespace CL.Game.BLL.View
{
    public class udv_ComputeTicketChaseTasksBLL
    {
        udv_ComputeTicketChaseTasksDAL dal = new udv_ComputeTicketChaseTasksDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 获取需要算奖的电子票
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_ComputeTicketChaseTasks> QueryComputeTicketList(int LotteryCode)
        {
            return dal.QueryComputeTicketList(LotteryCode);
        }

    }
}
