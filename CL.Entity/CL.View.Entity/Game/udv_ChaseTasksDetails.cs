using System;

namespace CL.View.Entity.Game
{
    public class udv_ChaseTasksDetails
    {
        public long ID { get; set; }
        public long ChaseTaskID { get; set; }
        public DateTime CreateTime { get; set; }
        public long IsuseID { get; set; }
        public int PlayCode { get; set; }
        public string PlayName { get; set; }
        public string LotteryNumber { get; set; }
        public int Multiple { get; set; }
        public long Amount { get; set; }
        public int RedPacketMoney { get; set; }
        public string RedPacketId { get; set; }
        public short QuashStatus { get; set; }
        public long SchemeID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int LotteryCode { get; set; }
        public string IsuseName { get; set; }
        public string SchemeNumber { get; set; }
        public long WinMoney { get; set; }
        public long WinMoneyNoWithTax { get; set; }
    }
}
