using CL.Enum.Common.Lottery;
using CL.Game.BLL;
using CL.Game.BLL.View;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using CL.Tools.TicketInterface;
using CL.Game.Entity;
using CL.View.Entity.Interface;
using System.Threading;
using System.Configuration;
using CL.Plugins.Award;
using CL.Game.BLL.Tools;
using HtmlAgilityPack;
using System.Text;

namespace CL.Tools.LotteryTickets.Lottery.SSC
{
    public class CQSSC : LotteryBase
    {
        //彩种编号
        private const int LotteryCode = 301;
        static string LotteryFlag = "LotteryTickets.CQSSC";
        private readonly Log log = new Log(LotteryFlag);
        private XmlNode xml = null;

        private IsusesBLL bllis = new IsusesBLL();
        private SchemesBLL blls = new SchemesBLL();
        private SchemeETicketsBLL bllet = new SchemeETicketsBLL();
        private SchemesWinBLL bllwin = new SchemesWinBLL();

        public CQSSC()
        {
            GetWinType();
        }

        public CQSSC(XmlNode _xml)
        {
            xml = _xml;
            GetWinType();
        }

        /// <summary>
        /// 奖等数据
        /// </summary>
        private void GetWinType()
        {
            #region 奖等
            WinType model1 = new WinType() { WinCode = 3010101, PlayCode = 30101, WinName = "五星直选", DefaultMoney = 100000 * 100, DefaultMoneyNoWithTax = 80000 * 100 };
            wt.Add(model1);
            WinType model2 = new WinType() { WinCode = 3010201, PlayCode = 30102, WinName = "五星通选", DefaultMoney = 20440 * 100, DefaultMoneyNoWithTax = 16352 * 100 };
            wt.Add(model2);
            WinType model3 = new WinType() { WinCode = 3010202, PlayCode = 30102, WinName = "五星通选", DefaultMoney = 220 * 100, DefaultMoneyNoWithTax = 220 * 100 };
            wt.Add(model3);
            WinType model4 = new WinType() { WinCode = 3010203, PlayCode = 30102, WinName = "五星通选", DefaultMoney = 20 * 100, DefaultMoneyNoWithTax = 20 * 100 };
            wt.Add(model4);
            WinType model5 = new WinType() { WinCode = 3010301, PlayCode = 30103, WinName = "三星直选", DefaultMoney = 1000 * 100, DefaultMoneyNoWithTax = 1000 * 100 };
            wt.Add(model5);
            WinType model6 = new WinType() { WinCode = 3010401, PlayCode = 30104, WinName = "三星组三", DefaultMoney = 320 * 100, DefaultMoneyNoWithTax = 320 * 100 };
            wt.Add(model6);
            WinType model7 = new WinType() { WinCode = 3010501, PlayCode = 30105, WinName = "三星组六", DefaultMoney = 160 * 100, DefaultMoneyNoWithTax = 160 * 100 };
            wt.Add(model7);
            WinType model8 = new WinType() { WinCode = 3010601, PlayCode = 30106, WinName = "二星直选", DefaultMoney = 100 * 100, DefaultMoneyNoWithTax = 100 * 100 };
            wt.Add(model8);
            WinType model9 = new WinType() { WinCode = 3010701, PlayCode = 30107, WinName = "二星直选和值", DefaultMoney = 100 * 100, DefaultMoneyNoWithTax = 100 * 100 };
            wt.Add(model9);
            WinType model10 = new WinType() { WinCode = 3010801, PlayCode = 30108, WinName = "二星组选", DefaultMoney = 50 * 100, DefaultMoneyNoWithTax = 50 * 100 };
            wt.Add(model10);
            WinType model11 = new WinType() { WinCode = 3010901, PlayCode = 30109, WinName = "二星组选和值", DefaultMoney = 50 * 100, DefaultMoneyNoWithTax = 50 * 100 };
            wt.Add(model11);
            WinType model12 = new WinType() { WinCode = 3011001, PlayCode = 30110, WinName = "一星", DefaultMoney = 10 * 100, DefaultMoneyNoWithTax = 10 * 100 };
            wt.Add(model12);
            WinType model13 = new WinType() { WinCode = 3011101, PlayCode = 30111, WinName = "大小单双", DefaultMoney = 4 * 100, DefaultMoneyNoWithTax = 4 * 100 };
            wt.Add(model13);
            #endregion
        }

