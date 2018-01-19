using CL.Tools.Common;
using CL.Tools.LotteryTickets;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CL.TicketOutService
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
            quartzhelper.AddTrigger("ms", "ms_OutTicket_group", "*/1 * * * * ?", new Action(() =>
            {
                this.Invoke(new Action(() =>
                {
                    this.Text = "电子票出票服务     " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }));
            }));
            #endregion

            #region 自动出票
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "Interface.xml");
            XmlNodeList XmlList = doc.SelectNodes("//EntryModel/Item");
            foreach (XmlNode item in XmlList)
            {
                string LotteryName = item.Attributes["LotteryName"].InnerText;
                int LotteryCode = Convert.ToInt32(item.SelectSingleNode("SystemLotteryCode").InnerText);
                string IntervalTime = item.SelectSingleNode("IntervalTime").InnerText;
                string jobname = "OutTicket_" + LotteryName;
                string jobgroup = jobname + "_OutTicketLottery";
                LotteryBase builder = new LotteryBase()[LotteryCode];

                //自动出票
                quartzhelper.AddTrigger(jobname, jobgroup, IntervalTime, new Action(() =>
                {
                    MidlifText(string.Format("【彩种：{0}.{1}】 自动出票开始时间：{2}", LotteryCode, LotteryName, DateTime.Now.ToString("HH:mm:ss")));
                    Thread.Sleep(5 * 1000);
                    try
                    {
                        builder.OutTicket(item);
                    }
                    catch (Exception ex)
                    {
                        MidlifText(jobname + ":" + ex.Message);
                    }
                }));
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

        private void MidlifText(string str)
        {
            this.richTextBox1.Invoke(new Action(() =>
            {
                this.richTextBox1.AppendText(str + "\r\n");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "Interface.xml");
            XmlNodeList XmlList = doc.SelectNodes("//EntryModel/Item");
            XmlNode item = XmlList[1];
            int LotteryCode = Convert.ToInt32(item.SelectSingleNode("SystemLotteryCode").InnerText);
            LotteryBase builder = new LotteryBase()[LotteryCode];
            builder.OutTicket(item);

        }
    }
}
