using CL.Dapper.Repository;
using CL.View.Entity.Game;
using CL.Enum.Common;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace CL.Game.DAL.View
{
    public class udv_ChaseRevokeDAL : DataRepositoryBase<udv_ChaseRevoke>
    {
        public udv_ChaseRevokeDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 查询需要撤销的追号数据
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_ChaseRevoke> QueryRevoke(int LotteryCode)
        {
            return base.GetList(new { LotteryCode = LotteryCode }).ToList();
        }
    }
}
