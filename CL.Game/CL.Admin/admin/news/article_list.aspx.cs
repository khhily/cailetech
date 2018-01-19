using CL.Enum.Common;
using CL.Game.BLL;
using CL.Tools.Common;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.news
{
    public partial class article_list : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string keywords = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = QPRequest.GetQueryString("keywords");

            this.pageSize = GetPageSize(100); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("article_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind(keywords);
            }
        }

        #region 数据绑定=================================
        private void RptBind(string _keywords)
        {
            txtKeywords.Text = _keywords;
            NewsBLL bll = new NewsBLL();
            this.rptList.DataSource = bll.QueryListByPage(_keywords, 0, this.pageSize, this.page, ref this.totalCount);
            this.rptList.DataBind();
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("article_list_page_size", "QPcmsPage"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt("article_list.aspx", "keywords={0}", txtKeywords.Text));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("article_list", CaileEnums.ActionEnum.Delete.ToString()); //检查权限
            int sucCount = 0;
            int errorCount = 0;
            NewsBLL bll = new NewsBLL();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
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
            AddAdminLog(CaileEnums.ActionEnum.Delete.ToString(), "删除文章" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("article_list.aspx", "keywords={0}", this.keywords));
        }

    }
}