using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;
using System.Collections.Generic;

namespace CL.Game.BLL.View
{
    public class udv_UserPayReRefundBLL
    {
        udv_UserPayReRefundDAL dal = new udv_UserPayReRefundDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public udv_UserPayReRefund GetPayReRefund(long ReID)
        {
            return dal.GetPayReRefund(ReID);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<udv_UserPayReRefund> QueryListByPage(string keywords, int iType, int pageSize, int pageIndex, ref int recordCount)
        {
            return dal.QueryListByPage(keywords, iType, pageSize, pageIndex, ref recordCount);
        }
    }
}
