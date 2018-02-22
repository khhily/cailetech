using CL.Enum.Common.Lottery;
using CL.LotteryGameService.Model;
using CL.Tools.Common;
using CL.Tools.LotteryTickets;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CL.LotteryGameService.Award
{
    public class Handle_Award
    {
        /// <summary>
        /// 委托启动程序
        /// </summary>
        /// <param name="rich"></param>
        protected delegate void action_delegate(RichTextBox rich);
        /// <summary>
        /// 委托写日志
        /// </summary>
        protected static WritText log_Writ;
        /// <summary>
        /// 任务调度
        /// </summary>
        private QuartzHelper quartzhelper_award = new QuartzHelper();
        private QuartzHelper quartzhelper_compute = new QuartzHelper();
        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="rich"></param>
        public void Run(RichTextBox rich)
        {
            if (rich.InvokeRequired)
            {
                action_delegate dt = new action_delegate(Start);
                rich.Invoke(dt, new object[] { rich });
            }
        }
        /// <summary>
        /// 程序启动
        /// </summary>
        /// <param name="rich"></param>
        protected void Start(RichTextBox rich)
        {
            try
            {
                rich.ForeColor = System.Drawing.Color.Blue;
                log_Writ = new WritText(rich);
                Thread th_Compute = new Thread(new ThreadStart(Compute));
                Thread th_Award = new Thread(new ThreadStart(Award));
                th_Compute.Start();
                th_Award.Start();
            }
            catch (Exception ex)
            {
                log_Writ.WritTextBox(string.Format("算奖派奖启动失败：{0}", ex.Message));
                throw;
            }
        }
        protected void Compute()
        {
            try
            {
                XmlNode Node = Utils.QueryConfigNode("root/interface");
                XmlNodeList XmlList = Node.SelectNodes("item");
                foreach (XmlNode item in XmlList)
                {
                    string jobname = "AutomaticCompute_" + item.Attributes["lotteryname"].InnerText;
                    string jobgroup = jobname + "_ComputeLottery";
                    LotteryBase builder = new LotteryBase()[Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText)];
                    quartzhelper_compute.AddTrigger(jobname, jobgroup, item.SelectSingleNode("intervaltime").InnerText, new Action(() =>
                    {
                        try
                        {
                            log_Writ.WritTextBox(string.Format("【彩种：{0}.{1}】 自动算奖开始时间：{2}", Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText), item.Attributes["lotteryname"].InnerText, DateTime.Now.ToString("HH:mm:ss")));
                            if (Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText) != (int)LotteryInfo.CJDLT && Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText) != (int)LotteryInfo.SSQ)
                            {
                                builder.ComputeWin(item);
                                builder.ComputeChaseTasksWin(item);//追号算奖
                            }
                        }
                        catch (Exception ex)
                        {
                            log_Writ.WritTextBox(jobname + ":" + ex.Message);
                        }
                    }));
                }
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        quartzhelper_compute.Start();
                    }
                    catch (Exception ex)
                    {
                        log_Writ.WritTextBox(ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                log_Writ.WritTextBox(string.Format("算奖失败：{0}", ex.Message));
            }
        }
        protected void Award()
        {
            try
            {
                XmlNode Node = Utils.QueryConfigNode("root/interface");
                XmlNodeList XmlList = Node.SelectNodes("item");
                foreach (XmlNode item in XmlList)
                {
                    string jobname = "AutomaticAward_" + item.Attributes["lotteryname"].InnerText;
                    string jobgroup = jobname + "_AwardLottery";
                    LotteryBase builder = new LotteryBase()[Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText)];

                    quartzhelper_award.AddTrigger(jobname, jobgroup, item.SelectSingleNode("intervaltime").InnerText, new Action(() =>
                    {
                        try
                        {
                            log_Writ.WritTextBox(string.Format("【彩种：{0}.{1}】 自动派奖开始时间：{2} 派奖接口：{3}", Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText), item.Attributes["lotteryname"].InnerText, DateTime.Now.ToString("HH:mm:ss"), item.SelectSingleNode("interfacetype").InnerText));
                            if (Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText) == (int)LotteryInfo.SSQ || Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText) == (int)LotteryInfo.CJDLT)
                                builder.AwardWin(item);
                        }
                        catch (Exception ex)
                        {
                            log_Writ.WritTextBox(jobname + ":" + ex.Message);
                        }
                    }));
                }
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        quartzhelper_award.Start();
                    }
                    catch (Exception ex)
                    {
                        log_Writ.WritTextBox(ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                log_Writ.WritTextBox(string.Format("派奖失败：{0}", ex.Message));
            }
        }

    }
}
