using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.BLL.View;
using CL.Game.Entity;
using CL.Tools;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CL.Enum.Common.Type;

namespace CL.Admin.admin.lotteries
{
    public partial class chase_List : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;

        protected int iType = 0;
        protected string IsuseName = string.Empty;
        protected string UserName = string.Empty;
        protected string SchemeNumber = string.Empty;
        protected int LotteryCode = 0;
        protected int ChaseStatus = 0;
        protected string StartTime = string.Empty;
        protected string EndTime = string.Empty;
        protected long SumMoney = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            iType = QPRequest.GetQueryInt("iType");
            IsuseName = QPRequest.GetQueryString("IsuseName");
            UserName = QPRequest.GetQueryString("UserName");
            SchemeNumber = QPRequest.GetQueryString("SchemeNumber");
            LotteryCode = QPRequest.GetQueryInt("LotteryCode");
            ChaseStatus = QPRequest.GetQueryInt("ChaseStatus");

            StartTime = QPRequest.GetQueryString("StartTime");
            if (string.IsNullOrEmpty(StartTime))
                StartTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss");
            EndTime = QPRequest.GetQueryString("EndTime");
            if (string.IsNullOrEmpty(EndTime))
                EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("chase_List", CaileEnums.ActionEnum.View.ToString()); //检查权限
                LotteryBind();
                RptBind(iType, IsuseName, UserName, SchemeNumber, LotteryCode, ChaseStatus, StartTime, EndTime);
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
        private void RptBind(int _iType, string _IsuseName, string _UserName, string _SchemeNumber, int _LotteryCode, int _ChaseStatus, string _StartTime, string _EndTime)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            ddlType.SelectedValue = _iType.ToString();
            ddlLotteryCode.SelectedValue = _LotteryCode.ToString();
            txtIsuseName.Text = _IsuseName;
            txtUserName.Text = _UserName;
            ddlChaseStatus.SelectedValue = _ChaseStatus.ToString();
            txtSchemeNumber.Text = _SchemeNumber;
            txtStartTime.Text = _StartTime;
            txtEndTime.Text = _EndTime;

            udv_ChaseListBLL bll = new udv_ChaseListBLL();
            this.rptList.DataSource = bll.QueryListByPage(_iType, _IsuseName, _UserName, _SchemeNumber, _LotteryCode, _ChaseStatus, _StartTime, _EndTime, this.pageSize, this.page, ref this.totalCount, ref SumMoney);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("chase_List.aspx", "iType={0}&IsuseName={1}&UserName={2}&SchemeNumber={3}&LotteryCode={4}&ChaseStatus={5}&StartTime={6}&EndTime={7}&page={8}",
                _iType.ToString(), _IsuseName, _UserName, _SchemeNumber, _LotteryCode.ToString(), _ChaseStatus.ToString(), _StartTime.ToString(), _EndTime.ToString(), "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("chase_page_size", "QPcmsPage"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt("chase_List.aspx", "iType={0}&IsuseName={1}&UserName={2}&SchemeNumber={3}&LotteryCode={4}&ChaseStatus={5}&StartTime={6}&EndTime={7}",
                ddlType.SelectedValue, txtIsuseName.Text, txtUserName.Text, txtSchemeNumber.Text, ddlLotteryCode.SelectedValue, ddlChaseStatus.SelectedValue, txtStartTime.Text, txtEndTime.Text));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("chase_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("chase_List.aspx", "iType={0}&IsuseName={1}&UserName={2}&SchemeNumber={3}&LotteryCode={4}&ChaseStatus={5}&StartTime={6}&EndTime={7}",
                  iType.ToString(), IsuseName, UserName, SchemeNumber, LotteryCode.ToString(), ChaseStatus.ToString(), StartTime, EndTime));
        }


        //获取终止条件枚举中对应值或名的描述
        protected string SetStopTypeName(long StopTypeWhenWinMoney)
        {
            int isExisEnum = 1;
            Int32.TryParse(StopTypeWhenWinMoney.ToString(), out isExisEnum);
            if (!System.Enum.IsDefined(typeof(StopType), Convert.ToInt32(StopTypeWhenWinMoney)))
            {
                isExisEnum = 0;
            }
            if(isExisEnum == 0)
                return "超过" + StopTypeWhenWinMoney + "元奖金停止追加";
            else
                return Common.GetDescription((StopType)StopTypeWhenWinMoney);
        }
    }
}