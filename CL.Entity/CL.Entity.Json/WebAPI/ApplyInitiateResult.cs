using CL.Entity.Json.WebAPI;
using System.Collections.Generic;

namespace CL.Json.Entity.WebAPI
{
    public class ApplyInitiateResult : JsonResult
    {
        public string Token { set; get; }
        public string SystemTotalWin { set; get; }
        public string SystemTime { set; get; }
        public bool IsVerifyToken { set; get; }
        public List<LotteryData> LotteryData { set; get; }
        public LoginData LoginData { set; get; }
        public List<ApplyAd> ActivityData { set; get; }
    }
    public class LotteryData
    {
        public int LotteryCode { set; get; }
        public string LotteryName { set; get; }
        public string LotteryIconUrl { set; get; }
        public string Subhead { set; get; }
        public string AddawardSubhead { set; get; }
        public bool IsHot { set; get; }
        public int LotteryStatus { set; get; }
        /// <summary>
        /// 群组编号
        /// </summary>
        public string ChatGroups { set; get; }
        /// <summary>
        /// 聊天室编号
        /// </summary>
        public string ChatRooms { set; get; }
    }
    public class LoginData
    {
        public string Token { set; get; }
        public long UserCode { set; get; }
        public string Nick { set; get; }
        public string Mobie { set; get; }
        public long Balance { set; get; }
        public long Gold { set; get; }
        public bool IsRobot { set; get; }
        public string AvatarUrl { set; get; }
        public int VIP { set; get; }
        public int Level { set; get; }
        public int SpecialGroup { set; get; }
        public bool IsCertification { set; get; }
        public string FullName { set; get; }
        /// <summary>
        /// 是否设置体现密码
        /// </summary>
        public bool IsWithdrawPwd { set; get; }

        /// <summary>
        /// 彩券个数
        /// </summary>
        public int CouponsCount { set; get; }

        /// <summary>
        /// 对称加密的UUID
        /// </summary>
        public string Uid { set; get; }
    }
    public class ApplyAd
    {
        /// <summary>
        /// 广告地址
        /// </summary>
        public string ad { set; get; }

        /// <summary>
        /// 落地页地址
        /// </summary>
        public string page { set; get; }
    }
    public class MiniGame
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int ID { set; get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 单价
        /// </summary>
        public long Gold { set; get; }
    }
}
