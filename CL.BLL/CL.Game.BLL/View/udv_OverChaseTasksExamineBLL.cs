using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;
using System.Collections.Generic;

namespace CL.Game.BLL.View
{
    public class udv_OverChaseTasksExamineBLL
    {
        udv_OverChaseTasksExamineDAL dal = new udv_OverChaseTasksExamineDAL(DbConnectionEnum.CaileGame);
        /// <summary>
        /// 查询未完成追号信息(便于结束追号)
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_OverChaseTasksExamine> QueryModelList(int LotteryCode)
        {
            return dal.QueryModelList(LotteryCode);
        }
    }
}
