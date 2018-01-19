using CL.Tools.LotterySplitTickets.CaiLe.Tickets;
using System;
using System.Collections.Generic;

namespace CL.Tools.LotterySplitTickets.CaiLe
{
    public class CLRegister
    {
        public Dictionary<Int32, Func<TickBuilder>> Regist()
        {
            var container = new Dictionary<Int32, Func<TickBuilder>>();
            container.Add(801, () => new SSQ());    //双色球
            container.Add(901, () => new CJDLT());    //大乐透
            return container;
        }
    }
}
