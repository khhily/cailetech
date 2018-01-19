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
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace CL.Tools.LotteryTickets.Lottery.K3
{
    /// <summary>
    /// 江西快三
    /// 2017年5月9日
    /// </summary>
    public class JXK3 : LotteryBase
    {
        public const int LotteryCode = 102;
        private readonly Log log = new Log("LotteryTickets.JXK3");
        private XmlNode xml = null;

        private IsusesBLL bllis = new IsusesBLL();
        private SchemesBLL blls = new SchemesBLL();
        private SchemeETicketsBLL bllet = new SchemeETicketsBLL();
        private SchemesWinBLL bllwin = new SchemesWinBLL();

        public JXK3()
        {
            GetWinType();
        }
        public JXK3(XmlNode _xml)
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
            WinType model1 = new WinType() { WinCode = 1020104, PlayCode = 10201, WinName = "和值4", DefaultMoney = 80 * 100, DefaultMoneyNoWithTax = 80 * 100 };
            wt.Add(model1);
            WinType model2 = new WinType() { WinCode = 1020105, PlayCode = 10201, WinName = "和值5", DefaultMoney = 40 * 100, DefaultMoneyNoWithTax = 40 * 100 };
            wt.Add(model2);
            WinType model3 = new WinType() { WinCode = 1020106, PlayCode = 10201, WinName = "和值6", DefaultMoney = 25 * 100, DefaultMoneyNoWithTax = 25 * 100 };
            wt.Add(model3);
            WinType model4 = new WinType() { WinCode = 1020107, PlayCode = 10201, WinName = "和值7", DefaultMoney = 16 * 100, DefaultMoneyNoWithTax = 16 * 100 };
            wt.Add(model4);
            WinType model5 = new WinType() { WinCode = 1020108, PlayCode = 10201, WinName = "和值8", DefaultMoney = 12 * 100, DefaultMoneyNoWithTax = 12 * 100 };
            wt.Add(model5);
            WinType model6 = new WinType() { WinCode = 1020109, PlayCode = 10201, WinName = "和值9", DefaultMoney = 10 * 100, DefaultMoneyNoWithTax = 10 * 100 };
            wt.Add(model6);
            WinType model7 = new WinType() { WinCode = 1020110, PlayCode = 10201, WinName = "和值10", DefaultMoney = 9 * 100, DefaultMoneyNoWithTax = 9 * 100 };
            wt.Add(model7);
            WinType model8 = new WinType() { WinCode = 1020111, PlayCode = 10201, WinName = "和值11", DefaultMoney = 9 * 100, DefaultMoneyNoWithTax = 9 * 100 };
            wt.Add(model8);
            WinType model9 = new WinType() { WinCode = 1020112, PlayCode = 10201, WinName = "和值12", DefaultMoney = 10 * 100, DefaultMoneyNoWithTax = 10 * 100 };
            wt.Add(model9);
            WinType model10 = new WinType() { WinCode = 1020113, PlayCode = 10201, WinName = "和值13", DefaultMoney = 12 * 100, DefaultMoneyNoWithTax = 12 * 100 };
            wt.Add(model10);
            WinType model11 = new WinType() { WinCode = 1020114, PlayCode = 10201, WinName = "和值14", DefaultMoney = 16 * 100, DefaultMoneyNoWithTax = 16 * 100 };
            wt.Add(model11);
            WinType model12 = new WinType() { WinCode = 1020115, PlayCode = 10201, WinName = "和值15", DefaultMoney = 25 * 100, DefaultMoneyNoWithTax = 25 * 100 };
            wt.Add(model12);
            WinType model13 = new WinType() { WinCode = 1020116, PlayCode = 10201, WinName = "和值16", DefaultMoney = 40 * 100, DefaultMoneyNoWithTax = 40 * 100 };
            wt.Add(model13);
            WinType model14 = new WinType() { WinCode = 1020117, PlayCode = 10201, WinName = "和值17", DefaultMoney = 80 * 100, DefaultMoneyNoWithTax = 80 * 100 };
            wt.Add(model14);
            WinType model15 = new WinType() { WinCode = 1020201, PlayCode = 10202, WinName = "三同号通选", DefaultMoney = 40 * 100, DefaultMoneyNoWithTax = 40 * 100 };
            wt.Add(model15);
            WinType model16 = new WinType() { WinCode = 1020301, PlayCode = 10203, WinName = "三同号单选", DefaultMoney = 240 * 100, DefaultMoneyNoWithTax = 240 * 100 };
            wt.Add(model16);
            WinType model17 = new WinType() { WinCode = 1020401, PlayCode = 10204, WinName = "三不同号单选", DefaultMoney = 40 * 100, DefaultMoneyNoWithTax = 40 * 100 };
            wt.Add(model17);
            WinType model18 = new WinType() { WinCode = 1020501, PlayCode = 10205, WinName = "三连号通选", DefaultMoney = 10 * 100, DefaultMoneyNoWithTax = 10 * 100 };
            wt.Add(model18);
            WinType model19 = new WinType() { WinCode = 1020601, PlayCode = 10206, WinName = "二同号复选", DefaultMoney = 15 * 100, DefaultMoneyNoWithTax = 15 * 100 };
            wt.Add(model19);
            WinType model20 = new WinType() { WinCode = 1020701, PlayCode = 10207, WinName = "二同号单选", DefaultMoney = 80 * 100, DefaultMoneyNoWithTax = 80 * 100 };
            wt.Add(model20);
            WinType model21 = new WinType() { WinCode = 1020801, PlayCode = 10208, WinName = "二不同号复选", DefaultMoney = 8 * 100, DefaultMoneyNoWithTax = 8 * 100 };
            wt.Add(model21);
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
            new SchemesBLL().IsuseStopRevokeSchemes(LotteryCode);
        }

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
                    ent = GetValue4();
                    if (ent != null)
                    {
                        #region 新的开奖方式
                        string ReturnDescription = "";
                        bool res = bllis.EnteringDrawResults(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, DateTime.Now, ref ReturnDescription);
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
                        //接口
                        string[] Url = { "http://www.woying.com/Static/Flash/jxk3/jxk3.xml",
                                     "http://www.woying.com/kaijiang/jxk3ls/100.html",
                                     "http://www.jxfczx.cn/report/K3_WinMessage.aspx"
                                   };
                        ent = GetValue1();
                        if (ent == null)
                            ent = GetValue2(Url[0]);
                        if (ent == null)
                            ent = GetValue3(Url[1]);
                        if (ent == null)
                            ent = GetValue4(Url[2]);

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

            //接口数据是每 600秒开奖，有的期号不定期间隔时间为594秒、593秒，通过脚本维护，每2:00分执行一次,每一期重新更新期号时间
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

        private LotteryResult GetValue2(string Url)
        {
            try
            {
                string htmlContent = Utils.HttpGet(Url);
                LotteryResult ent = new LotteryResult();
                if (htmlContent == "")
                    return null;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                HtmlNode XK3_woying = doc.DocumentNode.SelectSingleNode("info");

                ent.LotteryCode = LotteryCode;
                string period = XK3_woying.SelectSingleNode("currentdraw").GetAttributeValue("openno", "");
                string pro = DateTime.Now.ToString().Substring(0, 2);
                ent.IsuseName = pro + period;
                DateTime dttime = DateTime.Now; //DateTime.ParseExact(XK3_woying.GetAttributeValue("", "").Replace("\n", "").Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                ent.StartTime = dttime.AddMinutes(-10);
                ent.EndTime = dttime;
                ent.LotteryTime = dttime;

                ent.LotteryWinNum = string.Join(" ", XK3_woying.SelectSingleNode("currentdraw").GetAttributeValue("result", "").ToCharArray());
                return ent;
            }
            catch
            {
                return null;
            }
        }

        private LotteryResult GetValue3(string Url)
        {
            try
            {
                string htmlContent = Utils.HttpGet(Url).Replace("\r\n", "").Replace("                        ", "").Replace("    ", "");
                LotteryResult ent = new LotteryResult();
                if (htmlContent == "")
                    return null;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                HtmlNodeCollection XK3_woying = doc.DocumentNode.SelectNodes("//div[@id='box']//table[1]/tbody[1]/tr[2]/td[1]/table[1]/tbody[1]/tr");
                var lstR = XK3_woying.Reverse().ToList();

                ent.LotteryCode = LotteryCode;
                string period = lstR[lstR.Count - 2].ChildNodes[0].InnerText;
                string pro = DateTime.Now.ToString().Substring(0, 2);
                ent.IsuseName = pro + period;
                DateTime dttime = DateTime.ParseExact(lstR[lstR.Count - 2].ChildNodes[1].InnerText, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                ent.StartTime = dttime.AddMinutes(-1);
                ent.EndTime = dttime;
                ent.LotteryTime = dttime;
                ent.LotteryWinNum = string.Join(" ", lstR[lstR.Count - 2].ChildNodes[2].InnerText.ToCharArray());

                return ent;
            }
            catch
            {
                return null;
            }
        }

        private LotteryResult GetValue4(string Url)
        {
            try
            {
                string htmlContent = Utils.HttpGet(Url).Replace("\r\n", "").Replace("                        ", "").Replace("	", "").Replace("    ", "");
                LotteryResult ent = new LotteryResult();
                if (htmlContent == "")
                    return null;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                HtmlNodeCollection XK3_woying = doc.DocumentNode.SelectNodes("//tr[@id='termCode2']");
                var lstR = XK3_woying.Reverse().ToList();

                ent.LotteryCode = LotteryCode;
                string period = lstR[lstR.Count - 1].ChildNodes[0].InnerText;
                string pro = DateTime.Now.ToString().Substring(0, 2);
                ent.IsuseName = pro + period;
                DateTime dttime = DateTime.Now; 
                ent.StartTime = dttime.AddMinutes(-1);
                ent.EndTime = dttime;
                ent.LotteryTime = dttime;
                ent.LotteryWinNum = string.Join(" ", lstR[lstR.Count - 1].ChildNodes[2].InnerText.ToCharArray());

                return ent;
            }
            catch
            {
                return null;
            }
        }
        private LotteryResult GetValue4()
        {
            try
            {
                LotteryResult ent = new LotteryResult();
                Random rand = new Random();
                string getData = HttpProxy.HttpGetProxy("http://www.jxfczx.cn/report/K3_WinMessage.aspx" + "?_A=" + rand.Next());
                if (string.IsNullOrEmpty(getData))
                    return null;
                string sData = Regex.Replace(getData, ">\\s+<", "><");
                string sRegxCode = "<tr id=\"termCode2\"><td.*>\\s*(\\d*)\\s*</td><td.*k3_0>(\\d+)</b><b id=k3_1>(\\d+)</b><b id=k3_2>(\\d+)</b>";
                Match mc = Regex.Match(sData, sRegxCode);
                if (mc.Success)
                {
                    int[] OpenNumber = { Convert.ToInt32(mc.Groups[2].Value), Convert.ToInt32(mc.Groups[3].Value), Convert.ToInt32(mc.Groups[4].Value) };
                    OpenNumber = OpenNumber.OrderBy(o => o).ToArray();
                    string IsuseName = mc.Groups[1].Value;
                    ent.IsuseName = IsuseName.StartsWith("20") == true ? IsuseName : "20" + mc.Groups[1].Value;
                    ent.LotteryWinNum = string.Format("{0}", string.Join(" ", OpenNumber));
                    ent.LotteryCode = LotteryCode;
                    ent.LotteryTime = DateTime.Now;
                }
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
                GetSuppNum2();  //华阳接口获取开奖数据
            }
            catch (Exception ex)
            {
                log.Write("爬取遗漏号码失败,错误日志:" + ex.Message, true);
            }
        }

        private void GetSuppNum1(string Url)
        {
            List<IsusesEntity> list = bllis.QueryNumEmpty(LotteryCode);
            if (list == null || list.Count <= 0)
                return;

            foreach (IsusesEntity item in list)
            {
                try
                {
                    Thread.Sleep(3 * 1000);
                    string IsuseName = item.IsuseName;
                    IsuseName = IsuseName.Substring(2, IsuseName.Length - 2);

                    LotteryResult ent = new LotteryResult();
                    string htmlContent = Utils.HttpGet(Url).Replace("\r\n", "").Replace("                        ", "").Replace("    ", "");
                    if (htmlContent == "")
                        continue;

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    HtmlNode XK3_woying = doc.DocumentNode.SelectSingleNode("//div[@id='box']//td[text()='" + IsuseName + "']/..");

                    ent.LotteryCode = LotteryCode;
                    string period = XK3_woying.SelectSingleNode("./td[1]").InnerText.Replace("\n", "").Trim();
                    string pro = DateTime.Now.ToString().Substring(0, 2);
                    ent.IsuseName = pro + period;

                    DateTime dttime = DateTime.ParseExact(XK3_woying.SelectSingleNode("./td[2]").InnerText.Replace("\n", "").Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    ent.StartTime = dttime.AddMinutes(-1);
                    ent.EndTime = dttime.AddMinutes(-1);
                    ent.LotteryTime = dttime;
                    ent.LotteryWinNum = string.Join(" ", XK3_woying.SelectSingleNode("./td[3]").InnerText.Replace("\r\n", "").Replace(" ", "").Trim().ToCharArray());

                    string ReturnDescription = "";
                    bllis.AddLotteryOpenNumber(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.StartTime, ent.EndTime, ent.LotteryTime, ref ReturnDescription);
                }
                catch { break; }
            }
        }

        private void GetSuppNum2()
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
                    string IsuseName = item.IsuseName.Substring(2, item.IsuseName.Length - 2);
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
        /// 算奖
        /// </summary>
        /// <param name="model">电子票实体</param>
        /// <param name="OpenNumber">开奖号码</param>
        /// <param name="WinMoneyNoWithTax">税后奖金</param>
        /// <returns></returns>
        public override void ComputeWin(XmlNode xml)
        {
            if (wt.Count <= 0) return;
            try
            {
                List<udv_ComputeTicket> Sql = new List<udv_ComputeTicket>();
                List<long> ListWinSID = new List<long>();   //中奖记录方案号
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

                    #region 计算中奖
                    switch (item.PlayCode)
                    {
                        case 10201: //和值
                            WinMoney = ComputeWin_HZ(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            break;
                        case 10202: //三同号通选
                            WinMoney = ComputeWin_STTX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020201;
                            break;
                        case 10203: //三同号单选
                            WinMoney = ComputeWin_STDX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020301;
                            break;
                        case 10204: //三不同号
                            WinMoney = ComputeWin_SBTH(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020401;
                            break;
                        case 10205: //三连号通选
                            WinMoney = ComputeWin_SLHTX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020501;
                            break;
                        case 10206: //二同号复选
                            WinMoney = ComputeWin_ETHFX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020601;
                            break;
                        case 10207: //二同号单选
                            WinMoney = ComputeWin_ETHDX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020701;
                            break;
                        case 10208: //二不同号
                            WinMoney = ComputeWin_EDTH(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020801;
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
                List<long> ListWinSID = new List<long>();   //中奖记录方案号
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

                    #region 计算中奖
                    switch (item.PlayCode)
                    {
                        case 10201: //和值
                            WinMoney = ComputeWin_HZ(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            break;
                        case 10202: //三同号通选
                            WinMoney = ComputeWin_STTX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020201;
                            break;
                        case 10203: //三同号单选
                            WinMoney = ComputeWin_STDX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020301;
                            break;
                        case 10204: //三不同号
                            WinMoney = ComputeWin_SBTH(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020401;
                            break;
                        case 10205: //三连号通选
                            WinMoney = ComputeWin_SLHTX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020501;
                            break;
                        case 10206: //二同号复选
                            WinMoney = ComputeWin_ETHFX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020601;
                            break;
                        case 10207: //二同号单选
                            WinMoney = ComputeWin_ETHDX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020701;
                            break;
                        case 10208: //二不同号
                            WinMoney = ComputeWin_EDTH(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1020801;
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
                //处理中奖记录
                bllwin.SchemesDetailsWinByChaseTasks(Sql, ListWinSID, NewListNoWinSID);
                blls.Redis_Compute(Sql);
            }
            catch (Exception ex)
            {
                log.Write("自动算奖失败(追号)：" + ex.Message, true);
            }
        }
        public override long ComputeWin(udv_ComputeTicket model, ref string Description, ref long WinMoneyNoWithTax)
        {
            long WinMoney = 0;
            int WinCode = 0;
            if (model == null)
                return 0;
            if (wt.Count <= 0)
                return 0;
            switch (model.PlayCode)
            {
                case 10201: //和值
                    WinMoney = ComputeWin_HZ(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                    break;
                case 10202: //三同号通选
                    WinMoney = ComputeWin_STTX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 10203: //三同号单选
                    WinMoney = ComputeWin_STDX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 10204: //三不同号
                    WinMoney = ComputeWin_SBTH(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 10205: //三连号通选
                    WinMoney = ComputeWin_SLHTX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 10206: //二同号复选
                    WinMoney = ComputeWin_ETHFX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 10207: //二同号单选
                    WinMoney = ComputeWin_ETHDX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 10208: //二不同号
                    WinMoney = ComputeWin_EDTH(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
            }
            return WinMoney;
        }

        #region 奖等算奖
        //和值
        public long ComputeWin_HZ(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            //4^5^6^ .....15^16^17
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 3) return 0;

            string[] Numbers = SplitLotteryNumber(Number, '^');
            if (Numbers == null) return 0;
            if (Numbers.Length < 1) return 0;

            int WinNumberSum = Convert.ToInt32(OpenNum[0]) + Convert.ToInt32(OpenNum[1]) + Convert.ToInt32(OpenNum[2]);
            if (WinNumberSum < 3 || WinNumberSum > 18)
                return 0;

            int Description1 = 0;
            long WinMoney = 0;
            for (int i = 0; i < Numbers.Length; i++)
            {
                if (Numbers[i].Length < 1) continue;
                if (Convert.ToInt32(Numbers[i]) == WinNumberSum)
                {
                    int Num = 4;
                    int WinTypeCode = 1020104;
                    for (int j = 4; j < 18; j++)
                    {
                        if (WinNumberSum == Num)
                        {
                            WinType model = wt.Find(p => p.WinCode == WinTypeCode && p.PlayCode == 10201);
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                            WinCode = WinTypeCode;  //注意这里的和值是一注已票的
                            break;
                        }
                        Num++;
                        WinTypeCode++;
                    }
                }
            }
            if (Description1 > 0)
                Description = "和值" + Description1 + "注。";
            return WinMoney;
        }
        //三同号通选
        public long ComputeWin_STTX(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //1*2*3*4*5*6^1*2*3*4*5*6
            int[] OpenNum = OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
            if (OpenNum.Length < 3) return 0;
            if (OpenNum[0] == OpenNum[1] && OpenNum[1] == OpenNum[2])
            {
                int Description1 = 0;
                long WinMoney = 0;
                string[] Numbers = SplitLotteryNumber(Number, '^');
                if (Numbers == null) return 0;
                if (Numbers.Length < 1) return 0;
                for (int i = 0; i < Numbers.Length; i++)
                {
                    WinType model = wt.Find(p => p.WinCode == 1020201 && p.PlayCode == 10202);
                    Description1++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
                if (Description1 > 0)
                    Description = "三同号通选" + Description1 + "注。";
                return WinMoney;
            }
            return 0;
        }
        //三同号单选
        public long ComputeWin_STDX(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //1*1*1^2*2*2^3*3*3^4*4*4^5*5*5^6*6*6
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 3) return 0;
            if (OpenNum[0] != OpenNum[1] || OpenNum[0] != OpenNum[2] || OpenNum[1] != OpenNum[2]) return 0;

            string[] Numbers = SplitLotteryNumber(Number, '^');
            if (Numbers == null) return 0;
            if (Numbers.Length < 1) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            string WinNumber = string.Join("*", OpenNum);
            for (int i = 0; i < Numbers.Length; i++)
            {
                if (Numbers[i].Length < 5) continue;
                if (Numbers[i] == WinNumber)
                {
                    WinType model = wt.Find(p => p.WinCode == 1020301 && p.PlayCode == 10203);
                    Description1++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
            }
            if (Description1 > 0)
                Description = "三同号单选" + Description1 + "注。";
            return WinMoney;
        }
        //三不同号
        public long ComputeWin_SBTH(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //1*2*3^1*2*4^1*2*5^1*2*6^1*3*4
            //1*3*5^1*3*6^1*4*5^1*4*6^1*5*6
            //2*3*4^2*3*5^2*3*6^2*4*5^2*4*6
            //2*5*6^3*4*5^3*4*6^3*5*6^4*5*6
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 3) return 0;

            string[] Numbers = SplitLotteryNumber(Number, '^');
            if (Numbers == null) return 0;
            if (Numbers.Length < 1) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            string WinNumber = string.Join("", OpenNum);
            string sortStr = "";
            for (int i = 0; i < Numbers.Length; i++)
            {
                sortStr = Numbers[i].Replace("*", "");
                if (sortStr.Length < 3) continue;
                if (Sort(sortStr) == Sort(WinNumber))
                {
                    WinType model = wt.Find(p => p.WinCode == 1020401 && p.PlayCode == 10204);
                    Description1++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
            }
            if (Description1 > 0)
                Description = "三不同号" + Description1 + "注。";
            return WinMoney;
        }
        //三连号通选
        public long ComputeWin_SLHTX(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //1*2*3*4*5*6^1*2*3*4*5*6   --123,234,345,456
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 3) return 0;
            int firstnum = Convert.ToInt32(OpenNum[0]);
            int secondnum = Convert.ToInt32(OpenNum[1]);
            int thirdnum = Convert.ToInt32(OpenNum[2]);
            if (!((firstnum + 1) == secondnum && (secondnum + 1) == thirdnum)) return 0;

            string[] Numbers = SplitLotteryNumber(Number, '^');
            if (Numbers == null) return 0;
            if (Numbers.Length < 1) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            for (int i = 0; i < Numbers.Length; i++)
            {
                WinType model = wt.Find(p => p.WinCode == 1020501 && p.PlayCode == 10205);
                Description1++;
                WinMoney += model.DefaultMoney;
                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
            }
            if (Description1 > 0)
                Description = "三连号通选" + Description1 + "注。";
            return WinMoney;
        }
        //二同号复选
        public long ComputeWin_ETHFX(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //1*1^2*2^3*3^4*4^5*5^6*6
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 3) return 0;
            if (OpenNum[0] == OpenNum[1] && OpenNum[0] == OpenNum[2] && OpenNum[1] == OpenNum[2])
                return 0;

            string WinNumber = "";
            if (OpenNum[0] == OpenNum[1])
            {
                WinNumber = OpenNum[0] + "*" + OpenNum[1];
            }
            else if (OpenNum[0] == OpenNum[2])
            {
                WinNumber = OpenNum[0] + "*" + OpenNum[2];
            }
            else if (OpenNum[1] == OpenNum[2])
            {
                WinNumber = OpenNum[1] + "*" + OpenNum[2];
            }
            else return 0;

            string[] Numbers = SplitLotteryNumber(Number, '^');
            if (Numbers == null) return 0;
            if (Numbers.Length < 1) return 0;

            int Description1 = 0;
            long WinMoney = 0;

            for (int i = 0; i < Numbers.Length; i++)
            {
                if (Numbers[i].Length < 3) continue;
                if (Numbers[i] == WinNumber)
                {
                    WinType model = wt.Find(p => p.WinCode == 1020601 && p.PlayCode == 10206);
                    Description1++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
            }
            if (Description1 > 0)
                Description = "二同号复选" + Description1 + "注。";
            return WinMoney;
        }
        //二同号单选
        public long ComputeWin_ETHDX(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //1*1*3^1*1*4^1*1*5^1*1*6^2*2*3^2*2*4^2*2*5^2*2*6
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 3) return 0;

            string[] Numbers = SplitLotteryNumber(Number, '^');
            if (Numbers == null) return 0;
            if (Numbers.Length < 1) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            string WinNumber = Sort(String.Join("", OpenNum));
            string sortStr = "";
            for (int i = 0; i < Numbers.Length; i++)
            {
                sortStr = Sort(Numbers[i].Replace("*", ""));
                if (sortStr.Length < 3) continue;
                if (sortStr == WinNumber)
                {
                    WinType model = wt.Find(p => p.WinCode == 1020701 && p.PlayCode == 10207);
                    Description1++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
            }
            if (Description1 > 0)
                Description = "二同号单选" + Description1 + "注。";
            return WinMoney;
        }
        //二不同号
        public long ComputeWin_EDTH(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //1*2^1*3^1*4^1*5^1*6^2*3^2*4^2*5^2*6^3*4^3*5^3*6^4*5^4*6^5*6
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 3) return 0;

            string[] Numbers = SplitLotteryNumber(Number, '^');
            if (Numbers == null) return 0;
            if (Numbers.Length < 1) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            for (int i = 0; i < Numbers.Length; i++)
            {
                string[] sortStr = Numbers[i].Split('*');
                if (sortStr.Length < 2) continue;
                if (OpenNum.Contains(sortStr[0]) && OpenNum.Contains(sortStr[1]))
                {
                    WinType model = wt.Find(p => p.WinCode == 1020801 && p.PlayCode == 10208);
                    Description1++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
            }
            if (Description1 > 0)
                Description = "二不同号" + Description1 + "注。";
            return WinMoney;
        }
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
