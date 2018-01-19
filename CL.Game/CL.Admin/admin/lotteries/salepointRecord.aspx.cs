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

namespace CL.Admin.admin.lotteries
{
    public partial class salepointRecord : UI.AdminPage
    {
        protected int TicketSource = 0;
        protected int LotteryCode = 0;
        protected string StartTime = string.Empty;
        protected string EndTime = string.Empty;
        protected int recordCount;
        protected int page;
        protected int pageSize;
        protected void Page_Load(object sender, EventArgs e)
        {
            TicketSource = QPRequest.GetQueryInt("TicketSource", -1);
            LotteryCode = QPRequest.GetQueryInt("LotteryCode");
            StartTime = QPRequest.GetQueryString("StartTime");
            if (string.IsNullOrEmpty(StartTime))
                StartTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss");
            EndTime = QPRequest.GetQueryString("EndTime");
            if (string.IsNullOrEmpty(EndTime))
                EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            pageSize = GetPageSize(10); //每页数量

            if (!Page.IsPostBack)
            {
                ChkAdminLevel("salepointRecord", CaileEnums.ActionEnum.View.ToString()); //检查权限
                LotteryBind();
                RptBind(TicketSource, LotteryCode, StartTime, EndTime);
            }
        }


        #region 数据绑定
        private void RptBind(int _ticketSource, int _lotteryCode, string _startTime, string _endTime)
        {
            page = QPRequest.GetQueryInt("page", 1);
            ddlTicketSource.SelectedValue = _ticketSource.ToString();
            ddlLotteryCode.SelectedValue = _lotteryCode.ToString();
            txtStartTime.Text = _startTime;
            txtEndTime.Text = _endTime;
            rptList.DataSource = new SalePointRecordBLL().QuerySalePointRecord(_ticketSource, _lotteryCode, _startTime, _endTime, pageSize, page, ref recordCount);
            rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("salepointRecord.aspx", "TicketSource={0}&LotteryCode={1}&StartTime={2}&EndTime={3}&page={4}", _ticketSource.ToString(), _lotteryCode.ToString(), _startTime, _endTime, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, recordCount, pageUrl, 8);
        }
        #endregion


        #region 出票商下拉切换事件=========================
        protected void ddlTicketSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("salepointRecord.aspx", "TicketSource={0}&LotteryCode={1}&StartTime={2}&EndTime={3}",
                ddlTicketSource.SelectedValue, LotteryCode.ToString(), StartTime, EndTime));
        }
        #endregion


        #region 彩种种类下拉切换事件=========================
        protected void ddlLotteryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("salepointRecord.aspx", "TicketSource={0}&LotteryCode={1}&StartTime={2}&EndTime={3}",
             TicketSource.ToString(), ddlLotteryCode.SelectedValue, StartTime, EndTime));
        }
        #endregion


        #region 查询事件=========================
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("salepointRecord.aspx", "TicketSource={0}&LotteryCode={1}&StartTime={2}&EndTime={3}",
                TicketSource.ToString(), LotteryCode.ToString(), txtStartTime.Text, txtEndTime.Text));
        }
        #endregion


        #region 通过LotteryCode获取对应LotteryName=========================
        protected string SetLotteryName(int LotteryCode)
        {
            return Common.GetDescription((LotteryInfo)LotteryCode);
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
            if (int.TryParse(Utils.GetCookie("salepointRecord_page_size", "QPcmsPage"), out _pagesize))
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
                    Utils.WriteCookie("salepointRecord_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("salepointRecord.aspx", "TicketSource={0}&LotteryCode={1}&StartTime={2}&EndTime={3}", TicketSource.ToString(), LotteryCode.ToString(), StartTime, EndTime));
        }
        #endregion


        #region 转换SalesRebate为HTML=========================
        protected string GetHTML(object SalesRebate)
        {
            if (SalesRebate == null)
                return "";
            string html = string.Empty;
            string[] SalesRebateArr = SalesRebate.ToString().Split(',');
            foreach (string item in SalesRebateArr)
            {
                html += "<div>销售阶梯：大于" + item.Split('#')[0] + "元 ";
                html += "销售点位：" + item.Split('#')[1] + "%</div>";
            }
            return html;
        }
        #endregion
    }
}