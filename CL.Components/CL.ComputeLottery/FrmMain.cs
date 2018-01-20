﻿using CL.Enum.Common.Lottery;
using CL.Game.BLL.View;
using CL.Tools.Common;
using CL.Tools.LotteryTickets;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CL.ComputeLottery
{
    public partial class FrmMain : Form
    {
        private QuartzHelper quartzhelper = new QuartzHelper();

        public FrmMain()
        {
            InitializeComponent();
            button1.Visible = false;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (button1.Visible) return;

            #region 时钟
            quartzhelper.AddTrigger("ms", "ms_ComputeLotter_group", "*/1 * * * * ?", new Action(() =>
            {
                this.Invoke(new Action(() =>
                {
                    this.Text = "自动算奖服务     " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }));
            }));
            #endregion

            #region 自动算奖
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "Interface.xml");
            XmlNodeList XmlList = doc.SelectNodes("//EntryModel/Item");

            foreach (XmlNode item in XmlList)
            {
                string LotteryName = item.Attributes["LotteryName"].InnerText;
                int LotteryCode = Convert.ToInt32(item.SelectSingleNode("SystemLotteryCode").InnerText);
                string IntervalTime = item.SelectSingleNode("IntervalTime").InnerText;
                string jobname = "AutomaticCompute_" + LotteryName;
                string jobgroup = jobname + "_ComputeLottery";
                LotteryBase builder = new LotteryBase()[LotteryCode];

                quartzhelper.AddTrigger(jobname, jobgroup, IntervalTime, new Action(() =>
                {
                    MidlifText(string.Format("【彩种：{0}.{1}】 自动算奖开始时间：{2}", LotteryCode, LotteryName, DateTime.Now.ToString("HH:mm:ss")));
                    Thread.Sleep(5 * 1000);
                    try
                    {
                        if (LotteryCode != (int)LotteryInfo.CJDLT && LotteryCode != (int)LotteryInfo.SSQ)
                        {
                            builder.ComputeWin(item);
                            builder.ComputeChaseTasksWin(item);//追号算奖
                        }
                    }
                    catch (Exception ex)
                    {
                        MidlifText(jobname + ":" + ex.Message);
                    }
                }));

                #region 服务启动时执行一次算奖
                MidlifText(string.Format("【彩种：{0}.{1}】 服务启动时执行一次算奖，开始时间：{2}", LotteryCode, LotteryName, DateTime.Now.ToString("HH:mm:ss")));
                try
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        if (LotteryCode != (int)LotteryInfo.CJDLT && LotteryCode != (int)LotteryInfo.SSQ)
                        {
                            builder.ComputeWin(item);
                            builder.ComputeChaseTasksWin(item);//追号算奖
                        }
                    }));
                }
                catch (Exception ex)
                {
                    MidlifText(jobname + ":" + ex.Message);
                }
                #endregion
            }
            #endregion

            #region 启动调度
            Task.Factory.StartNew(new Action(() =>
            {
                try
                {
                    quartzhelper.Start();
                }
                catch (Exception ex)
                {
                    MidlifText(ex.Message);
                }
            }));
            #endregion

        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否立即退出", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                quartzhelper.End();
                this.Dispose();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                nfico.Visible = true;
            }
        }

        private void MidlifText(string str)
        {
            this.richTextBox1.Invoke(new Action(() =>
            {
                this.richTextBox1.AppendText(str + "\r\n");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }));
        }

        private void nfico_DoubleClick(object sender, EventArgs e)
        {
            //判断是否已经最小化于托盘 
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示 
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点 
                this.Activate();
                //任务栏区显示图标 
                this.ShowInTaskbar = true;
                //托盘区图标隐藏 
                nfico.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            MidlifText(string.Format("自动算奖共花费{0}ms.", ts2.TotalMilliseconds));


        }
    }
}