using CL.Tools.Common;
using System;
using System.Linq;
using System.Messaging;

namespace CL.Tools.MSMQManager
{
    /// <summary>
    /// 拆票队列
    /// </summary>
    public class ElectronicTicketSender
    {
        private readonly ElectronicTicket ticket = new ElectronicTicket();
        private readonly Log log = new Log("MSMQManager");

        public ElectronicTicketSender() { }

        public void GetInfo(string type)
        {
            var ini = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "\\MSMQManager.ini");
            try
            {
                ticket.isRemote = Utils.StrToBool(ini.Read(type, "IsRemote"), false);
                if (!ticket.isRemote)    //判断是否远程发送队列
                {
                    ticket.ElectronicTicketQueueName = ini.Read(type, "LocalPath");
                }
                else
                {
                    ticket.ElectronicTicketQueueName = ini.Read(type, "RemotePath");
                }
            }
            catch (Exception ex)
            {
                log.Write("ElectronicTicket读取配置路径出错：" + ex.Message, true);
            }
        }

        /// <summary>
        /// 电子票信息
        /// </summary>
        public ElectronicTicket SchemeTicket
        {
            get { return ticket; }
        }

        /// <summary>
        /// 创建队列
        /// </summary>
        /// <returns></returns>
        protected MessageQueue Create()
        {
            MessageQueue queue = null;
            try
            {
                //创建队列
                if (ticket.isRemote)
                {
                    queue = new MessageQueue(ticket.ElectronicTicketQueueName);
                }
                else
                {
                    if (MessageQueue.Exists(ticket.ElectronicTicketQueueName))
                        queue = new MessageQueue(ticket.ElectronicTicketQueueName);
                    else
                    {
                        queue = MessageQueue.Create(ticket.ElectronicTicketQueueName);
                    }
                }
                queue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl); //设置权限
            }
            catch (Exception ex)
            {
                log.Write("消息队列创建错误！" + ex, true);
            }
            return queue;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        public bool Sender()
        {
            if (ticket == null)
                return false;

            int SumMultiple = ticket.TicketDetails.Sum(x => x.Multiple);
            long SumAmount = ticket.SchemeMoney; //ticket.TicketDetails.Sum(x => x.amount);
            string sectionName = "ElectronicTicket";
            if (SumMultiple > 500 || SumAmount > 2000000)    //大于500倍数或大于2000000金额的进入大票队列
                sectionName = "ElectronicBigTicket";
            if(ticket.IsRobot) //判断是否机器人
                sectionName = "ElectronicRobotTicket";

            GetInfo(sectionName);
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
                log.Write(String.Format("{0} 提交成功! SchemeID:{1}", sectionName, ticket.SchemeID));
#endif
                return true;
            }
            catch (MessageQueueException ex)
            {
                log.Write(String.Format("{0} 提交失败! SchemeID:{1}\n{2}", sectionName, ticket.SchemeID, ex.Message));
                return false;
            }
        }

    }
}
