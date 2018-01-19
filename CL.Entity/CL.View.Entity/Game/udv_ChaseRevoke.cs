using Dapper;
using System;

namespace CL.View.Entity.Game
{
    [Table("udv_ChaseRevoke")]
    public class udv_ChaseRevoke
    {
        //															
        /// <summary>
        /// 追号详细标识
        /// </summary>
        [Key]
        public long ID { set; get; }
        /// <summary>
        /// 追号标识
        /// </summary>
        public long ChaseTaskID { set; get; }
        /// <summary>
        /// 方案标识
        /// </summary>
        public long SchemeID { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 期号标识
        /// </summary>
        public long IsuseID { set; get; }
        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { set; get; }
        /// <summary>
        /// 金额
        /// </summary>
        public long Amount { set; get; }
        /// <summary>
        /// 彩券标识
        /// </summary>
        public string RedPacketId { set; get; }
        /// <summary>
        /// 彩券金额
        /// </summary>
        public long RedPacketMoney { set; get; }
        /// <summary>
        /// 撤销状态
        /// </summary>
        public int QuashStatus { set; get; }
        /// <summary>
        /// 是否执行追号
        /// </summary>
        public bool IsExecuted { set; get; }
        /// <summary>
        /// 保密级别：0 不保密 1 到截止 2 到开奖 3 永远
        /// </summary>
        public byte SecrecyLevel { set; get; }
        /// <summary>
        /// 投注号码
        /// </summary>
        public string LotteryNumber { set; get; }
        /// <summary>
        /// 是否分享
        /// </summary>
        public byte IsShare { set; get; }
        /// <summary>
        /// 是否出票
        /// </summary>
        public bool IsSendOut { set; get; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserID { set; get; }
        /// <summary>
        /// 彩种编号
        /// </summary>
        public long LotteryCode { set; get; }
        /// <summary>
        /// 期号
        /// </summary>
        public string IsuseName { set; get; }

    }
}
