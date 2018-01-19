using Dapper;

namespace CL.View.Entity.Game
{
    [Table("udv_OverChaseTasksExamine")]
    public class udv_OverChaseTasksExamine
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long SchemeID { set; get; }

        public long ChaseTaskID { set; get; }

        public long StopTypeWhenWinMoney { set; get; }

        public short OrderStatus { set; get; }

        public short QuashStatus { set; get; }

        public int LotteryCode { set; get; }
    }
}
