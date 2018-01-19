using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;
using System.Collections.Generic;

namespace CL.Game.BLL.View
{
    public class udv_ChaseRevokeBLL
    {
        udv_ChaseRevokeDAL dal = new udv_ChaseRevokeDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 查询需要撤销的追号数据
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_ChaseRevoke> QueryRevoke(int LotteryCode)
        {
            return dal.QueryRevoke(LotteryCode);
        }
    }
}
