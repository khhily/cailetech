using CL.View.Entity.ElectronicTicket;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Tools.LotterySplitTickets.HuaYang.Tickets
{
    /// <summary>
    /// 超级大乐透
    /// 2017年5月9日
    /// </summary>
    public class CJDLT : TickBuilder
    {
        /// <summary>
        /// 彩种编号
        /// </summary>
        private const int LotteryCode = 901;
        private const int SuppId = 106;  //接口编码

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
                long money = rawTicket.PlayCode == 90101 ? 2 : 3;     //追加玩法是一注3元
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
                    SDID = rawTicket.SDID,
                    Money = money * rawTicket.Multiple * rawTicket.Bet
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
                //投注格式：
                //标准选号：
                //单式：01,02,03,04,05|01,02
                //复式：01,02,03,04,05,06|01,02
                //          01,02,03,04,05|01,02,03
                //          01,02,03,04,05,06|01,02,03

                //后区不设置胆拖，没有胆拖格式投注，只能拆分为单式格式
                //胆拖选号：01,02,03,04#05,06|01,02
                //          01,02,03,04#05,06|01,02,03
                //          01,02,03#04,05,06|01,02,03
                //          01,02#03,04,05,06|01,02,03
                //          01#02,03,04,05,06|01,02,03

                //接口格式
                //单式：0102030405|0102
                //复式：010203040506|0102
                //          0102030405|010203
                //          010203040506|010203

                long BaseMoney = ticket.PlayTypeCode == 90101 ? 2 : 3;     //追加玩法是一注3元
                int bet = 0;
                if (ticket.Number.IndexOf('#') > -1)    //是否胆拖 前段没有后去胆拖，转为单式或复式格式投注
                {
                    #region
                    string[] num = ticket.Number.Split('|');
                    string[] QianQu = num[0].Split('#');
                    string[] HouQu = num[1].Split(',');
                    string[] QianQu_first_num = QianQu[0].Split(',');
                    string[] QianQu_second_num = QianQu[1].Split(',');

                    int n = QianQu_second_num.Length;
                    bet = Convert.ToInt32((5 * (5 - 1) * (5 - 2) * (5 - 3) * (5 - 4) / 120) * (HouQu.Length * (HouQu.Length - 1) / 2));

                    if (QianQu_first_num.Length == 1)
                    {
                        #region
                        if (n < 4) continue;
                        for (int i = 0; i < n - 3; i++)
                        {
                            for (int j = i + 1; j < n - 2; j++)
                            {
                                for (int k = j + 1; k < n - 1; k++)
                                {
                                    for (int a = k + 1; a < n; a++)
                                    {
                                        udv_Ticket list_dlt_dt = new udv_Ticket()
                                        {
                                            LotteryCode = ticket.LotteryCode,
                                            SchemeID = ticket.SchemeID,
                                            PlayTypeCode = ticket.PlayTypeCode,
                                            Number = string.Join("", QianQu_first_num) + QianQu_second_num[i] + QianQu_second_num[j] + QianQu_second_num[k] + QianQu_second_num[a] + "|" + string.Join("", HouQu),
                                            Multiple = ticket.Multiple,
                                            PickWayID = HouQu.Length == 2 ? PickWay_DS : PickWay_FS,
                                            Bet = bet,
                                            SDID = ticket.SDID,
                                            Money = BaseMoney * bet * ticket.Multiple
                                        };
                                        list.Add(list_dlt_dt);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else if (QianQu_first_num.Length == 2)
                    {
                        #region
                        if (n < 3) continue;
                        for (int i = 0; i < n - 2; i++)
                        {
                            for (int j = i + 1; j < n - 1; j++)
                            {
                                for (int k = j + 1; k < n; k++)
                                {
                                    udv_Ticket list_dlt_dt = new udv_Ticket()
                                    {
                                        LotteryCode = ticket.LotteryCode,
                                        SchemeID = ticket.SchemeID,
                                        PlayTypeCode = ticket.PlayTypeCode,
                                        Number = string.Join("", QianQu_first_num) + QianQu_second_num[i] + QianQu_second_num[j] + QianQu_second_num[k] + "|" + string.Join("", HouQu),
                                        Multiple = ticket.Multiple,
                                        PickWayID = HouQu.Length == 2 ? PickWay_DS : PickWay_FS,
                                        Bet = bet,
                                        SDID = ticket.SDID,
                                        Money = BaseMoney * bet * ticket.Multiple
                                    };
                                    list.Add(list_dlt_dt);
                                }
                            }
                        }
                        #endregion
                    }
                    else if (QianQu_first_num.Length == 3)
                    {
                        #region
                        if (n < 2) continue;
                        for (int i = 0; i < n - 1; i++)
                        {
                            for (int j = i + 1; j < n; j++)
                            {
                                udv_Ticket list_dlt_dt = new udv_Ticket()
                                {
                                    LotteryCode = ticket.LotteryCode,
                                    SchemeID = ticket.SchemeID,
                                    PlayTypeCode = ticket.PlayTypeCode,
                                    Number = string.Join("", QianQu_first_num) + QianQu_second_num[i] + QianQu_second_num[j] + "|" + string.Join("", HouQu),
                                    Multiple = ticket.Multiple,
                                    PickWayID = HouQu.Length == 2 ? PickWay_DS : PickWay_FS,
                                    Bet = bet,
                                    SDID = ticket.SDID,
                                    Money = BaseMoney * bet * ticket.Multiple
                                };
                                list.Add(list_dlt_dt);
                            }
                        }
                        #endregion
                    }
                    else if (QianQu_first_num.Length == 4)
                    {
                        #region
                        foreach (string QQStr in QianQu_second_num)
                        {
                            udv_Ticket list_dlt_dt = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", QianQu_first_num) + QQStr + "|" + string.Join("", HouQu),
                                Multiple = ticket.Multiple,
                                PickWayID = HouQu.Length == 2 ? PickWay_DS : PickWay_FS,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = BaseMoney * bet * ticket.Multiple
                            };
                            list.Add(list_dlt_dt);
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region
                    string[] num = ticket.Number.Split(SEP_NUM_G);
                    string[] QianQu = num[0].Split(SEP_NUM_A);
                    string[] HouQu = num[1].Split(SEP_NUM_A);

                    if (QianQu.Length == 5 && HouQu.Length == 2)
                    {
                        udv_Ticket list_dlt = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = string.Join("", QianQu) + "|" + string.Join("", HouQu),
                            Multiple = ticket.Multiple,
                            PickWayID = PickWay_DS,
                            Bet = 1,
                            SDID = ticket.SDID,
                            Money = BaseMoney * 1 * ticket.Multiple
                        };
                        list.Add(list_dlt);
                    }
                    else
                    {
                        #region
                        decimal m = QianQu.Length;
                        decimal qbet = m * (m - 1) * (m - 2) * (m - 3) * (m - 4) / 120;
                        decimal n = HouQu.Length;
                        decimal hbet = n * (n - 1) / 2;
                        bet = Convert.ToInt32(qbet * hbet);

                        long money = BaseMoney * bet;   //按照一倍来算是否超过2万
                        if (money < 20000)
                        {
                            udv_Ticket list_dlt = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", QianQu) + "|" + string.Join("", HouQu),
                                Multiple = ticket.Multiple,
                                PickWayID = PickWay_FS,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = BaseMoney * bet * ticket.Multiple
                            };
                            list.Add(list_dlt);
                        }
                        else if ((QianQu.Length <= 18 && ticket.PlayTypeCode == 90101) ||
                            (QianQu.Length <= 17 && ticket.PlayTypeCode == 90102))
                        {
                            List<string> NewHouQu = new List<string>();
                            for (int a = 0; a < HouQu.Length - 1; a++)
                            {
                                for (int b = a + 1; b < HouQu.Length; b++)
                                {
                                    NewHouQu.Add(HouQu[a] + HouQu[b]);
                                }
                            }

                            //前区最多18个（追加17个）号码，不然拆出来的复式格式一样大于2万，只能拆成单式格式
                            foreach (string HQStr in NewHouQu)
                            {
                                bet = Convert.ToInt32(qbet * 1);
                                udv_Ticket list_dlt = new udv_Ticket()
                                {
                                    LotteryCode = ticket.LotteryCode,
                                    SchemeID = ticket.SchemeID,
                                    PlayTypeCode = ticket.PlayTypeCode,
                                    Number = string.Join("", QianQu) + "|" + HQStr,
                                    Multiple = ticket.Multiple,
                                    PickWayID = PickWay_FS,
                                    Bet = bet,
                                    SDID = ticket.SDID,
                                    Money = BaseMoney * bet * ticket.Multiple
                                };
                                list.Add(list_dlt);
                            }
                        }
                        else
                        {
                            string[] QianQuNum = new string[5];
                            for (int i = 0; i < m - 4; i++)
                            {
                                for (int j = i + 1; j < m - 3; j++)
                                {
                                    for (int x = j + 1; x < m - 2; x++)
                                    {
                                        for (int y = x + 1; y < m - 1; y++)
                                        {
                                            for (int z = y + 1; z < m; z++)
                                            {
                                                //QianQuNum = { QianQu[i], QianQu[j], QianQu[x], QianQu[y], QianQu[z] };
                                                QianQuNum[0] = QianQu[i];
                                                QianQuNum[1] = QianQu[j];
                                                QianQuNum[2] = QianQu[x];
                                                QianQuNum[3] = QianQu[y];
                                                QianQuNum[4] = QianQu[z];
                                                bet = Convert.ToInt32((5 * (5 - 1) * (5 - 2) * (5 - 3) * (5 - 4) / 120) * HouQu.Length);

                                                udv_Ticket list_dlt = new udv_Ticket()
                                                {
                                                    LotteryCode = ticket.LotteryCode,
                                                    SchemeID = ticket.SchemeID,
                                                    PlayTypeCode = ticket.PlayTypeCode,
                                                    Number = string.Join("", QianQuNum) + "|" + string.Join("", HouQu),
                                                    Multiple = ticket.Multiple,
                                                    PickWayID = HouQu.Length == 2 ? PickWay_DS : PickWay_FS,
                                                    Bet = bet,
                                                    SDID = ticket.SDID,
                                                    Money = BaseMoney * bet * ticket.Multiple
                                                };
                                                list.Add(list_dlt);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
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
                int BaseMoney = ticket.PlayTypeCode == 90101 ? 2 : 3;
                var money_per_num = BaseMoney * ticket.Bet;
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
