using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.news
{
    public partial class newstypes_edit : UI.AdminPage
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
                if (!new NewsTypesBLL().Exists(this.id))
                {
                    JscriptMsg("导航不存在或已被删除！", "back");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("newstypes_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                TreeBind(); //绑定栏目
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
            NewsTypesBLL bll = new NewsTypesBLL();
            List<udv_NewsTypes> list = bll.QueryEntitys(0, 0);

            this.ddlParentId.Items.Clear();
            this.ddlParentId.Items.Add(new ListItem("无父级栏目", "0"));
            foreach (udv_NewsTypes item in list)
            {
                string Id = item.TypeID.ToString();
                int ClassLayer = item.ClassLayer;
                string Title = item.TypeName;

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

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            NewsTypesBLL bll = new NewsTypesBLL();
            NewsTypesEntity model = bll.QueryEntity(_id);

            ddlParentId.SelectedValue = model.ParentID.ToString();
            txtSortId.Text = model.Sort.ToString();
            if (model.IsShow)
            {
                cbIsShow.Checked = true;
            }
            txtName.Text = model.TypeName;
            if (model.IsSystem)
            {
                ddlParentId.Enabled = false;
                txtName.ReadOnly = true;
            }
            txtRemark.Text = model.Remarks;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            try
            {
                NewsTypesEntity model = new NewsTypesEntity();
                NewsTypesBLL bll = new NewsTypesBLL();

                model.TypeName = txtName.Text.Trim();
                model.Sort = int.Parse(txtSortId.Text.Trim());
                model.IsShow = cbIsShow.Checked;
                model.IsSystem = false;
                model.Remarks = txtRemark.Text.Trim();
                model.ParentID = int.Parse(ddlParentId.SelectedValue);

                if (bll.InsertEntity(model) > 0)
                {
                    AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加栏目:" + model.TypeName); //记录日志
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
                NewsTypesBLL bll = new NewsTypesBLL();
                NewsTypesEntity model = bll.QueryEntity(_id);

                model.TypeName = txtName.Text.Trim();
                model.Sort = int.Parse(txtSortId.Text.Trim());
                model.IsShow = cbIsShow.Checked;
                model.Remarks = txtRemark.Text.Trim();
                model.ParentID = int.Parse(ddlParentId.SelectedValue);

                if (!model.IsSystem)
                {
                    int parentId = int.Parse(ddlParentId.SelectedValue);
                    //如果选择的父ID不是自己,则更改
                    if (parentId != model.TypeID)
                    {
                        model.ParentID = parentId;
                    }
                }

                if (bll.ModifyEntity(model) > 0)
                {
                    AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "修改栏目:" + model.TypeName); //记录日志
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
                ChkAdminLevel("newstypes_list", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("修改栏目成功！", "newstypes_list.aspx", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel("newstypes_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("添加栏目成功！", "newstypes_list.aspx", "parent.loadMenuTree");
            }
        }

    }
}