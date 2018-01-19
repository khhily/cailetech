using CL.Dapper.Repository;
using CL.Game.Entity;
using CL.Enum.Common;
using System.Data;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Text;
using CL.Enum.Common.Type;
using System;

namespace CL.Game.DAL
{
    public class SalePointDAL : DataRepositoryBase<SalePointEntity>
    {
        public SalePointDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }


        /// <summary>
        /// 销售返点记录
        /// </summary>
        public List<SalePointEntity> QuerySalePointLst(int ticketSource, int lotteryCode, int salePointStatus, int pageSize, int pageIndex, ref int recordCount)
        {
            var para = new DynamicParameters();
            para.Add("@ticketSource", ticketSource, DbType.Int32, null, 4);
            para.Add("@lotteryCode", lotteryCode, DbType.Int32, null, 4);
            para.Add("@salePointStatus", salePointStatus, DbType.Int32, null, 4);
            para.Add("@pageSize", pageSize, DbType.Int32, null, 4);
            para.Add("@pageIndex", pageIndex, DbType.Int32, null, 4);
            para.Add("@recordCount", null, DbType.Int32, ParameterDirection.Output, 4);

            SqlMapper.GridReader grid = base.QueryMultiple("udp_SalePointLst", para, CommandType.StoredProcedure);

            List<SalePointEntity> list = grid.Read<SalePointEntity>().ToList();
            recordCount = para.Get<int>("@recordCount");
            grid.Dispose();
            return list;
        }
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(SalePointEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public SalePointEntity QueryEntity(long SalePointID)
        {
            return base.Get(SalePointID);
        }
    }
}
