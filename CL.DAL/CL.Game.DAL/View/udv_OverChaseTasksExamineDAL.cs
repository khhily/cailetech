using CL.Dapper.Repository;
using CL.View.Entity.Game;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL.View
{
    public class udv_OverChaseTasksExamineDAL : DataRepositoryBase<udv_OverChaseTasksExamine>
    {
        public udv_OverChaseTasksExamineDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 查询未完成追号信息(便于结束追号)
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_OverChaseTasksExamine> QueryModelList(int LotteryCode)
        {
            return base.GetList(new { LotteryCode = LotteryCode }, "SchemeID desc").ToList();
        }

    }
}
