using CL.Enum.Common;
using CL.Game.DAL;
using CL.Game.Entity;
using CL.Json.Entity.WebAPI;
using CL.Redis.BLL;
using CL.Tools.Common;
using System.Collections.Generic;
using System;

namespace CL.Game.BLL
{
    public class OutETicketsBLL
    {
        OutETicketsDAL dal = new OutETicketsDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 插入变更函数据对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(OutETicketsEntity entity)
        {
            return dal.InsertEntity(entity);
        }

        public List<OutETicketsEntity> QueryOutETickets(int _merchantCode, int _lotteryCode, int _outTicketStauts, string _startTime, string _endTime, int pageSize, int page, ref int recordCount, ref long SumMoney, ref long SumBonus)
        {
            return dal.QueryOutETickets(_merchantCode, _lotteryCode, _outTicketStauts, _startTime, _endTime, pageSize, page, recordCount, ref SumMoney, ref SumBonus);
        }
    }
}
