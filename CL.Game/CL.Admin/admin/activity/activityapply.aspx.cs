using CL.Enum.Common;
using CL.Enum.Common.Activity;
using CL.Game.BLL;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.activity
{
    public partial class activityapply : UI.AdminPage
    {
        protected static int ActivityID = 0;
        public static ActivityEntity Entity = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int.TryParse(Request.QueryString["id"], out ActivityID);
                if (ActivityID > 0)
                    BindData();
                else
                    Entity = null;
            }
        }
        protected void BindData()
        {
            Entity = new ActivityBLL().QueryEntity(ActivityID);
            txtActivityApply.Text = Common.GetDescription((ActivityApply)Entity.ActivityApply);
            txtActivityType.Text = Common.GetDescription((ActivityType)Entity.ActivityType);
            txtActivitySubject.Text = Entity.ActivitySubject;
            txtStartTime.Text = Entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
            txtEndTime.Text = Entity.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            txtActivityMoney.Text = (Entity.ActivityMoney / 100).ToString();
            txtCurrencyUnit.Text = Common.GetDescription((ActivityUnit)Entity.CurrencyUnit);
            txtLandingPage.Text = Entity.LandingPage;
            txtActivityDescribe.Text = Entity.ActivityDescribe;
            txtModifyDescribe.Text = Entity.ModifyDescribe;
            txtModifyMoney.Text = (Entity.ModifyMoney / 100).ToString();
            txtModifyTime.Text = Entity.ModifyTime == null ? "" : Convert.ToDateTime(Entity.ModifyTime).ToString("yyyy-MM-dd HH:mm:ss");
            if (Entity.ActivityApply == (int)ActivityApply.AuditingAdopt || Entity.ActivityApply == (int)ActivityApply.UpdateApply)
                btnSubmit.Enabled = true;
            else
            {
                btnSubmit.BackColor = Color.Gray;
                btnSubmit.Enabled = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DateTime ModifyTime = DateTime.Now;
            long ModifyMoney = 0;
            long.TryParse(txtModifyMoney.Text.Trim(), out ModifyMoney);
            if (string.IsNullOrEmpty(txtModifyTime.Text.Trim()))
            {
                lbMsg.Text = "请选择变更时间";
                return;
            }
            if (ModifyMoney == 0)
            {
                lbMsg.Text = "请填写正确金额";
                return;
            }
            if (ModifyMoney < Entity.ActivityMoney)
            {
                lbMsg.Text = "变更金额必须大于或等于活动金额";
                return;
            }
            DateTime.TryParse(txtModifyTime.Text.Trim(), out ModifyTime);
            if (ModifyTime <= Entity.EndTime)
            {
                lbMsg.Text = "变更时间必须比结束时间大";
                return;
            }
            Entity.ModifyMoney = ModifyMoney * 100;
            Entity.IsModify = true;
            Entity.ModifyTime = ModifyTime;
            Entity.ModifyDescribe = txtModifyDescribe.Text.Trim();
            int rec = new ActivityBLL().UpdateEntity(Entity);
            if (rec > 0)
            {
                lbMsg.Text = "变更申请完成，等待审核";
                return;
            }
            else
            {
                lbMsg.Text = "变更申请失败，请联系管理员";
                return;
            }
        }
    }
}