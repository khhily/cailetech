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
    public partial class audit : UI.AdminPage
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
                ChkAdminLevel("audit", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind(PayOutStatus.ApplyFor, keywords, "CreateTime desc,PayOutID desc");
            }
        }
        #region 数据绑定=================================
        private void RptBind(PayOutStatus ApplyFor, string _strName, string _orderby)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;
            this.rptList.DataSource = new udv_UsersWithdrawBLL().QueryListPageByFullName(this.page, this.pageSize, (int)ApplyFor, _strName, ref this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("audit.aspx", "keywords={0}&page={1}", this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion
        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("audit_page_size", "QPcmsPage"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt("audit.aspx", "keywords={0}", txtKeywords.Text));
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
                    Utils.WriteCookie("audit_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("audit.aspx", "keywords={0}", this.keywords));
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
        /// <summary>
        /// 审核(批量处理)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAudit_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("audit", CaileEnums.ActionEnum.Delete.ToString()); //检查权限\
            List<long> Ids = new List<long>();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                long id = Convert.ToInt64(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                    Ids.Add(id);
            }
            var result = new UsersWithdrawBLL().AuditPayOutStatus(Ids, (int)PayOutStatus.DealWith, this.GetAdminInfo().id);
            if (result)
                JscriptMsg("审核完成", Utils.CombUrlTxt("audit.aspx", "keywords={0}", this.keywords));
            else
                JscriptMsg("审核失败,请联系管理员", Utils.CombUrlTxt("audit.aspx", "keywords={0}", this.keywords));

        }
    }
}