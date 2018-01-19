using Dapper;
using System;

namespace CL.View.Entity.Game
{
    /// <summary>
    /// 提现明细
    /// </summary>
    [Table("udv_WithdrawDetailReport")]
    public class udv_WithdrawDetailReport
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
        /// 提现流水号
        /// </summary>
        public long PayOutID { set; get; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public long Amount { set; get; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime OperTime { set; get; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 提现状态
        /// 0.申请，2.处理中，4.处理完成，6.提现失败
        /// </summary>
        public byte PayOutStatus { set; get; }
        /// <summary>
        /// 开会银行
        /// </summary>
        public string BankName { set; get; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string CardNumber { set; get; }
        /// <summary>
        /// 银行预留手机号码
        /// </summary>
        public string ReservedPhone { set; get; }
        /// <summary>
        /// 银行卡所属城市
        /// </summary>
        public string Area { set; get; }
        /// <summary>
        /// 银行卡类型
        /// </summary>
        public byte BankType { set; get; }
        /// <summary>
        /// 银行卡编号
        /// </summary>
        public long BankID { set; get; }
    }
}
