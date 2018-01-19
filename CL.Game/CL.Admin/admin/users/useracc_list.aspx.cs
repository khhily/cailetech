using CL.Enum.Common;
using CL.Tools.Common;
using System;
using System.Web.UI;

namespace CL.Admin.admin.users
{
    public partial class useracc_list : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string keywords = string.Empty;
        protected int bingtype = 0;
        protected int Count = 0;
        protected int BingCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = QPRequest.GetQueryString("keywords");
            this.bingtype = QPRequest.GetQueryInt("bingtype");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("useracc_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind(keywords, bingtype, "ID desc");
            }
        }

        #region 数据绑定=================================
        private void RptBind(string _keywords, int _bingtype, string _order)
        {
            //this.page = QPRequest.GetQueryInt("page", 1);
            //txtKeywords.Text = _keywords;
            //ddlBingType.SelectedValue = _bingtype.ToString();

            //BLL.UserAccBll bll = new BLL.UserAccBll();
            //this.rptList.DataSource = bll.GetListByPage(_keywords, _bingtype, _order, this.pageSize, this.page, out this.totalCount);
            //this.rptList.DataBind();
            //bll.GetCount(out this.Count, out this.BingCount);

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("useracc_list.aspx", "keywords={0}&bingtype={1}&page={2}", this.keywords, _bingtype.ToString(), "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("useracc_list_page_size", "QPcmsPage"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt("useracc_list.aspx", "keywords={0}", txtKeywords.Text));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("useracc_list_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("useracc_list.aspx", "keywords={0}&bingtype={1}", this.keywords, bingtype.ToString()));
        }

        protected void ddlBingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("useracc_list.aspx", "keywords={0}&bingtype={1}", this.keywords, ddlBingType.SelectedValue));
        }

    }
}