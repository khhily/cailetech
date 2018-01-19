using CL.Enum.Common.Lottery;
using CL.Game.BLL.View;
using CL.Tools.Common;
using CL.Tools.LotteryTickets;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using CL.Game.BLL;

namespace CL.AwardServer
{
    public partial class FrmMain : Form
    {
        private readonly Log log = new Log("AutomaticAwardServer");
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
            quartzhelper.AddTrigger("ms", "ms_AwardServer_group", "*/1 * * * * ?", new Action(() =>
            {
                this.Invoke(new Action(() =>
                {
                    this.Text = "自动派奖服务     " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }));
            }));
            #endregion

            #region 自动派奖
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "Interface.xml");
            XmlNodeList XmlList = doc.SelectNodes("//EntryModel/Item");

            foreach (XmlNode item in XmlList)
            {
                string LotteryName = item.Attributes["LotteryName"].InnerText;
                int LotteryCode = Convert.ToInt32(item.SelectSingleNode("SystemLotteryCode").InnerText);
                string IntervalTime = item.SelectSingleNode("IntervalTime").InnerText;
                string InterfaceType = item.SelectSingleNode("InterfaceType").InnerText;

                string jobname = "AutomaticAward_" + LotteryName;
                string jobgroup = jobname + "_AwardLottery";
                LotteryBase builder = new LotteryBase()[LotteryCode];

                quartzhelper.AddTrigger(jobname, jobgroup, IntervalTime, new Action(() =>
                {
                    MidlifText(string.Format("【彩种：{0}.{1}】 自动派奖开始时间：{2} 派奖接口：{3}", LotteryCode, LotteryName, DateTime.Now.ToString("HH:mm:ss"), InterfaceType));
                    Thread.Sleep(5 * 1000);
                    try
                    {
                        if (LotteryCode == (int)LotteryInfo.SSQ || LotteryCode == (int)LotteryInfo.CJDLT)
                            builder.AwardWin(item);
                    }
                    catch (Exception ex)
                    {
                        MidlifText(jobname + ":" + ex.Message);
                    }
                }));

                #region 服务启动时执行一次派奖
                MidlifText(string.Format("【彩种：{0}.{1}】 服务启动时执行一次派奖，开始时间：{2}", LotteryCode, LotteryName, DateTime.Now.ToString("HH:mm:ss")));
                try
                {
                    Task.Factory.StartNew(new Action(() =>
                    {
                        if (LotteryCode == (int)LotteryInfo.SSQ || LotteryCode == (int)LotteryInfo.CJDLT)
                            builder.AwardWin(item);
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
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                this.Activate();
                this.ShowInTaskbar = true;
                nfico.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
