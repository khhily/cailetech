﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//     2017-09-08 16:20:36 Created by LEON
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dapper;
namespace CL.Game.Entity
{

    /// <summary>
    ///SalePointRecord info
    /// </summary>
    [Table("CT_SalePointRecord")]
    public class SalePointRecordEntity
    {

        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long ID { get; set; }

        /// <summary>
        /// 出票平台：0彩乐:1华阳
        /// </summary>
        public int TicketSourceID { get; set; }

        /// <summary>
        /// 彩票编码
        /// </summary>
        public int LotteryCode { get; set; }

        /// <summary>
        /// 销售阶梯及点位的字符串拼接
        /// </summary>
        public string SalesRebate { get; set; }

        /// <summary>
        /// 上一次的销售阶梯及点位的字符串拼接，如果有
        /// </summary>
        public string LastSalesRebate { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
