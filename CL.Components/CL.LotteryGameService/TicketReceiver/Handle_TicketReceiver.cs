using CL.Enum.Common.Status;
using CL.Enum.Common.Type;
using CL.Game.BLL;
using CL.Game.BLL.View;
using CL.Game.Entity;
using CL.LotteryGameService.Model;
using CL.Tools.Common;
using CL.Tools.LotterySplitTickets;
using CL.Tools.MSMQManager;
using CL.Tools.TicketInterface;
using CL.View.Entity.ElectronicTicket;
using CL.View.Entity.Game;
using CL.View.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CL.LotteryGameService.TicketReceiver
{
    public class Handle_TicketReceiver
    {
        /// <summary>
        /// 委托启动程序
        /// </summary>
        /// <param name="rich"></param>
        protected delegate void action_delegate(RichTextBox rich);
        /// <summary>
        /// 委托写日志
        /// </summary>
        public WritText log_Writ;
        /// <summary>
        /// 消息队列过期时间
        /// </summary>
        protected int QueueExpireTime = 30;
        /// <summary>
        /// 方案
        /// </summary>
        protected SchemesBLL blls = new SchemesBLL();
        /// <summary>
        /// 电子票
        /// </summary>
        protected SchemeETicketsBLL blle = new SchemeETicketsBLL();
        /// <summary>
        /// 彩种
        /// </summary>
        protected LotteriesBLL blll = new LotteriesBLL();
        /// <summary>
        /// 追号
        /// </summary>
        protected ChaseTaskDetailsBLL bblct = new ChaseTaskDetailsBLL();
        /// <summary>
        /// 期号
        /// </summary>
        protected IsusesBLL blli = new IsusesBLL();
        /// <summary>
        /// 会员
        /// </summary>
        protected UsersBLL bllu = new UsersBLL();

        public static readonly Dictionary<Int32, XmlNode> SupplierProvider = new Dictionary<Int32, XmlNode>(9999);

        /// <summary>
        /// 程序启动
        /// </summary>
        /// <param name="rich"></param>
        protected void Start(RichTextBox rich)
        {
            rich.ForeColor = System.Drawing.Color.Blue;
            log_Writ = new WritText(rich);
            try
            {
                //加载配置
                Thread th_Dupplier = new Thread(new ThreadStart(RegisterLotteryDupplier));
                th_Dupplier.Start();
                //启动小票拆票队列
                Thread th_SpiltTicket = new Thread(new ThreadStart(Start_SpiltTicket));
                th_SpiltTicket.Start();
                //启动大票拆票队列
                Thread th_BigSpiltTicket = new Thread(new ThreadStart(Start_BigSpiltTicket));
                th_BigSpiltTicket.Start();
                //启动小票投注队列
                Thread th_BettingTicket = new Thread(new ThreadStart(Start_BettingTicket));
                th_BettingTicket.Start();
                //启动大票投注队列
                Thread th_BettingBigTicket = new Thread(new ThreadStart(Start_BettingBigTicket));
                th_BettingBigTicket.Start();
            }
            catch (Exception ex)
            {
                log_Writ.WritTextBox(string.Format("方案拆票启动失败：{0}", ex.Message));
                throw;
            }
        }
        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="rich"></param>
        public void Run(RichTextBox rich)
        {
            if (rich.InvokeRequired)
            {
                action_delegate dt = new action_delegate(Start);
                rich.Invoke(dt, new object[] { rich });
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
        #region 公共方法

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
                TickBuilder bulider = TickBuilder.Create(SupplierProvider[lotteryCode].SelectSingleNode("interfacetype").InnerText, lotteryCode);
                tickets = bulider.Generate(SchemeID, lotteryCode, ticketList);

                if (tickets == null || tickets.Count == 0)
                {
                    log_Writ.WritTextBox("方案ID:" + SchemeID + " 电子票拆分错误,进行撤单");
                    return false;
                }
                if (Math.Abs(tickets.Sum(x => x.Money) - schemeMoney) < 0)   //金额错误，进行撤单
                {
                    log_Writ.WritTextBox("方案ID:" + SchemeID + " 金额拆分错误 方案金额" + schemeMoney + " 电子票总金额" + tickets.Sum(x => x.Money) + ",进行撤单");
                    return false;
                }
                bool success = false;

                #region 电子票入库处理


                List<udv_Ticket> SqlPara = new List<udv_Ticket>();
                SqlPara.AddRange(tickets);

                if (SqlPara.Count == 0) return false;
                if (!blls.Exists(SchemeID)) return false;

                if (SqlPara != null)
                {
                    if (SqlPara.Count <= 100)
                        success = blle.TicketsStorage(SchemeID, ChaseTaskDetailsID, SqlPara);
                    else
                    {
                        int PageSize = 100;
                        int Pages = (SqlPara.Count / PageSize) + ((SqlPara.Count % PageSize) > 0 ? 1 : 0);
                        for (int i = 0; i < Pages; i++)
                        {
                            success = blle.TicketsStorage(SchemeID, ChaseTaskDetailsID, SqlPara.Skip(i * PageSize).Take(PageSize).ToList());
                        }
                    }
                }
                #endregion

                return success;
            }
            catch (Exception ex)
            {
                log_Writ.WritTextBox("拆票出现异常 " + ex);
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
                TickBuilder bulider = TickBuilder.Create(SupplierProvider[lotteryCode].SelectSingleNode("interfacetype").InnerText, lotteryCode);
                tickets = bulider.Generate(SchemeID, lotteryCode, ticketList);
                if (tickets == null || tickets.Count == 0)
                {
                    log_Writ.WritTextBox("方案ID:" + SchemeID + " 电子票拆分错误,进行撤单");
                    return false;
                }
                if (Math.Abs(tickets.Sum(x => x.Money) - schemeMoney) < 0)   //金额错误，进行撤单
                {
                    log_Writ.WritTextBox("方案ID:" + SchemeID + " 金额拆分错误 方案金额" + schemeMoney + " 电子票总金额" + tickets.Sum(x => x.Money) + ",进行撤单");
                    return false;
                }
                bool success = false;

                #region 电子票入库处理

                List<udv_Ticket> SqlPara = new List<udv_Ticket>();
                SqlPara.AddRange(tickets);

                if (SqlPara.Count == 0) return false;
                if (!blls.Exists(SchemeID)) return false;
                success = blle.TicketsStorage_Robot(SchemeID, ChaseTaskDetailsID, SqlPara);
                #endregion

                return success;
            }
            catch (Exception ex)
            {
                log_Writ.WritTextBox("拆票出现异常 " + ex);
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
            IsusesEntity model = blli.QueryEntitysByLotteryCode(LotteryCode, IsuseName);
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
            bool CallResult = blle.QuashScheme(SchemeID, IsSystemQuash, ref ReturnDescription);
            if (!CallResult)
            {
                log_Writ.WritTextBox("方案ID：" + SchemeID + "撤单处理失败");
                return false;
            }
            if (CallResult)
            {
                log_Writ.WritTextBox("方案ID：" + SchemeID + "撤单处理成功");
                return true;
            }
            log_Writ.WritTextBox("方案ID：" + SchemeID + "撤单处理，撤单描述：" + ReturnDescription);
            return true;
        }


        /// <summary>
        /// 彩种接口类型配置
        /// </summary>
        protected void RegisterLotteryDupplier()
        {
            if (SupplierProvider.Count > 0) return;
            XmlNode Node = Utils.QueryConfigNode("root/interface");
            XmlNodeList XmlList = Node.SelectNodes("item");
            foreach (XmlNode item in XmlList)
            {
                try
                {
                    int LotteryCode = Convert.ToInt32(item.SelectSingleNode("systemlotterycode").InnerText);
                    SupplierProvider.Add(LotteryCode, item);
                }
                catch (Exception ex)
                {
                    log_Writ.WritTextBox(string.Format("彩种接口类型配置错误：{0}", ex.Message));
                    throw;
                }
            }
        }
        #endregion

        #region 拆票队列(小票)
        /// <summary>
        /// 消息队列
        /// </summary>
        protected MessageQueue SpiltTicket_MessageQueue = null;
        /// <summary>
        /// 消息队列名称
        /// </summary>
        protected string SpiltTicket_QueueName = String.Empty;
        /// <summary>
        /// 是否远程队列
        /// </summary>
        protected bool SpiltTicket_IsRemote = false;
        /// <summary>
        /// 实时投注队列路径
        /// </summary>
        protected string SpiltTicket_QueueBettingticket = String.Empty;
        /// <summary>
        /// 请求停止状态
        /// </summary>
        protected bool SpiltTicket_RequestStop = false;

        /// <summary>
        /// 启动拆票队列
        /// </summary>
        protected void Start_SpiltTicket()
        {
            try
            {
                SpiltTicket_Initilize();
                SpiltTicket_MessageQueue = this.GetMessageQueue(SpiltTicket_QueueName, SpiltTicket_IsRemote);
                SpiltTicket_MessageQueue.Formatter = new XmlMessageFormatter(new[] { typeof(ElectronicTicket) });

                Task.Factory.StartNew(() =>
                {
                    log_Writ.WritTextBox("读取小票拆票队列:" + SpiltTicket_QueueName);
                    using (MessageEnumerator messageEnumerator = SpiltTicket_MessageQueue.GetMessageEnumerator2())
                    {
                        while (true)
                        {
                            try
                            {
                                SpiltTicket_PeekMessage(messageEnumerator);
                                messageEnumerator.Reset();
                                log_Writ.WritTextBox("处理完成");
                            }
                            catch (Exception ex)
                            {
                                //把当前的方案移除
                                SpiltTicket_MessageQueue.ReceiveById(messageEnumerator.Current.Id);
                                log_Writ.WritTextBox(String.Format("消息队列读取发送异常，{1}", ex));
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                SpiltTicket_RequestStop = true;
                log_Writ.WritTextBox("初始化配置发生异常 " + ex);
            }
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        protected void SpiltTicket_Initilize()
        {
            string sectionName = "ElectronicTicket";
            var iniFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Config\\MSMQManager.ini";
            var ini = new IniFile(iniFilePath);

            SpiltTicket_QueueBettingticket = ini.Read(sectionName, "QueueBetting");

            SpiltTicket_IsRemote = Utils.StrToBool(ini.Read(sectionName, "IsRemote"), false);
            if (!SpiltTicket_IsRemote)    //判断是否远程发送队列
            {
                SpiltTicket_QueueName = ini.Read(sectionName, "LocalPath");
            }
            else
            {
                SpiltTicket_QueueName = ini.Read(sectionName, "RemotePath");
            }
        }

        /// <summary>
        /// 读取消息队列
        /// </summary>
        protected void SpiltTicket_PeekMessage(MessageEnumerator messageEnumerator)
        {
            while (messageEnumerator.MoveNext(TimeSpan.FromDays(QueueExpireTime))) //默认30天
            {
                if (SpiltTicket_RequestStop)
                {
                    log_Writ.WritTextBox("请求终止");
                    return;
                }
                var current = messageEnumerator.Current;
                if (current == null) continue;
                System.Messaging.Message message = null;
                message = SpiltTicket_MessageQueue.PeekById(current.Id);
                if (message == null) continue;
                var order = (ElectronicTicket)message.Body;
                try
                {
                    //处理拆票
                    SpiltTicket_ActionStart(order);
                }
                catch (Exception ex)
                {
                    log_Writ.WritTextBox("拆票出现异常，撤单 " + ex);
                    //拆票是否成功 队列中的方案ID应该删除
                    SpiltTicket_MessageQueue.ReceiveById(current.Id);
                    //拆票失败撤单
                    P_QuashScheme(order.SchemeID, true);
                    blls.RevokeRedisSchemeEntity(order.SchemeID, order.ChaseTaskDetailsID);
                    continue;
                }
                SpiltTicket_MessageQueue.ReceiveById(current.Id);
            }
        }
        /// <summary>
        /// 开始处理拆票
        /// </summary>
        /// <param name="order"></param>
        protected void SpiltTicket_ActionStart(ElectronicTicket order)
        {
            var SchModel = blls.QueryEntity(order.SchemeID);
            if (SchModel == null)
            {
                log_Writ.WritTextBox("方案不存在，取消拆票 SchemeID:" + order.SchemeID);
                return;
            }
            var LotteryEntity = blll.QueryEntity(SchModel.LotteryCode);
            //10.下单失败（限号），12.订单撤销
            if (SchModel.SchemeStatus == (int)SchemeStatusEnum.OrderFailure || SchModel.SchemeStatus == (int)SchemeStatusEnum.OrderRevoke)
            {
                log_Writ.WritTextBox("订单下单失败或订单已经撤销，取消拆票 SchemeID:" + order.SchemeID);
                return;
            }

            log_Writ.WritTextBox("小票队列：开始电子票拆票 SchemeID:" + order.SchemeID);
            bool isTrue = this.SplitLotteryNumber(order.SchemeID, order.LotteryCode, order.SchemeMoney / 100, order.TicketDetails, order.ChaseTaskDetailsID);
            log_Writ.WritTextBox(string.Format("方案号:{0} 总金额：{1} 拆票 {2}", order.SchemeID, order.SchemeMoney / 100, isTrue ? "成功" : "失败"));
            if (LotteryEntity.PrintOutType == 0)
            {
                #region 内容
                if (SchModel.BuyType == (byte)BuyTypeEnum.BuyChase)
                {
                    //验证
                    var ChaseTaskDetailsEntity = bblct.QueryEntity(order.ChaseTaskDetailsID);
                    if (!(ChaseTaskDetailsEntity.IsSendOut ?? false))
                    {
                        bblct.ModifySendOut(order.ChaseTaskDetailsID);
                    }
                    else
                    {
                        log_Writ.WritTextBox(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                    }
                }
                else
                {
                    if (!SchModel.IsSendOut)
                    {
                        //更新为发送状态，防止重新发送出票
                        blls.ModifySendOut(order.SchemeID);
                    }
                    else
                    {
                        log_Writ.WritTextBox(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                    }
                }
                #endregion
                blls.OutApply(order.SchemeID, order.ChaseTaskDetailsID, true, DateTime.Now);
                log_Writ.WritTextBox("出票完成：本地电子票 SchemeID:" + order.SchemeID);
                return;
            }
            if (!isTrue)
            {
                //拆票失败撤单
                P_QuashScheme(order.SchemeID, true);
                blls.RevokeRedisSchemeEntity(SchModel.SchemeID, order.ChaseTaskDetailsID);

            }
            else //成功发送消息到出票队列
            {
                if (IsIsuseSale(order.LotteryCode, order.IsuseName))
                {
                    #region 发送消息至出票队列
                    BettingTicketSender TicketSender = null;
                    //判断拆分大小 以便通知队列
                    TicketSender = new BettingTicketSender(SpiltTicket_QueueBettingticket);

                    //是否已经发送到出票队列
                    if (SchModel.BuyType == (byte)BuyTypeEnum.BuyChase)
                    {
                        //验证
                        var ChaseTaskDetailsEntity = bblct.QueryEntity(order.ChaseTaskDetailsID);
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
                                blls.RevokeRedisSchemeEntity(order.SchemeID, order.ChaseTaskDetailsID);
                                log_Writ.WritTextBox("发送到出票队列发送错误，完成撤单");
                            }
                            bblct.ModifySendOut(order.ChaseTaskDetailsID);
                        }
                        else
                        {
                            log_Writ.WritTextBox(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
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
                                blls.RevokeRedisSchemeEntity(order.SchemeID, order.ChaseTaskDetailsID);
                                log_Writ.WritTextBox("发送到出票队列发送错误，完成撤单");
                            }
                            //更新为发送状态，防止重新发送出票
                            blls.ModifySendOut(order.SchemeID);
                        }
                        else
                        {
                            log_Writ.WritTextBox(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                        }
                    }
                    #endregion
                }
                else
                {
                    P_QuashScheme(order.SchemeID, true);
                    blls.RevokeRedisSchemeEntity(order.SchemeID, order.ChaseTaskDetailsID);
                    log_Writ.WritTextBox(string.Format("期号：{0} 已经结束，取消发送到实时出票队列，方案ID：{1} ", order.IsuseName, order.SchemeID));
                }
            }
        }
        #endregion

        #region 拆票队列(大票)
        /// <summary>
        /// 消息队列
        /// </summary>
        protected MessageQueue BigSpiltTicket_MessageQueue = null;
        /// <summary>
        /// 消息队列名称
        /// </summary>
        protected string BigSpiltTicket_QueueName = String.Empty;
        /// <summary>
        /// 是否远程队列
        /// </summary>
        protected bool BigSpiltTicket_IsRemote = false;
        /// <summary>
        /// 实时投注队列路径
        /// </summary>
        protected string BigSpiltTicket_QueueBettingBigticket = String.Empty;
        /// <summary>
        /// 请求停止状态
        /// </summary>
        protected bool BigSpiltTicket_QequestStop = false;

        /// <summary>
        /// 启动拆票队列
        /// </summary>
        protected void Start_BigSpiltTicket()
        {
            BigSpiltTicket_Initilize();
            BigSpiltTicket_MessageQueue = this.GetMessageQueue(BigSpiltTicket_QueueName, BigSpiltTicket_IsRemote);
            BigSpiltTicket_MessageQueue.Formatter = new XmlMessageFormatter(new[] { typeof(ElectronicTicket) });

            Task.Factory.StartNew(() =>
            {
                log_Writ.WritTextBox("读取大票拆票队列:" + BigSpiltTicket_QueueName);
                using (MessageEnumerator messageEnumerator = BigSpiltTicket_MessageQueue.GetMessageEnumerator2())
                {
                    while (true)
                    {
                        try
                        {
                            BigSpiltTicket_PeekMessage(messageEnumerator);
                            messageEnumerator.Reset();
                            log_Writ.WritTextBox("处理完成");
                        }
                        catch (Exception ex)
                        {
                            //把当前的方案移除
                            BigSpiltTicket_MessageQueue.ReceiveById(messageEnumerator.Current.Id);
                            log_Writ.WritTextBox(String.Format("消息队列读取发送异常，{1}", ex));
                        }
                    }
                }
            });
        }
        /// <summary>
        /// 初始化配置
        /// </summary>
        protected void BigSpiltTicket_Initilize()
        {
            string sectionName = "ElectronicBigTicket";
            var iniFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Config\\MSMQManager.ini";
            var ini = new IniFile(iniFilePath);

            BigSpiltTicket_QueueBettingBigticket = ini.Read(sectionName, "QueueBetting");

            BigSpiltTicket_IsRemote = Utils.StrToBool(ini.Read(sectionName, "IsRemote"), false);
            if (!BigSpiltTicket_IsRemote)    //判断是否远程发送队列
            {
                BigSpiltTicket_QueueName = ini.Read(sectionName, "LocalPath");
            }
            else
            {
                BigSpiltTicket_QueueName = ini.Read(sectionName, "RemotePath");
            }
        }

        /// <summary>
        /// 读取消息队列
        /// </summary>
        protected void BigSpiltTicket_PeekMessage(MessageEnumerator messageEnumerator)
        {
            while (messageEnumerator.MoveNext(TimeSpan.FromDays(QueueExpireTime))) //默认30天
            {
                if (BigSpiltTicket_QequestStop)
                {
                    log_Writ.WritTextBox("请求终止");
                    return;
                }
                var current = messageEnumerator.Current;
                if (current == null) continue;
                System.Messaging.Message message = null;
                message = BigSpiltTicket_MessageQueue.PeekById(current.Id);
                if (message == null) continue;
                var order = (ElectronicTicket)message.Body;
                try
                {
                    //处理拆票
                    BigSpiltTicket_ActionStart(order);
                }
                catch (Exception ex)
                {
                    log_Writ.WritTextBox("拆票出现异常，撤单 " + ex);
                    //拆票是否成功 队列中的方案ID应该删除
                    BigSpiltTicket_MessageQueue.ReceiveById(current.Id);
                    //拆票失败撤单
                    P_QuashScheme(order.SchemeID, true);
                    blls.RevokeRedisSchemeEntity(order.SchemeID, order.ChaseTaskDetailsID);
                    continue;
                }
                BigSpiltTicket_MessageQueue.ReceiveById(current.Id);
            }
        }

        /// <summary>
        /// 开始处理拆票
        /// </summary>
        /// <param name="order"></param>
        protected void BigSpiltTicket_ActionStart(ElectronicTicket order)
        {
            var SchModel = blls.QueryEntity(order.SchemeID);
            if (SchModel == null)
            {
                log_Writ.WritTextBox("方案不存在，取消拆票 SchemeID:" + order.SchemeID);
                return;
            }
            var LotteryEntity = blll.QueryEntity(SchModel.LotteryCode);
            //10.下单失败（限号），12.订单撤销
            if (SchModel.SchemeStatus == (int)SchemeStatusEnum.OrderFailure || SchModel.SchemeStatus == (int)SchemeStatusEnum.OrderRevoke)
            {
                log_Writ.WritTextBox("订单下单失败或订单已经撤销，取消拆票 SchemeID:" + order.SchemeID);
                return;
            }
            log_Writ.WritTextBox("大票队列：开始电子票拆票 SchemeID:" + order.SchemeID);
            bool isTrue = this.SplitLotteryNumber(order.SchemeID, order.LotteryCode, order.SchemeMoney / 100, order.TicketDetails, order.ChaseTaskDetailsID);
            log_Writ.WritTextBox(string.Format("方案号:{0} 总金额：{1} 拆票 {2}", order.SchemeID, order.SchemeMoney / 100, isTrue ? "成功" : "失败"));
            if (LotteryEntity.PrintOutType == 0)
            {
                #region 内容
                if (SchModel.BuyType == (byte)BuyTypeEnum.BuyChase)
                {
                    //验证
                    var ChaseTaskDetailsEntity = bblct.QueryEntity(order.ChaseTaskDetailsID);
                    if (!(ChaseTaskDetailsEntity.IsSendOut ?? false))
                    {
                        bblct.ModifySendOut(order.ChaseTaskDetailsID);
                    }
                    else
                    {
                        log_Writ.WritTextBox(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                    }
                }
                else
                {
                    if (!SchModel.IsSendOut)
                    {
                        //更新为发送状态，防止重新发送出票
                        blls.ModifySendOut(order.SchemeID);
                    }
                    else
                    {
                        log_Writ.WritTextBox(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                    }
                }
                #endregion
                blls.OutApply(order.SchemeID, order.ChaseTaskDetailsID, true, DateTime.Now);
                log_Writ.WritTextBox("出票完成：本地电子票 SchemeID:" + order.SchemeID);
                return;
            }
            if (!isTrue)
            {
                //拆票失败撤单
                this.P_QuashScheme(order.SchemeID, true);
                blls.RevokeRedisSchemeEntity(SchModel.SchemeID, order.ChaseTaskDetailsID);
            }
            else //成功发送消息到出票队列
            {
                if (IsIsuseSale(order.LotteryCode, order.IsuseName))
                {
                    #region 发送消息至出票队列
                    BettingTicketSender TicketSender = null;
                    //判断拆分大小 以便通知队列
                    TicketSender = new BettingTicketSender(BigSpiltTicket_QueueBettingBigticket);

                    //是否已经发送到出票队列
                    if (SchModel.BuyType == (byte)BuyTypeEnum.BuyChase)
                    {
                        //验证
                        var ChaseTaskDetailsEntity = bblct.QueryEntity(order.ChaseTaskDetailsID);
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
                                blls.RevokeRedisSchemeEntity(order.SchemeID, order.ChaseTaskDetailsID);
                                log_Writ.WritTextBox("发送到出票队列发送错误，完成撤单");
                            }
                            bblct.ModifySendOut(order.ChaseTaskDetailsID);
                        }
                        else
                        {
                            log_Writ.WritTextBox(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
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
                                this.P_QuashScheme(order.SchemeID, true);
                                blls.RevokeRedisSchemeEntity(order.SchemeID, order.ChaseTaskDetailsID);
                                log_Writ.WritTextBox("发送到出票队列发送错误，完成撤单");
                            }
                            //更新为发送状态，防止重新发送出票
                            blls.ModifySendOut(order.SchemeID);
                        }
                        else
                        {
                            log_Writ.WritTextBox(string.Format("方案ID：{0} 已经发送到实时出票队列，不要重复发送", order.SchemeID));
                        }
                    }
                    #endregion
                }
                else
                {
                    this.P_QuashScheme(order.SchemeID, true);
                    blls.RevokeRedisSchemeEntity(SchModel.SchemeID, order.ChaseTaskDetailsID);
                    log_Writ.WritTextBox(string.Format("期号：{0} 已经结束，取消发送到实时出票队列，方案ID：{1} ", order.IsuseName, order.SchemeID));
                }
            }
        }
        #endregion

        #region 投注队列(小票)
        protected MessageQueue BettingTicket_Queue;
        protected string BettingTicket_QueueName = string.Empty;
        protected void Start_BettingTicket()
        {
            BettingTicket_InitializeConfiguration();
            BettingTicket_Queue = this.GetMessageQueue(BettingTicket_QueueName);
            BettingTicket_Queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(BettingTicket) });
            Task.Factory.StartNew(() =>
            {
                this.BettingTicket_Run();
            });

        }
        /// <summary>
        /// 读取消息队列
        /// </summary>
        public void BettingTicket_Run()
        {
            try
            {
                log_Writ.WritTextBox("读取小票投注队列:" + BettingTicket_QueueName);
                using (MessageEnumerator messagesEnumerator = BettingTicket_Queue.GetMessageEnumerator2())
                {
                    bool IsBettingRevoke = false;
                    while (messagesEnumerator.MoveNext(TimeSpan.FromDays(QueueExpireTime))) //默认3天
                    {
                        var messageA = messagesEnumerator.Current;
                        if (messageA == null) return;
                        BettingTicket_Queue.MessageReadPropertyFilter.Priority = true;
                        System.Messaging.Message message = BettingTicket_Queue.PeekById(messageA.Id);
                        var order = message.Body as BettingTicket;
                        if (order != null)
                        {
                            if (order.InterfaceConfig == null)
                            {
                                log_Writ.WritTextBox("发送的XML数据信息失败");
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
                                        log_Writ.WritTextBox("初始化接口失败");
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
                                        List<udv_BettingTickets> TricketList = blls.QueryDataList(order.SchemeID, order.ChaseTaskDetailsID);

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
                                            log_Writ.WritTextBox(String.Format("方案[{0}]调用[{1}]接口处理方案 返回 -->状态码：{2}信息：{3}", order.SchemeID, xml.SelectSingleNode("interfacetype").InnerText, ResItem.ErrorCode, ResItem.ErrorMsg));
                                        }
                                        #endregion
                                        #region 更新电子票状态信息
                                        if (TicketNoticeList.Count > 0)
                                        {
                                            if (blls.Exists(order.SchemeID))
                                            {
                                                bWithdrawal = blle.TicketBetting(TicketNoticeList, order.SchemeID, order.ChaseTaskDetailsID);
                                            }
                                        }
                                        var SchemeEntity = blls.QueryEntity(order.SchemeID);
                                        if (!bWithdrawal)//整个方案号投注失败进行撤单
                                        {
                                            blle.SchemeETicketsWithdrawal(order.SchemeID, ErrorMsg);
                                            //redis撤单
                                            if (SchemeEntity != null)
                                            {
                                                if (SchemeEntity.BuyType == (byte)BuyTypeEnum.BuyChase)
                                                {
                                                    var ChaseDetailEntity = bblct.QueryEntity(order.ChaseTaskDetailsID);
                                                    if (ChaseDetailEntity != null)
                                                    {
                                                        ChaseDetailEntity.QuashStatus = (short)QuashStatusEnum.SysQuash;
                                                        bllu.ModifyUserBalanceRedis(SchemeEntity.InitiateUserID, ChaseDetailEntity.Amount, true);//撤单
                                                        string IssueName = blli.QueryIssueName(ChaseDetailEntity.IsuseID);
                                                        bllu.SendWinMoneyContent(order.SchemeID, IssueName, ChaseDetailEntity.Amount);//撤单推送
                                                        bblct.ModifyEntity(ChaseDetailEntity);  //更新
                                                    }
                                                    var ChaseDetailEntitys = bblct.QueryEntitysBySchemeID(order.SchemeID);
                                                    var IsExecuted = ChaseDetailEntitys.Where(w => w.IsExecuted == true).ToList();
                                                    if (IsExecuted.Count == ChaseDetailEntitys.Count)
                                                    {
                                                        SchemeEntity.SchemeStatus = (short)SchemeStatusEnum.ChaseSuccess;
                                                        blls.ModifyEntity(SchemeEntity);
                                                    }
                                                    else
                                                    {
                                                        IsExecuted = ChaseDetailEntitys.Where(w => w.IsExecuted == false).ToList();
                                                        var QuashStatus = ChaseDetailEntitys.Where(w => w.QuashStatus != (short)QuashStatusEnum.NoQuash).ToList();
                                                        if (IsExecuted.Count == QuashStatus.Count)
                                                        {
                                                            SchemeEntity.SchemeStatus = (short)SchemeStatusEnum.ChaseSuccess;
                                                            blls.ModifyEntity(SchemeEntity);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    SchemeEntity.SchemeStatus = (short)SchemeStatusEnum.OrderRevoke;
                                                    bllu.ModifyUserBalanceRedis(SchemeEntity.InitiateUserID, SchemeEntity.SchemeMoney, true);//撤单
                                                    bllu.SendWinMoneyContent(order.SchemeID, SchemeEntity.IsuseName, SchemeEntity.SchemeMoney);//撤单推送
                                                    blls.ModifyEntity(SchemeEntity);
                                                }
                                            }
                                            blls.OutApply(order.SchemeID, order.ChaseTaskDetailsID, true, DateTime.Now);
                                        }
                                        else
                                        {
                                            List<SchemeETicketsEntity> Entitys = blle.QueryEntitysBySchemeID(order.SchemeID);
                                            if (SchemeEntity != null)
                                            {
                                                var Entitys_3 = Entitys.Where(w => w.TicketStatus == (byte)TicketStatusEnum.BettingFailure).ToList();  //投注失败撤单
                                                if (Entitys_3.Count == Entitys.Count)
                                                {
                                                    if (SchemeEntity.BuyType == (byte)BuyTypeEnum.BuyChase)
                                                    {
                                                        var ChaseDetailEntity = bblct.QueryEntity(order.ChaseTaskDetailsID);
                                                        if (ChaseDetailEntity != null)
                                                        {
                                                            ChaseDetailEntity.QuashStatus = (short)QuashStatusEnum.SysQuash;
                                                            bllu.ModifyUserBalanceRedis(SchemeEntity.InitiateUserID, ChaseDetailEntity.Amount, true);//撤单

                                                            string IssueName = blli.QueryIssueName(ChaseDetailEntity.IsuseID);
                                                            bllu.SendWinMoneyContent(order.SchemeID, IssueName, ChaseDetailEntity.Amount);//撤单推送
                                                        }
                                                    }
                                                    else
                                                    {
                                                        SchemeEntity.SchemeStatus = (short)SchemeStatusEnum.OrderRevoke;
                                                        bllu.ModifyUserBalanceRedis(SchemeEntity.InitiateUserID, SchemeEntity.SchemeMoney, true);//撤单
                                                        bllu.SendWinMoneyContent(order.SchemeID, SchemeEntity.IsuseName, SchemeEntity.SchemeMoney);//撤单推送

                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        blls.OutApply(order.SchemeID, order.ChaseTaskDetailsID, true, DateTime.Now);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log_Writ.WritTextBox(String.Format("{0}队列投注出错:\n{1}", BettingTicket_QueueName, ex.Message));
                                    IsBettingRevoke = true;
                                }
                                #endregion
                            }
                            //出错撤单
                            if (IsBettingRevoke)
                            {
                                blle.SchemeETicketsWithdrawal(order.SchemeID, "");
                                blls.OutApply(order.SchemeID, order.ChaseTaskDetailsID, true, DateTime.Now);
                                IsBettingRevoke = false;
                            }
                        }
                        //投注是否成功 队列中的方案ID应该删除
                        BettingTicket_Queue.ReceiveById(messageA.Id);
                    }
                }
                log_Writ.WritTextBox(QueueExpireTime + "天内没有检测到订单 退出消息队列");
            }
            catch (MessageQueueException ex)
            {
                log_Writ.WritTextBox(String.Format("{0}队列出错:\n{1}", BettingTicket_QueueName, ex.Message));
            }
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="sectionName"></param>
        public void BettingTicket_InitializeConfiguration()
        {
            string sectionName = "BettingTicket";
            var iniFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Config\\MSMQManager.ini";
            var ini = new IniFile(iniFilePath);
            BettingTicket_QueueName = ini.Read(sectionName, "QueuePath");
        }
        #endregion

        #region 投注队列(大票)
        protected MessageQueue BettingBigTicket_Queue;
        protected string BettingBigTicket_QueueName = string.Empty;
        protected void Start_BettingBigTicket()
        {
            BettingBigTicket_InitializeConfiguration();
            BettingBigTicket_Queue = this.GetMessageQueue(BettingBigTicket_QueueName);
            BettingBigTicket_Queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(BettingTicket) });
            Task.Factory.StartNew(() =>
            {
                this.BettingBigTicket_Run();
            });
        }
        protected void BettingBigTicket_Run()
        {
            try
            {
                log_Writ.WritTextBox("读取大票投注队列:" + BettingBigTicket_QueueName);
                using (MessageEnumerator messagesEnumerator = BettingBigTicket_Queue.GetMessageEnumerator2())
                {
                    bool IsBettingRevoke = false;
                    while (messagesEnumerator.MoveNext(TimeSpan.FromDays(QueueExpireTime))) //默认3天
                    {
                        var messageA = messagesEnumerator.Current;
                        if (messageA == null) return;
                        BettingBigTicket_Queue.MessageReadPropertyFilter.Priority = true;
                        System.Messaging.Message message = BettingBigTicket_Queue.PeekById(messageA.Id);
                        var order = message.Body as BettingTicket;
                        if (order != null)
                        {
                            if (order.InterfaceConfig == null)
                            {
                                log_Writ.WritTextBox("发送的XML数据信息失败");
                                IsBettingRevoke = true;
                            }
                            else
                            {
                                #region
                                try
                                {
                                    if (!DetectionInterfaceBalance(order.SchemeID))
                                    {
                                        blle.SchemeETicketsWithdrawal(order.SchemeID, "");
                                        blls.OutApply(order.SchemeID, order.ChaseTaskDetailsID, true, DateTime.Now);
                                        return;
                                    }

                                    XmlNode xml = order.InterfaceConfig;
                                    InterfaceBase InterTicker = new InterfaceBase()[xml];
                                    if (order.InterfaceConfig == null)
                                    {
                                        log_Writ.WritTextBox("发送的XML数据信息失败");
                                        IsBettingRevoke = true;
                                    }

                                    List<udv_BettingEntites> TicketNoticeList = new List<udv_BettingEntites>();
                                    List<udv_ResultBetting> ResModel = new List<udv_ResultBetting>();
                                    bool bWithdrawal = false;
                                    string ErrorMsg = string.Empty;

                                    //电子票投注 由单个方案ID进行投注
                                    #region 电子票投注
                                    List<udv_ParaBettingTicker> para = new List<udv_ParaBettingTicker>();
                                    List<udv_BettingTickets> TricketList = new udv_BettingTicketsBLL().QueryDataList(order.SchemeID, order.ChaseTaskDetailsID);

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
                                        log_Writ.WritTextBox(String.Format("方案[{0}]调用[{1}]接口处理方案 返回 -->状态码：{2}信息：{3}", order.SchemeID, xml.SelectSingleNode("interfacetype").InnerText, ResItem.ErrorCode, ResItem.ErrorMsg));
                                    }
                                    #endregion
                                    #region 更新电子票状态信息
                                    if (TicketNoticeList.Count > 0)
                                    {
                                        if (blls.Exists(order.SchemeID))
                                        {
                                            bWithdrawal = blle.TicketBetting(TicketNoticeList, order.SchemeID, order.ChaseTaskDetailsID);
                                        }
                                    }
                                    var SchemeEntity = blls.QueryEntity(order.SchemeID);
                                    if (!bWithdrawal)//整个方案号投注失败进行撤单
                                    {
                                        blle.SchemeETicketsWithdrawal(order.SchemeID, ErrorMsg);
                                        //redis撤单
                                        if (SchemeEntity != null)
                                        {
                                            if (SchemeEntity.BuyType == (byte)BuyTypeEnum.BuyChase)
                                            {
                                                var ChaseDetailEntity = bblct.QueryEntity(order.ChaseTaskDetailsID);
                                                if (ChaseDetailEntity != null)
                                                {
                                                    ChaseDetailEntity.QuashStatus = (short)QuashStatusEnum.SysQuash;
                                                    bblct.ModifyEntity(ChaseDetailEntity);
                                                    bllu.ModifyUserBalanceRedis(SchemeEntity.InitiateUserID, ChaseDetailEntity.Amount, true);//撤单

                                                    string IssueName = blli.QueryIssueName(ChaseDetailEntity.IsuseID);
                                                    bllu.SendWinMoneyContent(order.SchemeID, IssueName, ChaseDetailEntity.Amount);//撤单推送
                                                    bblct.ModifyEntity(ChaseDetailEntity);  //更新
                                                }
                                                var ChaseDetailEntitys = bblct.QueryEntitysBySchemeID(order.SchemeID);
                                                var IsExecuted = ChaseDetailEntitys.Where(w => w.IsExecuted == true).ToList();
                                                if (IsExecuted.Count == ChaseDetailEntitys.Count)
                                                {
                                                    SchemeEntity.SchemeStatus = (short)SchemeStatusEnum.ChaseSuccess;
                                                    blls.ModifyEntity(SchemeEntity);
                                                }
                                                else
                                                {
                                                    IsExecuted = ChaseDetailEntitys.Where(w => w.IsExecuted == false).ToList();
                                                    var QuashStatus = ChaseDetailEntitys.Where(w => w.QuashStatus != (short)QuashStatusEnum.NoQuash).ToList();
                                                    if (IsExecuted.Count == QuashStatus.Count)
                                                    {
                                                        SchemeEntity.SchemeStatus = (short)SchemeStatusEnum.ChaseSuccess;
                                                        blls.ModifyEntity(SchemeEntity);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                SchemeEntity.SchemeStatus = (short)SchemeStatusEnum.OrderRevoke;
                                                bllu.ModifyUserBalanceRedis(SchemeEntity.InitiateUserID, SchemeEntity.SchemeMoney, true);//撤单
                                                bllu.SendWinMoneyContent(order.SchemeID, SchemeEntity.IsuseName, SchemeEntity.SchemeMoney);//撤单推送
                                                blls.ModifyEntity(SchemeEntity);
                                            }
                                        }
                                        blls.OutApply(order.SchemeID, order.ChaseTaskDetailsID, true, DateTime.Now);
                                    }
                                    else
                                    {
                                        List<SchemeETicketsEntity> Entitys = blle.QueryEntitysBySchemeID(order.SchemeID);
                                        if (SchemeEntity != null)
                                        {
                                            var Entitys_3 = Entitys.Where(w => w.TicketStatus == (byte)TicketStatusEnum.BettingFailure).ToList();  //投注失败撤单
                                            if (Entitys_3.Count == Entitys.Count)
                                            {
                                                if (SchemeEntity.BuyType == (byte)BuyTypeEnum.BuyChase)
                                                {
                                                    var ChaseDetailEntity = bblct.QueryEntity(order.ChaseTaskDetailsID);
                                                    if (ChaseDetailEntity != null)
                                                    {
                                                        ChaseDetailEntity.QuashStatus = (short)QuashStatusEnum.SysQuash;
                                                        bllu.ModifyUserBalanceRedis(SchemeEntity.InitiateUserID, ChaseDetailEntity.Amount, true);//撤单
                                                        string IssueName = blli.QueryIssueName(ChaseDetailEntity.IsuseID);
                                                        bllu.SendWinMoneyContent(order.SchemeID, IssueName, ChaseDetailEntity.Amount);//撤单推送
                                                    }
                                                }
                                                else
                                                {
                                                    SchemeEntity.SchemeStatus = (short)SchemeStatusEnum.OrderRevoke;
                                                    bllu.ModifyUserBalanceRedis(SchemeEntity.InitiateUserID, SchemeEntity.SchemeMoney, true);//撤单
                                                    bllu.SendWinMoneyContent(order.SchemeID, SchemeEntity.IsuseName, SchemeEntity.SchemeMoney);//撤单推送
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    blls.OutApply(order.SchemeID, order.ChaseTaskDetailsID, true, DateTime.Now);
                                }
                                catch (Exception ex)
                                {
                                    log_Writ.WritTextBox(String.Format("{0}队列投注出错:\n{1}", BettingBigTicket_QueueName, ex.Message));
                                    IsBettingRevoke = true;
                                }
                                #endregion
                            }
                            //出错撤单
                            if (IsBettingRevoke)
                            {
                                blle.SchemeETicketsWithdrawal(order.SchemeID, "");
                                blls.OutApply(order.SchemeID, order.ChaseTaskDetailsID, true, DateTime.Now);
                                IsBettingRevoke = false;
                            }
                        }
                        //投注是否成功 队列中的方案ID应该删除
                        BettingBigTicket_Queue.ReceiveById(messageA.Id);
                    }
                }
                log_Writ.WritTextBox(QueueExpireTime + "天内没有检测到订单 退出消息队列");
            }
            catch (MessageQueueException ex)
            {
                log_Writ.WritTextBox(String.Format("{0}队列出错:\n{1}", BettingBigTicket_QueueName, ex.Message));
            }
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="sectionName"></param>
        public void BettingBigTicket_InitializeConfiguration()
        {
            string sectionName = "BettingBigTicket";
            var iniFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Config\\MSMQManager.ini";
            var ini = new IniFile(iniFilePath);
            BettingBigTicket_QueueName = ini.Read(sectionName, "QueuePath");
        }

        #region 接口余额检查
        /// <summary>
        /// 检查接口余额
        /// </summary>
        /// <returns></returns>
        public bool DetectionInterfaceBalance(long SchemeID)
        {
            try
            {
                long InterfaceBalance = 0;
                var SchemeEntity = blls.QueryEntity(SchemeID);
                if (SchemeEntity != null)
                {
                    InterfaceBalance = this.HuaYangBalance(SchemeEntity.SchemeMoney, SchemeEntity.SchemeNumber);
                    if (InterfaceBalance > (SchemeEntity.SchemeMoney / 100))
                        return true;
                    else
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log_Writ.WritTextBox(string.Format("大票检测接口余额报错：{0}", ex.Message));
                return true;
            }
        }
        protected long HuaYangBalance(long SchemeMoney, string SchemeNumber)
        {
            XmlNode Node = Utils.QueryConfigNode("root/interface");
            XmlNodeList XmlList = Node.SelectNodes("item");
            XmlNode xml = XmlList[0];
            long InterfaceBalance = 0;
            InterfaceBase InterTicker = new InterfaceBase()[xml];
            if (InterTicker != null)
            {
                string NoticeTel = System.Configuration.ConfigurationManager.AppSettings["NOTICETEL"];
                udv_ParaAccBalance para = new udv_ParaAccBalance();
                para.username = xml.SelectSingleNode("AgenterId").InnerText;
                udv_ResultBalance ResModel = InterTicker.GetAccBalance(para);
                if (ResModel.ErrorCode == "0")
                {
                    InterfaceBalance = Convert.ToInt64(ResModel.ActMoney) / 100;
                    SchemeMoney = SchemeMoney / 100;
                    if (InterfaceBalance < SchemeMoney)
                    {
                        if (!string.IsNullOrEmpty(NoticeTel))
                        {
                            string Content = string.Format("(方案号:{1})华阳接口余额({2})少于当前方案投注金额({0})", SchemeMoney, SchemeNumber, InterfaceBalance);
                            log_Writ.WritTextBox(Content);
                            Task.Factory.StartNew(() =>
                            {
                                SMS.SendModel(NoticeTel, Content);
                            });
                        }
                    }
                }
            }
            return InterfaceBalance;
        }
        #endregion
        #endregion
    }
}
