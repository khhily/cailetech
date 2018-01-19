using CL.Enum.Common;
using CL.SystemInfo.BLL;
using CL.SystemInfo.Entity;
using CL.Tools.Common;
using CL.View.Entity.SystemInfo;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.settings
{
    public partial class nav_edit : UI.AdminPage
    {
        private string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = QPRequest.GetQueryString("action");
            this.id = QPRequest.GetQueryInt("id");

            if (!string.IsNullOrEmpty(_action) && _action == CaileEnums.ActionEnum.Edit.ToString())
            {
                this.action = CaileEnums.ActionEnum.Edit.ToString();//修改类型
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new NavigationBLL().Exists(this.id))
                {
                    JscriptMsg("导航不存在或已被删除！", "back");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("sys_navigation", CaileEnums.ActionEnum.View.ToString()); //检查权限
                TreeBind(); //绑定导航菜单
                ActionTypeBind(); //绑定操作权限类型
                if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
                else
                {
                    if (this.id > 0)
                    {
                        this.ddlParentId.SelectedValue = this.id.ToString();
                    }
                    txtName.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=navigation_validate");
                }
            }
        }

        #region 绑定导航菜单=============================
        private void TreeBind()
        {
            NavigationBLL bll = new NavigationBLL();
            List<udv_Navigation> dt = bll.QueryEntitys(0);

            this.ddlParentId.Items.Clear();
            this.ddlParentId.Items.Add(new ListItem("无父级导航", "0"));
            foreach (udv_Navigation dr in dt)
            {
                string Id = dr.id.ToString();
                int ClassLayer = dr.class_layer;
                string Title = dr.Title;

                if (ClassLayer == 1)
                {
                    this.ddlParentId.Items.Add(new ListItem(Title, Id));
                }
                else
                {
                    Title = "├ " + Title;
                    Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                    this.ddlParentId.Items.Add(new ListItem(Title, Id));
                }
            }
        }
        #endregion

        #region 绑定操作权限类型=========================
        private void ActionTypeBind()
        {
            cblActionType.Items.Clear();
            foreach (KeyValuePair<string, string> kvp in Utils.ActionType())
            {
                cblActionType.Items.Add(new ListItem(kvp.Value + "(" + kvp.Key + ")", kvp.Key));
            }
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            NavigationBLL bll = new NavigationBLL();
            NavigationEntity model = bll.QueryEntity(_id);

            ddlParentId.SelectedValue = model.ParentID.ToString();
            txtSortId.Text = model.SortID.ToString();
            if (model.IsLock == 1)
            {
                cbIsLock.Checked = true;
            }
            txtName.Text = model.Name;
            txtName.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=navigation_validate&old_name=" + Utils.UrlEncode(model.Name));
            txtName.Focus(); //设置焦点，防止JS无法提交
            if (model.IsSys == 1)
            {
                ddlParentId.Enabled = false;
                txtName.ReadOnly = true;
            }
            txtTitle.Text = model.Title;
            txtLinkUrl.Text = model.LinkUrl;
            txtRemark.Text = model.Remark;
            //赋值操作权限类型
            string[] actionTypeArr = model.ActionType.Split(',');
            for (int i = 0; i < cblActionType.Items.Count; i++)
            {
                for (int n = 0; n < actionTypeArr.Length; n++)
                {
                    if (actionTypeArr[n].ToLower() == cblActionType.Items[i].Value.ToLower())
                    {
                        cblActionType.Items[i].Selected = true;
                    }
                }
            }
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            try
            {
                NavigationEntity model = new NavigationEntity();
                NavigationBLL bll = new NavigationBLL();

                model.Name = txtName.Text.Trim();
                model.Title = txtTitle.Text.Trim();
                model.LinkUrl = txtLinkUrl.Text.Trim();
                model.SortID = int.Parse(txtSortId.Text.Trim());
                model.IsLock = 0;
                if (cbIsLock.Checked == true)
                {
                    model.IsLock = 1;
                }
                model.Remark = txtRemark.Text.Trim();
                model.ParentID = int.Parse(ddlParentId.SelectedValue);

                //添加操作权限类型
                string action_type_str = string.Empty;
                for (int i = 0; i < cblActionType.Items.Count; i++)
                {
                    if (cblActionType.Items[i].Selected && Utils.ActionType().ContainsKey(cblActionType.Items[i].Value))
                    {
                        action_type_str += cblActionType.Items[i].Value + ",";
                    }
                }
                model.ActionType = Utils.DelLastComma(action_type_str);

                if (bll.InsertEntity(model) > 0)
                {
                    AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加导航菜单:" + model.Title); //记录日志
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            try
            {
                NavigationBLL bll = new NavigationBLL();
                NavigationEntity model = bll.QueryEntity(_id);

                model.Name = txtName.Text.Trim();
                model.Title = txtTitle.Text.Trim();
                model.LinkUrl = txtLinkUrl.Text.Trim();
                model.SortID = int.Parse(txtSortId.Text.Trim());
                model.IsLock = 0;
                if (cbIsLock.Checked == true)
                {
                    model.IsLock = 1;
                }
                model.Remark = txtRemark.Text.Trim();
                if (model.IsSys == 0)
                {
                    int parentId = int.Parse(ddlParentId.SelectedValue);
                    //如果选择的父ID不是自己,则更改
                    if (parentId != model.id)
                    {
                        model.ParentID = parentId;
                    }
                }

                //添加操作权限类型
                string action_type_str = string.Empty;
                for (int i = 0; i < cblActionType.Items.Count; i++)
                {
                    if (cblActionType.Items[i].Selected && Utils.ActionType().ContainsKey(cblActionType.Items[i].Value))
                    {
                        action_type_str += cblActionType.Items[i].Value + ",";
                    }
                }
                model.ActionType = Utils.DelLastComma(action_type_str);

                if (bll.ModifyEntity(model) > 0)
                {
                    AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "修改导航菜单:" + model.Title); //记录日志
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("sys_navigation", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("修改导航菜单成功！", "nav_list.aspx", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel("sys_navigation", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("添加导航菜单成功！", "nav_list.aspx", "parent.loadMenuTree");
            }
        }

    }
}