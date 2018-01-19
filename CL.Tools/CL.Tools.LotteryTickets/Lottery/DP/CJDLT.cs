using CL.Game.BLL;
using CL.Game.BLL.Tools;
using CL.Game.BLL.View;
using CL.Game.Entity;
using CL.Plugins.Award;
using CL.Tools.Common;
using CL.Tools.TicketInterface;
using CL.View.Entity.Game;
using CL.View.Entity.Interface;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace CL.Tools.LotteryTickets.Lottery.DP
{
    /// <summary>
    /// 超级大乐透
    /// </summary>
    public class CJDLT : LotteryBase
    {
        private const int LotteryCode = 901;
        private readonly Log log = new Log("LotteryTickets.DLT");
        private XmlNode xml = null;

        
        
        
                   

        public CJDLT()
        {
            GetWinType();
        }
        public CJDLT(XmlNode _xml)
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
            WinType model1 = new WinType() { WinCode = 9010101, PlayCode = 90101, WinName = "一等奖", DefaultMoney = 0, DefaultMoneyNoWithTax = 0 };
            wt.Add(model1);
            WinType model2 = new WinType() { WinCode = 9010102, PlayCode = 90101, WinName = "二等奖", DefaultMoney = 0, DefaultMoneyNoWithTax = 0 };
            wt.Add(model2);
            WinType model3 = new WinType() { WinCode = 9010103, PlayCode = 90101, WinName = "三等奖", DefaultMoney = 0, DefaultMoneyNoWithTax = 0 };
            wt.Add(model3);
            WinType model4 = new WinType() { WinCode = 9010104, PlayCode = 90101, WinName = "四等奖", DefaultMoney = 200 * 100, DefaultMoneyNoWithTax = 200 * 100 };
            wt.Add(model4);
            WinType model5 = new WinType() { WinCode = 9010105, PlayCode = 90101, WinName = "五等奖", DefaultMoney = 10 * 100, DefaultMoneyNoWithTax = 10 * 100 };
            wt.Add(model5);
            WinType model6 = new WinType() { WinCode = 9010106, PlayCode = 90101, WinName = "六等奖", DefaultMoney = 5 * 100, DefaultMoneyNoWithTax = 5 * 100 };
            wt.Add(model6);
            WinType model7 = new WinType() { WinCode = 9010201, PlayCode = 90102, WinName = "一等奖追加", DefaultMoney = 0, DefaultMoneyNoWithTax = 0 };
            wt.Add(model7);
            WinType model8 = new WinType() { WinCode = 9010202, PlayCode = 90102, WinName = "二等奖追加", DefaultMoney = 0, DefaultMoneyNoWithTax = 0 };
            wt.Add(model8);
            WinType model9 = new WinType() { WinCode = 9010203, PlayCode = 90102, WinName = "三等奖追加", DefaultMoney = 0, DefaultMoneyNoWithTax = 0 };
            wt.Add(model9);
            WinType model10 = new WinType() { WinCode = 9010204, PlayCode = 90102, WinName = "四等奖追加", DefaultMoney = 200 * 100, DefaultMoneyNoWithTax = 200 * 100 };
            wt.Add(model10);
            WinType model11 = new WinType() { WinCode = 9010205, PlayCode = 90102, WinName = "五等奖追加", DefaultMoney = 10 * 100, DefaultMoneyNoWithTax = 10 * 100 };
            wt.Add(model11);
            #endregion
        }

        #region 采集
        /// <summary>
        /// 维护当天的期号
        /// </summary>
        public override void ModifyIsuseInfo()
        {
            #region
            #endregion
        }

        /// <summary>
        /// 期号截止后撤销等待拆票的订单
        /// </summary>
        /// <param name="LotteryCode"></param>
        public override void IsuseStopRevokeSchemes(int LotteryCode)
        {
            new SchemesBLL().IsuseStopRevokeSchemes(LotteryCode);
        }

        #region 正常采集数据
        public override LotteryResult GetValue()
        {
            try
            {
                IsusesBLL bllis = new IsusesBLL();
                LotteryResult ent = new LotteryResult();
                ent = GetValue2();
                if (ent != null)
                {
                    #region 新的开奖方式
                    string ReturnDescription = "";
                    bool res = bllis.EnteringDrawResults(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.LotteryTime, ref ReturnDescription);
                    if (res)
                    {
                        if (ReturnDescription.Trim() == "添加开奖信息成功" && !string.IsNullOrEmpty(ent.LotteryWinNum))
                        {
                            new IsusesBLL().InsertIsuseInfoRedis(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.StartTime, ent.EndTime, DateTime.Now, 0, 0);
                        }
                        Console.WriteLine(string.Format("抓取开奖结果：{0}", ent.ToString()));
                    }
                    #endregion
                }
                else
                {
                    ent = GetValue3();
                    if (ent != null)
                    {
                        string ReturnDescription = "";
                        bool res = bllis.EnteringDrawResults(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.LotteryTime, ref ReturnDescription);
                        if (res)
                        {
                            if (ReturnDescription.Trim() == "添加开奖信息成功" && !string.IsNullOrEmpty(ent.LotteryWinNum))
                            {
                                new IsusesBLL().InsertIsuseInfoRedis(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.StartTime, ent.EndTime, DateTime.Now, 0, 0);
                            }
                            Console.WriteLine(string.Format("抓取开奖结果：{0}", ent.ToString()));
                        }
                    }
                }
                return ent;
            }
            catch (Exception ex)
            {
                log.Write("爬取数据失败,错误日志:" + ex.Message, true);
                return null;
            }
        }

        private LotteryResult GetValue1()
        {

            IsusesBLL bllis = new IsusesBLL();
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
        /// 163彩票网抓取
        /// </summary>
        /// <returns></returns>
        private LotteryResult GetValue2()
        {
            try
            {
                string htmlContent = Utils.HttpGet("http://caipiao.163.com/order/dlt/");
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);
                if (htmlContent == "")
                    return null;
                HtmlNode collection_GetUrl = doc.DocumentNode.SelectSingleNode("//div[@class='n_kjgg']");
                LotteryResult ent = new LotteryResult();

                string IsuseName = collection_GetUrl.ChildNodes[3].InnerText.Trim().Replace("第", "").Replace("期", "");
                IsuseName = IsuseName.StartsWith("20") == true ? IsuseName : "20" + IsuseName;
                string OpenNumber = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                                    collection_GetUrl.ChildNodes[9].InnerText,
                                    collection_GetUrl.ChildNodes[10].InnerText,
                                    collection_GetUrl.ChildNodes[11].InnerText,
                                    collection_GetUrl.ChildNodes[12].InnerText,
                                    collection_GetUrl.ChildNodes[13].InnerText,
                                    collection_GetUrl.ChildNodes[14].InnerText,
                                    collection_GetUrl.ChildNodes[15].InnerText
                                    );

                ent.LotteryCode = LotteryCode;
                ent.IsuseName = IsuseName;
                ent.LotteryTime = DateTime.Now;
                ent.LotteryWinNum = OpenNumber;
                return ent;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 中国体彩网抓取
        /// </summary>
        /// <returns></returns>
        private LotteryResult GetValue3()
        {
            try
            {
                LotteryResult ent = new LotteryResult();
                //体彩官网抓取超级大乐透开奖详情页
                string htmlContent = HttpProxy.HttpGetProxy("http://www.lottery.gov.cn/historykj/history.jspx?_ltype=dlt");
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);
                if (htmlContent == "")
                    return null;
                HtmlNode collection_GetUrl = doc.DocumentNode.SelectSingleNode("//div[@class='result']/table/tbody");
                var Nodes = collection_GetUrl.ChildNodes[1];
                string IsuseName = Nodes.ChildNodes[1].InnerText.Trim();
                IsuseName = IsuseName.StartsWith("20") == true ? IsuseName : "20" + IsuseName;
                string OpenNumber = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                                    Nodes.ChildNodes[3].InnerText,
                                    Nodes.ChildNodes[5].InnerText,
                                    Nodes.ChildNodes[7].InnerText,
                                    Nodes.ChildNodes[9].InnerText,
                                    Nodes.ChildNodes[11].InnerText,
                                    Nodes.ChildNodes[13].InnerText,
                                    Nodes.ChildNodes[15].InnerText
                                    );
                ent.LotteryCode = LotteryCode;
                ent.IsuseName = IsuseName;
                ent.LotteryWinNum = OpenNumber;
                ent.LotteryTime = DateTime.Now;
                return ent;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 获取遗漏的号码
        public override void GetSuppNum()
        {
            try
            {
                GetSuppNum1();
            }
            catch (Exception ex)
            {
                log.Write("爬取遗漏号码失败,错误日志:" + ex.Message, true);
            }
        }

        private void GetSuppNum1()
        {
            try
            {

                IsusesBLL bllis = new IsusesBLL();
                string htmlContent = Utils.HttpGet("http://www.lecai.com/lottery/draw/list/1");
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);
                if (htmlContent == "")
                    return;

                var collection = doc.DocumentNode.SelectNodes("//table[@class='historylist']/tbody/tr");
                foreach (var item in collection)
                {
                    Thread.Sleep(1 * 1000);

                    LotteryResult ent = new LotteryResult();
                    ent.LotteryCode = LotteryCode;
                    string pro = DateTime.Now.ToString().Substring(0, 2);
                    ent.IsuseName = pro + item.ChildNodes[1].InnerText;
                    string OpenTime = item.ChildNodes[3].InnerText.Substring(0, 10);
                    string datetype = item.ChildNodes[3].InnerText.Substring(item.ChildNodes[3].InnerText.IndexOf('（') + 1, 1);
                    ent.StartTime = DateTime.Parse(OpenTime).AddDays(datetype == "六" ? -3 : -2).AddHours(20).AddMinutes(30);
                    ent.EndTime = DateTime.Parse(OpenTime).AddHours(19).AddMinutes(30);
                    ent.LotteryTime = DateTime.Parse(OpenTime).AddHours(21).AddMinutes(15);
                    string[] OpenNum = SplitLotteryNumber(item.ChildNodes[5].InnerText.Replace(" ", "").Replace("\r", "").Replace("\n", "").Trim('\n'));
                    ent.LotteryWinNum = String.Join(" ", OpenNum);

                    string ReturnDescription = "";
                    bllis.AddLotteryOpenNumber(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.StartTime, ent.EndTime, ent.LotteryTime, ref ReturnDescription);
                    if (ReturnDescription.Trim() == "添加开奖信息成功" && !string.IsNullOrEmpty(ent.LotteryWinNum))
                    {
                        new IsusesBLL().InsertIsuseInfoRedis(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.StartTime, ent.EndTime, ent.LotteryTime, 0, 0);
                    }
                }
            }
            catch {
                throw;
            }
        }
        #endregion
        #endregion
        
        public override long ComputeWin(udv_ComputeTicket model, ref string Description, ref long WinMoneyNoWithTax)
        {
            long WinMoney = 0;
            if (model == null)
                return 0;
            if (wt.Count <= 0)
                return 0;

            int Bet1 = 0, Bet2 = 0, Bet3 = 0, Bet4 = 0, Bet5 = 0, Bet6 = 0;
            #region 计算中奖
            string[] OpenNum = model.OpenNumber.Split(' ');
            string[] OpenQianQuNum = { OpenNum[0], OpenNum[1], OpenNum[2], OpenNum[3], OpenNum[4] };
            string[] OpenHouQuNum = { OpenNum[5], OpenNum[6] };

            if (model.Number.IndexOf('*') > -1)    //胆拖
            {
                #region
                string[] arr = model.Number.Split('|');
                string[] QianQuD = SplitLotteryNumber(arr[0].Split('*')[0]);    //前区 B
                string[] QianQuT = SplitLotteryNumber(arr[0].Split('*')[1]);    //前区 T
                string[] HouQuD = SplitLotteryNumber(arr[1].Split('*')[0]);     //后去 B
                string[] HouQuT = SplitLotteryNumber(arr[1].Split('*')[1]);     //后去 T

                int n = QianQuT.Length;
                int w = HouQuT.Length;

                if (QianQuD.Length == 1)
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
                                    string[] QianQuNum = { QianQuD[0], QianQuT[i], QianQuT[j], QianQuT[x], QianQuT[y] };
                                    for (int a = 0; a < w; a++)
                                    {
                                        string[] RedNum = { HouQuD[0], HouQuT[a] };
                                        NumComparison(OpenQianQuNum, OpenHouQuNum, QianQuNum, RedNum, model.PlayCode, ref Bet1, ref Bet2, ref Bet3, ref Bet4, ref Bet5, ref Bet6, ref WinMoney, ref WinMoneyNoWithTax);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (QianQuD.Length == 2)
                {
                    #region
                    for (int i = 0; i < n - 2; i++)
                    {
                        for (int j = i + 1; j < n - 1; j++)
                        {
                            for (int x = j + 1; x < n; x++)
                            {
                                string[] QianQuNum = { QianQuD[0], QianQuD[1], QianQuT[i], QianQuT[j], QianQuT[x] };
                                for (int a = 0; a < w; a++)
                                {
                                    string[] RedNum = { HouQuD[0], HouQuT[a] };
                                    NumComparison(OpenQianQuNum, OpenHouQuNum, QianQuNum, RedNum, model.PlayCode, ref Bet1, ref Bet2, ref Bet3, ref Bet4, ref Bet5, ref Bet6, ref WinMoney, ref WinMoneyNoWithTax);
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (QianQuD.Length == 3)
                {
                    #region
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = i + 1; j < n; j++)
                        {
                            string[] QianQuNum = { QianQuD[0], QianQuD[1], QianQuD[2], QianQuT[i], QianQuT[j] };
                            for (int a = 0; a < w; a++)
                            {
                                string[] RedNum = { HouQuD[0], HouQuT[a] };
                                NumComparison(OpenQianQuNum, OpenHouQuNum, QianQuNum, RedNum, model.PlayCode, ref Bet1, ref Bet2, ref Bet3, ref Bet4, ref Bet5, ref Bet6, ref WinMoney, ref WinMoneyNoWithTax);
                            }
                        }
                    }
                    #endregion
                }
                else if (QianQuD.Length == 4)
                {
                    #region
                    for (int i = 0; i < n; i++)
                    {
                        string[] QianQuNum = { QianQuD[0], QianQuD[1], QianQuD[2], QianQuD[3], QianQuT[i] };
                        for (int a = 0; a < w; a++)
                        {
                            string[] RedNum = { HouQuD[0], HouQuT[a] };
                            NumComparison(OpenQianQuNum, OpenHouQuNum, QianQuNum, RedNum, model.PlayCode, ref Bet1, ref Bet2, ref Bet3, ref Bet4, ref Bet5, ref Bet6, ref WinMoney, ref WinMoneyNoWithTax);
                        }
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region
                string[] Num = model.Number.Split('^');
                foreach (string strNum in Num)
                {
                    string[] num = strNum.Split('|');
                    string[] QianQu = SplitLotteryNumber(num[0]);
                    string[] HouQu = SplitLotteryNumber(num[1]);
                    int n = QianQu.Length;

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
                                        string[] QianQuNum = { QianQu[i], QianQu[j], QianQu[x], QianQu[y], QianQu[z] };
                                        for (int a = 0; a < HouQu.Length; a++)
                                        {
                                            for (int b = a + 1; b < HouQu.Length - 1; b++)
                                            {
                                                string[] HouQuNum = { HouQu[a], HouQu[b] };
                                                NumComparison(OpenQianQuNum, OpenHouQuNum, QianQuNum, HouQuNum, model.PlayCode, ref Bet1, ref Bet2, ref Bet3, ref Bet4, ref Bet5, ref Bet6, ref WinMoney, ref WinMoneyNoWithTax);
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
            #endregion

            #region
            if (Bet1 > 0)
                Description += "一等奖" + Bet1 + "注，";
            if (Bet2 > 0)
                Description += "二等奖" + Bet2 + "注，";
            if (Bet3 > 0)
                Description += "三等奖" + Bet3 + "注，";
            if (Bet4 > 0)
                Description += "四等奖" + Bet4 + "注，";
            if (Bet5 > 0)
                Description += "五等奖" + Bet5 + "注，";
            if (Bet6 > 0)
                Description += "六等奖" + Bet6 + "注。";

            if (Bet1 > 0 || Bet2 > 0)   //一等奖、二等奖、三等奖的通过人工服务派奖维护
            {
                WinMoney = 0;
                WinMoneyNoWithTax = 0;
            }
            #endregion

            WinMoney = WinMoney * model.Multiple;
            WinMoneyNoWithTax = WinMoneyNoWithTax * model.Multiple;
            return WinMoney;
        }

        private void NumComparison(string[] OpenQianQuNum, string[] OpenHouQuNum, string[] QianQuNum, string[] HouQuNum, int PlayCode, ref int Bet1, ref int Bet2, ref int Bet3,
            ref int Bet4, ref int Bet5, ref int Bet6, ref long WinMoney, ref long WinMoneyNoWithTax)
        {
            int QianQu = 0, HouQu = 0;
            foreach (string str in QianQuNum)
            {
                if (OpenQianQuNum.Contains(str))
                    QianQu++;
            }
            foreach (string str in HouQuNum)
            {
                if (OpenHouQuNum.Contains(str))
                    HouQu++;
            }

            //六等奖   3+0、2+1、1+2、0+2
            if ((QianQu == 0 && HouQu == 2) || (QianQu == 1 && HouQu == 2) || (QianQu == 2 && HouQu == 1) || (QianQu == 3 && HouQu == 0))
            {
                Bet6++;
                WinType model = wt.Find(p => p.WinCode == 9010106 && p.PlayCode == 90101);
                WinMoney += model.DefaultMoney;
                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
            }
            //五等奖、五等奖追加   4+0、3+1、2+2
            if ((QianQu == 4 && HouQu == 0) || (QianQu == 3 && HouQu == 1) || (QianQu == 2 && HouQu == 2))
            {
                Bet5++;
                WinType model = wt.Find(p => p.WinCode == (PlayCode == 90101 ? 9010105 : 9010205) && p.PlayCode == PlayCode);
                WinMoney += model.DefaultMoney;
                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
            }
            //四等奖、四等奖追加   4+1、3+2
            if ((QianQu == 4 && HouQu == 1) || (QianQu == 3 && HouQu == 2))
            {
                Bet4++;
                WinType model = wt.Find(p => p.WinCode == (PlayCode == 90101 ? 9010104 : 9010204) && p.PlayCode == PlayCode);
                WinMoney += model.DefaultMoney;
                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
            }
            //三等奖、三等奖追加   5+0、4+2
            if ((QianQu == 5 && HouQu == 0) || (QianQu == 4 && HouQu == 2))
            {
                Bet3++;
                WinType model = wt.Find(p => p.WinCode == (PlayCode == 90101 ? 9010103 : 9010203) && p.PlayCode == PlayCode);
                WinMoney += model.DefaultMoney;
                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
            }
            //二等奖、二等奖追加   5+1
            if (QianQu == 5 && HouQu == 1)
            {
                Bet2++;
                WinType model = wt.Find(p => p.WinCode == (PlayCode == 90101 ? 9010102 : 9010202) && p.PlayCode == PlayCode);
                WinMoney += model.DefaultMoney;
                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
            }
            //一等奖、一等奖追加   5+2
            if (QianQu == 5 && HouQu == 2)
            {
                Bet1++;
                WinType model = wt.Find(p => p.WinCode == (PlayCode == 90101 ? 9010101 : 9010201) && p.PlayCode == PlayCode);
                WinMoney += model.DefaultMoney;
                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
            }
        }

        /// <summary>
        /// 派奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="SupplierID"></param>
        public override void AwardWin(XmlNode xml)
        {
            try
            {
                //非追号
                Task.Factory.StartNew(() =>
                {
                    this.AwardWin_NotChase(xml);
                });
                //追号
                Task.Factory.StartNew(() =>
                {
                    this.AwardWin_Chase(xml);
                });
            }
            catch (Exception ex)
            {
                log.Write("自动派奖失败：" + ex.Message, true);
            }
        }
        /// <summary>
        /// 非追号派奖
        /// </summary>
        protected void AwardWin_NotChase(XmlNode xml)
        {
            SchemesBLL blls = new SchemesBLL();
            SchemeETicketsBLL bllet = new SchemeETicketsBLL();
            SchemesWinBLL bllwin = new SchemesWinBLL();
            InterfaceBase InterTicker = new InterfaceBase()[xml];
            List<udv_ParaWinInfo> para = new List<udv_ParaWinInfo>();
            List<long> SchemeIDList = new List<long>();

            int LotteryCode = Convert.ToInt32(xml.SelectSingleNode("SystemLotteryCode").InnerText);
            string InterfaceType = xml.SelectSingleNode("InterfaceType").InnerText;
            List<udv_SchemesDetailsWin> AwardList = bllwin.QueryAwardListByNotChase(LotteryCode);
            if (AwardList.Count == 0) return;

            foreach (udv_SchemesDetailsWin item in AwardList)
            {
                if (!SchemeIDList.Contains(item.SchemeID))  //记录本次处理的方案号
                    SchemeIDList.Add(item.SchemeID);
                udv_ParaWinInfo model = new udv_ParaWinInfo()
                {
                    SchemeETicketID = item.SchemeETicketsID,
                    WinMoney = item.WinMoney,
                    WinMoneyNoWithTax = item.WinMoneyNoWithTax
                };
                para.Add(model);
            }

            List<udv_WinInfoEntites> SqlPara = new List<udv_WinInfoEntites>();
            List<udv_ResultWinInfo> ResModel = InterTicker.HandleWinInfo(para);
            if (ResModel != null)
                foreach (udv_ResultWinInfo ResItem in ResModel)
                {
                    if (ResItem.ErrorCode == "0")
                    {
                        //记录中奖的数据
                        foreach (udv_WinInfoEntites item in ResItem.ListWinInfo)
                        {
                            if (item.Status == 1 || item.Status == 2)//未中奖   中奖
                            {
                                item.SchemeID = AwardList.Where(w => w.SchemeETicketsID == Convert.ToInt64(item.SchemeETicketID)).Select(s => s.SchemeID).FirstOrDefault();
                                SqlPara.Add(item);
                            }
                            //未开奖和可算奖的落到下一次派奖
                        }
                    }
                }
            bllet.AutomaticComputeWin(SqlPara, SchemeIDList, InterfaceType);
            blls.Redis_AwardServer(SqlPara);
        }
        /// <summary>
        /// 追号派奖
        /// </summary>
        protected void AwardWin_Chase(XmlNode xml)
        {
            SchemesBLL blls = new SchemesBLL();
            SchemeETicketsBLL bllet = new SchemeETicketsBLL();
            SchemesWinBLL bllwin = new SchemesWinBLL();
            InterfaceBase InterTicker = new InterfaceBase()[xml];
            List<udv_ParaWinInfo> para = new List<udv_ParaWinInfo>();
            List<long> SchemeIDList = new List<long>();

            int LotteryCode = Convert.ToInt32(xml.SelectSingleNode("SystemLotteryCode").InnerText);
            string InterfaceType = xml.SelectSingleNode("InterfaceType").InnerText;
            List<udv_SchemesDetailsWin> AwardList = bllwin.QueryAwardListByChase(LotteryCode);
            if (AwardList.Count == 0) return;

            foreach (udv_SchemesDetailsWin item in AwardList)
            {
                if (!SchemeIDList.Contains(item.SchemeID))  //记录本次处理的方案号
                    SchemeIDList.Add(item.SchemeID);
                udv_ParaWinInfo model = new udv_ParaWinInfo()
                {
                    SchemeETicketID = item.SchemeETicketsID,
                    WinMoney = item.WinMoney,
                    WinMoneyNoWithTax = item.WinMoneyNoWithTax
                };
                para.Add(model);
            }

            List<udv_WinInfoEntites> SqlPara = new List<udv_WinInfoEntites>();
            List<udv_ResultWinInfo> ResModel = InterTicker.HandleWinInfo(para);
            if (ResModel != null)
                foreach (udv_ResultWinInfo ResItem in ResModel)
                {
                    if (ResItem.ErrorCode == "0")
                    {
                        //记录中奖的数据
                        foreach (udv_WinInfoEntites item in ResItem.ListWinInfo)
                        {
                            if (item.Status == 1 || item.Status == 2)   //未中奖   中奖
                            {
                                var Entity = AwardList.Where(w => w.SchemeETicketsID == Convert.ToInt64(item.SchemeETicketID)).FirstOrDefault();
                                item.SchemeID = Entity.SchemeID;
                                item.ChaseTaskDetailsID = Entity.ChaseTaskDetailsID;
                                SqlPara.Add(item);
                            }
                            //未开奖和可算奖的落到下一次派奖
                        }
                    }
                }
            bllet.AutomaticComputeWin(SqlPara, SchemeIDList, InterfaceType);
            blls.Redis_AwardServer_Chase(SqlPara);
        }

        /// <summary>
        /// 处理电子票出票
        /// </summary>
        /// <param name="xml"></param>
        public override void OutTicket(XmlNode xml)
        {
            try
            {
                SchemesBLL blls = new SchemesBLL();
                SchemeETicketsBLL bllet = new SchemeETicketsBLL();
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
