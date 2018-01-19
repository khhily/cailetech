using CL.Game.BLL;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.report.finance
{
    public partial class withdrawreport : UI.AdminPage
    {
        protected static long TotalAmount = 0;
        protected static int PageIndex = 1;
        protected static int PageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartTime.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss");
                txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                BindData();
            }
        }
        private void BindData(int Page = 0)
        {
            if (Page == 0)
                PageIndex = QPRequest.GetQueryInt("PageIndex", 1);
            else
                PageIndex = Page;
            int PayOutStatus = Convert.ToInt32(ddlPayOutStatus.SelectedValue);
            long UserCode = 0;
            long.TryParse(txtUserCode.Text.Trim(), out UserCode);
            string UserName = txtUserName.Text.Trim();
            DateTime StartTime = DateTime.Now;
            DateTime EndTime = DateTime.Now;
            DateTime.TryParse(txtStartTime.Text.Trim(), out StartTime);
            DateTime.TryParse(txtEndTime.Text.Trim(), out EndTime);
            if ((DateTime.Compare(EndTime, StartTime)) != 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('开始时间必须小于结束时间')", true);
                return;
            }
            int TotalCount = 0;
            var Entitys = new UsersWithdrawBLL().QuertWithdrawReport(StartTime, EndTime, UserName, UserCode, (byte)PayOutStatus, PageIndex, PageSize, ref TotalAmount, ref TotalCount);
            this.rptList.DataSource = Entitys;
            this.rptList.DataBind();
            string pageUrl = Utils.CombUrlTxt("withdrawreport.aspx", "PageIndex={0}", "__id__");
            PageContent.InnerHtml = Utils.OutPageList(PageSize, PageIndex, TotalCount, pageUrl, 8);
        }
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            BindData(1);
        }

        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    PageIndex = _pagesize;
                    BindData();
                    Utils.WriteCookie("withdrawreport_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                    Response.Redirect(Utils.CombUrlTxt("withdrawreport.aspx", "PageIndex={0}", _pagesize.ToString()));
                }
            }
        }
    }
}