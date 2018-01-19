using System;
using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;
using System.Collections.Generic;

namespace CL.Game.BLL.View
{
    public class udv_SchemeChaseTaskDetailBLL
    {
        udv_SchemeChaseTaskDetailDAL dal = new udv_SchemeChaseTaskDetailDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public udv_SchemeChaseTaskDetail QueryEntitysBySchemeID(long SchemeID)
        {
            return dal.QueryEntitysBySchemeID(SchemeID);
        }

        public List<udv_SchemeChaseTaskDetail> QueryListByPage(long SchemeID, string orderby, int PageIndex, int PageSize, ref int RecordCount)
        {
            return dal.QueryListByPage(SchemeID, orderby, PageIndex, PageSize, ref RecordCount);
        }
    }
}
