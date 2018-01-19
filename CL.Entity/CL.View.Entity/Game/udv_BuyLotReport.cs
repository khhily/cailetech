using System;

namespace CL.View.Entity.Game
{
    public class udv_BuyLotReport
    {
        public long SchemeID { set; get; }
        public string SchemeNumber { set; get; }
        public int LotteryCode { set; get; }
        public long SchemeMoney { set; get; }
        public long UserID { set; get; }
        public string UserName { set; get; }
        public DateTime CreateTime { set; get; }
        public int SchemeStatus { set; get; }
        public int PrintOutType { set; get; }
        public int BuyType { set; get; }

    }
}
