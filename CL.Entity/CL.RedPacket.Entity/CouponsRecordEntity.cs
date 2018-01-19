using Dapper;
using System;

namespace CL.Coupons.Entity
{
    /// <summary>
    /// 优惠券记录表
    /// </summary>
    [Table("CT_CouponsRecord")]
    public class CouponsRecordEntity
    {
        /// <summary>
        /// 表标识
        /// </summary>
        [Key]
        public long RecordID { set; get; }

        /// <summary>
        /// 优惠券基础表标识
        /// </summary>
        public long CouponsID { set; get; }

        /// <summary>
        /// 日志类型：0 消费日志，1 会员领取日志，2 赠送者(赠送彩券给别人的人)
        /// </summary>
        public int LogType { set; get; }

        /// <summary>
        /// 日志生成时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 关联用户
        /// </summary>
        public long UserID { set; get; }

        /// <summary>
        /// 关联订单
        /// </summary>
        public string RelationID { set; get; }
    }
}
