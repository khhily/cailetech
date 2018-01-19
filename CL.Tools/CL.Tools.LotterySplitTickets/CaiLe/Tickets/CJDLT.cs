using CL.View.Entity.ElectronicTicket;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Tools.LotterySplitTickets.CaiLe.Tickets
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
                    Money = money * rawTicket.Multiple * rawTicket.Bet,
                    SDID = rawTicket.SDID
                };
                list.Add(ticket);
            }
            list = NumberSplit(list);
            list = MultipleSplit(list);
            list = MergeTicket(list);
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
                //胆拖选号：01,02,03,04#05,06|01#02
                //          01,02,03,04#05,06|01#02,03
                //          01,02,03#04,05,06|01#02,03
                //          01,02#03,04,05,06|01#02,03
                //          01#02,03,04,05,06|01#02,03

                //接口格式
                //单式：0102030405|0102
                //复式：010203040506|0102
                //          0102030405|010203
                //          010203040506|010203
                //胆拖选号：01020304*0506|01*02
                //	        01020304*0506|01*0203
                //	        010203*040506|01*0203
                //	        0102*03040506|01*0203
                //         01*0203040506|01*0203

                long BaseMoney = ticket.PlayTypeCode == 90101 ? 2 : 3;     //追加玩法是一注3元
                int bet = 0;
                if (ticket.Number.IndexOf('#') > -1)    //是否胆拖
                {
                    #region
                    string[] arr = ticket.Number.Split('|');
                    string[] QianQu = arr[0].Split('#');
                    string[] HouQu = arr[1].Split('#');

                    string[] QianQu_first_num = QianQu[0].Split(',');
                    string[] QianQu_second_num = QianQu[1].Split(',');
                    string[] HouQu_first_num = HouQu[0].Split(',');
                    string[] HouQu_second_num = HouQu[1].Split(',');

                    decimal n = QianQu_second_num.Length;
                    decimal w = HouQu_second_num.Length;

                    if (QianQu_first_num.Length == 1)
                    {
                        bet = Convert.ToInt32((n * (n - 1) * (n - 2) * (n - 3) / 24) * w);
                    }
                    else if (QianQu_first_num.Length == 2)
                    {
                        bet = Convert.ToInt32((n * (n - 1) * (n - 2) / 6) * w);
                    }
                    else if (QianQu_first_num.Length == 3)
                    {
                        bet = Convert.ToInt32((n * (n - 1) / 2) * w);
                    }
                    else if (QianQu_first_num.Length == 4)
                    {
                        bet = QianQu_second_num.Length * HouQu_second_num.Length;
                    }

                    udv_Ticket list_dlt_dt = new udv_Ticket()
                    {
                        LotteryCode = ticket.LotteryCode,
                        SchemeID = ticket.SchemeID,
                        PlayTypeCode = ticket.PlayTypeCode,
                        Number = string.Join("", QianQu_first_num) + "*" + string.Join("", QianQu_second_num) + "|" + string.Join("", HouQu_first_num) + "*" + string.Join("", HouQu_second_num),
                        Multiple = ticket.Multiple,
                        PickWayID = PickWay_DT,
                        Bet = bet,
                        SDID = ticket.SDID,
                        Money = BaseMoney * bet * ticket.Multiple
                    };
                    list.Add(list_dlt_dt);
                    #endregion
                }
                else
                {
                    #region
                    string[] num = ticket.Number.Split('|');
                    string[] QianQu = num[0].Split(',');
                    string[] HouQu = num[1].Split(',');

                    if (QianQu.Length == 5 && HouQu.Length == 2)
                    {
                        bet = 1;
                        udv_Ticket list_dlt = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = string.Join("", QianQu) + "|" + string.Join("", HouQu),
                            Multiple = ticket.Multiple,
                            PickWayID = PickWay_DS,
                            Bet = bet,
                            SDID = ticket.SDID,
                            Money = BaseMoney * bet * ticket.Multiple
                        };
                        list.Add(list_dlt);
                    }
                    else
                    {
                        #region
                        decimal m = QianQu.Length;
                        decimal qbet = (m * (m - 1) * (m - 2) * (m - 3) * (m - 4) / 120);
                        decimal n = HouQu.Length;
                        decimal hbet = (n * (n - 1) / 2);
                        bet = Convert.ToInt32(qbet * hbet);

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
