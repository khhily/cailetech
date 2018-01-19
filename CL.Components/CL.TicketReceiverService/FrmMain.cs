using CL.TicketReceiverService.BetTicketReceivers;
using CL.TicketReceiverService.SplitTicket;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CL.TicketReceiverService
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //启动投注队列
            BettingTicketOperation bettingticket = new BettingTicketOperation(richTextBox1);
            Task.Factory.StartNew(bettingticket.Run);

            //启动大票队列
            BettingBigTicketOperation bettingbigticket = new BettingBigTicketOperation(richTextBox1);
            Task.Factory.StartNew(bettingbigticket.Run);

            //启动拆票队列
            SplitTicketOperation splitticket = new SplitTicketOperation(richTextBox1);
            Task.Factory.StartNew(splitticket.Run);

            //启动大票拆票队列
            SplitBigTicketOperation splitbigtick = new SplitBigTicketOperation(richTextBox1);
            Task.Factory.StartNew(splitbigtick.Run);

            //启动机器人队列
            SplitRobotTicketOperation splitrobottick = new SplitRobotTicketOperation(richTextBox1);
            Task.Factory.StartNew(splitrobottick.Run);

        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否立即退出", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
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
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                this.Activate();
                this.ShowInTaskbar = true;
                nfico.Visible = false;
            }
        }
    }
}
