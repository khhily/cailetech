using System.Collections.Generic;

namespace CL.Json.Entity.WebAPI
{
    public class BankCardsResult : JsonResult
    {
        public List<BankCardEntity> Data { set; get; }
    }
    public class BankCardEntity
    {
        public long BankCode { set; get; }
        public string CardNum { set; get; }
        public string BankName { set; get; }
        public string BankArea { set; get; }
    }
}
