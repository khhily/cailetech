using Dapper;
using System;

namespace CL.View.Entity.Game
{
    [Table("udv_ChaseTaskDetails")]
    public class udv_ChaseTaskDetails
    {
        [Key]
        public long SchemeID { set; get; }

        public int LotteryCode { set; get; }

        public DateTime StartTime { set; get; }

        public DateTime EndTime { set; get; }

        public string IsuseName { set; get; }

        public long ChaseTaskDetailsID { set; get; }

        public long Amount { set; get; }

        public int Multiple { set; get; }

        public int UserID { set; get; }
    }
}
