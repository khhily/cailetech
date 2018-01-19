using CL.Dapper.Repository;
using CL.View.Entity.Game;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL.View
{
    public class udv_BettingTicketsDAL : DataRepositoryBase<udv_BettingTickets>
    {
        public udv_BettingTicketsDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }



    }
}
