//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//     2017-09-08 15:26:15 Created by LEON
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dapper;

namespace CL.Game.Entity
{

    /// <summary>
    /// 投注金额上限加奖
    /// </summary>
    [Table("CT_RegularTopLimit")]
    public class RegularTopLimitEntity
    {

        /// <summary>
        /// 标识列
        /// </summary>
        [Key]
        public int RTopLimitID { get; set; }

        /// <summary>
        /// 加奖规则标识
        /// </summary>
        public int RegularID { get; set; }

        /// <summary>
        /// 累计金额
        /// </summary>
        public long TotalMoney { get; set; }

        /// <summary>
        /// 加奖金额
        /// </summary>
        public long AwardMoney { get; set; }

        /// <summary>
        /// 加奖玩法
        /// </summary>
        public int PlayCode { get; set; }

    }
}