using System;
using System.Collections.Generic;

namespace CL.Json.Entity.WebAPI
{
    public class TradingResult : JsonResult
    {
        public List<TradingEntity> Data { set; get; }
        /// <summary>
        /// 可用彩券总额
        /// </summary>
        public long CouponsMoney { set; get; }
        /// <summary>
        /// 可提现金额
        /// </summary>
        public long Withdraw { set; get; }
        /// <summary>
        /// 可用余额
        /// </summary>
        public long Balance { set; get; }
    }
    public class TradingEntity
    {
        public long UserCode { set; get; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Time { set; get; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public long Amount { set; get; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
    }
    public class TradingEntity_sf
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserCode { set; get; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public long Amount { set; get; }
        /// <summary>
        /// 余额
        /// </summary>
        public long Balance { set; get; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 操作类型 0.充值 1.购彩消费 2.提现冻结 3.提现失败解冻 4.金豆兑换 5.中奖 11.用户撤单 12.系统撤单 13.追号撤单 14.投注失败退款 15.出票失败退款 16.充值退款冻结 17.退款失败返回金额
        /// </summary>
        public int TradeType { set; get; }
    }
}
