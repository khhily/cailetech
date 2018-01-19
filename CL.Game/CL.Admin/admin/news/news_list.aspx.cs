using CL.Enum.Common;
using CL.Enum.Common.Lottery;
using CL.Enum.Common.Other;
using CL.Enum.Common.Status;
using CL.Game.BLL;
using CL.Tools;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.news
{
    public partial class news_list : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string keywords = string.Empty;
        protected int TypeID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = QPRequest.GetQueryString("keywords");
            this.TypeID = QPRequest.GetQueryInt("typeid");
            //if (TypeID == 0)
            //    TypeID = 0;

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("news_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                NewsTypeBind();
                RptBind(keywords, TypeID);
            }
        }

        #region 新闻类型
        private void NewsTypeBind()
        {
            NewsTypesBLL bll = new NewsTypesBLL();
            List<udv_NewsTypes> list = bll.QueryEntitys(0, 3);

            ddlNewsType.Items.Clear();
            ddlNewsType.Items.Add(new ListItem("请选择类型..", ""));
            foreach (udv_NewsTypes item in list)
            {
                string Id = item.TypeID.ToString();
                int ClassLayer = item.ClassLayer;
                string Title = item.TypeName;

                if (ClassLayer == 1)
                {
                    this.ddlNewsType.Items.Add(new ListItem(Title, Id));
                }
                else
                {
                    Title = "├ " + Title;
                    Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                    this.ddlNewsType.Items.Add(new ListItem(Title, Id));
                }
            }
        }
        #endregion

        #region 数据绑定=================================
        private void RptBind(string _keywords, int _TypeID)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            txtKeywords.Text = _keywords;
            ddlNewsType.SelectedValue = _TypeID.ToString();
            NewsBLL bll = new NewsBLL();
            this.rptList.DataSource = bll.QueryListByPage(_keywords, _TypeID, this.pageSize, this.page, ref this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("news_list.aspx", "keywords={0}&typeid={1}&page={2}", this.keywords, _TypeID.ToString(), "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("news_list_page_size", "QPcmsPage"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt("news_list.aspx", "keywords={0}", txtKeywords.Text));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("news_list_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("news_list.aspx", "keywords={0}&typeid={1}", this.keywords, TypeID.ToString()));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("news_list", CaileEnums.ActionEnum.Delete.ToString()); //检查权限
            int sucCount = 0;
            int errorCount = 0;
            NewsBLL bll = new NewsBLL();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    if (bll.DelEntity(id))
                    {
                        sucCount += 1;
                    }
                    else
                    {
                        errorCount += 1;
                    }
                }
            }
            AddAdminLog(CaileEnums.ActionEnum.Delete.ToString(), "删除新闻" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("news_list.aspx", "keywords={0}&typeid={1}", this.keywords, TypeID.ToString()));
        }

        protected void ddlNewsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("news_list.aspx", "keywords={0}&typeid={1}", this.keywords, ddlNewsType.SelectedValue));
        }

        public string SetLottery(object obj)
        {
            string Rec = string.Empty;
            if (obj == null)
                Rec = "--";
            else
                Rec = Common.GetDescription((LotteryInfo)Convert.ToInt32(obj));
            return Rec;
        }
        public string SetEquipment(object obj)
        {
            string Rec = string.Empty;
            if (obj == null)
                Rec = "--";
            else
                Rec = Common.GetDescription((EquipmentEnum)Convert.ToInt32(obj));
            return Rec;
        }
        public string SetStatus(object obj)
        {
            string Rec = string.Empty;
            if (obj == null)
                Rec = "--";
            else
                Rec = Common.GetDescription((AuditingStatusEnum)Convert.ToInt32(obj));
            return Rec;
        }

    }
}