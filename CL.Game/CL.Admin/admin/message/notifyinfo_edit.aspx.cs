using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.message
{
    public partial class notifyinfo_edit : UI.AdminPage
    {
        protected string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("notifyinfo_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                TemplateBind();
            }
        }

        #region 彩种类型
        private void TemplateBind()
        {
            TemplateConfigBLL bll = new TemplateConfigBLL();
            List<TemplateConfigEntity> list = bll.QueryEntitys();

            ddlTemplate.Items.Clear();
            ddlTemplate.Items.Add(new ListItem("请选择模板...", ""));
            foreach (TemplateConfigEntity item in list)
            {
                ddlTemplate.Items.Add(new ListItem(item.Title, item.ID.ToString()));
            }
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = false;

            //NotifyInfo model = new NotifyInfo();
            //BLL.NotifyInfoBll bll = new BLL.NotifyInfoBll();

            //model.Title = txtTitle.Text.Trim();
            //model.NotifyType = Convert.ToByte(ddlNotifyType.SelectedValue);
            //model.TargetType = Convert.ToByte(ddlTargetType.SelectedValue);
            //model.Content = txtContent.Text.Trim();
            //model.SenderID = Managerinfo.id;

            //string apiKey = siteConfig.baiduappkey;
            //string secretKey = siteConfig.baidusecretkey;
            //string url = siteConfig.baiduurl;

            //if (bll.Add(model, apiKey, secretKey, url))
            //{
            //    AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加消息:" + model.Title); //记录日志
            //}
            return result;
        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == CaileEnums.ActionEnum.Add.ToString())
            {
                ChkAdminLevel("notifyinfo_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", string.Empty);
                    return;
                }
                JscriptMsg("添加消息成功！", "notifyinfo_list.aspx");
            }
        }

    }
}