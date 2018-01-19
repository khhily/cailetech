using Dapper;
using System;

namespace CL.View.Entity.Game
{
    [Table("udv_RechargeDetailReport")]
    public class udv_RechargeDetailReport
    {
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
        /// 交易金额
        /// </summary>
        public long TradeAmount { set; get; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public long Balance { set; get; }
        /// <summary>
        /// 交易备注
        /// </summary>
        public string TradeRemark { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 交易记录标识
        /// </summary>
        public long ChangeID { set; get; }
        /// <summary>
        /// 交易号
        /// </summary>
        public string OrderNo { set; get; }
        /// <summary>
        /// 手续费
        /// </summary>
        public long FormalitiesFees { set; get; }
        /// <summary>
        /// 交易编号
        /// </summary>
        public long PayID { set; get;}
        /// <summary>
        /// 接口交易号
        /// </summary>
        public string RechargeNo { set; get; }
        /// <summary>
        /// 第三方交易号
        /// </summary>
        public string OutRechargeNo { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string PayType { set; get; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime CompleteTime { set; get; }
        /// <summary>
        /// 支付结果 0.未成功 1.已成功 2.已退款 3.退款处理中
        /// </summary>
        public short Result { set; get; }
    }
}
