using CL.Tools.Common;
using CL.Tools.LotteryTickets;
using CL.Tools.TicketInterface;
using CL.View.Entity.Interface;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CL.ReptileServer
{
    public partial class FrmMain : Form
    {
        private QuartzHelper quartzhelper = new QuartzHelper();
        Log log = new Log("CL.ReptileServer.FrmMain");

        public FrmMain()
        {
            InitializeComponent();
            btnTest.Visible = false;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (btnTest.Visible)
                return;

            #region 时钟
            quartzhelper.AddTrigger("ms", "ms_group", "*/1 * * * * ?", new Action(() =>
            {
                this.Invoke(new Action(() =>
                {
                    lbtd.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }));
            }));
            #endregion

            #region 注册正常获取数据任务
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "Interface.xml");
            XmlNodeList XmlList = doc.SelectNodes("//EntryModel/Item");            
            foreach (XmlNode item in XmlList)
            {
                string LotteryName = item.Attributes["LotteryName"].InnerText;
                int LotteryCode = Convert.ToInt32(item.SelectSingleNode("SystemLotteryCode").InnerText);
                string IntervalTime = item.SelectSingleNode("IntervalTime").InnerText;
                string RevokeSchemeTime = item.SelectSingleNode("RevokeSchemeTime").InnerText;
                string TrappingTime = item.SelectSingleNode("TrappingTime").InnerText;

                string jobname = "craw_" + LotteryName;
                string jobgroup = jobname + "_group";
                LotteryBase builder = new LotteryBase()[item];

                //期号截止撤销等待拆票投注的订单
                quartzhelper.AddTrigger(jobname + "_RevokeScheme", jobgroup + "_RevokeScheme", RevokeSchemeTime, new Action(() =>
                {
                    builder.IsuseStopRevokeSchemes(LotteryCode);
                }));

                //维护当天的期号
                quartzhelper.AddTrigger(jobname + "_ModifyIsuseInfo", jobgroup + "_ModifyIsuseInfo", "0 0 6 * * ?", new Action(() =>
                {
                    MidlifText(string.Format("【彩种：{0}.{1}】 期号维护时间：{2}", LotteryCode, LotteryName, DateTime.Now.ToString("HH:mm:ss")));
                    builder.ModifyIsuseInfo();
                }));
                //注册获取遗漏的号码任务
                quartzhelper.AddTrigger(jobname + "_supp", jobgroup + "_supp", TrappingTime, new Action(() =>
                {
                    MidlifText(string.Format("【彩种：{0}.{1}】 采集遗漏号码开始时间：{2}", LotteryCode, LotteryName, DateTime.Now.ToString("HH:mm:ss")));
                    try
                    {
                        builder.GetSuppNum();
                    }
                    catch (Exception ex)
                    {
                        log.Write(string.Format("FrmMain_Load[获取遗漏的号码失败]:({0}) {1}:{2}", ex.Message, jobname, ex.StackTrace), true);
                        MidlifText(jobname + ":" + ex.Message);
                    }
                }));

                //正常获取数据任务
                quartzhelper.AddTrigger(jobname, jobgroup, IntervalTime, new Action(() =>
                {
                    MidlifText(string.Format("【彩种：{0}.{1}】 采集开始时间：{2}", LotteryCode, LotteryName, DateTime.Now.ToString("HH:mm:ss")));
                    Thread.Sleep(5 * 1000);
                    try
                    {
                        LotteryResult ent = builder.GetValue();
                        if (ent != null)
                        {
                            MidlifText(ent.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Write(string.Format("FrmMain_Load[获取开奖号码失败]:({0}) {1}:{2}", ex.Message, jobname, ex.StackTrace), true);
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
                    log.Write(string.Format("FrmMain_Load[启动调度失败]:({0}) {1}", ex.Message, ex.StackTrace), true);
                    MidlifText(ex.Message);
                }
            }));
            #endregion

            MidBntStatus(false);
        }

        private void bntStart_Click(object sender, EventArgs e)
        {
            quartzhelper.Start();
            MidBntStatus(false);
        }

        private void bntStop_Click(object sender, EventArgs e)
        {
            quartzhelper.End();
            MidBntStatus(true);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {

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
            //判断是否选择的是最小化按钮 
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标 
                this.ShowInTaskbar = false;
                //图标显示在托盘区 
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

        private void MidBntStatus(bool _status)
        {
            bntStart.Enabled = _status;
            bntStop.Enabled = !_status;
        }

    }
}
