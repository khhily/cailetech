using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using CL.Tools.LotterySplitTickets;
using CL.View.Entity.ElectronicTicket;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CL.TicketReceiverService
{
    public class RecTicketBase
    {
        private readonly Log log = new Log("TicketBase");
        /// <summary>
        /// 彩种接口
        /// </summary>
        public static readonly Dictionary<Int32, XmlNode> SupplierProvider = new Dictionary<Int32, XmlNode>(9999);

        private RichTextBox InfoRichTextBox;

        public RecTicketBase(RichTextBox _infoBox)
        {
            InfoRichTextBox = _infoBox;
            InfoRichTextBox.ForeColor = System.Drawing.Color.Green;
            

            RegisterLotteryDupplier();
        }

        public void LogInfo(String str)
        {
            InfoRichTextBox.Invoke(new Action(() =>
            {
                string val = String.Format("{0}->{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), str);
                InfoRichTextBox.AppendText(val);
                InfoRichTextBox.SelectionStart = InfoRichTextBox.Text.Length;
                InfoRichTextBox.ScrollToCaret();
            }));
        }

        /// <summary>
        /// 彩种接口类型配置
        /// </summary>
        private void RegisterLotteryDupplier()
        {
            if (SupplierProvider.Count > 0) return;

            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "Interface.xml");
            XmlNodeList XmlList = doc.SelectNodes("//EntryModel/Item");
            foreach (XmlNode item in XmlList)
            {
                try
                {
                    int LotteryCode = Convert.ToInt32(item.SelectSingleNode("SystemLotteryCode").InnerText);
                    SupplierProvider.Add(LotteryCode, item);
                }
                catch (Exception ex)
                {
                    log.Write("初始化错误，接口配置不存在" + ex, true);
                    throw;
                }
            }
        }

        /// <summary>
        /// 获取消息队列
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public MessageQueue GetMessageQueue(string path, bool isRemote)
        {
            MessageQueue queue = null;
            //创建队列
            if (isRemote)
            {
                queue = new MessageQueue(path);
            }
            else
            {
                if (MessageQueue.Exists(path))
                {
                    queue = new MessageQueue(path);
                }
                else
                {
                    queue = MessageQueue.Create(path);
                }
            }
            queue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl); //设置权限
            return queue;
        }

        /// <summary>
        /// 拆票
        /// </summary>
        /// <param name="schemeId"></param>
        /// <param name="lotteryCode"></param>
        /// <param name="isuseName"></param>
        /// <param name="schemeMoney"></param>
        /// <param name="ticketList"></param>
        /// <returns></returns>
        public bool SplitLotteryNumber(long SchemeID, int lotteryCode, long schemeMoney, List<udv_Parameter> ticketList, long ChaseTaskDetailsID)
        {
            string strlog = string.Empty;
            try
            {
                ICollection<udv_Ticket> tickets = null;
                TickBuilder bulider = TickBuilder.Create(SupplierProvider[lotteryCode].SelectSingleNode("InterfaceType").InnerText, lotteryCode);
                tickets = bulider.Generate(SchemeID, lotteryCode, ticketList);

                if (tickets == null || tickets.Count == 0)
                {
                    strlog = "方案ID:" + SchemeID + " 电子票拆分错误,进行撤单";
                    LogInfo(strlog);
                    log.Write(strlog);
                    return false;
                }
                if (Math.Abs(tickets.Sum(x => x.Money) - schemeMoney) < 0)   //金额错误，进行撤单
                {
                    strlog = "方案ID:" + SchemeID + " 金额拆分错误 方案金额" + schemeMoney + " 电子票总金额" + tickets.Sum(x => x.Money) + ",进行撤单";
                    LogInfo(strlog);
                    log.Write(strlog);
                    return false;
                }
                bool success = false;

                #region 电子票入库处理

                #region 写日志
#if DEBUG
                Task.Factory.StartNew(() =>
                {
                    foreach (udv_Ticket item in tickets)
                    {
                        log.Write(String.Format("玩法编码：{0}，投注号码：{1}，注数：{2}，倍数：{3}，金额：{4}，方案：{5}，追号：{6}", item.PlayTypeCode, item.Number, item.Bet, item.Multiple, item.Money, SchemeID, ChaseTaskDetailsID));
                    }
                });
#endif
                #endregion

                List<udv_Ticket> SqlPara = new List<udv_Ticket>();
                SqlPara.AddRange(tickets);

                if (SqlPara.Count == 0) return false;
                SchemesBLL blls = new SchemesBLL();
                if (!blls.Exists(SchemeID)) return false;

                if (SqlPara != null)
                {
                    if (SqlPara.Count <= 100)
                        success = new SchemeETicketsBLL().TicketsStorage(SchemeID, ChaseTaskDetailsID, SqlPara);
                    else
                    {
                        int PageSize = 100;
                        int Pages = (SqlPara.Count / PageSize) + ((SqlPara.Count % PageSize) > 0 ? 1 : 0);
                        for (int i = 0; i < Pages; i++)
                        {
                            success = new SchemeETicketsBLL().TicketsStorage(SchemeID, ChaseTaskDetailsID, SqlPara.Skip(i * PageSize).Take(PageSize).ToList());
                        }
                    }
                }
                #endregion

                return success;
            }
            catch (Exception ex)
            {
                strlog = "拆票出现异常 " + ex;
                LogInfo(strlog);
                log.Write(strlog, true);
                return false;
            }
        }
        /// <summary>
        /// 拆票
        /// </summary>
        /// <param name="schemeId"></param>
        /// <param name="lotteryCode"></param>
        /// <param name="isuseName"></param>
        /// <param name="schemeMoney"></param>
        /// <param name="ticketList"></param>
        /// <returns></returns>
        public bool SplitLotteryNumber_Robot(long SchemeID, int lotteryCode, long schemeMoney, List<udv_Parameter> ticketList, long ChaseTaskDetailsID)
        {
            string strlog = string.Empty;
            try
            {
                ICollection<udv_Ticket> tickets = null;
                TickBuilder bulider = TickBuilder.Create(SupplierProvider[lotteryCode].SelectSingleNode("InterfaceType").InnerText, lotteryCode);
                tickets = bulider.Generate(SchemeID, lotteryCode, ticketList);

                if (tickets == null || tickets.Count == 0)
                {
                    strlog = "方案ID:" + SchemeID + " 电子票拆分错误,进行撤单";
                    LogInfo(strlog);
                    log.Write(strlog);
                    return false;
                }
                if (Math.Abs(tickets.Sum(x => x.Money) - schemeMoney) < 0)   //金额错误，进行撤单
                {
                    strlog = "方案ID:" + SchemeID + " 金额拆分错误 方案金额" + schemeMoney + " 电子票总金额" + tickets.Sum(x => x.Money) + ",进行撤单";
                    LogInfo(strlog);
                    log.Write(strlog);
                    return false;
                }
                bool success = false;

                #region 电子票入库处理

                #region 写日志
#if DEBUG
                Task.Factory.StartNew(() =>
                {
                    foreach (udv_Ticket item in tickets)
                    {
                        log.Write(String.Format("玩法编码：{0}，投注号码：{1}，注数：{2}，倍数：{3}，金额：{4}", item.PlayTypeCode, item.Number, item.Bet, item.Multiple, item.Money));
                    }
                });
#endif
                #endregion

                List<udv_Ticket> SqlPara = new List<udv_Ticket>();
                SqlPara.AddRange(tickets);

                if (SqlPara.Count == 0) return false;
                SchemesBLL blls = new SchemesBLL();
                if (!blls.Exists(SchemeID)) return false;
                success = new SchemeETicketsBLL().TicketsStorage_Robot(SchemeID, ChaseTaskDetailsID, SqlPara);
                #endregion

                return success;
            }
            catch (Exception ex)
            {
                strlog = "拆票出现异常 " + ex;
                LogInfo(strlog);
                log.Write(strlog, true);
                return false;
            }
        }

        /// <summary>
        /// 验证当期是否开售
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <returns></returns>
        public bool IsIsuseSale(int LotteryCode, string IsuseName)
        {
            IsusesEntity model = new IsusesBLL().QueryEntitysByLotteryCode(LotteryCode, IsuseName);
            if (model == null)
                return false;
            if (model.IsuseState == 0 || model.IsuseState == 1)
                return true;
            return false;
        }

        /// <summary>
        /// 撤单处理
        /// </summary>
        /// <param name="SchemeID">方案号</param>
        /// <param name="IsSystemQuash">是否系统撤单</param>
        /// <returns></returns>
        public bool P_QuashScheme(long SchemeID, bool IsSystemQuash)
        {
            string ReturnDescription = "";
            bool CallResult = new SchemeETicketsBLL().QuashScheme(SchemeID, IsSystemQuash, ref ReturnDescription);
            if (!CallResult)
            {
                log.Write("方案ID：" + SchemeID + "撤单处理失败");
                return false;
            }
            if (CallResult)
            {
                log.Write("方案ID：" + SchemeID + "撤单处理成功");
                return true;
            }
            log.Write("方案ID：" + SchemeID + "撤单处理，撤单描述：" + ReturnDescription);
            return true;
        }
    }
}
