using CL.Enum.Common;
using CL.SystemInfo.BLL;
using CL.SystemInfo.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.manager
{
    public partial class manageredit : UI.AdminPage
    {
        string defaultpassword = "0|0|0|0"; //默认显示密码
        private string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = QPRequest.GetQueryString("action");
            if (!string.IsNullOrEmpty(_action) && _action == CaileEnums.ActionEnum.Edit.ToString())
            {
                this.action = CaileEnums.ActionEnum.Edit.ToString();//修改类型
                if (!int.TryParse(Request.QueryString["id"] as string, out this.id))
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new ManagerBLL().Exists(this.id))
                {
                    JscriptMsg("记录不存在或已被删除！", "back");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("manager_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                ManagerEntity model = GetAdminInfo(); //取得管理员信息
                RoleBind(ddlRoleId, model.RoleType);
                if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }

        #region 角色类型================================= 
        private void RoleBind(DropDownList ddl, int role_type)
        {
            RosleBLL bll = new RosleBLL();
            List<RosleEntity> list = bll.GetList("");

            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("请选择角色...", ""));
            foreach (RosleEntity item in list)
            {
                if (item.RoleType >= role_type)
                {
                    ddl.Items.Add(new ListItem(item.RoleName, item.RoleID.ToString()));
                }
            }
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            ManagerBLL bll = new ManagerBLL();
            ManagerEntity model = bll.GetModel(_id);
            ddlRoleId.SelectedValue = model.RoleID.ToString();
            if (model.IsLock == 0)
            {
                cbIsLock.Checked = true;
            }
            else
            {
                cbIsLock.Checked = false;
            }
            txtUserName.Text = model.UserName;
            txtUserName.ReadOnly = true;
            txtUserName.Attributes.Remove("ajaxurl");
            if (!string.IsNullOrEmpty(model.PassWord))
            {
                txtPassword.Attributes["value"] = txtPassword1.Attributes["value"] = defaultpassword;
            }
            txtRealName.Text = model.NickName;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            ManagerEntity model = new ManagerEntity();
            ManagerBLL bll = new ManagerBLL();
            model.RoleID = int.Parse(ddlRoleId.SelectedValue);
            model.RoleType = new RosleBLL().GetModel(model.RoleID).RoleType;
            if (cbIsLock.Checked == true)
            {
                model.IsLock = 0;
            }
            else
            {
                model.IsLock = 1;
            }
            //检测用户名是否重复
            if (bll.Exists(txtUserName.Text.Trim()))
            {
                return false;
            }
            model.UserName = txtUserName.Text.Trim();
            //以随机生成的6位字符串做为密钥加密
            model.PassWord = Utils.MD5(txtPassword.Text.Trim());
            model.NickName = txtRealName.Text.Trim();
            model.AddTime = DateTime.Now;

            if (bll.InsertEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加管理员:" + model.UserName); //记录日志
                return true;
            }
            return false;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            ManagerBLL bll = new ManagerBLL();
            ManagerEntity model = bll.GetModel(_id);

            model.RoleID = int.Parse(ddlRoleId.SelectedValue);
            model.RoleType = new RosleBLL().GetModel(model.RoleID).RoleType;
            if (cbIsLock.Checked == true)
            {
                model.IsLock = 0;
            }
            else
            {
                model.IsLock = 1;
            }
            //判断密码是否更改
            if (txtPassword.Text.Trim() != defaultpassword)
            {
                //获取用户已生成的salt作为密钥加密
                model.PassWord = Utils.MD5(txtPassword.Text.Trim());
            }
            model.NickName = txtRealName.Text.Trim();

            if (bll.ModifyEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Edit.ToString(), "修改管理员:" + model.UserName); //记录日志
                result = true;
            }

            return result;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("manager_list", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("修改管理员信息成功！", "managerlist.aspx");
            }
            else //添加
            {
                ChkAdminLevel("manager_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("添加管理员信息成功！", "managerlist.aspx");
            }
        }

    }
}