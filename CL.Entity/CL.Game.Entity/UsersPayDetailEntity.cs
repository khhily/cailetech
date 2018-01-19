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
    ///UsersPayDetail info
    /// </summary>
    [Table("CT_UsersPayDetail")]
    public class UsersPayDetailEntity
    {

        /// <summary>
        /// 流水号
        /// </summary>
        [Key]
        public long PayID { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 交易号(接口交易号)
        /// </summary>
        public string RechargeNo { get; set; }

        /// <summary>
        /// 支付类型,如"Alipay","WeChatPay"
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal FormalitiesFees { get; set; }

        /// <summary>
        /// 支付结果 0.未成功 1.已成功 2.已退款 3.退款处理中
        /// </summary>
        public short Result { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 第三方交易号
        /// </summary>
        public string OutRechargeNo { get; set; }
        

    }
}
