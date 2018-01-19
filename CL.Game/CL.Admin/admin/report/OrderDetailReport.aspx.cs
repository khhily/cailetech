using CL.Admin.UI;
using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.BLL.View;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.report
{
    public partial class OrderDetailReport : AdminPage
    {
        protected int lotteryCode = 0;
        protected int schemeStatus = -1;
        protected string schemeNumber = string.Empty;
        protected string userName = string.Empty;
        protected string mobile = string.Empty;
        protected string startTime = string.Empty;
        protected string endTime = string.Empty;
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lotteryCode = QPRequest.GetQueryInt("lotteryCode");
            this.schemeStatus = QPRequest.GetQueryInt("schemeStatus");
            this.schemeNumber = QPRequest.GetQueryString("schemeNumber");
            this.userName = QPRequest.GetQueryString("userName");
            this.mobile = QPRequest.GetQueryString("mobile");
            this.startTime = QPRequest.GetQueryString("startTime");
            if (string.IsNullOrEmpty(this.startTime))
                this.startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            this.endTime = QPRequest.GetQueryString("endTime");
            if (string.IsNullOrEmpty(this.endTime))
                this.endTime = DateTime.Now.ToString("yyyy-MM-dd");
            this.pageSize = GetPageSize(20); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("orderdetail_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                LotteryBind();
                RptBind(this.lotteryCode, this.schemeStatus, this.schemeNumber, this.userName, this.mobile, this.startTime, this.endTime, "CreateTime desc");
            }
        }
        private void RptBind(int _lotteryCode, int _schemeStatus, string _schemeNumber, string _userName, string _mobile, string _startTime, string _endTime, string _orderBy)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            this.ddlLotteryCode.SelectedValue = _lotteryCode.ToString();
            this.ddlSchemeStatus.SelectedValue = _schemeStatus.ToString();
            this.txtSchemeNumber.Text = _schemeNumber;
            this.txtUserName.Text = _userName;
            this.txtMobile.Text = _mobile;
            this.txtStartTime.Text = _startTime;
            this.txtEndTime.Text = _endTime;

            this.rptList.DataSource = new udv_OrderDetailReportBLL().QueryModeListByPages(_lotteryCode, _schemeStatus, _schemeNumber, _userName, _mobile, _startTime + " 00:00:00", _endTime + " 23:59:59", _orderBy, this.page, this.pageSize, ref this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("orderdetailreport.aspx", "lotteryCode={0}&schemeStatus={1}&schemeNumber={2}&userName={3}&mobile={4}&startTime={5}&endTime={6}&page={7}", _lotteryCode.ToString(), _schemeStatus.ToString(), _schemeNumber, _userName, _mobile, _startTime, _endTime, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("orderdetailreport.aspx", "lotteryCode={0}&schemeStatus={1}&schemeNumber={2}&userName={3}&mobile={4}&startTime={5}&endTime={6}", this.lotteryCode.ToString(), this.schemeStatus.ToString(), this.txtSchemeNumber.Text, this.txtUserName.Text, this.txtMobile.Text, this.txtStartTime.Text, this.txtEndTime.Text));
        }
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("orderdetail_list_page_size", "QPcmsPage"), out _pagesize))
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
                    Utils.WriteCookie("orderdetail_list_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("orderdetailreport.aspx", "lotteryCode={0}&schemeStatus={1}&schemeNumber={2}&userName={3}&mobile={4}&startTime={5}&endTime={6}", this.lotteryCode.ToString(), this.schemeStatus.ToString(), this.schemeNumber, this.userName, this.mobile, this.startTime, this.endTime));
        }


        #region 彩种类型
        private void LotteryBind()
        {
            LotteriesBLL bll = new LotteriesBLL();
            List<LotteriesEntity> list = bll.QueryEntitys("");

            ddlLotteryCode.Items.Clear();
            ddlLotteryCode.Items.Add(new ListItem("请选择彩种..", ""));
            foreach (LotteriesEntity item in list)
            {
                ddlLotteryCode.Items.Add(new ListItem(item.LotteryName, item.LotteryCode.ToString()));
            }
        }
        #endregion


        protected void ddlLotteryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("orderdetailreport.aspx", "lotteryCode={0}&schemeStatus={1}&schemeNumber={2}&userName={3&mobile={4}}&startTime={5}&endTime={6}",
             this.ddlLotteryCode.SelectedValue, this.schemeStatus.ToString(), this.schemeNumber, this.userName, this.mobile, this.startTime, this.endTime));
        }

        protected void ddlSchemeStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("orderdetailreport.aspx", "lotteryCode={0}&schemeStatus={1}&schemeNumber={2}&userName={3}&mobile={4}&startTime={5}&endTime={6}",
               this.lotteryCode.ToString(), this.ddlSchemeStatus.SelectedValue, this.schemeNumber, this.userName, this.mobile, this.startTime, this.endTime));
        }


        #region 处理内容
        protected string ConvertMoney(object val)
        {
            decimal money = 0;
            decimal.TryParse(val.ToString(), out money);
            return (money > 0 ? (money / 100) : 0) + " 元";
        }
        protected string ConvertStatus(object val)
        {
            int OrderStatus = val == null ? 0 : Convert.ToInt32(val);
            string result = string.Empty;
            switch (OrderStatus)
            {
                case 0:
                    result = "待付款";
                    break;
                case 2:
                    result = "订单过期";
                    break;
                case 4:
                    result = "下单成功";
                    break;
                case 6:
                    result = "出票成功";
                    break;
                case 8:
                    result = "部分出票成功";
                    break;
                case 10:
                    result = "下单失败（限号）";
                    break;
                case 12:
                    result = "订单撤销";
                    break;
                case 14:
                    result = "中奖";
                    break;
                case 15:
                    result = "派奖中";
                    break;
                case 16:
                    result = "派奖完成";
                    break;
                case 18:
                    result = "不中奖完成";
                    break;
                case 19:
                    result = "追号进行中";
                    break;
                case 20:
                    result = "追号完成";
                    break;
            }
            return result;
        }
        protected static string ConvertBuyType(object val)
        {
            int BuyType = val == null ? 0 : Convert.ToInt32(val);
            string result = string.Empty;
            switch (BuyType)
            {
                case 0: result = "代购"; break;
                case 1: result = "追号"; break;
                case 2: result = "跟单"; break;
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