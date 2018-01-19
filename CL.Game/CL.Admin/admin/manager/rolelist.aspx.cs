using CL.Enum.Common;
using CL.SystemInfo.BLL;
using CL.Tools.Common;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.manager
{
    public partial class rolelist : UI.AdminPage
    {
        protected string keywords = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = QPRequest.GetQueryString("keywords");
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("manage_rrole", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind("RoleType>=" + Managerinfo.RoleType + CombSqlTxt(this.keywords));
            }
        }

        #region 数据绑定=================================
        private void RptBind(string _strWhere)
        {
            this.txtKeywords.Text = this.keywords;
            RosleBLL bll = new RosleBLL();
            this.rptList.DataSource = bll.GetList(_strWhere);
            this.rptList.DataBind();
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" and RoleName like '%" + _keywords + "%'");
            }

            return strTemp.ToString();
        }
        #endregion

        #region 返回角色类型名称=========================
        protected string GetTypeName(int role_type)
        {
            string str = "";
            switch (role_type)
            {
                case 1:
                    str = "超级用户";
                    break;
                default:
                    str = "系统用户";
                    break;
            }
            return str;
        }
        #endregion

        //查询操作
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("rolelist.aspx", "keywords={0}", txtKeywords.Text.Trim()));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("manager_role", CaileEnums.ActionEnum.Delete.ToString()); //检查权限
            int sucCount = 0; //成功数量
            int errorCount = 0; //失败数量
            RosleBLL bll = new RosleBLL();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    if (bll.DelEntity(id))
                    {
                        sucCount++;
                    }
                    else
                    {
                        errorCount++;
                    }
                }
            }
            AddAdminLog(CaileEnums.ActionEnum.Delete.ToString(), "删除管理角色" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("rolelist.aspx", "keywords={0}", this.keywords));
        }



    }
}