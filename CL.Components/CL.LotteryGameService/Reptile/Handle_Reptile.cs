using CL.LotteryGameService.Model;
using CL.Tools.Common;
using CL.Tools.LotteryTickets;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CL.LotteryGameService.Reptile
{
    public class Handle_Reptile
    {
        protected delegate void action_delegate(RichTextBox rich);
        protected QuartzHelper quartzhelper_reptile = new QuartzHelper();
        protected static WritText log_Writ;
        /// <summary>
        /// 程序启动
        /// </summary>
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
                log_Writ.WritTextBox(string.Format("期号处理程序启动失败：{0}", ex.Message));
                throw;
            }
        }
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

        public void BindData()
        {

            XmlNode Node = Utils.QueryConfigNode("root/interface");
            foreach (XmlNode item in Node.SelectNodes("item"))
            {
                string jobname = "jobname_" + item.Attributes["lotteryname"].InnerText;
                string jobgroup = "jobgroup_" + jobname;
                LotteryBase builder = new LotteryBase()[item];

                #region 期号维护
                /**
                 * 维护当天的期号
                 * **/
                quartzhelper_reptile.AddTrigger(jobname + "_ModifyIsuseInfo", jobgroup + "_ModifyIsuseInfo", "0 0 6 * * ?", new Action(() =>
                {
                    try
                    {
                        log_Writ.WritTextBox(string.Format("【彩种：{0}.{1}】 期号维护时间：{2}", Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText), item.Attributes["lotteryname"].InnerText, DateTime.Now.ToString("HH:mm:ss")));
                        builder.ModifyIsuseInfo();
                    }
                    catch (Exception ex)
                    {
                        log_Writ.WritTextBox(jobname + ":" + ex.Message);
                    }
                }));
                /**
                 * 维护当天的期号(加拿大彩种(28))
                 * **/
                quartzhelper_reptile.AddTrigger(jobname + "_ModifyIsuseInfo_JND", jobgroup + "_ModifyIsuseInfo_JND", "0 0/1 21-22 * * ?", new Action(() =>
                {
                    try
                    {
                        log_Writ.WritTextBox(string.Format("【特殊彩种：{0}.{1}】 期号维护时间：{2}", Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText), item.Attributes["lotteryname"].InnerText, DateTime.Now.ToString("HH:mm:ss")));
                        builder.ModifyIsuseInfo_Foreign();
                    }
                    catch (Exception ex)
                    {
                        log_Writ.WritTextBox(jobname + ":" + ex.Message);
                    }
                }));
                #endregion

                #region 撤单
                /**
                 * 期号截止撤销等待拆票投注的订单
                 * **/
                quartzhelper_reptile.AddTrigger(jobname + "_RevokeScheme", jobgroup + "_RevokeScheme", item.SelectSingleNode("revokeschemetime").InnerText, new Action(() =>
                {
                    try
                    {
                        builder.IsuseStopRevokeSchemes(Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText));
                    }
                    catch (Exception ex)
                    {
                        log_Writ.WritTextBox(jobname + ":" + ex.Message);
                    }
                }));
                #endregion

                #region 期号开奖
                /**
                 * 获取遗漏的号码任务
                 * **/
                quartzhelper_reptile.AddTrigger(jobname + "_supp", jobgroup + "_supp", item.SelectSingleNode("trappingtime").InnerText, new Action(() =>
                {
                    try
                    {
                        log_Writ.WritTextBox(string.Format("【彩种：{0}.{1}】 采集遗漏号码开始时间：{2}", Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText), item.Attributes["lotteryname"].InnerText, DateTime.Now.ToString("HH:mm:ss")));
                        builder.GetSuppNum();
                    }
                    catch (Exception ex)
                    {
                        log_Writ.WritTextBox(jobname + ":" + ex.Message);
                    }
                }));
                /**
                 * 正常获取数据任务
                 * **/
                quartzhelper_reptile.AddTrigger(jobname, jobgroup, item.SelectSingleNode("intervaltime").InnerText, new Action(() =>
                {
                    try
                    {
                        log_Writ.WritTextBox(string.Format("【彩种：{0}.{1}】 采集开始时间：{2}", Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText), item.Attributes["lotteryname"].InnerText, DateTime.Now.ToString("HH:mm:ss")));
                        LotteryResult ent = builder.GetValue();
                        if (ent != null)
                        {
                            log_Writ.WritTextBox(ent.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        log_Writ.WritTextBox(jobname + ":" + ex.Message);
                    }
                }));
                #endregion
            }
            #region 启动调度
            Task.Factory.StartNew(new Action(() =>
            {
                try
                {
                    quartzhelper_reptile.Start();
                }
                catch (Exception ex)
                {
                    log_Writ.WritTextBox(string.Format("[Start]启动调度失败:({0}) {1}", ex.Message, ex.StackTrace));
                }
            }));
            #endregion
        }
    }
}
