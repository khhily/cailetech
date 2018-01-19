using CL.Enum.Common;
using CL.Enum.Common.Lottery;
using CL.Coupons.BLL;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.coupons
{
    public partial class couponslist : UI.AdminPage
    {
        protected static int PageIndex = 1;
        protected static int PageSize = 10;
        protected static string Start_Time = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
        protected static string Expire_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        protected static long RecordFaceValue = 0;
        protected static long RecordEmploy = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartTime.Text = Start_Time;
                txtExpireTime.Text = Expire_Time;
                BindData();

            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            BindData(1);
        }
        protected void BindData(int Page = 0)
        {
            if (Page == 0)
                PageIndex = QPRequest.GetQueryInt("PageIndex", 1);
            else
                PageIndex = Page;
            //彩种
            int LotteryCode = Convert.ToInt32(ddlLotteryCode.SelectedValue);
            //彩券状态：0未发放，1已发放，2未使用，3已使用
            int CouponsStatus = Convert.ToInt32(ddlState.SelectedValue);
            //彩券类型：0固定时间段，1固定时长，2满减，3永不过期
            int CouponsType = Convert.ToInt32(ddlType.SelectedValue);
            //彩券来源：1 平台活动发放，2 平台赠送，3 任务获得,游戏获得(例如轮盘游戏,玩游戏时有一定机率获得彩券)，4 线下推广二维码，5会员赠送
            int CouponsSource = Convert.ToInt32(ddlSource.SelectedValue);
            //用户名
            string UserName = txtUserName.Text.Trim();
            int totalCount = 0;
            DateTime StartTime = Convert.ToDateTime(txtStartTime.Text);
            DateTime EndTime = Convert.ToDateTime(txtExpireTime.Text);
            Start_Time = txtStartTime.Text;
            Expire_Time = txtExpireTime.Text;
            this.rptList.DataSource = new CouponsBLL().QueryCouponsList(UserName, LotteryCode, CouponsStatus, CouponsType, CouponsSource, StartTime, EndTime, PageIndex, PageSize, ref totalCount,ref RecordFaceValue,ref RecordEmploy);
            this.rptList.DataBind();

            string pageUrl = Utils.CombUrlTxt("couponslist.aspx", "PageIndex={0}", "__id__");
            PageContent.InnerHtml = Utils.OutPageList(PageSize, PageIndex, totalCount, pageUrl, 8);
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
                    Utils.WriteCookie("couponslist_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                    Response.Redirect(Utils.CombUrlTxt("couponslist.aspx", "PageIndex={0}", _pagesize.ToString()));
                }
            }
        }
        protected string SetLotteryCode(int LotteryCode)
        {
            string rec = string.Empty;
            if (LotteryCode == 0)
                rec = "全场通用";
            else
                rec = Common.GetDescription((LotteryInfo)LotteryCode);
            return rec;
        }
        protected string SetCouponsStatus(int CouponsStatus)
        {
            //彩券状态：0未发放，1已发放，2未使用，3已使用
            string rec = string.Empty;
            switch (CouponsStatus)
            {
                case 0:
                    rec = "未发放";
                    break;
                case 1:
                    rec = "已发放";
                    break;
                case 2:
                    rec = "未使用";
                    break;
                case 3:
                    rec = "已使用";
                    break;
            }
            return rec;
        }
        protected string SetCouponsType(int CouponsType)
        {
            //彩券类型：0固定时间段，1固定时长，2满减，3永不过期
            string rec = string.Empty;
            switch (CouponsType)
            {
                case 0:
                    rec = "固定时间段";
                    break;
                case 1:
                    rec = "固定时长";
                    break;
                case 2:
                    rec = "满减";
                    break;
                case 3:
                    rec = "永不过期";
                    break;
            }
            return rec;
        }
        protected string SetCouponsSource(int CouponsSource)
        {
            //彩券来源：1 平台活动发放，2 平台赠送，3 任务获得,游戏获得(例如轮盘游戏,玩游戏时有一定机率获得彩券)，4 线下推广二维码，5会员赠送
            string rec = string.Empty;
            switch (CouponsSource)
            {
                case 1:
                    rec = "平台活动发放";
                    break;
                case 2:
                    rec = "平台赠送";
                    break;
                case 3:
                    rec = "任务获得,游戏获得";
                    break;
                case 4:
                    rec = "线下推广二维码,游戏获得";
                    break;
                case 5:
                    rec = "会员赠送";
                    break;
                default:
                    rec = "--";
                    break;
            }
            return rec;
        }
    }
}