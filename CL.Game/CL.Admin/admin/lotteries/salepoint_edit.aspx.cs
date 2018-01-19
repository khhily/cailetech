using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CL.Enum.Common.Status;
using CL.Enum.Common.Lottery;

namespace CL.Admin.admin.lotteries
{
    public partial class salepoint_edit : UI.AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("salepoint_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                LotteryBind();
                hidFileSign.Value = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }


        #region 彩种类型=========================
        private void LotteryBind()
        {
            List<LotteriesEntity> list = new LotteriesBLL().QueryEntitys(string.Empty);
            ddlLottery.DataValueField = "LotteryCode";
            ddlLottery.DataTextField = "LotteryName";
            ddlLottery.DataSource = list;
            ddlLottery.DataBind();
        }
        #endregion


        #region 表单提交
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            ChkAdminLevel("lotteries_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd())
            {
                JscriptMsg("保存过程中发生错误！", string.Empty);
                return;
            }
            JscriptMsg("提交点位变更成功，请尽快联系审核人进行审核！", "salepoint_list.aspx");
        }
        #endregion


        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = false;
            SalePointEntity model = new SalePointEntity();
            SalePointBLL bll = new SalePointBLL();

            model.TicketSourceID = Convert.ToInt32(rblTicketSourceID.SelectedValue);
            model.LotteryCode = Convert.ToInt32(ddlLottery.SelectedValue);
            model.LotteryName = ddlLottery.SelectedItem.Text;
            model.SalesRebate = issalepoint.Value;
            model.FileSign = Convert.ToInt64(hidFileSign.Value);
            model.OperatorID = GetAdminInfo().id;
            model.OperatorName = GetAdminInfo().NickName;
            model.OperatorTime = DateTime.Now;
            model.SalePointStatus = 1;
            model.StartTime = Convert.ToDateTime(txtStartTime.Text.Trim() + " 00:00:00");
            model.Describe = txtDescribe.Text.Trim();

            if (bll.InsertEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加点位变更 出票商:" + model.TicketSourceID + ",彩票编码:" + model.LotteryCode + ",生效时间:" + model.StartTime); //记录日志
                result = true;
            }
            return result;
        }
        #endregion


        /// <summary>
        /// 得到CheckBoxList中选中了的值
        /// </summary>
        /// <param name="checkList">CheckBoxList</param>
        /// <param name="separator">分割符号</param>
        /// <returns></returns>
        public static string GetChecked(CheckBoxList checkList, string separator)
        {
            string selval = "";
            for (int i = 0; i < checkList.Items.Count; i++)
            {
                if (checkList.Items[i].Selected)
                {
                    selval += checkList.Items[i].Value + separator;
                }
            }
            selval = selval.Substring(0, selval.Length - 1);
            return selval;
        }
        /// <summary>
        /// 得到CheckBoxList中选中了的文本
        /// </summary>
        /// <param name="checkList">CheckBoxList</param>
        /// <param name="separator">分割符号</param>
        /// <returns></returns>
        public static string GetCheckedText(CheckBoxList checkList, string separator)
        {
            string selval = "";
            for (int i = 0; i < checkList.Items.Count; i++)
            {
                if (checkList.Items[i].Selected)
                {
                    selval += checkList.Items[i].Text + separator;
                }
            }
            selval = selval.Substring(0, selval.Length - 1);
            return selval;
        }
    }
}