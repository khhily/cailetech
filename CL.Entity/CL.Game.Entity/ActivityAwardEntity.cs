//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//     2017-09-08 15:26:14 Created by LEON
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dapper;

namespace CL.Game.Entity
{

    /// <summary>
    /// 活动加奖规则表
    /// </summary>
    [Table("CT_ActivityAward")]
    public class ActivityAwardEntity
    {

        /// <summary>
        /// 加奖规则ID
        /// </summary>
        [Key]
        public int RegularID { get; set; }

        /// <summary>
        /// 活动标识
        /// </summary>
        public int ActivityID { get; set; }

        /// <summary>
        /// 规则类型：
        /// 0 标准玩法加奖，1 追号加奖，2 胆拖玩法加奖，3 球队加奖，
        /// 4 串关加奖，5 投注金额累计区间加奖，6 中奖金额区间加奖，
        /// 7 活动期间投注金额累计名次加奖，8 活动期间中奖金额累计名次加奖，
        /// 9 数字彩中球加奖，10 节假日加奖
        /// </summary>
        public int RegularType { get; set; }

        /// <summary>
        /// 活动的彩种
        /// </summary>
        public int LotteryCode { get; set; }

        /// <summary>
        /// 累计已加奖金额
        /// </summary>
        public long TotalAwardMoney { get; set; }

        /// <summary>
        /// 规则状态：0 初始化规则，1 规则作废(活动审核失败)，2 规则开始并生效(活动审核通过)，
        /// 3 活动截止并开始加奖(这里加奖针对活动期间累计加奖规则)，
        /// 4 活动结束并销毁(所有加奖派发完成后结束和销毁活动，销毁后的活动规则无法直接启用，启用销毁的规则需要重置及走审核流程)
        /// </summary>
        public int RegularStatus { get; set; }

    }
}
