using Dapper;
using System;

namespace CL.View.Entity.Game
{
    [Table("udv_SchemeChaseTask")]
    public class udv_SchemeChaseTask
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long SchemeID { get; set; }
        /// <summary>
        /// 发起时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 方案号
        /// </summary>
        public string SchemeNumber { get; set; }
        /// <summary>
        /// 方案标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 方案描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 发起人编号
        /// </summary>
        public long InitiateUserID { get; set; }
        /// <summary>
        /// 玩法编号 规则，采取五位编码，三位彩种编号+两位序号，具体如下：新快三和值3,10101  新快三和值4,10102  新快三三同号,10117双色球机选一注,80101
        /// </summary>
        public int LotteryCode { get; set; }
        /// <summary>
        /// 期号流水号
        /// </summary>
        public long IsuseID { get; set; }
        /// <summary>
        /// 购买号码（具体数据格式如下：注与注之间用“|”分割，玩法与注之间用“-”分割，投注号码之间用“,”分割，红球与篮球之间用“+”分割,篮球与幸运蓝球用”(“分割，例如【10101-3|10113-15|10119-3,3,3|80101-1,2,3,5,6,7+5(2|90101-4,5,6,7,8+4,8】）
        /// </summary>
        public string LotteryNumber { get; set; }
        /// <summary>
        /// 方案总金额
        /// </summary>
        public long SchemeMoney { get; set; }
        /// <summary>
        /// 保密级别 0不保密 1 保密到截止 2保密到开奖 3永远保密
        /// </summary>
        public short SecrecyLevel { get; set; }
        /// <summary>
        /// 订单状态 0待付款，2.订单过期，4下单成功，6.出票成功，8.部分出票成功，10.下单失败（限号），12.订单撤销，14.中奖，15.派奖中，16.派奖完成，18.不中奖完成，19.追号进行中，20.追号完成
        /// </summary>
        public short SchemeStatus { get; set; }
        /// <summary>
        /// 方案来源 1网页 2 IOS 3 Andiord
        /// </summary>
        public byte FromClient { get; set; }
        /// <summary>
        /// 购买类型 0.代购 1追号2.跟单
        /// </summary>
        public byte BuyType { get; set; }
        /// <summary>
        /// 跟单方案编号
        /// </summary>
        public long FollowSchemeID { get; set; }
        /// <summary>
        /// 复制跟单佣金
        /// </summary>
        public int FollowSchemeBonus { get; set; }
        /// <summary>
        /// 复制跟单佣金比例
        /// </summary>
        public int FollowSchemeBonusScale { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsSendOut { get; set; }
        /// <summary>
        /// 会员名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 彩种名称
        /// </summary>
        public string LotteryName { get; set; }
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
        /// 玩法名称
        /// </summary>
        public string PlayName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 投注方式（1.机选  2.自选）
        /// </summary>
        public short BetType { get; set; }
        /// <summary>
        /// 定制期数
        /// </summary>
        public int IsuseCount { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 终止金额
        /// </summary>
        public long StopTypeWhenWinMoney { get; set; }
        /// <summary>
        /// 撤销状态:0 未撤销 1 用户撤销 2 系统撤销
        /// </summary>
        public short QuashStatus { get; set; }
    }
}
