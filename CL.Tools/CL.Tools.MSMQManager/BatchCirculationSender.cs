using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CL.Tools.MSMQManager
{
    public class BatchCirculationSender
    {
        private readonly BatchCirculation circulation = new BatchCirculation();
        private readonly Log log = new Log("BatchCirculation");
        
        public BatchCirculationSender()
        {
        }

        /// <summary>
        /// 电子票信息
        /// </summary>
        public BatchCirculation BatchCirculation
        {
            get { return circulation; }
        }
        public void GetInfo(string type)
        {
            var ini = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "\\MSMQManager.ini");
            try
            {
                circulation.isRemote = Utils.StrToBool(ini.Read(type, "IsRemote"), false);
                if (!circulation.isRemote)    //判断是否远程发送队列
                {
                    circulation.ElectronicTicketQueueName = ini.Read(type, "LocalPath");
                }
                else
                {
                    circulation.ElectronicTicketQueueName = ini.Read(type, "RemotePath");
                }
            }
            catch (Exception ex)
            {
                log.Write("ElectronicTicket读取配置路径出错：" + ex.Message, true);
            }
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
                if (circulation.isRemote)
                {
                    queue = new MessageQueue(circulation.ElectronicTicketQueueName);
                }
                else
                {
                    if (MessageQueue.Exists(circulation.ElectronicTicketQueueName))
                        queue = new MessageQueue(circulation.ElectronicTicketQueueName);
                    else
                    {
                        queue = MessageQueue.Create(circulation.ElectronicTicketQueueName);
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
            if (circulation == null)
                return false;
            string sectionName = "BatchCirculation";
            //GetInfo(sectionName);
            try
            {
                using (var queue = Create())
                {
                    using (var message = new Message(circulation) { Recoverable = true, Priority = MessagePriority.High })
                    {
                        queue.Send(message, String.Format("CirculationID {0}", circulation.CirculationID)); //发送消息
                    }
                }
                return true;
            }
            catch (MessageQueueException ex)
            {
                log.Write(String.Format("{0} 提交失败! CirculationID:{1}\n{2}", sectionName, circulation.CirculationID, ex.Message));
                return false;
            }
        }
    }
}
