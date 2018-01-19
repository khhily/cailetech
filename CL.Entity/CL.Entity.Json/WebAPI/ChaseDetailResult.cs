
namespace CL.Json.Entity.WebAPI
{
    public class ChaseDetailResult : JsonResult
    {
        public ChaseDetailEntity Data { set; get; }
    }
    public class ChaseDetailEntity
    {
        public string OrderNum { set; get; }
        public string OrderTime { set; get; }
        public int LotteryCode { set; get; }
        public long Amount { set; get; }
        public int OrderStatus { set; get; }
        public string Number { set; get; }
        public long WinMoney { set; get; }
        public string IsuseNum { set; get; }
        public string OpenNumber { set; get; }
        public bool IsOpened { set; get; }
    }
}
