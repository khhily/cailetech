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
    public class SalePointRecordDAL : DataRepositoryBase<SalePointRecordEntity>
    {
        public SalePointRecordDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {

        }

        /// <summary>
        /// 销售点位明细
        /// </summary>
        /// <param name="_ticketSource"></param>
        /// <param name="_lotteryCode"></param>
        /// <param name="_startTime"></param>
        /// <param name="_endTime"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<SalePointRecordEntity> QuerySalePointRecord(int ticketSource, int lotteryCode, string startTime, string endTime, int pageSize, int pageIndex, ref int recordCount)
        {
            var para = new DynamicParameters();
            para.Add("@ticketSource", ticketSource, DbType.Int32, null, 4);
            para.Add("@lotteryCode", lotteryCode, DbType.Int32, null, 4);
            para.Add("@startTime", startTime, DbType.DateTime, null, 10);
            para.Add("@endTime", endTime, DbType.DateTime, null, 10);
            para.Add("@pageSize", pageSize, DbType.Int32, null, 4);
            para.Add("@pageIndex", pageIndex, DbType.Int32, null, 4);
            para.Add("@recordCount", null, DbType.Int32, ParameterDirection.Output, 4);

            SqlMapper.GridReader grid = base.QueryMultiple("udp_SalePointRecordLst", para, CommandType.StoredProcedure);

            List<SalePointRecordEntity> list = grid.Read<SalePointRecordEntity>().ToList();
            recordCount = para.Get<int>("@recordCount");
            grid.Dispose();
            return list;
        }

        /// <summary>
        /// 插入销售返点数据
        /// </summary>
        public void AddSalePointRecord(int ticketSource, long lotteryCode, string salesRebate, string startTime, ref int ReturnValue, ref string ReturnDescription)
        {
            var para = new DynamicParameters();
            para.Add("@TicketSource", ticketSource, DbType.Int32, null, 4);
            para.Add("@LotteryCode", lotteryCode, DbType.Int64, null, 4);
            para.Add("@SalesRebate", salesRebate, DbType.String, null, 500);
            para.Add("@StartTime", startTime, DbType.DateTime, null, 10);
            para.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            para.Add("@ReturnDescription", null, DbType.String, ParameterDirection.Output, 100);

            base.Execute("udp_AddSalePointRecord", para);
            ReturnValue = para.Get<int>("@ReturnValue");
            ReturnDescription = para.Get<string>("@ReturnDescription");
        }
    }
}
