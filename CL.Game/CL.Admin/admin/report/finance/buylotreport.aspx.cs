using CL.Enum.Common;
using CL.Enum.Common.Lottery;
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
    public partial class buylotreport : UI.AdminPage
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
            long UserCode = 0;
            long.TryParse(txtUserCode.Text.Trim(), out UserCode);
            string UserName = txtUserName.Text.Trim();
            int LotteryCode = Convert.ToInt32(ddlLottery.SelectedValue.Trim());
            int SchemeStatus = Convert.ToInt32(ddlSchemeStatus.SelectedValue.Trim());
            string SchemeNumber = txtSchemeNumber.Text.Trim();
            int PrintOutType = Convert.ToInt32(ddlPrintOutType.SelectedValue.Trim());
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
            var Entitys = new SchemesBLL().QueryBuyLotReport(StartTime, EndTime, UserCode, UserName, LotteryCode, SchemeStatus, SchemeNumber, PrintOutType, PageIndex, PageSize, ref TotalAmount, ref TotalCount);
            this.rptList.DataSource = Entitys;
            this.rptList.DataBind();
            string pageUrl = Utils.CombUrlTxt("buylotreport.aspx", "PageIndex={0}", "__id__");
            PageContent.InnerHtml = Utils.OutPageList(PageSize, PageIndex, TotalCount, pageUrl, 8);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            BindData(1);
        }

        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    PageIndex = _pagesize;
                    BindData();
                    Utils.WriteCookie("buylotreport_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                    Response.Redirect(Utils.CombUrlTxt("buylotreport.aspx", "PageIndex={0}", _pagesize.ToString()));
                }
            }
        }
        /// <summary>
        /// 订单状态 0.待付款，2.订单过期，4 下单成功，6.出票成功，8.部分出票成功，10.下单失败（限号），
        /// 12.订单撤销，14.中奖，15.派奖中，16.派奖完成，18.不中奖完成，19.追号进行中，20.追号完成
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string SetStatus(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            switch (Convert.ToInt32(obj))
            {
                case 0: rec = "待付款"; break;
                case 2: rec = "订单过期"; break;
                case 4: rec = "下单成功"; break;
                case 6: rec = "出票成功"; break;
                case 8: rec = "部分出票成功"; break;
                case 10: rec = "下单失败（限号）"; break;
                case 12: rec = "订单撤销"; break;
                case 14: rec = "中奖"; break;
                case 15: rec = "派奖中"; break;
                case 16: rec = "派奖完成"; break;
                case 18: rec = "未中奖"; break;
                case 19: rec = "追号中"; break;
                case 20: rec = "追号完成"; break;
                default: rec = "--"; break;
            }
            return rec;
        }
        /// <summary>
        /// 出票方式 1本地出票 2华阳电子票
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string SetOutType(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            switch (Convert.ToInt32(obj))
            {
                case 1: rec = "本地出票"; break;
                case 2: rec = "华阳电子票"; break;
                default: rec = "--"; break;
            }
            return rec;
        }
        /// <summary>
        /// 购买类型 0.代购 1.追号 2.跟单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string SetBuyType(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            switch (Convert.ToInt32(obj))
            {
                case 0: rec = "代购"; break;
                case 1: rec = "追号"; break;
                case 2: rec = "跟单"; break;
                default: rec = "--"; break;
            }
            return rec;
        }
        protected string SetLottery(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            rec = Common.GetDescription((LotteryInfo)Convert.ToInt32(obj));
            return rec;
        }
    }
}