using CL.Enum.Common;
using CL.Game.DAL;
using CL.Game.Entity;
using CL.Json.Entity.WebAPI;
using CL.Redis.BLL;
using CL.Tools.Common;
using System.Collections.Generic;

namespace CL.Game.BLL
{
    public class SalePointBLL
    {
        SalePointDAL dal = new SalePointDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 销售返点记录
        /// </summary>
        public List<SalePointEntity> QuerySalePointLst(int ticketSource, int lotteryCode, int salePointStatus, int pageSize, int pageIndex, ref int recordCount)
        {
            return dal.QuerySalePointLst(ticketSource, lotteryCode, salePointStatus, pageSize, pageIndex, ref recordCount);
        }
        /// <summary>
        /// 插入变更函数据对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(SalePointEntity entity)
        {
            return dal.InsertEntity(entity);
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public SalePointEntity QueryEntity(long SalePointID)
        {
            return dal.QueryEntity(SalePointID);
        }
        /// <summary>
        /// 审核对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public int UpadteEntity(SalePointEntity entity)
        {
            return dal.Update(entity);
        }
    }
}
