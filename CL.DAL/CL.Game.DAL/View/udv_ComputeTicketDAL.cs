using CL.Dapper.Repository;
using CL.View.Entity.Game;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL.View
{
    public class udv_ComputeTicketDAL : DataRepositoryBase<udv_ComputeTicket>
    {
        public udv_ComputeTicketDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 获取需要算奖的电子票
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_ComputeTicket> QueryComputeTicketList(int LotteryCode)
        {
            object Paramters = new { LotteryCode = LotteryCode, TicketStatus = 2, IsOpened = 1, IsuseState = 4 };
            return base.GetList(Paramters).ToList();
        }
    }
}
