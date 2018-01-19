using CL.Enum.Common;
using CL.Game.DAL;
using CL.Game.Entity;
using CL.Json.Entity.WebAPI;
using CL.Redis.BLL;
using CL.Tools.Common;
using System.Collections.Generic;

namespace CL.Game.BLL
{
    public class SalePointFileBLL
    {
        SalePointFileDAL dal = new SalePointFileDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 插入变更函数据对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(SalePointFileEntity entity)
        {
            return dal.InsertEntity(entity);
        }
       
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<SalePointFileEntity> QueryEntitys(string strWhere)
        {
            return dal.QueryEntitys(strWhere);
        }
    }
}
