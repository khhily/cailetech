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
    public class JLK3 : LotteryBase
    {
        private const int LotteryCode = 101;
        private readonly Log log = new Log("LotteryTickets.JLK3");
        private XmlNode xml = null;

        private IsusesBLL bllis = new IsusesBLL();
        private SchemesBLL blls = new SchemesBLL();
        private SchemeETicketsBLL bllet = new SchemeETicketsBLL();
        private SchemesWinBLL bllwin = new SchemesWinBLL();

        public JLK3()
        {
            GetWinType();
        }
        public JLK3(XmlNode _xml)
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
            WinType model1 = new WinType() { WinCode = 1010104, PlayCode = 10101, WinName = "和值4", DefaultMoney = 80 * 100, DefaultMoneyNoWithTax = 80 * 100 };
            wt.Add(model1);
            WinType model2 = new WinType() { WinCode = 1010105, PlayCode = 10101, WinName = "和值5", DefaultMoney = 40 * 100, DefaultMoneyNoWithTax = 40 * 100 };
            wt.Add(model2);
            WinType model3 = new WinType() { WinCode = 1010106, PlayCode = 10101, WinName = "和值6", DefaultMoney = 25 * 100, DefaultMoneyNoWithTax = 25 * 100 };
            wt.Add(model3);
            WinType model4 = new WinType() { WinCode = 1010107, PlayCode = 10101, WinName = "和值7", DefaultMoney = 16 * 100, DefaultMoneyNoWithTax = 16 * 100 };
            wt.Add(model4);
            WinType model5 = new WinType() { WinCode = 1010108, PlayCode = 10101, WinName = "和值8", DefaultMoney = 12 * 100, DefaultMoneyNoWithTax = 12 * 100 };
            wt.Add(model5);
            WinType model6 = new WinType() { WinCode = 1010109, PlayCode = 10101, WinName = "和值9", DefaultMoney = 10 * 100, DefaultMoneyNoWithTax = 10 * 100 };
            wt.Add(model6);
            WinType model7 = new WinType() { WinCode = 1010110, PlayCode = 10101, WinName = "和值10", DefaultMoney = 9 * 100, DefaultMoneyNoWithTax = 9 * 100 };
            wt.Add(model7);
            WinType model8 = new WinType() { WinCode = 1010111, PlayCode = 10101, WinName = "和值11", DefaultMoney = 9 * 100, DefaultMoneyNoWithTax = 9 * 100 };
            wt.Add(model8);
            WinType model9 = new WinType() { WinCode = 1010112, PlayCode = 10101, WinName = "和值12", DefaultMoney = 10 * 100, DefaultMoneyNoWithTax = 10 * 100 };
            wt.Add(model9);
            WinType model10 = new WinType() { WinCode = 1010113, PlayCode = 10101, WinName = "和值13", DefaultMoney = 12 * 100, DefaultMoneyNoWithTax = 12 * 100 };
            wt.Add(model10);
            WinType model11 = new WinType() { WinCode = 1010114, PlayCode = 10101, WinName = "和值14", DefaultMoney = 16 * 100, DefaultMoneyNoWithTax = 16 * 100 };
            wt.Add(model11);
            WinType model12 = new WinType() { WinCode = 1010115, PlayCode = 10101, WinName = "和值15", DefaultMoney = 25 * 100, DefaultMoneyNoWithTax = 25 * 100 };
            wt.Add(model12);
            WinType model13 = new WinType() { WinCode = 1010116, PlayCode = 10101, WinName = "和值16", DefaultMoney = 40 * 100, DefaultMoneyNoWithTax = 40 * 100 };
            wt.Add(model13);
            WinType model14 = new WinType() { WinCode = 1010117, PlayCode = 10101, WinName = "和值17", DefaultMoney = 80 * 100, DefaultMoneyNoWithTax = 80 * 100 };
            wt.Add(model14);
            WinType model15 = new WinType() { WinCode = 1010201, PlayCode = 10102, WinName = "三同号通选", DefaultMoney = 40 * 100, DefaultMoneyNoWithTax = 40 * 100 };
            wt.Add(model15);
            WinType model16 = new WinType() { WinCode = 1010301, PlayCode = 10103, WinName = "三同号单选", DefaultMoney = 240 * 100, DefaultMoneyNoWithTax = 240 * 100 };
            wt.Add(model16);
            WinType model17 = new WinType() { WinCode = 1010401, PlayCode = 10104, WinName = "三不同号单选", DefaultMoney = 40 * 100, DefaultMoneyNoWithTax = 40 * 100 };
            wt.Add(model17);
            WinType model18 = new WinType() { WinCode = 1010501, PlayCode = 10105, WinName = "三连号通选", DefaultMoney = 10 * 100, DefaultMoneyNoWithTax = 10 * 100 };
            wt.Add(model18);
            WinType model19 = new WinType() { WinCode = 1010601, PlayCode = 10106, WinName = "二同号复选", DefaultMoney = 15 * 100, DefaultMoneyNoWithTax = 15 * 100 };
            wt.Add(model19);
            WinType model20 = new WinType() { WinCode = 1010701, PlayCode = 10107, WinName = "二同号单选", DefaultMoney = 80 * 100, DefaultMoneyNoWithTax = 80 * 100 };
            wt.Add(model20);
            WinType model21 = new WinType() { WinCode = 1010801, PlayCode = 10108, WinName = "二不同号复选", DefaultMoney = 8 * 100, DefaultMoneyNoWithTax = 8 * 100 };
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
            blls.IsuseStopRevokeSchemes(LotteryCode);
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
                    //接口
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
                        string[] Url = { "http://www.lecai.com/lottery/ajax_latestdrawn.php?lottery_type=560",
                                      "http://baidu.lecai.com/lottery/draw/view/560",
                                      "http://www.woying.com/kaijiang/jxk3ls/100.html"
                                   };
                        ent = GetValue3();
                        if (ent == null)
                            ent = GetValue2(Url[1]);
                        if (ent == null)
                            ent = GetValue1(Url[0]);
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
        private LotteryResult GetValue1(string Url)
        {
            try
            {
                string htmlContent = HttpProxy.HttpGetProxy(Url);
                LotteryResult ent = new LotteryResult();
                if (htmlContent == "")
                    return null;

                RootData JsonData = JsonHelper.JSONToObject<RootData>(htmlContent);
                ent.LotteryCode = LotteryCode;
                string pro = DateTime.Now.ToString().Substring(0, 2);
                ent.IsuseName = pro + JsonData.data[0].phase;
                ent.StartTime = DateTime.Parse(JsonData.data[0].time_startsale);
                ent.EndTime = DateTime.Parse(JsonData.data[0].time_endsale);
                ent.LotteryTime = DateTime.Parse(JsonData.data[0].time_draw);
                ent.LotteryWinNum = String.Join(" ", JsonData.data[0].result.result[0].data);
                return ent;
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
                string htmlContent = HttpProxy.HttpGetProxy(Url);
                LotteryResult ent = new LotteryResult();
                if (htmlContent == "")
                    return null;

                int iindex = htmlContent.IndexOf("var latest_draw_result");
                int iend = htmlContent.IndexOf("var phaseData");
                string[] strArr = htmlContent.Substring(iindex, iend - iindex).Replace("var", "").Replace("latest_draw_result", "").Replace("latest_draw_phase", "").Replace("latest_draw_time", "").Replace("=", "").Replace("\n", "").Split(';');
                ent.LotteryCode = LotteryCode;
                string pro = DateTime.Now.ToString().Substring(0, 2);
                ent.IsuseName = pro + strArr[1].Replace("'", "").Trim();
                ent.StartTime = DateTime.Parse(strArr[2].Replace("'", "").Trim()).AddMinutes(-1);
                ent.EndTime = DateTime.Parse(strArr[2].Replace("'", "").Trim()).AddMinutes(-1);
                ent.LotteryTime = DateTime.Parse(strArr[2].Replace("'", "").Trim());
                strArr[0] = strArr[0].Replace("\n", "").Trim();
                
                int i1 = strArr[0].IndexOf("red");
                int i2 = strArr[0].IndexOf("blue");
                string Num = strArr[0].Substring(i1, i2 - i1).Replace("red", "").Replace(":", "").Replace("[", "").Replace("]", "").Replace("\"", "").Replace(",", " ").Trim();
                ent.LotteryWinNum = Num;

                return ent;
            }
            catch
            {
                return null;
            }
        }
        private LotteryResult GetValue3()
        {
            InterfaceBase InterTicker = new InterfaceBase()[xml];
            if (InterTicker == null) return null;

            //接口数据是每 546秒 开奖 不能做定时获取，临时处理方法每2:00分执行一次
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

        private LotteryResult GetValue4()
        {
            try
            {
                LotteryResult ent = new LotteryResult();
                Random rand = new Random();
                //吉林快3 官方获取
                string getData = HttpProxy.HttpGetProxy("http://www.jlfc.com.cn/View/KS_WinInfo.aspx" + "?_A=" + rand.Next());
                if (string.IsNullOrEmpty(getData))
                    return null;
                string sData = Regex.Replace(getData, ">\\s+<", "><");

                string sRegxCode = "<td class=\"td_play.*>\\s*(\\d*)\\s*</td><.*red_ball'>(\\d+)<.*red_ball'>(\\d+)<.*red_ball'>(\\d+)</li></ul>";

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
                GetSuppNum4();
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

        private void GetSuppNum2(string Url)
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
                    string htmlContent = Utils.HttpGet(Url).Replace("\r\n", "").Replace("                        ", "").Replace("    ", "").Replace("\r", "").Replace("\n", "");
                    if (htmlContent == "")
                        continue;

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    HtmlNode XK3_woying = doc.DocumentNode.SelectSingleNode("//table[@id='draw_list']//td[text()='" + IsuseName + "']/..");
                    ent.LotteryCode = LotteryCode;
                    string period = XK3_woying.SelectSingleNode("./td[2]").InnerText.Replace("\n", "").Trim();
                    string pro = DateTime.Now.ToString().Substring(0, 2);
                    ent.IsuseName = pro + period;
                    
                    ent.StartTime = Convert.ToDateTime(item.StartTime);
                    ent.EndTime = Convert.ToDateTime(item.EndTime);
                    ent.LotteryTime = Convert.ToDateTime(item.EndTime);
                    
                    ent.LotteryWinNum = string.Join(" ", XK3_woying.SelectSingleNode("./td[3]").InnerText.Replace("\r\n", "").Replace(" ", "").Trim().ToCharArray());

                    string ReturnDescription = "";
                    bllis.AddLotteryOpenNumber(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.StartTime, ent.EndTime, ent.LotteryTime, ref ReturnDescription);
                }
                catch { break; }
            }
        }

        private void GetSuppNum3()
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

        private void GetSuppNum4()
        {
            try
            {
                #region 组装地址 两天历史查询 昨天 今天
                DateTime dt = DateTime.Now;
                string Url = string.Empty;
                HtmlDocument doc = null;
                List<IsusesEntity> list = null;
                for (int i = 0; i < 2; i++)
                {
                    if (i != 0)
                        dt = dt.AddDays(-i);
                    list = bllis.QueryNumEmpty(LotteryCode, dt.ToString("yyyy-MM-dd"));
                    if (list == null || list.Count <= 0)
                        return;

                    Url = string.Format("{0}kjh.55128.cn/jlk3-kjjg-{1}.htm", "http://", dt.ToString("yyyy-MM-dd"));
                    string htmlContent = HttpProxy.HttpGetProxy(Url);
                    doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);
                    if (htmlContent == "")
                        continue;



                }
                #endregion
            }
            catch
            {
                return;
            }
        }
        #endregion

        #region JSON结构体
        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string key { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> data { get; set; }
        }

        public class Result
        {
            /// <summary>
            /// 
            /// </summary>
            public List<ResultItem> result { get; set; }
        }

        public class ResultDetailItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string key { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string bet { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int prize { get; set; }
        }

        public class Result_detail
        {
            /// <summary>
            /// 
            /// </summary>
            public List<ResultDetailItem> resultDetail { get; set; }
        }

        public class DataItem
        {
            /// <summary>
            /// 
            /// </summary>
            public int phasetype { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string phase { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string create_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time_startsale { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time_endsale { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time_endticket { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time_draw { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int forsale { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int is_current { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Result result { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Result_detail result_detail { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string pool_amount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sale_amount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ext { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string fc3d_sjh { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int terminal_status { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int fordraw { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time_startsale_fixed { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time_endsale_fixed { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time_endsale_syndicate_fixed { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time_endsale_upload_fixed { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time_draw_fixed { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int time_startsale_correction { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int time_endsale_correction { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int time_endsale_syndicate_correction { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int time_endsale_upload_correction { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int time_draw_correction { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time_exchange { get; set; }
        }

        public class RootData
        {
            /// <summary>
            /// 
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string message { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<DataItem> data { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string redirect { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string datetime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int timestamp { get; set; }
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

                    #region 计算中奖
                    switch (item.PlayCode)
                    {
                        case 10101: //和值
                            WinMoney = ComputeWin_HZ(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            break;
                        case 10102: //三同号通选
                            WinMoney = ComputeWin_STTX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010201;
                            break;
                        case 10103: //三同号单选
                            WinMoney = ComputeWin_STDX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010301;
                            break;
                        case 10104: //三不同号
                            WinMoney = ComputeWin_SBTH(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010401;
                            break;
                        case 10105: //三连号通选
                            WinMoney = ComputeWin_SLHTX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010501;
                            break;
                        case 10106: //二同号复选
                            WinMoney = ComputeWin_ETHFX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010601;
                            break;
                        case 10107: //二同号单选
                            WinMoney = ComputeWin_ETHDX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010701;
                            break;
                        case 10108: //二不同号
                            WinMoney = ComputeWin_EDTH(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010801;
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

                    #region 计算中奖
                    switch (item.PlayCode)
                    {
                        case 10101: //和值
                            WinMoney = ComputeWin_HZ(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            break;
                        case 10102: //三同号通选
                            WinMoney = ComputeWin_STTX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010201;
                            break;
                        case 10103: //三同号单选
                            WinMoney = ComputeWin_STDX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010301;
                            break;
                        case 10104: //三不同号
                            WinMoney = ComputeWin_SBTH(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010401;
                            break;
                        case 10105: //三连号通选
                            WinMoney = ComputeWin_SLHTX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010501;
                            break;
                        case 10106: //二同号复选
                            WinMoney = ComputeWin_ETHFX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010601;
                            break;
                        case 10107: //二同号单选
                            WinMoney = ComputeWin_ETHDX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010701;
                            break;
                        case 10108: //二不同号
                            WinMoney = ComputeWin_EDTH(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 1010801;
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
                case 10101: //和值
                    WinMoney = ComputeWin_HZ(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                    break;
                case 10102: //三同号通选
                    WinMoney = ComputeWin_STTX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 10103: //三同号单选
                    WinMoney = ComputeWin_STDX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 10104: //三不同号
                    WinMoney = ComputeWin_SBTH(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 10105: //三连号通选
                    WinMoney = ComputeWin_SLHTX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 10106: //二同号复选
                    WinMoney = ComputeWin_ETHFX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 10107: //二同号单选
                    WinMoney = ComputeWin_ETHDX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 10108: //二不同号
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
                    int WinTypeCode = 1010104;
                    for (int j = 4; j < 18; j++)
                    {
                        if (WinNumberSum == Num)
                        {
                            WinType model = wt.Find(p => p.WinCode == WinTypeCode && p.PlayCode == 10101);
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
                    WinType model = wt.Find(p => p.WinCode == 1010201 && p.PlayCode == 10102);
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
                    WinType model = wt.Find(p => p.WinCode == 1010301 && p.PlayCode == 10103);
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
                    WinType model = wt.Find(p => p.WinCode == 1010401 && p.PlayCode == 10104);
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
                WinType model = wt.Find(p => p.WinCode == 1010501 && p.PlayCode == 10105);
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
                    WinType model = wt.Find(p => p.WinCode == 1010601 && p.PlayCode == 10106);
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
                    WinType model = wt.Find(p => p.WinCode == 1010701 && p.PlayCode == 10107);
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
                    WinType model = wt.Find(p => p.WinCode == 1010801 && p.PlayCode == 10108);
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
