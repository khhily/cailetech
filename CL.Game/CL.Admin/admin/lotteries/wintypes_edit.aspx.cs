using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Web.UI;

namespace CL.Admin.admin.lotteries
{
    public partial class wintypes_edit : UI.AdminPage
    {
        private string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;
        protected int LotteryCode = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            LotteryCode = QPRequest.GetQueryInt("LotteryCode");
            string _action = QPRequest.GetQueryString("action");

            if (_action == CaileEnums.ActionEnum.Edit.ToString())
            {
                this.action = _action;//修改类型
                this.id = QPRequest.GetQueryInt("id");
                string message = string.Empty;
                if (!new WinTypesBLL().QueryExists(this.id, ref message))
                    JscriptMsg(message, "back");
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("lotteries_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }

        #region 赋值操作
        private void ShowInfo(int _id)
        {
            WinTypesBLL bll = new WinTypesBLL();
            WinTypesEntity model = bll.QueryEntity(_id);

            txtWinName.Text = model.WinName;
            txtWinCode.Text = model.WinCode.ToString();
            txtWinCode.ReadOnly = true;
            txtWinCode.Attributes.Remove("ajaxurl");

            txtDefaultMoney.Text = (model.DefaultMoney / 100).ToString();
            txtDefaultMoneyNoWithTax.Text = (model.DefaultMoneyNoWithTax / 100).ToString();
            rblIsSumValue.SelectedValue = model.IsSumValue == null ? "0" : model.IsSumValue == 1 ? "1" : "0";
            txtWinNumber.Text = model.WinNumber;
            txtSortId.Text = model.Sort.ToString();
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = false;
            WinTypesEntity model = new WinTypesEntity();
            WinTypesBLL bll = new WinTypesBLL();
            model.WinID = 0;
            model.WinName = txtWinName.Text.Trim();
            model.WinCode = Convert.ToInt32(txtWinCode.Text);
            model.LotteryCode = LotteryCode;
            model.DefaultMoney = Convert.ToInt32(txtDefaultMoney.Text) * 100;
            model.DefaultMoneyNoWithTax = Convert.ToInt32(txtDefaultMoneyNoWithTax.Text) * 100;
            model.IsSumValue = (byte)(rblIsSumValue.SelectedValue == "1" ? 1 : 0);
            model.WinNumber = txtWinNumber.Text.Trim();
            model.Sort = Convert.ToInt32(txtSortId.Text);

            if (bll.InsertEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加奖等:" + model.WinName); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            WinTypesBLL bll = new WinTypesBLL();
            WinTypesEntity model = bll.QueryEntity(_id);

            model.WinName = txtWinName.Text.Trim();
            model.WinCode = Convert.ToInt32(txtWinCode.Text);
            model.LotteryCode = LotteryCode;
            model.DefaultMoney = Convert.ToInt32(txtDefaultMoney.Text) * 100;
            model.DefaultMoneyNoWithTax = Convert.ToInt32(txtDefaultMoneyNoWithTax.Text) * 100;
            model.IsSumValue = (byte)(rblIsSumValue.SelectedValue == "1" ? 1 : 0);
            model.WinNumber = txtWinNumber.Text.Trim();
            model.Sort = Convert.ToInt32(txtSortId.Text);

            if (bll.ModifyEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Edit.ToString(), "修改奖等:" + model.WinName); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("lotteries_list", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", string.Empty);
                    return;
                }
                JscriptMsg("修改奖等成功！", "wintypes_list.aspx?LotteryCode=" + LotteryCode);
            }
            else //添加
            {
                ChkAdminLevel("lotteries_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", string.Empty);
                    return;
                }
                JscriptMsg("添加奖等成功！", "wintypes_list.aspx?LotteryCode=" + LotteryCode);
            }
        }

    }
}