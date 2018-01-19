using CL.Dapper.Repository;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL.View
{
    public class udv_ActivityApplyDAL : DataRepositoryBase<udv_ActivityApply>
    {
        public udv_ActivityApplyDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {

        }
        /// <summary>
        /// 查询视图数据集
        /// </summary>
        /// <param name="ActivityApply"></param>
        /// <returns></returns>
        public List<udv_ActivityApply> QueryEntitys(int ActivityApply)
        {
            return base.GetList(new { ActivityApply = ActivityApply }, "EndTime asc").ToList();
        }
    }
}
