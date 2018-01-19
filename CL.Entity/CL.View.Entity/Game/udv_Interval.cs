using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    public class udv_Interval
    {
        public int pk { set; get; }
        public long min { set; get; }
        public long max { set; get; }
        public long award { set; get; }
    }
    public class udv_Ranking
    {
        public int placing { set; get; }
        public long award { set; get; }
    }
}
