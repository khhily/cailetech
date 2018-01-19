using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Linq;
using CL.Tools.Common;
using CL.Admin.UI;
using CL.SystemInfo.BLL;
using CL.SystemInfo.Entity;
using CL.View.Entity.SystemInfo;
using CL.Game.BLL;
using CL.Enum.Common;
using CL.Game.Entity;
using CL.View.Entity.Other;
using CL.Entity.Json.Pay;
using CL.Redis.BLL;
using CL.Enum.Common.Status;
using System.Text;
using CL.View.Entity.Game;
using CL.Enum.Common.Type;
using CL.Tools.LotteryTickets;

namespace Admin.tools
{
    /// <summary>
    /// admin_ajax 的摘要说明
    /// </summary>
    public class admin_ajax : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            //检查管理员是否登录
            if (!new AdminPage().IsAdminLogin())
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"尚未登录或已超时，请登录后操作！\"}");
                return;
            }
            //取得处事类型
            string action = QPRequest.GetQueryString("action");

            switch (action)
            {
                case "navigation_validate": //验证导航菜单别名是否重复
                    navigation_validate(context);
                    break;
                case "get_navigation_list": //获取后台导航字符串
                    get_navigation_list(context);
                    break;
                case "manager_validate": //验证管理员用户名是否重复
                    manager_validate(context);
                    break;
                case "rosle_validate": //验证角色名称是否重复
                    rosle_validate(context);
                    break;
                case "lotterycode_validate": //验证彩种编码是否重复
                    lotterycode_validate(context);
                    break;
                case "playcode_validate":   //验证玩法编码是否重复
                    playcode_validate(context);
                    break;
                case "wincode_validate":    //验证奖等编码是否重复
                    wincode_validate(context);
                    break;
                case "create_isuse":    //批量创建期号
                    create_isuse(context);
                    break;
                case "isusename_validate":  //验证期号是否存在
                    isusename_validate(context);
                    break;
                case "wintypes_list":   //获取奖等列表
                    wintypes_list(context);
                    break;
                case "setwinmoney":    //设置中奖方案
                    SetWinMoney(context);
                    break;
                case "username_validate":   //验证用户账号是否存在
                    username_validate(context);
                    break;
                //case "sethomeowners":   //设置房主
                //    sethomeowners(context);
                //    break;
                case "template":    //获取模板信息
                    getTemplate(context);
                    break;
                case "Refund":    //充值退款
                    PostRefund(context);
                    break;
                case "PostRefundQuery": //查询充值退款到帐没有
                    PostRefundQuery(context);
                    break;
                case "Lotteries_IsStop":    //设置彩种停售
                    LotteriesIsStop(context);
                    break;
                case "Lotteries_IsHot":    //设置热门推荐
                    LotteriesIsHot(context);
                    break;
                case "syn_client":    //发送环信消息
                    Syn_Client(context);
                    break;
                case "quashScheme_and_syn_client":    //撤单并同步客户端
                    QuashScheme_And_Syn_Client(context);
                    break;
                case "withdraw":    //体现审核
                    Withdraw(context);
                    break;
                case "replenishment"://充值补单
                    Replenishment(context);
                    break;
                case "userpushlist"://充值补单
                    UserPushList(context);
                    break;
                case "regular"://充值补单
                    Regular(context);
                    break;
            }
        }

        #region 验证导航菜单别名是否重复
        private void navigation_validate(HttpContext context)
        {
            string navname = QPRequest.GetString("param");
            string old_name = QPRequest.GetString("old_name");
            if (string.IsNullOrEmpty(navname))
            {
                context.Response.Write("{ \"info\":\"该导航别名不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (navname.ToLower() == old_name.ToLower())
            {
                context.Response.Write("{ \"info\":\"该导航别名可使用\", \"status\":\"y\" }");
                return;
            }
            //检查保留的名称开头
            if (navname.ToLower().StartsWith("channel_"))
            {
                context.Response.Write("{ \"info\":\"该导航别名系统保留，请更换！\", \"status\":\"n\" }");
                return;
            }
            NavigationBLL bll = new NavigationBLL();
            if (bll.Exists(navname))
            {
                context.Response.Write("{ \"info\":\"该导航别名已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该导航别名可使用\", \"status\":\"y\" }");
            return;
        }
        #endregion

        #region 获取后台导航字符串
        private void get_navigation_list(HttpContext context)
        {
            ManagerEntity adminModel = new AdminPage().GetAdminInfo(); //获得当前登录管理员信息
            if (adminModel == null)
            {
                return;
            }
            RosleEntity roleModel = new RosleBLL().GetModelByCache(adminModel.RoleID); //获得管理角色信息
            if (roleModel == null)
            {
                return;
            }
            List<udv_Navigation> dt = new NavigationBLL().GetListCache(0);
            this.get_navigation_childs(context, dt, 0, roleModel.RoleType, roleModel.manager_role_values);

        }
        private void get_navigation_childs(HttpContext context, List<udv_Navigation> oldData, int parent_id, int role_type, List<RosleValueEntity> ls)
        {
            List<udv_Navigation> dr = oldData.FindAll(p => p.ParentID == parent_id);

            bool isWrite = false; //是否输出开始标签
            int i = 0;
            foreach (udv_Navigation nav in dr)
            {
                //检查是否显示在界面上====================
                bool isActionPass = true;
                if (nav.IsLock == 1)
                {
                    isActionPass = false;
                }
                //检查管理员权限==========================
                if (isActionPass && role_type > 1)
                {
                    string[] actionTypeArr = nav.ActionType.Split(',');
                    foreach (string action_type_str in actionTypeArr)
                    {
                        //如果存在显示权限资源，则检查是否拥有该权限
                        if (action_type_str == "Show")
                        {
                            RosleValueEntity modelt = ls.Find(p => p.NavName == nav.Name && p.ActionType == "Show");
                            if (modelt == null)
                            {
                                isActionPass = false;
                            }
                        }
                    }
                }
                //如果没有该权限则不显示
                if (!isActionPass)
                {
                    if (isWrite && i == (dr.Count - 1) && parent_id > 0)
                    {
                        context.Response.Write("</ul>\n");
                    }
                    continue;
                }
                //如果是顶级导航
                if (parent_id == 0)
                {
                    context.Response.Write("<div class=\"list-group\">\n");
                    context.Response.Write("<h1 title=\"" + nav.Title + "\">");
                    context.Response.Write("</h1>\n");
                    context.Response.Write("<div class=\"list-wrap\">\n");
                    context.Response.Write("<h2>" + nav.Title + "<i></i></h2>\n");
                    //调用自身迭代
                    this.get_navigation_childs(context, oldData, nav.id, role_type, ls);
                    context.Response.Write("</div>\n");
                    context.Response.Write("</div>\n");
                }
                else //下级导航
                {
                    if (!isWrite)
                    {
                        isWrite = true;
                        context.Response.Write("<ul>\n");
                    }
                    context.Response.Write("<li>\n");
                    context.Response.Write("<a navid=\"" + nav.Name + "\"");
                    if (!string.IsNullOrEmpty(nav.LinkUrl))
                    {
                        context.Response.Write(" href=\"" + nav.LinkUrl + "\" target=\"mainframe\"");
                    }

                    context.Response.Write(" target=\"mainframe\">\n");
                    context.Response.Write("<span>" + nav.Title + "</span>\n");
                    context.Response.Write("</a>\n");
                    //调用自身迭代
                    this.get_navigation_childs(context, oldData, nav.id, role_type, ls);
                    context.Response.Write("</li>\n");

                    if (i == (dr.Count - 1))
                    {
                        context.Response.Write("</ul>\n");
                    }
                }
                i++;
            }
        }
        #endregion

        #region 验证管理员用户名是否重复
        private void manager_validate(HttpContext context)
        {
            string user_name = QPRequest.GetString("param");
            if (string.IsNullOrEmpty(user_name))
            {
                context.Response.Write("{ \"info\":\"请输入用户名\", \"status\":\"n\" }");
                return;
            }
            ManagerBLL bll = new ManagerBLL();
            if (bll.Exists(user_name))
            {
                context.Response.Write("{ \"info\":\"用户名已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"用户名可使用\", \"status\":\"y\" }");
            return;
        }
        #endregion

        #region 验证角色名称是否重复
        private void rosle_validate(HttpContext context)
        {
            string user_name = QPRequest.GetString("param");
            if (string.IsNullOrEmpty(user_name))
            {
                context.Response.Write("{ \"info\":\"请输入角色名称\", \"status\":\"n\" }");
                return;
            }
            RosleBLL bll = new RosleBLL();
            if (bll.Exists(user_name))
            {
                context.Response.Write("{ \"info\":\"角色名称已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"角色名称可使用\", \"status\":\"y\" }");
            return;
        }
        #endregion

        #region 验证彩种编码是否重复
        private void lotterycode_validate(HttpContext context)
        {
            string sCode = QPRequest.GetString("param");
            int lotterycode = Convert.ToInt32(sCode);
            if (lotterycode <= 0)
            {
                context.Response.Write("{ \"info\":\"请输入彩种编码\", \"status\":\"n\" }");
                return;
            }
            LotteriesBLL bll = new LotteriesBLL();
            if (bll.ExistsCode(lotterycode))
            {
                context.Response.Write("{ \"info\":\"彩种编码已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"彩种编码可使用\", \"status\":\"y\" }");
            return;
        }
        #endregion

        #region 验证玩法编码是否重复
        private void playcode_validate(HttpContext context)
        {
            string sCode = QPRequest.GetString("param");
            int playtypescode = Convert.ToInt32(sCode);
            if (playtypescode <= 0)
            {
                context.Response.Write("{ \"info\":\"请输入玩法编码\", \"status\":\"n\" }");
                return;
            }
            PlayTypesBLL bll = new PlayTypesBLL();
            if (bll.ExistsCode(playtypescode))
            {
                context.Response.Write("{ \"info\":\"玩法编码已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"玩法编码可使用\", \"status\":\"y\" }");
            return;
        }
        #endregion

        #region 验证奖等编码是否重复
        private void wincode_validate(HttpContext context)
        {
            int WinCode = QPRequest.GetQueryInt("param");
            string message = string.Empty;
            string status = string.Empty;
            new WinTypesBLL().QueryExistsCode(WinCode, ref message, ref status);
            context.Response.Write("{ \"info\":\"" + message + "\", \"status\":\"" + status + "\" }");
            return;
        }
        #endregion

        #region 批量创建期号
        private void create_isuse(HttpContext context)
        {
            int LotteryCode = QPRequest.GetInt("LotteryCode", 0);
            string sDate = QPRequest.GetString("DateValue");
            int Days = QPRequest.GetInt("Days", 0);
            int itype = QPRequest.GetInt("Type", 0);

            if (sDate.Length == 0)
            {
                context.Response.Write("{\"status\": 1, \"msg\": \"请填写日期！\"}");
                return;
            }
            if (Days <= 0 && itype == 1)
            {
                context.Response.Write("{\"status\": 2, \"msg\": \"请填写天数！\"}");
                return;
            }
            int iRueslt = -1;
            if (itype == 1)
            {
                if (LotteryCode == 301)
                    iRueslt = new IsusesBLL().GenerateIssue(LotteryCode, sDate, Days);
                else
                    iRueslt = new IsusesBLL().AddIsuseAdd(LotteryCode, sDate, Days);
            }
            else if (itype == 5)
            {
                int IsuseName = QPRequest.GetInt("IsuseName", 0);
                iRueslt = new IsusesBLL().AddIssueLuck(IsuseName, LotteryCode, sDate, Days);
            }
            else if (itype == 4)
                iRueslt = new IsusesBLL().AddIsuseAddFC(LotteryCode, sDate);
            if (iRueslt < 0)
            {
                context.Response.Write("{\"status\": " + iRueslt + ", \"msg\": \"批量创建期号失败！\"}");
                return;
            }
            AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "批量创建期号，彩种编号:" + LotteryCode); //记录日志
            context.Response.Write("{\"status\": 0, \"msg\": \"批量创建期号成功！\"}");
        }

        public bool ModifyIsuseInfo_Foreign(int LotteryCode)
        {
            try
            {
                int IsuseName = 0;
                IsusesEntity IsuseEntity = null;
                IsusesBLL IssueBLL = new IsusesBLL();
                DateTime IsuseStartTime = DateTime.Now;
                DateTime IsuseEndTime = DateTime.Now;
                List<IsusesEntity> ls = new List<IsusesEntity>();
                DateTime dt = Convert.ToDateTime(string.Format("{0} 20:00:00", DateTime.Now.ToString("yyyy-MM-dd")));
                DateTime DrawTime = DateTime.Now;
                int Seconds_Span = 3 * 60 + 30;
                TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0);
                string url = string.Format("http://lotto.bclc.com/services2/keno/draw/latest/today?_={0}", ts.TotalMilliseconds);
                string Json = Utils.HttpGet(url, 10000);
                if (string.IsNullOrEmpty(Json))
                    return false;
                List<JNDAward> Entitys = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JNDAward>>(Json);
                if (Entitys != null && Entitys.Count > 0)
                {
                    var Entity = Entitys[0];
                    #region 检查当前期期号是否存在
                    IsuseName = Entity.drawNbr;
                    IsuseEntity = IssueBLL.QueryEntitysByLotteryCode(LotteryCode, IsuseName.ToString());
                    if (IsuseEntity == null)
                    {
                        IsuseStartTime = DrawTime.AddSeconds(-Seconds_Span);
                        IsuseEndTime = DrawTime;
                        IsuseEntity = new IsusesEntity();
                        IsuseEntity.LotteryCode = LotteryCode;
                        IsuseEntity.IsuseName = IsuseName.ToString();
                        IsuseEntity.StartTime = IsuseStartTime;
                        IsuseEntity.EndTime = IsuseEndTime;
                        IsuseEntity.IsuseState = 0;
                        IsuseEntity.UpdateTime = DateTime.Now;
                        IsuseEntity.OpenNumber = string.Empty;
                        IsuseEntity.OpenNotice = string.Empty;
                        IsuseEntity.BettingPrompt = string.Empty;
                        IsuseEntity.WinRollover = string.Empty;
                        ls.Add(IsuseEntity);
                        #endregion
                        IsuseName = (Entity.drawNbr + 1);
                        IsuseEntity = IssueBLL.QueryEntitysByLotteryCode(LotteryCode, IsuseName.ToString());
                        if (IsuseEntity == null) //开始维护当天期号(时间小于20：00)
                        {
                            DrawTime = Convert.ToDateTime(Entity.drawTime);
                            DrawTime = Convert.ToDateTime(string.Format("{0} {1}", Convert.ToDateTime(Entity.drawDate).ToString("yyyy-MM-dd"), DrawTime.TimeOfDay));
                            DrawTime = DrawTime.AddHours(16); //北京时间东八区 温哥华时间西八区 计算时区的时差
                            if (DrawTime > dt) //维护到第二天晚上八点
                                dt = dt.AddDays(1);
                            IsuseStartTime = DrawTime;
                            IsuseEndTime = DrawTime.AddSeconds(Seconds_Span);
                            while (IsuseEndTime <= dt)
                            {
                                IsuseEntity = new IsusesEntity();
                                IsuseEntity.LotteryCode = LotteryCode;
                                IsuseEntity.IsuseName = IsuseName.ToString();
                                IsuseEntity.StartTime = IsuseStartTime;
                                IsuseEntity.EndTime = IsuseEndTime;
                                IsuseEntity.IsuseState = 0;
                                IsuseEntity.UpdateTime = DateTime.Now;
                                IsuseEntity.OpenNumber = string.Empty;
                                IsuseEntity.OpenNotice = string.Empty;
                                IsuseEntity.BettingPrompt = string.Empty;
                                IsuseEntity.WinRollover = string.Empty;

                                IsuseName += 1;
                                IsuseStartTime = IsuseEndTime;
                                IsuseEndTime = IsuseEndTime.AddSeconds(Seconds_Span);
                                ls.Add(IsuseEntity);
                            }
                            IssueBLL.InsertEntity(ls);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                new Log("admin_ajax").Write(string.Format("维护当天期号错误：{0}", ex.Message), true);
                return false;
            }
        }
        #endregion

        #region 验证期号是否存在
        private void isusename_validate(HttpContext context)
        {
            int LotteryCode = QPRequest.GetInt("LotteryCode", 0);
            string IsuseName = QPRequest.GetString("param");

            if (LotteryCode <= 0)
            {
                context.Response.Write("{\"status\": 1, \"msg\": \"请选择彩种！\"}");
                return;
            }
            if (IsuseName.Length == 0)
            {
                context.Response.Write("{\"status\": 2, \"msg\": \"请填写期号！\"}");
                return;
            }

            if (!new IsusesBLL().Exists(IsuseName, LotteryCode))
            {
                context.Response.Write("{\"status\": -1, \"msg\": \"期号不存在！\"}");
                return;
            }
            IsusesEntity model = new IsusesBLL().QueryEntitysByLotteryCode(LotteryCode, IsuseName);
            context.Response.Write("{\"status\": 0, \"msg\": \"获取开奖号码成功！\",\"OpenNumber\":\"" + model.OpenNumber + "\"}");
        }
        #endregion

        #region 获取奖等列表
        private void wintypes_list(HttpContext context)
        {
            udv_JsonResult<List<WinTypesEntity>> Retjson = new udv_JsonResult<List<WinTypesEntity>>();

            int LotteryCode = QPRequest.GetInt("LotteryCode", 0);
            if (LotteryCode <= 0)
            {
                Retjson.Code = 1;
                Retjson.Msg = "获取奖等列表失败";
                context.Response.Write(JsonHelper.ObjectToJSON(Retjson));
                return;
            }

            List<WinTypesEntity> list = new WinTypesBLL().QueryEntitysByLotteryCode(LotteryCode);
            Retjson.Code = 0;
            Retjson.Msg = "获取奖等列表成功";
            Retjson.Data = list.Select(s => new WinTypesEntity()
            {
                DefaultMoney = s.DefaultMoney == null ? 0 : Convert.ToInt32(Convert.ToInt32(s.DefaultMoney) / 100),
                DefaultMoneyNoWithTax = s.DefaultMoneyNoWithTax == null ? 0 : Convert.ToInt32(Convert.ToInt32(s.DefaultMoneyNoWithTax) / 100),
                IsSumValue = s.IsSumValue,
                LotteryCode = s.LotteryCode,
                Sort = s.Sort,
                WinCode = s.WinCode,
                WinID = s.WinID,
                WinName = s.WinName,
                WinNumber = s.WinNumber

            }).ToList();
            context.Response.Write(JsonHelper.ObjectToJSON(Retjson));
        }
        #endregion

        #region 获取奖等信息
        private void wintypes_info(HttpContext context)
        {
            int WinID = QPRequest.GetInt("WinID", 0);
            WinTypesEntity model = new WinTypesBLL().QueryEntity(WinID);
            context.Response.Write("{\"status\": 0, \"msg\": \"获取奖等信息成功！\",\"DefaultMoney\":\"" + model.DefaultMoney + "\",\"DefaultMoneyNoWithTax\":\"" + model.DefaultMoneyNoWithTax + "\"}");
            return;
        }
        #endregion

        #region 设置中奖方案
        private void SetWinMoney(HttpContext context)
        {
            int SchemeID = QPRequest.GetInt("SchemeID", 0);
            if (SchemeID == 0)
            {
                context.Response.Write("{\"status\": -1, \"msg\": \"方案号不正确！\"}");
                return;
            }

            SchemesBLL bll = new SchemesBLL();
            SchemesEntity model = bll.QueryEntity(SchemeID);

            if (model.SchemeStatus == (short)SchemeStatusEnum.InPayment)
            {
                context.Response.Write("{\"status\": -3, \"msg\": \"方案还没付款！\"}");
                return;
            }
            if (model.SchemeStatus == (short)SchemeStatusEnum.OrderOver)
            {
                context.Response.Write("{\"status\": -3, \"msg\": \"方案已过期！\"}");
                return;
            }
            if (model.SchemeStatus == (short)SchemeStatusEnum.OrderFailure || model.SchemeStatus == (short)SchemeStatusEnum.OrderRevoke || model.SchemeStatus == (short)SchemeStatusEnum.NoWinningSuccess)
            {
                context.Response.Write("{\"status\": -3, \"msg\": \"限号、撤销、不中奖！\"}");
                return;
            }
            context.Response.Write("{\"status\": -2, \"msg\": \"设置中奖方案失败！\"}");
        }
        #endregion

        #region 验证用户账号是否存在
        private void username_validate(HttpContext context)
        {
            string user_name = QPRequest.GetString("param");
            if (string.IsNullOrEmpty(user_name))
            {
                context.Response.Write("{ \"info\":\"请输入用户名\", \"status\":\"n\" }");
                return;
            }
            UsersBLL bll = new UsersBLL();
            if (bll.Exists(user_name))
            {
                context.Response.Write("{ \"info\":\"用户名存在！\", \"status\":\"y\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"用户名不存在\", \"status\":\"n\" }");
            return;
        }
        #endregion


        #region 获取模板信息
        private void getTemplate(HttpContext context)
        {
            int tid = QPRequest.GetInt("tid", 0);
            TemplateConfigBLL bll = new TemplateConfigBLL();
            TemplateConfigEntity model = bll.QueryEntity(tid);
            if (model == null)
            {
                context.Response.Write("{\"status\": -1, \"msg\": \"模板不存在！\"}");
                return;
            }
            context.Response.Write("{\"status\": 0, \"msg\": \"获取模板成功！\",\"Content\":\"" + model.TemplateContent + "\"}");
        }
        #endregion

        #region 提交充值退款申请
        private void PostRefund(HttpContext context)
        {
            long UserID = QPRequest.GetLong("UserID");
            long PayID = QPRequest.GetLong("PayID");
            long RefundFee = QPRequest.GetLong("RefundFee");
            UsersEntity model = new UsersBLL().QueryEntityByUserCode(UserID);
            UsersPayDetailEntity PayModel = new UsersPayDetailBLL().QueryPayDetailsByPayID(PayID);
            if (PayModel.Amount < RefundFee)
            {
                context.Response.Write("{\"status\": -1, \"msg\": \"退款余额大于充值余额，退款失败！\"}");
                return;
            }
            if (model.Balance < RefundFee)
            {
                context.Response.Write("{\"status\": -2, \"msg\": \"退款余额大于用户余额，退款失败！\"}");
                return;
            }
            WFTResult Result = new CL.Game.BLL.PayInterface.WFTPay().PostRefund(PayID, RefundFee);
            if (Result.Code == 0)
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"" + Result.Msg + "\"}");
                return;
            }
            context.Response.Write("{\"status\": -1, \"msg\": \"提交退款失败,失败原因：" + Result.Msg + "！\"}");
        }
        #endregion

        #region 查询退款申请到帐没有
        private void PostRefundQuery(HttpContext context)
        {
            long ReID = QPRequest.GetLong("ReID");
            WFTResult Result = new CL.Game.BLL.PayInterface.WFTPay().PostRefundQuery(ReID);
            if (Result.Code == 0)
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"" + Result.Msg + "\"}");
                return;
            }
            context.Response.Write("{\"status\": -1, \"msg\": \"" + Result.Msg + "！\"}");
        }
        #endregion

        #region 设置彩种停售
        private void LotteriesIsStop(HttpContext context)
        {
            int LotteryID = QPRequest.GetInt("LotteryID", 0);
            string StopReason = QPRequest.GetString("StopReason");

            LotteriesBLL bll = new LotteriesBLL();
            LotteriesEntity model = bll.QueryLotterys(LotteryID);
            var IsStop = model.IsStop == null ? false : !Convert.ToBoolean(model.IsStop);
            if (IsStop)
                model.StopReason = StopReason;
            else
                model.StopReason = "";
            model.IsStop = IsStop;
            if (bll.ModifyEntity(model))
            {
                new SystemRedis().RemoveApplyLotteryDataRedis();
                context.Response.Write("{\"status\": 0, \"msg\": \"停售成功\"}");

                if (IsStop)
                    new CL.Game.BLL.IM.Communication().Sendd_Api_Lot(model.LotteryCode, 4);
                else
                    new CL.Game.BLL.IM.Communication().Sendd_Api_Lot(model.LotteryCode, 0);
                return;
            }
            context.Response.Write("{\"status\": -1, \"msg\": \"停售失败\"}");
        }
        #endregion
        #region 设置热门彩种
        //
        private void LotteriesIsHot(HttpContext context)
        {
            int LotteryID = QPRequest.GetInt("LotteryID", 0);
            LotteriesBLL bll = new LotteriesBLL();
            LotteriesEntity model = bll.QueryLotterys(LotteryID);
            model.IsHot = !model.IsHot;

            if (bll.ModifyEntity(model))
            {
                new SystemRedis().RemoveApplyLotteryDataRedis();
                context.Response.Write("{\"status\": 0, \"msg\": \"设置成功\"}");
                return;
            }
            context.Response.Write("{\"status\": -1, \"msg\": \"设置失败\"}");
        }
        #endregion


        #region 撤单并同步客户端
        private void QuashScheme_And_Syn_Client(HttpContext context)
        {
            try
            {
                long SchemeID = QPRequest.GetQueryLong("order");
                #region 撤单操作
                string ReturnDescription = string.Empty;
                new SchemeETicketsBLL().QuashScheme(SchemeID, false, ref ReturnDescription);
                #endregion
                if (!ReturnDescription.Equals("撤单成功"))
                    context.Response.Write("{\"status\": -1, \"msg\": \"撤单失败，" + ReturnDescription + "\"}");
                else
                {
                    var db_SchemeEntity = new SchemesBLL().QueryEntity(SchemeID);
                    if (db_SchemeEntity == null)
                        context.Response.Write("{\"status\": -1, \"msg\": \"撤单成功，同步失败，DB查询失败，请联系管理员\"}");
                    else
                    {
                        if (db_SchemeEntity.BuyType == (byte)BuyTypeEnum.BuyChase)
                        {
                            var db_ChaseTaskDetailEntitys = new ChaseTaskDetailsBLL().QueryEntitysBySchemeID(SchemeID);
                            if (db_ChaseTaskDetailEntitys == null || db_ChaseTaskDetailEntitys.Count == 0)
                                context.Response.Write("{\"status\": -1, \"msg\": \"撤单成功，同步失败，DB查询失败，请联系管理员\"}");
                            else
                            {
                                db_ChaseTaskDetailEntitys.ForEach((Entity) =>
                                {
                                    new SchemesBLL().OutApply(SchemeID, Entity.ID, true, DateTime.Now);
                                });
                                context.Response.Write("{\"status\": 0, \"msg\": \"撤单成功，同步完成\"}");
                            }
                        }
                        else
                            new SchemesBLL().OutApply(SchemeID, 0, true, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"status\": -1, \"msg\": \"撤单成功，同步错误：" + ex.Message + "\"}");
            }
        }
        #endregion


        #region 同步客户端
        private void Syn_Client(HttpContext context)
        {
            try
            {
                long SchemeID = QPRequest.GetQueryLong("order");
                var db_SchemeEntity = new SchemesBLL().QueryEntity(SchemeID);
                if (db_SchemeEntity == null)
                    context.Response.Write("{\"status\": -1, \"msg\": \"同步失败，DB查询失败，请联系管理员\"}");
                else
                {
                    if (db_SchemeEntity.BuyType == (byte)BuyTypeEnum.BuyChase)
                    {
                        var db_ChaseTaskDetailEntitys = new ChaseTaskDetailsBLL().QueryEntitysBySchemeID(SchemeID);
                        if (db_ChaseTaskDetailEntitys == null || db_ChaseTaskDetailEntitys.Count == 0)
                            context.Response.Write("{\"status\": -1, \"msg\": \"同步失败，DB查询失败，请联系管理员\"}");
                        else
                        {
                            db_ChaseTaskDetailEntitys.ForEach((Entity) =>
                            {
                                new SchemesBLL().OutApply(SchemeID, Entity.ID, true, DateTime.Now);
                            });
                            context.Response.Write("{\"status\": 0, \"msg\": \"同步完成\"}");
                        }
                    }
                    else
                        new SchemesBLL().OutApply(SchemeID, 0, true, DateTime.Now);
                }

            }
            catch (Exception ex)
            {
                context.Response.Write("{\"status\": -1, \"msg\": \"同步错误：" + ex.Message + "\"}");
            }
        }
        #endregion
        /// <summary>
        /// 体现审核
        /// </summary>
        /// <param name="context"></param>
        public void Withdraw(HttpContext context)
        {
            try
            {
                string Rec = string.Empty;
                long PayOutID = 0;
                bool result = false;
                string Remark = string.Empty;
                int IsRefuse = QPRequest.GetQueryInt("IsRefuse");
                var Entity = new AdminPage().GetAdminInfo();
                switch (IsRefuse)
                {
                    case 1: //初审通过
                        PayOutID = QPRequest.GetQueryLong("PayOutID");
                        result = new UsersWithdrawBLL().AuditPayOutStatus(PayOutID, (int)PayOutStatus.DealWith, Entity.id);
                        if (result)
                            Rec = "{\"status\": 0, \"msg\": \"审核完成\"}";
                        else
                            Rec = "{\"status\": -1, \"msg\": \"审核失败，请重新审核\"}";
                        break;
                    case 2: //初审拒绝并返余额
                        PayOutID = QPRequest.GetLong("PayOutID");
                        Remark = QPRequest.GetString("Remark");
                        result = new UsersWithdrawBLL().AuditPayOutFailure(PayOutID, Entity.id, Remark);
                        if (result)
                            Rec = "{\"status\": 0, \"msg\": \"审核完成\"}";
                        else
                            Rec = "{\"status\": -1, \"msg\": \"审核失败，请重新审核\"}";
                        break;
                    case 3: //复审通过并体现 
                        PayOutID = QPRequest.GetQueryLong("PayOutID");
                        result = new UsersWithdrawBLL().AuditPayOutSuccess(PayOutID, Entity.id);
                        if (result)
                            Rec = "{\"status\": 0, \"msg\": \"审核完成\"}";
                        else
                            Rec = "{\"status\": -1, \"msg\": \"审核失败，请重新审核\"}";
                        break;
                    case 4: //复审拒绝并返余额
                        PayOutID = QPRequest.GetLong("PayOutID");
                        Remark = QPRequest.GetString("Remark");
                        result = new UsersWithdrawBLL().AuditPayOutFailure(PayOutID, Entity.id, Remark);
                        if (result)
                            Rec = "{\"status\": 0, \"msg\": \"审核完成\"}";
                        else
                            Rec = "{\"status\": -1, \"msg\": \"审核失败，请重新审核\"}";
                        break;
                }
                context.Response.Write(Rec);
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"status\": -1, \"msg\": \"审核错误：" + ex.Message + "\"}");
            }
        }
        /// <summary>
        /// 充值补单
        /// </summary>
        /// <param name="context"></param>
        public void Replenishment(HttpContext context)
        {
            try
            {
                UsersPayDetailBLL payDetailBLL = new UsersPayDetailBLL();
                long PayID = QPRequest.GetLong("PayID");
                string RechargeNo = QPRequest.GetString("RechargeNo");
                string OutRechargeNo = QPRequest.GetString("OutRechargeNo");
                var Entity = payDetailBLL.QueryPayDetailsByPayID(PayID);
                if (Entity != null && Entity.Result == 0)
                {
                    //可补单状态
                    int rec = payDetailBLL.Replenishment(PayID, RechargeNo, OutRechargeNo);
                    if (rec == 0)
                        context.Response.Write("{\"status\": 0, \"msg\": \"完成补单\"}");
                    else
                        context.Response.Write("{\"status\": -1, \"msg\": \"补单失败，请联系管理员\"}");
                }
                else
                {
                    context.Response.Write("{\"status\": -1, \"msg\": \"当前状态不可补单\"}");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"status\": -1, \"msg\": \"充值补单错误：" + ex.Message + "\"}");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 写入管理日志
        /// </summary>
        /// <param name="action_type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private bool AddAdminLog(string action_type, string remark)
        {
            return new AdminPage().AddAdminLog(action_type, remark);
        }

        #region 查询推送用户
        private void UserPushList(HttpContext context)
        {
            try
            {
                string UserName = QPRequest.GetString("key");
                var DataList = new UsersPushBLL().QueryPushList(UserName);
                string Template = "{0}\"UserID\":{1},\"UserName\":\"{2}\",\"PushIdentify\":\"{3}\"{4}";
                StringBuilder msg = new StringBuilder();
                if (DataList != null && DataList.Count > 0)
                    foreach (var item in DataList)
                        msg.AppendFormat(Template, "{", item.UserID, item.UserName, item.PushIdentify, "},");
                context.Response.Write("{\"status\": 0, \"msg\": [" + msg.ToString().TrimEnd(',') + "]}");
            }
            catch
            {
                context.Response.Write("{\"status\": -1, \"msg\": \"查询失败，请联系管理员\"}");
            }
        }
        #endregion

        #region 规则
        protected void Regular(HttpContext context)
        {
            try
            {
                string rec = string.Empty;
                int RegularType = Convert.ToInt32(context.Request.Form["RegularType"]);
                int RegularID = Convert.ToInt32(context.Request.Form["RegularID"]);
                int PlayCode = Convert.ToInt32(context.Request.Form["PlayCode"]);
                var Entity = new ActivityAwardBLL().QueryEntity(RegularID);
                int result = 0;
                long AwardMoney = 0;
                long TopLimit = 0;
                string Param = string.Empty;
                string Xml = string.Empty;
                if (RegularID == 0 || Entity == null)
                {
                    rec = "{\"status\": -1, \"msg\": \"参数错误\"}";
                }
                else
                {
                    if (Entity != null && Entity.RegularStatus != 0)
                        rec = "{\"status\": -1, \"msg\": \"审核后规则不可变更\"}";
                    else
                        switch (RegularType)
                        {
                            case 0:
                                #region 标准
                                int RNormID = Convert.ToInt32(context.Request.Form["RNormID"]);
                                AwardMoney = Convert.ToInt64(context.Request.Form["AwardMoney"]) * 100;
                                TopLimit = Convert.ToInt64(context.Request.Form["TopLimit"]) * 100;
                                var NormEntity = new RegularNormEntity();
                                if (RNormID > 0)
                                {
                                    //变更
                                    NormEntity.AwardMoney = AwardMoney;
                                    NormEntity.PlayCode = PlayCode;
                                    NormEntity.RegularID = RegularID;
                                    NormEntity.TopLimit = TopLimit;
                                    result = new RegularNormBLL().ModifyEntity(NormEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"规则变更成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"规则变更失败，请联系管理员\"}";
                                }
                                else
                                {
                                    //新增
                                    NormEntity.AwardMoney = AwardMoney;
                                    NormEntity.PlayCode = PlayCode;
                                    NormEntity.RegularID = RegularID;
                                    NormEntity.TopLimit = TopLimit;
                                    result = new RegularNormBLL().InsertEntity(NormEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"新增规则成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"新增规则失败，请联系管理员\"}";
                                }
                                #endregion
                                break;
                            case 1:
                                #region 追号
                                int RChaseID = Convert.ToInt32(context.Request.Form["RChaseID"]);
                                AwardMoney = Convert.ToInt64(context.Request.Form["AwardMoney"]) * 100;
                                TopLimit = Convert.ToInt64(context.Request.Form["TopLimit"]) * 100;
                                int ChaseType = Convert.ToInt32(context.Request.Form["ChaseType"]);
                                int Unit = Convert.ToInt32(context.Request.Form["Unit"]);
                                var ChaseEntity = new RegularChaseEntity();
                                if (RChaseID > 0)
                                {
                                    ChaseEntity.AwardMoney = AwardMoney;
                                    ChaseEntity.PlayCode = PlayCode;
                                    ChaseEntity.RChaseID = RChaseID;
                                    ChaseEntity.RChaseType = ChaseType;
                                    ChaseEntity.RegularID = RegularID;
                                    ChaseEntity.TopLimit = TopLimit;
                                    ChaseEntity.Unit = Unit;
                                    result = new RegularChaseBLL().ModifyEntity(ChaseEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"规则变更成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"规则变更失败，请联系管理员\"}";
                                }
                                else
                                {
                                    ChaseEntity.AwardMoney = AwardMoney;
                                    ChaseEntity.PlayCode = PlayCode;
                                    ChaseEntity.RChaseType = ChaseType;
                                    ChaseEntity.RegularID = RegularID;
                                    ChaseEntity.TopLimit = TopLimit;
                                    ChaseEntity.Unit = Unit;
                                    result = new RegularChaseBLL().InsertEntity(ChaseEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"新增规则成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"新增规则失败，请联系管理员\"}";
                                }
                                #endregion
                                break;
                            case 2:
                                #region 胆拖
                                int RDanTuoID = Convert.ToInt32(context.Request.Form["RDanTuoID"]);
                                AwardMoney = Convert.ToInt64(context.Request.Form["AwardMoney"]) * 100;
                                TopLimit = Convert.ToInt64(context.Request.Form["TopLimit"]) * 100;
                                int DanNums = Convert.ToInt32(context.Request.Form["DanNums"]);
                                int TuoNums = Convert.ToInt32(context.Request.Form["TuoNums"]);
                                var DanTuoEntity = new RegularDanTuoEntity();
                                if (RDanTuoID > 0)
                                {
                                    DanTuoEntity.AwardMoney = AwardMoney;
                                    DanTuoEntity.DanNums = DanNums;
                                    DanTuoEntity.PlayCode = PlayCode;
                                    DanTuoEntity.RDanTuoID = RDanTuoID;
                                    DanTuoEntity.RegularID = RegularID;
                                    DanTuoEntity.TopLimit = TopLimit;
                                    DanTuoEntity.TuoNums = TuoNums;
                                    result = new RegularDanTuoBLL().ModifyEntity(DanTuoEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"规则变更成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"规则变更失败，请联系管理员\"}";
                                }
                                else
                                {
                                    DanTuoEntity.AwardMoney = AwardMoney;
                                    DanTuoEntity.DanNums = DanNums;
                                    DanTuoEntity.PlayCode = PlayCode;
                                    DanTuoEntity.RegularID = RegularID;
                                    DanTuoEntity.TopLimit = TopLimit;
                                    DanTuoEntity.TuoNums = TuoNums;
                                    result = new RegularDanTuoBLL().InsertEntity(DanTuoEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"新增规则成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"新增规则失败，请联系管理员\"}";
                                }
                                #endregion
                                break;
                            case 3:

                                break;
                            case 4:

                                break;
                            case 5:
                                #region  投注区间加奖
                                int RBetIntervalID = Convert.ToInt32(context.Request.Form["RBetIntervalID"]);
                                Param = context.Request.Form["Param"];
                                List<udv_Interval> BetInterval = new GenericRegulate<udv_Interval>().SetString(Param);
                                Xml = XmlHelper.Serializer(BetInterval.GetType(), BetInterval);
                                Xml = Xml.Replace("ArrayOfUdv_Interval", "root").Replace("udv_Interval", "item");

                                var BetIntervalEntity = new RegularBetIntervalEntity();
                                if (RBetIntervalID > 0)
                                {
                                    BetIntervalEntity.RegularID = RegularID;
                                    BetIntervalEntity.RBetIntervalID = RBetIntervalID;
                                    BetIntervalEntity.PlayCode = PlayCode;
                                    BetIntervalEntity.BetInterval = Xml;
                                    result = new RegularBetIntervalBLL().ModifyEntity(BetIntervalEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"规则变更成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"规则变更失败，请联系管理员\"}";
                                }
                                else
                                {
                                    BetIntervalEntity.RegularID = RegularID;
                                    BetIntervalEntity.RBetIntervalID = RBetIntervalID;
                                    BetIntervalEntity.PlayCode = PlayCode;
                                    BetIntervalEntity.BetInterval = Xml;
                                    result = new RegularBetIntervalBLL().InsertEntity(BetIntervalEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"新增规则成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"新增规则失败，请联系管理员\"}";
                                }

                                #endregion
                                break;
                            case 6:
                                #region 加奖
                                int RAwardIntervalID = Convert.ToInt32(context.Request.Form["RAwardIntervalID"]);
                                Param = context.Request.Form["Param"];
                                List<udv_Interval> Interval = new GenericRegulate<udv_Interval>().SetString(Param);
                                Xml = XmlHelper.Serializer(Interval.GetType(), Interval);
                                Xml = Xml.Replace("ArrayOfUdv_Interval", "root").Replace("udv_Interval", "item");
                                var AwardIntervalEntity = new RegularAwardIntervalEntity();
                                if (RAwardIntervalID > 0)
                                {
                                    AwardIntervalEntity.PlayCode = PlayCode;
                                    AwardIntervalEntity.RAwardIntervalID = RAwardIntervalID;
                                    AwardIntervalEntity.RegularID = RegularID;
                                    AwardIntervalEntity.AwardInterval = Xml;
                                    result = new RegularAwardIntervalBLL().ModifyEntity(AwardIntervalEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"规则变更成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"规则变更失败，请联系管理员\"}";
                                }
                                else
                                {
                                    AwardIntervalEntity.PlayCode = PlayCode;
                                    AwardIntervalEntity.RegularID = RegularID;
                                    AwardIntervalEntity.AwardInterval = Xml;
                                    result = new RegularAwardIntervalBLL().InsertEntity(AwardIntervalEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"新增规则成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"新增规则失败，请联系管理员\"}";
                                }
                                #endregion
                                break;
                            case 7:
                                #region 加奖
                                int RBetRankingID = Convert.ToInt32(context.Request.Form["RBetRankingID"]);
                                Param = context.Request.Form["Param"];
                                List<udv_Ranking> BetRanking = new GenericRegulate<udv_Ranking>().SetString(Param);
                                Xml = XmlHelper.Serializer(BetRanking.GetType(), BetRanking);
                                Xml = Xml.Replace("ArrayOfUdv_Ranking", "root").Replace("udv_Ranking", "item");
                                var BetRankingEntity = new RegularBetRankingEntity();
                                if (RBetRankingID > 0)
                                {
                                    BetRankingEntity.BetRanking = Xml;
                                    BetRankingEntity.PlayCode = PlayCode;
                                    BetRankingEntity.RBetRanID = RBetRankingID;
                                    BetRankingEntity.RegularID = RegularID;
                                    result = new RegularBetRankingBLL().ModifyEntity(BetRankingEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"规则变更成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"规则变更失败，请联系管理员\"}";
                                }
                                else
                                {
                                    BetRankingEntity.BetRanking = Xml;
                                    BetRankingEntity.PlayCode = PlayCode;
                                    BetRankingEntity.RegularID = RegularID;
                                    result = new RegularBetRankingBLL().InsertEntity(BetRankingEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"新增规则成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"新增规则失败，请联系管理员\"}";
                                }
                                #endregion
                                break;
                            case 8:
                                #region 加奖
                                int RAwardRankingID = Convert.ToInt32(context.Request.Form["RAwardRankingID"]);
                                Param = context.Request.Form["Param"];
                                List<udv_Ranking> RankingEntity = new GenericRegulate<udv_Ranking>().SetString(Param);
                                Xml = XmlHelper.Serializer(RankingEntity.GetType(), RankingEntity);
                                Xml = Xml.Replace("ArrayOfUdv_Ranking", "root").Replace("udv_Ranking", "item");
                                var AwardRankingEntity = new RegularAwardRankingEntity();
                                if (RAwardRankingID > 0)
                                {
                                    AwardRankingEntity.AwardRanking = Xml;
                                    AwardRankingEntity.PlayCode = PlayCode;
                                    AwardRankingEntity.RAwardRanID = RAwardRankingID;
                                    AwardRankingEntity.RegularID = RegularID;
                                    result = new RegularAwadRankingBLL().ModifyEntity(AwardRankingEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"规则变更成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"规则变更失败，请联系管理员\"}";
                                }
                                else
                                {
                                    AwardRankingEntity.AwardRanking = Xml;
                                    AwardRankingEntity.PlayCode = PlayCode;
                                    AwardRankingEntity.RegularID = RegularID;
                                    result = new RegularAwadRankingBLL().InsertEntity(AwardRankingEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"新增规则成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"新增规则失败，请联系管理员\"}";
                                }
                                #endregion
                                break;
                            case 9:
                                #region 中球
                                int RBallID = Convert.ToInt32(context.Request.Form["RBallID"]);
                                int BallType = Convert.ToInt32(context.Request.Form["BallType"]);
                                string Ball = context.Request.Form["Ball"];
                                AwardMoney = Convert.ToInt64(context.Request.Form["AwardMoney"]) * 100;
                                TopLimit = Convert.ToInt64(context.Request.Form["TopLimit"]) * 100;
                                var BallEntity = new RegularBallEntity();
                                if (RBallID > 0)
                                {
                                    BallEntity.AwardMoney = AwardMoney;
                                    BallEntity.Ball = Ball;
                                    BallEntity.BallType = BallType;
                                    BallEntity.PlayCode = PlayCode;
                                    BallEntity.RBallID = RBallID;
                                    BallEntity.RegularID = RegularID;
                                    BallEntity.TopLimit = TopLimit;
                                    result = new RegularBallBLL().ModifyEntity(BallEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"规则变更成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"规则变更失败，请联系管理员\"}";
                                }
                                else
                                {
                                    BallEntity.AwardMoney = AwardMoney;
                                    BallEntity.Ball = Ball;
                                    BallEntity.BallType = BallType;
                                    BallEntity.PlayCode = PlayCode;
                                    BallEntity.RegularID = RegularID;
                                    BallEntity.TopLimit = TopLimit;
                                    result = new RegularBallBLL().InsertEntity(BallEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"新增规则成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"新增规则失败，请联系管理员\"}";
                                }
                                #endregion
                                break;
                            case 10:
                                #region 节假日
                                int RHolidayID = Convert.ToInt32(context.Request.Form["RHolidayID"]);
                                int HolidayType = Convert.ToInt32(context.Request.Form["HoliadyType"]);
                                AwardMoney = Convert.ToInt64(context.Request.Form["AwardMoney"]) * 100;
                                TopLimit = Convert.ToInt64(context.Request.Form["TopLimit"]) * 100;
                                var HolidayEntity = new RegularHolidayEntity();
                                if (RHolidayID > 0)
                                {
                                    HolidayEntity.AwardMoney = AwardMoney;
                                    HolidayEntity.HolidayType = HolidayType;
                                    HolidayEntity.PlayCode = PlayCode;
                                    HolidayEntity.RegularID = RegularID;
                                    HolidayEntity.RHolidayID = RHolidayID;
                                    HolidayEntity.TopLimitDay = TopLimit;
                                    result = new RegularHolidayBLL().ModifyEntity(HolidayEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"规则变更成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"规则变更失败，请联系管理员\"}";
                                }
                                else
                                {
                                    HolidayEntity.AwardMoney = AwardMoney;
                                    HolidayEntity.HolidayType = HolidayType;
                                    HolidayEntity.PlayCode = PlayCode;
                                    HolidayEntity.RegularID = RegularID;
                                    HolidayEntity.TopLimitDay = TopLimit;
                                    result = new RegularHolidayBLL().InsertEntity(HolidayEntity);
                                    if (result > 0)
                                        rec = "{\"status\": 0, \"msg\": \"新增规则成功\"}";
                                    else
                                        rec = "{\"status\": -1, \"msg\": \"新增规则失败，请联系管理员\"}";
                                }
                                #endregion
                                break;
                            default:
                                rec = "{\"status\": -1, \"msg\": \"参数错误\"}";
                                break;
                        }
                }
                context.Response.Write(rec);
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"status\": -1, \"msg\": \"规则数据操作失败：" + ex.Message + "\"}");
            }
        }
        #endregion


    }
}