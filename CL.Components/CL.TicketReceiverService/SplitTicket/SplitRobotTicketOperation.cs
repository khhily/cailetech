using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using CL.Tools.MSMQManager;
using System;
using System.Messaging;
using System.Windows.Forms;
using CL.Redis.BLL;
using CL.Enum.Common.Status;

namespace CL.TicketReceiverService.SplitTicket
{
    /// <summary>
    /// 机器人拆票队列
    /// </summary>
    public class SplitRobotTicketOperation : RecTicketBase
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
        private int queueExpireTime = 30;
        /// <summary>
        /// 实时投注队列路径
        /// </summary>
        private string QueueBettingRobotTicket = String.Empty;

        private readonly Log log = new Log("ElectronicRobotTicketReceiver");
        /// <summary>
        /// 请求停止状态
        /// </summary>
        private bool requestStop = false;
        #endregion
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="_infoBox"></param>
        public SplitRobotTicketOperation(RichTextBox _infoBox) : base(_infoBox)
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
            string sectionName = "ElectronicRobotTicket";
            var iniFilePath = AppDomain.CurrentDomain.BaseDirectory + "MSMQManager.ini";
            var ini = new IniFile(iniFilePath);
            if (!int.TryParse(ini.Read(sectionName, "QueueTimeBetting"), out queueExpireTime))
                queueExpireTime = 3;

            QueueBettingRobotTicket = ini.Read(sectionName, "QueueBetting");

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
        /// <summary>
        /// 入口
        /// </summary>
        public void Run()
        {
            LogInfo("读取机器人拆票队列:" + queueName);
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
            var SchModel = new SchemesBLL().QueryEntity(order.SchemeID);
            if (SchModel == null)
            {
                LogInfo("方案不存在，取消拆票 SchemeID:" + order.SchemeID);
                return;
            }
            //10.下单失败（限号），12.订单撤销
            if (SchModel.SchemeStatus == (int)SchemeStatusEnum.OrderFailure || SchModel.SchemeStatus == (int)SchemeStatusEnum.OrderRevoke)
            {
                LogInfo("订单下单失败或订单已经撤销，取消拆票 SchemeID:" + order.SchemeID);
                return;
            }

            LogInfo("小票队列：开始电子票拆票 SchemeID:" + order.SchemeID);
            bool isTrue = SplitLotteryNumber_Robot(order.SchemeID, order.LotteryCode, order.SchemeMoney / 100, order.TicketDetails, order.ChaseTaskDetailsID);
            LogInfo(string.Format("方案号:{0} 总金额：{1} 拆票 {2}", order.SchemeID, order.SchemeMoney / 100, isTrue ? "成功" : "失败"));
        }
    }
}
