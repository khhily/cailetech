using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CL.Enum.Common.Lottery;

namespace CL.Admin.admin.lotteries
{
    public partial class isuses_list : UI.AdminPage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string keywords = string.Empty;
        protected int LotteryCode = 0;
        protected string LastIsuse = string.Empty;
        protected string sDate = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = QPRequest.GetQueryString("keywords");
            LotteryCode = QPRequest.GetQueryInt("LotteryCode");
            sDate = QPRequest.GetQueryString("sDate");
            if (string.IsNullOrEmpty(sDate))
                sDate = DateTime.Now.ToString("yyyy-MM-dd");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("lotteries_isuses", CaileEnums.ActionEnum.View.ToString()); //检查权限
                LotteryBind();
                RptBind(LotteryCode, sDate, keywords, "IsuseID desc");
            }
        }

        #region 彩种类型
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

        #region 数据绑定=================================
        private void RptBind(int _LotteryCode, string _date, string _strName, string _orderby)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;
            ddlLotteryCode.SelectedValue = LotteryCode.ToString();
            txtDate.Text = sDate;

            IsusesBLL bll = new IsusesBLL();
            this.rptList.DataSource = bll.QueryListByPage(_LotteryCode, _date, _strName, _orderby, this.pageSize, this.page, ref this.totalCount);
            this.rptList.DataBind();

            if (_LotteryCode > 0)
            {
                IsusesEntity model = bll.QueryEntitysLastIsues(_LotteryCode);
                if (model == null)
                {
                    LastIsuse = "此彩种还没有添加过任何期号";
                }
                else
                {
                    LastIsuse = "已添加的最后期号：" + model.IsuseName + ",开始时间：" + model.StartTime + "截止时间：" + model.EndTime;
                }
            }

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("isuses_list.aspx", "keywords={0}&LotteryCode={1}&sDate={2}&page={3}", this.keywords, LotteryCode.ToString(), sDate, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("isuses_page_size", "QPcmsPage"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt("isuses_list.aspx", "keywords={0}&LotteryCode={1}&sDate={2}", txtKeywords.Text, LotteryCode.ToString(), sDate));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("isuses_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("isuses_list.aspx", "keywords={0}&LotteryCode={1}&sDate={2}", this.keywords, LotteryCode.ToString(), sDate));
        }

        protected void ddlLotteryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("isuses_list.aspx", "keywords={0}&LotteryCode={1}&sDate={2}", "", ddlLotteryCode.SelectedValue, ""));
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            DateTime _date = DateTime.Now;
            if (DateTime.TryParse(txtDate.Text.Trim(), out _date))
            {
                Response.Redirect(Utils.CombUrlTxt("isuses_list.aspx", "keywords={0}&LotteryCode={1}&sDate={2}", this.keywords, LotteryCode.ToString(), _date.ToString("yyyy-MM-dd")));
            }
        }
        protected string SetLotteryName(int LotteryCode)
        {
            return Common.GetDescription((LotteryInfo)LotteryCode);
        }
    }
}