using System;
using System.Collections.Generic;

namespace CL.Json.Entity.WebAPI
{
    public class OfLotteryDetailResult : JsonResult
    {
        public OfLotteryDetailEntity Data { set; get; }
    }
    public class OfLotteryDetailEntity
    {
        /// <summary>
        /// 开奖账号
        /// </summary>
        public int OpenLotUser { set; get; }
        public int LotteryCode { set; get; }
        public string IsuseNum { set; get; }
        public string OpenNumber { set; get; }
        public string OpenTime { set; get; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { set; get; }
        public long WinRollover { set; get; }
        public long TotalSales { set; get; }
        public string Isusebonuses { set; get; }
        public List<DataAwardEntity> DataAward { set; get; }
    }
    public class DataAwardEntity
    {
        public long DefaultMoney { set; get; }
        public int WinBet { set; get; }
        public string WinLevel { set; get; }
    }
}
