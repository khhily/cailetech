using System.Collections.Generic;

namespace CL.Json.Entity.WebAPI
{
    public class OfLotteryResult : JsonResult
    {
        public List<OfLotteryEntity> Data { set; get; }
    }
    public class OfLotteryEntity
    {
        public int LotteryCode { set; get; }
        public string IsuseNum { set; get; }
        public string Number { set; get; }
        public string OpenTime { set; get; }
    }
}
