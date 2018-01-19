using CL.Enum.Common.Lottery;
using CL.Game.BLL;
using CL.Game.BLL.View;
using CL.Plugins.Award;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace CL.Tools.LotteryTickets.Lottery.SSC
{
    /// <summary>
    /// 江西时时彩
    /// 2017年5月9日
    /// </summary>
    public class JXSSC : LotteryBase
    {
        //彩种编号
        private const int LotteryCode = 302;
        static string LotteryFlag = "LotteryTickets.JXSSC";
        private readonly Log log = new Log(LotteryFlag);
        private XmlNode xml = null;

        private IsusesBLL bllis = new IsusesBLL();
        private SchemesBLL blls = new SchemesBLL();
        private SchemeETicketsBLL bllet = new SchemeETicketsBLL();
        private SchemesWinBLL bllwin = new SchemesWinBLL();

        public JXSSC()
        {
            GetWinType();
        }

        public JXSSC(XmlNode _xml)
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
            WinType model1 = new WinType() { WinCode = 3020101, PlayCode = 30201, WinName = "五星直选", DefaultMoney = 100000 * 100, DefaultMoneyNoWithTax = 80000 * 100 };
            wt.Add(model1);
            WinType model2 = new WinType() { WinCode = 3020201, PlayCode = 30202, WinName = "五星通选", DefaultMoney = 20440 * 100, DefaultMoneyNoWithTax = 16352 * 100 };
            wt.Add(model2);
            WinType model3 = new WinType() { WinCode = 3020202, PlayCode = 30202, WinName = "五星通选", DefaultMoney = 220 * 100, DefaultMoneyNoWithTax = 220 * 100 };
            wt.Add(model3);
            WinType model4 = new WinType() { WinCode = 3020203, PlayCode = 30202, WinName = "五星通选", DefaultMoney = 20 * 100, DefaultMoneyNoWithTax = 20 * 100 };
            wt.Add(model4);
            WinType model5 = new WinType() { WinCode = 3020302, PlayCode = 30203, WinName = "三星直选", DefaultMoney = 1000 * 100, DefaultMoneyNoWithTax = 1000 * 100 };
            wt.Add(model5);
            WinType model6 = new WinType() { WinCode = 3020401, PlayCode = 30204, WinName = "三星组三", DefaultMoney = 320 * 100, DefaultMoneyNoWithTax = 320 * 100 };
            wt.Add(model6);
            WinType model7 = new WinType() { WinCode = 3020501, PlayCode = 30205, WinName = "三星组六", DefaultMoney = 160 * 100, DefaultMoneyNoWithTax = 160 * 100 };
            wt.Add(model7);
            WinType model8 = new WinType() { WinCode = 3020601, PlayCode = 30206, WinName = "二星直选", DefaultMoney = 100 * 100, DefaultMoneyNoWithTax = 100 * 100 };
            wt.Add(model8);
            WinType model9 = new WinType() { WinCode = 3020701, PlayCode = 30207, WinName = "二星直选和值", DefaultMoney = 100 * 100, DefaultMoneyNoWithTax = 100 * 100 };
            wt.Add(model9);
            WinType model10 = new WinType() { WinCode = 3020801, PlayCode = 30208, WinName = "二星组选", DefaultMoney = 50 * 100, DefaultMoneyNoWithTax = 50 * 100 };
            wt.Add(model10);
            WinType model11 = new WinType() { WinCode = 3020901, PlayCode = 30209, WinName = "二星组选和值", DefaultMoney = 50 * 100, DefaultMoneyNoWithTax = 50 * 100 };
            wt.Add(model11);
            WinType model12 = new WinType() { WinCode = 3021001, PlayCode = 30210, WinName = "一星", DefaultMoney = 10 * 100, DefaultMoneyNoWithTax = 10 * 100 };
            wt.Add(model12);
            WinType model13 = new WinType() { WinCode = 3021101, PlayCode = 30211, WinName = "大小单双", DefaultMoney = 4 * 100, DefaultMoneyNoWithTax = 4 * 100 };
            wt.Add(model13);
            WinType model14 = new WinType() { WinCode = 3021201, PlayCode = 30212, WinName = "四星直选", DefaultMoney = 10000 * 100, DefaultMoneyNoWithTax = 10000 * 100 };
            wt.Add(model14);
            WinType model15 = new WinType() { WinCode = 3021301, PlayCode = 30213, WinName = "任选一", DefaultMoney = 11 * 100, DefaultMoneyNoWithTax = 11 * 100 };
            wt.Add(model15);
            WinType model16 = new WinType() { WinCode = 3021401, PlayCode = 30214, WinName = "任选二", DefaultMoney = 116 * 100, DefaultMoneyNoWithTax = 116 * 100 };
            wt.Add(model16);
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
                List<udv_ComputeTicket> Sql = new List<udv_ComputeTicket>();
                List<long> ListWinSID = new List<long>();  //中奖记录方案号
                List<long> ListNoWinSID = new List<long>(); //没有中奖记录方案号
                List<long> NewListNoWinSID = new List<long>();
                List<udv_ComputeTicket> list = new udv_ComputeTicketBLL().QueryComputeTicketList(LotteryCode);
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
                        case (int)JXSSCPlayCode.ZX_1X:
                            #region 一星直选单式
                            WinMoney = ComputeWin_ZX_1X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.DXDS:
                            #region 大小单双
                            WinMoney = ComputeWin_DXDS(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.ZX_2X:
                            #region 二星直选
                            WinMoney = ComputeWin_ZX_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.ZXHZ_2X:
                            #region 二星直选和值
                            WinMoney = ComputeWin_ZXHZ_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.ZU_2X:
                            #region 二星组选
                            WinMoney = ComputeWin_ZU_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.ZUHZ_2X:
                            #region 二星组选和值
                            WinMoney = ComputeWin_ZUHZ_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.ZX_3X:
                            #region 三星直选
                            WinMoney = ComputeWin_ZX_3X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.ZU3_3X:
                            #region 三星组选3
                            WinMoney = ComputeWin_ZU3_3X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.ZU6_3X:
                            #region 三星组选6
                            WinMoney = ComputeWin_ZU6_3X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.ZX_4X:
                            #region 四星直选
                            WinMoney = ComputeWin_ZX_4X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.ZX_5X:
                            #region 五星直选
                            WinMoney = ComputeWin_ZX_5X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.TX_5X:
                            #region 五星通选
                            WinMoney = ComputeWin_TX_5X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.RX_1X:
                            #region 任选一
                            WinMoney = ComputeWin_RX_1X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            #endregion
                            break;
                        case (int)JXSSCPlayCode.RX_2X:
                            #region 任选2
                            WinMoney = ComputeWin_RX_2X(item.Number, OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
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

        #region 奖金计算
        /// <summary>
        /// 一星直选单式 方案格式: 1,2,3,4,5
        /// </summary>
        public long ComputeWin_ZX_1X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            string[] Numbers = Number.Split(',');
            if (Numbers == null) return 0;
            if (Numbers.Length < 1) return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.ZX_1X && p.PlayCode == (int)JXSSCPlayCode.ZX_1X);

            for (int i = 0; i < Numbers.Length; i++)
            {
                if (!string.IsNullOrEmpty(Numbers[i]))
                {
                    //购买的号码只要和开奖号码最后一位相等即为中奖
                    if (Numbers[i].Equals(OpenNum[4].ToString()))
                    {
                        winNum++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
            }
            if (winNum > 0)
            {
                Description = "一星奖" + winNum.ToString() + "注";
                WinCode = (int)JXSSCWinCode.ZX_1X;
            }
            return WinMoney;
        }

        /// <summary>
        /// 大小单双    方案格式: 大小单双,大小单
        /// </summary>
        public long ComputeWin_DXDS(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.DXDS && p.PlayCode == (int)JXSSCPlayCode.DXDS);

            string WinNumber_1 = "";
            string WinNumber_2 = "";
            int Num = int.Parse(OpenNumber.Substring(0, 1), 0);
            WinNumber_1 = WinNumber_1 + ((Num <= 4) ? "小" : "大") + (((Num % 2) == 0) ? "双" : "单");
            Num = int.Parse(OpenNumber.Substring(1, 1), 0);
            WinNumber_2 = WinNumber_2 + ((Num <= 4) ? "小" : "大") + (((Num % 2) == 0) ? "双" : "单");

            string[] szLottery = this.ToSingle_DX(Number);
            if ((szLottery != null) && (szLottery.Length >= 1))
            {
                for (int i = 0; i < szLottery.Length; i++)
                {
                    if ((szLottery[i].Length >= 2) && ((WinNumber_1.IndexOf(szLottery[i][0]) >= 0) && (WinNumber_2.IndexOf(szLottery[i][1]) >= 0)))
                    {
                        winNum++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
            }

            if (winNum > 0)
            {
                Description = "大小单双奖" + winNum.ToString() + "注";
                WinCode = (int)JXSSCWinCode.DXDS;
            }

            return WinMoney;
        }

        /// <summary>
        /// 二星直选  单式方案格式: 1 2 8  复式方案格式: 012345,0123
        /// </summary>
        public long ComputeWin_ZX_2X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.ZX_2X && p.PlayCode == (int)JXSSCPlayCode.ZX_2X);

            string[] szNumber = Number.Split(' ');
            if (szNumber != null)
            {
                //复式
                if (szNumber.Length == 1 && szNumber[0].Contains(","))
                {
                    string[] sTmpNumber = szNumber[0].Split(',');
                    if (sTmpNumber != null && sTmpNumber.Length == 2)
                    {
                        //把方案内容和开奖号码最后两位对比，存在即为中奖
                        if (sTmpNumber[0].Contains(OpenNumber[3].ToString()) && sTmpNumber[1].Contains(OpenNumber[4].ToString()))
                        {
                            winNum++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }
                else
                {
                    //单式
                    foreach (string sigNumber in szNumber)
                    {
                        if (sigNumber.Equals(OpenNumber))
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
                Description = "二星直选奖" + winNum.ToString() + "注";
                WinCode = (int)JXSSCWinCode.ZX_2X;
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
            WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.ZXHZ_2X && p.PlayCode == (int)JXSSCPlayCode.ZXHZ_2X);
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
                WinCode = (int)JXSSCWinCode.ZXHZ_2X;
            }

            return WinMoney;
        }

        /// <summary>
        /// 二星组选  复式方案格式: 0,1,2,3,4,9
        /// </summary>
        public long ComputeWin_ZU_2X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            //开奖号码后两位是否为对子
            bool isPair = OpenNum[3].Equals(OpenNum[4]);

            long WinMoney = 0;
            int winNum = 0;
            //二星组选时开奖号码不能是对子
            if (!isPair)
            {
                string[] szNumber = Number.Split(',');
                if (szNumber != null)
                {
                    WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.ZU_2X && p.PlayCode == (int)JXSSCPlayCode.ZU_2X);
                    foreach (string sNumber in szNumber)
                    {
                        if (sNumber.Contains(OpenNum[3]) && sNumber.Contains(OpenNum[4]))
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
                Description = "二星组选奖" + winNum.ToString() + "注";
                WinCode = (int)JXSSCWinCode.ZU_2X;
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

            WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.ZUHZ_2X && p.PlayCode == (int)JXSSCPlayCode.ZUHZ_2X);
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
                WinCode = (int)JXSSCWinCode.ZUHZ_2X;
            }

            return WinMoney;
        }

        /// <summary>
        /// 三星直选  单式方案格式: 012 001 018  复式方案格式: 012345,0123,0123
        /// </summary>
        public long ComputeWin_ZX_3X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            string WinNumber = OpenNum[2] + OpenNum[3] + OpenNum[4];
            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.ZX_3X && p.PlayCode == (int)JXSSCPlayCode.ZX_3X);

            string[] szNumber = Number.Split(' ');
            if (szNumber != null)
            {
                //复式
                if (szNumber.Length == 1 && szNumber[0].Contains(","))
                {
                    string[] sTmpNumber = szNumber[0].Split(',');
                    if (sTmpNumber != null && sTmpNumber.Length == 3)
                    {
                        //购买号码每位存在于开奖号码则中奖一注
                        if (sTmpNumber[0].Contains(OpenNum[2].ToString()) && sTmpNumber[1].Contains(OpenNum[3].ToString())
                            && sTmpNumber[2].Contains(OpenNum[4].ToString()))
                        {
                            winNum++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }
                else
                {
                    //单式
                    foreach (string sigNumber in szNumber)
                    {
                        //按位一致则中奖
                        if (sigNumber.Equals(WinNumber))
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
                Description = "三星直选奖" + winNum.ToString() + "注";
                WinCode = (int)JXSSCWinCode.ZX_3X;
            }

            return WinMoney;
        }

        /// <summary>
        /// 三星组选3 单式、复式  单式方案格式: 012 001 018  复式方案格式: 012345678
        /// </summary>
        public long ComputeWin_ZU3_3X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
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

                //开奖号码是组三再继续
                if (isZus)
                {
                    WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.ZU3_3X && p.PlayCode == (int)JXSSCPlayCode.ZU3_3X);

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
                        Description = "三星组选3奖" + winNum.ToString() + "注";
                        WinCode = (int)JXSSCWinCode.ZU3_3X;
                    }
                }
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
                    WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.ZU6_3X && p.PlayCode == (int)JXSSCPlayCode.ZU6_3X);

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
                        WinCode = (int)JXSSCWinCode.ZU6_3X;
                    }
                }
            }

            return WinMoney;
        }

        /// <summary>
        /// 四星直选  单式方案格式: 0124 0002 0182  复式方案格式: 012345,01236,01237,789
        /// </summary>
        public long ComputeWin_ZX_4X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.ZX_4X && p.PlayCode == (int)JXSSCPlayCode.ZX_4X);

            string[] szNumber = Number.Split(' ');
            if (szNumber != null)
            {
                //复式
                if (szNumber.Length == 1 && szNumber[0].Contains(","))
                {
                    string[] sTmpNumber = szNumber[0].Split(',');
                    if (sTmpNumber != null && sTmpNumber.Length == 4)
                    {
                        if (sTmpNumber[0].Contains(OpenNum[0]) && sTmpNumber[1].Contains(OpenNum[1])
                            && sTmpNumber[2].Contains(OpenNum[2]) && sTmpNumber[3].Contains(OpenNum[3]))
                        {
                            winNum++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }
                else
                {
                    //单式
                    foreach (string sigNumber in szNumber)
                    {
                        if (sigNumber == OpenNumber)
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
                Description = "四星直选奖" + winNum.ToString() + "注";
                WinCode = (int)JXSSCWinCode.ZX_4X;
            }

            return WinMoney;
        }


        /// <summary>
        /// 五星直选  单式方案格式: 01234 00012 01782  复式方案格式: 012345,01236,01237,789,1234
        /// </summary>
        public long ComputeWin_ZX_5X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.ZX_5X && p.PlayCode == (int)JXSSCPlayCode.ZX_5X);

            string[] szNumber = Number.Split(' ');
            if (szNumber != null)
            {
                //复式
                if (szNumber.Length == 1 && szNumber[0].Contains(","))
                {
                    string[] sTmpNumber = szNumber[0].Split(',');
                    if (sTmpNumber != null && sTmpNumber.Length == 5)
                    {
                        if (sTmpNumber[0].Contains(OpenNum[0]) && sTmpNumber[1].Contains(OpenNum[1])
                            && sTmpNumber[2].Contains(OpenNum[2]) && sTmpNumber[3].Contains(OpenNum[3])
                            && sTmpNumber[4].Contains(OpenNum[4]))
                        {
                            winNum++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }
                else
                {
                    //单式
                    foreach (string sigNumber in szNumber)
                    {
                        if (sigNumber == OpenNumber)
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
                Description = "五星直选奖" + winNum.ToString() + "注";
                WinCode = (int)JXSSCWinCode.ZX_5X;
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
            long winMoneyNoWithTax = 0;

            int winNum_5XTX_1 = 0;
            int winMoney_5XTX_1 = 0;
            int winMoney_NoWithTax_5XTX_1 = 0;

            int winNum_5XTX_2 = 0;
            int winMoney_5XTX_2 = 0;
            int winMoney_NoWithTax_5XTX_2 = 0;

            int winNum_5XTX_3 = 0;
            int winMoney_5XTX_3 = 0;
            int winMoney_NoWithTax_5XTX_3 = 0;

            WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.TX_5X_ALLWin && p.PlayCode == (int)JXSSCPlayCode.TX_5X);
            int defaultMoney1 = model.DefaultMoney;
            int defaultMoneyNoWithTax1 = model.DefaultMoneyNoWithTax;

            WinType model2 = wt.Find(p => p.WinCode == (int)JXSSCWinCode.TX_5X_Q3HH3 && p.PlayCode == (int)JXSSCPlayCode.TX_5X);
            int defaultMoney2 = model2.DefaultMoney;
            int defaultMoneyNoWithTax2 = model2.DefaultMoneyNoWithTax;

            WinType model3 = wt.Find(p => p.WinCode == (int)JXSSCWinCode.TX_5X_Q2HH2 && p.PlayCode == (int)JXSSCPlayCode.TX_5X);
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
                string[] szNumber = Number.Split(' ');

                if (szNumber == null || szNumber.Length < 1)
                    return 0;
                else
                {
                    foreach (string sNumber in szNumber)
                    {
                        if (sNumber[0].Equals(OpenNumber[0]) && sNumber[1].Equals(OpenNumber[1]) &&
                            sNumber[2].Equals(OpenNumber[2]) && sNumber[3].Equals(OpenNumber[3]) &&
                            sNumber[4].Equals(OpenNumber[4]))
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
                        else if ((sNumber[0].Equals(OpenNumber[0]) && sNumber[1].Equals(OpenNumber[1]) &&
                           sNumber[2].Equals(OpenNumber[2])) || (sNumber[2].Equals(OpenNumber[2]) &&
                           sNumber[3].Equals(OpenNumber[3]) && sNumber[4].Equals(OpenNumber[4])))
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
                            if (sNumber[0].Equals(OpenNumber[0]) && sNumber[1].Equals(OpenNumber[1]))
                            {
                                winNum_5XTX_3++;

                                winMoney_5XTX_3 += defaultMoney3;
                                winMoney_NoWithTax_5XTX_3 += defaultMoneyNoWithTax3;
                            }
                            if (sNumber[3].Equals(OpenNumber[3]) && sNumber[4].Equals(OpenNumber[4]))
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
            winMoneyNoWithTax = winMoney_NoWithTax_5XTX_1 + winMoney_NoWithTax_5XTX_2 + winMoney_NoWithTax_5XTX_3;

            if (WinMoney > 0 && WinMoney != winMoneyNoWithTax)
            {
                //如果税前税后奖金不等,则添加进奖金描述
                Description += "税前奖金: " + WinMoney / 100 + "元,税后奖金: " + winMoneyNoWithTax / 100 + "元";
            }
            //WinCode = (int)JXSSCWinCodeEnum;
            //奖金代码有问题,不能返回多个代码
            return WinMoney;
        }

        /// <summary>
        /// 任选一 方案格式: 123,012,3,4,5
        /// </summary>
        public long ComputeWin_RX_1X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            string[] Numbers = Number.Split(',');
            if (Numbers == null) return 0;
            if (Numbers.Length < 1) return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.RX_1X && p.PlayCode == (int)JXSSCPlayCode.RX_1X);

            //按位查找
            for (int i = 0; i < 5; i++)
            {
                //只要对应的位数包含开奖号码 则中奖
                if (Numbers[i].Contains(OpenNum[i]))
                {
                    winNum++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
            }

            if (winNum > 0)
            {
                Description = "任选一奖" + winNum.ToString() + "注";
                WinCode = (int)JXSSCWinCode.RX_1X;
            }
            return WinMoney;
        }

        /// <summary>
        /// 任选二 方案格式: 123,012,3,4,5
        /// </summary>
        public long ComputeWin_RX_2X(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            string[] szLottery = SplitLotteryNumber(Number);
            if (szLottery == null || szLottery.Length < 1)
                return 0;

            long WinMoney = 0;
            int winNum = 0;
            WinType model = wt.Find(p => p.WinCode == (int)JXSSCWinCode.RX_2X && p.PlayCode == (int)JXSSCPlayCode.RX_2X);

            for (int i = 0; i < szLottery.Length; i++)
            {
                if (szLottery[i].Length < 5)
                    continue;

                int Flag = 0;

                for (int j = 0; j < szLottery[i].Length; j++)
                {
                    if (szLottery[i].Substring(j, 1) == OpenNumber.Substring(j, 1))
                    {
                        Flag++;
                    }

                    if (Flag == 2)
                    {
                        winNum++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;

                        break;
                    }
                }
            }

            if (winNum > 0)
            {
                Description = "任选一奖" + winNum.ToString() + "注";
                WinCode = (int)JXSSCWinCode.RX_1X;
            }
            return WinMoney;
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

        /// <summary>
        /// 任选二
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="CanonicalNumber"></param>
        /// <returns></returns>
        private string[] ToSingle_RX2(string Number, ref string CanonicalNumber)
        {
            string[] Locate = new string[5];
            CanonicalNumber = "";

            Regex regex = new Regex(@"^(?<L0>(\d)|(-)|([(][\d]+?[)]))(?<L1>(\d)|(-)|([(][\d]+?[)]))(?<L2>(\d)|(-)|([(][\d]+?[)]))(?<L3>(\d)|(-)|([(][\d]+?[)]))(?<L4>(\d)|(-)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(Number);
            for (int i = 0; i < 5; i++)
            {
                Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();
                if (Locate[i] == "")
                {
                    CanonicalNumber = "";
                    return null;
                }
                if (Locate[i].Length > 1)
                {
                    Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                    if (Locate[i].Length > 1)
                        Locate[i] = FilterRepeated(Locate[i]);
                    if (Locate[i] == "")
                    {
                        CanonicalNumber = "";
                        return null;
                    }
                }
                if (Locate[i].Length > 1)
                    CanonicalNumber += "(" + Locate[i] + ")";
                else
                    CanonicalNumber += Locate[i];

                if (Locate[i] != "-")
                {
                    Locate[i] = Locate[i] + "-";
                }
            }

            int Num = 0;

            ArrayList al = new ArrayList();

            #region 循环取单式
            for (int i_0 = 0; i_0 < Locate[0].Length; i_0++)
            {
                string str_0 = Locate[0][i_0].ToString();

                if (Locate[0][i_0].ToString() != "-")
                {
                    Num++;
                }

                for (int i_1 = 0; i_1 < Locate[1].Length; i_1++)
                {
                    string str_1 = str_0 + Locate[1][i_1].ToString();

                    if ((Num == 1) && (Locate[1][i_1].ToString() != "-"))
                    {
                        al.Add(str_1 + "---");
                        Num = 1;

                        continue;
                    }

                    if (Locate[1][i_1].ToString() != "-")
                    {
                        Num++;
                    }

                    for (int i_2 = 0; i_2 < Locate[2].Length; i_2++)
                    {
                        string str_2 = str_1 + Locate[2][i_2].ToString();

                        if ((Num == 1) && (Locate[2][i_2].ToString() != "-"))
                        {
                            al.Add(str_2 + "--");
                            Num = 1;
                            continue;
                        }

                        if (Locate[2][i_2].ToString() != "-")
                        {
                            Num++;
                        }

                        for (int i_3 = 0; i_3 < Locate[3].Length; i_3++)
                        {
                            string str_3 = str_2 + Locate[3][i_3].ToString();

                            if ((Num == 1) && (Locate[3][i_3].ToString() != "-"))
                            {
                                al.Add(str_3 + "-");
                                Num = 1;
                                continue;
                            }

                            if (Locate[3][i_3].ToString() != "-")
                            {
                                Num++;
                            }

                            for (int i_4 = 0; i_4 < Locate[4].Length; i_4++)
                            {
                                string str_4 = str_3 + Locate[4][i_4].ToString();

                                if ((Num == 1) && (Locate[4][i_4].ToString() != "-"))
                                {
                                    al.Add(str_4);
                                    Num = 1;

                                    continue;
                                }

                                Num = 0;
                            }
                        }
                    }
                }
            }
            #endregion

            string[] Result = new string[al.Count];
            for (int i = 0; i < al.Count; i++)
                Result[i] = al[i].ToString();
            return Result;
        }
        #endregion
    }
}
