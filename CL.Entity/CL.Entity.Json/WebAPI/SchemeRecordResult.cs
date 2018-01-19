using System;
using System.Collections.Generic;

namespace CL.Json.Entity.WebAPI
{
    public class SchemeRecordResult : JsonResult
    {
        public List<SchemeRecordEntity> Data { set; get; }
    }
    public class SchemeRecordEntity
    {
        public long OrderCode { set; get; }
        public string OrderTime { set; get; }
        public string LotteryName { set; get; }
        public string IsuseNum { set; get; }
        public long Amount { set; get; }
        public int OrderStatus { set; get; }
        public short BuyType { set; get; }
        public long WinMoney { set; get; }
    }
    public class udv_SchemeRecordEntity
    {
        public long OrderCode { set; get; }
        public DateTime OrderTime { set; get; }
        public string LotteryName { set; get; }
        public string IsuseNum { set; get; }
        public long Amount { set; get; }
        public int OrderStatus { set; get; }
        public short BuyType { set; get; }
        public long WinMoney { set; get; }
    }
}
