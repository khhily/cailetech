using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;
using System.Collections.Generic;

namespace CL.Game.BLL.View
{
    public class udv_OutTicketsBLL
    {
        udv_OutTicketsDAL dal = new udv_OutTicketsDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 获取需要投注成功的电子票
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_OutTickets> QueryOutTicketList(int LotteryCode)
        {
            return dal.QueryOutTicketList(LotteryCode);
        }

    }
}
