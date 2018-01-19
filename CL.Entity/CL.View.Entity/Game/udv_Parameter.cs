
namespace CL.View.Entity.Game
{
    public class udv_Parameter
    {
        /// <summary>
        /// 方案标识列
        /// </summary>
        public long SchemeID { set; get; }

        /// <summary>
        /// 方案详情标识列
        /// </summary>
        public long SDID { set; get; }
        /// <summary>
        /// 投注注数
        /// </summary>
        public int Bet { get; set; }
        /// <summary>
        /// 投注倍数
        /// </summary>
        public int Multiple { set; get; }
        /// <summary>
        /// 玩法id
        /// </summary>
        public int PlayCode { set; get; }
        /// <summary>
        /// 投注号码
        /// </summary>
        public string Number { set; get; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public long Amount { set; get; }
        /// <summary>
        /// 是否非胆拖号码
        /// </summary>
        public bool IsNorm { get; set; }

        public long UserCode { set; get; }
        public int LotteryCode { set; get; }
    }
}
