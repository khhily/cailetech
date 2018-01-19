using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Redis.BLL;
using CL.Tools.Common;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.users
{
    public partial class user_list : UI.AdminPage
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
                ChkAdminLevel("user_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind(keywords);
            }
        }


        #region 数据绑定=================================
        private void RptBind(string _strName)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;

            this.rptList.DataSource = new UsersBLL().QueryListByPage(_strName, "", "", this.pageSize, this.page, ref this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("user_list.aspx", "keywords={0}&page={1}", this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("user_list_page_size", "QPcmsPage"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt("user_list.aspx", "keywords={0}", txtKeywords.Text));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("user_list_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("user_list.aspx", "keywords={0}", this.keywords));
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ChkAdminLevel("user_list", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
            long userid = Convert.ToInt64(((HiddenField)e.Item.FindControl("hidId")).Value);
            UsersBLL bll = new UsersBLL();
            UsersEntity model = bll.QueryEntityByUserCode(userid);
            switch (e.CommandName)
            {
                case "lbtnIsShow":
                    model.IsCanLogin = model.IsCanLogin ? false : true;
                    bll.ModifyEntity(model);

                    #region 更新Redis
                    var Redis_Entity = new SystemRedis().SignInByUserCodeRedis(userid);
                    Redis_Entity.IsCanLogin = model.IsCanLogin;
                    new SystemRedis().SignInSessionRedis(Redis_Entity);
                    #endregion
                    break;
            }
            this.RptBind(keywords);
        }

    }
}