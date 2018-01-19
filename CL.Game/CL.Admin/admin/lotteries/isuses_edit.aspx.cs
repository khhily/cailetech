using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Web.UI;

namespace CL.Admin.admin.lotteries
{
    public partial class isuses_edit : UI.AdminPage
    {
        private string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型
        private long id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = QPRequest.GetQueryString("action");

            if (_action == CaileEnums.ActionEnum.Edit.ToString())
            {
                this.action = _action;//修改类型
                this.id = QPRequest.GetQueryInt("id");
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new IsusesBLL().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("lotteries_isuses", CaileEnums.ActionEnum.View.ToString()); //检查权限
                if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }

        #region 赋值操作
        private void ShowInfo(long _id)
        {
            IsusesBLL bll = new IsusesBLL();
            IsusesEntity model = bll.QueryEntity(_id);

            txtIsuseName.Text = model.IsuseName;
            txtStartTime.Text = model.StartTime.ToString();
            txtEndTime.Text = model.EndTime.ToString();
            ddlIsuseState.SelectedValue = model.IsuseState.ToString();
            txtOpenNumber.Text = model.OpenNumber;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(long _id)
        {
            bool result = false;
            IsusesBLL bll = new IsusesBLL();
            IsusesEntity model = bll.QueryEntity(_id);

            model.IsuseName = txtIsuseName.Text.Trim();
            model.StartTime = Convert.ToDateTime(txtStartTime.Text.Trim());
            model.EndTime = Convert.ToDateTime(txtEndTime.Text.Trim());
            model.IsuseState = Convert.ToByte(ddlIsuseState.SelectedValue);
            //model.OpenNumber = txtOpenNumber.Text.Trim();
            model.IsOpened = string.IsNullOrEmpty(txtOpenNumber.Text.Trim());

            if (bll.ModifyEntity(model))
            {
                AddAdminLog(CaileEnums.ActionEnum.Edit.ToString(), "修改彩种：" + model.LotteryCode + "的期号:" + model.IsuseName); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("lotteries_isuses", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", string.Empty);
                    return;
                }
                JscriptMsg("修改期号信息成功！", "isuses_list.aspx");
            }
        }

    }
}