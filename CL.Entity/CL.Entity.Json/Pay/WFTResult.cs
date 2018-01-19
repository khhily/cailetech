using CL.Json.Entity;

namespace CL.Entity.Json.Pay
{
    public class WFTResult : JsonResult
    {
        /// <summary>
        /// 威富通的参数
        /// </summary>
        public string TokenId { set; get; }
        /// <summary>
        /// 本地参数(订单号)
        /// </summary>
        public string OrderNo { set; get; }
    }
}
