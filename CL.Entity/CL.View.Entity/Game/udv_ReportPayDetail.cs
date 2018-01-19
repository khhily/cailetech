using System;

namespace CL.View.Entity.Game
{
    public class udv_ReportPayDetail
    {
        public string OrderNo { set; get; }
        public long UserID { set; get; }
        public string UserName { set; get; }
        public string UserMobile { set; get; }
        public string PayType { set; get; }
        public DateTime CreateTime { set; get; }
        public long TradeAmount { set; get; }
        public string RechargeNo { set; get; }
        public int Result { set; get; }
    }
}
