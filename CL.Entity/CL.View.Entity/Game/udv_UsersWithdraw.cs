using Dapper;
using System;

namespace CL.View.Entity.Game
{
    [Table("udv_UsersWithdraw")]
    public class udv_UsersWithdraw
    {
        /// <summary>
        /// 提现标识
        /// </summary>
        [Key]
        public long PayOutID { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        /// 提现申请时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 提现状态，0.申请，2.处理中，4.处理完成，6.提现失败
        /// </summary>
        public byte PayOutStatus { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 身份证编号
        /// </summary>
        public string IDNumber { get; set; }
        /// <summary>
        /// 银行类型
        /// </summary>
        public short BankType { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string CardNumber { get; set; }
        public string Remark { set; get; }
    }
}
