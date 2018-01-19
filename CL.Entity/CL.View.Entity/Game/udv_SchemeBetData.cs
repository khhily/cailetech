using System.Collections.Generic;

namespace CL.View.Entity.Game
{
    public class udv_SchemeBetData
    {
        public int PlayCode { set; get; }
        public List<udv_SchemeBetDataDetail> Data { set; get; }
    }
    public class udv_SchemeBetDataDetail
    {
        public int Multiple { set; get; }
        public int Bet { set; get; }
        public short IsNorm { set; get; }
        public string Number { set; get; }
    }
}
