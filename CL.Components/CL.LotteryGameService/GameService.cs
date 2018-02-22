using CL.Tools.Common;
using System;
using System.Threading;
using System.Windows.Forms;

namespace CL.LotteryGameService
{
    public partial class GameService : Form
    {
        protected GameServiceBase serviceBase;
        protected string SystemTitle = string.Format("彩票游戏服务");
        public GameService()
        {
            InitializeComponent();
        }

        #region 窗体事件
        /// <summary>
        /// 窗体事件-加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameService_Load(object sender, EventArgs e)
        {
            Thread th_Service = new Thread(new ThreadStart(Start));
            th_Service.Name = string.Format("彩票游戏服务");
            th_Service.Start();
        }
        /// <summary>
        /// 窗体事件-退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameService_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否立即彩票游戏服务退出", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    serviceBase.Stop();
                    //图标显示在托盘区 
                    serviceicon.Visible = false;
                }
                catch (Exception ex)
                {
                    new Log("GameService").Write(string.Format("停止线程并退出系统错误：{0}", ex.Message), true);
                }
                finally
                {
                    this.Dispose();
                    Application.Exit();
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
        /// <summary>
        /// 窗体事件-窗口最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameService_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮 
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标 
                this.ShowInTaskbar = false;
                //图标显示在托盘区 
                serviceicon.Visible = true;
            }
        }
        /// <summary>
        /// 窗体事件-窗体最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serviceicon_DoubleClick(object sender, EventArgs e)
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
                serviceicon.Visible = false;
            }
        }
        #endregion

        #region 自定义事件
        protected void Start()
        {
            serviceBase = new GameServiceBase(rich_Reptile_Log, rich_TicketReceiver_Log, rich_TicketOut_Log, rich_Award_Log, rich_Notice_Log);
            serviceBase.Start();
        }
        #endregion
    }
}
