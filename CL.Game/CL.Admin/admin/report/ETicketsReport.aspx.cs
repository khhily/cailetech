using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CL.Enum.Common.Status;
using CL.Enum.Common.Lottery;

namespace CL.Admin.admin.report
{
    public partial class ETicketsReport : UI.AdminPage
    {
        protected int MerchantCode = 0;
        protected int LotteryCode = 0;
        protected int OutTicketStauts = 0;
        protected string StartTime = string.Empty;
        protected string EndTime = string.Empty;
        protected int recordCount;
        protected long SumMoney = 0;
        protected long SumBonus = 0;
        protected int page;
        protected int pageSize;
        protected void Page_Load(object sender, EventArgs e)
        {
            MerchantCode = QPRequest.GetQueryInt("MerchantCode");
            LotteryCode = QPRequest.GetQueryInt("LotteryCode");
            OutTicketStauts = QPRequest.GetQueryInt("OutTicketStauts");
            StartTime = QPRequest.GetQueryString("StartTime");
            if (string.IsNullOrEmpty(StartTime))
                StartTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            EndTime = QPRequest.GetQueryString("EndTime");
            if (string.IsNullOrEmpty(EndTime))
                EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            pageSize = GetPageSize(10); //每页数量

            if (!Page.IsPostBack)
            {
                ChkAdminLevel("ETicketsReport", CaileEnums.ActionEnum.View.ToString()); //检查权限
                LotteryBind();
                RptBind(MerchantCode, LotteryCode, OutTicketStauts, StartTime, EndTime);
            }
        }


        #region 数据绑定
        private void RptBind(int _merchantCode, int _lotteryCode, int _outTicketStauts, string _startTime, string _endTime)
        {
            page = QPRequest.GetQueryInt("page", 1);
            ddlMerchantCode.SelectedValue = _merchantCode.ToString();
            ddlLotteryCode.SelectedValue = _lotteryCode.ToString();
            ddlOutTicketStauts.SelectedValue = _outTicketStauts.ToString();
            txtStartTime.Text = _startTime;
            txtEndTime.Text = _endTime;
            rptList.DataSource = new OutETicketsBLL().QueryOutETickets(_merchantCode, _lotteryCode, _outTicketStauts, _startTime + " 00:00:00", _endTime + " 23:59:59", pageSize, page, ref recordCount, ref SumMoney, ref SumBonus);
            rptList.DataBind();
            monrySP.InnerText = SumMoney.ToString();
            BonusSP.InnerText = SumBonus.ToString();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("salepointRecord.aspx", "MerchantCode={0}&LotteryCode={1}&OutTicketStauts={2}&StartTime={3}&EndTime={4}&page={5}", _merchantCode.ToString(), _lotteryCode.ToString(), _outTicketStauts.ToString(), _startTime, _endTime, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, recordCount, pageUrl, 8);
        }
        #endregion


        #region 查询事件=========================
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("ETicketsReport.aspx", "MerchantCode={0}&LotteryCode={1}&OutTicketStauts={2}&StartTime={3}&EndTime={4}",
                MerchantCode.ToString(), LotteryCode.ToString(), OutTicketStauts.ToString(), txtStartTime.Text, txtEndTime.Text));
        }
        #endregion


        #region 出票商下拉切换事件=========================
        protected void ddlMerchantCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("ETicketsReport.aspx", "MerchantCode={0}&LotteryCode={1}&OutTicketStauts={2}&StartTime={3}&EndTime={4}",
                ddlMerchantCode.SelectedValue, LotteryCode.ToString(), OutTicketStauts.ToString(), StartTime, EndTime));
        }
        #endregion


        #region 彩种种类下拉切换事件=========================
        protected void ddlLotteryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("ETicketsReport.aspx", "MerchantCode={0}&LotteryCode={1}&OutTicketStauts={2}&StartTime={3}&EndTime={4}",
             MerchantCode.ToString(), ddlLotteryCode.SelectedValue, OutTicketStauts.ToString(), StartTime, EndTime));
        }
        #endregion


        #region 出票状态下拉切换事件=========================
        protected void ddlOutTicketStauts_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("ETicketsReport.aspx", "MerchantCode={0}&LotteryCode={1}&OutTicketStauts={2}&StartTime={3}&EndTime={4}",
                MerchantCode.ToString(), LotteryCode.ToString(), ddlOutTicketStauts.SelectedValue, StartTime, EndTime));
        }
        #endregion


        #region 彩种类型=========================
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


        #region 返回每页数量=========================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("ETicketsReport_page_size", "QPcmsPage"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion


        #region 设置分页变量=========================
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("ETicketsReport_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("ETicketsReport.aspx", "MerchantCode={0}&LotteryCode={1}&OutTicketStauts={2}&StartTime={3}&EndTime={4}", MerchantCode.ToString(), LotteryCode.ToString(), OutTicketStauts.ToString(), StartTime, EndTime));
        }
        #endregion


        #region 通过LotteryCode获取对应LotteryName=========================
        protected string SetLotteryName(int LotteryCode)
        {
            return Common.GetDescription((LotteryInfo)LotteryCode);
        }
        #endregion
    }
}