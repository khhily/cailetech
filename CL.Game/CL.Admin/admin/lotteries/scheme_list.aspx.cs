using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CL.Enum.Common.Status;

namespace CL.Admin.admin.lotteries
{
    public partial class scheme_list : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string keywords = string.Empty;

        protected int LotteryCode = 0;
        protected int iState = 0;
        protected int iWinState = 0;
        protected string StartTime = string.Empty;
        protected string EndTime = string.Empty;
        protected string UserName = string.Empty;
        protected long SumMoney = 0;
        protected long WinSumMoney = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = QPRequest.GetQueryString("keywords");
            LotteryCode = QPRequest.GetQueryInt("LotteryCode");
            iState = QPRequest.GetQueryInt("State");
            iWinState = QPRequest.GetQueryInt("WinState");
            UserName = QPRequest.GetQueryString("UserName");

            StartTime = QPRequest.GetQueryString("StartTime");
            if (string.IsNullOrEmpty(StartTime))
                StartTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss");
            EndTime = QPRequest.GetQueryString("EndTime");
            if (string.IsNullOrEmpty(EndTime))
                EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("scheme_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                LotteryBind();
                RptBind(LotteryCode, iState, iWinState, StartTime, EndTime, keywords, UserName, "CreateTime desc");
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
        private void RptBind(int _LotteryCode, int _iState, int _iWinState, string _StartTime, string _EndTime, string _SchemeNumber, string _UserName, string _orderby)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            txtKeywords.Text = _SchemeNumber;
            ddlLotteryCode.SelectedValue = LotteryCode.ToString();
            ddlState.SelectedValue = _iState.ToString();
            ddlWinState.SelectedValue = _iWinState.ToString();
            txtStartTime.Text = _StartTime;
            txtEndTime.Text = _EndTime;
            txtUserName.Text = _UserName;

            SchemesBLL bll = new SchemesBLL();
            this.rptList.DataSource = bll.QueryListByPage(_LotteryCode, _iState, _iWinState, _StartTime, _EndTime, "", _SchemeNumber, _UserName, _orderby, this.pageSize, this.page, ref this.totalCount, ref SumMoney, ref WinSumMoney);
            this.rptList.DataBind();


            //long.TryParse(Convert.ToString(SumMoney), out SumMoney);
            //long.TryParse(Convert.ToString(WinSumMoney), out WinSumMoney);

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("scheme_list.aspx", "keywords={0}&LotteryCode={1}&State={2}&WinState={3}&StartTime={4}&EndTime={5}&UserName={6}&page={7}",
                this.keywords, LotteryCode.ToString(), _iState.ToString(), _iWinState.ToString(), _StartTime, _EndTime, _UserName, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("scheme_page_size", "QPcmsPage"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt("scheme_list.aspx", "keywords={0}&LotteryCode={1}&State={2}&WinState={3}&StartTime={4}&EndTime={5}&UserName={6}",
                txtKeywords.Text, LotteryCode.ToString(), iState.ToString(), iWinState.ToString(), txtStartTime.Text, txtEndTime.Text, txtUserName.Text));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("scheme_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("scheme_list.aspx", "keywords={0}&LotteryCode={1}&State={2}&WinState={3}&StartTime={4}&EndTime={5}&UserName={6}",
                keywords, LotteryCode.ToString(), iState.ToString(), iWinState.ToString(), StartTime, EndTime, UserName));
        }

        protected void ddlLotteryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("scheme_list.aspx", "keywords={0}&LotteryCode={1}&State={2}&WinState={3}&StartTime={4}&EndTime={5}&UserName={6}",
             keywords, ddlLotteryCode.SelectedValue, iState.ToString(), iWinState.ToString(), StartTime, EndTime, UserName));
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("scheme_list.aspx", "keywords={0}&LotteryCode={1}&State={2}&WinState={3}&StartTime={4}&EndTime={5}&UserName={6}",
                keywords, LotteryCode.ToString(), ddlState.SelectedValue, iWinState.ToString(), StartTime, EndTime, UserName));
        }

        protected void ddlWinState_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("scheme_list.aspx", "keywords={0}&LotteryCode={1}&State={2}&WinState={3}&StartTime={4}&EndTime={5}&UserName={6}",
               keywords, LotteryCode.ToString(), iState.ToString(), ddlWinState.SelectedValue, StartTime, EndTime, UserName));
        }
        
        protected string SetSchemeStatusName(int SchemeStatus)
        {
            return Common.GetDescription((SchemeStatusEnum)SchemeStatus);
        }

        //protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    ChkAdminLevel("scheme_list", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
        //    long SchemeID = Convert.ToInt64(((HiddenField)e.Item.FindControl("hidId")).Value);
        //    switch (e.CommandName)
        //    {
        //        case "lbtnIsRevoke":
        //            string ReturnDescription = string.Empty;
        //            new SchemeETicketsBLL().QuashScheme(SchemeID, false, ref ReturnDescription);
        //            break;
        //    }
        //    Response.Redirect(Utils.CombUrlTxt("scheme_list.aspx", "keywords={0}&LotteryCode={1}&State={2}&WinState={3}&StartTime={4}&EndTime={5}&UserName={6}",
        //       keywords, LotteryCode.ToString(), iState.ToString(), iWinState.ToString(), StartTime, EndTime, UserName));
        //}

    }
}