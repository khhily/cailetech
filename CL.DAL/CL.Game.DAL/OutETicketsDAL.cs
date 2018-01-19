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
    public class OutETicketsDAL : DataRepositoryBase<OutETicketsEntity>
    {
        public OutETicketsDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(OutETicketsEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }

        public List<OutETicketsEntity> QueryOutETickets(int _merchantCode, int _lotteryCode, int _outTicketStauts, string _startTime, string _endTime, int pageSize, int page, int recordCount, ref long SumMoney, ref long SumBonus)
        {
            var para = new DynamicParameters();
            para.Add("@merchantCode", _merchantCode, DbType.Int32, null, 4);
            para.Add("@lotteryCode", _lotteryCode, DbType.Int32, null, 4);
            para.Add("@outTicketStauts", _outTicketStauts, DbType.Int32, null, 4);
            para.Add("@startTime", _startTime, DbType.DateTime, null, 10);
            para.Add("@endTime", _endTime, DbType.DateTime, null, 10);
            para.Add("@pageSize", pageSize, DbType.Int32, null, 4);
            para.Add("@pageIndex", page, DbType.Int32, null, 4);
            para.Add("@recordCount", null, DbType.Int32, ParameterDirection.Output, 4);
            para.Add("@SumMoney", null, DbType.Int64, ParameterDirection.Output, 8);
            para.Add("@SumBonus", null, DbType.Int64, ParameterDirection.Output, 8);

            SqlMapper.GridReader grid = base.QueryMultiple("udp_OutETicketsLst", para, CommandType.StoredProcedure);

            List<OutETicketsEntity> list = grid.Read<OutETicketsEntity>().ToList();
            recordCount = para.Get<int>("@recordCount");
            SumMoney = para.Get<long>("@SumMoney");
            SumBonus = para.Get<long>("@SumBonus");
            grid.Dispose();
            return list;
        }
    }
}
