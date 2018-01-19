using CL.Enum.Common;
using CL.Enum.Common.Status;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.TicketInterface;
using CL.View.Entity.Game;
using CL.View.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace CL.Admin.admin.lotteries
{
    public partial class resultwinsetup : UI.AdminPage
    {
        protected static List<udv_InterfaceAward> InterfaceAwards = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("resultwinsetup", CaileEnums.ActionEnum.View.ToString()); //检查权限
                LotteryBind();
            }
        }

        #region 彩种类型
        private void LotteryBind()
        {
            LotteriesBLL bll = new LotteriesBLL();
            List<LotteriesEntity> list = bll.QueryEntitys("");

            ddlLotteryCode.Items.Clear();
            ddlLotteryCode.Items.Add(new ListItem("请选择彩种..", ""));
            foreach (LotteriesEntity item in list)
            {
                ddlLotteryCode.Items.Add(new ListItem(item.LotteryName, item.LotteryCode.ToString()));
            }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int LotteryCode = 0; //彩种代码
            int.TryParse(ddlLotteryCode.SelectedValue, out LotteryCode);
            string IsuseName = this.txtIsuseName.Text.Trim(); //期号
            if (LotteryCode == 0)
            {
                JscriptMsg("请选择彩种", string.Empty);
                return;
            }
            if (string.IsNullOrEmpty(IsuseName))
            {
                JscriptMsg("请填写期号信息", string.Empty);
                return;
            }
            List<udv_ParaWinInfo> para = new List<udv_ParaWinInfo>();
            List<udv_SchemeETicketAward> Entitys = new SchemeETicketsBLL().QuerySchemeETicketAward(LotteryCode, IsuseName);
            if (Entitys == null || Entitys.Count == 0)
            {
                JscriptMsg("当期无电子票信息", string.Empty);
                return;
            }
            foreach (udv_SchemeETicketAward item in Entitys)
                para.Add(new udv_ParaWinInfo()
                {
                    SchemeETicketID = item.SchemeETicketsID,
                    WinMoney = 0,
                    WinMoneyNoWithTax = 0
                });
            InterfaceBase InterTicker = new InterfaceBase()[SetInterfaceXml(LotteryCode)];
            List<udv_ResultWinInfo> ResModel = InterTicker.HandleWinInfo(para);
            InterfaceAwards = new List<udv_InterfaceAward>();
            udv_InterfaceAward InterfaceAwardEntity = null;
            foreach (udv_ResultWinInfo ResItem in ResModel)
            {
                if (ResItem.ErrorCode == "0")
                {
                    foreach (udv_WinInfoEntites item in ResItem.ListWinInfo)
                    {
                        var AwardEntity = Entitys.Where(w => w.SchemeETicketsID == Convert.ToInt64(item.SchemeETicketID)).FirstOrDefault();
                        if (AwardEntity != null)
                        {
                            Entitys.Remove(AwardEntity);
                            InterfaceAwardEntity = new udv_InterfaceAward();
                            InterfaceAwardEntity.InterfaceStatus = item.Status;
                            InterfaceAwardEntity.WinMoney = Convert.ToInt64(item.PrebonusValue);
                            InterfaceAwardEntity.WinMoneyNoWithTax = Convert.ToInt64(item.BonusValue);
                            InterfaceAwardEntity.Identifiers = AwardEntity.Identifiers;
                            InterfaceAwardEntity.Ticket = AwardEntity.Ticket;
                            InterfaceAwardEntity.IsuseName = AwardEntity.IsuseName;
                            InterfaceAwardEntity.LotteryCode = AwardEntity.LotteryCode;
                            InterfaceAwardEntity.Multiple = AwardEntity.Multiple;
                            InterfaceAwardEntity.Number = AwardEntity.Number;
                            InterfaceAwardEntity.SchemeETicketsID = AwardEntity.SchemeETicketsID;
                            InterfaceAwardEntity.SchemeNumber = AwardEntity.SchemeNumber;
                            InterfaceAwardEntity.TicketMoney = AwardEntity.TicketMoney;
                            InterfaceAwardEntity.TicketStatus = AwardEntity.TicketStatus;
                            InterfaceAwardEntity.UserMobile = AwardEntity.UserMobile;
                            InterfaceAwardEntity.UserName = AwardEntity.UserName;
                            InterfaceAwardEntity.UserID = AwardEntity.UserID;
                            InterfaceAwards.Add(InterfaceAwardEntity);
                        }
                    }
                }
            }
            if (Entitys != null && Entitys.Count > 0)
                foreach (var item in Entitys)
                    InterfaceAwards.Add(new udv_InterfaceAward()
                    {
                        InterfaceStatus = -1,
                        WinMoney = 0,
                        WinMoneyNoWithTax = 0,
                        Identifiers = item.Identifiers,
                        IsuseName = item.IsuseName,
                        LotteryCode = item.LotteryCode,
                        Multiple = item.Multiple,
                        Number = item.Number,
                        SchemeETicketsID = item.SchemeETicketsID,
                        SchemeNumber = item.SchemeNumber,
                        Ticket = item.Ticket,
                        TicketMoney = item.TicketMoney,
                        TicketStatus = item.TicketStatus,
                        UserMobile = item.UserMobile,
                        UserName = item.UserName,
                        UserID = item.UserID
                    });
            rptList.DataSource = InterfaceAwards;
            rptList.DataBind();
        }
        /// <summary>
        /// 设置接口参数
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        protected XmlNode SetInterfaceXml(int LotteryCode)
        {
            XmlNode RecXml = null;
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "Config/Interface.xml");
            XmlNodeList XmlList = doc.SelectNodes("//EntryModel/Item");
            foreach (XmlNode item in XmlList)
                if (Convert.ToInt32(item.SelectSingleNode("SystemLotteryCode").InnerText) == LotteryCode)
                {
                    RecXml = item;
                    break;
                }
            return RecXml;
        }

        protected void lbtnPayOutWin_Click(object sender, EventArgs e)
        {

            List<long> Ids = new List<long>();
            foreach (RepeaterItem item in rptList.Items)
            {
                CheckBox chkId = (CheckBox)item.FindControl("chkId");
                if (chkId.Checked)
                    Ids.Add(Convert.ToInt64(((HiddenField)item.FindControl("hidId")).Value));
            }
            if (Ids.Count == 0)
            {
                JscriptMsg("请选择数据行进行派奖", string.Empty);
                return;
            }
            var Entitys = InterfaceAwards.Where(w => Ids.Contains(w.SchemeETicketsID)).ToList();
            if (Entitys != null && Entitys.Count > 0)
            {
                List<udv_Award> ls = new List<udv_Award>();
                //重新派奖
                foreach (udv_InterfaceAward item in Entitys)
                    ls.Add(new udv_Award()
                    {
                        tid = item.SchemeETicketsID,
                        uuid = item.UserID,
                        wm = item.WinMoney,
                        nwm = item.WinMoneyNoWithTax
                    });
                if (ls != null && ls.Count > 0)
                {
                    int Rec = new SchemeETicketsBLL().ReplenishAward(ls);
                    if (Rec == 0)
                    {
                        JscriptMsg("派奖成功", string.Empty);
                        return;
                    }
                    else
                    {
                        JscriptMsg("派奖错误", string.Empty);
                        return;
                    }
                }
                else
                {
                    JscriptMsg("没有派奖数据", string.Empty);
                    return;
                }
            }
        }


        protected string SetTicketStatus(object obj)
        {
            string Rec = string.Empty;
            if (obj == null)
                Rec = "--";
            else
                Rec = Common.GetDescription((TicketStatusEnum)Convert.ToInt32(obj));
            return Rec;
        }
        protected string InterfaceStatus(object obj)
        {
            string Rec = string.Empty;
            if (obj == null)
                Rec = "--";
            else if (Convert.ToInt32(obj) == -1)
            {
                Rec = "未知";
            }
            else
                Rec = Common.GetDescription((HYInterfaceStatusEnum)Convert.ToInt32(obj));
            return Rec;
        }
    }
}