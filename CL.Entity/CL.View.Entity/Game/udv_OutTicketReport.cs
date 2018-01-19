using System;

namespace CL.View.Entity.Game
{
    public class udv_OutTicketReport
    {						
        public long SchemeID { set; get; }
        public long TicketMoney { set; get; }
        public string SchemeNumber { set; get; }
        public int LotteryCode { set; get; }
        public int TicketSourceID { set; get; }
        public DateTime HandleDateTime { set; get; }
        public int SchemeStatus { set; get; }
        public string Ticket { set; get; }
        public string Number { set; get; }
        public long WinMoney { set; get; }

    }
}
