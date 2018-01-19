using CL.Enum.Common;
using CL.Game.BLL;
using CL.Tools;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.users
{
    public partial class commission_details : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected int userid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            userid = QPRequest.GetQueryInt("userid");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("user_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind(userid);
            }
        }

        #region 数据绑定=================================
        private void RptBind(int _userid)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            
            //this.rptList.DataSource = new UsersBLL().GetCommendMemberList(_userid, this.pageSize, this.page, out this.totalCount);
            //this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("commission_details.aspx", "userid={0}&page={1}", userid.ToString(), "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("commission_details_page_size", "QPcmsPage"), out _pagesize))
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
                    Utils.WriteCookie("commission_details_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("commission_details.aspx", "userid={0}", userid.ToString()));
        }

    }
}