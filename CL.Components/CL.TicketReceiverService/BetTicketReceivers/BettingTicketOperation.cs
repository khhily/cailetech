using CL.Game.BLL;
using CL.Game.BLL.View;
using CL.Tools.Common;
using CL.Tools.MSMQManager;
using CL.Tools.TicketInterface;
using CL.View.Entity.Game;
using CL.View.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Messaging;
using System.Windows.Forms;
using System.Xml;
using CL.Redis.BLL;
using CL.Game.Entity;
using System.Linq;
using CL.Enum.Common.Status;
using CL.Enum.Common.Type;

namespace CL.TicketReceiverService.BetTicketReceivers
{
    public class BettingTicketOperation : RecTicketBase
    {
        private MessageQueue m_Queue;
        private int m_timespan = 30;
        private readonly Log m_log = new Log("BettingTicketOperation");
        private string QueueName = string.Empty;
        private XmlDocument doc = new XmlDocument();
        private SchemesBLL blls = new SchemesBLL();
        private SchemeETicketsBLL bll = new SchemeETicketsBLL();

        public BettingTicketOperation(RichTextBox _infoBox) : base(_infoBox)
        {
            try
            {
                InitializeConfiguration();
                m_Queue = GetMessageQueue(QueueName);
                m_Queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(BettingTicket) });
            }
            catch (Exception ex)
            {
                LogInfo("接口配置初始化失败：" + ex.Message);
                m_log.Write("接口配置初始化失败：" + ex.Message, true);
            }
        }

        /// <summary>
        /// 获取消息队列
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private MessageQueue GetMessageQueue(string path)
        {
            MessageQueue queue = null;
            if (MessageQueue.Exists(path))
            {
                queue = new MessageQueue(path);
            }
            else
            {
                queue = MessageQueue.Create(path);  //创建队列
                queue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl); //设置权限
            }
            return queue;
        }

        /// <summary>
        /// 读取消息队列
        /// </summary>
        public void Run()
        {
            try
            {
                LogInfo("读取小票投注队列:" + QueueName);
                using (MessageEnumerator messagesEnumerator = m_Queue.GetMessageEnumerator2())
                {
                    bool IsBettingRevoke = false;
                    while (messagesEnumerator.MoveNext(TimeSpan.FromDays(m_timespan))) //默认3天
                    {
                        var messageA = messagesEnumerator.Current;
                        if (messageA == null) return;
                        m_Queue.MessageReadPropertyFilter.Priority = true;
                        System.Messaging.Message message = m_Queue.PeekById(messageA.Id);
                        var order = message.Body as BettingTicket;
                        if (order != null)
                        {
                            if (order.InterfaceConfig == null)
                            {
                                LogInfo("发送的XML数据信息失败");
                                m_log.Write("发送的XML数据信息失败");
                                IsBettingRevoke = true;
                            }
                            else
                            {
                                #region
                                try
                                {
                                    XmlNode xml = order.InterfaceConfig;
                                    InterfaceBase InterTicker = new InterfaceBase()[xml];
                                    if (InterTicker == null)
                                    {
                                        LogInfo("初始化接口失败");
                                        m_log.Write("初始化接口失败");
                                        IsBettingRevoke = true;
                                    }
                                    else
                                    {
                                        List<udv_BettingEntites> TicketNoticeList = new List<udv_BettingEntites>();
                                        List<udv_ResultBetting> ResModel = new List<udv_ResultBetting>();
                                        bool bWithdrawal = false;
                                        string ErrorMsg = string.Empty;

                                        //电子票投注 由单个方案ID进行投注
                                        #region 电子票投注
                                        List<udv_ParaBettingTicker> para = new List<udv_ParaBettingTicker>();
                                        List<udv_BettingTickets> TricketList = new SchemesBLL().QueryDataList(order.SchemeID, order.ChaseTaskDetailsID);

                                        foreach (udv_BettingTickets item in TricketList)
                                        {
                                            udv_ParaBettingTicker model = new udv_ParaBettingTicker()
                                            {
                                                TicketUser = string.IsNullOrEmpty(item.TicketUser) == true ? "彩乐彩票" : item.TicketUser,
                                                Identify = string.IsNullOrEmpty(item.Identify) == true ? "020852004750000000" : item.Identify,
                                                Phone = item.Phone,
                                                Email = item.Email,
                                                SchemeETicketID = item.SchemeETicketsID,
                                                LotteryCode = item.LotteryCode,
                                                Issue = item.IsuseName,
                                                PlayType = item.PlayCode,
                                                Number = item.Number,
                                                Multiple = item.Multiple,
                                                Bet = item.Bet,
                                                Amount = item.Amount
                                            };
                                            para.Add(model);
                                        }
                                        ResModel = InterTicker.HandleTicketBetting(para);
                                        #endregion

                                        #region 查询电子票投注状态
                                        foreach (udv_ResultBetting ResItem in ResModel)
                                        {
                                            if (ResItem.ErrorCode == "0")
                                            {
                                                TicketNoticeList.AddRange(ResItem.ListTicker);
                                            }
                                            else    //整个方案号通过接口投注返回失败进行撤单
                                            {
                                                //bWithdrawal = true;
                                                ErrorMsg = ResItem.ErrorMsg;
                                            }

                                            String StrLog = String.Format("方案[{0}]调用[{1}]接口处理方案 返回 -->状态码：{2}信息：{3}", order.SchemeID, xml.SelectSingleNode("InterfaceType").InnerText, ResItem.ErrorCode, ResItem.ErrorMsg);
                                            LogInfo(StrLog);
#if DEBUG
                                            m_log.Write(StrLog);
#endif
                                        }
                                        #endregion
                                        #region 更新电子票状态信息
                                        if (TicketNoticeList.Count > 0)
                                        {
                                            if (blls.Exists(order.SchemeID))
                                            {
                                                bWithdrawal = bll.TicketBetting(TicketNoticeList, order.SchemeID, order.ChaseTaskDetailsID);
                                            }
                                        }
                                        var SchemeEntity = new SchemesBLL().QueryEntity(order.SchemeID);
                                        if (!bWithdrawal)//整个方案号投注失败进行撤单
                                        {
                                            bll.SchemeETicketsWithdrawal(order.SchemeID, ErrorMsg);
                                            //redis撤单
                                            if (SchemeEntity != null)
                                            {
                                                if (SchemeEntity.BuyType == (byte)BuyTypeEnum.BuyChase)
                                                {
                                                    var ChaseDetailEntity = new ChaseTaskDetailsBLL().QueryEntity(order.ChaseTaskDetailsID);
                                                    if (ChaseDetailEntity != null)
                                                    {
                                                        ChaseDetailEntity.QuashStatus = (short)QuashStatusEnum.SysQuash;
                                                        new UsersBLL().ModifyUserBalanceRedis(SchemeEntity.InitiateUserID, ChaseDetailEntity.Amount, true);//撤单
                                                        string IssueName = new IsusesBLL().QueryIssueName(ChaseDetailEntity.IsuseID);
                                                        new UsersBLL().SendWinMoneyContent(order.SchemeID, IssueName, ChaseDetailEntity.Amount);//撤单推送
                                                        new ChaseTaskDetailsBLL().ModifyEntity(ChaseDetailEntity);  //更新
                                                    }
                                                    var ChaseDetailEntitys = new ChaseTaskDetailsBLL().QueryEntitysBySchemeID(order.SchemeID);
                                                    var IsExecuted = ChaseDetailEntitys.Where(w => w.IsExecuted == true).ToList();
                                                    if (IsExecuted.Count == ChaseDetailEntitys.Count)
                                                    {
                                                        SchemeEntity.SchemeStatus = (short)SchemeStatusEnum.ChaseSuccess;
                                                        new SchemesBLL().ModifyEntity(SchemeEntity);
                                                    }
                                                    else
                                                    {
                                                        IsExecuted = ChaseDetailEntitys.Where(w => w.IsExecuted == false).ToList();
                                                        var QuashStatus = ChaseDetailEntitys.Where(w => w.QuashStatus != (short)QuashStatusEnum.NoQuash).ToList();
                                                        if (IsExecuted.Count == QuashStatus.Count)
                                                        {
                                                            SchemeEntity.SchemeStatus = (short)SchemeStatusEnum.ChaseSuccess;
                                                            new SchemesBLL().ModifyEntity(SchemeEntity);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    SchemeEntity.SchemeStatus = (short)SchemeStatusEnum.OrderRevoke;
                                                    new UsersBLL().ModifyUserBalanceRedis(SchemeEntity.InitiateUserID, SchemeEntity.SchemeMoney, true);//撤单
                                                    new UsersBLL().SendWinMoneyContent(order.SchemeID, SchemeEntity.IsuseName, SchemeEntity.SchemeMoney);//撤单推送
                                                    new SchemesBLL().ModifyEntity(SchemeEntity);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            List<SchemeETicketsEntity> Entitys = new SchemeETicketsBLL().QueryEntitysBySchemeID(order.SchemeID);
                                            if (SchemeEntity != null)
                                            {
                                                var Entitys_3 = Entitys.Where(w => w.TicketStatus == (byte)TicketStatusEnum.BettingFailure).ToList();  //投注失败撤单
                                                if (Entitys_3.Count == Entitys.Count)
                                                {
                                                    if (SchemeEntity.BuyType == (byte)BuyTypeEnum.BuyChase)
                                                    {
                                                        var ChaseDetailEntity = new ChaseTaskDetailsBLL().QueryEntity(order.ChaseTaskDetailsID);
                                                        if (ChaseDetailEntity != null)
                                                        {
                                                            ChaseDetailEntity.QuashStatus = (short)QuashStatusEnum.SysQuash;
                                                            new UsersBLL().ModifyUserBalanceRedis(SchemeEntity.InitiateUserID, ChaseDetailEntity.Amount, true);//撤单

                                                            string IssueName = new IsusesBLL().QueryIssueName(ChaseDetailEntity.IsuseID);
                                                            new UsersBLL().SendWinMoneyContent(order.SchemeID, IssueName, ChaseDetailEntity.Amount);//撤单推送
                                                        }
                                                    }
                                                    else
                                                    {
                                                        SchemeEntity.SchemeStatus = (short)SchemeStatusEnum.OrderRevoke;
                                                        new UsersBLL().ModifyUserBalanceRedis(SchemeEntity.InitiateUserID, SchemeEntity.SchemeMoney, true);//撤单
                                                        new UsersBLL().SendWinMoneyContent(order.SchemeID, SchemeEntity.IsuseName, SchemeEntity.SchemeMoney);//撤单推送

                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        new SchemesBLL().OutApply(order.SchemeID, order.ChaseTaskDetailsID, true, DateTime.Now);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    m_log.Write(String.Format("{0}队列投注出错:\n{1}", QueueName, ex.Message), true);
                                    LogInfo(String.Format("{0}队列投注出错:\n{1}", QueueName, ex.Message));
                                    IsBettingRevoke = true;
                                }
                                #endregion
                            }
                            //出错撤单
                            if (IsBettingRevoke)
                            {
                                bll.SchemeETicketsWithdrawal(order.SchemeID, "");
                                IsBettingRevoke = false;
                            }
                        }
                        //投注是否成功 队列中的方案ID应该删除
                        m_Queue.ReceiveById(messageA.Id);
                    }
                }
                m_log.Write(m_timespan + "天内没有检测到订单 退出消息队列");
            }
            catch (MessageQueueException ex)
            {
                m_log.Write(String.Format("{0}队列出错:\n{1}", QueueName, ex.Message));
                LogInfo(String.Format("{0}队列出错:\n{1}", QueueName, ex.Message));
            }
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="sectionName"></param>
        public void InitializeConfiguration()
        {
            string sectionName = "BettingTicket";
            var iniFilePath = AppDomain.CurrentDomain.BaseDirectory + "MSMQManager.ini";
            var ini = new IniFile(iniFilePath);

            QueueName = ini.Read(sectionName, "QueuePath");
            m_timespan = Convert.ToInt32(ini.Read(sectionName, "QueueTimeBetting"));

            //接口配置
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "Interface.xml");
        }

    }
}
