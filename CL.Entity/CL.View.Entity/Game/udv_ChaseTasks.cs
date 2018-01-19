using Dapper;
namespace CL.View.Entity.Game
{
    [Table("udv_ChaseTasks")]
    public class udv_ChaseTasks
    {
        public long ChaseTaskID { get; set; }
        public long UserID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int LotteryCode { get; set; }
        public string LotteryName { get; set; }
        public short QuashStatus { get; set; }
        public short StopTypeWhenWin { get; set; }
        public long StopTypeWhenWinMoney { get; set; }
        public long SumMoney { get; set; }
        public int SumIsuseNum { get; set; }
        public int BuyedIsuseNum { get; set; }
        public int QuashedIsuseNum { get; set; }
        public int BuyedMoney { get; set; }
        public int QuashedMoney { get; set; }
    }
}
