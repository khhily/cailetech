
namespace CL.View.Entity.Game
{
    /// <summary>
    /// 电子票入库
    /// TicketsStorage
    /// </summary>
    public class udv_ts
    {
        /// <summary>
        /// 玩法编码
        /// </summary>
        public int pc { set; get; }
        /// <summary>
        /// 票金额
        /// </summary>
        public long at { set; get; }
        /// <summary>
        /// 投注倍数
        /// </summary>
        public int mp { set; get; }
        /// <summary>
        /// 投注内容
        /// </summary>
        public string n { set; get; }
        /// <summary>
        /// 方案详细表标识
        /// </summary>
        public long sdid { set; get; }
        /// <summary>
        /// 出票平台
        /// </summary>
        public int ts { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public byte s { set; get; }
    }
    /// <summary>
    /// 电子票投注
    /// TicketBetting 缩写
    /// </summary>
    public class udv_tb
    {
        /// <summary>
        /// 电子票标识
        /// </summary>
        public long seid { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string msg { set; get; }
        /// <summary>
        /// 彩票标识码
        /// </summary>
        public string idf { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public byte s { set; get; }
    }
}
