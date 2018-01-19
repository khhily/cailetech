using CL.Enum.Common;
using CL.SystemInfo.BLL;
using CL.Tools.Common;
using System;
using System.Web.UI;

namespace CL.Admin.admin.manager
{
    public partial class managerlog : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;

        protected string keywords = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = QPRequest.GetQueryString("keywords").Replace("'", "");
            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("manager_log", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind(keywords, "AddTime desc,id desc");
            }
        }

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;
            ManagerLogBLL bll = new ManagerLogBLL();
            this.rptList.DataSource = bll.QueryListByPage(_strWhere, _orderby, this.pageSize, this.page, ref this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("managerlog.aspx", "keywords={0}&page={1}", this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("manager_page_size", "QPcmsPage"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt("managerlog.aspx", "keywords={0}", txtKeywords.Text));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("manager_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("managerlog.aspx", "keywords={0}", this.keywords));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("manager_log", CaileEnums.ActionEnum.Delete.ToString()); //检查权限
            ManagerLogBLL bll = new ManagerLogBLL();
            int sucCount = bll.DeletedDay(7);
            AddAdminLog(CaileEnums.ActionEnum.Delete.ToString(), "删除管理日志" + sucCount + "条"); //记录日志
            JscriptMsg("删除日志" + sucCount + "条", Utils.CombUrlTxt("managerlog.aspx", "keywords={0}", this.keywords));
        }
    }
}