        /// <summary>
        /// 算奖
        /// </summary>
        public override void ComputeWin(XmlNode xml)
        {
            if (wt.Count <= 0) return;
            try
            {
                //开奖号码左到右  万位到个位
                List<udv_ComputeTicket> Sql = new List<udv_ComputeTicket>();
                List<long> ListWinSID = new List<long>();  //中奖记录方案号
                List<long> ListNoWinSID = new List<long>(); //没有中奖记录方案号
                List<long> NewListNoWinSID = new List<long>();
                List<udv_ComputeTicket> list = new SchemesBLL().QueryComputeTicketList(LotteryCode);
                if (list == null || list.Count == 0) return;
                #region 算奖前查询电子票状态(接口查)
                OutTicket(xml);
                #endregion
                foreach (udv_ComputeTicket item in list)
                {
                    string Description = "";
                    long WinMoneyNoWithTax = 0;
                    long WinMoney = 0;
                    int WinCode = 0;

                    if (item == null) continue;

                    string OpenNumber = item.OpenNumber.Trim();
                    #region 计算中奖
                    switch (item.PlayCode)
                    {
                        case (int)CQSSCPlayCode.ZX_1X:
                            #region 一星直选单式
                            WinMoney = ComputeWin_1XZX(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.DXDS:
                            #region 大小单双
                            WinMoney = ComputeWin_DXDS(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZX_2X:
                            #region 二星直选
                            WinMoney = ComputeWin_ZX_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZXHZ_2X:
                            #region 二星直选和值
                            WinMoney = ComputeWin_ZXHZ_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZU_2X:
                            #region 二星组选
                            WinMoney = ComputeWin_ZU_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZUHZ_2X:
                            #region 二星组选和值
                            WinMoney = ComputeWin_ZUHZ_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZX_3X:
                            #region 三星直选
                            WinMoney = ComputeWin_ZX_3X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZU3_3X:
                            #region 三星组选3
                            WinMoney = ComputeWin_ZU3_3X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZU6_3X:
                            #region 三星组选6
                            WinMoney = ComputeWin_ZU6_3X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZX_5X:
                            #region 五星直选
                            WinMoney = ComputeWin_ZX_5X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.TX_5X:
                            #region 五星通选
                            WinMoney = ComputeWin_TX_5X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                            //江西时时彩增加任选1和任性2
                    }
                    #endregion

                    item.LotteryCode = LotteryCode;

                    //涉及五星通选 多个中奖等级,考虑是否取消WinCode返回,看看有没有意义
                    //item.WinCode = WinCode;

                    item.SumWinMoney = WinMoney * item.Multiple;
                    item.SumWinMoneyNoWithTax = WinMoneyNoWithTax * item.Multiple;
                    item.Description = Description;

                    Sql.Add(item);
                    if (WinMoney > 0 && !ListWinSID.Contains(item.SchemeID))
                        ListWinSID.Add(item.SchemeID);
                    if (!ListNoWinSID.Contains(item.SchemeID))
                        ListNoWinSID.Add(item.SchemeID);
                }

                //过滤没有中奖的方案
                foreach (long item in ListNoWinSID)
                {
                    if (ListWinSID.Contains(item))
                        continue;
                    NewListNoWinSID.Add(item);
                }
                //处理中奖记录
                bllwin.SchemesDetailsWin(Sql, ListWinSID, NewListNoWinSID);
                blls.Redis_Compute(Sql);
                //加奖
                new AwardEntrance(LotteryCode, Sql).Main();
            }
            catch (Exception ex)
            {
                log.Write("自动算奖失败：" + ex.Message, true);
            }
        }
        /// <summary>
        /// 追号算奖
        /// </summary>
        public override void ComputeChaseTasksWin(XmlNode xml)
        {
            if (wt.Count <= 0) return;
            try
            {
                List<udv_ComputeTicketChaseTasks> Sql = new List<udv_ComputeTicketChaseTasks>();
                List<long> ListWinSID = new List<long>();  //中奖记录方案号
                List<long> ListNoWinSID = new List<long>(); //没有中奖记录方案号
                List<long> NewListNoWinSID = new List<long>();
                List<udv_ComputeTicketChaseTasks> list = new SchemesBLL().QueryComputeTicketList(LotteryCode, string.Empty);
                if (list == null || list.Count == 0) return;
                #region 算奖前查询电子票状态(接口查)
                OutTicket(xml);
                #endregion
                foreach (udv_ComputeTicketChaseTasks item in list)
                {
                    string Description = "";
                    long WinMoneyNoWithTax = 0;
                    long WinMoney = 0;
                    int WinCode = 0;

                    if (item == null) continue;

                    string OpenNumber = item.OpenNumber.Trim();
                    #region 计算中奖
                    switch (item.PlayCode)
                    {
                        case (int)CQSSCPlayCode.ZX_1X:
                            #region 一星直选单式
                            WinMoney = ComputeWin_1XZX(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.DXDS:
                            #region 大小单双
                            WinMoney = ComputeWin_DXDS(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZX_2X:
                            #region 二星直选
                            WinMoney = ComputeWin_ZX_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZXHZ_2X:
                            #region 二星直选和值
                            WinMoney = ComputeWin_ZXHZ_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZU_2X:
                            #region 二星组选
                            WinMoney = ComputeWin_ZU_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZUHZ_2X:
                            #region 二星组选和值
                            WinMoney = ComputeWin_ZUHZ_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZX_3X:
                            #region 三星直选
                            WinMoney = ComputeWin_ZX_3X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZU3_3X:
                            #region 三星组选3
                            WinMoney = ComputeWin_ZU3_3X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZU6_3X:
                            #region 三星组选6
                            WinMoney = ComputeWin_ZU6_3X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.ZX_5X:
                            #region 五星直选
                            WinMoney = ComputeWin_ZX_5X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)CQSSCPlayCode.TX_5X:
                            #region 五星通选
                            WinMoney = ComputeWin_TX_5X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                    }
                    #endregion

                    item.LotteryCode = LotteryCode;
                    item.WinCode = WinCode;
                    item.SumWinMoney = WinMoney * item.Multiple;
                    item.SumWinMoneyNoWithTax = WinMoneyNoWithTax * item.Multiple;
                    item.Description = Description;

                    Sql.Add(item);
                    if (WinMoneyNoWithTax > 0 && !ListWinSID.Contains(item.SchemeID))
                        ListWinSID.Add(item.SchemeID);
                    if (!ListNoWinSID.Contains(item.SchemeID))
                        ListNoWinSID.Add(item.SchemeID);
                }

                //过滤没有中奖的方案
                foreach (long item in ListNoWinSID)
                {
                    if (ListWinSID.Contains(item))
                        continue;
                    NewListNoWinSID.Add(item);
                }
                bllwin.SchemesDetailsWinByChaseTasks(Sql, ListWinSID, NewListNoWinSID);
                blls.Redis_Compute(Sql);
            }
            catch (Exception ex)
            {
                log.Write("自动算奖失败(追号)：" + ex.Message, true);
            }
        }

        #region 奖金计算
        /// <summary>
        /// 一星直选单式 方案格式: 1,2,3,4,5
        /// </summary>
        public long ComputeWin_1XZX(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            int[] OpenNum = OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
            if (OpenNum.Length < 5) return 0;

            int[] Numbers = Number.Trim().Split(',').Select(s => Convert.ToInt32(s)).ToArray();
            if (Numbers == null) return 0;
            if (Numbers.Length < 1) return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)CQSSCWinCode.ZX_1X && p.PlayCode == (int)CQSSCPlayCode.ZX_1X);

            for (int i = 0; i < Numbers.Length; i++)
            {
                //购买的号码只要和开奖号码最后一位相等即为中奖
                if (Numbers[i].Equals(OpenNum[4]))
                {
                    winNum++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
            }
            if (winNum > 0)
            {
                Description = "一星奖" + winNum.ToString() + "注";
                WinCode = (int)CQSSCWinCode.ZX_1X;
            }
            return WinMoney;
        }

        /// <summary>
        /// 大小单双    方案格式: 大小单双,大小单
        /// </summary>
        public long ComputeWin_DXDS(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            int[] OpenNum = OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
            if (OpenNum.Length < 5) return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)CQSSCWinCode.DXDS && p.PlayCode == (int)CQSSCPlayCode.DXDS);

            //十位:下标为0 个位:下标为1
            int[] Nums = Number.Split('*').Select(s => Convert.ToInt32(s)).ToArray();
            if (ToSingle_DXDS(OpenNum[3]).Contains(Nums[0]) && ToSingle_DXDS(OpenNum[4]).Contains(Nums[1]))
            {
                winNum++;
                WinMoney += model.DefaultMoney;
                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
            }
            if (winNum > 0)
            {
                Description = "大小单双奖" + winNum.ToString() + "注";
                WinCode = (int)CQSSCWinCode.DXDS;
            }
            return WinMoney;
        }

