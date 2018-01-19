
namespace CL.Json.Entity.WebAPI
{
    public class CurrentIsuseInfoResult : JsonResult
    {
        public CurrentIsuseInfoEntity Data { set; get; }
    }
    public class CurrentIsuseInfoEntity
    {
        public string IsuseNum { set; get; }
        public int TotalIsuse { set; get; }
        public string BeginTime { set; get; }
        public string EndTime { set; get; }
        public string ServerTime { set; get; }
    }
}
