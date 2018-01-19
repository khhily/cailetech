using CL.Enum.Common;
using CL.Game.BLL;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.news
{
    public partial class auditinglist : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RptBind();
            }
        }
        private void RptBind()
        {
            this.rptList.DataSource = new NewsBLL().QueryListByPage(this.pageSize, this.page, ref this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("auditinglist.aspx", "page={0}", "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }

        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("auditinglist_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
        }
    }
}