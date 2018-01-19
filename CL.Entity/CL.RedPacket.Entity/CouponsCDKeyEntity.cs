using Dapper;
using System;

namespace CL.Coupons.Entity
{
    /// <summary>
    /// 优惠券兑换码表
    /// </summary>
    [Table("CT_CouponsCDKey")]
    public class CouponsCDKeyEntity
    {
        /// <summary>
        /// 优惠券兑换码标识
        /// </summary>
        [Key]
        public long CDKeyID { set; get; }

        /// <summary>
        /// 优惠券基础表标识
        /// </summary>
        public long CouponsID { set; get; }

        /// <summary>
        /// 合作方代码
        /// </summary>
        public string PartnerCode { set; get; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime GenerateTime { set; get; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { set; get; }

        /// <summary>
        /// 优惠券兑换码
        /// </summary>
        public string CDKey { set; get; }

        /// <summary>
        /// 加密的兑换码
        /// </summary>
        public string EncryptKey { set; get; }

        /// <summary>
        /// 是否已兑换
        /// </summary>
        public bool IsExchanger { set; get; }

        /// <summary>
        /// 兑换用户
        /// </summary>
        public long ExchangerUserID { set; get; }

        /// <summary>
        /// 兑换时间
        /// </summary>
        public DateTime? ExchangerTime { set; get; }
    }
}
