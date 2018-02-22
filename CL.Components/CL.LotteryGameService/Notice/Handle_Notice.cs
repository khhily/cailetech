using CL.Coupons.BLL;
using CL.Enum.Common.Lottery;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.LotteryGameService.Model;
using CL.Redis.BLL;
using CL.Tools.Common;
using CL.Tools.JiGuangPush;
using CL.Tools.TicketInterface;
using CL.View.Entity.Coupons;
using CL.View.Entity.Game;
using CL.View.Entity.Interface;
using cn.jpush.api.push;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CL.LotteryGameService.Notice
{
    public class Handle_Notice
    {
        /// <summary>
        /// 委托启动程序
        /// </summary>
        /// <param name="rich"></param>
        protected delegate void action_delegate(RichTextBox rich);
        /// <summary>
        /// 委托写日志
        /// </summary>
        protected static WritText log_Writ;
        /// <summary>
        /// 任务调度
        /// </summary>
        private QuartzHelper quartzhelper_notice = new QuartzHelper();


        protected static List<LotteriesEntity> static_LotteriesEntity = null;

        protected static List<TemplateConfigEntity> TemplateConfig = null;

        //protected static List<LotteryRoomEntity> static_LotteryRoomEntity = null;

        protected TemplateConfigBLL bllt = new TemplateConfigBLL();

        protected BusinessRedis redisb = new BusinessRedis();

        protected static string[] TemplateName = new string[] { "Open", "Prep", "End" };

        protected static string OpenTemplate = string.Empty;

        protected static string PrepTemplate = string.Empty;

        protected static string EndTemplate = string.Empty;

        protected static string IntervalTime_Coupons = ConfigurationManager.AppSettings["quartz_coupons"];
        protected static string IntervalTime_GLottery = ConfigurationManager.AppSettings["quartz_glottery"];
        protected static string IntervalTime_DLottery = ConfigurationManager.AppSettings["quartz_dlottery"];
        protected static string IntervalTime_Rebate = ConfigurationManager.AppSettings["quartz_rebate"];
        protected static string G_LotteryCodes = ConfigurationManager.AppSettings["g_LotteryCodes"] ?? "";
        protected static string D_LotteryCodes = ConfigurationManager.AppSettings["d_LotteryCodes"] ?? "";

        protected static int[] g_LotteryCodes = new int[] { };
        protected static int[] d_LotteryCodes = new int[] { };

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
        /// 程序启动
        /// </summary>
        /// <param name="rich"></param>
        protected void Start(RichTextBox rich)
        {
            try
            {
                rich.ForeColor = System.Drawing.Color.Blue;
                log_Writ = new WritText(rich);
                if (!string.IsNullOrEmpty(G_LotteryCodes))
                    g_LotteryCodes = G_LotteryCodes.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                if (!string.IsNullOrEmpty(D_LotteryCodes))
                    d_LotteryCodes = D_LotteryCodes.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                Thread th_Template = new Thread(new ThreadStart(BindTemplate));
                Thread th_Data = new Thread(new ThreadStart(BindData));
                th_Template.Start();
                th_Data.Start();
            }
            catch (Exception ex)
            {
                log_Writ.WritTextBox(string.Format("游戏推送启动失败：{0}", ex.Message));
                throw;
            }
        }
        /// <summary>
        /// 板顶模板
        /// </summary>
        protected void BindTemplate()
        {
            TemplateConfig = bllt.QueryIssuePush();
            if (TemplateConfig != null)
                foreach (var item in TemplateName)
                {
                    var Entity = TemplateConfig.Where(w => w.Title.Trim() == item.Trim()).FirstOrDefault();
                    if (Entity != null && !string.IsNullOrEmpty(Entity.TemplateContent))
                        switch (item)
                        {
                            case "Open":
                                OpenTemplate = Entity.TemplateContent.Trim();
                                break;
                            case "Prep":
                                PrepTemplate = Entity.TemplateContent.Trim();
                                break;
                            case "End":
                                EndTemplate = Entity.TemplateContent.Trim();
                                break;
                        }
                }
            log_Writ.WritTextBox(string.Format("初始化Open值为：{0}", OpenTemplate));
            log_Writ.WritTextBox(string.Format("初始化Prep值为：{0}", PrepTemplate));
            log_Writ.WritTextBox(string.Format("初始化End值为：{0}", EndTemplate));
        }

        protected void BindData()
        {
            #region 加载
            log_Writ.WritTextBox(" --------------------------------- 开始加载配置信息 --------------------------------- ");
            log_Writ.WritTextBox(string.Format("quartz_coupons:{0}", IntervalTime_Coupons));
            log_Writ.WritTextBox(string.Format("IntervalTime_GLottery:{0}", IntervalTime_GLottery));
            log_Writ.WritTextBox(string.Format("IntervalTime_DLottery:{0}", IntervalTime_DLottery));
            log_Writ.WritTextBox(string.Format("g_LotteryCodes:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(g_LotteryCodes)));
            log_Writ.WritTextBox(string.Format("d_LotteryCodes:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(d_LotteryCodes)));

            static_LotteriesEntity = new LotteriesBLL().QueryLotterys();
            //static_LotteryRoomEntity = new LotteryRoomBLL().QueryEntitys();
            log_Writ.WritTextBox(Newtonsoft.Json.JsonConvert.SerializeObject(static_LotteriesEntity.Select(s => new { s.LotteryCode, s.LotteryName, s.ChatRooms, s.Shorthand, s.IsStop, s.AdvanceEndTime })));

            log_Writ.WritTextBox(" --------------------------------- 配置信息加载完成 --------------------------------- ");
            #endregion
            #region 彩券通知
            string jobname_coupons = "jobname_coupons";
            string jobgroup_coupons = "jobgroup_coupons";
            quartzhelper_notice.AddTrigger(jobname_coupons, jobgroup_coupons, IntervalTime_Coupons, new Action(() =>
            {
                try
                {
                    HandleCoupons();
                }
                catch (Exception ex)
                {
                    log_Writ.WritTextBox(string.Format("彩券通知定时器发生错误 --> {0}", ex.StackTrace));
                }
            }));
            #endregion
            #region 彩种期号通知
            string jobname_glottery = "jobname_lottery";
            string jobgroup_glottery = "jobgroup_lottery";
            quartzhelper_notice.AddTrigger(jobname_glottery, jobgroup_glottery, IntervalTime_GLottery, new Action(() =>
            {
                try
                {
                    foreach (int LotteryCode in g_LotteryCodes)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            HandleLotteryInfo(LotteryCode);
                        });
                    }
                }
                catch (Exception ex)
                {
                    log_Writ.WritTextBox(string.Format("高频彩期号通知定时器发生错误 --> {0}", ex.StackTrace));
                }
            }));
            string jobname_dlottery = "jobname_lottery";
            string jobgroup_dlottery = "jobgroup_lottery";
            quartzhelper_notice.AddTrigger(jobname_dlottery, jobgroup_dlottery, IntervalTime_DLottery, new Action(() =>
            {
                try
                {
                    foreach (int LotteryCode in d_LotteryCodes)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            HandleLotteryInfo(LotteryCode);
                        });
                    }
                }
                catch (Exception ex)
                {
                    log_Writ.WritTextBox(string.Format("低频彩期号通知定时器发生错误 --> {0}", ex.StackTrace));
                }
            }));
            #endregion
            #region 启动调度
            Task.Factory.StartNew(() =>
            {
                try
                {
                    quartzhelper_notice.Start();
                }
                catch (Exception ex)
                {
                    log_Writ.WritTextBox(string.Format("启动调度[quartzhelper] --> {0}", ex.StackTrace));
                }
            });
        }
        #endregion
        #region 定义方法
        /// <summary>
        /// 期号
        /// </summary>

        public void HandleLotteryInfo(int LotteryCode)
        {
            try
            {
                QuartzHelper quartzhelper_lottery = new QuartzHelper();
                var LotteryInfoEntity = static_LotteriesEntity.Where(w => w.LotteryCode == LotteryCode).FirstOrDefault();
                if (LotteryInfoEntity == null)
                    return;
                string Subhead = LotteryInfoEntity.Subhead;
                string Chatroom_ID = LotteryInfoEntity.ChatRooms;
                //string[] IMRooms = static_LotteryRoomEntity.Where(w => w.LotteryCode == LotteryCode).Select(s => Convert.ToString(s.RoomID)).ToArray();
                if (string.IsNullOrEmpty(Chatroom_ID))
                    return;
                var Entity = redisb.CurrentIsuseRedis(LotteryCode);
                if (Entity == null)
                {
                    Entity = new IsusesBLL().QueryLotteryCurrIsuses(LotteryCode);
                    if (Entity != null)
                    {
                        #region ---------- 读取接口期号数据 红快3 赢快3 重庆时时彩 江西时时彩(四个彩种华阳接口期次号规则与官网期次号规则不同) ---------- 
                        if (LotteryCode == (int)LotteryInfo.JLK3 || LotteryCode == (int)LotteryInfo.JXK3 || LotteryCode == (int)LotteryInfo.CQSSC || LotteryCode == (int)LotteryInfo.JXSSC)
                        {
                            try
                            {
                                XmlNode Node = Utils.QueryConfigNode("root/interface");
                                if (Node != null)
                                {
                                    XmlNodeList XmlList = Node.SelectNodes("item");
                                    log_Writ.WritTextBox(string.Format("({1})开始对比期号[彩乐-华阳]:{0}", Entity.IsuseName, Enum.Common.Common.GetDescription((LotteryInfo)Entity.LotteryCode)));
                                    foreach (XmlNode xml in XmlList)
                                    {
                                        if (Convert.ToInt32(xml.SelectSingleNode("systemlotterycode").InnerText) == LotteryCode)
                                        {
                                            InterfaceBase InterTicker = new InterfaceBase()[xml];
                                            udv_ParaIsuse ParaIsuse = new udv_ParaIsuse()
                                            {
                                                LotteryID = Entity.LotteryCode.ToString(),
                                                Issue = Entity.IsuseName.Substring(2, Entity.IsuseName.Length - 2)
                                            };
                                            udv_ResultIssue ResModel = InterTicker.GetIsuseInfo(ParaIsuse);
                                            if (ResModel.ErrorCode == "0")
                                            {
                                                Entity.StartTime = DateTime.Parse(ResModel.StartTime);
                                                Entity.EndTime = DateTime.Parse(ResModel.EndTime);

                                                #region 更新对象
                                                var et = new IsusesBLL().QueryEntity(Entity.IsuseID);
                                                et.StartTime = DateTime.Parse(ResModel.StartTime);
                                                et.EndTime = DateTime.Parse(ResModel.EndTime);
                                                new IsusesBLL().ModifyEntity(et);
                                                #endregion

                                                #region 更新下一期对象
                                                int IsuseNum = Convert.ToInt32(Entity.IsuseName.Substring(9, Entity.IsuseName.Length - 9)) + 1;
                                                string Day = Entity.IsuseName.Substring(0, 8);
                                                string[] arr = (Entity.IntervalType ?? "").Split('@');
                                                if (Convert.ToInt32(arr[2]) >= IsuseNum)
                                                {
                                                    string CombinationIsuseName = string.Format("{0}{1}", Day, IsuseNum.ToString().PadLeft(3, '0'));
                                                    var NextIssueEntity = new IsusesBLL().QueryEntitysByLotteryCode(LotteryCode, CombinationIsuseName);
                                                    if (NextIssueEntity != null)
                                                    {
                                                        NextIssueEntity.StartTime = Entity.EndTime;
                                                        //NextIssueEntity.EndTime = NextIssueEntity.EndTime.AddMinutes(10);
                                                        new IsusesBLL().ModifyEntity(NextIssueEntity);
                                                    }
                                                }
                                                #endregion
                                                break;
                                            }
                                        }
                                    }
                                    log_Writ.WritTextBox(string.Format("({1})期号对比完成[彩乐-华阳]:{0}", Entity.IsuseName, Enum.Common.Common.GetDescription((LotteryInfo)Entity.LotteryCode)));
                                }
                            }
                            catch (Exception ex)
                            {
                                log_Writ.WritTextBox(string.Format("华阳接口期次号规则与官网期次号规则不同错误 --> {0}", ex.StackTrace));

                            }
                        }
                        #endregion
                        var EndTime = Entity.EndTime;
                        var StartTime = Entity.StartTime;
                        if (Entity.AdvanceEndTime > 0)
                            EndTime = Convert.ToDateTime(Entity.EndTime).AddMinutes(-Entity.AdvanceEndTime);
                        if (Entity.PresellTime > 0)
                            StartTime = Convert.ToDateTime(Entity.StartTime).AddMinutes(-Entity.PresellTime);
                        Entity.EndTime = EndTime;
                        Entity.StartTime = StartTime;
                        #region ---------- 保存到Redis ----------
                        redisb.CurrentIsuseRedis(Entity, EndTime);
                        #endregion
                        int totalIsuse = 0;
                        if (LotteryCode != 801 && LotteryCode != 901)
                        {
                            string[] arr = (Entity.IntervalType ?? "").Split('@');
                            if (arr.Length >= 3)
                            {
                                totalIsuse = Convert.ToInt32(arr[2]);
                            }
                        }
                        #region IM发送处理

                        #region 开期
                        string jobname_open = string.Format("jobname_open_{0}_{1}", LotteryCode, Entity.IsuseName);
                        string jobgroup_open = string.Format("jobgroup_open_{0}_{1}", LotteryCode, Entity.IsuseName);
                        if (string.IsNullOrEmpty(new SystemRedis().GetScheduler(jobname_open)))
                        {
                            if (LotteryCode == (int)LotteryInfo.SSQ || LotteryCode == (int)LotteryInfo.CJDLT)
                                new SystemRedis().SetScheduler_d(jobname_open);
                            else
                                new SystemRedis().SetScheduler(jobname_open);
                            string TemplateStr = string.Format(OpenTemplate, Enum.Common.Common.GetDescription((LotteryInfo)LotteryCode), Entity.IsuseName, Subhead);
                            bool Rec = new Game.BLL.IM.Communication().Send_Api_Chatrooms_Notice(Chatroom_ID, TemplateStr);
                            log_Writ.WritTextBox(string.Format("[{0}]开期消息回调：{1}", Enum.Common.Common.GetDescription((LotteryInfo)LotteryCode), Rec));
                        }
                        #endregion
                        #region 预截期
                        string jobname_prep = string.Format("jobname_prep_{0}_{1}", LotteryCode, Entity.IsuseName);
                        string jobgroup_prep = string.Format("jobgroup_prep_{0}_{1}", LotteryCode, Entity.IsuseName);
                        if (string.IsNullOrEmpty(new SystemRedis().GetScheduler(jobname_prep)))
                        {
                            if (LotteryCode == (int)LotteryInfo.SSQ || LotteryCode == (int)LotteryInfo.CJDLT)
                                new SystemRedis().SetScheduler_d(jobname_open);
                            else
                                new SystemRedis().SetScheduler(jobname_prep);
                            DateTime time = Entity.EndTime;
                            string IntervalTime_Prep = string.Empty;
                            string TemplateStr = string.Empty;
                            int AdvanceEndTime = 0;
                            if (LotteryInfoEntity.AdvanceEndTime != null)
                                AdvanceEndTime = Convert.ToInt32(LotteryInfoEntity.AdvanceEndTime);
                            //时间格式0 0 0 29 3 2014
                            if (LotteryCode == (int)LotteryInfo.SSQ || LotteryCode == (int)LotteryInfo.CJDLT)
                            {
                                IntervalTime_Prep = time.AddMinutes(-30).ToString("ss mm HH dd MM ? yyyy");//yyyy-MM-dd HH:mm:ss
                                TemplateStr = string.Format(PrepTemplate, Enum.Common.Common.GetDescription((LotteryInfo)LotteryCode), Entity.IsuseName, 30);
                            }
                            else
                            {
                                IntervalTime_Prep = time.AddMinutes(-1).ToString("ss mm HH dd MM ? yyyy");//yyyy-MM-dd HH:mm:ss
                                TemplateStr = string.Format(PrepTemplate, Enum.Common.Common.GetDescription((LotteryInfo)LotteryCode), Entity.IsuseName, 1);
                            }
                            quartzhelper_lottery.AddTrigger(jobname_prep, jobgroup_prep, IntervalTime_Prep, new Action(() =>
                            {
                                var Rec = new Game.BLL.IM.Communication().Send_Api_Chatrooms_Notice(Chatroom_ID, TemplateStr);
                                log_Writ.WritTextBox(string.Format("[{0}]预结期消息回调：{1}", Enum.Common.Common.GetDescription((LotteryInfo)LotteryCode), Rec));
                                HandleChaseTasks(LotteryCode, Entity.IsuseName);
                            }));
                        }
                        #endregion
                        #region 截期
                        string jobname_over = string.Format("jobname_over_{0}_{1}", LotteryCode, Entity.IsuseName);
                        string jobgroup_over = string.Format("jobgroup_over_{0}_{1}", LotteryCode, Entity.IsuseName);
                        if (string.IsNullOrEmpty(new SystemRedis().GetScheduler(jobname_over)))
                        {
                            if (LotteryCode == (int)LotteryInfo.SSQ || LotteryCode == (int)LotteryInfo.CJDLT)
                                new SystemRedis().SetScheduler_d(jobname_open);
                            else
                                new SystemRedis().SetScheduler(jobname_over);
                            string IntervalTime_Over = string.Empty;
                            string TemplateStr = string.Format(EndTemplate,
                                                Enum.Common.Common.GetDescription((LotteryInfo)LotteryCode), Entity.IsuseName);
                            IntervalTime_Over = Entity.EndTime.AddSeconds(-1).ToString("ss mm HH dd MM ? yyyy");//yyyy-MM-dd HH:mm:ss
                            //时间格式0 0 0 29 3 2014
                            quartzhelper_lottery.AddTrigger(jobname_over, jobgroup_over, IntervalTime_Over, new Action(() =>
                            {
                                var Rec = new Game.BLL.IM.Communication().Send_Api_Chatrooms_Notice(Chatroom_ID, TemplateStr);
                                log_Writ.WritTextBox(string.Format("[{0}]结期消息回调：{1}", Enum.Common.Common.GetDescription((LotteryInfo)LotteryCode), Rec));
                            }));
                        }
                        #endregion
                        #endregion
                        #region 启动调度
                        Task.Factory.StartNew(new Action(() =>
                        {
                            try
                            {
                                quartzhelper_lottery.ReStart();
                            }
                            catch (Exception ex)
                            {
                                log_Writ.WritTextBox(string.Format("启动调度[quartzhelper] --> {0}", ex.StackTrace));
                            }
                        }));
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log_Writ.WritTextBox(string.Format("处理彩种期号发生错误({0})", ex.Message));
            }
        }

        /// <summary>
        /// 极光推送
        /// </summary>
        protected void HandleCoupons()
        {
            try
            {
                List<udv_CouponsExpireTimeList> Coupons = new CouponsBLL().QueryCouponsExpireTimeList();
                string NoticeContent = new TemplateConfigBLL().QueryCouponsExpireTime();
                string TempContent = string.Empty;
                //亲爱的{0}会员，您有{1}元彩券即将过期。
                var Entitys = Coupons.GroupBy(g => new { g.UserName, g.PushIdentify }).Select(s => s.Key).ToArray();
                foreach (var item in Entitys)
                {
                    var Entity = Coupons.Where(w => w.PushIdentify == item.PushIdentify && w.UserName == item.UserName).OrderByDescending(o => o.FaceValue).FirstOrDefault();
                    long CouponsMoney = (Entity.FaceValue) / 100;
                    string[] PushIds = new string[] { item.PushIdentify };
                    TempContent = string.Format(NoticeContent, item.UserName, CouponsMoney);
                    MessageResult result = null;
                    new PushHelper().PushPersonal(PushIds, "彩券到期通知", TempContent, string.Empty, ref result);
                    log_Writ.WritTextBox(string.Format("[{0}:{1}]彩券到期通知极光推送回调({2}:{3})", item.UserName, item.PushIdentify, result.msg_id, result.sendno));
                }
            }
            catch (Exception ex)
            {
                log_Writ.WritTextBox(string.Format("处理彩券发生错误({0})", ex.Message));
            }
        }

        /// <summary>
        /// 处理截期还未追号的数据
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        protected void HandleChaseTasks(int LotteryCode, string IsuseName)
        {
            try
            {
                List<udv_ExpireRevokeChase> Entitys = new ChaseTaskDetailsBLL().QueryExpireRevokeChase(LotteryCode, IsuseName);
                if (Entitys != null && Entitys.Count > 0)
                {
                    //亲爱的{0}会员，你购买的{1}第{2}期订单[单号：{3}]撤单，撤单金额为{4}元
                    string TemplateStr = new TemplateConfigBLL().QueryRevokePushTemplate();
                    if (string.IsNullOrEmpty(TemplateStr))
                        return;
                    foreach (udv_ExpireRevokeChase Entity in Entitys)
                    {
                        string Content = string.Format(TemplateStr, Entity.Nick, Enum.Common.Common.GetDescription((LotteryInfo)LotteryCode), IsuseName, Entity.SchemeNumber, Entity.Amount);
                        #region 短信
                        try
                        {
                            if (!string.IsNullOrEmpty(Entity.Mobile))
                                SMS.SendModel(Entity.Mobile, TemplateStr);
                        }
                        catch (Exception ex)
                        {
                            string Msg = string.Format("撤单短信发送错误：{0}", ex.Message);
                            log_Writ.WritTextBox(Msg);
                            new Log("Push").Write(Msg, true);
                        }
                        #endregion
                        #region 推送
                        try
                        {
                            //极光推送
                            new UsersPushBLL().PushMessager_Android_Single(Entity.UserID, "彩乐撤单", TemplateStr, string.Empty);
                        }
                        catch (Exception ex)
                        {
                            string Msg = string.Format("撤单极光推送错误：{0}", ex.Message);
                            log_Writ.WritTextBox(Msg);
                            new Log("Push").Write(Msg, true);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                log_Writ.WritTextBox(string.Format("处理截期还未追号的数据发生错误({0})", ex.Message));
            }
        }
        #endregion
    }
}
