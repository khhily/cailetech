using CL.Enum.Common;
using CL.Enum.Common.Activity;
using CL.Game.BLL.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.activity
{
    public partial class activityauditinglist : UI.AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        protected void BindData()
        {
            this.rptList.DataSource = new udv_ActivityApplyBLL().QueryEntitys(0);
            this.rptList.DataBind();
        }


        protected string SetCurrencyUnit(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            else
                rec = Common.GetDescription((ActivityUnit)Convert.ToInt32(obj));
            return rec;
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int ActivityID = Convert.ToInt32(((HiddenField)e.Item.FindControl("hidActivityID")).Value);
            switch (e.CommandName.Trim())
            {
                case "Apply":
                    Response.Redirect(string.Format("regular/awardauditing.aspx?id={0}", ActivityID));
                    break;
            }
        }
    }
}