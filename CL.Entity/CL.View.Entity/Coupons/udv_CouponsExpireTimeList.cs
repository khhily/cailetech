using System;

namespace CL.View.Entity.Coupons
{
    public class udv_CouponsExpireTimeList
    {

        public long CouponsID { set; get; }
        public DateTime ExpireTime { set; get; }
        public string UserName { set; get; }
        public string PushIdentify { set; get; }

        public long FaceValue { set; get; }
    }
}
