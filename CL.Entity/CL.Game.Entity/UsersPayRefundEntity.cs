//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//     2017-04-25 20:45:31 Created by LEON
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dapper;
namespace CL.Game.Entity
{

    /// <summary>
    ///UsersPayRefund info
    /// </summary>
    [Table("CT_UsersPayRefund")]
    public class UsersPayRefundEntity
    {

        /// <summary>
        /// 退款ID
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
        /// 处理结果 0.退款处理中 1.退款成功 2.退款失败 3.退款失败,可以重复退款 4.退款需要工人线下干预
        /// </summary>
        public byte Result { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteTime { get; set; }

    }
}