using CL.Game.DAL.View;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.BLL.View
{
    public class udv_ActivityApplyBLL
    {
        udv_ActivityApplyDAL dal = new udv_ActivityApplyDAL(Enum.Common.DbConnectionEnum.CaileGame);

        /// <summary>
        /// 查询视图数据集
        /// </summary>
        /// <param name="ActivityApply"></param>
        /// <returns></returns>
        public List<udv_ActivityApply> QueryEntitys(int ActivityApply)
        {
            return dal.QueryEntitys(ActivityApply);
        }



    }
}
