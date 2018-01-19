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
    public partial class activity : UI.AdminPage
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
            var Entity = new ActivityBLL().QueryEntity(ActivityID);
            txtActivitySubject.Text = Entity.ActivitySubject;
            txtLandingPage.Text = Entity.LandingPage;
            txtActivityMoney.Text = (Entity.ActivityMoney / 100).ToString();
            txtStartTime.Text = Entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
            txtKeyEndTime.Text = Entity.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            areaActivityDescribe.Value = Entity.ActivityDescribe;
            if (Entity.ActivityApply == (int)ActivityApply.Apply)
                btnSubmit.Enabled = true;
            else
            {
                btnSubmit.BackColor = Color.Gray;
                btnSubmit.Enabled = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lbMsg.Text = "";
            try
            {
                string ActivitySubject = txtActivitySubject.Text.Trim();
                string ActivityType = ddlActivityType.SelectedValue;
                DateTime StartTime = DateTime.Now;  //txtStartTime
                DateTime EndTime = DateTime.Now; //txtKeyEndTime
                long ActivityMoney = 0;              //txtActivityMoney
                string CurrencyUnit = ddlCurrencyUnit.SelectedValue;
                string LandingPage = txtLandingPage.Text.Trim();
                string ActivityDescribe = areaActivityDescribe.Value.Trim();
                string ADUrl = hidAdUrl.Value.Trim();
                if (string.IsNullOrEmpty(txtStartTime.Text.Trim()))
                {
                    lbMsg.Text = "请输入活动开始时间";
                    return;
                }
                if (string.IsNullOrEmpty(txtKeyEndTime.Text.Trim()))
                {
                    lbMsg.Text = "请输入活动结束时间";
                    return;
                }
                if (string.IsNullOrEmpty(txtActivityMoney.Text.Trim()))
                {
                    lbMsg.Text = "请输入活动总金额";
                    return;
                }
                if (string.IsNullOrEmpty(ADUrl))
                {
                    lbMsg.Text = "请上传app广告图片(规格为：1020*225)";
                    return;
                }
                DateTime.TryParse(txtStartTime.Text.Trim(), out StartTime);
                DateTime.TryParse(txtKeyEndTime.Text.Trim(), out EndTime);
                long.TryParse(txtActivityMoney.Text.Trim(), out ActivityMoney);
                if (ActivityMoney == 0)
                {
                    lbMsg.Text = "请输入有效的活动总金额";
                    return;
                }
                if (StartTime >= EndTime)
                {
                    lbMsg.Text = "开始时间必须小于结束时间";
                    return;
                }
                if (Entity == null)
                {
                    Entity = new ActivityEntity();
                    Entity.CreateTime = DateTime.Now;
                }
                else
                {
                    if (Entity.ActivityApply != 0)
                    {
                        lbMsg.Text = "活动为不可更新状态";
                        return;
                    }
                }
                Entity.ActivityID = ActivityID;
                Entity.ActivityApply = (int)ActivityApply.Apply;
                Entity.ActivityDescribe = ActivityDescribe;
                Entity.ActivityMoney = ActivityMoney * 100;
                Entity.ActivitySubject = ActivitySubject;
                Entity.ActivityType = Convert.ToInt32(ActivityType);
                Entity.CurrencyUnit = Convert.ToByte(CurrencyUnit);
                Entity.EndTime = EndTime;
                Entity.IsModify = false;
                Entity.StartTime = StartTime;
                Entity.LandingPage = LandingPage;
                Entity.ADUrl = ADUrl;
                int rec = this.ModifyEntity(Entity);
                if (rec > 0)
                {
                    lbMsg.Text = "活动申请完成，等待审核";
                    return;
                }
                else
                {
                    lbMsg.Text = "活动申请失败，请联系管理员";
                    return;
                }
            }
            catch
            {
                lbMsg.Text = "系统繁忙，请联系管理员";
                return;
            }
        }
        protected int ModifyEntity(ActivityEntity Entity)
        {
            if (ActivityID > 0)
                return new ActivityBLL().UpdateEntity(Entity);
            else
                return new ActivityBLL().InsertEntity(Entity);
        }
    }
}