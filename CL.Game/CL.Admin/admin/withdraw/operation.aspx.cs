using CL.Enum.Common;
using CL.Enum.Common.Status;
using CL.Game.BLL;
using CL.Game.BLL.View;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.withdraw
{
    public partial class operation : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string keywords = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = QPRequest.GetQueryString("keywords");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("operation", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind(PayOutStatus.DealWith, keywords, "CreateTime desc,PayOutID desc");
            }
        }
        #region 数据绑定=================================
        private void RptBind(PayOutStatus DealWith, string _strName, string _orderby)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;
            this.rptList.DataSource = new udv_UsersWithdrawBLL().QueryListPageByFullName(this.page, this.pageSize, (int)DealWith, _strName, ref this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("operation.aspx", "keywords={0}&page={1}", this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion
        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("operation_page_size", "QPcmsPage"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion
        /// <summary>
        /// 关健字查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("operation.aspx", "keywords={0}", txtKeywords.Text));
        }
        /// <summary>
        /// 设置分页数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("operation_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("operation.aspx", "keywords={0}", this.keywords));
        }
        /// <summary>
        /// 获取银行卡所属银行
        /// </summary>
        /// <param name="BankType">类型</param>
        /// <returns></returns>
        public string GetEnumValue(int BankType)
        {
            try
            {
                return Common.GetDescription((CL.Enum.Common.Type.BankType)BankType);
            }
            catch
            {
                return "未知";
            }
        }
        public string GetAmount(long Amount)
        {
            return string.Format("{0:C3}", Amount * 0.01);
        }
        protected void btn_pay_Command(object sender, CommandEventArgs e)
        {
            bool result = false;
            long PayOutID = Convert.ToInt64(e.CommandArgument);
            string CommandName = Convert.ToString(e.CommandName).Trim();
            switch (CommandName.ToLower())
            {
                case "payout":
                    //提现
                    var id = GetAdminInfo().id;
                    result = new UsersWithdrawBLL().AuditPayOutSuccess(PayOutID, id);
                    if (result)
                        JscriptMsg("提现完成", Utils.CombUrlTxt("operation.aspx", "keywords={0}", this.keywords));
                    else
                    {
                        result = new UsersWithdrawBLL().AuditPayOutFailure(PayOutID, id, string.Empty);
                        if (result)
                            JscriptMsg("提现失败", Utils.CombUrlTxt("operation.aspx", "keywords={0}", this.keywords));
                        else
                            JscriptMsg("提现失败,请联系管理员", Utils.CombUrlTxt("operation.aspx", "keywords={0}", this.keywords));
                    }
                    break;
                case "nopayout":
                    //拒绝提现
                    result = new UsersWithdrawBLL().AuditPayOutStatus(new List<long>() { PayOutID }, (int)PayOutStatus.Failure, this.GetAdminInfo().id);
                    if (result)
                        JscriptMsg("拒绝完成", Utils.CombUrlTxt("operation.aspx", "keywords={0}", this.keywords));
                    else
                        JscriptMsg("拒绝失败,请联系管理员", Utils.CombUrlTxt("operation.aspx", "keywords={0}", this.keywords));
                    break;
            }
        }
    }
}