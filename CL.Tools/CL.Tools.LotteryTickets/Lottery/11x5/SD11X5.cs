using CL.Enum.Common.Lottery;
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
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace CL.Tools.LotteryTickets.Lottery._11x5
{
    /// <summary>
    /// 山东十一运夺金
    /// 2017-5-9
    /// </summary>
    public class SD11X5 : LotteryBase
    {
        private const int LotteryCode = 202;
        private readonly Log log = new Log("LotteryTickets.SD11X5");
        private XmlNode xml = null;

        private IsusesBLL bllis = new IsusesBLL();
        private SchemesBLL blls = new SchemesBLL();
        private SchemeETicketsBLL bllet = new SchemeETicketsBLL();
        private SchemesWinBLL bllwin = new SchemesWinBLL();

        public SD11X5()
        {
            GetWinType();
        }
        public SD11X5(XmlNode _xml)
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
            WinType model1 = new WinType() { WinCode = 2020101, PlayCode = 20201, WinName = "任选二", DefaultMoney = 6 * 100, DefaultMoneyNoWithTax = 6 * 100 };
            wt.Add(model1);
            WinType model2 = new WinType() { WinCode = 2020201, PlayCode = 20202, WinName = "任选三", DefaultMoney = 19 * 100, DefaultMoneyNoWithTax = 19 * 100 };
            wt.Add(model2);
            WinType model3 = new WinType() { WinCode = 2020301, PlayCode = 20203, WinName = "任选四", DefaultMoney = 78 * 100, DefaultMoneyNoWithTax = 78 * 100 };
            wt.Add(model3);
            WinType model4 = new WinType() { WinCode = 2020401, PlayCode = 20204, WinName = "任选五", DefaultMoney = 540 * 100, DefaultMoneyNoWithTax = 540 * 100 };
            wt.Add(model4);
            WinType model5 = new WinType() { WinCode = 2020501, PlayCode = 20205, WinName = "任选六", DefaultMoney = 90 * 100, DefaultMoneyNoWithTax = 90 * 100 };
            wt.Add(model5);
            WinType model6 = new WinType() { WinCode = 2020601, PlayCode = 20206, WinName = "任选七", DefaultMoney = 26 * 100, DefaultMoneyNoWithTax = 26 * 100 };
            wt.Add(model6);
            WinType model7 = new WinType() { WinCode = 2020701, PlayCode = 20207, WinName = "任选八", DefaultMoney = 9 * 100, DefaultMoneyNoWithTax = 9 * 100 };
            wt.Add(model7);
            WinType model8 = new WinType() { WinCode = 2020801, PlayCode = 20208, WinName = "前一直选", DefaultMoney = 13 * 100, DefaultMoneyNoWithTax = 13 * 100 };
            wt.Add(model8);
            WinType model9 = new WinType() { WinCode = 2020901, PlayCode = 20209, WinName = "前二直选", DefaultMoney = 130 * 100, DefaultMoneyNoWithTax = 130 * 100 };
            wt.Add(model9);
            WinType model10 = new WinType() { WinCode = 2021001, PlayCode = 20210, WinName = "前二组选", DefaultMoney = 65 * 100, DefaultMoneyNoWithTax = 65 * 100 };
            wt.Add(model10);
            WinType model11 = new WinType() { WinCode = 2021101, PlayCode = 20211, WinName = "前三直选", DefaultMoney = 1170 * 100, DefaultMoneyNoWithTax = 1170 * 100 };
            wt.Add(model11);
            WinType model12 = new WinType() { WinCode = 2021201, PlayCode = 20212, WinName = "前三组选", DefaultMoney = 195 * 100, DefaultMoneyNoWithTax = 195 * 100 };
            wt.Add(model12);
            #region 新增乐选玩法 乐选345
            WinType model13 = new WinType() { WinCode = 2021301, PlayCode = 20213, WinName = "乐选3一等奖", DefaultMoney = 1384 * 100, DefaultMoneyNoWithTax = 1384 * 100 };
            wt.Add(model13);
            WinType model14 = new WinType() { WinCode = 2021302, PlayCode = 20213, WinName = "乐选3二等奖", DefaultMoney = 214 * 100, DefaultMoneyNoWithTax = 214 * 100 };
            wt.Add(model14);
            WinType model15 = new WinType() { WinCode = 2021303, PlayCode = 20213, WinName = "乐选3三等奖", DefaultMoney = 19 * 100, DefaultMoneyNoWithTax = 19 * 100 };
            wt.Add(model15);
            WinType model16 = new WinType() { WinCode = 2021401, PlayCode = 20214, WinName = "乐选4一等奖", DefaultMoney = 154 * 100, DefaultMoneyNoWithTax = 154 * 100 };
            wt.Add(model16);
            WinType model17 = new WinType() { WinCode = 2021402, PlayCode = 20214, WinName = "乐选4二等奖", DefaultMoney = 19 * 100, DefaultMoneyNoWithTax = 19 * 100 };
            wt.Add(model17);
            WinType model18 = new WinType() { WinCode = 2021501, PlayCode = 20215, WinName = "乐选5一等奖", DefaultMoney = 1080 * 100, DefaultMoneyNoWithTax = 1080 * 100 };
            wt.Add(model18);
            WinType model19 = new WinType() { WinCode = 2021502, PlayCode = 20215, WinName = "乐选5二等奖", DefaultMoney = 90 * 100, DefaultMoneyNoWithTax = 90 * 100 };
            wt.Add(model19);
            #endregion
            #endregion
        }

        #region 采集
        /// <summary>
        /// 维护当天的期号
        /// </summary>
        public override void ModifyIsuseInfo()
        {
            #region
#if DEBUG

#endif
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
                string htmlContent = HttpProxy.HttpGetProxy("http://sd11x5kjjg.icaile.com/");
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);
                if (htmlContent == "")
                    return null;
                HtmlNode collection = doc.DocumentNode.SelectSingleNode("//table[@class='tablelist']/tbody");
                string content = collection.ChildNodes[collection.ChildNodes.Count - 2].InnerText;
                string[] LotteryInfo = content.Trim('\n').Split('\n');
                if (LotteryInfo != null)
                {
                    LotteryResult ent = new LotteryResult();
                    ent.IsuseName = LotteryInfo[0].Trim().StartsWith("20") == true ? LotteryInfo[0].Trim() : "20" + LotteryInfo[0].Trim();
                    ent.LotteryCode = LotteryCode;
                    ent.LotteryTime = DateTime.Now;
                    ent.LotteryWinNum = string.Format("{0} {1} {2} {3} {4}", LotteryInfo[1].Trim(), LotteryInfo[2].Trim(), LotteryInfo[3].Trim(), LotteryInfo[4].Trim(), LotteryInfo[5].Trim());
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
                        case (int)SD11X5PlayCode.R2: //任选二
                            WinMoney = ComputeWin_RX2(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020101;
                            break;
                        case (int)SD11X5PlayCode.R3: //任选三
                            WinMoney = ComputeWin_RX3(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020201;
                            break;
                        case (int)SD11X5PlayCode.R4: //任选四
                            WinMoney = ComputeWin_RX4(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020301;
                            break;
                        case (int)SD11X5PlayCode.R5: //任选五
                            WinMoney = ComputeWin_RX5(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020401;
                            break;
                        case (int)SD11X5PlayCode.R6: //任选六
                            WinMoney = ComputeWin_RX6(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020501;
                            break;
                        case (int)SD11X5PlayCode.R7: //任选七
                            WinMoney = ComputeWin_RX7(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020601;
                            break;
                        case (int)SD11X5PlayCode.R8: //任选八
                            WinMoney = ComputeWin_RX8(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020701;
                            break;
                        case (int)SD11X5PlayCode.Q1: //前一
                            WinMoney = ComputeWin_Q1(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020801;
                            break;
                        case (int)SD11X5PlayCode.Q2_ZX: //前二直选
                            WinMoney = ComputeWin_Q2ZX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020901;
                            break;
                        case (int)SD11X5PlayCode.Q2_ZUX: //前二组选
                            WinMoney = ComputeWin_Q2ZUX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2021001;
                            break;
                        case (int)SD11X5PlayCode.Q3_ZX: //前三直选
                            WinMoney = ComputeWin_Q3ZX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2021101;
                            break;
                        case (int)SD11X5PlayCode.Q3_ZUX: //前三组选
                            WinMoney = ComputeWin_Q3ZUX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2021201;
                            break;
                        case (int)SD11X5PlayCode.LX3: //乐选3
                            WinMoney = ComputeWin_LX3(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            break;
                        case (int)SD11X5PlayCode.LX4: //乐选4
                            WinMoney = ComputeWin_LX4(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
                            break;
                        case (int)SD11X5PlayCode.LX5: //乐选5
                            WinMoney = ComputeWin_LX5(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax, ref WinCode);
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
                        case 20201: //任选二
                            WinMoney = ComputeWin_RX2(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020101;
                            break;
                        case 20202: //任选三
                            WinMoney = ComputeWin_RX3(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020201;
                            break;
                        case 20203: //任选四
                            WinMoney = ComputeWin_RX4(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020301;
                            break;
                        case 20204: //任选五
                            WinMoney = ComputeWin_RX5(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020401;
                            break;
                        case 20205: //任选六
                            WinMoney = ComputeWin_RX6(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020501;
                            break;
                        case 20206: //任选七
                            WinMoney = ComputeWin_RX7(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020601;
                            break;
                        case 20207: //任选八
                            WinMoney = ComputeWin_RX8(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020701;
                            break;
                        case 20208: //前一
                            WinMoney = ComputeWin_Q1(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020801;
                            break;
                        case 20209: //前二直选
                            WinMoney = ComputeWin_Q2ZX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2020901;
                            break;
                        case 20210: //前二组选
                            WinMoney = ComputeWin_Q2ZUX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2021001;
                            break;
                        case 20211: //前三直选
                            WinMoney = ComputeWin_Q3ZX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2021101;
                            break;
                        case 20212: //前三组选
                            WinMoney = ComputeWin_Q3ZUX(item.Number, item.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                            WinCode = 2021201;
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
                StringBuilder OperateParameter = new StringBuilder();
                OperateParameter.Append("{");
                OperateParameter.AppendFormat("\"LotteryCode\":{0},", LotteryCode);
                OperateParameter.AppendFormat("\"DateTime\":{0}", DateTime.Now);
                OperateParameter.Append("}");
                //new SystemLog().ErrorLog("LotteryTickets.HB11X5", "ComputeChaseTasksWin", "自动算奖失败(追号)", OperateParameter.ToString(), ex.Message, ex.StackTrace);
                log.Write("自动算奖失败(追号)：" + ex.Message, true);
            }
        }
        public override long ComputeWin(udv_ComputeTicket model, ref string Description, ref long WinMoneyNoWithTax)
        {
            long WinMoney = 0;
            if (model == null)
                return 0;
            if (wt.Count <= 0)
                return 0;
            switch (model.PlayCode)
            {
                case 20201: //任选二
                    WinMoney = ComputeWin_RX2(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 20202: //任选三
                    WinMoney = ComputeWin_RX3(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 20203: //任选四
                    WinMoney = ComputeWin_RX4(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 20204: //任选五
                    WinMoney = ComputeWin_RX5(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 20205: //任选六
                    WinMoney = ComputeWin_RX6(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 20206: //任选七
                    WinMoney = ComputeWin_RX7(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 20207: //任选八
                    WinMoney = ComputeWin_RX8(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 20208: //前一
                    WinMoney = ComputeWin_Q1(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 20209: //前二直选
                    WinMoney = ComputeWin_Q2ZX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 20210: //前二组选
                    WinMoney = ComputeWin_Q2ZUX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 20211: //前三直选
                    WinMoney = ComputeWin_Q3ZX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
                case 20212: //前三组选
                    WinMoney = ComputeWin_Q3ZUX(model.Number, model.OpenNumber, ref Description, ref WinMoneyNoWithTax);
                    break;
            }
            return WinMoney;
        }

        #region 奖等算奖
        //任选二
        public long ComputeWin_RX2(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //单式：0102   复式：010203   胆拖：01*020304
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            WinType model = wt.Find(p => p.WinCode == 2020101 && p.PlayCode == 20201);

            if (Number.IndexOf('*') > -1) //胆拖
            {
                string[] arr = Number.Split('*');
                string first_num = arr[0];
                string[] second_num = SplitLotteryNumber(arr[1]);
                if (second_num == null) return 0;
                if (second_num.Length < 1) return 0;

                for (int i = 0; i < second_num.Length; i++)
                {
                    if (OpenNum.Contains(first_num) && OpenNum.Contains(second_num[i]))
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
                if (Description1 > 0)
                    Description = "任选二,胆拖" + Description1 + "注。";
            }
            else if (Number.IndexOf('^') > -1 || Number.Length == 4) //单式
            {
                string[] arr = Number.Split('^');
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] num = SplitLotteryNumber(arr[i]);
                    if (num.Length < 2) continue;
                    if (OpenNum.Contains(num[0]) && OpenNum.Contains(num[1]))
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
                if (Description1 > 0)
                    Description = "任选二,单式" + Description1 + "注。";
            }
            else if (Number.Length > 4) //复式
            {
                string[] arr = SplitLotteryNumber(Number);
                if (arr.Length < 2) return 0;
                int n = arr.Length;
                for (int i = 0; i < n - 1; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        if (OpenNum.Contains(arr[i]) && OpenNum.Contains(arr[j]))
                        {
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }
                if (Description1 > 0)
                    Description = "任选二,复式" + Description1 + "注。";
            }

            return WinMoney;
        }
        //任选三
        public long ComputeWin_RX3(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //单式：010203   复式：01020304   胆拖：01*020304、0102*0304
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            WinType model = wt.Find(p => p.WinCode == 2020201 && p.PlayCode == 20202);

            if (Number.IndexOf('*') > -1) //胆拖
            {
                #region
                string[] arr = Number.Split('*');
                string[] first_num = SplitLotteryNumber(arr[0]);
                string[] second_num = SplitLotteryNumber(arr[1]);
                if (second_num == null) return 0;
                if (second_num.Length < 1) return 0;
                int n = second_num.Length;

                if (first_num.Length == 1)
                {
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = i + 1; j < n; j++)
                        {
                            if (OpenNum.Contains(first_num[0]) && OpenNum.Contains(second_num[i]) && OpenNum.Contains(second_num[j]))
                            {
                                Description1++;
                                WinMoney += model.DefaultMoney;
                                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                            }
                        }
                    }
                }
                else if (first_num.Length == 2)
                {
                    for (int i = 0; i < second_num.Length; i++)
                    {
                        if (OpenNum.Contains(first_num[0]) && OpenNum.Contains(first_num[1]) && OpenNum.Contains(second_num[i]))
                        {
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "任选三,胆拖" + Description1 + "注。";
                #endregion
            }
            else if (Number.IndexOf('^') > -1 || Number.Length == 6) //单式
            {
                string[] arr = Number.Split('^');
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] num = SplitLotteryNumber(arr[i]);
                    if (num.Length < 3) continue;
                    if (OpenNum.Contains(num[0]) && OpenNum.Contains(num[1]) && OpenNum.Contains(num[2]))
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
                if (Description1 > 0)
                    Description = "任选三,单式" + Description1 + "注。";
            }
            else if (Number.Length > 6) //复式
            {
                string[] arr = SplitLotteryNumber(Number);
                int n = arr.Length;
                for (int i = 0; i < n - 2; i++)
                {
                    for (int j = i + 1; j < n - 1; j++)
                    {
                        for (int x = j + 1; x < n; x++)
                        {
                            if (OpenNum.Contains(arr[i]) && OpenNum.Contains(arr[j]) && OpenNum.Contains(arr[x]))
                            {
                                Description1++;
                                WinMoney += model.DefaultMoney;
                                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                            }
                        }
                    }
                }
                if (Description1 > 0)
                    Description = "任选三,复式" + Description1 + "注。";
            }

            return WinMoney;
        }
        //任选四
        public long ComputeWin_RX4(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //单式：01020304   复式：0102030405   胆拖：01*02030405、010203*0405
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            WinType model = wt.Find(p => p.WinCode == 2020301 && p.PlayCode == 20203);

            if (Number.IndexOf('*') > -1) //胆拖
            {
                #region
                string[] arr = Number.Split('*');
                string[] first_num = SplitLotteryNumber(arr[0]);
                string[] second_num = SplitLotteryNumber(arr[1]);
                if (second_num == null) return 0;
                if (second_num.Length < 1) return 0;
                int n = second_num.Length;

                if (first_num.Length == 1)
                {
                    for (int i = 0; i < n - 2; i++)
                    {
                        for (int j = i + 1; j < n - 1; j++)
                        {
                            for (int x = j + 1; x < n; x++)
                            {
                                if (OpenNum.Contains(first_num[0]) && OpenNum.Contains(second_num[i]) && OpenNum.Contains(second_num[j]) && OpenNum.Contains(second_num[x]))
                                {
                                    Description1++;
                                    WinMoney += model.DefaultMoney;
                                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                }
                            }
                        }
                    }
                }
                else if (first_num.Length == 2)
                {
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = i + 1; j < n; j++)
                        {
                            if (OpenNum.Contains(first_num[0]) && OpenNum.Contains(first_num[1]) && OpenNum.Contains(second_num[i]) && OpenNum.Contains(second_num[j]))
                            {
                                Description1++;
                                WinMoney += model.DefaultMoney;
                                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                            }
                        }
                    }
                }
                else if (first_num.Length == 3)
                {
                    for (int i = 0; i < second_num.Length; i++)
                    {
                        if (OpenNum.Contains(first_num[0]) && OpenNum.Contains(first_num[1]) && OpenNum.Contains(first_num[2]) && OpenNum.Contains(second_num[i]))
                        {
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "任选四,胆拖" + Description1 + "注。";
                #endregion
            }
            else if (Number.IndexOf('^') > -1 || Number.Length == 8) //单式
            {
                string[] arr = Number.Split('^');
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] num = SplitLotteryNumber(arr[i]);
                    if (num.Length < 4) continue;
                    if (OpenNum.Contains(num[0]) && OpenNum.Contains(num[1]) && OpenNum.Contains(num[2]) && OpenNum.Contains(num[3]))
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
                if (Description1 > 0)
                    Description = "任选四,单式" + Description1 + "注。";
            }
            else if (Number.Length > 8) //复式
            {
                string[] arr = SplitLotteryNumber(Number);
                int n = arr.Length;
                for (int i = 0; i < n - 3; i++)
                {
                    for (int j = i + 1; j < n - 2; j++)
                    {
                        for (int x = j + 1; x < n - 1; x++)
                        {
                            for (int y = x + 1; y < n; y++)
                            {
                                if (OpenNum.Contains(arr[i]) && OpenNum.Contains(arr[j]) && OpenNum.Contains(arr[x]) && OpenNum.Contains(arr[y]))
                                {
                                    Description1++;
                                    WinMoney += model.DefaultMoney;
                                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                }
                            }
                        }
                    }
                }
                if (Description1 > 0)
                    Description = "任选四,复式" + Description1 + "注。";
            }
            return WinMoney;
        }
        //任选五
        public long ComputeWin_RX5(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //单式：0102030405   复式：010203040506   胆拖：01*0203040506、01020304*0506
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            WinType model = wt.Find(p => p.WinCode == 2020401 && p.PlayCode == 20204);

            if (Number.IndexOf('*') > -1) //胆拖
            {
                //号码大于五的情况下都是只有一注中奖
                #region
                string[] arr = Number.Split('*');
                string[] first_num = SplitLotteryNumber(arr[0]);
                string[] second_num = SplitLotteryNumber(arr[1]);
                if (second_num == null) return 0;
                if (second_num.Length < 1) return 0;

                //string[] NewArr = new string[first_num.Length + second_num.Length];
                //first_num.CopyTo(NewArr, 0);
                //second_num.CopyTo(NewArr, first_num.Length);
                //int iWin = 0;
                //for (int i = 0; i < OpenNum.Length; i++)
                //{
                //    if (NewArr.Contains(OpenNum[i]))
                //        iWin = iWin + 1;
                //}
                //if (iWin == 5)
                //{
                //    Description1++;
                //    WinMoney += model.DefaultMoney;
                //    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                //}
                //030607*0102040508091011
                #region 优化胆拖中奖规则(最多四个胆拖)
                if (first_num.Length == 1)
                {
                    if (OpenNum.Contains(first_num[0]))
                    {
                        int t = 0;
                        for (int i = 0; i < second_num.Length; i++)
                            if (OpenNum.Contains(second_num[i]))
                                t++;
                        if (t >= 4)
                        {
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }
                else if (first_num.Length == 2)
                {
                    if (OpenNum.Contains(first_num[0]) && OpenNum.Contains(first_num[1]))
                    {
                        int t = 0;
                        for (int i = 0; i < second_num.Length; i++)
                            if (OpenNum.Contains(second_num[i]))
                                t++;
                        if (t >= 3)
                        {
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }
                else if (first_num.Length == 3)
                {
                    if (OpenNum.Contains(first_num[0]) && OpenNum.Contains(first_num[1]) && OpenNum.Contains(first_num[2]))
                    {
                        int t = 0;
                        for (int i = 0; i < second_num.Length; i++)
                            if (OpenNum.Contains(second_num[i]))
                                t++;
                        if (t >= 2)
                        {
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }
                else if (first_num.Length == 4)
                {
                    if (OpenNum.Contains(first_num[0]) && OpenNum.Contains(first_num[1]) && OpenNum.Contains(first_num[2]) && OpenNum.Contains(first_num[3]))
                    {
                        int t = 0;
                        for (int i = 0; i < second_num.Length; i++)
                            if (OpenNum.Contains(second_num[i]))
                                t++;
                        if (t >= 1)
                        {
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }
                #endregion

                if (Description1 > 0)
                    Description = "任选五,胆拖" + Description1 + "注。";
                #endregion
            }
            else if (Number.IndexOf('^') > -1 || Number.Length == 10) //单式
            {
                string[] arr = Number.Split('^');
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] num = SplitLotteryNumber(arr[i]);
                    if (num.Length < 5) continue;
                    if (OpenNum.Contains(num[0]) && OpenNum.Contains(num[1]) && OpenNum.Contains(num[2]) && OpenNum.Contains(num[3]) && OpenNum.Contains(num[4]))
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
                if (Description1 > 0)
                    Description = "任选五,单式" + Description1 + "注。";
            }
            else if (Number.Length > 10) //复式
            {
                string[] arr = SplitLotteryNumber(Number);
                //号码大于五的情况下都是只有一注中奖
                int iWin = 0;
                for (int i = 0; i < OpenNum.Length; i++)
                {
                    if (arr.Contains(OpenNum[i]))
                        iWin = iWin + 1;
                }
                if (iWin == 5)
                {
                    Description1++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
                if (Description1 > 0)
                    Description = "任选五,复式" + Description1 + "注。";
            }
            return WinMoney;
        }
        //任选六
        public long ComputeWin_RX6(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //单式：010203040506   复式：01020304050607   胆拖：01*020304050607、0102030405*0607
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            WinType model = wt.Find(p => p.WinCode == 2020501 && p.PlayCode == 20205);

            if (Number.IndexOf('*') > -1) //胆拖
            {
                #region
                string[] arr = Number.Split('*');
                string[] first_num = SplitLotteryNumber(arr[0]);
                string[] second_num = SplitLotteryNumber(arr[1]);
                if (second_num == null) return 0;
                if (second_num.Length < 1) return 0;

                int iWin = 0;
                string[] NewArr = new string[6];
                first_num.CopyTo(NewArr, 0);
                int n = second_num.Length;
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
                                        iWin = 0;
                                        NewArr[1] = second_num[i]; NewArr[2] = second_num[j]; NewArr[3] = second_num[x]; NewArr[4] = second_num[y]; NewArr[5] = second_num[z];
                                        for (int ii = 0; ii < OpenNum.Length; ii++)
                                        {
                                            if (NewArr.Contains(OpenNum[ii]))
                                                iWin = iWin + 1;
                                        }
                                        if (iWin == 5)
                                        {
                                            Description1++;
                                            WinMoney += model.DefaultMoney;
                                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                        }
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
                                    iWin = 0;
                                    NewArr[2] = second_num[i]; NewArr[3] = second_num[j]; NewArr[4] = second_num[x]; NewArr[5] = second_num[y];
                                    for (int ii = 0; ii < OpenNum.Length; ii++)
                                    {
                                        if (NewArr.Contains(OpenNum[ii]))
                                            iWin = iWin + 1;
                                    }
                                    if (iWin == 5)
                                    {
                                        Description1++;
                                        WinMoney += model.DefaultMoney;
                                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                    }
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
                                iWin = 0;
                                NewArr[3] = second_num[i]; NewArr[4] = second_num[j]; NewArr[5] = second_num[x];
                                for (int ii = 0; ii < OpenNum.Length; ii++)
                                {
                                    if (NewArr.Contains(OpenNum[ii]))
                                        iWin = iWin + 1;
                                }
                                if (iWin == 5)
                                {
                                    Description1++;
                                    WinMoney += model.DefaultMoney;
                                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                }
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
                            iWin = 0;
                            NewArr[4] = second_num[i]; NewArr[5] = second_num[j];
                            for (int ii = 0; ii < OpenNum.Length; ii++)
                            {
                                if (NewArr.Contains(OpenNum[ii]))
                                    iWin = iWin + 1;
                            }
                            if (iWin == 5)
                            {
                                Description1++;
                                WinMoney += model.DefaultMoney;
                                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                            }
                        }
                    }
                    #endregion
                }
                else if (first_num.Length == 5)
                {
                    #region
                    for (int i = 0; i < n; i++)
                    {
                        iWin = 0;
                        NewArr[5] = second_num[i];
                        for (int ii = 0; ii < OpenNum.Length; ii++)
                        {
                            if (NewArr.Contains(OpenNum[ii]))
                                iWin = iWin + 1;
                        }
                        if (iWin == 5)
                        {
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                    #endregion
                }
                if (Description1 > 0)
                    Description = "任选六,胆拖" + Description1 + "注。";
                #endregion
            }
            else if (Number.IndexOf('^') > -1 || Number.Length == 12) //单式
            {
                string[] arr = Number.Split('^');
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] num = SplitLotteryNumber(arr[i]);
                    if (num.Length < 6) continue;

                    if (num.Contains(OpenNum[0]) && num.Contains(OpenNum[1]) && num.Contains(OpenNum[2]) && num.Contains(OpenNum[3]) && num.Contains(OpenNum[4]))
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
                if (Description1 > 0)
                    Description = "任选六,单式" + Description1 + "注。";
            }
            else if (Number.Length > 12) //复式
            {
                #region
                string[] arr = SplitLotteryNumber(Number);
                int n = arr.Length;
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
                                    for (int a = z + 1; a < n; a++)
                                    {
                                        int iWin = 0;
                                        string[] NewArr = new string[6];
                                        NewArr[0] = arr[i]; NewArr[1] = arr[j]; NewArr[2] = arr[x]; NewArr[3] = arr[y]; NewArr[4] = arr[z]; NewArr[5] = arr[a];
                                        for (int ii = 0; ii < OpenNum.Length; ii++)
                                        {
                                            if (NewArr.Contains(OpenNum[ii]))
                                                iWin = iWin + 1;
                                        }
                                        if (iWin == 5)
                                        {
                                            Description1++;
                                            WinMoney += model.DefaultMoney;
                                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (Description1 > 0)
                    Description = "任选六,复式" + Description1 + "注。";
                #endregion
            }
            return WinMoney;
        }
        //任选七
        public long ComputeWin_RX7(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //单式：01020304050607   复式：0102030405060708   胆拖：01*02030405060708、010203040506*0708
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            WinType model = wt.Find(p => p.WinCode == 2020601 && p.PlayCode == 20206);

            if (Number.IndexOf('*') > -1) //胆拖
            {
                #region
                string[] arr = Number.Split('*');
                string[] first_num = SplitLotteryNumber(arr[0]);
                string[] second_num = SplitLotteryNumber(arr[1]);
                if (second_num == null) return 0;
                if (second_num.Length < 1) return 0;

                int iWin = 0;
                string[] NewArr = new string[7];
                first_num.CopyTo(NewArr, 0);
                int n = second_num.Length;

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
                                        for (int a = z + 1; a < n; a++)
                                        {
                                            iWin = 0;
                                            NewArr[1] = second_num[i]; NewArr[2] = second_num[j]; NewArr[3] = second_num[x]; NewArr[4] = second_num[y]; NewArr[5] = second_num[z]; NewArr[6] = second_num[a];
                                            for (int ii = 0; ii < OpenNum.Length; ii++)
                                            {
                                                if (NewArr.Contains(OpenNum[ii]))
                                                    iWin = iWin + 1;
                                            }
                                            if (iWin == 5)
                                            {
                                                Description1++;
                                                WinMoney += model.DefaultMoney;
                                                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                            }
                                        }
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
                                        iWin = 0;
                                        NewArr[2] = second_num[i]; NewArr[3] = second_num[j]; NewArr[4] = second_num[x]; NewArr[5] = second_num[y]; NewArr[6] = second_num[z];
                                        for (int ii = 0; ii < OpenNum.Length; ii++)
                                        {
                                            if (NewArr.Contains(OpenNum[ii]))
                                                iWin = iWin + 1;
                                        }
                                        if (iWin == 5)
                                        {
                                            Description1++;
                                            WinMoney += model.DefaultMoney;
                                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (first_num.Length == 3)
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
                                    iWin = 0;
                                    NewArr[3] = second_num[i]; NewArr[4] = second_num[j]; NewArr[5] = second_num[x]; NewArr[6] = second_num[y];
                                    for (int ii = 0; ii < OpenNum.Length; ii++)
                                    {
                                        if (NewArr.Contains(OpenNum[ii]))
                                            iWin = iWin + 1;
                                    }
                                    if (iWin == 5)
                                    {
                                        Description1++;
                                        WinMoney += model.DefaultMoney;
                                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (first_num.Length == 4)
                {
                    #region
                    for (int i = 0; i < n - 2; i++)
                    {
                        for (int j = i + 1; j < n - 1; j++)
                        {
                            for (int x = j + 1; x < n; x++)
                            {
                                iWin = 0;
                                NewArr[4] = second_num[i]; NewArr[5] = second_num[j]; NewArr[6] = second_num[x];
                                for (int ii = 0; ii < OpenNum.Length; ii++)
                                {
                                    if (NewArr.Contains(OpenNum[ii]))
                                        iWin = iWin + 1;
                                }
                                if (iWin == 5)
                                {
                                    Description1++;
                                    WinMoney += model.DefaultMoney;
                                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (first_num.Length == 5)
                {
                    #region
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = i + 1; j < n; j++)
                        {
                            iWin = 0;
                            NewArr[5] = second_num[i]; NewArr[6] = second_num[j];
                            for (int ii = 0; ii < OpenNum.Length; ii++)
                            {
                                if (NewArr.Contains(OpenNum[ii]))
                                    iWin = iWin + 1;
                            }
                            if (iWin == 5)
                            {
                                Description1++;
                                WinMoney += model.DefaultMoney;
                                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                            }
                        }
                    }
                    #endregion
                }
                else if (first_num.Length == 6)
                {
                    #region
                    for (int i = 0; i < n; i++)
                    {
                        iWin = 0;
                        NewArr[6] = second_num[i];
                        for (int ii = 0; ii < OpenNum.Length; ii++)
                        {
                            if (NewArr.Contains(OpenNum[ii]))
                                iWin = iWin + 1;
                        }
                        if (iWin == 5)
                        {
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                    #endregion
                }

                if (Description1 > 0)
                    Description = "任选七,胆拖" + Description1 + "注。";
                #endregion
            }
            else if (Number.IndexOf('^') > -1 || Number.Length == 14) //单式
            {
                string[] arr = Number.Split('^');
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] num = SplitLotteryNumber(arr[i]);
                    if (num.Length < 7) continue;

                    if (num.Contains(OpenNum[0]) && num.Contains(OpenNum[1]) && num.Contains(OpenNum[2]) && num.Contains(OpenNum[3]) && num.Contains(OpenNum[4]))
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
                if (Description1 > 0)
                    Description = "任选七,单式" + Description1 + "注。";
            }
            else if (Number.Length > 14) //复式
            {
                #region
                string[] arr = SplitLotteryNumber(Number);
                int n = arr.Length;
                for (int i = 0; i < n - 6; i++)
                {
                    for (int j = i + 1; j < n - 5; j++)
                    {
                        for (int x = j + 1; x < n - 4; x++)
                        {
                            for (int y = x + 1; y < n - 3; y++)
                            {
                                for (int z = y + 1; z < n - 2; z++)
                                {
                                    for (int a = z + 1; a < n - 1; a++)
                                    {
                                        for (int b = a + 1; b < n; b++)
                                        {
                                            int iWin = 0;
                                            string[] NewArr = new string[7];
                                            NewArr[0] = arr[i]; NewArr[1] = arr[j]; NewArr[2] = arr[x]; NewArr[3] = arr[y]; NewArr[4] = arr[z]; NewArr[5] = arr[a]; NewArr[6] = arr[b];
                                            for (int ii = 0; ii < OpenNum.Length; ii++)
                                            {
                                                if (NewArr.Contains(OpenNum[ii]))
                                                    iWin = iWin + 1;
                                            }
                                            if (iWin == 5)
                                            {
                                                Description1++;
                                                WinMoney += model.DefaultMoney;
                                                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "任选七,复式" + Description1 + "注。";
                #endregion
            }
            return WinMoney;
        }
        //任选八
        public long ComputeWin_RX8(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //单式：0102030405060708   复式：010203040506070809   胆拖：01*0203040506070809、01020304050607*0809
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            WinType model = wt.Find(p => p.WinCode == 2020701 && p.PlayCode == 20207);

            if (Number.IndexOf('*') > -1) //胆拖
            {
                #region
                string[] arr = Number.Split('*');
                string[] first_num = SplitLotteryNumber(arr[0]);
                string[] second_num = SplitLotteryNumber(arr[1]);
                if (second_num == null) return 0;
                if (second_num.Length < 1) return 0;

                int iWin = 0;
                string[] NewArr = new string[8];
                first_num.CopyTo(NewArr, 0);
                int n = second_num.Length;

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
                                        for (int a = z + 1; a < n; a++)
                                        {
                                            for (int b = a + 1; b < n; b++)
                                            {
                                                iWin = 0;
                                                NewArr[1] = second_num[i]; NewArr[2] = second_num[j]; NewArr[3] = second_num[x]; NewArr[4] = second_num[y]; NewArr[5] = second_num[z];
                                                NewArr[6] = second_num[a]; NewArr[7] = second_num[b];
                                                for (int ii = 0; ii < OpenNum.Length; ii++)
                                                {
                                                    if (NewArr.Contains(OpenNum[ii]))
                                                        iWin = iWin + 1;
                                                }
                                                if (iWin == 5)
                                                {
                                                    Description1++;
                                                    WinMoney += model.DefaultMoney;
                                                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
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
                else if (first_num.Length == 2)
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
                                        for (int a = z + 1; a < n; a++)
                                        {
                                            iWin = 0;
                                            NewArr[2] = second_num[i]; NewArr[3] = second_num[j]; NewArr[4] = second_num[x]; NewArr[5] = second_num[y]; NewArr[6] = second_num[z]; NewArr[7] = second_num[a];
                                            for (int ii = 0; ii < OpenNum.Length; ii++)
                                            {
                                                if (NewArr.Contains(OpenNum[ii]))
                                                    iWin = iWin + 1;
                                            }
                                            if (iWin == 5)
                                            {
                                                Description1++;
                                                WinMoney += model.DefaultMoney;
                                                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (first_num.Length == 3)
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
                                        iWin = 0;
                                        NewArr[3] = second_num[i]; NewArr[4] = second_num[j]; NewArr[5] = second_num[x]; NewArr[6] = second_num[y]; NewArr[7] = second_num[z];
                                        for (int ii = 0; ii < OpenNum.Length; ii++)
                                        {
                                            if (NewArr.Contains(OpenNum[ii]))
                                                iWin = iWin + 1;
                                        }
                                        if (iWin == 5)
                                        {
                                            Description1++;
                                            WinMoney += model.DefaultMoney;
                                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (first_num.Length == 4)
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
                                    iWin = 0;
                                    NewArr[4] = second_num[i]; NewArr[5] = second_num[j]; NewArr[6] = second_num[x]; NewArr[7] = second_num[y];
                                    for (int ii = 0; ii < OpenNum.Length; ii++)
                                    {
                                        if (NewArr.Contains(OpenNum[ii]))
                                            iWin = iWin + 1;
                                    }
                                    if (iWin == 5)
                                    {
                                        Description1++;
                                        WinMoney += model.DefaultMoney;
                                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (first_num.Length == 5)
                {
                    #region
                    for (int i = 0; i < n - 2; i++)
                    {
                        for (int j = i + 1; j < n - 1; j++)
                        {
                            for (int x = j + 1; x < n; x++)
                            {
                                iWin = 0;
                                NewArr[5] = second_num[i]; NewArr[6] = second_num[j]; NewArr[7] = second_num[x];
                                for (int ii = 0; ii < OpenNum.Length; ii++)
                                {
                                    if (NewArr.Contains(OpenNum[ii]))
                                        iWin = iWin + 1;
                                }
                                if (iWin == 5)
                                {
                                    Description1++;
                                    WinMoney += model.DefaultMoney;
                                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (first_num.Length == 6)
                {
                    #region
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = i + 1; j < n; j++)
                        {
                            iWin = 0;
                            NewArr[6] = second_num[i]; NewArr[7] = second_num[j];
                            for (int ii = 0; ii < OpenNum.Length; ii++)
                            {
                                if (NewArr.Contains(OpenNum[ii]))
                                    iWin = iWin + 1;
                            }
                            if (iWin == 5)
                            {
                                Description1++;
                                WinMoney += model.DefaultMoney;
                                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                            }
                        }
                    }
                    #endregion
                }
                else if (first_num.Length == 7)
                {
                    #region
                    for (int i = 0; i < n; i++)
                    {
                        iWin = 0;
                        NewArr[7] = second_num[i];
                        for (int ii = 0; ii < OpenNum.Length; ii++)
                        {
                            if (NewArr.Contains(OpenNum[ii]))
                                iWin = iWin + 1;
                        }
                        if (iWin == 5)
                        {
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                    #endregion
                }

                if (Description1 > 0)
                    Description = "任选八,胆拖" + Description1 + "注。";
                #endregion
            }
            else if (Number.IndexOf('^') > -1 || Number.Length == 16) //单式
            {
                string[] arr = Number.Split('^');
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] num = SplitLotteryNumber(arr[i]);
                    if (num.Length < 8) continue;

                    if (num.Contains(OpenNum[0]) && num.Contains(OpenNum[1]) && num.Contains(OpenNum[2]) && num.Contains(OpenNum[3]) && num.Contains(OpenNum[4]))
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
                if (Description1 > 0)
                    Description = "任选八,单式" + Description1 + "注。";
            }
            else if (Number.Length > 16) //复式
            {
                #region 
                string[] arr = SplitLotteryNumber(Number);
                int n = arr.Length;
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
                                                int iWin = 0;
                                                string[] NewArr = new string[8];
                                                NewArr[0] = arr[i]; NewArr[1] = arr[j]; NewArr[2] = arr[x]; NewArr[3] = arr[y]; NewArr[4] = arr[z]; NewArr[5] = arr[a]; NewArr[6] = arr[b]; NewArr[7] = arr[c];
                                                for (int ii = 0; ii < OpenNum.Length; ii++)
                                                {
                                                    if (NewArr.Contains(OpenNum[ii]))
                                                        iWin = iWin + 1;
                                                }
                                                if (iWin == 5)
                                                {
                                                    Description1++;
                                                    WinMoney += model.DefaultMoney;
                                                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "任选八,复式" + Description1 + "注。";
                #endregion
            }
            return WinMoney;
        }
        //前一
        public long ComputeWin_Q1(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //单式5注：01^02^03^04^05
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            string[] Numbers = SplitLotteryNumber(Number, '^');
            if (Numbers == null) return 0;
            if (Numbers.Length < 1) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            WinType model = wt.Find(p => p.WinCode == 2020801 && p.PlayCode == 20208);

            for (int i = 0; i < Numbers.Length; i++)
            {
                if (OpenNum[0] == Numbers[i])
                {
                    Description1++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
            }
            if (Description1 > 0)
                Description = "前一,单式" + Description1 + "注。";
            return WinMoney;
        }
        //前二直选
        public long ComputeWin_Q2ZX(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //单式：0102 复式：010203 胆拖：01*0203
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            WinType model = wt.Find(p => p.WinCode == 2020901 && p.PlayCode == 20209);

            if (Number.IndexOf('*') > -1)
            {
                string[] arr = Number.Split('*');
                string[] first_num = SplitLotteryNumber(arr[0]);
                string[] second_num = SplitLotteryNumber(arr[1]);
                if (second_num == null) return 0;
                if (second_num.Length < 1) return 0;

                if (first_num.Contains(OpenNum[0]) && second_num.Contains(OpenNum[1]))
                {
                    Description1++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
                if (Description1 > 0)
                    Description = "前二直选,定位复式" + Description1 + "注。";
            }
            else
            {
                string[] Numbers = SplitLotteryNumber(Number, '^');
                if (Numbers == null) return 0;
                if (Numbers.Length < 1) return 0;

                for (int i = 0; i < Numbers.Length; i++)
                {
                    string[] num = SplitLotteryNumber(Numbers[i]);
                    if (OpenNum[0] == num[0] && OpenNum[1] == num[1])
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
                if (Description1 > 0)
                    Description = "前二直选,单式" + Description1 + "注。";
            }
            return WinMoney;
        }
        //前二组选
        public long ComputeWin_Q2ZUX(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //单式：0102   定位：01 * 0203、0102 * 0304
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            WinType model = wt.Find(p => p.WinCode == 2021001 && p.PlayCode == 20210);

            if (Number.IndexOf('*') > -1)
            {
                string[] arr = Number.Split('*');
                string first_num = arr[0];
                string[] second_num = SplitLotteryNumber(arr[1]);
                if (second_num == null) return 0;
                if (second_num.Length < 1) return 0;

                for (int i = 0; i < second_num.Length; i++)
                {
                    if (OpenNum[0] == first_num && OpenNum[1] == second_num[i])
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        break;
                    }
                }
                if (Description1 > 0)
                    Description = "前二组选,胆拖" + Description1 + "注。";
            }
            else if (Number.IndexOf('^') > -1 || Number.Length == 4) //单式
            {
                string[] arr = Number.Split('^');
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] num = SplitLotteryNumber(arr[i]);
                    if (num.Length < 2) continue;
                    int j = 0;
                    if (num.Contains(OpenNum[0]))
                        j++;
                    if (num.Contains(OpenNum[1]))
                        j++;

                    if (j == 2)
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
                if (Description1 > 0)
                    Description = "前二组选,单式" + Description1 + "注。";
            }
            else if (Number.Length > 4) //复式
            {
                string[] arr = SplitLotteryNumber(Number);
                if (arr.Length < 2) return 0;
                int n = arr.Length;
                for (int i = 0; i < n - 1; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        if (OpenNum[0] == arr[i] && OpenNum[1] == arr[j])
                        {
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }
                if (Description1 > 0)
                    Description = "前二组选,复式" + Description1 + "注。";
            }
            return WinMoney;
        }
        //前三直选
        public long ComputeWin_Q3ZX(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //单式：0102   定位：01 * 0203、0102 * 0304
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            WinType model = wt.Find(p => p.WinCode == 2021101 && p.PlayCode == 20211);

            if (Number.IndexOf('*') > -1)
            {
                string[] arr = Number.Split('*');
                string[] first_num = SplitLotteryNumber(arr[0]);
                string[] second_num = SplitLotteryNumber(arr[1]);
                string[] third_num = SplitLotteryNumber(arr[2]);
                if (second_num == null) return 0;
                if (second_num.Length < 1) return 0;
                if (third_num == null) return 0;
                if (third_num.Length < 1) return 0;

                if (first_num.Contains(OpenNum[0]) && second_num.Contains(OpenNum[1]) && second_num.Contains(OpenNum[2]))
                {
                    Description1++;
                    WinMoney += model.DefaultMoney;
                    WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                }
                if (Description1 > 0)
                    Description = "前三直选,定位复式" + Description1 + "注。";
            }
            else
            {
                string[] Numbers = SplitLotteryNumber(Number, '^');
                if (Numbers == null) return 0;
                if (Numbers.Length < 1) return 0;

                for (int i = 0; i < Numbers.Length; i++)
                {
                    string[] num = SplitLotteryNumber(Numbers[i]);
                    if (OpenNum[0] == num[0] && OpenNum[1] == num[1] && OpenNum[2] == num[2])
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
                if (Description1 > 0)
                    Description = "前三直选,单式" + Description1 + "注。";
            }
            return WinMoney;
        }
        //前三组选
        public long ComputeWin_Q3ZUX(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax)
        {
            //单式：010203  复式：01020304  胆拖：01*020304、0102*0304
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;

            int Description1 = 0;
            long WinMoney = 0;
            WinType model = wt.Find(p => p.WinCode == 2021201 && p.PlayCode == 20212);

            if (Number.IndexOf('*') > -1)
            {
                #region
                string[] arr = Number.Split('*');
                string[] first_num = SplitLotteryNumber(arr[0]);
                string[] second_num = SplitLotteryNumber(arr[1]);
                if (second_num == null) return 0;
                if (second_num.Length < 1) return 0;
                int n = second_num.Length;

                if (first_num.Length == 1)
                {
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = i + 1; j < n; j++)
                        {
                            if (OpenNum[0] == first_num[0] && OpenNum[1] == second_num[i] && OpenNum[2] == second_num[j])
                            {
                                Description1++;
                                WinMoney += model.DefaultMoney;
                                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                                break;
                            }
                        }
                    }
                }
                else if (first_num.Length == 2)
                {
                    for (int i = 0; i < second_num.Length; i++)
                    {
                        if (OpenNum[0] == first_num[0] && OpenNum[1] == first_num[1] && OpenNum[2] == second_num[i])
                        {
                            Description1++;
                            WinMoney += model.DefaultMoney;
                            WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                        }
                    }
                }

                if (Description1 > 0)
                    Description = "前三组选,胆拖" + Description1 + "注。";
                #endregion
            }
            else if (Number.IndexOf('^') > -1 || Number.Length == 6) //单式
            {
                string[] arr = Number.Split('^');

                for (int i = 0; i < arr.Length; i++)
                {
                    string[] num = SplitLotteryNumber(arr[i]);
                    if (num.Length < 3) continue;
                    int j = 0;


                    if (num.Contains(OpenNum[0]))
                        j++;
                    if (num.Contains(OpenNum[1]))
                        j++;
                    if (num.Contains(OpenNum[2]))
                        j++;

                    if (j == 3)
                    {
                        Description1++;
                        WinMoney += model.DefaultMoney;
                        WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                    }
                }
                if (Description1 > 0)
                    Description = "前三组选,单式" + Description1 + "注。";
            }
            else if (Number.Length > 6) //复式
            {
                string[] arr = SplitLotteryNumber(Number);
                int n = arr.Length;
                for (int i = 0; i < n - 2; i++)
                {
                    for (int j = i + 1; j < n - 1; j++)
                    {
                        for (int x = j + 1; x < n; x++)
                        {
                            if (OpenNum[0] == arr[i] && OpenNum[1] == arr[j] && OpenNum[2] == arr[x])
                            {
                                Description1++;
                                WinMoney += model.DefaultMoney;
                                WinMoneyNoWithTax += model.DefaultMoneyNoWithTax;
                            }
                        }
                    }
                }
                if (Description1 > 0)
                    Description = "前三组选,复式" + Description1 + "注。";
            }
            return WinMoney;
        }
        //乐选3
        public long ComputeWin_LX3(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            int Description1 = 0;
            long WinMoney = 0;
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;
            string[] num = SplitLotteryNumber(Number);
            #region 一等奖
            WinType model = wt.Find(p => p.WinCode == 2021301 && p.PlayCode == 20213);
            if (Description1 == 0)
            {
                if (num[0] == OpenNum[0] && num[1] == OpenNum[1] && num[2] == OpenNum[2])
                {
                    Description1++;
                    WinMoney = model.DefaultMoney;
                    WinMoneyNoWithTax = model.DefaultMoneyNoWithTax;
                    WinCode = model.WinCode;
                    Description = "乐选3按位中奖,单式" + Description1 + "注。";
                }
            }
            #endregion
            #region 二等奖
            model = wt.Find(p => p.WinCode == 2021302 && p.PlayCode == 20213);
            if (Description1 == 0)
            {
                if (num.Contains(OpenNum[0]) && num.Contains(OpenNum[1]) && num.Contains(OpenNum[2]))
                {
                    Description1++;
                    WinMoney = model.DefaultMoney;
                    WinMoneyNoWithTax = model.DefaultMoneyNoWithTax;
                    WinCode = model.WinCode;
                    Description = "乐选3不定位前三中奖,单式" + Description1 + "注。";
                }
            }
            #endregion
            #region 三等奖
            model = wt.Find(p => p.WinCode == 2021303 && p.PlayCode == 20213);
            if (Description1 == 0)
            {
                if (OpenNum.Contains(num[0]) && OpenNum.Contains(num[1]) && OpenNum.Contains(num[2]))
                {
                    Description1++;
                    WinMoney = model.DefaultMoney;
                    WinMoneyNoWithTax = model.DefaultMoneyNoWithTax;
                    WinCode = model.WinCode;
                    Description = "乐选3不定位任意中奖,单式" + Description1 + "注。";
                }
            }
            #endregion
            return WinMoney;
        }
        //乐选4
        public long ComputeWin_LX4(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            int Description1 = 0;
            long WinMoney = 0;
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;
            string[] num = SplitLotteryNumber(Number);
            #region 一等奖
            WinType model = wt.Find(p => p.WinCode == 2021401 && p.PlayCode == 20214);
            if (Description1 == 0)
            {
                if (OpenNum.Contains(num[0]) && OpenNum.Contains(num[1]) && OpenNum.Contains(num[2]) && OpenNum.Contains(num[3]))
                {
                    Description1++;
                    WinMoney = model.DefaultMoney;
                    WinMoneyNoWithTax = model.DefaultMoneyNoWithTax;
                    WinCode = model.WinCode;
                    Description = "乐选4任4中奖,单式" + Description1 + "注。";
                }
            }
            #endregion
            #region 二等奖
            model = wt.Find(p => p.WinCode == 2021402 && p.PlayCode == 20214);
            if (Description1 == 0)
            {
                int n = num.Length;
                for (int i = 0; i < n; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        for (int x = j + 1; x < n; x++)
                        {
                            if (OpenNum.Contains(num[i]) && OpenNum.Contains(num[j]) && OpenNum.Contains(num[x]))
                            {
                                Description1++;
                                WinMoney = model.DefaultMoney;
                                WinMoneyNoWithTax = model.DefaultMoneyNoWithTax;
                                WinCode = model.WinCode;
                                Description = "乐选4任3中奖,单式" + Description1 + "注。";
                            }
                        }
                    }
                }
            }
            #endregion
            return WinMoney;
        }
        //乐选5
        public long ComputeWin_LX5(string Number, string OpenNumber, ref string Description, ref long WinMoneyNoWithTax, ref int WinCode)
        {
            int Description1 = 0;
            long WinMoney = 0;
            string[] OpenNum = OpenNumber.Split(' ');
            if (OpenNum.Length < 5) return 0;
            string[] num = SplitLotteryNumber(Number);
            #region 一等奖
            WinType model = wt.Find(p => p.WinCode == 2021501 && p.PlayCode == 20215);
            if (Description1 == 0)
            {
                if (OpenNum.Contains(num[0]) && OpenNum.Contains(num[1]) && OpenNum.Contains(num[2]) && OpenNum.Contains(num[3]) && OpenNum.Contains(num[4]))
                {
                    Description1++;
                    WinMoney = model.DefaultMoney;
                    WinMoneyNoWithTax = model.DefaultMoneyNoWithTax;
                    WinCode = model.WinCode;
                    Description = "乐选5任5中奖,单式" + Description1 + "注。";
                }
            }
            #endregion
            #region 二等奖
            model = wt.Find(p => p.WinCode == 2021502 && p.PlayCode == 20215);
            if (Description1 == 0)
            {
                int n = num.Length;
                for (int i = 0; i < n; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        for (int x = j + 1; x < n; x++)
                        {
                            for (int y = x + 1; y < n; y++)
                            {
                                if (OpenNum.Contains(num[i]) && OpenNum.Contains(num[j]) && OpenNum.Contains(num[x]) && OpenNum.Contains(num[y]))
                                {
                                    Description1++;
                                    WinMoney = model.DefaultMoney;
                                    WinMoneyNoWithTax = model.DefaultMoneyNoWithTax;
                                    WinCode = model.WinCode;
                                    Description = "乐选5任4中奖,单式" + Description1 + "注。";
                                }
                            }
                        }
                    }
                }

            }
            #endregion
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
