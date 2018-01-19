
namespace CL.View.Entity.Coupons
{
    public class udv_ReportCouponsList
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string DTime { set; get; }

        /// <summary>
        /// 发放彩券个数
        /// </summary>
        public int ReleaseCount { set; get; }

        /// <summary>
        /// 发放彩券金额
        /// </summary>
        public long ReleaseMoney { set; get; }

        /// <summary>
        /// 过期彩券个数
        /// </summary>
        public int ExpireCount { set; get; }

        /// <summary>
        /// 过期彩券金额
        /// </summary>
        public long ExpireAmount { set; get; }

        /// <summary>
        /// 生成彩券金额
        /// </summary>
        public long Amount { set; get; }

        /// <summary>
        /// 固定时长彩券数
        /// </summary>
        public int GDTimeCount { set; get; }

        /// <summary>
        /// 固定时长彩券金额
        /// </summary>
        public long GDTimeAmount { set; get; }

        /// <summary>
        /// 固定长度彩券数
        /// </summary>
        public int GDCount { set; get; }

        /// <summary>
        /// 固定长度彩券金额
        /// </summary>
        public long GDAmount { set; get; }

        /// <summary>
        /// 满减彩券数
        /// </summary>
        public int MJCount { set; get; }

        /// <summary>
        /// 满减彩券金额
        /// </summary>
        public long MJAmount { set; get; }

        /// <summary>
        /// 永不过期彩券数
        /// </summary>
        public int NoExpireCount { set; get; }

        /// <summary>
        /// 永不过期彩券金额
        /// </summary>
        public long NoExpireAmount { set; get; }
    }
}
