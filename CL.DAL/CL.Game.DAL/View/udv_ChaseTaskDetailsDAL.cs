using CL.Dapper.Repository;
using CL.View.Entity.Game;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL.View
{
    public class udv_ChaseTaskDetailsDAL : DataRepositoryBase<udv_ChaseTaskDetails>
    {
        public udv_ChaseTaskDetailsDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 查询当前彩种的追号任务
        /// </summary>
        /// <param name="LotteryCode">彩种编码</param>
        /// <returns></returns>
        public List<udv_ChaseTaskDetails> QueryModelList(int LotteryCode)
        {
            return base.GetList(new { LotteryCode = LotteryCode }, "Amount desc").ToList();
        }
    }
}
