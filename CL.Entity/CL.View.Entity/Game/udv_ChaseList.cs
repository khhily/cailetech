using Dapper;
using System;

namespace CL.View.Entity.Game
{
    [Table("udv_ChaseList")]
    public class udv_ChaseList
    {
        [Key]
        public int ChaseTaskID { get; set; }
        public long SchemeID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public int LotteryCode { get; set; }
        public string LotteryName { get; set; }
        public string Title { get; set; }
        public long SchemeMoney { get; set; }
        public int IsuseCount { get; set; }
        public int BuyedIsuseNum { get; set; }
        public int QuashedIsuseNum { get; set; }
        public short QuashStatus { get; set; }
        public short StopTypeWhenWin { get; set; }
        public long StopTypeWhenWinMoney { get; set; }

        public DateTime StartTime { set; get; }

        public DateTime EndTime { set; get; }

        public string SchemeNumber { set; get; }

        public string IsuseName { set; get; }
        public short SchemeStatus { set; get; }

    }
}
