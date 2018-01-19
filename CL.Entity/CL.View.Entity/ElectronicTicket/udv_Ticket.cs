using System;

namespace CL.View.Entity.ElectronicTicket
{
    public class udv_Ticket
    {
        /// <summary>
        /// 方案ID
        /// </summary>
        public long SchemeID { get; set; }

        public long SDID { set; get; }

        /// <summary>
        /// 彩种编码
        /// </summary>
        public int LotteryCode { get; set; }
        /// <summary>
        /// 玩法编码
        /// </summary>
        public int PlayTypeCode { get; set; }
        /// <summary>
        /// 选号方式
        /// </summary>
        public int PickWayID { get; set; }
        /// <summary>
        /// 投注号码
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 投注倍数
        /// </summary>
        public Int32 Multiple { get; set; }
        /// <summary>
        /// 投注数
        /// </summary>
        public Int32 Bet { get; set; }
        /// <summary>
        /// 投注金额
        /// </summary>
        public Int64 Money { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
