using CL.Dapper.Repository;
using CL.View.Entity.Game;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL.View
{
    public class udv_OutTicketsDAL : DataRepositoryBase<udv_OutTickets>
    {
        public udv_OutTicketsDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 获取需要投注成功的电子票
        /// 备注：修改本地出票业务逻辑
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_OutTickets> QueryOutTicketList(int LotteryCode)
        {
            return base.GetList(new { TicketStatus = 1, LotteryCode = LotteryCode }).ToList();
        }
    }
}
