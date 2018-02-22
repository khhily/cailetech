using CL.LotteryGameService.Model;
using CL.Tools.Common;
using CL.Tools.LotteryTickets;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CL.LotteryGameService.TicketOut
{
    public class Handle_TicketOut
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
        private QuartzHelper quartzhelper_ticketout = new QuartzHelper();
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
                Thread th_Data = new Thread(new ThreadStart(BindData));
                th_Data.Start();
            }
            catch (Exception ex)
            {
                log_Writ.WritTextBox(string.Format("方案出票启动失败：{0}", ex.Message));
                throw;
            }
        }

        protected void BindData()
        {
            XmlNode Node = Utils.QueryConfigNode("root/interface");
            XmlNodeList XmlList = Node.SelectNodes("item");
            foreach (XmlNode item in XmlList)
            {
                string LotteryName = item.Attributes["lotteryname"].InnerText;
                int LotteryCode = Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText);
                string IntervalTime = item.SelectSingleNode("intervaltime").InnerText;
                string jobname = "OutTicket_" + LotteryName;
                string jobgroup = jobname + "_OutTicketLottery";
                LotteryBase builder = new LotteryBase()[LotteryCode];
                //自动出票
                quartzhelper_ticketout.AddTrigger(jobname, jobgroup, IntervalTime, new Action(() =>
                {
                    log_Writ.WritTextBox(string.Format("【彩种：{0}.{1}】 自动出票开始时间：{2}", LotteryCode, LotteryName, DateTime.Now.ToString("HH:mm:ss")));
                    try
                    {
                        builder.OutTicket(item);
                    }
                    catch (Exception ex)
                    {
                        log_Writ.WritTextBox(jobname + ":" + ex.Message);
                    }
                }));
            }
            Task.Factory.StartNew(new Action(() =>
            {
                try
                {
                    quartzhelper_ticketout.Start();
                }
                catch (Exception ex)
                {
                    log_Writ.WritTextBox(ex.Message);
                }
            }));
        }
    }
}
