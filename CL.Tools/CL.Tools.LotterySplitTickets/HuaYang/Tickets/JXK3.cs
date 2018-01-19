using CL.Enum.Common.Lottery;
using CL.View.Entity.ElectronicTicket;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Tools.LotterySplitTickets.HuaYang.Tickets
{
    /// <summary>
    /// 江西快三
    /// 2017年5月9日
    /// </summary>
    public class JXK3 : TickBuilder
    {
        /// <summary>
        /// 彩种编号
        /// </summary>
        private const int LotteryCode = 102;
        private const int SuppId = 123;  //接口编码

        #region 分隔符
        private const char SEP_NUM_A = ',';
        private const char SEP_NUM_B = '#';
        private const char SEP_NUM_C = '-';
        private const char SEP_NUM_D = '@';
        private const char SEP_NUM_E = ';';
        private const char SEP_NUM_F = '^';
        private const char SEP_NUM_G = '|';
        #endregion

        #region 选号方式
        /// <summary>
        /// 单式
        /// </summary>
        private const int PickWay_DS = 1;
        /// <summary>
        /// 复式
        /// </summary>
        private const int PickWay_FS = 2;
        /// <summary>
        /// 胆拖
        /// </summary>
        private const int PickWay_DT = 3;
        #endregion

        /// <summary>
        /// 转换投注内容格式
        /// </summary>
        /// <param name="lottertNumbers"></param>
        /// <returns></returns>
        public override ICollection<udv_Ticket> ConvertToTickets(ICollection<udv_Parameter> lottertNumbers)
        {
            ICollection<udv_Ticket> list = new List<udv_Ticket>(lottertNumbers.Count);
            var enumrator = lottertNumbers.GetEnumerator();
            while (enumrator.MoveNext())
            {
                var rawTicket = enumrator.Current;
                var ticket = new udv_Ticket()
                {
                    SchemeID = rawTicket.SchemeID,
                    LotteryCode = LotteryCode,
                    Number = rawTicket.Number,
                    PlayTypeCode = rawTicket.PlayCode,
                    PickWayID = PickWay_DS,
                    Multiple = rawTicket.Multiple,
                    Bet = rawTicket.Bet,
                    Money = 2 * rawTicket.Multiple * rawTicket.Bet,
                    SDID = rawTicket.SDID
                };
                list.Add(ticket);
            }
            list = NumberSplit(list);
            list = MultipleSplit(list);
            //只接受1票1注
            //list = MergeTicket(list);
            return list;
        }

        /// <summary>
        /// 号码拆分
        /// </summary>
        /// <param name="tickets"></param>
        /// <returns></returns>
        private ICollection<udv_Ticket> NumberSplit(ICollection<udv_Ticket> tickets)
        {
            ICollection<udv_Ticket> list = new List<udv_Ticket>();
            foreach (var ticket in tickets)
            {
                switch (ticket.PlayTypeCode)
                {
                    case (int)JXK3PlayCode.HZ:
                        #region 和值是一张一注
                        string[] num = ticket.Number.Split(SEP_NUM_A);
                        foreach (string s in num)
                        {
                            udv_Ticket list_hz = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = s,
                                SDID = ticket.SDID,
                                Multiple = ticket.Multiple
                            };
                            list.Add(list_hz);
                        }
                        #endregion
                        break;
                    case (int)JXK3PlayCode.STHTX:
                        #region 三同号通选
                        udv_Ticket list_sthtx = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = "1*2*3*4*5*6",
                            SDID = ticket.SDID,
                            Multiple = ticket.Multiple
                        };
                        list.Add(list_sthtx);
                        #endregion
                        break;
                    case (int)JXK3PlayCode.STHDX:
                        #region 三同号单选
                        string[] num_sthdx = ticket.Number.Split(SEP_NUM_A);
                        foreach (string s in num_sthdx)
                        {
                            char[] c = s.ToCharArray();
                            udv_Ticket list_sthdx = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("*", c),
                                SDID = ticket.SDID,
                                Multiple = ticket.Multiple
                            };
                            list.Add(list_sthdx);
                        }
                        #endregion
                        break;
                    case (int)JXK3PlayCode.SBTH:
                        #region 三不同号
                        if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        {
                            string first_num = ticket.Number.Split(SEP_NUM_B)[0].Replace(SEP_NUM_A, '*');
                            string[] second_sbth = ticket.Number.Split(SEP_NUM_B)[1].Split(SEP_NUM_A);
                            int s = second_sbth.Length;
                            if (ticket.Number.Split(SEP_NUM_B)[0].Split(SEP_NUM_A).Length > 1)
                            {
                                for (int j = 0; j < s; j++)
                                {
                                    List<int> Numbers = new List<int>();
                                    Numbers.Add(Convert.ToInt32(first_num.Split('*')[0]));
                                    Numbers.Add(Convert.ToInt32(first_num.Split('*')[1]));
                                    Numbers.Add(Convert.ToInt32(second_sbth[j]));
                                    Numbers = Numbers.OrderBy(o => o).ToList();
                                    udv_Ticket list_sbth = new udv_Ticket()
                                    {
                                        LotteryCode = ticket.LotteryCode,
                                        SchemeID = ticket.SchemeID,
                                        PlayTypeCode = ticket.PlayTypeCode,
                                        Number = string.Join("*", Numbers),
                                        SDID = ticket.SDID,
                                        Multiple = ticket.Multiple
                                    };
                                    list.Add(list_sbth);
                                }
                            }
                            else
                            {
                                for (int j = 0; j < s - 1; j++)
                                {
                                    for (int k = j + 1; k < s; k++)
                                    {
                                        List<int> Numbers = new List<int>();
                                        Numbers.Add(Convert.ToInt32(first_num));
                                        Numbers.Add(Convert.ToInt32(second_sbth[j]));
                                        Numbers.Add(Convert.ToInt32(second_sbth[k]));
                                        Numbers = Numbers.OrderBy(o => o).ToList();
                                        udv_Ticket list_sbth = new udv_Ticket()
                                        {
                                            LotteryCode = ticket.LotteryCode,
                                            SchemeID = ticket.SchemeID,
                                            PlayTypeCode = ticket.PlayTypeCode,
                                            Number = string.Join("*", Numbers),
                                            SDID = ticket.SDID,
                                            Multiple = ticket.Multiple
                                        };
                                        list.Add(list_sbth);
                                    }
                                }
                            }
                        }
                        else
                        {
                            string[] num_sbth = ticket.Number.Split(SEP_NUM_A);
                            int n = num_sbth.Length;
                            for (int i = 0; i < n - 2; i++)
                            {
                                for (int j = i + 1; j < n - 1; j++)
                                {
                                    for (int k = j + 1; k < n; k++)
                                    {
                                        udv_Ticket list_sbth = new udv_Ticket()
                                        {
                                            LotteryCode = ticket.LotteryCode,
                                            SchemeID = ticket.SchemeID,
                                            PlayTypeCode = ticket.PlayTypeCode,
                                            Number = num_sbth[i] + "*" + num_sbth[j] + "*" + num_sbth[k],
                                            SDID = ticket.SDID,
                                            Multiple = ticket.Multiple
                                        };
                                        list.Add(list_sbth);
                                    }
                                }
                            }
                        }
                        #endregion
                        break;
                    case (int)JXK3PlayCode.SLHTX:
                        #region 三连号通选
                        udv_Ticket list_slhtx = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = "1*2*3*4*5*6",
                            SDID = ticket.SDID,
                            Multiple = ticket.Multiple
                        };
                        list.Add(list_slhtx);
                        #endregion
                        break;
                    case (int)JXK3PlayCode.ETHFX:
                        #region 二同号复选
                        string[] num_ethfx = ticket.Number.Split(SEP_NUM_A);
                        foreach (string s in num_ethfx)
                        {
                            char[] c = s.ToCharArray();
                            udv_Ticket list_ethfx = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("*", c),
                                SDID = ticket.SDID,
                                Multiple = ticket.Multiple
                            };
                            list.Add(list_ethfx);
                        }
                        #endregion
                        break;
                    case (int)JXK3PlayCode.ETHDX:
                        #region 二同号单选
                        string[] first_num_ethdx = ticket.Number.Split(SEP_NUM_G)[0].Split(SEP_NUM_A);
                        string[] second_sbth_ethdx = ticket.Number.Split(SEP_NUM_G)[1].Split(SEP_NUM_A);
                        for (int i = 0; i < first_num_ethdx.Length; i++)
                        {
                            char[] c = first_num_ethdx[i].ToCharArray();
                            for (int j = 0; j < second_sbth_ethdx.Length; j++)
                            {
                                udv_Ticket list_ethfx = new udv_Ticket()
                                {
                                    LotteryCode = ticket.LotteryCode,
                                    SchemeID = ticket.SchemeID,
                                    PlayTypeCode = ticket.PlayTypeCode,
                                    Number = string.Join("*", c) + "*" + second_sbth_ethdx[j],
                                    SDID = ticket.SDID,
                                    Multiple = ticket.Multiple
                                };
                                list.Add(list_ethfx);
                            }
                        }
                        #endregion
                        break;
                    case (int)JXK3PlayCode.EBTH:
                        #region 二不同号
                        if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        {
                            string first_num = ticket.Number.Split(SEP_NUM_B)[0];
                            string[] second_sbth = ticket.Number.Split(SEP_NUM_B)[1].Split(SEP_NUM_A);
                            int s = second_sbth.Length;
                            for (int j = 0; j < s; j++)
                            {
                                var Number = string.Empty;
                                if (Convert.ToInt32(first_num) < Convert.ToInt32(second_sbth[j]))
                                    Number = first_num + "*" + second_sbth[j];
                                else
                                    Number = second_sbth[j] + "*" + first_num;
                                udv_Ticket list_sbth = new udv_Ticket()
                                {
                                    LotteryCode = ticket.LotteryCode,
                                    SchemeID = ticket.SchemeID,
                                    PlayTypeCode = ticket.PlayTypeCode,
                                    Number = Number,
                                    SDID = ticket.SDID,
                                    Multiple = ticket.Multiple
                                };
                                list.Add(list_sbth);
                            }
                        }
                        else
                        {
                            string[] num_sbth = ticket.Number.Split(SEP_NUM_A);
                            int n = num_sbth.Length;
                            for (int i = 0; i < n - 1; i++)
                            {
                                for (int j = i + 1; j < n; j++)
                                {
                                    udv_Ticket list_sbth = new udv_Ticket()
                                    {
                                        LotteryCode = ticket.LotteryCode,
                                        SchemeID = ticket.SchemeID,
                                        PlayTypeCode = ticket.PlayTypeCode,
                                        Number = num_sbth[i] + "*" + num_sbth[j],
                                        SDID = ticket.SDID,
                                        Multiple = ticket.Multiple
                                    };
                                    list.Add(list_sbth);
                                }
                            }
                        }
                        #endregion
                        break;
                    default:
                        list.Add(ticket);
                        break;
                }
            }
            foreach (udv_Ticket item in list)
            {
                item.PickWayID = PickWay_DS;
                item.Bet = 1;
                item.Money = 2 * item.Multiple;
            }
            return list;
        }

        /// <summary>
        /// 倍数金额拆分
        /// </summary>
        /// <param name="tickets"></param>
        /// <returns></returns>
        private ICollection<udv_Ticket> MultipleSplit(ICollection<udv_Ticket> tickets)
        {
            ICollection<udv_Ticket> list = new List<udv_Ticket>(tickets.Count);
            #region
            foreach (var ticket in tickets)
            {
                if (ticket.Multiple < MAX_MULTIPLE_PER_TICKET && ticket.Money < MAX_MONEY_PER_TICKET)
                {
                    list.Add(ticket);
                    continue;
                }
                //每张票单价
                var money_per_num = 2 * ticket.Bet;
                //倍数拆分 单票 * 倍数 < 2W 
                var multiples = MultipleSplit(ticket.Multiple, money_per_num);
                if (multiples.Count > 0)
                {
                    foreach (var Item in multiples)
                    {
                        udv_Ticket t = (udv_Ticket)ticket.Clone();
                        t.Multiple = Item;
                        t.Money = money_per_num * Item;
                        list.Add(t);
                    }
                }
                else
                {
                    list.Add(ticket);
                }
            }
            #endregion

            return list;
        }

        /// <summary>
        /// 合并单张票
        /// </summary>
        /// <param name="tickets"></param>
        /// <returns></returns>
        private ICollection<udv_Ticket> MergeTicket(ICollection<udv_Ticket> tickets)
        {
            List<udv_Ticket> list = new List<udv_Ticket>(tickets);
            var s_list = MergeTicket(list.FindAll(x => x.PlayTypeCode != 10201), PickWay_DS, SINGLE_NUMBER_PER_TICKET, SEP_NUM_F);
            return list.FindAll(x => x.PlayTypeCode == 10201).Concat(s_list).ToList<udv_Ticket>();
        }
    }
}
