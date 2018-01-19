using Dapper;
using System;

namespace CL.Coupons.Entity
{
    /// <summary>
    /// 优惠券基础表
    /// </summary>
    [Table("CT_Coupons")]
    public class CouponsEntity
    {
        /// <summary>
        /// 优惠券基础标识
        /// </summary>
        [Key]
        public long CouponsID { set; get; }

        /// <summary>
        /// 活动表标识
        /// </summary>
        public int ActivityID { set; get; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime GenerateTime { set; get; }

        /// <summary>
        /// 发放时间
        /// </summary>
        public DateTime? ReleaseTime { set; get; }

        /// <summary>
        /// 第一次使用时间
        /// </summary>
        public DateTime? FirstTime { set; get; }

        /// <summary>
        /// 最后一次使用时间
        /// </summary>
        public DateTime? LastTime { set; get; }

        /// <summary>
        /// 优惠券状态：0未发放，1已发放，2未使用，3已使用
        /// </summary>
        public int CouponsStatus { set; get; }

        /// <summary>
        /// 优惠券类型：0固定时间段，1固定时长，2满减，3永不过期
        /// </summary>
        public int CouponsType { set; get; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { set; get; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpireTime { set; get; }

        /// <summary>
        /// 面值
        /// </summary>
        public long FaceValue { set; get; }

        /// <summary>
        /// 余额
        /// </summary>
        public long Balance { set; get; }

        /// <summary>
        /// 满减金额
        /// </summary>
        public long SatisfiedMoney { set; get; }

        /// <summary>
        /// 购彩彩种限制：当前值为0则满足所有彩种要求，如果当前有值，表示当前彩券只允许投注指定彩种
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 是否允许赠送
        /// </summary>
        public bool IsGive { set; get; }

        /// <summary>
        /// 是否允许追号
        /// </summary>
        public bool IsChaseTask { set; get; }

        /// <summary>
        /// 是否允许叠加使用
        /// </summary>
        public bool IsSuperposition { set; get; }

        /// <summary>
        /// 是否多次使用
        /// </summary>
        public bool IsTimes { set; get; }

        /// <summary>
        /// 是否允许合买
        /// </summary>
        public bool IsJoinBuy { set; get; }

        /// <summary>
        /// 拥有者
        /// </summary>
        public long UserID { set; get; }

        /// <summary>
        ///  优惠券来源：1 平台活动发放，2 平台赠送，3 任务获得,游戏获得(例如轮盘游戏,玩游戏时有一定机率获得彩券)，4 线下推广二维码
        /// </summary>
        public int CouponsSource { set; get; }

    }
}
