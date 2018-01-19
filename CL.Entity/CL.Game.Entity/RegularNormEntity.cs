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
    /// 标准玩法加奖
    /// </summary>
    [Table("CT_RegularNorm")]
    public class RegularNormEntity
    {

        /// <summary>
        /// 标识列
        /// </summary>
        [Key]
        public int RNormID { get; set; }

        /// <summary>
        /// 加奖规则标识
        /// </summary>
        public int RegularID { get; set; }

        /// <summary>
        /// 加奖玩法
        /// </summary>
        public int PlayCode { get; set; }

        /// <summary>
        /// 单次加奖金额
        /// </summary>
        public long AwardMoney { get; set; }

        /// <summary>
        /// 单用户累计中奖上线金额：0 表示没有限制，大于0表示累计上限多少停止单用户加奖
        /// </summary>
        public long TopLimit { get; set; }

    }
}
