using System.Collections.Generic;

namespace CL.Json.Entity.WebAPI
{
    public class ChaseIsuseNumResult : JsonResult
    {
        public List<ChaseIsuseNumEntity> Data { set; get; }
    }
    public class ChaseIsuseNumEntity
    {
        public long IsuseCode { set; get; }
        public string IsuseNum { set; get; }
        public string BeginTime { set; get; }
        public string EndTime { set; get; }

    }
}
