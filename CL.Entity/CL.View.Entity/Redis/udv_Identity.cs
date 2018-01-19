
namespace CL.View.Entity.Redis
{
    public class udv_Identity
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        public long UserCode { set; get; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string FullName { set; get; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCardNo { set; get; }
    }
}
