using System;
using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;

namespace CL.Game.BLL.View
{
    public class udv_SchemeChaseTaskBLL
    {
        udv_SchemeChaseTaskDAL dal = new udv_SchemeChaseTaskDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public udv_SchemeChaseTask QueryEntity(long SchemeID)
        {
            return dal.QueryEntity(SchemeID);
        }
    }
}
