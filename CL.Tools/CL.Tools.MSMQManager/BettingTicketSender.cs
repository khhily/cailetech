using CL.Tools.Common;
using System;
using System.Messaging;

namespace CL.Tools.MSMQManager
{
    /// <summary>
    /// 投注队列
    /// </summary>
    public class BettingTicketSender
    {
        private readonly BettingTicket ticket = new BettingTicket();    //电子票实体
        private readonly Log log = new Log("BettingTick");

        private readonly string queuePath = String.Empty;
        public BettingTicketSender(string _queuePath)
        {
            this.queuePath = _queuePath;
        }

        /// <summary>
        /// 电子票
        /// </summary>
        public BettingTicket SchemeTicket { get { return ticket; } }

        /// <summary>
        /// 创建队列
        /// </summary>
        /// <returns></returns>
        protected MessageQueue Create()
        {
            MessageQueue queue = null;
            try
            {
                if (MessageQueue.Exists(queuePath))  //判断是否存在队列
                {
                    queue = new MessageQueue(queuePath);
                }
                else
                {
                    queue = MessageQueue.Create(queuePath); //创建队列
                    queue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl); //设置权限
                }
            }
            catch (Exception ex)
            {
                log.Write(String.Format("{0}消息队列创建错误！\n{1}", queuePath, ex), true);
            }
            return queue;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        public bool Sender()
        {
            try
            {
                using (var queue = Create())
                {
                    using (var message = new Message(ticket) { Recoverable = true, Priority = MessagePriority.High })
                    {
                        queue.Send(message, String.Format("SchemeID {0}", ticket.SchemeID)); //发送消息
                    }
                }
#if DEBUG
                log.Write(String.Format("BettingTicket 提交成功! SchemeID:{0}", ticket.SchemeID));
#endif
                return true;
            }
            catch (MessageQueueException ex)
            {
                log.Write(String.Format("BettingTicket 提交失败! SchemeID:{0}\n{1}", ticket.SchemeID, ex.Message));
                return false;
            }
        }
    }
}
