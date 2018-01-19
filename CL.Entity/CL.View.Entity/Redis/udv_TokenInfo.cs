
namespace CL.View.Entity.Redis
{
    public class udv_TokenInfo
    {
        /// <summary>
        /// 新的令牌
        /// </summary>
        public string Token { set; get; }
        /// <summary>
        /// 设备标识
        /// </summary>
        public string Equipment { set; get; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserCode { set; get; }
        /// <summary>
        /// 令牌类型 0游客 1会员
        /// </summary>
        public short TokenType { set; get; }

    }
}
