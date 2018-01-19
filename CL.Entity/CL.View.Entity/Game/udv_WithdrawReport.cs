using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    public class udv_WithdrawReport
    {

        public long PayOutID { set; get; }
        public long UserID { set; get; }
        public string UserName { set; get; }
        public string UserMobile { set; get; }
        public DateTime CreateTime { set; get; }
        public long TradeAmount { set; get; }
        public int PayOutStatus { set; get; }

    }
}
