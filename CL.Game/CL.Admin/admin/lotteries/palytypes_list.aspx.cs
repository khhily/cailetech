﻿using CL.Enum.Common;
using CL.Game.BLL;
using CL.Tools;
using CL.Tools.Common;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.lotteries
{
    public partial class palytypes_list : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string keywords = string.Empty;
        protected int LotteryCode = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = QPRequest.GetQueryString("keywords");
            LotteryCode = QPRequest.GetQueryInt("LotteryCode");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("lotteries_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                RptBind(LotteryCode, keywords, "Sort asc");
            }
        }

        #region 数据绑定=================================
        private void RptBind(int _LotteryCode, string _strName, string _orderby)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;
            this.rptList.DataSource = new PlayTypesBLL().QueryListByPage(_LotteryCode, _strName, _orderby, this.pageSize, this.page, ref this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("palytypes_list.aspx", "keywords={0}&LotteryCode={1}&page={2}", this.keywords, LotteryCode.ToString(), "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("palytypes_page_size", "QPcmsPage"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt("palytypes_list.aspx", "keywords={0}&LotteryCode={1}", txtKeywords.Text, LotteryCode.ToString()));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("palytypes_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("palytypes_list.aspx", "keywords={0}&LotteryCode={1}", this.keywords, LotteryCode.ToString()));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("lotteries_list", CaileEnums.ActionEnum.Delete.ToString()); //检查权限
            int sucCount = 0;
            int errorCount = 0;
            PlayTypesBLL bll = new PlayTypesBLL();
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
            AddAdminLog(CaileEnums.ActionEnum.Delete.ToString(), "删除玩法" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("palytypes_list.aspx", "keywords={0}&LotteryCode={1}", this.keywords, LotteryCode.ToString()));
        }
    }
}