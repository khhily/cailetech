
namespace CL.View.Entity.Other
{
    public class udv_JsonResult<T> where T : new()
    {
        public int Code { set; get; }
        public string Msg { set; get; }
        public T Data { set; get; }
    }
}
