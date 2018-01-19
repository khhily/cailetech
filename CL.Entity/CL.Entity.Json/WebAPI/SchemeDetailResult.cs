
namespace CL.Json.Entity.WebAPI
{
    public class SchemeDetailResult : JsonResult
    {
        public SchemeDetailEntity Data { set; get; }
    }
    public class SchemeDetailEntity
    {
        public string OrderNum { set; get; }
        public string OrderTime { set; get; }
        public int LotteryCode { set; get; }
        public string LotteryName { set; get; }
        public long Amount { set; get; }
        public int OrderStatus { set; get; }
        public long WinMoney { set; get; }
        public string Number { set; get; }
        public string IsuseNum { set; get; }
        public string OpenNumber { set; get; }
        public string RoomID { set; get; }
    }
}
