using CL.Enum.Common;
using CL.Game.BLL.View;
using CL.Tools.Common;
using System;
using System.Web.UI;

namespace CL.Admin.admin.withdraw
{
    public partial class refund_list : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string keywords = string.Empty;
        protected int itype = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = QPRequest.GetQueryString("keywords");
            this.itype = string.IsNullOrEmpty(QPRequest.GetQueryString("type")) ? -1 : QPRequest.GetQueryInt("type");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("refund_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind(keywords, itype);
            }
        }

        #region 数据绑定=================================
        private void RptBind(string _keywords, int _type)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            txtKeywords.Text = _keywords;
            ddlType.SelectedValue = _type.ToString();

            this.rptList.DataSource = new udv_UserPayReRefundBLL().QueryListByPage(_keywords, _type, this.pageSize, this.page, ref this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("refund_list.aspx", "keywords={0}&type={1}&page={2}", this.keywords, _type.ToString(), "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("refund_list_page_size", "QPcmsPage"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt("refund_list.aspx", "keywords={0}", txtKeywords.Text));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("refund_list_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("refund_list.aspx", "keywords={0}&type={1}", this.keywords, itype.ToString()));
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("refund_list.aspx?keywords={0}&type={1}", this.keywords, ddlType.SelectedValue));
        }

        protected string OutputOperat(int iStatus, long ReID)
        {
            if (iStatus == 0)
                return "<a href=\"#\" class='Refund' data-value=" + ReID + ">查询退款到帐</a>";
            return string.Empty;
        }

    }
}