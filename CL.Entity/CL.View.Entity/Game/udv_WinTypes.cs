using Dapper;

namespace CL.View.Entity.Game
{
    [Table("udv_WinTypes")]
    public class udv_WinTypes
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int WinID { get; set; }

        /// <summary>
        /// 奖等编码
        /// </summary>
        public int WinCode { get; set; }

        /// <summary>
        /// 彩种编码
        /// </summary>
        public int? LotteryCode { get; set; }

        /// <summary>
        /// 奖等名称
        /// </summary>
        public string WinName { get; set; }

        /// <summary>
        /// 中奖号码，列出所有符合中奖的号码
        /// </summary>
        public string WinNumber { get; set; }

        /// <summary>
        /// 是否和值
        /// </summary>
        public bool IsSumValue { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 默认奖金
        /// </summary>
        public int? DefaultMoney { get; set; }

        /// <summary>
        /// 默认税后奖金
        /// </summary>
        public int? DefaultMoneyNoWithTax { get; set; }
        public string LotteryName { get; set; }
    }
}
