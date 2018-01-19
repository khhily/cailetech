using CL.Tools.LotterySplitTickets.HuaYang.Tickets;
using System;
using System.Collections.Generic;

namespace CL.Tools.LotterySplitTickets.HuaYang
{
    public class HYRegister
    {
        public Dictionary<Int32, Func<TickBuilder>> Regist()
        {
            var container = new Dictionary<Int32, Func<TickBuilder>>();

            container.Add(101, () => new JLXK3());  //吉林快3
            container.Add(102, () => new JXK3());   //江西快3
            container.Add(201, () => new HB11X5()); //湖北11选5
            container.Add(202, () => new SD11X5()); //山东11选5
            container.Add(801, () => new SSQ());    //双色球
            container.Add(901, () => new CJDLT());  //大乐透
            container.Add(301, () => new CQSSC());  //重庆时时彩
            return container;
        }
    }
}
