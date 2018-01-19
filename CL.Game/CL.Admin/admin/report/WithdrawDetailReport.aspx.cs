using CL.Admin.UI;
using CL.Enum.Common;
using CL.Game.BLL.View;
using CL.Tools.Common;
using System;
using System.Web.UI;

namespace CL.Admin.admin.report
{
    public partial class WithdrawDetailReport : AdminPage
    {
        protected int payOutStatus = -1;
        protected string userName = string.Empty;
        protected string mobile = string.Empty;
        protected string reservedPhone = string.Empty; 
        protected string startTime = string.Empty;
        protected string endTime = string.Empty;
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.userName = QPRequest.GetQueryString("userName");
            this.mobile = QPRequest.GetQueryString("mobile");
            this.reservedPhone = QPRequest.GetQueryString("reservedPhone");
            this.startTime = QPRequest.GetQueryString("startTime");
            if (string.IsNullOrEmpty(this.startTime))
                this.startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            this.endTime = QPRequest.GetQueryString("endTime");
            if (string.IsNullOrEmpty(this.endTime))
                this.endTime = DateTime.Now.ToString("yyyy-MM-dd");
            this.pageSize = GetPageSize(20); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("withdrawdetail_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind(this.payOutStatus, this.userName, this.mobile, this.reservedPhone, this.startTime, this.endTime, "CreateTime desc");
            }
        }
        private void RptBind(int _payOutStatus, string _userName, string _mobile, string _reservedPhone, string _startTime, string _endTime, string _orderBy)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            
            this.txtUserName.Text = _userName;
            this.txtMobile.Text = _mobile;
            this.txtReservedPhone.Text = _reservedPhone;
            this.txtStartTime.Text = _startTime;
            this.txtEndTime.Text = _endTime;

            this.rptList.DataSource = new udv_WithdrawDetailReportBLL().QueryModeListByPages(_payOutStatus, _userName, _mobile, _reservedPhone, _startTime + " 00:00:00", _endTime + " 23:59:59", _orderBy, this.page, this.pageSize, ref this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("withdrawdetailreport.aspx", "payOutStatus={0}&userName={1}&mobile={2}&reservedPhone={3}&startTime={4}&endTime={5}&page={6}", _payOutStatus.ToString(), _userName, _mobile, _reservedPhone, _startTime, _endTime, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("withdrawdetailreport.aspx", "payOutStatus={0}&userName={1}&mobile={2}&reservedPhone={3}&startTime={4}&endTime={5}", this.payOutStatus.ToString(), this.txtUserName.Text, this.txtMobile.Text, this.txtReservedPhone.Text, this.txtStartTime.Text, this.txtEndTime.Text));
        }
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("withdrawdetail_list_page_size", "QPcmsPage"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("Withdrawdetail_list_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("Withdrawdetailreport.aspx", "payOutStatus={0}&userName={1}&mobile={2}&reservedPhone={3}&startTime={4}&endTime={5}", this.payOutStatus.ToString(), this.txtUserName.Text, this.txtMobile.Text, this.txtReservedPhone.Text, this.txtStartTime.Text, this.txtEndTime.Text));
        }

        #region 处理内容
        protected string ConvertMoney(object val)
        {
            decimal money = 0;
            decimal.TryParse(val.ToString(), out money);
            return (money > 0 ? (money / 100) : 0) + " 元";
        }
        protected static string ConvertPayOutStatus(object val)
        {
            int BuyType = val == null ? 0 : Convert.ToInt32(val);
            string result = string.Empty;
            switch (BuyType)
            {
                case 0: result = "申请"; break;
                case 2: result = "处理中"; break;
                case 4: result = "处理完成"; break;
                case 6: result = "提现失败"; break;
            }
            return result;
        }
        protected static string IsTrueOrFalse(object val)
        {
            string result = string.Empty;
            switch (val.ToString().ToLower())
            {
                case "false": result = "否"; break;
                case "true": result = "是"; break;
                case "0": result = "否"; break;
                case "1": result = "是"; break;
            }
            return result;
        }
        #endregion
    }
}