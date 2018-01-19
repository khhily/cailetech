using Dapper;

namespace CL.View.Entity.Game
{
    /// <summary>
    /// view udv_PlayTypes
    /// </summary>
    [Table("udv_PlayTypes")]
    public class udv_PlayTypes
    {

        /// <summary>
        /// 玩法编号
        /// </summary>
        [Key]
        public int PlayID { get; set; }

        /// <summary>
        /// 玩法编号
        /// </summary>
        public int PlayCode { get; set; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { get; set; }

        /// <summary>
        /// 玩法名称
        /// </summary>
        public string PlayName { get; set; }

        /// <summary>
        /// 单注金额
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 能投注的最大倍数
        /// </summary>
        public int MaxMultiple { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }

        public string LotteryName { get; set; }
    }
}
