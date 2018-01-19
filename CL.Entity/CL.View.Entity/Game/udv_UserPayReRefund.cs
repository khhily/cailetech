using Dapper;
using System;

namespace CL.View.Entity.Game
{
    [Table("udv_UserPayReRefund")]
    public class udv_UserPayReRefund
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [Key]
        public long ReID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public long PayID { get; set; }

        /// <summary>
        /// 退款单号
        /// </summary>
        public string RefundNo { get; set; }

        /// <summary>
        /// 接口退款交易号
        /// </summary>
        public string RechargeNo { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public int FormalitiesFees { get; set; }

        /// <summary>
        /// 0.退款处理中 1.退款成功 2.退款失败 3.退款失败,可重复退款 4.退款需要工人线下干预
        /// </summary>
        public short Result { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public long PayAmount { get; set; }

        /// <summary>
        /// 交易号
        /// </summary>
        public string PayRechargeNo { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserMobile { get; set; }
    }
}
