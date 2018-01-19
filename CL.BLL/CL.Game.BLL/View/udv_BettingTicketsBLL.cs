using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;
using System.Collections.Generic;
using System.Linq;

namespace CL.Game.BLL.View
{
    public class udv_BettingTicketsBLL
    {
        udv_BettingTicketsDAL dal = new udv_BettingTicketsDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 获取电子票明细数据
        /// </summary>
        /// <returns></returns>
        public List<udv_BettingTickets> QueryDataList(long SchemeID, long ChaseTaskDetailsID)
        {
            return dal.GetList(new { SchemeID = SchemeID, TicketStatus = 0, ChaseTaskDetailsID = ChaseTaskDetailsID }).ToList();
        }
    }
}
