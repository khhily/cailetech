using CL.Coupons.Entity;

namespace CL.View.Entity.Coupons
{
    public class udv_CouponsList : CouponsEntity
    {
        /// <summary>
        /// 活动标题
        /// </summary>
        public string ActivitySubject { set; get; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 用户手机号码
        /// </summary>
        public string UserMobile { set; get; }
    }
}
