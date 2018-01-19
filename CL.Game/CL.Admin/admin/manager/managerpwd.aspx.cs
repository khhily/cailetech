using CL.SystemInfo.BLL;
using CL.SystemInfo.Entity;
using CL.Tools.Common;
using System;
using System.Web.UI;

namespace CL.Admin.admin.manager
{
    public partial class managerpwd : UI.AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowInfo(Managerinfo.id);
            }
        }

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            ManagerBLL bll = new ManagerBLL();
            ManagerEntity model = bll.GetModel(_id);
            txtUserName.Text = model.UserName;
            txtRealName.Text = model.NickName;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ManagerBLL bll = new ManagerBLL();
            ManagerEntity model = GetAdminInfo();

            if (Utils.MD5(txtOldPassword.Text.Trim()) != model.PassWord)
            {
                JscriptMsg("旧密码不正确！", "");
                return;
            }
            if (txtPassword.Text.Trim() != txtPassword1.Text.Trim())
            {
                JscriptMsg("两次密码不一致！", "");
                return;
            }
            model.PassWord = Utils.MD5(txtPassword.Text.Trim());
            model.NickName = txtRealName.Text.Trim();

            if (bll.ModifyEntity(model) <= 0)
            {
                JscriptMsg("保存过程中发生错误！", "");
                return;
            }
            Session[CLKeys.SESSION_ADMIN_INFO] = null;
            JscriptMsg("密码修改成功！", "managerpwd.aspx");
        }

    }
}