using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Redis.BLL;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace CL.Admin.admin.message
{
    public partial class template_edit : UI.AdminPage
    {
        protected string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = QPRequest.GetQueryString("action");

            //如果是编辑则检查信息是否存在
            if (_action == CaileEnums.ActionEnum.Edit.ToString())
            {
                this.action = _action;//修改类型
                this.id = QPRequest.GetQueryInt("id");
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new TemplateConfigBLL().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("template_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }

        #region 赋值操作
        private void ShowInfo(int _id)
        {
            TemplateConfigBLL bll = new TemplateConfigBLL();
            TemplateConfigEntity model = bll.QueryEntity(_id);

            txtTitle.Text = model.Title;
            ddlTemplateType.SelectedValue = model.TemplateType.ToString();
            txtContent.Text = model.TemplateContent;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = false;

            TemplateConfigEntity model = new TemplateConfigEntity();
            TemplateConfigBLL bll = new TemplateConfigBLL();

            model.Title = txtTitle.Text.Trim();
            model.TemplateType = Convert.ToByte(ddlTemplateType.SelectedValue);
            model.TemplateContent = txtContent.Text.Trim();
            model.AdminID = Managerinfo.id;

            if (bll.InsertEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加模板:" + model.Title); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;

            TemplateConfigBLL bll = new TemplateConfigBLL();
            TemplateConfigEntity model = bll.QueryEntity(_id);

            model.Title = txtTitle.Text.Trim();
            model.TemplateType = Convert.ToByte(ddlTemplateType.SelectedValue);
            model.TemplateContent = txtContent.Text.Trim();

            if (bll.ModifyEntity(model) > 0)
            {
                if (model.TemplateType == 5)
                    new BusinessRedis().TemplateConfigEntityRedis(model);
                AddAdminLog(CaileEnums.ActionEnum.Edit.ToString(), "修改模板:" + model.Title); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("template_list", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", string.Empty);
                    return;
                }

                JscriptMsg("修改模板成功！", "template_list.aspx");
            }
            else //添加
            {
                ChkAdminLevel("template_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", string.Empty);
                    return;
                }
                JscriptMsg("添加模板成功！", "template_list.aspx");
            }
        }

    }
}