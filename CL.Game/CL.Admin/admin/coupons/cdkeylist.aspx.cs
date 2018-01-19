using CL.Coupons.BLL;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.coupons
{
    public partial class cdkeylist : UI.AdminPage
    {
        protected static int PageIndex = 1;
        protected static int PageSize = 10;
        protected static string Start_Time = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
        protected static string Expire_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartTime.Text = Start_Time;
                txtExpireTime.Text = Expire_Time;
                BindData();

            }
        }
        protected void BindData(int Page = 0)
        {
            if (Page == 0)
                PageIndex = QPRequest.GetQueryInt("PageIndex", 1);
            else
                PageIndex = Page;
            //用户名
            string UserName = txtUserName.Text.Trim();
            int totalCount = 0;
            DateTime StartTime = Convert.ToDateTime(txtStartTime.Text);
            DateTime EndTime = Convert.ToDateTime(txtExpireTime.Text);
            Start_Time = txtStartTime.Text;
            Expire_Time = txtExpireTime.Text;
            this.rptList.DataSource = new CouponsCDKeyBLL().QueryCouponsList(UserName, StartTime, EndTime, PageIndex, PageSize, ref totalCount);
            this.rptList.DataBind();

            string pageUrl = Utils.CombUrlTxt("cdkeylist.aspx", "PageIndex={0}", "__id__");
            PageContent.InnerHtml = Utils.OutPageList(PageSize, PageIndex, totalCount, pageUrl, 8);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            BindData(1);
        }
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    PageIndex = _pagesize;
                    BindData();
                    Utils.WriteCookie("cdkeylist_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                    Response.Redirect(Utils.CombUrlTxt("cdkeylist.aspx", "PageIndex={0}", _pagesize.ToString()));
                }
            }
        }
    }
}