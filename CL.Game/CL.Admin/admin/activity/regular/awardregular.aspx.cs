using CL.Enum.Common;
using CL.Enum.Common.Activity.Regular;
using CL.Game.BLL;
using CL.Game.Entity;
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
    public partial class awardregular : UI.AdminPage
    {
        protected static int RegularID = 0;
        protected static int ActivityID = 0;
        protected static List<PlayTypesEntity> PlayTypes = null;
        protected static ActivityAwardEntity Entity = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int.TryParse(Request.QueryString["id"], out RegularID);
                int.TryParse(Request.QueryString["acid"], out ActivityID);
                if (RegularID > 0)
                    BindData();
            }
        }
        protected void BindData()
        {
            Entity = new ActivityAwardBLL().QueryEntity(RegularID);
            if (Entity != null)
            {
                ddlRegularLottery.SelectedValue = Entity.LotteryCode.ToString();
                ddlRegularType.SelectedValue = Entity.RegularType.ToString();
                ddlRegularLottery.Enabled = false;
                ddlRegularType.Enabled = false;
                lbtnRegularSubmit.BackColor = Color.Gray;
                lbtnRegularSubmit.Enabled = false;
                #region 绑定数据
                PlayTypes = new PlayTypesBLL().QueryEntitysByLotteryCode(Entity.LotteryCode);

                switch (Entity.RegularType)
                {
                    case 0:
                        //标准玩法
                        rptList_0.Visible = true;
                        rptList_0.DataSource = new RegularNormBLL().QueryEntitys(RegularID);
                        rptList_0.DataBind();
                        ddlRegularNormPlayCode.DataValueField = "PlayCode";
                        ddlRegularNormPlayCode.DataTextField = "PlayName";
                        ddlRegularNormPlayCode.DataSource = PlayTypes;
                        ddlRegularNormPlayCode.DataBind();
                        break;
                    case 1:
                        rptList_1.Visible = true;
                        rptList_1.DataSource = new RegularChaseBLL().QueryEntitys(RegularID);
                        rptList_1.DataBind();
                        //追号玩法
                        ddlRegularChasePlayCode.DataValueField = "PlayCode";
                        ddlRegularChasePlayCode.DataTextField = "PlayName";
                        ddlRegularChasePlayCode.DataSource = PlayTypes;
                        ddlRegularChasePlayCode.DataBind();
                        break;
                    case 2:
                        rptList_2.Visible = true;
                        rptList_2.DataSource = new RegularDanTuoBLL().QueryEntitys(RegularID);
                        rptList_2.DataBind();
                        //追号玩法
                        ddlRegularDanTuoPlayCode.DataValueField = "PlayCode";
                        ddlRegularDanTuoPlayCode.DataTextField = "PlayName";
                        ddlRegularDanTuoPlayCode.DataSource = PlayTypes;
                        ddlRegularDanTuoPlayCode.DataBind();
                        break;
                    case 5:
                        rptList_5.Visible = true;
                        rptList_5.DataSource = new RegularBetIntervalBLL().QueryEntitys(RegularID);
                        rptList_5.DataBind();
                        //投注区间加奖
                        ddlRegularBetIntervalPlayCode.DataValueField = "PlayCode";
                        ddlRegularBetIntervalPlayCode.DataTextField = "PlayName";
                        ddlRegularBetIntervalPlayCode.DataSource = PlayTypes;
                        ddlRegularBetIntervalPlayCode.DataBind();
                        break;
                    case 6:
                        rptList_6.Visible = true;
                        rptList_6.DataSource = new RegularAwardIntervalBLL().QueryEntitys(RegularID);
                        rptList_6.DataBind();
                        //中奖区间加奖
                        ddlRegularAwardIntervalPlayCode.DataValueField = "PlayCode";
                        ddlRegularAwardIntervalPlayCode.DataTextField = "PlayName";
                        ddlRegularAwardIntervalPlayCode.DataSource = PlayTypes;
                        ddlRegularAwardIntervalPlayCode.DataBind();
                        break;
                    case 7:
                        rptList_7.Visible = true;
                        rptList_7.DataSource = new RegularBetRankingBLL().QueryEntitys(RegularID);
                        rptList_7.DataBind();
                        //投注名次加奖
                        ddlRegularBetRankingPlayCode.DataValueField = "PlayCode";
                        ddlRegularBetRankingPlayCode.DataTextField = "PlayName";
                        ddlRegularBetRankingPlayCode.DataSource = PlayTypes;
                        ddlRegularBetRankingPlayCode.DataBind();
                        break;
                    case 8:
                        rptList_8.Visible = true;
                        rptList_8.DataSource = new RegularAwadRankingBLL().QueryEntitys(RegularID);
                        rptList_8.DataBind();
                        //中奖名次加奖
                        ddlRegularAwardRankingPlayCode.DataValueField = "PlayCode";
                        ddlRegularAwardRankingPlayCode.DataTextField = "PlayName";
                        ddlRegularAwardRankingPlayCode.DataSource = PlayTypes;
                        ddlRegularAwardRankingPlayCode.DataBind();
                        break;
                    case 9:
                        rptList_9.Visible = true;
                        rptList_9.DataSource = new RegularBallBLL().QueryEntitys(RegularID);
                        rptList_9.DataBind();
                        //中球
                        ddlRegularBallPlayCode.DataValueField = "PlayCode";
                        ddlRegularBallPlayCode.DataTextField = "PlayName";
                        ddlRegularBallPlayCode.DataSource = PlayTypes;
                        ddlRegularBallPlayCode.DataBind();
                        break;
                    case 10:
                        rptList_10.Visible = true;
                        rptList_10.DataSource = new RegularHolidayBLL().QueryEntitys(RegularID);
                        rptList_10.DataBind();
                        //节假日
                        ddlRegularHolidayPlayCode.DataValueField = "PlayCode";
                        ddlRegularHolidayPlayCode.DataTextField = "PlayName";
                        ddlRegularHolidayPlayCode.DataSource = PlayTypes;
                        ddlRegularHolidayPlayCode.DataBind();
                        break;
                }
                #endregion
            }
            else
                RegularID = 0;
        }

        protected void lbtnRegularSubmit_Click(object sender, EventArgs e)
        {
            int LotteryCode = Convert.ToInt32(ddlRegularLottery.SelectedValue);
            int RegularType = Convert.ToInt32(ddlRegularType.SelectedValue);
            if (RegularID == 0)
            {
                Entity = new ActivityAwardEntity();
                Entity.ActivityID = ActivityID;
                Entity.LotteryCode = LotteryCode;
                Entity.RegularType = RegularType;
                Entity.RegularStatus = 0;
                Entity.TotalAwardMoney = 0;
                int rec = new ActivityAwardBLL().InsertEntity(Entity);
                if (rec > 0)
                {
                    lbMsg.Text = "新增成功";
                    return;
                }
                else
                {
                    lbMsg.Text = "新增失败，请联系管理员";
                    return;
                }
            }
        }
        protected string SetPlayCode(object obj)
        {
            string rec = string.Empty;
            if (obj == null || PlayTypes == null || PlayTypes.Count == 0)
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

    }
}