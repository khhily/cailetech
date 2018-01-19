using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;
using System.Collections.Generic;

namespace CL.Game.BLL.View
{
    public class udv_SchemesDetailBLL
    {
        udv_SchemesDetailDAL dal = new udv_SchemesDetailDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 获取方案明细数据
        /// </summary>
        /// <returns></returns>
        public List<udv_SchemesDetail> QueryEntitysBySchemeID(long SchemeID)
        {
            return dal.QueryEntitysBySchemeID(SchemeID);
        }
    }
}
