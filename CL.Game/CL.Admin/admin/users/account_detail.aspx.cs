using CL.Enum.Common;
using CL.Game.BLL;
using CL.Tools.Common;
using System;
using System.Web.UI;
using CL.Enum.Common.Type;

namespace CL.Admin.admin.users
{
    public partial class account_detail : UI.AdminPage
    {
        protected int tradeType = -1;
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected int userid = 0;
        protected string startTime = string.Empty;
        protected string endTime = string.Empty;
        protected long SumMoneyAdd = 0;
        protected long SumMoneySub = 0;
        protected long SumReward = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            userid = QPRequest.GetQueryInt("userid");
            this.tradeType = QPRequest.GetQueryIntDefaultNegative("tradeType");
            this.startTime = QPRequest.GetQueryString("startTime");
            if (string.IsNullOrEmpty(this.startTime))
                this.startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss");
            this.endTime = QPRequest.GetQueryString("endTime");
            if (string.IsNullOrEmpty(this.endTime))
                this.endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("user_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind(userid, tradeType, startTime, endTime);
            }
        }

        #region 数据绑定=================================
        private void RptBind(int _userid, int _tradeType, string _startTime, string _endTime)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            this.ddlTradeType.SelectedValue = _tradeType.ToString();
            this.txtStartTime.Text = _startTime;
            this.txtEndTime.Text = _endTime;
            this.rptList.DataSource = new UsersRecordBLL().QueryUserAccountDetail(_userid, _tradeType, _startTime, _endTime, this.pageSize, this.page, ref this.totalCount, ref SumMoneyAdd, ref SumMoneySub, ref SumReward);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("account_detail.aspx", "userid={0}&tradeType={1}&startTime={2}&endTime={3}&page={4}", _userid.ToString(), _tradeType.ToString(), _startTime, _endTime, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion


        #region 查找事件=============================
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("account_detail.aspx", "userid={0}&tradeType={1}&startTime={2}&endTime={3}", userid.ToString(), tradeType.ToString(), this.txtStartTime.Text, this.txtEndTime.Text));
        }
        #endregion


        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("account_detail_page_size", "QPcmsPage"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("account_detail_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("account_detail.aspx", "userid={0}&tradeType={1}&startTime={2}&endTime={3}", 
                this.userid.ToString(), tradeType.ToString(), this.startTime, this.endTime));
        }

        #region TradeType下拉Change事件=============================
        protected void ddlTradeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("account_detail.aspx", "userid={0}&tradeType={1}&startTime={2}&endTime={3}",
             this.userid.ToString(), ddlTradeType.SelectedValue, this.startTime, this.endTime));
        }
        #endregion

        #region 根据Enum获取对应描述=============================
        protected string SetTradeTypeName(int TradeType)
        {
            return Common.GetDescription((TradeType)TradeType);
        }
        #endregion
    }
}