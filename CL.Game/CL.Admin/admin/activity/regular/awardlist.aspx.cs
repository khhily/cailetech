using CL.Enum.Common;
using CL.Enum.Common.Activity.Regular;
using CL.Enum.Common.Lottery;
using CL.Game.BLL;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.activity.regular
{
    public partial class award : UI.AdminPage
    {
        protected static int ActivityID = 0;
        protected static string ActivitySubject = string.Empty;
        protected static List<ActivityAwardEntity> Entitys = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int.TryParse(Request.QueryString["id"], out ActivityID);
                ActivitySubject = Request.QueryString["sub"];
                if (ActivityID > 0)
                {
                    BindData();
                }
                else
                    Response.Redirect(Request.UrlReferrer.AbsoluteUri);
            }
        }


        protected void BindData()
        {
            lbActivitySubject.Text = ActivitySubject.Trim();
            int RegularType = Convert.ToInt32(ddlRegularType.SelectedValue);
            int RegularLottery = Convert.ToInt32(ddlRegularLottery.SelectedValue);
            int RegularStatus = Convert.ToInt32(ddlRegularStatus.SelectedValue);
            Entitys = new ActivityAwardBLL().QueryEntitys(ActivityID);
            if (RegularType != -1)
                Entitys = Entitys.Where(w => w.RegularType == RegularType).ToList();
            if (RegularLottery != -1)
                Entitys = Entitys.Where(w => w.LotteryCode == RegularLottery).ToList();
            if (RegularStatus != -1)
                Entitys = Entitys.Where(w => w.RegularStatus == RegularStatus).ToList();
            this.rptList.DataSource = Entitys;
            this.rptList.DataBind();

        }

        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            BindData();
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

        protected void linkUrl_Click(object sender, EventArgs e)
        {

            var ActivityEntity = new ActivityBLL().QueryEntity(ActivityID);
            if (ActivityEntity != null)
            {
                if (ActivityEntity.ActivityApply != 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "javascript", "alert('只能申请活动时增加规则')", true);
                    return;
                }
                //跳转
                Response.Redirect(string.Format("awardregular.aspx?acid={0}", ActivityID));
            }
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int RegularID = Convert.ToInt32(((HiddenField)e.Item.FindControl("hidRegularID")).Value);
            switch (e.CommandName.Trim())
            {
                case "Regular":
                    Response.Redirect(string.Format("awardregular.aspx?id={0}&acid={1}", RegularID, ActivityID));
                    break;
            }

        }
    }
}