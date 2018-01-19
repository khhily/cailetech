using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.lotteries
{
    public partial class winfind_list : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;

        protected string IsuseName = string.Empty;
        protected string SchemeNumber = string.Empty;
        protected int LotteryCode = 0;
        protected string UserName = string.Empty;
        protected string StartTime = string.Empty;
        protected string EndTime = string.Empty;
        protected long SumMoney = 0;
        protected long WinSumMoney = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            IsuseName = QPRequest.GetQueryString("IsuseName");
            UserName = QPRequest.GetQueryString("UserName");
            SchemeNumber = QPRequest.GetQueryString("SchemeNumber");
            LotteryCode = QPRequest.GetQueryInt("LotteryCode");

            StartTime = QPRequest.GetQueryString("StartTime");
            if (string.IsNullOrEmpty(StartTime))
                StartTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss");
            EndTime = QPRequest.GetQueryString("EndTime");
            if (string.IsNullOrEmpty(EndTime))
                EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("winfind_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                LotteryBind();
                RptBind(IsuseName, UserName, SchemeNumber, LotteryCode, StartTime, EndTime);
            }
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

        #region 数据绑定
        private void RptBind(string _IsuseName, string _UserName, string _SchemeNumber, int _LotteryCode, string _StartTime, string _EndTime)
        {
            this.page = QPRequest.GetQueryInt("page", 1);

            ddlLotteryCode.SelectedValue = _LotteryCode.ToString();
            txtIsuseName.Text = _IsuseName;
            txtUserName.Text = _UserName;
            txtSchemeNumber.Text = _SchemeNumber;
            txtStartTime.Text = _StartTime;
            txtEndTime.Text = _EndTime;

            SchemesBLL bll = new SchemesBLL();
            this.rptList.DataSource = bll.QueryListByPage(_LotteryCode, 0, 3, _StartTime, _EndTime, _IsuseName, _SchemeNumber, _UserName, "CreateTime desc", this.pageSize, this.page,
                ref this.totalCount, ref SumMoney, ref WinSumMoney);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("winfind_list.aspx", "IsuseName={0}&UserName={1}&SchemeNumber={2}&LotteryCode={3}&StartTime={4}&EndTime={5}&page={6}",
                _IsuseName, _UserName, _SchemeNumber, _LotteryCode.ToString(), _StartTime.ToString(), _EndTime.ToString(), "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("winfind_page_size", "QPcmsPage"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt("winfind_list.aspx", "IsuseName={0}&UserName={1}&SchemeNumber={2}&LotteryCode={3}&StartTime={4}&EndTime={5}",
                txtIsuseName.Text, txtUserName.Text, txtSchemeNumber.Text, ddlLotteryCode.SelectedValue, txtStartTime.Text, txtEndTime.Text));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("winfind_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("winfind_list.aspx", "IsuseName={0}&UserName={1}&SchemeNumber={2}&LotteryCode={3}&StartTime={4}&EndTime={5}",
                 IsuseName, UserName, SchemeNumber, LotteryCode.ToString(), StartTime, EndTime));
        }

        protected void ddlLotteryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("winfind_list.aspx", "IsuseName={0}&UserName={1}&SchemeNumber={2}&LotteryCode={3}&StartTime={4}&EndTime={5}",
              txtIsuseName.Text, txtUserName.Text, txtSchemeNumber.Text, ddlLotteryCode.SelectedValue, txtStartTime.Text, txtEndTime.Text));
        }
    }
}