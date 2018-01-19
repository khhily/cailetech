using Dapper;

namespace CL.View.Entity.Game
{
    [Table("udv_SchemesDetail")]
    public class udv_SchemesDetail
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [Key]
        public long SDID { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public long SchemeID { get; set; }

        /// <summary>
        /// 玩法编码
        /// </summary>
        public int PlayCode { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { get; set; }

        /// <summary>
        /// 注数
        /// </summary>
        public int BetNum { get; set; }

        /// <summary>
        /// 投注号码
        /// </summary>
        public string BetNumber { get; set; }

        /// <summary>
        /// 是否非胆拖操作
        /// </summary>
        public byte? IsNorm { get; set; }

        /// <summary>
        /// 是否中奖
        /// </summary>
        public byte? IsWin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PlayName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LotteryCode { get; set; }
    }
}
