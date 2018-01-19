using CL.Enum.Common;
using CL.Enum.Common.Activity;
using CL.Game.BLL;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.activity
{
    public partial class activitylist : UI.AdminPage
    {
        protected static int PageIndex = 1;
        protected static int PageSize = 10;
        protected static string Start_Time = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
        protected static string End_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartTime.Text = Start_Time;
                txtEndTime.Text = End_Time;
                BindData();

            }
        }

        protected void BindData(int Page = 0)
        {
            if (Page == 0)
                PageIndex = QPRequest.GetQueryInt("PageIndex", 1);
            else
                PageIndex = Page;
            //参数
            int ActivityType = Convert.ToInt32(ddlActivityType.SelectedValue);
            int IsModify = Convert.ToInt32(ddlIsModify.SelectedValue);
            int ActivityApply = Convert.ToInt32(ddlActivityApply.SelectedValue);
            int CurrencyUnit = Convert.ToInt32(ddlCurrencyUnit.SelectedValue);
            string Keys = txtKeys.Text.Trim();
            DateTime StartTime = Convert.ToDateTime(txtStartTime.Text);
            DateTime EndTime = Convert.ToDateTime(txtEndTime.Text);
            Start_Time = txtStartTime.Text;
            End_Time = txtEndTime.Text;
            int totalCount = 0;
            this.rptList.DataSource = new ActivityBLL().QueryActivityList(Keys, ActivityType, StartTime, EndTime, IsModify, ActivityApply, CurrencyUnit, PageIndex, PageSize, ref totalCount);
            this.rptList.DataBind();
            string pageUrl = Utils.CombUrlTxt("activitylist.aspx", "PageIndex={0}", "__id__");
            PageContent.InnerHtml = Utils.OutPageList(PageSize, PageIndex, totalCount, pageUrl, 10);
        }

        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            BindData(1);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ChkAdminLevel("user_list", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
            int ActivityID = Convert.ToInt32(((HiddenField)e.Item.FindControl("hidActivityID")).Value);
            switch (e.CommandName)
            {
                case "Apply":
                    //活动申请状态：0 申请中，1 审核通过，2 审核拒绝，3变更申请，4变更审核通过，5变更审核拒绝
                    this.Response.Redirect(string.Format("activityapply.aspx?id={0}", ActivityID));
                    break;
                case "Look":
                    bool IsModify = Convert.ToBoolean(e.CommandArgument);
                    if (IsModify)
                        this.Response.Redirect(string.Format("activityapply.aspx?id={0}", ActivityID));
                    else
                        this.Response.Redirect(string.Format("activity.aspx?id={0}", ActivityID));
                    break;
                case "Regular":
                    LinkButton LinRegular = ((LinkButton)e.Item.FindControl("LinRegular"));
                    string ActivitySubject = LinRegular.CommandArgument;
                    this.Response.Redirect(string.Format("regular/awardlist.aspx?id={0}&sub={1}", ActivityID, ActivitySubject));
                    break;
            }
        }
        protected string SetApply(object obj)
        {
            //活动申请状态：0 申请中，1 审核通过，2 审核拒绝，3 变更申请，4 变更审核通过，5 变更审核拒绝
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            else
                rec = Common.GetDescription((ActivityApply)Convert.ToInt32(obj));
            return rec;
        }
        protected string SetType(object obj)
        {
            //活动类型：0 官方活动，1 彩乐平台活动
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            else
                rec = Common.GetDescription((ActivityType)Convert.ToInt32(obj));
            return rec;
        }
        protected string SetUnit(object obj)
        {
            //币种单位(彩乐平台币种)：0 人民币(余额)，1 元宝，2 游戏币，3 彩券
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            else
                rec = Common.GetDescription((ActivityUnit)Convert.ToInt32(obj));
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
        protected string Sub(object obj)
        {
            string rec = string.Empty;
            if (obj != null)
            {
                if (obj.ToString().Length <= 5)
                    rec = obj.ToString();
                else
                    rec = obj.ToString().Substring(0, 5) + "...";
            }
            return rec;
        }

        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    PageIndex = _pagesize;
                    BindData();
                    Utils.WriteCookie("activitylist_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                    Response.Redirect(Utils.CombUrlTxt("activitylist.aspx", "PageIndex={0}", _pagesize.ToString()));
                }
            }
        }

    }
}