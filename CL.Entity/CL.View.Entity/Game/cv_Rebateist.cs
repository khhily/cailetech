using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    public class cv_RebateList
    {
        public long UserID { set; get; }

        public long RoomID { set; get; }

        public int Counts { set; get; }

        public long Amount { set; get; }

        public int Rebates { set; get; }
    }
    public class cv_Rebate
    {
        public long uuid { set; get; }
        public long rid { set; get; }
        public long am { set; get; }
    }
}
