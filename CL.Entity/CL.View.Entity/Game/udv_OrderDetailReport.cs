using Dapper;
using System;

namespace CL.View.Entity.Game
{
    [Table("udv_OrderDetailReport")]
    public class udv_OrderDetailReport
    {
        /// <summary>
        /// 用户流水号
        /// </summary>
        [Key]
        public long UserID { set; get; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string UserMobile { set; get; }
        /// <summary>
        /// 是否实名认证
        /// </summary>
        public bool IsVerify { set; get; }
        /// <summary>
        /// 是否机器人
        /// </summary>
        public bool IsRobot { set; get; }
        /// <summary>
        /// 是否允许登录
        /// </summary>
        public bool IsCanLogin { set; get; }
        /// <summary>
        /// 购彩类型：0代购 1追号 2跟单
        /// </summary>
        public byte BuyType { set; get; }
        /// <summary>
        /// 方案编号
        /// </summary>
        public string SchemeNumber { set; get; }
        /// <summary>
        /// 选号
        /// </summary>
        public string LotteryNumber { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 期号
        /// </summary>
        public string IsuseName { set; get; }
        /// <summary>
        /// 订单状态 0待付款，2.订单过期，4下单成功，6.出票成功，8.部分出票成功，10.下单失败（限号），12.订单撤销，14.中奖，15.派奖中，16.派奖完成，18.不中奖完成，19.追号进行中，20.追号完成
        /// </summary>
        public int SchemeStatus { set; get; }
        /// <summary>
        /// 方案总额
        /// </summary>
        public long SchemeMoney { set; get; }
        /// <summary>
        /// 彩种名称
        /// </summary>
        public string LotteryName { set; get; }
        /// <summary>
        /// 追号期数
        /// </summary>
        public string IsuseCount { set; get; }
        /// <summary>
        /// 追号ＩＤ
        /// </summary>
        public long ChaseTaskID { set; get; }
        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { set; get; }
        /// <summary>
        /// 总中奖金额
        /// </summary>
        public long WinMoney { set; get; }
        public long SchemeID { set; get; }
    }
}
