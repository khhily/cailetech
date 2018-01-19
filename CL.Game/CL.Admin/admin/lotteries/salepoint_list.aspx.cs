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
    public partial class salepoint_list : UI.AdminPage
    {
        protected int TicketSource = 0;
        protected int LotteryCode = 0;
        protected int SalePointStatus = 0;
        protected int recordCount;
        protected int page;
        protected int pageSize;
        protected void Page_Load(object sender, EventArgs e)
        {
            TicketSource = QPRequest.GetQueryInt("TicketSource", -1);
            LotteryCode = QPRequest.GetQueryInt("LotteryCode");
            SalePointStatus = QPRequest.GetQueryInt("SalePointStatus", 0);
            pageSize = GetPageSize(10); //每页数量

            if (!Page.IsPostBack)
            {
                ChkAdminLevel("salepoint_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                LotteryBind();
                RptBind(TicketSource, LotteryCode, SalePointStatus);
            }
        }


        #region 数据绑定
        private void RptBind(int _ticketSource, int _lotteryCode, int _salePointStatus)
        {
            page = QPRequest.GetQueryInt("page", 1);
            ddlTicketSource.SelectedValue = _ticketSource.ToString();
            ddlLotteryCode.SelectedValue = _lotteryCode.ToString();
            ddlSalePointStatus.SelectedValue = _salePointStatus.ToString();
            rptList.DataSource = new SalePointBLL().QuerySalePointLst(_ticketSource, _lotteryCode, _salePointStatus, pageSize, page, ref recordCount);
            rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("salepoint_list.aspx", "TicketSource={0}&LotteryCode={1}&SalePointStatus={2}&page={3}", _ticketSource.ToString(), _lotteryCode.ToString(), _salePointStatus.ToString(), "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, recordCount, pageUrl, 8);
        }
        #endregion


        #region 设置分页变量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("salepoint_list_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("salepoint_list.aspx", "TicketSource={0}&LotteryCode={1}&SalePointStatus={2}", TicketSource.ToString(), LotteryCode.ToString(), SalePointStatus.ToString()));
        }
        #endregion


        #region 返回每页数量=========================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("salepoint_list_page_size", "QPcmsPage"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion


        #region 出票商下拉切换事件=========================
        protected void ddlTicketSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("salepoint_list.aspx", "TicketSource={0}&LotteryCode={1}&SalePointStatus={2}",
                ddlTicketSource.SelectedValue, LotteryCode.ToString(), SalePointStatus.ToString()));
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


        #region 彩种种类下拉切换事件=========================
        protected void ddlLotteryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("salepoint_list.aspx", "TicketSource={0}&LotteryCode={1}&SalePointStatus={2}",
             TicketSource.ToString(), ddlLotteryCode.SelectedValue, SalePointStatus.ToString()));
        }
        #endregion


        #region 状态下拉切换事件=========================
        protected void ddlSalePointStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("salepoint_list.aspx", "TicketSource={0}&LotteryCode={1}&SalePointStatus={2}",
                TicketSource.ToString(), LotteryCode.ToString(), ddlSalePointStatus.SelectedValue));
        }
        #endregion


        #region 查询事件=========================
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("salepoint_list.aspx", "TicketSource={0}&LotteryCode={1}&SalePointStatus={2}",
                TicketSource.ToString(), LotteryCode.ToString(), SalePointStatus.ToString()));
        }
        #endregion


        #region 通过LotteryCode获取对应LotteryName=========================
        protected string SetLotteryName(int LotteryCode)
        {
            return Common.GetDescription((LotteryInfo)LotteryCode);
        }
        #endregion


        #region 通过SalePointStatus获取对应SalePointStatusName=========================
        protected string SetSalePointStatusName(int SalePointStatus)
        {
            return Common.GetDescription((SalePointStatus)SalePointStatus);
        }
        #endregion


        #region 转换为金额显示
        protected string ConvertMoney(object val)
        {
            decimal money = 0;
            decimal.TryParse(val.ToString(), out money);
            return (money > 0 ? (money / 100) : 0) + " 元";
        }
        #endregion
    }
}