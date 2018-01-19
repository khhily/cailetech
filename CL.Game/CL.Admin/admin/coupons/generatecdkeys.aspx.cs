using CL.Game.BLL;
using CL.Coupons.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.coupons
{
    public partial class generatecdkeys : UI.AdminPage
    {
        protected static bool IsSubmit = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.BindDropDownList();
            }
        }
        protected void BindDropDownList()
        {
            var Entitys = new ActivityBLL().QueryEntitys();
            if (Entitys == null)
            {
                lbMsg.Text = "暂无活动，请先添加彩乐平台活动再生成兑换码。";
                IsSubmit = false;
                return;
            }
            ddlActivity.DataTextField = "ActivitySubject";
            ddlActivity.DataValueField = "ActivityID";
            ddlActivity.DataSource = Entitys;
            ddlActivity.DataBind();
        }
        /// <summary>
        /// 开始生成兑换码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int ActivityID = 0;
                int.TryParse(ddlActivity.SelectedValue, out ActivityID);
                if (IsSubmit || ActivityID > 0)
                {
                    int LotteryCode = Convert.ToInt32(ddlLotteryCode.SelectedValue);
                    int CouponsType = Convert.ToInt32(ddlType.SelectedValue);
                    DateTime StartTime = DateTime.Now;
                    DateTime ExpireTime = DateTime.Now;
                    DateTime KeyExpireTime = DateTime.Now;
                    long SatisfiedMoney = 0;
                    long FaceValue = 0;
                    bool IsGive = Convert.ToBoolean(ddlIsGive.SelectedValue);
                    bool IsChaseTask = Convert.ToBoolean(ddlIsChaseTask.SelectedValue);
                    bool IsSuperposition = Convert.ToBoolean(ddlIsSuperposition.SelectedValue);
                    bool IsTimes = Convert.ToBoolean(ddlIsTimes.SelectedValue);
                    bool IsJoinBuy = Convert.ToBoolean(ddlIsJoinBuy.SelectedValue);
                    int CouponsProxy = Convert.ToInt32(ddlProxy.SelectedValue);
                    int CouponsCount = Convert.ToInt32(ddlCouponsCount.SelectedValue);
                    long.TryParse(txtSatisfiedMoney.Text.Trim(), out SatisfiedMoney);
                    long.TryParse(txtFaceValue.Text.Trim(), out FaceValue);
                    if (FaceValue == 0)
                    {
                        lbMsg.Text = "请填写正确彩券面额";
                        return;
                    }
                    if (CouponsType == 2 && SatisfiedMoney == 0)
                    {
                        lbMsg.Text = "请填写正确满减金额";
                        return;
                    }
                    if (string.IsNullOrEmpty(txtStartTime.Text.Trim()))
                    {
                        lbMsg.Text = "请填写正确彩券开始使用时间";
                        return;
                    }
                    if (CouponsType != 3)
                    {
                        if (string.IsNullOrEmpty(txtExpireTime.Text.Trim()))
                        {
                            lbMsg.Text = "请填写正确彩券失效时间";
                            return;
                        }
                    }
                    if (string.IsNullOrEmpty(txtKeyExpireTime.Text.Trim()))
                    {
                        lbMsg.Text = "请填写正确兑换码失效时间";
                        return;
                    }
                    DateTime.TryParse(txtStartTime.Text.Trim(), out StartTime);
                    DateTime.TryParse(txtExpireTime.Text.Trim(), out ExpireTime);
                    DateTime.TryParse(txtKeyExpireTime.Text.Trim(), out KeyExpireTime);

                    bool rec = new CouponsBLL().GenerateCDKeys(ActivityID, CouponsProxy, LotteryCode, CouponsType, StartTime, ExpireTime, KeyExpireTime, SatisfiedMoney, FaceValue, IsGive, IsChaseTask, IsSuperposition, IsTimes, IsJoinBuy, CouponsCount);
                    if (rec)
                        lbMsg.Text = "兑换码生成完成";
                    else
                        lbMsg.Text = "兑换码生成失败";
                }
            }
            catch (Exception ex)
            {
                lbMsg.Text = "请联系管理员，系统错误：" + ex.Message;
            }
        }
    }
}