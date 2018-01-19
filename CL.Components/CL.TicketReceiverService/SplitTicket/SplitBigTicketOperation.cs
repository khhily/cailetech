using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using CL.Tools.MSMQManager;
using System;
using System.Messaging;
using System.Windows.Forms;
using CL.Redis.BLL;
using CL.Enum.Common.Status;
using CL.Enum.Common.Type;

namespace CL.TicketReceiverService.SplitTicket
{
    public class SplitBigTicketOperation : RecTicketBase
    {
        #region 配置变量
        /// <summary>
        /// 消息队列
        /// </summary>
        private MessageQueue messageQueue = null;
        /// <summary>
        /// 消息队列名称
        /// </summary>
        private string queueName = String.Empty;
        /// <summary>
        /// 是否远程队列
        /// </summary>
        private bool isRemote = false;
        /// <summary>
        /// 消息队列过期时间
        /// </summary>
        private int queueExpireTime = 3;
        /// <summary>
        /// 实时投注队列路径
        /// </summary>
        private string QueueBettingBigticket = String.Empty;

        private readonly Log log = new Log("ElectronicBigTicketReceiver");
        /// <summary>
        /// 请求停止状态
        /// </summary>
        private bool requestStop = false;
        #endregion

        public SplitBigTicketOperation(RichTextBox _infoBox) : base(_infoBox)
        {
            try
            {
                Initilize();
                messageQueue = GetMessageQueue(queueName, isRemote);
                messageQueue.Formatter = new XmlMessageFormatter(new[] { typeof(ElectronicTicket) });
            }
            catch (Exception ex)
            {
                requestStop = true;
                log.Write("初始化配置发生异常 " + ex, true);
                LogInfo("初始化配置发生异常 " + ex);
            }
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private void Initilize()
        {
            string sectionName = "ElectronicBigTicket";
            var iniFilePath = AppDomain.CurrentDomain.BaseDirectory + "MSMQManager.ini";
            var ini = new IniFile(iniFilePath);
            if (!int.TryParse(ini.Read(sectionName, "QueueTimeBetting"), out queueExpireTime))
                queueExpireTime = 3;

            QueueBettingBigticket = ini.Read(sectionName, "QueueBetting");

            isRemote = Utils.StrToBool(ini.Read(sectionName, "IsRemote"), false);
            if (!isRemote)    //判断是否远程发送队列
            {
                queueName = ini.Read(sectionName, "LocalPath");
            }
            else
            {
                queueName = ini.Read(sectionName, "RemotePath");
            }
        }

        public void Run()
        {
            LogInfo("读取大票拆票队列:" + queueName);
            using (MessageEnumerator messageEnumerator = messageQueue.GetMessageEnumerator2())
            {
                while (true)
                {
                    try
                    {
                        PeekMessage(messageEnumerator);
                        messageEnumerator.Reset();
                        LogInfo("处理完成");
                    }
                    catch (Exception ex)
                    {
                        //把当前的方案移除
                        messageQueue.ReceiveById(messageEnumerator.Current.Id);
                        log.Write(String.Format("消息队列读取发送异常，{1}", ex), true);
                        LogInfo(String.Format("消息队列读取发送异常，{1}", ex));
                    }
                }
            }
        }

        /// <summary>
        /// 读取消息队列
        /// </summary>
        private void PeekMessage(MessageEnumerator messageEnumerator)
        {
            while (messageEnumerator.MoveNext(TimeSpan.FromDays(queueExpireTime))) //默认3天
            {
                if (requestStop)
                {
                    LogInfo("请求终止");
                    return;
                }
                var current = messageEnumerator.Current;
                if (current == null) continue;
                System.Messaging.Message message = null;
                message = messageQueue.PeekById(current.Id);
                if (message == null) continue;
                var order = (ElectronicTicket)message.Body;
                try
                {
                    //处理拆票
                    ActionStart(order);
                }
                catch (Exception ex)
                {
                    LogInfo("拆票出现异常，撤单 " + ex);
                    log.Write("拆票出现异常，撤单 " + ex, true);
                    //拆票是否成功 队列中的方案ID应该删除
                    messageQueue.ReceiveById(current.Id);
                    //messageEnumerator.Reset();
                    //拆票失败撤单
                    P_QuashScheme(order.SchemeID, true);
                    new SchemesBLL().RevokeRedisSchemeEntity(order.SchemeID, order.ChaseTaskDetailsID);
                    continue;
                }
                messageQueue.ReceiveById(current.Id);
            }
        }

        /// <summary>
        /// 开始处理拆票
        /// </summary>
        /// <param name="order"></param>
        private void ActionStart(ElectronicTicket order)
        {
            SchemesBLL bll = new SchemesBLL();
            var SchModel = new SchemesBLL().QueryEntity(order.SchemeID);
            if (SchModel == null)
            {
                LogInfo("方案不存在，取消拆票 SchemeID:" + order.SchemeID);
                return;
            }
            var LotteryEntity = new LotteriesBLL().QueryEntity(SchModel.LotteryCode);
            //10.下单失败（限号），12.订单撤销
            if (SchModel.SchemeStatus == (int)SchemeStatusEnum.OrderFailure || SchModel.SchemeStatus == (int)SchemeStatusEnum.OrderRevoke)
            {
                LogInfo("订单下单失败或订单已经撤销，取消拆票 SchemeID:" + order.SchemeID);
                return;
            }
            LogInfo("大票队列：开始电子票拆票 SchemeID:" + order.SchemeID);
            bool isTrue = SplitLotteryNumber(order.SchemeID, order.LotteryCode, order.SchemeMoney / 100, order.TicketDetails, order.ChaseTaskDetailsID);
            LogInfo(string.Format("方案号:{0} 总金额：{1} 拆票 {2}", order.SchemeID, order.SchemeMoney / 100, isTrue ? "成功" : "失败"));
            if (LotteryEntity.PrintOutType == 0)
            {
                #region 内容
                if (SchModel.BuyType == (byte)BuyTypeEnum.BuyChase)
                {
                    //验证
                    var ChaseTaskDetailsEntity = new ChaseTaskDetailsBLL().QueryEntity(order.ChaseTaskDetailsID);
                    if (!(ChaseTaskDetailsEntity.IsSendOut ?? false))
                    {
                        new ChaseTaskDetailsBLL().ModifySendOut(order.ChaseTaskDetailsID);
                    }
                    else
                    {
                        LogInfo(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                        log.Write(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                    }
                }
                else
                {
                    if (!SchModel.IsSendOut)
                    {
                        //更新为发送状态，防止重新发送出票
                        bll.ModifySendOut(order.SchemeID);
                    }
                    else
                    {
                        LogInfo(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                        log.Write(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                    }
                }
                #endregion
                new SchemesBLL().OutApply(order.SchemeID, order.ChaseTaskDetailsID, true, DateTime.Now);
                LogInfo("出票完成：本地电子票 SchemeID:" + order.SchemeID);
                return;
            }
            if (!isTrue)
            {
                //拆票失败撤单
                P_QuashScheme(order.SchemeID, true);
                bll.RevokeRedisSchemeEntity(SchModel.SchemeID, order.ChaseTaskDetailsID);
            }
            else //成功发送消息到出票队列
            {
                if (IsIsuseSale(order.LotteryCode, order.IsuseName))
                {
                    #region 发送消息至出票队列
                    BettingTicketSender TicketSender = null;
                    //判断拆分大小 以便通知队列
                    TicketSender = new BettingTicketSender(QueueBettingBigticket);

                    //是否已经发送到出票队列
                    if (SchModel.BuyType == (byte)BuyTypeEnum.BuyChase)
                    {
                        //验证
                        var ChaseTaskDetailsEntity = new ChaseTaskDetailsBLL().QueryEntity(order.ChaseTaskDetailsID);
                        if (!(ChaseTaskDetailsEntity.IsSendOut ?? false))
                        {
                            TicketSender.SchemeTicket.SchemeID = order.SchemeID;
                            TicketSender.SchemeTicket.ChaseTaskDetailsID = order.ChaseTaskDetailsID;
                            TicketSender.SchemeTicket.Status = 0;
                            TicketSender.SchemeTicket.InterfaceConfig = SupplierProvider[order.LotteryCode];
                            //发送到出票队列发生错误的时候进行撤单
                            if (!TicketSender.Sender())
                            {
                                P_QuashScheme(order.SchemeID, true);
                                bll.RevokeRedisSchemeEntity(order.SchemeID, order.ChaseTaskDetailsID);
                                log.Write("发送到出票队列发送错误，完成撤单");
                                LogInfo("发送到出票队列发送错误，完成撤单");
                            }
                            new ChaseTaskDetailsBLL().ModifySendOut(order.ChaseTaskDetailsID);
                        }
                        else
                        {
                            LogInfo(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                            log.Write(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                        }
                    }
                    else
                    {
                        if (!SchModel.IsSendOut)
                        {
                            TicketSender.SchemeTicket.SchemeID = order.SchemeID;
                            TicketSender.SchemeTicket.ChaseTaskDetailsID = order.ChaseTaskDetailsID;
                            TicketSender.SchemeTicket.Status = 0;
                            TicketSender.SchemeTicket.InterfaceConfig = SupplierProvider[order.LotteryCode];
                            //发送到出票队列发生错误的时候进行撤单
                            if (!TicketSender.Sender())
                            {
                                P_QuashScheme(order.SchemeID, true);
                                bll.RevokeRedisSchemeEntity(order.SchemeID, order.ChaseTaskDetailsID);
                                log.Write("发送到出票队列发送错误，完成撤单");
                                LogInfo("发送到出票队列发送错误，完成撤单");
                            }
                            //更新为发送状态，防止重新发送出票
                            bll.ModifySendOut(order.SchemeID);
                        }
                        else
                        {
                            LogInfo(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                            log.Write(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                        }
                    }
                    #endregion
                }
                else
                {
                    P_QuashScheme(order.SchemeID, true);
                    bll.RevokeRedisSchemeEntity(SchModel.SchemeID, order.ChaseTaskDetailsID);
                    LogInfo(string.Format("期号：{0} 已经结束，取消发送到实时出票队列，方案ID：{1} ", order.IsuseName, order.SchemeID));
                    log.Write(string.Format("期号：{0} 已经结束，取消发送到实时出票队列，方案ID：{1} ", order.IsuseName, order.SchemeID));
                }
            }
        }
    }
}
