using CL.Tools.LotterySplitTickets.CaiLe;
using CL.Tools.LotterySplitTickets.HuaYang;
using CL.View.Entity.ElectronicTicket;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Tools.LotterySplitTickets
{
    public abstract class TickBuilder
    {
        /// <summary>
        /// 电子票最大金额
        /// </summary>
        protected const long MAX_MONEY_PER_TICKET = 20000;
        /// <summary>
        /// 电子票最大倍数
        /// </summary>
        protected const int MAX_MULTIPLE_PER_TICKET = 50;
        /// <summary>
        /// 每张票允许多少个单式号码
        /// </summary>
        protected const int SINGLE_NUMBER_PER_TICKET = 5;

        /// <summary>
        /// 彩种类定义字典
        /// </summary>
        private static readonly Dictionary<String, Dictionary<Int32, Func<TickBuilder>>> SupplierProvider = new Dictionary<String, Dictionary<Int32, Func<TickBuilder>>>();

        #region
        #endregion

        static TickBuilder()
        {
            RegisterBuilder();
        }

        static void RegisterBuilder()
        {
            SupplierProvider.Add("HuaYang", new HYRegister().Regist()); //华阳
            SupplierProvider.Add("CaiLe", new CLRegister().Regist());   //彩乐测试
        }

        public static TickBuilder Create(string type, int lotteryCode)
        {
            TickBuilder builder = null;
            if (SupplierProvider.ContainsKey(type))
            {
                builder = SupplierProvider[type][lotteryCode]();
            }
            if (builder == null)
            {
                throw new Exception(String.Format("没有注册 {0} 彩种编码:{0} 的拆票类", type, lotteryCode));
            }
            return builder;
        }

        /// <summary>
        /// 转换投注内容格式
        /// </summary>
        /// <param name="lottertNumbers"></param>
        /// <returns></returns>
        public abstract ICollection<udv_Ticket> ConvertToTickets(ICollection<udv_Parameter> lottertNumbers);

        /// <summary>
        /// 倍数进行多长拆票
        /// </summary>
        /// <param name="multiple">倍数</param>
        /// <param name="money_per_num">单张票金额</param>
        /// <param name="MaxMultiple">最大倍数</param>
        /// <returns></returns>
        protected virtual ICollection<int> MultipleSplit(int multiple, int money_per_num, int MaxMultiple = MAX_MULTIPLE_PER_TICKET)
        {
            List<int> dNUm = new List<int>();
            while (multiple > 1 && (money_per_num * multiple) > MAX_MONEY_PER_TICKET || multiple > MaxMultiple)
            {
                /*
                 *  200，5 * 20  * 2 = 200
                 *  200 * 200 = 40000 > 20000 || 200 > 99
                 *  20000/200=100
                 *  100 > 99
                 *  100 -1 = 99
                 */
                int _newMultiple = (int)(MAX_MONEY_PER_TICKET / money_per_num); //算出实际倍数
                if (_newMultiple > MaxMultiple)
                    _newMultiple = MaxMultiple;

                if (multiple < _newMultiple)
                {
                    _newMultiple = multiple;
                }
                multiple -= _newMultiple;
                while (_newMultiple >= MaxMultiple)
                {
                    dNUm.Add((int)MaxMultiple);
                    _newMultiple -= (int)MaxMultiple;
                }
                if (_newMultiple > 0)
                {
                    dNUm.Add(_newMultiple);
                }
            }
            if (multiple > 0)
            {
                dNUm.Add(multiple);
            }

            return dNUm;
        }

        /// <summary>
        /// 每5个单式合并一张
        /// </summary>
        /// <param name="tickets"></param>
        /// <param name="pickWayId"></param>
        /// <param name="mergeNum"></param>
        /// <param name="spe_num"></param>
        /// <returns></returns>
        protected ICollection<udv_Ticket> MergeTicket(ICollection<udv_Ticket> tickets, int pickWayId, int mergeNum, char spe_num)
        {
            List<udv_Ticket> list = new List<udv_Ticket>(tickets);

            var s_list = list.FindAll(x => x.PickWayID == pickWayId);
            if (s_list.Count > 0)
            {
                var g_s_list = s_list.GroupBy(x => String.Concat(x.PlayTypeCode, "_", x.PickWayID, "_", x.Multiple));
                foreach (var s_l in s_list)
                {
                    list.Remove(s_l);
                }
                foreach (var g_s in g_s_list)
                {
                    s_list = g_s.ToList();

                    udv_Ticket[] comp_ticket = new udv_Ticket[(int)Math.Ceiling(s_list.Count / (decimal)mergeNum)];
                    for (int i = 0; i < s_list.Count; i++)
                    {
                        var idx = i / mergeNum;
                        if (i % mergeNum == 0)
                        {
                            comp_ticket[idx] = new udv_Ticket()
                            {
                                LotteryCode = s_list[i].LotteryCode,
                                PickWayID = s_list[i].PickWayID,
                                PlayTypeCode = s_list[i].PlayTypeCode,
                                Multiple = s_list[i].Multiple,
                                SDID = s_list[i].SDID,
                                SchemeID = s_list[i].SchemeID
                            };
                        }
                        comp_ticket[idx].Bet += s_list[i].Bet;
                        comp_ticket[idx].Number += s_list[i].Number + spe_num;
                        comp_ticket[idx].Money += s_list[i].Money;
                    }
                    foreach (var ticket in comp_ticket)
                    {
                        if (ticket != null)
                            ticket.Number = ticket.Number.TrimEnd(spe_num);
                    }
                    list.AddRange(comp_ticket);
                }
            }
            //else
            //    list = s_list;
            return list;
        }

        /// <summary>
        /// 方案转电子票
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <param name="ticketList"></param>
        /// <returns></returns>
        public ICollection<udv_Ticket> Generate(long SchemeID, int LotteryCode, List<udv_Parameter> ticketList)
        {
            ICollection<udv_Ticket> tickets = null;
            if (ticketList.Count == 0)
                return null;

            // 电子票拆分、转换投注内容格式
            tickets = ConvertToTickets(ticketList);
            return tickets;
        }
    }
}
