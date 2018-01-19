
namespace CL.Json.Entity.WebAPI
{
    public class PreparePaymentResult : JsonResult
    {
        public PreparePaymentEnttiy_WFT Data { set; get; }
    }
    public class PreparePaymentEnttiy_WFT
    {
        public string TokenID { set; get; }
        public string OrderNo { set; get; }
        public string PayLink { get; set; }
    }
}
