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
    public class SalePointRecordBLL
    {
        SalePointRecordDAL dal = new SalePointRecordDAL(DbConnectionEnum.CaileGame);

        public void AddSalePointRecord(int ticketSource, long lotteryCode, string salesRebate, string startTime, ref int ReturnValue, ref string ReturnDescription)
        {
            dal.AddSalePointRecord(ticketSource, lotteryCode, salesRebate, startTime, ref ReturnValue, ref ReturnDescription);
        }

        public List<SalePointRecordEntity> QuerySalePointRecord(int _ticketSource, int _lotteryCode, string _startTime, string _endTime, int pageSize, int page, ref int recordCount)
        {
            return dal.QuerySalePointRecord(_ticketSource, _lotteryCode, _startTime, _endTime, pageSize, page, ref recordCount);
        }
    }
}
