using CL.Enum.Common;
using CL.Game.BLL;
using CL.Tools.Common;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.message
{
    public partial class template_list : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string keywords = string.Empty;
        protected int templatetype = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = QPRequest.GetQueryString("keywords");
            this.templatetype = QPRequest.GetQueryInt("templatetype");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("template_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind(keywords, templatetype, "ID desc");
            }
        }

        #region 数据绑定=================================
        private void RptBind(string _keywords, int _templatetype, string _order)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            txtKeywords.Text = _keywords;
            ddlTemplateType.SelectedValue = _templatetype.ToString();
            TemplateConfigBLL bll = new TemplateConfigBLL();
            this.rptList.DataSource = bll.QueryListByPage(_keywords, _templatetype, _order, this.pageSize, this.page, ref this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("template_list.aspx", "keywords={0}&templatetype={1}&page={2}", this.keywords, _templatetype.ToString(), "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("template_list_page_size", "QPcmsPage"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion

        //关健字查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("template_list.aspx", "keywords={0}", txtKeywords.Text));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("template_list_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("template_list.aspx", "keywords={0}&templatetype={1}", this.keywords, ddlTemplateType.ToString()));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("template_list", CaileEnums.ActionEnum.Delete.ToString()); //检查权限
            int sucCount = 0;
            int errorCount = 0;
            TemplateConfigBLL bll = new TemplateConfigBLL();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                long id = Convert.ToInt64(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    if (bll.DelEntity(id))
                    {
                        sucCount += 1;
                    }
                    else
                    {
                        errorCount += 1;
                    }
                }
            }
            AddAdminLog(CaileEnums.ActionEnum.Delete.ToString(), "删除模板" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("template_list.aspx", "keywords={0}&templatetype={1}", this.keywords, templatetype.ToString()));
        }

        protected void ddlTemplateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("template_list.aspx", "keywords={0}&templatetype={1}", this.keywords, ddlTemplateType.SelectedValue));
        }


    }
}