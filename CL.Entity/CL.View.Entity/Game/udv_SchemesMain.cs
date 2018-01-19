using Dapper;
using System;

namespace CL.View.Entity.Game
{
    [Table("udv_SchemesMain")]
    public class udv_SchemesMain
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long SchemeID { get; set; }

        /// <summary>
        /// 发起时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 方案号
        /// </summary>
        public string SchemeNumber { get; set; }

        /// <summary>
        /// 方案标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 发起人编号
        /// </summary>
        public long? InitiateUserID { get; set; }

        /// <summary>
        /// 玩法编号 规则，采取五位编码，三位彩种编号+两位序号，具体如下：新快三和值3,10101  新快三和值4,10102  新快三三同号,10117双色球机选一注,80101
        /// </summary>
        public int LotteryCode { get; set; }

        /// <summary>
        /// 玩法编码
        /// </summary>
        //public int PlayCode { get; set; }

        /// <summary>
        /// 期号流水号
        /// </summary>
        public long IsuseID { get; set; }

        /// <summary>
        /// 购买号码（具体数据格式如下：注与注之间用“|”分割，玩法与注之间用“-”分割，投注号码之间用“,”分割，红球与篮球之间用“+”分割,篮球与幸运蓝球用”(“分割，例如【10101-3|10113-15|10119-3,3,3|80101-1,2,3,5,6,7+5(2|90101-4,5,6,7,8+4,8】）
        /// </summary>
        public string LotteryNumber { get; set; }

        /// <summary>
        /// 投注数
        /// </summary>
        //public int? BetNum { get; set; }

        /// <summary>
        /// 投注倍数
        /// </summary>
        //public int Multiple { get; set; }

        /// <summary>
        /// 方案金额
        /// </summary>
        public long SchemeMoney { get; set; }

        /// <summary>
        /// 保密级别 0不保密 1 保密到截止 2保密到开奖 3永远保密
        /// </summary>
        public short? SecrecyLevel { get; set; }

        ///// <summary>
        ///// 撤单状态 0未撤单 1用户撤单 2系统撤单
        ///// </summary>
        //public short? QuashStatus { get; set; }

        /// <summary>
        /// 订单状态 0待付款 1下单成功 10下单失败，2待开奖，3.开奖中，4.派奖中，5.完成
        /// </summary>
        //public short? OrderStatus { get; set; }
        public short? SchemeStatus { get; set; }

        /// <summary>
        /// 是否发送到电子票接口
        /// </summary>
        public bool IsSendOut { get; set; }

        /// <summary>
        /// 是否已出票
        /// </summary>
        //public bool? IsBuyed { get; set; }

        /// <summary>
        /// 出票员编号
        /// </summary>
        //public int? BuyOperatorID { get; set; }

        /// <summary>
        /// 出票方式 1本地出票 2电子票
        /// </summary>
        //public byte? PrintOutType { get; set; }

        /// <summary>
        /// 彩票标示码
        /// </summary>
        //public string Identifiers { get; set; }

        /// <summary>
        /// 是否开奖
        /// </summary>
        //public bool? IsOpened { get; set; }

        /// <summary>
        /// 开奖操作员编号
        /// </summary>
        //public int? OpenOperatorID { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public long? WinMoney { get; set; }

        /// <summary>
        /// 税后奖金
        /// </summary>
        public long? WinMoneyNoWithTax { get; set; }

        /// <summary>
        /// 方案描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 中奖描述
        /// </summary>
        //public string WinDescribe { get; set; }

        /// <summary>
        /// 中奖图片
        /// </summary>
        //public string WinImage { get; set; }

        /// <summary>
        /// 状态更新时间
        /// </summary>
        //public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 出票时间
        /// </summary>
        //public DateTime? PrintOutTime { get; set; }

        /// <summary>
        /// 方案来源 1网页 2 IOS 3 Andiord
        /// </summary>
        public byte? FromClient { get; set; }

        /// <summary>
        /// 购买类型 1追号2.跟单 3代购
        /// </summary>
        public byte? BuyType { get; set; }

        /// <summary>
        /// 电子票出票状态 0等待出票 1部分出票 2出票成功 3出票失败
        /// </summary>
        //public byte? TicketStatus { get; set; }

        /// <summary>
        /// 电子票出票时间
        /// </summary>
        //public DateTime? OutTicketTime { get; set; }

        /// <summary>
        /// 方案是否拆分
        /// </summary>
        //public bool? IsSplit { get; set; }

        /// <summary>
        /// 跟单方案编号
        /// </summary>
        public long? FollowSchemeID { get; set; }

        /// <summary>
        /// 复制跟单佣金
        /// </summary>
        public int FollowSchemeBonus { get; set; }

        /// <summary>
        /// 复制跟单佣金比例
        /// </summary>
        public int FollowSchemeBonusScale { get; set; }

        /// <summary>
        /// 加奖，特殊时候，会出现加奖的情况
        /// </summary>
        //public int? PlusAwards { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public int Schedule { get; set; }

        /// <summary>
        /// 预售状态：0.正常，1.当天预售，2.隔天预售，3.追号
        /// </summary>
        //public int AdvancedStatus { get; set; }
        /// <summary>
        /// 期号
        /// </summary>
        public string IsuseName { get; set; }
        public string SchemeIsOpened { get; set; }

        public string UserName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsExecuteChase { get; set; }

        public bool IsOpen { get; set; }

        public byte IsuseState { get; set; }

        public string PlayName { get; set; }

        public string LotteryName { get; set; }

        //public DateTime? ChaseExecuteTime { get; set; }

        //public DateTime? SystemEndTime { get; set; }

    }
}
