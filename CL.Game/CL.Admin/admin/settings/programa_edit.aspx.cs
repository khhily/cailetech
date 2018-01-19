using CL.Enum.Common;
using CL.SystemInfo.BLL;
using CL.SystemInfo.Entity;
using CL.View.Entity.SystemInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.settings
{
    public partial class programa_edit : UI.AdminPage
    {
        private string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = Request.QueryString["action"];
            //如果是编辑则检查信息是否存在
            if (_action == CaileEnums.ActionEnum.Edit.ToString())
            {
                this.action = _action;//修改类型
                int.TryParse(Request.QueryString["id"], out this.id);
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new NavigationBLL().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                MenuTypeBind();
                ChkAdminLevel("programa_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }



        #region 新闻类型
        private void MenuTypeBind()
        {
            NavigationBLL bll = new NavigationBLL();
            List<udv_Navigation> list = bll.GetChilds();
            ddlMenuType.Items.Clear();
            ddlMenuType.Items.Add(new ListItem("请选择上级菜单..", ""));
            foreach (udv_Navigation item in list)
            {
                string Id = item.id.ToString();
                string Title = item.Title;
                this.ddlMenuType.Items.Add(new ListItem(Title, Id));
            }
        }
        #endregion

        #region 赋值操作
        private void ShowInfo(int _id)
        {
            NavigationBLL bll = new NavigationBLL();
            NavigationEntity model = bll.QueryEntity(_id);
            txtTitle.Text = model.Title;
            string parentid = Convert.ToString(model.ParentID);
            for (int i = 0; i < this.ddlMenuType.Items.Count; i++)
            {
                if (this.ddlMenuType.Items[i].Value == parentid)
                {
                    this.ddlMenuType.Items[i].Selected = true;
                }
            }
            txtName.Text = model.Name;
            txtRemark.Text = model.Remark;
            txtSortID.Text = Convert.ToString(model.SortID);
            txtLinkUrl.Text = model.LinkUrl;
            ddlIsLock.SelectedValue = model.IsLock.ToString();
            txtRemark.Text = model.Remark;
            txtActionType.Text = model.ActionType;
        }
        #endregion






        #region 增加操作=================================
        private bool DoAdd()
        {
            NavigationEntity model = new NavigationEntity();
            NavigationBLL bll = new NavigationBLL();
            model.Title = txtTitle.Text.Trim();
            string parentid = ddlMenuType.SelectedValue;
            model.ParentID = Convert.ToInt32(parentid);
            string name = txtName.Text.Trim();
            if (bll.Exists(name))
            {
                JscriptMsg("导航名称已存在！", "programa_edit.aspx");
                return false;
            }
            model.Name = name;
            model.LinkUrl = txtLinkUrl.Text.Trim();
            model.SortID =int.Parse(txtSortID.Text.Trim());
            model.Remark = txtRemark.Text.Trim();
            model.ActionType = txtActionType.Text.Trim();
            string lockvalue = ddlIsLock.SelectedValue.Trim() == "False" ? "0" : "1";
            model.IsLock = Convert.ToByte(lockvalue);
            bool result = false;
            if (bll.InsertEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加栏目:" + model.Title); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            NavigationBLL bll = new NavigationBLL();
            NavigationEntity model = bll.QueryEntity(_id);
            model.ParentID = Convert.ToInt32(ddlMenuType.SelectedValue);
            string name = txtName.Text.Trim();
            if (!name.Equals(model.Name)&&bll.Exists(name))
            {
                JscriptMsg("导航名称已存在！", "programa_edit.aspx");
                return false;
            }
            model.Name = name;
            model.Title = txtTitle.Text.Trim();
            model.LinkUrl = txtLinkUrl.Text.Trim();
            model.SortID = Convert.ToInt32(txtSortID.Text.Trim());
            model.IsLock = Convert.ToByte(ddlIsLock.SelectedValue.Trim() == "False" ? 0 : 1);
            model.ActionType = txtActionType.Text.Trim();
            model.Remark = txtRemark.Text.Trim();
            if (bll.ModifyEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Edit.ToString(), "修改栏目:" + model.Title); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("programa_list", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", string.Empty);
                    return;
                }
                JscriptMsg("修改栏目成功！", "programa_list.aspx");
            }
            else //添加
            {
                ChkAdminLevel("programa_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", string.Empty);
                    return;
                }
                JscriptMsg("添加栏目成功！", "programa_list.aspx");
            }
        }
    }
}