        /// <summary>
        /// 二星直选  单式方案格式: 1 2 8  复式方案格式: 012345,0123
        /// </summary>
        public long ComputeWin_ZX_2X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            int[] OpenNum = OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
            if (OpenNum.Length < 5) return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)CQSSCWinCode.ZX_2X && p.PlayCode == (int)CQSSCPlayCode.ZX_2X);
            //0,1|2,3

            string[] numbers = Number.Split('*');
            int[] numbers_s = SplitLotteryNumbers(numbers[0]);
            int[] numbers_g = SplitLotteryNumbers(numbers[1]);

            //复式
            if (numbers_s.Length > 1 || numbers_g.Length > 1)
            {
                for (int i = 0; i < numbers_s.Length; i++)
                    for (int j = 0; j < numbers_g.Length; j++)
                        if (numbers_s[i] == OpenNum[3] && numbers_g[j] == OpenNum[4])
                        {
                            winNum++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
            }
            else
            {
                if (numbers_s[0] == OpenNum[3] && numbers_g[0] == OpenNum[4])
                {
                    winNum++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
            }

            if (winNum > 0)
            {
                Description = "二星直选奖" + winNum.ToString() + "注";
                WinCode = (int)CQSSCWinCode.ZX_2X;
            }

            return WinMoney;
        }

        /// <summary>
        /// 二星直选和值  单、复式方案格式: 0,1,18
        /// </summary>
        public long ComputeWin_ZXHZ_2X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)CQSSCWinCode.ZXHZ_2X && p.PlayCode == (int)CQSSCPlayCode.ZXHZ_2X);
            //统计开奖号码最后两位的和值
            int iWinSum = int.Parse(OpenNum[3].ToString()) + int.Parse(OpenNum[4].ToString());
            //每注内和值只可能有1注
            string[] szTmp = Number.Split(',');
            foreach (string sigNumber in szTmp)
            {
                if (iWinSum == int.Parse(sigNumber))
                {
                    winNum++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
            }

            if (winNum > 0)
            {
                Description = "二星直选和值奖" + winNum.ToString() + "注";
                WinCode = (int)CQSSCWinCode.ZXHZ_2X;
            }

            return WinMoney;
        }

        /// <summary>
        /// 二星组选  复式方案格式: 0,1,2,3,4,9
        /// </summary>
        public long ComputeWin_ZU_2X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            int[] OpenNum = OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
            if (OpenNum.Length < 5) return 0;

            //开奖号码后两位是否为对子
            bool isPair = OpenNum[3].Equals(OpenNum[4]);

            long WinMoney = 0;
            int winNum = 0;
            //二星组选时开奖号码不能是对子
            if (!isPair)
            {
                int[] Numbers = Number.Split('*').Select(s => Convert.ToInt32(s)).ToArray();
                WinType model = wt.Find(p => p.WinCode == (int)CQSSCWinCode.ZU_2X && p.PlayCode == (int)CQSSCPlayCode.ZU_2X);
                for (int j = 0; j < Numbers.Length; j++)
                    for (int x = j + 1; x < Numbers.Length; x++)
                        if ((Numbers[j] == OpenNum[3] || Numbers[j] == OpenNum[4]) && (Numbers[x] == OpenNum[3] || Numbers[x] == OpenNum[4]))
                        {
                            winNum++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
            }

            if (winNum > 0)
            {
                Description = "二星组选奖" + winNum.ToString() + "注";
                WinCode = (int)CQSSCWinCode.ZU_2X;
            }
            return WinMoney;
        }

        /// <summary>
        /// 二星组选和值  复式方案格式: 0,1,2,3,4,18
        /// </summary>
        public long ComputeWin_ZUHZ_2X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            //所选和值号码与开奖号码后两位之和相同，即中奖50元，如奖号为对子号，即中奖100元。
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            //开奖号码后两位是否为对子,对子奖金翻倍
            int pairMultiple = OpenNum[3].Equals(OpenNum[4]) ? 2 : 1;

            long WinMoney = 0;
            int winNum = 0;

            WinType model = wt.Find(p => p.WinCode == (int)CQSSCWinCode.ZUHZ_2X && p.PlayCode == (int)CQSSCPlayCode.ZUHZ_2X);
            //统计开奖号码最后两位的和值
            int iWinSum = int.Parse(OpenNum[3].ToString()) + int.Parse(OpenNum[4].ToString());
            //每注内和值只可能有1注
            string[] szTmp = Number.Split(',');
            foreach (string sigNumber in szTmp)
            {
                if (iWinSum == int.Parse(sigNumber))
                {
                    winNum++;
                    WinMoney += model.DefaultMoney * pairMultiple;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax * pairMultiple;
                }
            }

            if (winNum > 0)
            {
                Description = "二星组选和值奖" + winNum.ToString() + "注";
                WinCode = (int)CQSSCWinCode.ZUHZ_2X;
            }

            return WinMoney;
        }

        /// <summary>
        /// 三星直选  单式方案格式: 012 001 018  复式方案格式: 012345,0123,0123
        /// </summary>
        public long ComputeWin_ZX_3X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            int[] OpenNum = OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
            if (OpenNum.Length < 5) return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)CQSSCWinCode.ZX_3X && p.PlayCode == (int)CQSSCPlayCode.ZX_3X);

            string[] numbers = Number.Split('*');
            int[] numbers_b = SplitLotteryNumbers(numbers[0]);
            int[] numbers_s = SplitLotteryNumbers(numbers[1]);
            int[] numbers_g = SplitLotteryNumbers(numbers[2]);
            for (int i = 0; i < numbers_b.Length; i++)
                for (int j = 0; j < numbers_s.Length; j++)
                    for (int x = 0; x < numbers_g.Length; x++)
                        if (numbers_b[i] == OpenNum[2] && numbers_s[j] == OpenNum[3] && numbers_g[x] == OpenNum[4])
                        {
                            winNum++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
            if (winNum > 0)
            {
                Description = "三星直选奖" + winNum.ToString() + "注";
                WinCode = (int)CQSSCWinCode.ZX_3X;
            }

            return WinMoney;
        }

        /// <summary>
        /// 三星组选3 单式、复式  单式方案格式: 012 001 018  复式方案格式: 012345678
        /// </summary>
        public long ComputeWin_ZU3_3X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            int[] OpenNum = OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
            if (OpenNum.Length < 5) return 0;

            long WinMoney = 0;
            int winNum = 0;

            OpenNum = new int[] { OpenNum[2], OpenNum[3], OpenNum[4] };
            OpenNum = OpenNum.GroupBy(g => g).Select(s => s.Key).ToArray();
            //过滤开奖后三位是否符合组三中奖规则
            //过滤豹子
            if (OpenNum.Length == 2)
            {
                WinType model = wt.Find(p => p.WinCode == (int)CQSSCWinCode.ZU3_3X && p.PlayCode == (int)CQSSCPlayCode.ZU3_3X);

                int[] Numbers = Number.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                if (Numbers.Contains(OpenNum[0]) && Numbers.Contains(OpenNum[1]))
                {
                    winNum++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
            }
            if (winNum > 0)
            {
                Description = "三星组三中奖" + winNum.ToString() + "注";
                WinCode = (int)CQSSCWinCode.ZU3_3X;
            }
            return WinMoney;
        }

        /// <summary>
        /// 三星组选6 单式、复式  单式方案格式: 012 001 018  复式方案格式: 012345678
        /// </summary>
        public long ComputeWin_ZU6_3X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            long WinMoney = 0;
            int winNum = 0;
            string WinNumber = OpenNum[2] + OpenNum[3] + OpenNum[4];
            //是否豹子
            bool isBao = OpenNum[2].Equals(OpenNum[3]) && OpenNum[3].Equals(OpenNum[4]);

            //组三不能是豹子
            if (!isBao)
            {
                //是否组三
                bool isZus = OpenNum[2].Equals(OpenNum[3]) || OpenNum[2].Equals(OpenNum[4])
                    || OpenNum[3].Equals(OpenNum[4]);
                //开奖号码不是组三再继续
                if (!isZus)
                {
                    WinType model = wt.Find(p => p.WinCode == (int)CQSSCWinCode.ZU6_3X && p.PlayCode == (int)CQSSCPlayCode.ZU6_3X);

                    string[] shhNumber = Number.Split(' ');
                    if (shhNumber.Length == 1 && shhNumber[0].Length > 3)
                    {
                        string[] szLottery = this.ToSingle_Zu3F(Number);

                        foreach (string singNumber in szLottery)
                        {
                            if (singNumber.Length == 3 && singNumber.Equals(WinNumber))
                            {
                                winNum++;
                                WinMoney += model.DefaultMoney;
                                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                //复式只会有一注中奖
                                break;
                            }
                        }

                    }
                    else
                    {
                        //后3组选单式混合
                        foreach (string singNumber in shhNumber)
                        {
                            if (singNumber != null && singNumber.Length == 3)
                            {
                                if (base.Sort(singNumber) == WinNumber)
                                {
                                    winNum++;
                                    WinMoney += model.DefaultMoney;
                                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                }
                            }
                        }
                    }

                    if (winNum > 0)
                    {
                        Description = "三星组选6奖" + winNum.ToString() + "注";
                        WinCode = (int)CQSSCWinCode.ZU6_3X;
                    }
                }
            }

            return WinMoney;
        }

        /// <summary>
        /// 五星直选  单式方案格式: 01234 00012 01782  复式方案格式: 012345,01236,01237,789,1234
        /// </summary>
        public long ComputeWin_ZX_5X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            int[] OpenNum = OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
            if (OpenNum.Length < 5) return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)CQSSCWinCode.ZX_5X && p.PlayCode == (int)CQSSCPlayCode.ZX_5X);

            string[] numbers = Number.Split('*');
            if (numbers.Length < 5)
                return 0;

            int[] numbers_w = SplitLotteryNumbers(numbers[0]);
            int[] numbers_q = SplitLotteryNumbers(numbers[1]);
            int[] numbers_b = SplitLotteryNumbers(numbers[2]);
            int[] numbers_s = SplitLotteryNumbers(numbers[3]);
            int[] numbers_g = SplitLotteryNumbers(numbers[4]);

            for (int w = 0; w < numbers_w.Length; w++)
                for (int q = 0; q < numbers_q.Length; q++)
                    for (int b = 0; b < numbers_b.Length; b++)
                        for (int s = 0; s < numbers_s.Length; s++)
                            for (int g = 0; g < numbers_g.Length; g++)
                                if (numbers_w[w] == OpenNum[0] && numbers_q[q] == OpenNum[1] && numbers_b[b] == OpenNum[2] && numbers_s[s] == OpenNum[3] && numbers_g[g] == OpenNum[4])
                                {
                                    winNum++;
                                    WinMoney += model.DefaultMoney;
                                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                }
            if (winNum > 0)
            {
                Description = "三星直选奖" + winNum.ToString() + "注";
                WinCode = (int)CQSSCWinCode.ZX_3X;
            }

            if (winNum > 0)
            {
                Description = "五星直选奖" + winNum.ToString() + "注";
                WinCode = (int)CQSSCWinCode.ZX_5X;
            }
            return WinMoney;
        }

        /// <summary>
        /// 五星通选  单式方案格式: 01234 00012 01782  复式方案格式: 012345,01236,01237,789,1234
        /// </summary>
        public long ComputeWin_TX_5X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;
            long WinMoney = 0;

            int winNum_5XTX_1 = 0;
            int winMoney_5XTX_1 = 0;
            int winMoney_NoWithTax_5XTX_1 = 0;

            int winNum_5XTX_2 = 0;
            int winMoney_5XTX_2 = 0;
            int winMoney_NoWithTax_5XTX_2 = 0;

            int winNum_5XTX_3 = 0;
            int winMoney_5XTX_3 = 0;
            int winMoney_NoWithTax_5XTX_3 = 0;

            WinType model = wt.Find(p => p.WinCode == (int)CQSSCWinCode.TX_5X_ALLWin && p.PlayCode == (int)CQSSCPlayCode.TX_5X);
            int defaultMoney1 = model.DefaultMoney;
            int defaultMoneyNoWithTax1 = model.DefaultMoneyNoWithTax;

            WinType model2 = wt.Find(p => p.WinCode == (int)CQSSCWinCode.TX_5X_Q3HH3 && p.PlayCode == (int)CQSSCPlayCode.TX_5X);
            int defaultMoney2 = model2.DefaultMoney;
            int defaultMoneyNoWithTax2 = model2.DefaultMoneyNoWithTax;

            WinType model3 = wt.Find(p => p.WinCode == (int)CQSSCWinCode.TX_5X_Q2HH2 && p.PlayCode == (int)CQSSCPlayCode.TX_5X);
            int defaultMoney3 = model3.DefaultMoney;
            int defaultMoneyNoWithTax3 = model3.DefaultMoneyNoWithTax;

            if (Number.Contains(","))
            {
                #region 复式
                string[] szNumber = Number.Split(',');
                if (szNumber[0].Contains(OpenNumber[0]) && szNumber[1].Contains(OpenNumber[1]) &&
                            szNumber[2].Contains(OpenNumber[2]) && szNumber[3].Contains(OpenNumber[3]) &&
                            szNumber[4].Contains(OpenNumber[4]))
                {
                    //号码全中则兼中2 3奖
                    winNum_5XTX_1++;

                    winMoney_5XTX_1 += defaultMoney1;
                    winMoney_NoWithTax_5XTX_1 += defaultMoneyNoWithTax1;

                    winNum_5XTX_2 += 2;

                    winMoney_5XTX_2 += defaultMoney2;
                    winMoney_NoWithTax_5XTX_2 += defaultMoneyNoWithTax2;

                    winNum_5XTX_3 += 2;

                    winMoney_5XTX_3 += defaultMoney3;
                    winMoney_NoWithTax_5XTX_3 += defaultMoneyNoWithTax3;

                }
                else if ((szNumber[0].Contains(OpenNumber[0]) && szNumber[1].Contains(OpenNumber[1]) &&
                   szNumber[2].Contains(OpenNumber[2])) || (szNumber[2].Contains(OpenNumber[2]) &&
                   szNumber[3].Contains(OpenNumber[3]) && szNumber[4].Contains(OpenNumber[4])))
                {
                    //中前3或后3,不可能同时中前3和后3  或者中前3和中后2 反之亦然,所以不用区分
                    winNum_5XTX_2++;

                    winMoney_5XTX_2 += defaultMoney2;
                    winMoney_NoWithTax_5XTX_2 += defaultMoneyNoWithTax2;

                    winNum_5XTX_3++;

                    winMoney_5XTX_3 += defaultMoney3;
                    winMoney_NoWithTax_5XTX_3 += defaultMoneyNoWithTax3;
                }
                else
                {
                    //中前2或后2,可能同时中
                    if (szNumber[0].Contains(OpenNumber[0]) && szNumber[1].Contains(OpenNumber[1]))
                    {
                        winNum_5XTX_3++;

                        winMoney_5XTX_3 += defaultMoney3;
                        winMoney_NoWithTax_5XTX_3 += defaultMoneyNoWithTax3;
                    }
                    if (szNumber[3].Contains(OpenNumber[3]) && szNumber[4].Contains(OpenNumber[4]))
                    {
                        winNum_5XTX_3++;

                        winMoney_5XTX_3 += defaultMoney3;
                        winMoney_NoWithTax_5XTX_3 += defaultMoneyNoWithTax3;
                    }
                }
                #endregion
            }
            else
            {
                #region 单式
                string[] szNumber = Number.Split('*');

                if (szNumber == null || szNumber.Length < 1)
                    return 0;
                else
                {
                    if (szNumber[0].Equals(OpenNum[0]) && szNumber[1].Equals(OpenNum[1]) &&
                        szNumber[2].Equals(OpenNum[2]) && szNumber[3].Equals(OpenNum[3]) &&
                        szNumber[4].Equals(OpenNum[4]))
                    {
                        //号码全中则兼中2 3奖
                        winNum_5XTX_1++;

                        winMoney_5XTX_1 += defaultMoney1;
                        winMoney_NoWithTax_5XTX_1 += defaultMoneyNoWithTax1;

                        winNum_5XTX_2++;

                        winMoney_5XTX_2 += defaultMoney2;
                        winMoney_NoWithTax_5XTX_2 += defaultMoneyNoWithTax2;

                        winNum_5XTX_3++;

                        winMoney_5XTX_3 += defaultMoney3;
                        winMoney_NoWithTax_5XTX_3 += defaultMoneyNoWithTax3;

                    }
                    else
                    {
                        if ((szNumber[0].Equals(OpenNum[0]) && szNumber[1].Equals(OpenNum[1]) &&
                           szNumber[2].Equals(OpenNum[2])) || (szNumber[2].Equals(OpenNum[2]) &&
                           szNumber[3].Equals(OpenNum[3]) && szNumber[4].Equals(OpenNum[4])))
                        {
                            //中前3或后3,不可能同时中前3和后3  或者中前3和中后2 反之亦然,所以不用区分
                            winNum_5XTX_2++;

                            winMoney_5XTX_2 += defaultMoney2;
                            winMoney_NoWithTax_5XTX_2 += defaultMoneyNoWithTax2;

                            winNum_5XTX_3++;

                            winMoney_5XTX_3 += defaultMoney3;
                            winMoney_NoWithTax_5XTX_3 += defaultMoneyNoWithTax3;
                        }
                        else
                        {
                            //中前2或后2,可能同时中
                            if ((szNumber[0].Equals(OpenNum[0]) && szNumber[1].Equals(OpenNum[1])) || (szNumber[3].Equals(OpenNum[3]) && szNumber[4].Equals(OpenNum[4])))
                            {
                                winNum_5XTX_3++;

                                winMoney_5XTX_3 += defaultMoney3;
                                winMoney_NoWithTax_5XTX_3 += defaultMoneyNoWithTax3;
                            }
                        }
                    }
                }
                #endregion                
            }

            if (winNum_5XTX_1 > 0)
            {
                Description = "五星通选一奖" + winNum_5XTX_1.ToString() + "注";
            }
            if (winNum_5XTX_2 > 0)
            {
                Description += "五星通选二奖" + winNum_5XTX_2.ToString() + "注\n";
            }
            if (winNum_5XTX_3 > 0)
            {
                Description += "五星通选三奖" + winNum_5XTX_3.ToString() + "注\n";
            }
            WinMoney = winMoney_5XTX_1 + winMoney_5XTX_2 + winMoney_5XTX_3;
            WinMoneyNoWithTax = winMoney_NoWithTax_5XTX_1 + winMoney_NoWithTax_5XTX_2 + winMoney_NoWithTax_5XTX_3;

            if (WinMoney > 0 && WinMoney != WinMoneyNoWithTax)
            {
                //如果税前税后奖金不等,则添加进奖金描述
                Description += "税前奖金: " + WinMoney / 100 + "元,税后奖金: " + WinMoneyNoWithTax / 100 + "元";
            }
            //WinCode = (int)CQSSCWinCodeEnum;
            //奖金代码有问题,不能返回多个代码
            return WinMoney;
        }
        #endregion

        #region 正常采集数据
        public override LotteryResult GetValue()
        {
            try
            {
                LotteryResult ent = new LotteryResult();
                bool IsInterface = true;
                string Config_Key = ConfigurationManager.AppSettings["IsInterface"];
                bool.TryParse(Config_Key, out IsInterface);
                if (IsInterface)
                {
                    ent = GetValue3();
                    if (ent != null)
                    {
                        #region 新的开奖方式
                        string ReturnDescription = "";
                        bool res = bllis.EnteringDrawResults(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.LotteryTime, ref ReturnDescription);
                        if (res)
                        {
                            if (ReturnDescription.Trim() == "添加开奖信息成功" && !string.IsNullOrEmpty(ent.LotteryWinNum))
                            {
                                new IsusesBLL().InsertIsuseInfoRedis(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.StartTime, ent.EndTime, ent.LotteryTime, 0, 0);
                            }
                            Console.WriteLine(string.Format("抓取开奖结果：{0}", ent.ToString()));
                        }
                        #endregion
                    }
                    else
                    {
                        ent = GetValue1();
                        if (ent != null)
                        {
                            string ReturnDescription = "";
                            bool res = bllis.AddLotteryOpenNumber(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.StartTime, ent.EndTime, ent.LotteryTime, ref ReturnDescription);
                            if (res)
                            {
                                if (ReturnDescription.Trim() == "添加开奖信息成功" && !string.IsNullOrEmpty(ent.LotteryWinNum))
                                {
                                    new IsusesBLL().InsertIsuseInfoRedis(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.StartTime, ent.EndTime, ent.LotteryTime, 0, 0);
                                }
                                Console.WriteLine(string.Format("接口开奖结果：{0}", ent.ToString()));
                            }
                        }
                    }
                }
                return ent;
            }
            catch (Exception ex)
            {
                log.Write("读取XML数据失败,错误日志:" + ex.Message, true);
                return null;
            }
        }
        private LotteryResult GetValue1()
        {
            InterfaceBase InterTicker = new InterfaceBase()[xml];
            if (InterTicker == null) return null;

            try
            {
                IsusesEntity model = bllis.QueryNewSaleIsuses(LotteryCode);
                if (model == null)
                    return null;
                string IsuseName = model.IsuseName.Substring(2, model.IsuseName.Length - 2);

                LotteryResult ent = new LotteryResult();
                udv_ParaIsuse para = new udv_ParaIsuse();
                para.LotteryID = LotteryCode.ToString();
                para.Issue = IsuseName;
                udv_ResultIssue ResModel = InterTicker.GetIsuseInfo(para);

                if (ResModel.ErrorCode == "0")
                {
                    ent.LotteryCode = LotteryCode;
                    string pro = DateTime.Now.ToString().Substring(0, 2);
                    ent.IsuseName = pro + ResModel.Issue;
                    ent.StartTime = DateTime.Parse(ResModel.StartTime);
                    ent.EndTime = DateTime.Parse(ResModel.EndTime);
                    ent.LotteryTime = DateTime.Parse(ResModel.EndTime);
                    ent.LotteryWinNum = ResModel.OpenNumber.Replace(",", " ");
                    return ent;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 抓取地址 爱彩乐(icaile)
        /// </summary>
        /// <returns></returns>
        private LotteryResult GetValue3()
        {
            try
            {
                string htmlContent = HttpProxy.HttpGetProxy("http://www.cqcp.net/game/ssc/", Encoding.GetEncoding("GB2312"));
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);
                if (htmlContent == "")
                    return null;
                HtmlNode collection = doc.DocumentNode.SelectSingleNode("//div[@id='openlist']");
                if (collection != null)
                {
                    string IsuseName = collection.ChildNodes[1].ChildNodes[0].InnerText;
                    string OpenNumber = collection.ChildNodes[1].ChildNodes[1].InnerText.Replace("-", " ");
                    LotteryResult ent = new LotteryResult();
                    ent.IsuseName = IsuseName.Trim().StartsWith("20") == true ? IsuseName.Trim() : "20" + IsuseName.Trim();
                    ent.LotteryCode = LotteryCode;
                    ent.LotteryTime = DateTime.Now;
                    ent.LotteryWinNum = OpenNumber.Trim();
                    return ent;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region ToSingle函数
        /// <summary>
        /// 大小单双
        /// </summary>
        private string[] ToSingle_DX(string sNumber)
        {
            string sTmp = "";
            Match m = new Regex("(?<L0>([大小单双]))(?<L1>([大小单双]))", RegexOptions.Compiled | RegexOptions.IgnoreCase).Match(sNumber);
            for (int i = 0; i < 2; i++)
            {
                string Locate = m.Groups["L" + i.ToString()].ToString().Trim();
                if (Locate == "")
                {
                    return null;
                }
                sTmp = sTmp + Locate;
            }
            return new string[] { sTmp };
        }
        /// <summary>
        /// 大小单双
        /// </summary>
        /// <param name="OpenNumber"></param>
        /// <returns></returns>
        protected int[] ToSingle_DXDS(int OpenNumber)
        {
            //大:9  小:0  单: 1  双:2
            int[] d = new int[] { 5, 6, 7, 8, 9 };
            int[] x = new int[] { 0, 1, 2, 3, 4 };
            int[] dd = new int[] { 1, 3, 5, 7, 9 };
            int[] ss = new int[] { 0, 2, 4, 6, 8 };
            List<int> array = new List<int>();
            if (d.Contains(OpenNumber))
                array.Add(9);
            if (x.Contains(OpenNumber))
                array.Add(0);
            if (dd.Contains(OpenNumber))
                array.Add(1);
            if (ss.Contains(OpenNumber))
                array.Add(2);
            return array.ToArray();
        }

        private string FilterRepeated(string NumberPart)
        {
            string str = "";
            for (int i = 0; i < NumberPart.Length; i++)
            {
                if ((str.IndexOf(NumberPart.Substring(i, 1)) == -1) && ("0123456789-".IndexOf(NumberPart.Substring(i, 1)) >= 0))
                {
                    str = str + NumberPart.Substring(i, 1);
                }
            }
            return base.Sort(str);
        }

        /// <summary>
        /// 组三复式
        /// </summary>
        private string[] ToSingle_Zu3F(string sNumber)
        {
            int i;
            sNumber = FilterRepeated(sNumber.Trim());
            if (sNumber.Length < 2)
            {
                return null;
            }
            char[] strs = sNumber.ToCharArray();
            ArrayList al = new ArrayList();
            int n = strs.Length;
            for (i = 0; i < (n - 1); i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    al.Add(strs[i].ToString() + strs[i].ToString() + strs[j].ToString());
                    al.Add(strs[i].ToString() + strs[j].ToString() + strs[j].ToString());
                }
            }
            string[] Result = new string[al.Count];
            for (i = 0; i < al.Count; i++)
            {
                Result[i] = al[i].ToString();
            }
            return Result;
        }

        /// <summary>
        /// 三星组选单式、复式
        /// </summary>
        private string[] ToSingle_Zu3D_Zu6(string sNumber)
        {
            int num2;
            int num3;
            sNumber = this.FilterRepeated(sNumber.Trim());
            if (sNumber.Length < 2)
            {
                return null;
            }
            char[] chArray = sNumber.ToCharArray();
            ArrayList list = new ArrayList();
            int length = chArray.Length;
            if (length == 2)
            {
                for (num2 = 0; num2 < (length - 1); num2++)
                {
                    num3 = num2 + 1;
                    while (num3 < length)
                    {
                        list.Add(chArray[num2].ToString() + chArray[num2].ToString() + chArray[num3].ToString());
                        list.Add(chArray[num2].ToString() + chArray[num3].ToString() + chArray[num3].ToString());
                        num3++;
                    }
                }
            }
            else
            {
                num2 = 0;
                while (num2 < (length - 2))
                {
                    for (num3 = num2 + 1; num3 < (length - 1); num3++)
                    {
                        for (int i = num3 + 1; i < length; i++)
                        {
                            list.Add(chArray[num2].ToString() + chArray[num3].ToString() + chArray[i].ToString());
                        }
                    }
                    num2++;
                }
            }
            string[] strArray = new string[list.Count];
            for (num2 = 0; num2 < list.Count; num2++)
            {
                strArray[num2] = list[num2].ToString();
            }
            return strArray;
        }
        #endregion

        #region 采集
        /// <summary>
        /// 维护当天的期号
        /// </summary>
        public override void ModifyIsuseInfo()
        {
            try
            {
                bllis.AddIsuseAdd(LotteryCode, DateTime.Now.ToString(), 1);
            }
            catch (Exception ex)
            {
                log.Write("维护当天期号失败,错误日志:" + ex.Message, true);
            }
        }

        /// <summary>
        /// 期号截止后撤销等待拆票的订单
        /// </summary>
        /// <param name="LotteryCode"></param>
        public override void IsuseStopRevokeSchemes(int LotteryCode)
        {
            blls.IsuseStopRevokeSchemes(LotteryCode);
        }


        #region 获取遗漏的号码
        public override void GetSuppNum()
        {
            try
            {
                GetSuppNum1();  //接口获取开奖数据
            }
            catch (Exception ex)
            {
                log.Write("爬取遗漏号码失败,错误日志:" + ex.Message, true);
            }
        }

        private void GetSuppNum1()
        {
            InterfaceBase InterTicker = new InterfaceBase()[xml];
            if (InterTicker == null) return;

            List<IsusesEntity> list = bllis.QueryNumEmpty(LotteryCode);
            if (list == null || list.Count <= 0)
                return;

            foreach (IsusesEntity item in list)
            {
                try
                {
                    Thread.Sleep(3 * 1000);
                    LotteryResult ent = new LotteryResult();
                    udv_ParaIsuse para = new udv_ParaIsuse();
                    para.LotteryID = LotteryCode.ToString();
                    para.Issue = item.IsuseName;
                    udv_ResultIssue ResModel = InterTicker.GetIsuseInfo(para);

                    if (ResModel.ErrorCode == "0")
                    {
                        ent.LotteryCode = LotteryCode;
                        string pro = DateTime.Now.ToString().Substring(0, 2);
                        ent.IsuseName = pro + ResModel.Issue;
                        ent.StartTime = DateTime.Parse(ResModel.StartTime);
                        ent.EndTime = DateTime.Parse(ResModel.EndTime);
                        ent.LotteryTime = DateTime.Parse(ResModel.EndTime);
                        ent.LotteryWinNum = ResModel.OpenNumber.Replace(",", " ");

                        string ReturnDescription = "";
                        bllis.AddLotteryOpenNumber(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.StartTime, ent.EndTime, ent.LotteryTime, ref ReturnDescription);
                        if (ReturnDescription.Trim() == "添加开奖信息成功" && !string.IsNullOrEmpty(ent.LotteryWinNum))
                        {
                            new IsusesBLL().InsertIsuseInfoRedis(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.StartTime, ent.EndTime, ent.LotteryTime, 0, 0);
                        }
                    }
                    else
                    {
                        log.Write("接口返回为空");
                    }
                }
                catch { break; }
            }
        }
        #endregion
        #endregion
        /// <summary>
        /// 处理电子票出票
        /// </summary>
        /// <param name="xml"></param>
        public override void OutTicket(XmlNode xml)
        {
            try
            {
                InterfaceBase InterTicker = new InterfaceBase()[xml];
                List<udv_ParaOutTicket> para = new List<udv_ParaOutTicket>();
                List<long> SchemeIDList = new List<long>();

                int LotteryCode = Convert.ToInt32(xml.SelectSingleNode("SystemLotteryCode").InnerText);
                List<udv_OutTickets> list = new SchemesBLL().QueryOutTicketList(LotteryCode);
                if (list.Count == 0)
                    return;

                foreach (udv_OutTickets item in list)
                {
                    if (!SchemeIDList.Contains(item.SchemeID))
                        SchemeIDList.Add(item.SchemeID);
                    udv_ParaOutTicket model = new udv_ParaOutTicket()
                    {
                        SchemeETicketID = item.SchemeETicketsID
                    };
                    para.Add(model);
                }

                List<udv_OutTicketEntites> SqlPara = new List<udv_OutTicketEntites>();
                List<udv_ResultOutTicket> ResModel = InterTicker.HandleOutTicket(para);
                foreach (udv_ResultOutTicket Resitem in ResModel)
                {
                    if (Resitem.ErrorCode == "0")
                    {
                        SqlPara.AddRange(Resitem.ListOutTicker);
                    }
                }
                bllet.HandleOutTicket(SqlPara, SchemeIDList);
                blls.Redis_OutTicket(SchemeIDList, SqlPara);
            }
            catch (Exception ex)
            {
                log.Write("自动出票失败：" + ex.Message, true);
            }
        }
    }
}
