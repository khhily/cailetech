using CL.View.Entity.ElectronicTicket;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Tools.LotterySplitTickets.HuaYang.Tickets
{
    /// <summary>
    /// 双色球
    /// 2017年5月9日
    /// </summary>
    public class SSQ : TickBuilder
    {
        /// <summary>
        /// 彩种编号
        /// </summary>
        private const int LotteryCode = 801;
        private const int SuppId = 118;  //接口编码

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
                //单式一票最多五注,其他一注一票
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
                //示例：
                //标准选号：
                //单式：01,02,03,04,05,06|01
                //复式：01,02,03,04,05,06,07|01    接口红球不能超过16个数字
                //          01,02,03,04,05,06|01,02
                //          01,02,03,04,05,06,07|01,02
                //胆拖选号：01,02,03,04,05#06,07|01,02   接口拖码大于等于2 小于等于16  红球总数大于6 蓝球大于2
                //	        01,02,03,04#05,06,07|01,02
                //	        01,02,03#04,05,06,07|01,02
                //	        01,02#03,04,05,06,07|01,02
                //         01#02,03,04,05,06,07|01,02

                //接口格式
                //单式：010203040506|01
                //复式：01020304050607|01
                //          010203040506|0102
                //          01020304050607|0102
                //胆拖选号：0102030405*0607|0102
                //	        01020304*050607|0102
                //	        010203*04050607|0102
                //	        0102*0304050607|0102
                //         01*020304050607|0102

                if (ticket.Number.IndexOf('#') > -1)  //是否胆拖
                {
                    #region
                    string[] arr = ticket.Number.Split('|');
                    string[] blue = arr[1].Split(',');
                    string[] first_num = arr[0].Split('#')[0].Split(',');
                    string[] second_num = arr[0].Split('#')[1].Split(',');  //接口拖码不能超过16个数字
                    decimal n = second_num.Length;
                    int bet = 0;

                    if (first_num.Length == 1)
                    {
                        bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                    }
                    else if (first_num.Length == 2)
                    {
                        bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                    }
                    else if (first_num.Length == 3)
                    {
                        bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                    }
                    else if (first_num.Length == 4)
                    {
                        bet = Convert.ToInt32(n * (n - 1) / 2);
                    }
                    else if (first_num.Length == 5)
                    {
                        bet = second_num.Length;
                    }

                    long money = 2 * bet * blue.Length;   //按照一倍来算是否超过2万
                    if (money < 20000 && n <= 16)
                    {
                        udv_Ticket list_ssq_dt = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = string.Join("", first_num) + "*" + string.Join("", second_num) + "|" + string.Join("", blue),
                            Multiple = ticket.Multiple,
                            PickWayID = PickWay_DT,
                            Bet = bet * blue.Length,
                            SDID = ticket.SDID,
                            Money = 2 * bet * blue.Length * ticket.Multiple
                        };
                        list.Add(list_ssq_dt);
                    }
                    else
                    {
                        if (first_num.Length == 1)
                        {
                            #region
                            for (int i = 0; i < n - 4; i++)
                            {
                                for (int j = i + 1; j < n - 3; j++)
                                {
                                    for (int x = j + 1; x < n - 2; x++)
                                    {
                                        for (int y = x + 1; y < n - 1; y++)
                                        {
                                            for (int z = y + 1; z < n; z++)
                                            {
                                                string[] NewSecondNum = { second_num[i], second_num[j], second_num[x], second_num[y], second_num[z] };
                                                udv_Ticket list_ssq_dt = new udv_Ticket()
                                                {
                                                    LotteryCode = ticket.LotteryCode,
                                                    SchemeID = ticket.SchemeID,
                                                    PlayTypeCode = ticket.PlayTypeCode,
                                                    Number = string.Join("", first_num) + string.Join("", NewSecondNum) + "|" + string.Join("", blue),
                                                    Multiple = ticket.Multiple,
                                                    PickWayID = blue.Length == 1 ? PickWay_DS : PickWay_FS,
                                                    Bet = 1 * blue.Length,
                                                    SDID = ticket.SDID,
                                                    Money = 2 * 1 * blue.Length * ticket.Multiple
                                                };
                                                list.Add(list_ssq_dt);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (first_num.Length == 2)
                        {
                            #region
                            for (int i = 0; i < n - 3; i++)
                            {
                                for (int j = i + 1; j < n - 2; j++)
                                {
                                    for (int x = j + 1; x < n - 1; x++)
                                    {
                                        for (int y = x + 1; y < n; y++)
                                        {
                                            string[] NewSecondNum = { second_num[i], second_num[j], second_num[x], second_num[y] };
                                            udv_Ticket list_ssq_dt = new udv_Ticket()
                                            {
                                                LotteryCode = ticket.LotteryCode,
                                                SchemeID = ticket.SchemeID,
                                                PlayTypeCode = ticket.PlayTypeCode,
                                                Number = string.Join("", first_num) + string.Join("", NewSecondNum) + "|" + string.Join("", blue),
                                                Multiple = ticket.Multiple,
                                                PickWayID = blue.Length == 1 ? PickWay_DS : PickWay_FS,
                                                Bet = 1 * blue.Length,
                                                SDID = ticket.SDID,
                                                Money = 2 * 1 * blue.Length * ticket.Multiple
                                            };
                                            list.Add(list_ssq_dt);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (first_num.Length == 3)
                        {
                            #region
                            for (int i = 0; i < n - 2; i++)
                            {
                                for (int j = i + 1; j < n - 1; j++)
                                {
                                    for (int x = j + 1; x < n; x++)
                                    {
                                        string[] NewSecondNum = { second_num[i], second_num[j], second_num[x] };
                                        udv_Ticket list_ssq_dt = new udv_Ticket()
                                        {
                                            LotteryCode = ticket.LotteryCode,
                                            SchemeID = ticket.SchemeID,
                                            PlayTypeCode = ticket.PlayTypeCode,
                                            Number = string.Join("", first_num) + string.Join("", NewSecondNum) + "|" + string.Join("", blue),
                                            Multiple = ticket.Multiple,
                                            PickWayID = blue.Length == 1 ? PickWay_DS : PickWay_FS,
                                            Bet = 1 * blue.Length,
                                            SDID = ticket.SDID,
                                            Money = 2 * 1 * blue.Length * ticket.Multiple
                                        };
                                        list.Add(list_ssq_dt);
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (first_num.Length == 4)
                        {
                            #region
                            for (int i = 0; i < n - 1; i++)
                            {
                                for (int j = i + 1; j < n; j++)
                                {
                                    string[] NewSecondNum = { second_num[i], second_num[j] };
                                    udv_Ticket list_ssq_dt = new udv_Ticket()
                                    {
                                        LotteryCode = ticket.LotteryCode,
                                        SchemeID = ticket.SchemeID,
                                        PlayTypeCode = ticket.PlayTypeCode,
                                        Number = string.Join("", first_num) + string.Join("", NewSecondNum) + "|" + string.Join("", blue),
                                        Multiple = ticket.Multiple,
                                        PickWayID = blue.Length == 1 ? PickWay_DS : PickWay_FS,
                                        Bet = 1 * blue.Length,
                                        SDID = ticket.SDID,
                                        Money = 2 * 1 * blue.Length * ticket.Multiple
                                    };
                                    list.Add(list_ssq_dt);
                                }
                            }
                            #endregion
                        }
                        else if (first_num.Length == 5)
                        {
                            #region
                            for (int i = 0; i < n; i++)
                            {
                                udv_Ticket list_ssq_dt = new udv_Ticket()
                                {
                                    LotteryCode = ticket.LotteryCode,
                                    SchemeID = ticket.SchemeID,
                                    PlayTypeCode = ticket.PlayTypeCode,
                                    Number = string.Join("", first_num) + second_num[i] + "|" + string.Join("", blue),
                                    Multiple = ticket.Multiple,
                                    PickWayID = blue.Length == 1 ? PickWay_DS : PickWay_FS,
                                    Bet = 1 * blue.Length,
                                    SDID = ticket.SDID,
                                    Money = 2 * 1 * blue.Length * ticket.Multiple
                                };
                                list.Add(list_ssq_dt);
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                else
                {
                    #region
                    string[] num = ticket.Number.Split('|');
                    string[] first_num = num[0].Split(',');     //Red   接口红球不能超过16个数字
                    string[] second_num = num[1].Split(',');    //Blue
                    int bet = 1;

                    if (first_num.Length == 6 && second_num.Length == 1)
                    {
                        udv_Ticket list_ssq = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = string.Join("", first_num) + "|" + string.Join("", second_num),
                            Multiple = ticket.Multiple,
                            PickWayID = PickWay_DS,
                            Bet = bet,
                            SDID = ticket.SDID,
                            Money = 2 * bet * ticket.Multiple
                        };
                        list.Add(list_ssq);
                    }
                    else
                    {
                        decimal n = first_num.Length;
                        bet = Convert.ToInt32((n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) / 720) * second_num.Length);

                        long money = 2 * bet;   //按照一倍来算是否超过2万
                        if (money < 20000)
                        {
                            udv_Ticket list_ssq = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", first_num) + "|" + string.Join("", second_num),
                                Multiple = ticket.Multiple,
                                PickWayID = PickWay_FS,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_ssq);
                        }
                        else if (first_num.Length <= 16)
                        {
                            foreach (string BlueStr in second_num)
                            {
                                bet = Convert.ToInt32((n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) / 720) * 1);
                                udv_Ticket list_ssq = new udv_Ticket()
                                {
                                    LotteryCode = ticket.LotteryCode,
                                    SchemeID = ticket.SchemeID,
                                    PlayTypeCode = ticket.PlayTypeCode,
                                    Number = string.Join("", first_num) + "|" + BlueStr,
                                    Multiple = ticket.Multiple,
                                    PickWayID = PickWay_FS,
                                    Bet = bet,
                                    SDID = ticket.SDID,
                                    Money = 2 * bet * ticket.Multiple
                                };
                                list.Add(list_ssq);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < n - 5; i++)
                            {
                                for (int j = i + 1; j < n - 4; j++)
                                {
                                    for (int x = j + 1; x < n - 3; x++)
                                    {
                                        for (int y = x + 1; y < n - 2; y++)
                                        {
                                            for (int z = y + 1; z < n - 1; z++)
                                            {
                                                for (int k = z + 1; k < n; k++)
                                                {
                                                    string[] RedNum = { first_num[i], first_num[j], first_num[x], first_num[y], first_num[z], first_num[k] };
                                                    udv_Ticket list_ssq = new udv_Ticket()
                                                    {
                                                        LotteryCode = ticket.LotteryCode,
                                                        SchemeID = ticket.SchemeID,
                                                        PlayTypeCode = ticket.PlayTypeCode,
                                                        Number = string.Join("", RedNum) + "|" + string.Join("", second_num),
                                                        Multiple = ticket.Multiple,
                                                        PickWayID = PickWay_FS,
                                                        Bet = second_num.Length,
                                                        SDID = ticket.SDID,
                                                        Money = 2 * second_num.Length * ticket.Multiple
                                                    };
                                                    list.Add(list_ssq);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
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
            var s_list = MergeTicket(list, PickWay_DS, SINGLE_NUMBER_PER_TICKET, SEP_NUM_F);
            return s_list.ToList<udv_Ticket>();
        }
    }
}
