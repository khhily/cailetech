using CL.LotteryGameService.Award;
using CL.LotteryGameService.Model;
using CL.LotteryGameService.Notice;
using CL.LotteryGameService.Reptile;
using CL.LotteryGameService.TicketOut;
using CL.LotteryGameService.TicketReceiver;
using CL.Tools.Common;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CL.LotteryGameService
{
    public class GameServiceBase
    {
        protected RichTextBox Rich_Reptile;
        protected RichTextBox Rich_TicketReceiver;
        protected RichTextBox Rich_TicketOut;
        protected RichTextBox Rich_Award;
        protected RichTextBox Rich_Notice;
        protected Thread th_reptile;
        protected Thread th_receiver;
        protected Thread th_out;
        protected Thread th_award;
        protected Thread th_notice;
        protected Thread th_clearbag;
        protected const string LogName = "GameServiceBase";

        private QuartzHelper quartzhelper = new QuartzHelper();
        protected static string IntervalTime_GC = ConfigurationManager.AppSettings["quartz_gc"] ?? "0 0/30 * * * ?";

        public static bool IsRun = false;

        public GameServiceBase(RichTextBox reptile, RichTextBox ticketReceiver, RichTextBox ticketOut, RichTextBox award, RichTextBox notice)
        {
            new WritText(reptile).WritTextBox("系统初始化");
            Rich_Reptile = reptile;
            Rich_TicketReceiver = ticketReceiver;
            Rich_TicketOut = ticketOut;
            Rich_Award = award;
            Rich_Notice = notice;
            IsRun = true;
        }

        #region 启动项
        /// <summary>
        /// 系统启动方法
        /// </summary>
        public void Start()
        {
            try
            {
                th_reptile = new Thread(new ThreadStart(Start_Reptile));
                th_reptile.Name = string.Format("期号抓取服务");
                th_reptile.Start();
                th_receiver = new Thread(new ThreadStart(Start_TicketReceiver));
                th_receiver.Name = string.Format("方案拆票服务");
                th_receiver.Start();
                th_out = new Thread(new ThreadStart(Start_TicketOut));
                th_out.Name = string.Format("方案出票服务");
                th_out.Start();
                th_award = new Thread(new ThreadStart(Start_Award));
                th_award.Name = string.Format("算奖派奖服务");
                th_award.Start();
                th_notice = new Thread(new ThreadStart(Start_Notice));
                th_notice.Name = string.Format("游戏通知服务");
                th_notice.Start();
                th_clearbag = new Thread(new ThreadStart(Quartz_ClearBag));
                th_clearbag.Name = string.Format("定时清理服务");
                th_clearbag.Start();
            }
            catch (Exception ex)
            {
                new Log(LogName).Write(string.Format("[Start]系统总线程启动失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[Start]系统总线程启动失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 启动期号抓取
        /// </summary>
        protected void Start_Reptile()
        {
            try
            {
                new WritText(Rich_Reptile).WritTextBox("启动期号抓取");
                new Handle_Reptile().Run(Rich_Reptile);
            }
            catch (Exception ex)
            {
                new WritText(Rich_Reptile).WritTextBox(string.Format("[Start_Reptile]期号抓取启动失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[Start_Reptile]期号抓取启动失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[Start_Reptile]期号抓取启动失败：{0}", ex.Message));
            }
        }


        /// <summary>
        /// 启动方案拆票
        /// </summary>
        protected void Start_TicketReceiver()
        {
            try
            {
                new WritText(Rich_TicketReceiver).WritTextBox("启动方案拆票");
                new Handle_TicketReceiver().Run(Rich_TicketReceiver);
            }
            catch (Exception ex)
            {
                new WritText(Rich_TicketReceiver).WritTextBox(string.Format("[Start_TicketReceiver]方案拆票启动失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[Start_TicketReceiver]方案拆票启动失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[Start_TicketReceiver]方案拆票启动失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 启动方案出票
        /// </summary>
        protected void Start_TicketOut()
        {
            try
            {
                new WritText(Rich_TicketOut).WritTextBox("启动方案出票");
                new Handle_TicketOut().Run(Rich_TicketOut);
            }
            catch (Exception ex)
            {
                new WritText(Rich_TicketOut).WritTextBox(string.Format("[Start_TicketOut]方案出票启动失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[Start_TicketOut]方案出票启动失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[Start_TicketOut]方案出票启动失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 启动算奖派奖
        /// </summary>
        protected void Start_Award()
        {
            try
            {
                new WritText(Rich_Award).WritTextBox("启动算奖派奖");
                new Handle_Award().Run(Rich_Award);
            }
            catch (Exception ex)
            {
                new WritText(Rich_Award).WritTextBox(string.Format("[Start_Award]算奖派奖启动失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[Start_Award]算奖派奖启动失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[Start_Award]算奖派奖启动失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 启动系统通知
        /// </summary>
        protected void Start_Notice()
        {
            try
            {
                new WritText(Rich_Notice).WritTextBox("启动系统通知");
                new Handle_Notice().Run(Rich_Notice);
            }
            catch (Exception ex)
            {
                new WritText(Rich_Notice).WritTextBox(string.Format("[Start_Notice]系统通知启动失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[Start_Notice]系统通知启动失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[Start_Notice]系统通知启动失败：{0}", ex.Message));
            }
        }
        #endregion

        #region 重启项
        /// <summary>
        /// 系统重启方法
        /// </summary>
        public void ReStart()
        {
            try
            {
                th_reptile.Start();
                th_receiver.Start();
                th_out.Start();
                th_award.Start();
                th_notice.Start();
            }
            catch (ThreadStartException ex)
            {
                new Log(LogName).Write(string.Format("[ReStart]系统重启失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[ReStart]系统重启失败：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 重启期号抓取
        /// </summary>
        public void ReStart_Reptile()
        {
            try
            {
                new WritText(Rich_Reptile).WritTextBox("期号抓取重启");
                th_reptile.Start();
            }
            catch (Exception ex)
            {
                new WritText(Rich_Reptile).WritTextBox(string.Format("[ReStart_Reptile]期号抓取启动失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[ReStart_Reptile]期号抓取启动失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[ReStart_Reptile]期号抓取启动失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 重启方案拆票
        /// </summary>
        public void ReStart_TicketReceiver()
        {
            try
            {
                new WritText(Rich_TicketReceiver).WritTextBox("方案拆票重启");
                th_receiver.Start();
            }
            catch (Exception ex)
            {
                new WritText(Rich_TicketReceiver).WritTextBox(string.Format("[ReStart_TicketReceiver]方案拆票启动失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[ReStart_TicketReceiver]方案拆票启动失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[ReStart_TicketReceiver]方案拆票启动失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 重启方案出票
        /// </summary>
        public void ReStart_TicketOut()
        {
            try
            {
                new WritText(Rich_TicketOut).WritTextBox("方案出票重启");
                th_out.Start();
            }
            catch (Exception ex)
            {
                new WritText(Rich_TicketOut).WritTextBox(string.Format("[ReStart_TicketOut]方案出票启动失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[ReStart_TicketOut]方案出票启动失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[ReStart_TicketOut]方案出票启动失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 重启算奖派奖
        /// </summary>
        public void ReStart_Award()
        {
            try
            {
                new WritText(Rich_Award).WritTextBox("算奖派奖重启");
                th_award.Start();
            }
            catch (Exception ex)
            {
                new WritText(Rich_Award).WritTextBox(string.Format("[ReStart_Award]算奖派奖启动失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[ReStart_Award]算奖派奖启动失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[ReStart_Award]算奖派奖启动失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 重启系统通知
        /// </summary>
        public void ReStart_Notice()
        {
            try
            {
                new WritText(Rich_Notice).WritTextBox("系统通知重启");
                th_notice.Start();
            }
            catch (Exception ex)
            {
                new WritText(Rich_Notice).WritTextBox(string.Format("[ReStart_Notice]系统通知启动失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[ReStart_Notice]系统通知启动失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[ReStart_Notice]系统通知启动失败：{0}", ex.Message));
            }
        }
        #endregion

        #region 停止项
        /// <summary>
        /// 系统停止方法
        /// </summary>
        public void Stop()
        {
            try
            {
                th_reptile.Abort();
                th_receiver.Abort();
                th_out.Abort();
                th_award.Abort();
                th_notice.Abort();
            }
            catch (ThreadStartException ex)
            {
                new Log(LogName).Write(string.Format("[Stop]系统停止失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[Stop]系统停止失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 停止期号抓取
        /// </summary>
        public void Stop_Reptile()
        {
            try
            {
                new WritText(Rich_Reptile).WritTextBox("期号抓取停止");
                th_reptile.Abort();
            }
            catch (Exception ex)
            {
                new WritText(Rich_Reptile).WritTextBox(string.Format("[Stop_Reptile]期号抓取停止失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[Stop_Reptile]期号抓取停止失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[Stop_Reptile]期号抓取停止失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 停止方案拆票
        /// </summary>
        public void Stop_TicketReceiver()
        {
            try
            {
                new WritText(Rich_TicketReceiver).WritTextBox("方案拆票停止");
                th_receiver.Abort();
            }
            catch (Exception ex)
            {
                new WritText(Rich_TicketReceiver).WritTextBox(string.Format("[Stop_TicketReceiver]方案拆票停止失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[Stop_TicketReceiver]方案拆票停止失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[Stop_TicketReceiver]方案拆票停止失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 停止方案出票
        /// </summary>
        public void Stop_TicketOut()
        {
            try
            {
                new WritText(Rich_TicketOut).WritTextBox("方案出票停止");
                th_out.Abort();
            }
            catch (Exception ex)
            {
                new WritText(Rich_TicketOut).WritTextBox(string.Format("[Stop_TicketOut]方案出票停止失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[Stop_TicketOut]方案出票停止失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[Stop_TicketOut]方案出票停止失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 停止算奖派奖
        /// </summary>
        public void Stop_Award()
        {
            try
            {
                new WritText(Rich_Award).WritTextBox("算奖派奖停止");
                th_award.Abort();
            }
            catch (Exception ex)
            {
                new WritText(Rich_Award).WritTextBox(string.Format("[Stop_Award]算奖派奖停止失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[Stop_Award]算奖派奖停止失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[Stop_Award]算奖派奖停止失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 停止系统通知
        /// </summary>
        public void Stop_Notice()
        {
            try
            {
                new WritText(Rich_Notice).WritTextBox("系统通知停止");
                th_notice.Abort();
            }
            catch (Exception ex)
            {
                new WritText(Rich_Notice).WritTextBox(string.Format("[Stop_Notice]系统通知停止失败：{0}", ex.Message));
                new Log(LogName).Write(string.Format("[Stop_Notice]系统通知停止失败：{0}", ex.Message), true);
                MessageBox.Show(string.Format("[Stop_Notice]系统通知停止失败：{0}", ex.Message));
            }
        }
        #endregion

        #region 定时器
        /// <summary>
        /// 垃圾定时清理
        /// </summary>
        protected void Quartz_ClearBag()
        {
            try
            {
                string jobname_gc = "jobname_gc";
                string jobgroup_gc = "jobgroup_gc";
                quartzhelper.AddTrigger(jobname_gc, jobgroup_gc, IntervalTime_GC, new Action(() =>
                {
                    try
                    {
                        Task.Factory.StartNew(() =>
                        {
                            //期号抓取
                            ClearBag_Reptile();
                        });
                        Task.Factory.StartNew(() =>
                        {
                            //方案拆票
                            ClearBag_TicketReceiver();
                        });
                        Task.Factory.StartNew(() =>
                        {
                            //方案出票
                            ClearBag_TicketOut();
                        });
                        Task.Factory.StartNew(() =>
                        {
                            //算奖派奖
                            ClearBag_Award();
                        });
                        Task.Factory.StartNew(() =>
                        {
                            //游戏通知
                            ClearBag_Notice();
                        });
                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        new WritText(Rich_Reptile).WritTextBox(string.Format("日代理分成发生错误 --> {0}", ex.StackTrace));
                    }
                }));
                Task.Factory.StartNew(() =>
                {
                    quartzhelper.Start();
                });
            }
            catch (Exception ex)
            {
                new WritText(Rich_Reptile).WritTextBox(string.Format("垃圾定时清理失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 期号垃圾清理
        /// </summary>
        protected void ClearBag_Reptile()
        {
            try
            {
                var Rich_Content = Rich_Reptile.Text;
                Rich_Reptile.Clear();
                new Log("Reptile").Write(Rich_Content, true);
                new WritText(Rich_Reptile).WritTextBox(string.Format("以上内容定时清理,如需查看请在日志查看"));
            }
            catch (Exception ex)
            {
                new WritText(Rich_Reptile).WritTextBox(string.Format("[期号抓取]垃圾定时清理失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 拆票垃圾清理
        /// </summary>
        protected void ClearBag_TicketReceiver()
        {
            try
            {
                var Rich_Content = Rich_TicketReceiver.Text;
                Rich_TicketReceiver.Clear();
                new Log("TicketReceiver").Write(Rich_Content, true);
                new WritText(Rich_TicketReceiver).WritTextBox(string.Format("以上内容定时清理,如需查看请在日志查看"));
            }
            catch (Exception ex)
            {
                new WritText(Rich_TicketReceiver).WritTextBox(string.Format("[方案拆票]垃圾定时清理失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 出票垃圾清理
        /// </summary>
        protected void ClearBag_TicketOut()
        {
            try
            {
                var Rich_Content = Rich_TicketOut.Text;
                Rich_TicketOut.Clear();
                new Log("TicketOut").Write(Rich_Content, true);
                new WritText(Rich_TicketOut).WritTextBox(string.Format("以上内容定时清理,如需查看请在日志查看"));
            }
            catch (Exception ex)
            {
                new WritText(Rich_TicketOut).WritTextBox(string.Format("[方案出票]垃圾定时清理失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 算奖垃圾清理
        /// </summary>
        protected void ClearBag_Award()
        {
            try
            {
                var Rich_Content = Rich_Award.Text;
                Rich_Award.Clear();
                new Log("Award").Write(Rich_Content, true);
                new WritText(Rich_Award).WritTextBox(string.Format("以上内容定时清理,如需查看请在日志查看"));
            }
            catch (Exception ex)
            {
                new WritText(Rich_Award).WritTextBox(string.Format("[算奖派奖]垃圾定时清理失败：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 通知垃圾清理
        /// </summary>
        protected void ClearBag_Notice()
        {
            try
            {
                var Rich_Content = Rich_Notice.Text;
                Rich_Notice.Clear();
                new Log("Notice").Write(Rich_Content, true);
                new WritText(Rich_Notice).WritTextBox(string.Format("以上内容定时清理,如需查看请在日志查看"));
            }
            catch (Exception ex)
            {
                new WritText(Rich_Notice).WritTextBox(string.Format("[游戏通知]垃圾定时清理失败：{0}", ex.Message));
            }
        }
        #endregion
    }
}
