using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    [Table("udv_ActivityApply")]
    public class udv_ActivityApply
    {
        [Key]
        public int ActivityID { set; get; }

        public int ActivityType { set; get; }

        public string ActivitySubject { set; get; }

        public DateTime CreateTime { set; get; }

        public DateTime StartTime { set; get; }

        public DateTime EndTime { set; get; }

        public long ActivityMoney { set; get; }

        public int CurrencyUnit { set; get; }

        public int ActivityApply { set; get; }

        public int RegularCount { set; get; }

    }
}
