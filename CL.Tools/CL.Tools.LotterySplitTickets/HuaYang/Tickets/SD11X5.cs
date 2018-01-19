using CL.Enum.Common.Lottery;
using CL.View.Entity.ElectronicTicket;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Tools.LotterySplitTickets.HuaYang.Tickets
{
    /// <summary>
    /// 山东十一云夺金
    /// 2017年5月9日
    /// </summary>
    public class SD11X5 : TickBuilder
    {
        /// <summary>
        /// 彩种编号
        /// </summary>
        private const int LotteryCode = 202;
        private const int SuppId = 112;  //接口编码

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
        /// <summary>
        /// 定位复式
        /// </summary>
        private const int PickWay_DWFS = 4;
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
                switch (ticket.PlayTypeCode)
                {
                    case (int)SD11X5PlayCode.R2:
                        #region 任选二
                        //示例：标准选号：单式：01,02 复式：01,02,03 胆拖选号：01#02,03
                        //单式：0102   复式：010203   胆拖：01*020304
                        if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        {
                            #region
                            string[] arr_rxe = ticket.Number.Split(SEP_NUM_B);
                            string first_num = arr_rxe[0];
                            string[] second_num = arr_rxe[1].Split(SEP_NUM_A);
                            udv_Ticket list_rxe_dt = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = first_num + "*" + string.Join("", second_num),
                                Multiple = ticket.Multiple,
                                PickWayID = PickWay_DT,
                                Bet = second_num.Length,
                                SDID = ticket.SDID,
                                Money = 2 * second_num.Length * ticket.Multiple
                            };
                            list.Add(list_rxe_dt);
                            #endregion
                        }
                        else
                        {
                            #region
                            string[] num_rxe = ticket.Number.Split(SEP_NUM_A);
                            int pickwayid = 0;
                            int bet = 0;
                            if (num_rxe.Length == 2)
                            {
                                pickwayid = PickWay_DS;
                                bet = 1;
                            }
                            else if (num_rxe.Length > 2)
                            {
                                pickwayid = PickWay_FS;
                                decimal n = num_rxe.Length;
                                bet = Convert.ToInt32(n * (n - 1) / 2);
                            }
                            udv_Ticket list_rxe = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", num_rxe),
                                Multiple = ticket.Multiple,
                                PickWayID = pickwayid,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxe);
                            #endregion
                        }
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.R3:
                        #region 任选三
                        //示例：标准选号：单式：01,02,03 复式：01,02,03,04 胆拖选号：01#02,03,04、01,02#03,04
                        //单式：010203   复式：01020304   胆拖：01*020304、0102*0304
                        if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        {
                            #region
                            string[] arr_rxs = ticket.Number.Split(SEP_NUM_B);
                            string[] first_num = arr_rxs[0].Split(SEP_NUM_A);
                            string[] second_num = arr_rxs[1].Split(SEP_NUM_A);
                            int bet = 0;
                            if (first_num.Length == 1)
                            {
                                decimal n = second_num.Length;
                                bet = Convert.ToInt32(n * (n - 1) / 2);
                            }
                            else if (first_num.Length == 2)
                            {
                                bet = second_num.Length;
                            }
                            udv_Ticket list_rxs_dt = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", first_num) + "*" + string.Join("", second_num),
                                Multiple = ticket.Multiple,
                                PickWayID = PickWay_DT,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxs_dt);
                            #endregion
                        }
                        else
                        {
                            #region
                            string[] num_rxs = ticket.Number.Split(SEP_NUM_A);
                            int pickwayid = 0;
                            int bet = 0;
                            if (num_rxs.Length == 3)
                            {
                                pickwayid = PickWay_DS;
                                bet = 1;
                            }
                            else if (num_rxs.Length > 3)
                            {
                                pickwayid = PickWay_FS;
                                decimal n = num_rxs.Length;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                            }
                            udv_Ticket list_rxs = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", num_rxs),
                                Multiple = ticket.Multiple,
                                PickWayID = pickwayid,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxs);
                            #endregion
                        }
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.R4:
                        #region 任选四
                        //示例：标准选号：单式：01,02,03,04 复式：01,02,03,04,05 胆拖选号：01#02,03,04,05、01,02,03#04,05
                        //单式：01020304   复式：0102030405   胆拖：01*02030405、010203*0405
                        if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        {
                            #region
                            string[] arr_rxs = ticket.Number.Split(SEP_NUM_B);
                            string[] first_num = arr_rxs[0].Split(SEP_NUM_A);
                            string[] second_num = arr_rxs[1].Split(SEP_NUM_A);
                            decimal n = second_num.Length;
                            int bet = 1;
                            if (first_num.Length == 1)
                            {
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                            }
                            else if (first_num.Length == 2)
                            {
                                bet = Convert.ToInt32(n * (n - 1) / 2);
                            }
                            else if (first_num.Length == 3)
                            {
                                bet = second_num.Length;
                            }

                            udv_Ticket list_rxs_dt = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", first_num) + "*" + string.Join("", second_num),
                                Multiple = ticket.Multiple,
                                PickWayID = PickWay_DT,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxs_dt);
                            #endregion
                        }
                        else
                        {
                            #region
                            string[] num_rxs = ticket.Number.Split(SEP_NUM_A);
                            int pickwayid = 0;
                            int bet = 0;
                            if (num_rxs.Length == 4)
                            {
                                pickwayid = PickWay_DS;
                                bet = 1;
                            }
                            else if (num_rxs.Length > 4)
                            {
                                pickwayid = PickWay_FS;
                                decimal n = num_rxs.Length;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                            }
                            udv_Ticket list_rxs = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", num_rxs),
                                Multiple = ticket.Multiple,
                                PickWayID = pickwayid,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxs);
                            #endregion
                        }
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.R5:
                        #region 任选五
                        //示例：示例：标准选号：单式：01,02,03,04,05 复式：01,02,03,04,05,06 胆拖选号：01#02,03,04,05,06、01,02,03,04#05,06
                        //单式：0102030405   复式：010203040506   胆拖：01*0203040506、01020304*0506
                        if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        {
                            #region
                            string[] arr_rxw = ticket.Number.Split(SEP_NUM_B);
                            string[] first_num = arr_rxw[0].Split(SEP_NUM_A);
                            string[] second_num = arr_rxw[1].Split(SEP_NUM_A);
                            decimal n = second_num.Length;
                            int bet = 1;
                            if (first_num.Length == 1)
                            {
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                            }
                            else if (first_num.Length == 2)
                            {
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                            }
                            else if (first_num.Length == 3)
                            {
                                bet = Convert.ToInt32(n * (n - 1) / 2);
                            }
                            else if (first_num.Length == 4)
                            {
                                bet = second_num.Length;
                            }

                            udv_Ticket list_rxw_dt = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", first_num) + "*" + string.Join("", second_num),
                                Multiple = ticket.Multiple,
                                PickWayID = PickWay_DT,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxw_dt);
                            #endregion
                        }
                        else
                        {
                            #region
                            string[] num_rxw = ticket.Number.Split(SEP_NUM_A);
                            int pickwayid = 0;
                            int bet = 0;
                            if (num_rxw.Length == 5)
                            {
                                pickwayid = PickWay_DS;
                                bet = 1;
                            }
                            else if (num_rxw.Length > 5)
                            {
                                pickwayid = PickWay_FS;
                                decimal n = num_rxw.Length;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                            }
                            udv_Ticket list_rxw = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", num_rxw),
                                Multiple = ticket.Multiple,
                                PickWayID = pickwayid,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxw);
                            #endregion
                        }
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.R6:
                        #region 任选六
                        //示例：标准选号：单式：01,02,03,04,05,06 复式：01,02,03,04,05,06,07 胆拖选号：01#02,03,04,05,06,07、01,02,03,04,05#06,07
                        //单式：010203040506   复式：01020304050607   胆拖：01*020304050607、0102030405*0607
                        if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        {
                            #region
                            string[] arr_rxl = ticket.Number.Split(SEP_NUM_B);
                            string[] first_num = arr_rxl[0].Split(SEP_NUM_A);
                            string[] second_num = arr_rxl[1].Split(SEP_NUM_A);
                            decimal n = second_num.Length;
                            int bet = 1;
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

                            udv_Ticket list_rxl_dt = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", first_num) + "*" + string.Join("", second_num),
                                Multiple = ticket.Multiple,
                                PickWayID = PickWay_DT,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxl_dt);
                            #endregion
                        }
                        else
                        {
                            #region
                            string[] num_rxl = ticket.Number.Split(SEP_NUM_A);
                            int pickwayid = 0;
                            int bet = 0;
                            if (num_rxl.Length == 6)
                            {
                                pickwayid = PickWay_DS;
                                bet = 1;
                            }
                            else if (num_rxl.Length > 6)
                            {
                                pickwayid = PickWay_FS;
                                decimal n = num_rxl.Length;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) / 720);
                            }
                            udv_Ticket list_rxw = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", num_rxl),
                                Multiple = ticket.Multiple,
                                PickWayID = pickwayid,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxw);
                            #endregion
                        }
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.R7:
                        #region 任选七
                        //示例：标准选号：单式：01,02,03,04,05,06,07 复式：01,02,03,04,05,06,07,08 胆拖选号：01#02,03,04,05,06,07,08、01,02,03,04,05,06#07,08
                        //单式：01020304050607   复式：0102030405060708   胆拖：01*02030405060708、010203040506*0708
                        if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        {
                            #region
                            string[] arr_rxq = ticket.Number.Split(SEP_NUM_B);
                            string[] first_num = arr_rxq[0].Split(SEP_NUM_A);
                            string[] second_num = arr_rxq[1].Split(SEP_NUM_A);
                            decimal n = second_num.Length;
                            int bet = 1;
                            if (first_num.Length == 1)
                            {
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) / 720);
                            }
                            else if (first_num.Length == 2)
                            {
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                            }
                            else if (first_num.Length == 3)
                            {
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                            }
                            else if (first_num.Length == 4)
                            {
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                            }
                            else if (first_num.Length == 5)
                            {
                                bet = Convert.ToInt32(n * (n - 1) / 2);
                            }
                            else if (first_num.Length == 6)
                            {
                                bet = second_num.Length;
                            }

                            udv_Ticket list_rxl_dt = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", first_num) + "*" + string.Join("", second_num),
                                Multiple = ticket.Multiple,
                                PickWayID = PickWay_DT,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxl_dt);
                            #endregion
                        }
                        else
                        {
                            #region
                            string[] num_rxq = ticket.Number.Split(SEP_NUM_A);
                            int pickwayid = 0;
                            int bet = 0;
                            if (num_rxq.Length == 7)
                            {
                                pickwayid = PickWay_DS;
                                bet = 1;
                            }
                            else if (num_rxq.Length > 7)
                            {
                                pickwayid = PickWay_FS;
                                decimal n = num_rxq.Length;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) * (n - 6) / 5040);
                            }
                            udv_Ticket list_rxq = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", num_rxq),
                                Multiple = ticket.Multiple,
                                PickWayID = pickwayid,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxq);
                            #endregion
                        }
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.R8:
                        #region 任选八
                        //任选八只有单式投注，把所有复式拆成功单式
                        //示例：标准选号：单式：01,02,03,04,05,06,07,08 复式：01,02,03,04,05,06,07,08,09 胆拖选号：01#02,03,04,05,06,07,08,09、01,02,03,04,05,06,07#08,09
                        //单式：0102030405060708   复式：010203040506070809   胆拖：01*0203040506070809、01020304050607*0809
                        if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        {
                            #region
                            string[] arr_rxb = ticket.Number.Split(SEP_NUM_B);
                            string[] first_num = arr_rxb[0].Split(SEP_NUM_A);
                            string[] second_num = arr_rxb[1].Split(SEP_NUM_A);
                            decimal n = second_num.Length;
                            int bet = 1;
                            if (first_num.Length == 1)
                            {
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) * (n - 6) / 5040);
                            }
                            else if (first_num.Length == 2)
                            {
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) / 720);
                            }
                            else if (first_num.Length == 3)
                            {
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                            }
                            else if (first_num.Length == 4)
                            {
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                            }
                            else if (first_num.Length == 5)
                            {
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                            }
                            else if (first_num.Length == 6)
                            {
                                bet = Convert.ToInt32(n * (n - 1) / 2);
                            }
                            else if (first_num.Length == 7)
                            {
                                bet = second_num.Length;
                            }

                            udv_Ticket list_rxb_dt = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", first_num) + "*" + string.Join("", second_num),
                                Multiple = ticket.Multiple,
                                PickWayID = PickWay_DT,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxb_dt);
                            #endregion
                        }
                        else
                        {
                            #region 来个恶心的 复式拆成单式
                            string[] num_rxb = ticket.Number.Split(SEP_NUM_A);
                            int n = num_rxb.Length;
                            for (int i = 0; i < n - 7; i++)
                            {
                                for (int j = i + 1; j < n - 6; j++)
                                {
                                    for (int x = j + 1; x < n - 5; x++)
                                    {
                                        for (int y = x + 1; y < n - 4; y++)
                                        {
                                            for (int z = y + 1; z < n - 3; z++)
                                            {
                                                for (int a = z + 1; a < n - 2; a++)
                                                {
                                                    for (int b = a + 1; b < n - 1; b++)
                                                    {
                                                        for (int c = b + 1; c < n; c++)
                                                        {
                                                            string num = num_rxb[i] + num_rxb[j] + num_rxb[x] + num_rxb[y] + num_rxb[z] + num_rxb[a] + num_rxb[b] + num_rxb[c];
                                                            udv_Ticket list_rxb = new udv_Ticket()
                                                            {
                                                                LotteryCode = ticket.LotteryCode,
                                                                SchemeID = ticket.SchemeID,
                                                                PlayTypeCode = ticket.PlayTypeCode,
                                                                Number = num,
                                                                Multiple = ticket.Multiple,
                                                                PickWayID = PickWay_DS,
                                                                Bet = 1,
                                                                SDID = ticket.SDID,
                                                                Money = 2 * 1 * ticket.Multiple
                                                            };
                                                            list.Add(list_rxb);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //int pickwayid = PickWay_FS;
                            //int bet = 0;
                            //if (num_rxb.Length == 8)
                            //{
                            //        pickwayid = PickWay_DS;
                            //        bet = 1;
                            //}
                            //else if (num_rxb.Length > 8) //复式拆成功单式
                            //{
                            //    decimal n = num_rxb.Length;
                            //    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) * (n - 6) * (n - 7) / 40320);
                            //}
                            #endregion
                        }
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.Q1:
                        #region 前一
                        //示例：标准选号：单式：01 复式：01,02,03
                        //单式5注：01^02^03^04^05
                        //一票不能有重复数字
                        //01,02,03,04,05,06,07,08,09,10,11
                        string[] num_qy = ticket.Number.Split(SEP_NUM_A);
                        int ii = 0, len = 1;
                        List<string> ListNum = new List<string>();
                        foreach (string item in num_qy)
                        {
                            ii++;
                            ListNum.Add(item);
                            if (len == 5 || ii == num_qy.Length)
                            {
                                udv_Ticket list_qy = new udv_Ticket()
                                {
                                    LotteryCode = ticket.LotteryCode,
                                    SchemeID = ticket.SchemeID,
                                    PlayTypeCode = ticket.PlayTypeCode,
                                    Number = string.Join("^", ListNum),
                                    Multiple = ticket.Multiple,
                                    PickWayID = PickWay_DS,
                                    Bet = ListNum.Count,
                                    SDID = ticket.SDID,
                                    Money = 2 * ListNum.Count * ticket.Multiple
                                };
                                list.Add(list_qy);
                                len = 1;
                                ListNum.Clear();
                            }
                            else len++;
                        }
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.Q2_ZX:
                        #region 前二直选
                        //示例：直选选号：单式：01|02 定位复式：01|02,03、01,02|03
                        //单式：0102   定位：01 * 0203、0102 * 0304
                        string[] arr_qezx = ticket.Number.Split(SEP_NUM_G);
                        string[] first_num_qezx = arr_qezx[0].Split(SEP_NUM_A);
                        string[] second_num_qezx = arr_qezx[1].Split(SEP_NUM_A);
                        int pickwayid_qezx = 0;
                        int bet_qezx = 1;
                        string num_qezx = "";
                        if (first_num_qezx.Length == 1 && second_num_qezx.Length == 1)
                        {
                            pickwayid_qezx = PickWay_DS;
                            bet_qezx = 1;
                            num_qezx = string.Join("", first_num_qezx) + string.Join("", second_num_qezx);
                        }
                        else if (first_num_qezx.Length > 1 || second_num_qezx.Length > 1)
                        {
                            List<int> listnum = new List<int>();
                            for (int i = 0; i < first_num_qezx.Length; i++)
                            {
                                for (int j = 0; j < second_num_qezx.Length; j++)
                                {
                                    if (first_num_qezx[i] != second_num_qezx[j])
                                    {
                                        listnum.Add(1);
                                    }
                                }
                            }
                            pickwayid_qezx = PickWay_DWFS;
                            bet_qezx = listnum.Count;
                            num_qezx = string.Join("", first_num_qezx) + "*" + string.Join("", second_num_qezx);
                        }

                        udv_Ticket list_sbth = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = num_qezx,
                            Multiple = ticket.Multiple,
                            PickWayID = pickwayid_qezx,
                            Bet = bet_qezx,
                            SDID = ticket.SDID,
                            Money = 2 * bet_qezx * ticket.Multiple
                        };
                        list.Add(list_sbth);
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.Q2_ZUX:
                        #region 前二组选
                        //示例：标准选号：单式：01,02 复式：01,02,03 胆拖选号：01#02,03、02#01,03
                        //单式：0102 复式：010203 胆拖：01*0203
                        if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        {
                            #region
                            string[] arr_qezux = ticket.Number.Split(SEP_NUM_B);
                            string first_num = arr_qezux[0];
                            string[] second_num = arr_qezux[1].Split(SEP_NUM_A);
                            udv_Ticket list_qezx_dt = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = first_num + "*" + string.Join("", second_num),
                                Multiple = ticket.Multiple,
                                PickWayID = PickWay_DT,
                                Bet = second_num.Length,
                                SDID = ticket.SDID,
                                Money = 2 * second_num.Length * ticket.Multiple
                            };
                            list.Add(list_qezx_dt);
                            #endregion
                        }
                        else
                        {
                            #region
                            string[] num_qezux = ticket.Number.Split(SEP_NUM_A);
                            int pickwayid = 0;
                            int bet = 0;
                            if (num_qezux.Length == 2)
                            {
                                pickwayid = PickWay_DS;
                                bet = 1;
                            }
                            else if (num_qezux.Length > 2)
                            {
                                pickwayid = PickWay_FS;
                                decimal n = num_qezux.Length;
                                bet = Convert.ToInt32(n * ((n - 1) / 2));
                            }
                            udv_Ticket list_qezx = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", num_qezux),
                                Multiple = ticket.Multiple,
                                PickWayID = pickwayid,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_qezx);
                            #endregion
                        }
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.Q3_ZX:
                        #region 前三直选
                        //示例：直选选号：定位单式：01|02|03 定位复式：01,02,03|03,04,06|03,06,07
                        //单式：0102   定位：01 * 0203、0102 * 0304
                        string[] arr_qszx = ticket.Number.Split(SEP_NUM_G);
                        string[] first_num_qszx = arr_qszx[0].Split(SEP_NUM_A);
                        string[] second_num_qszx = arr_qszx[1].Split(SEP_NUM_A);
                        string[] third_num_qszx = arr_qszx[2].Split(SEP_NUM_A);
                        int pickwayid_qszx = 0;
                        int bet_qszx = 1;
                        string num_qszx = "";
                        if (first_num_qszx.Length == 1 && second_num_qszx.Length == 1 && third_num_qszx.Length == 1)
                        {
                            pickwayid_qszx = PickWay_DS;
                            bet_qszx = 1;
                            num_qszx = string.Join("", first_num_qszx) + string.Join("", second_num_qszx) + string.Join("", third_num_qszx);
                        }
                        else if (first_num_qszx.Length > 1 || second_num_qszx.Length > 1 || third_num_qszx.Length > 1)
                        {
                            List<int> listnum = new List<int>();
                            for (int i = 0; i < first_num_qszx.Length; i++)
                            {
                                for (int j = 0; j < second_num_qszx.Length; j++)
                                {
                                    if (first_num_qszx[i] != second_num_qszx[j])
                                    {
                                        for (int k = 0; k < third_num_qszx.Length; k++)
                                        {
                                            if (first_num_qszx[i] != third_num_qszx[k] && second_num_qszx[j] != third_num_qszx[k])
                                                listnum.Add(1);
                                        }
                                    }
                                }
                            }
                            pickwayid_qezx = PickWay_DWFS;
                            bet_qszx = listnum.Count;
                            num_qszx = string.Join("", first_num_qszx) + "*" + string.Join("", second_num_qszx) + "*" + string.Join("", third_num_qszx);
                        }

                        udv_Ticket list_qszx = new udv_Ticket()
                        {
                            LotteryCode = ticket.LotteryCode,
                            SchemeID = ticket.SchemeID,
                            PlayTypeCode = ticket.PlayTypeCode,
                            Number = num_qszx,
                            Multiple = ticket.Multiple,
                            PickWayID = pickwayid_qszx,
                            Bet = bet_qszx,
                            SDID = ticket.SDID,
                            Money = 2 * bet_qszx * ticket.Multiple
                        };
                        list.Add(list_qszx);
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.Q3_ZUX:
                        #region 前三组选
                        //示例：标准选号：单式：01,02,03 复式：01,02,03,04 胆拖选号：01,02#03,04、01#02,03,04
                        //单式：010203  复式：01020304  胆拖：01*020304、0102*0304
                        if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        {
                            #region
                            string[] arr_qszux = ticket.Number.Split(SEP_NUM_B);
                            string[] first_num = arr_qszux[0].Split(SEP_NUM_A);
                            string[] second_num = arr_qszux[1].Split(SEP_NUM_A);
                            int bet = 0;
                            if (first_num.Length == 1)
                            {
                                decimal n = second_num.Length;
                                bet = Convert.ToInt32(n * (n - 1) / 2);
                            }
                            else if (first_num.Length == 2)
                            {
                                bet = second_num.Length;
                            }
                            udv_Ticket list_rxs_dt = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", first_num) + "*" + string.Join("", second_num),
                                Multiple = ticket.Multiple,
                                PickWayID = PickWay_DT,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxs_dt);
                            #endregion
                        }
                        else
                        {
                            #region
                            string[] num_qszux = ticket.Number.Split(SEP_NUM_A);
                            int pickwayid = 0;
                            int bet = 0;
                            if (num_qszux.Length == 3)
                            {
                                pickwayid = PickWay_DS;
                                bet = 1;
                            }
                            else if (num_qszux.Length > 3)
                            {
                                pickwayid = PickWay_FS;
                                decimal n = num_qszux.Length;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                            }
                            udv_Ticket list_rxs = new udv_Ticket()
                            {
                                LotteryCode = ticket.LotteryCode,
                                SchemeID = ticket.SchemeID,
                                PlayTypeCode = ticket.PlayTypeCode,
                                Number = string.Join("", num_qszux),
                                Multiple = ticket.Multiple,
                                PickWayID = pickwayid,
                                Bet = bet,
                                SDID = ticket.SDID,
                                Money = 2 * bet * ticket.Multiple
                            };
                            list.Add(list_rxs);
                            #endregion
                        }
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.LX3:
                        #region 乐选3
                        #region 复试和胆拖
                        //if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        //{
                        //    #region 
                        //    string[] arr_rxs = ticket.Number.Split(SEP_NUM_B);
                        //    string[] first_num = arr_rxs[0].Split(SEP_NUM_A);
                        //    string[] second_num = arr_rxs[1].Split(SEP_NUM_A);
                        //    int bet = 0;
                        //    if (first_num.Length == 1)
                        //    {
                        //        decimal n = second_num.Length;
                        //        bet = Convert.ToInt32(n * (n - 1) / 2);
                        //    }
                        //    else if (first_num.Length == 2)
                        //    {
                        //        bet = second_num.Length;
                        //    }
                        //    udv_Ticket list_rxs_dt = new udv_Ticket()
                        //    {
                        //        LotteryCode = ticket.LotteryCode,
                        //        SchemeID = ticket.SchemeID,
                        //        PlayTypeCode = ticket.PlayTypeCode,
                        //        Number = string.Join("", first_num) + "*" + string.Join("", second_num),
                        //        Multiple = ticket.Multiple,
                        //        PickWayID = PickWay_DT,
                        //        Bet = bet,
                        //        SDID = ticket.SDID,
                        //        Money = 6 * bet * ticket.Multiple
                        //    };
                        //    list.Add(list_rxs_dt);
                        //    #endregion
                        //}
                        //else
                        //{
                        //    string[] num_rxs = ticket.Number.Split(SEP_NUM_A);
                        //    int pickwayid = 0;
                        //    int bet = 0;
                        //    if (num_rxs.Length == 3)
                        //    {
                        //        pickwayid = PickWay_DS;
                        //        bet = 1;
                        //    }
                        //    else if (num_rxs.Length > 3)
                        //    {
                        //        pickwayid = PickWay_FS;
                        //        decimal n = num_rxs.Length;
                        //        bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                        //    }
                        //    udv_Ticket list_rxs = new udv_Ticket()
                        //    {
                        //        LotteryCode = ticket.LotteryCode,
                        //        SchemeID = ticket.SchemeID,
                        //        PlayTypeCode = ticket.PlayTypeCode,
                        //        Number = string.Join("", num_rxs),
                        //        Multiple = ticket.Multiple,
                        //        PickWayID = pickwayid,
                        //        Bet = bet,
                        //        SDID = ticket.SDID,
                        //        Money = 6 * bet * ticket.Multiple
                        //    };
                        //    list.Add(list_rxs);
                        //}
                        #endregion
                        #region 单式
                        string[] lx3_num = ticket.Number.Split(SEP_NUM_G);
                        string[] lx3_num_w = lx3_num[0].Split(SEP_NUM_A);
                        string[] lx3_num_q = lx3_num[1].Split(SEP_NUM_A);
                        string[] lx3_num_b = lx3_num[2].Split(SEP_NUM_A);
                        int lx3_length_w = lx3_num_w.Length;
                        int lx3_length_q = lx3_num_q.Length;
                        int lx3_length_b = lx3_num_b.Length;
                        for (int i = 0; i < lx3_length_w; i++)
                        {
                            for (int j = 0; j < lx3_length_q; j++)
                            {
                                for (int x = 0; x < lx3_length_b; x++)
                                {
                                    List<string> nums = new List<string>();
                                    nums.Add(lx3_num_w[i]);
                                    nums.Add(lx3_num_q[j]);
                                    nums.Add(lx3_num_b[x]);
                                    //nums = nums.OrderBy(o => o).ToList();

                                    udv_Ticket list_rxb = new udv_Ticket()
                                    {
                                        LotteryCode = ticket.LotteryCode,
                                        SchemeID = ticket.SchemeID,
                                        PlayTypeCode = ticket.PlayTypeCode,
                                        //Number = lx3_num_w[i] + lx3_num_q[j] + lx3_num_b[x],
                                        Number = string.Join("", nums),
                                        Multiple = ticket.Multiple,
                                        PickWayID = PickWay_DS,
                                        Bet = 1,
                                        SDID = ticket.SDID,
                                        Money = 6 * 1 * ticket.Multiple
                                    };
                                    list.Add(list_rxb);
                                }
                            }
                        }
                        #endregion
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.LX4:
                        #region 乐选4
                        #region 复试和胆拖
                        //if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        //{
                        //    #region 
                        //    string[] arr_rxs = ticket.Number.Split(SEP_NUM_B);
                        //    string[] first_num = arr_rxs[0].Split(SEP_NUM_A);
                        //    string[] second_num = arr_rxs[1].Split(SEP_NUM_A);
                        //    decimal n = second_num.Length;
                        //    int bet = 1;
                        //    if (first_num.Length == 1)
                        //    {
                        //        bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                        //    }
                        //    else if (first_num.Length == 2)
                        //    {
                        //        bet = Convert.ToInt32(n * (n - 1) / 2);
                        //    }
                        //    else if (first_num.Length == 3)
                        //    {
                        //        bet = second_num.Length;
                        //    }

                        //    udv_Ticket list_rxs_dt = new udv_Ticket()
                        //    {
                        //        LotteryCode = ticket.LotteryCode,
                        //        SchemeID = ticket.SchemeID,
                        //        PlayTypeCode = ticket.PlayTypeCode,
                        //        Number = string.Join("", first_num) + "*" + string.Join("", second_num),
                        //        Multiple = ticket.Multiple,
                        //        PickWayID = PickWay_DT,
                        //        Bet = bet,
                        //        SDID = ticket.SDID,
                        //        Money = 10 * bet * ticket.Multiple
                        //    };
                        //    list.Add(list_rxs_dt);
                        //    #endregion
                        //}
                        //else
                        //{
                        //    #region
                        //    string[] num_rxs = ticket.Number.Split(SEP_NUM_A);
                        //    int pickwayid = 0;
                        //    int bet = 0;
                        //    if (num_rxs.Length == 4)
                        //    {
                        //        pickwayid = PickWay_DS;
                        //        bet = 1;
                        //    }
                        //    else if (num_rxs.Length > 4)
                        //    {
                        //        pickwayid = PickWay_FS;
                        //        decimal n = num_rxs.Length;
                        //        bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                        //    }
                        //    udv_Ticket list_rxs = new udv_Ticket()
                        //    {
                        //        LotteryCode = ticket.LotteryCode,
                        //        SchemeID = ticket.SchemeID,
                        //        PlayTypeCode = ticket.PlayTypeCode,
                        //        Number = string.Join("", num_rxs),
                        //        Multiple = ticket.Multiple,
                        //        PickWayID = pickwayid,
                        //        Bet = bet,
                        //        SDID = ticket.SDID,
                        //        Money = 10 * bet * ticket.Multiple
                        //    };
                        //    list.Add(list_rxs);
                        //    #endregion
                        //}
                        #endregion
                        #region 单式
                        string[] lx4_num = ticket.Number.Split(SEP_NUM_A);
                        int lx4_length = lx4_num.Length;
                        for (int i = 0; i < lx4_length; i++)
                        {
                            for (int j = i + 1; j < lx4_length; j++)
                            {
                                for (int x = j + 1; x < lx4_length; x++)
                                {
                                    for (int y = x + 1; y < lx4_length; y++)
                                    {
                                        udv_Ticket list_rxb = new udv_Ticket()
                                        {
                                            LotteryCode = ticket.LotteryCode,
                                            SchemeID = ticket.SchemeID,
                                            PlayTypeCode = ticket.PlayTypeCode,
                                            Number = lx4_num[i] + lx4_num[j] + lx4_num[x] + lx4_num[y],
                                            Multiple = ticket.Multiple,
                                            PickWayID = PickWay_DS,
                                            Bet = 1,
                                            SDID = ticket.SDID,
                                            Money = 10 * 1 * ticket.Multiple
                                        };
                                        list.Add(list_rxb);
                                    }
                                }
                            }
                        }
                        #endregion
                        #endregion
                        break;
                    case (int)SD11X5PlayCode.LX5:
                        #region 乐选5
                        #region 复试和胆拖
                        //if (ticket.Number.IndexOf(SEP_NUM_B) > -1)  //是否胆拖
                        //{
                        //    #region 
                        //    string[] arr_rxw = ticket.Number.Split(SEP_NUM_B);
                        //    string[] first_num = arr_rxw[0].Split(SEP_NUM_A);
                        //    string[] second_num = arr_rxw[1].Split(SEP_NUM_A);
                        //    decimal n = second_num.Length;
                        //    int bet = 1;
                        //    if (first_num.Length == 1)
                        //    {
                        //        bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                        //    }
                        //    else if (first_num.Length == 2)
                        //    {
                        //        bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                        //    }
                        //    else if (first_num.Length == 3)
                        //    {
                        //        bet = Convert.ToInt32(n * (n - 1) / 2);
                        //    }
                        //    else if (first_num.Length == 4)
                        //    {
                        //        bet = second_num.Length;
                        //    }

                        //    udv_Ticket list_rxw_dt = new udv_Ticket()
                        //    {
                        //        LotteryCode = ticket.LotteryCode,
                        //        SchemeID = ticket.SchemeID,
                        //        PlayTypeCode = ticket.PlayTypeCode,
                        //        Number = string.Join("", first_num) + "*" + string.Join("", second_num),
                        //        Multiple = ticket.Multiple,
                        //        PickWayID = PickWay_DT,
                        //        Bet = bet,
                        //        SDID = ticket.SDID,
                        //        Money = 14 * bet * ticket.Multiple
                        //    };
                        //    list.Add(list_rxw_dt);
                        //    #endregion
                        //}
                        //else
                        //{
                        //    #region 
                        //    string[] num_rxw = ticket.Number.Split(SEP_NUM_A);
                        //    int pickwayid = 0;
                        //    int bet = 0;
                        //    if (num_rxw.Length == 5)
                        //    {
                        //        pickwayid = PickWay_DS;
                        //        bet = 1;
                        //    }
                        //    else if (num_rxw.Length > 5)
                        //    {
                        //        pickwayid = PickWay_FS;
                        //        decimal n = num_rxw.Length;
                        //        bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                        //    }
                        //    udv_Ticket list_rxw = new udv_Ticket()
                        //    {
                        //        LotteryCode = ticket.LotteryCode,
                        //        SchemeID = ticket.SchemeID,
                        //        PlayTypeCode = ticket.PlayTypeCode,
                        //        Number = string.Join("", num_rxw),
                        //        Multiple = ticket.Multiple,
                        //        PickWayID = pickwayid,
                        //        Bet = bet,
                        //        SDID = ticket.SDID,
                        //        Money = 14 * bet * ticket.Multiple
                        //    };
                        //    list.Add(list_rxw);
                        //    #endregion
                        //}
                        #endregion
                        #region 单式
                        string[] lx5_num = ticket.Number.Split(SEP_NUM_A);
                        int lx5_length = lx5_num.Length;
                        for (int i = 0; i < lx5_length; i++)
                        {
                            for (int j = i + 1; j < lx5_length; j++)
                            {
                                for (int x = j + 1; x < lx5_length; x++)
                                {
                                    for (int y = x + 1; y < lx5_length; y++)
                                    {
                                        for (int n = y + 1; n < lx5_length; n++)
                                        {
                                            udv_Ticket list_rxb = new udv_Ticket()
                                            {
                                                LotteryCode = ticket.LotteryCode,
                                                SchemeID = ticket.SchemeID,
                                                PlayTypeCode = ticket.PlayTypeCode,
                                                Number = lx5_num[i] + lx5_num[j] + lx5_num[x] + lx5_num[y] + lx5_num[n],
                                                Multiple = ticket.Multiple,
                                                PickWayID = PickWay_DS,
                                                Bet = 1,
                                                SDID = ticket.SDID,
                                                Money = 14 * 1 * ticket.Multiple
                                            };
                                            list.Add(list_rxb);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
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
            var s_list = MergeTicket(list.FindAll(x => x.PlayTypeCode != 20208 && x.PlayTypeCode != 20213 && x.PlayTypeCode != 20214 && x.PlayTypeCode != 20215), PickWay_DS, SINGLE_NUMBER_PER_TICKET, SEP_NUM_F);
            return list.FindAll(x => x.PlayTypeCode == 20208 || x.PlayTypeCode != 20213 || x.PlayTypeCode != 20214 || x.PlayTypeCode != 20215).Concat(s_list).ToList<udv_Ticket>();
        }
    }
}
