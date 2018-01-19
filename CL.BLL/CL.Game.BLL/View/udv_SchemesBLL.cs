using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.View.Entity.Game;

namespace CL.Game.BLL.View
{
    public class udv_SchemesBLL
    {
        udv_SchemesDAL dal = new udv_SchemesDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public udv_Schemes QueryEntity(long SchemeID)
        {
            return dal.QueryEntity(SchemeID);
        }
    }
}
