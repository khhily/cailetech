using CL.Enum.Common;
using CL.Enum.Common.Activity;
using CL.Enum.Common.Activity.Regular;
using CL.Enum.Common.Lottery;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace CL.Admin.admin.activity.regular
{
    public partial class awardauditing : UI.AdminPage
    {
        Log log = new Log("awardauditing");
        protected static int ActivityID = 0;
        protected static List<PlayTypesEntity> PlayTypes = null;
        protected static List<ActivityAwardEntity> ActivityAward = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int.TryParse(Request.QueryString["id"], out ActivityID);
                if (ActivityID > 0)
                    BindData();
                else
                {
                    btnAuditing.BackColor = Color.Gray;
                    btnRefuse.BackColor = Color.Gray;
                    btnAuditing.Enabled = false;
                    btnRefuse.Enabled = false;
                }
            }
        }
        protected void BindData()
        {
            try
            {
                #region 活动详情数据
                var ActivityEntity = new ActivityBLL().QueryEntity(ActivityID);
                if (ActivityEntity == null)
                {
                    btnAuditing.BackColor = Color.Gray;
                    btnRefuse.BackColor = Color.Gray;
                    btnAuditing.Enabled = false;
                    btnRefuse.Enabled = false;
                    ClientScript.RegisterStartupScript(this.GetType(), "script", "alert('未查询到相关活动业务，请检查');", true);
                    return;
                }
                lbActivityType.InnerText = ActivityEntity.ActivityType == 0 ? "彩种官方活动" : "彩乐彩票活动";
                lbActivitySubject.InnerText = ActivityEntity.ActivitySubject;
                lbActivityMoney.InnerText = (ActivityEntity.ActivityMoney / 100).ToString("C2");
                lbCreateTime.InnerText = ActivityEntity.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                lbCurrencyUnit.InnerText = Common.GetDescription((ActivityUnit)ActivityEntity.CurrencyUnit);
                lbStartTime.InnerText = ActivityEntity.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                lbEndTime.InnerText = ActivityEntity.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                linkLandingPage.InnerText = ActivityEntity.LandingPage;
                if (!string.IsNullOrEmpty(ActivityEntity.LandingPage))
                    linkLandingPage.HRef = (ActivityEntity.LandingPage.StartsWith("http://") == true ? ActivityEntity.LandingPage : "http://" + ActivityEntity.LandingPage);
                imgADUrl.ImageUrl = ActivityEntity.ADUrl;
                #endregion
                #region 加载活动规则
                ActivityAward = new ActivityAwardBLL().QueryEntitys(ActivityID);
                rptRegularList.DataSource = ActivityAward;
                rptRegularList.DataBind();
                #endregion
            }
            catch (Exception ex)
            {
                log.Write(string.Format("加载数据发生错误：{0}", ex.StackTrace), true);
                ClientScript.RegisterStartupScript(this.GetType(), "script", "alert('活动资料错误，请重修改');", true);
            }
        }


        protected string SetLot(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            else
                rec = Common.GetDescription((LotteryInfo)Convert.ToInt32(obj));
            return rec;
        }
        protected string SetMoney(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "0";
            else
                rec = (Convert.ToInt64(obj) / 100).ToString();
            return rec;
        }
        protected string SetType(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            else
                rec = Common.GetDescription((RegularType)Convert.ToInt32(obj));
            return rec;
        }
        protected string SetStatus(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            else
                rec = Common.GetDescription((RegularStatus)Convert.ToInt32(obj));
            return rec;
        }
        protected string SetPlayCode(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            else
            {
                var PlayTypeEntity = PlayTypes.Where(w => w.PlayCode == Convert.ToInt32(obj)).FirstOrDefault();
                if (PlayTypeEntity != null)
                    rec = PlayTypeEntity.PlayName;
                else
                    rec = "--";
            }
            return rec;
        }
        protected string SetHolidayType(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            else
                rec = Common.GetDescription((HolidayType)Convert.ToInt32(obj));
            return rec;
        }
        protected string SetInterval(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Convert.ToString(obj));
                if (doc == null)
                    rec = "--";
                else
                {
                    XmlNodeList XmlNodes = doc.SelectNodes("root/item");
                    StringBuilder tab = new StringBuilder("<table style=\"width: 100%;\"><thead><tr>");
                    tab.Append("<td>序号</td>");
                    tab.Append("<td>最小金额</td>");
                    tab.Append("<td>最大金额</td>");
                    tab.Append("<td>加奖金额</td>");
                    tab.Append("</tr></thead><tbody>");
                    foreach (XmlNode item in XmlNodes)
                    {
                        tab.Append("<tr>");
                        tab.AppendFormat("<td>{0}</td>", item.SelectSingleNode("pk").InnerText);
                        tab.AppendFormat("<td>{0}</td>", (Convert.ToInt64(item.SelectSingleNode("min").InnerText) / 100));
                        tab.AppendFormat("<td>{0}</td>", (Convert.ToInt64(item.SelectSingleNode("max").InnerText) / 100));
                        tab.AppendFormat("<td>{0}</td>", (Convert.ToInt64(item.SelectSingleNode("award").InnerText) / 100));
                        tab.Append("</tr>");
                    }
                    tab.Append("</tbody></table>");
                    rec = tab.ToString();
                }
            }
            return rec;
        }
        protected string SetRanking(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Convert.ToString(obj));
                if (doc == null)
                    rec = "--";
                else
                {
                    XmlNodeList XmlNodes = doc.SelectNodes("root/item");
                    StringBuilder tab = new StringBuilder("<table style=\"width: 100%;\"><thead><tr>");
                    tab.Append("<td>名次</td>");
                    tab.Append("<td>加奖金额</td>");
                    tab.Append("</tr></thead><tbody>");
                    foreach (XmlNode item in XmlNodes)
                    {
                        tab.Append("<tr>");
                        tab.AppendFormat("<td>{0}</td>", item.SelectSingleNode("placing").InnerText);
                        tab.AppendFormat("<td>{0}</td>", (Convert.ToInt64(item.SelectSingleNode("award").InnerText) / 100));
                        tab.Append("</tr>");
                    }
                    tab.Append("</tbody></table>");
                    rec = tab.ToString();
                }
            }

            return rec;
        }



        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAuditing_Click(object sender, EventArgs e)
        {
            bool result = new ActivityAwardBLL().ActivityAuditing(ActivityID, true);
            if (result)
                ClientScript.RegisterStartupScript(this.GetType(), "script", "alert('审核完成');", true);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "script", "alert('审核失败，请联系管理员');", true);
        }

        /// <summary>
        /// 拒绝审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRefuse_Click(object sender, EventArgs e)
        {
            bool result = new ActivityAwardBLL().ActivityAuditing(ActivityID, false);
            if (result)
                ClientScript.RegisterStartupScript(this.GetType(), "script", "alert('拒绝完成');", true);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "script", "alert('拒绝失败，请联系管理员');", true);
        }

        protected void rptRegularList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Regular":
                    HiddenField hidArgument = (HiddenField)e.Item.FindControl("hidArgument");
                    HiddenField hidRegularID = (HiddenField)e.Item.FindControl("hidRegularID");
                    LinkButton LinRegular = (LinkButton)e.Item.FindControl("LinRegular");
                    int RegularType = ActivityAward.Where(w => w.RegularID == Convert.ToInt32(hidRegularID.Value)).FirstOrDefault().RegularType;
                    string FindControlName = string.Format("rptList_{0}", RegularType);
                    Repeater rptList = (Repeater)e.Item.FindControl(FindControlName);
                    if (hidArgument.Value == "Open")
                    {
                        rptList.Visible = true;
                        hidArgument.Value = "Close";
                        LinRegular.Text = "关闭规则详情";
                        LinRegular.ForeColor = Color.Red;
                    }
                    else
                    {
                        rptList.Visible = false;
                        hidArgument.Value = "Open";
                        LinRegular.Text = "展开规则详情";
                        LinRegular.ForeColor = Color.Blue;
                    }
                    break;
            }
        }

        protected void rptRegularList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int RegularID = ((ActivityAwardEntity)e.Item.DataItem).RegularID;
                int RegularType = ((ActivityAwardEntity)e.Item.DataItem).RegularType;
                int LotteryCode = ((ActivityAwardEntity)e.Item.DataItem).LotteryCode;
                PlayTypes = new PlayTypesBLL().QueryEntitysByLotteryCode(LotteryCode);
                string FindControlName = string.Format("rptList_{0}", RegularType);
                Repeater rptList = (Repeater)e.Item.FindControl(FindControlName);
                switch (RegularType)
                {
                    case 0:
                        rptList.DataSource = new RegularNormBLL().QueryEntitys(RegularID);
                        rptList.DataBind();
                        break;
                    case 1:
                        rptList.DataSource = new RegularChaseBLL().QueryEntitys(RegularID);
                        rptList.DataBind();
                        break;
                    case 2:
                        rptList.DataSource = new RegularDanTuoBLL().QueryEntitys(RegularID);
                        rptList.DataBind();
                        break;
                    case 5:
                        rptList.DataSource = new RegularBetIntervalBLL().QueryEntitys(RegularID);
                        rptList.DataBind();
                        break;
                    case 6:
                        rptList.DataSource = new RegularAwardIntervalBLL().QueryEntitys(RegularID);
                        rptList.DataBind();
                        break;
                    case 7:
                        rptList.DataSource = new RegularBetRankingBLL().QueryEntitys(RegularID);
                        rptList.DataBind();
                        break;
                    case 8:
                        rptList.DataSource = new RegularAwadRankingBLL().QueryEntitys(RegularID);
                        rptList.DataBind();
                        break;
                    case 9:
                        rptList.DataSource = new RegularBallBLL().QueryEntitys(RegularID);
                        rptList.DataBind();
                        break;
                    case 10:
                        rptList.DataSource = new RegularHolidayBLL().QueryEntitys(RegularID);
                        rptList.DataBind();
                        break;
                }
            }
        }
    }
}