//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//     2017-04-25 20:45:31 Created by LEON
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Dapper;
namespace CL.Game.Entity
{

    /// <summary>
    ///IsuseBonuses info
    /// </summary>
    [Table("CT_IsuseBonuses")]
    public class IsuseBonusesEntity
    {

        /// <summary>
        /// 自增ID
        /// </summary>
        [Key]
        public long ID { get; set; }

        /// <summary>
        /// 管理员ID
        /// </summary>
        public int AdminID { get; set; }

        /// <summary>
        /// 期号ID
        /// </summary>
        public long IsuseID { get; set; }

        /// <summary>
        /// 原奖金
        /// </summary>
        public int defaultMoney { get; set; }

        /// <summary>
        /// 税后奖金
        /// </summary>
        public int DefaultMoneyNoWithTax { get; set; }

        /// <summary>
        /// 开奖号码
        /// </summary>
        public string WinNumber { get; set; }

        /// <summary>
        /// 中奖注数
        /// </summary>
        public string WinBet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WinLevel { get; set; }

    }
}
