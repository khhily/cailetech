using CL.Enum.Common.Lottery;
using CL.Redis.BLL;
using CL.Tools.TicketInterface.arithmetic;
using CL.View.Entity.BasicTable;
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
    public class CQSSC : TickBuilder
    {
        /// <summary>
        /// 彩种编号
        /// </summary>
        private const int LotteryCode = 301;
        //private const int SuppId = ;  //接口编码

        #region 分隔符
        private const char SEP_NUM_A = ',';
        private const char SEP_NUM_B = '#';
        private const char SEP_NUM_C = '|';
        private const char SEP_NUM_D = '^';
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
            var bet = 0; //注数,默认0
            udv_Ticket tList;
            string[] numArray;
            string number = string.Empty;
            List<ValueTable> ValueTableEntitys = null;
            foreach (var ticket in tickets)
            {
                switch (ticket.PlayTypeCode)
                {
                    case (int)CQSSCPlayCode.ZX_1X:
                        #region 一星直选单式(拆成单式)
                        string[] YX_Number = ticket.Number.Split(SEP_NUM_A);
                        for (int i = 0; i < YX_Number.Length; i++)
                        {
                            tList = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = YX_Number[i],
                                Multiple = ticket.Multiple,
                                PickWayID = PickWay_DS,
                                Bet = 1,
                                SDID = ticket.SDID,
                                Money = 2 * 1 * ticket.Multiple
                            };
                            list.Add(tList);
                        }

                        #endregion
                        break;
                    case (int)CQSSCPlayCode.ZX_2X:
                        #region 二星直选
                        bet = CQSSCSF.GetBet(SEP_NUM_C, SEP_NUM_A, ticket.Number);
                        number = ticket.Number.Replace(SEP_NUM_A.ToString(), string.Empty).Replace(SEP_NUM_C, '*');
                        tList = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = number,
                            Multiple = ticket.Multiple,
                            PickWayID = bet > 1 ? PickWay_FS : PickWay_DS,
                            Bet = bet,
                            SDID = ticket.SDID,
                            Money = 2 * bet * ticket.Multiple
                        };
                        list.Add(tList);
                        #endregion
                        break;
                    case (int)CQSSCPlayCode.ZXHZ_2X:
                        #region 二星直选和值
                        string[] EXZXHZ_Number = ticket.Number.Split(SEP_NUM_A);
                        ValueTableEntitys = new BasicTableRedis().ValueBasicTableRedis((int)CQSSCPlayCode.ZXHZ_2X);
                        for (int i = 0; i < EXZXHZ_Number.Length; i++)
                        {
                            var ZXHZ_2X = ValueTableEntitys.Where(w => w.SumValue == EXZXHZ_Number[i].Trim()).FirstOrDefault();
                            tList = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = number,
                                Multiple = ticket.Multiple,
                                PickWayID = ZXHZ_2X.Bet > 1 ? PickWay_FS : PickWay_DS,
                                Bet = ZXHZ_2X.Bet,
                                SDID = ticket.SDID,
                                Money = 2 * ZXHZ_2X.Bet * ticket.Multiple
                            };
                            list.Add(tList);
                        }
                        #endregion
                        break;
                    case (int)CQSSCPlayCode.ZU_2X:
                        #region 二星组选
                        numArray = ticket.Number.Split(SEP_NUM_A);
                        //组合算法C(M,N)
                        if (numArray.Length >= 2)
                        {
                            bet = CQSSCSF.GetJC(numArray.Length) / (CQSSCSF.GetJC(2) * CQSSCSF.GetJC(numArray.Length - 2));
                            if (bet > 1) //复式
                            {
                                number = string.Join(string.Empty, numArray);
                            }
                            else
                            {
                                number = string.Join("*", numArray);
                            }
                        }
                        tList = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = number,
                            Multiple = ticket.Multiple,
                            PickWayID = bet > 1 ? PickWay_FS : PickWay_DS,
                            Bet = bet,
                            SDID = ticket.SDID,
                            Money = 2 * bet * ticket.Multiple
                        };
                        list.Add(tList);
                        #endregion
                        break;
                    case (int)CQSSCPlayCode.ZUHZ_2X:
                        #region 二星组选和值
                        string[] ZUHZ_2X_Number = ticket.Number.Split(SEP_NUM_A);
                        ValueTableEntitys = new BasicTableRedis().ValueBasicTableRedis((int)CQSSCPlayCode.ZUHZ_2X);
                        for (int i = 0; i < ZUHZ_2X_Number.Length; i++)
                        {
                            var ZUHZ_2X = ValueTableEntitys.Where(w => w.SumValue == ZUHZ_2X_Number[i].Trim()).FirstOrDefault();
                            tList = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = number,
                                Multiple = ticket.Multiple,
                                PickWayID = ZUHZ_2X.Bet > 1 ? PickWay_FS : PickWay_DS,
                                Bet = ZUHZ_2X.Bet,
                                SDID = ticket.SDID,
                                Money = 2 * ZUHZ_2X.Bet * ticket.Multiple
                            };
                            list.Add(tList);
                        }
                        #endregion
                        break;
                    case (int)CQSSCPlayCode.ZX_3X:
                        #region 三星直选
                        bet = CQSSCSF.GetBet(SEP_NUM_C, SEP_NUM_A, ticket.Number);
                        number = ticket.Number.Replace(SEP_NUM_A.ToString(), string.Empty).Replace(SEP_NUM_C, '*');
                        tList = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = number,
                            Multiple = ticket.Multiple,
                            PickWayID = bet > 1 ? PickWay_FS : PickWay_DS,
                            Bet = bet,
                            SDID = ticket.SDID,
                            Money = 2 * bet * ticket.Multiple
                        };
                        list.Add(tList);
                        #endregion
                        break;
                    case (int)CQSSCPlayCode.ZU3_3X:
                        #region 三星组三
                        bet = CQSSCSF.GetBet(SEP_NUM_A, ticket.Number);
                        numArray = ticket.Number.Split(SEP_NUM_A);
                        if (bet > 1) //复式
                        {
                            number = string.Join(string.Empty, numArray);
                        }
                        else
                        {
                            number = string.Join("*", numArray);
                        }

                        tList = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = number,
                            Multiple = ticket.Multiple,
                            PickWayID = bet > 1 ? PickWay_FS : PickWay_DS,
                            Bet = bet,
                            SDID = ticket.SDID,
                            Money = 2 * bet * ticket.Multiple
                        };
                        list.Add(tList);
                        #endregion
                        break;
                    case (int)CQSSCPlayCode.ZU6_3X:
                        #region 三星组六
                        numArray = ticket.Number.Split(SEP_NUM_A);
                        //注数
                        bet = CQSSCSF.GetJC(numArray.Length) / (CQSSCSF.GetJC(3) * CQSSCSF.GetJC(numArray.Length - 3));
                        if (bet > 1) //复式
                        {
                            number = string.Join(string.Empty, numArray);
                        }
                        else
                        {
                            number = string.Join("*", numArray);
                        }
                        tList = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = number,
                            Multiple = ticket.Multiple,
                            PickWayID = bet > 1 ? PickWay_FS : PickWay_DS,
                            Bet = bet,
                            SDID = ticket.SDID,
                            Money = 2 * bet * ticket.Multiple
                        };
                        list.Add(tList);
                        #endregion
                        break;
                    case (int)CQSSCPlayCode.ZX_5X:
                        #region 五星直选
                        numArray = ticket.Number.Split(SEP_NUM_C);
                        int[] num_w = numArray[0].Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                        int[] num_q = numArray[1].Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                        int[] num_b = numArray[2].Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                        int[] num_s = numArray[3].Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                        int[] num_g = numArray[4].Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                        for (int w = 0; w < num_w.Length; w++)
                            for (int q = 0; q < num_q.Length; q++)
                                for (int b = 0; b < num_b.Length; b++)
                                    for (int s = 0; s < num_s.Length; s++)
                                        for (int g = 0; g < num_g.Length; g++)
                                        {
                                            tList = new udv_Ticket()
                                            {
                                                LotteryCode = ticket.LotteryCode,
                                                SchemeID = ticket.SchemeID,
                                                PlayTypeCode = ticket.PlayTypeCode,
                                                Number = string.Format("{0}*{1}*{2}*{3}*{4}", num_w[w], num_q[q], num_b[b], num_s[s], num_g[g]),
                                                Multiple = ticket.Multiple,
                                                PickWayID = bet > 1 ? PickWay_FS : PickWay_DS,
                                                Bet = 1,
                                                SDID = ticket.SDID,
                                                Money = 2 * 1 * ticket.Multiple
                                            };
                                            list.Add(tList);
                                        }
                        #endregion
                        break;
                    case (int)CQSSCPlayCode.TX_5X:
                        #region 五星通选
                        //2,3|0,1|0,1|0,2|0,2
                        numArray = ticket.Number.Split(SEP_NUM_C);
                        int[] numArray_w = numArray[0].Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                        int[] numArray_q = numArray[1].Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                        int[] numArray_b = numArray[2].Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                        int[] numArray_s = numArray[3].Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                        int[] numArray_g = numArray[4].Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                        for (int w = 0; w < numArray_w.Length; w++)
                            for (int q = 0; q < numArray_q.Length; q++)
                                for (int b = 0; b < numArray_b.Length; b++)
                                    for (int s = 0; s < numArray_s.Length; s++)
                                        for (int g = 0; g < numArray_g.Length; g++)
                                        {
                                            tList = new udv_Ticket()
                                            {
                                                LotteryCode = ticket.LotteryCode,
                                                SchemeID = ticket.SchemeID,
                                                PlayTypeCode = ticket.PlayTypeCode,
                                                Number = string.Format("{0}*{1}*{2}*{3}*{4}", numArray_w[w], numArray_q[q], numArray_b[b], numArray_s[s], numArray_g[g]),
                                                Multiple = ticket.Multiple,
                                                PickWayID = bet > 1 ? PickWay_FS : PickWay_DS,
                                                Bet = 1,
                                                SDID = ticket.SDID,
                                                Money = 2 * 1 * ticket.Multiple
                                            };
                                            list.Add(tList);
                                        }
                        #endregion
                        break;
                    case (int)CQSSCPlayCode.DXDS:
                        #region 大小单双
                        bet = 1;
                        numArray = ticket.Number.Split(SEP_NUM_C);
                        tList = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = string.Join("*", numArray),
                            Multiple = ticket.Multiple,
                            PickWayID = PickWay_DS,
                            Bet = bet,
                            SDID = ticket.SDID,
                            Money = 2 * bet * ticket.Multiple
                        };
                        list.Add(tList);
                        #endregion
                        break;
                    default:
                        list.Add(ticket);
                        break;
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
            var list_exzxhz = list.FindAll(x => x.PlayTypeCode == (int)CQSSCPlayCode.ZUHZ_2X);
            var s_list = MergeTicket(list.FindAll(x => x.PlayTypeCode != (int)CQSSCPlayCode.ZX_1X && x.PlayTypeCode != (int)CQSSCPlayCode.ZUHZ_2X), PickWay_DS, SINGLE_NUMBER_PER_TICKET, SEP_NUM_A);
            var result = list.FindAll(x => x.PlayTypeCode == (int)CQSSCPlayCode.ZX_1X && x.PlayTypeCode != (int)CQSSCPlayCode.ZUHZ_2X).Concat(s_list).ToList<udv_Ticket>();
            result.AddRange(list_exzxhz);
            return result;
        }
    }
}
