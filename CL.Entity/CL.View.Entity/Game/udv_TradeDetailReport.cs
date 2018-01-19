using Dapper;
using System;

namespace CL.View.Entity.Game
{
    /// <summary>
    /// 交易明细
    /// 中奖明细
    /// </summary>
    [Table("udv_TradeDetailReport")]
    public class udv_TradeDetailReport
    {
        /// <summary>
        /// 用户流水号
        /// </summary>
        [Key]
        public long UserID { set; get; }
        /// <summary>
        /// 用户名
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
        /// 交易金额
        /// </summary>
        public long TradeAmount { set; get; }
        /// <summary>
        /// 余额
        /// </summary>
        public long Balance { set; get; }
        /// <summary>
        /// 交易描述
        /// </summary>
        public string TradeRemark { set; get; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public long ChangeID { set; get; }
        /// <summary>
        /// 操作类型,0.充值，1.购彩消费，2.提现冻结，3.提现失败解冻，4.金豆兑换，5.中奖 11.用户撤单 12.系统撤单,13.追号撤单，14.投注失败退款 15.出票失败退款
        /// </summary>
        public byte TradeType { set; get; }
    }
}
