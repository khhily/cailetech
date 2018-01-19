using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.BLL.View;
using CL.Game.Entity;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CL.Enum.Common.Status;
using CL.Enum.Common.Type;

namespace CL.Admin.admin.users
{
    public partial class userschemeChaseTask_edit : UI.AdminPage
    {
        private string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型
        protected int page;
        protected int pageSize;
        protected int totalCount;
        protected long SchemeID = 0;
        protected udv_SchemeChaseTask model = new udv_SchemeChaseTask();
        protected string LotteryName = string.Empty;
        protected int Multiple = 1;
        protected int BetNum = 1;
        protected string SchemeNumber = string.Empty;
        protected string BetNumber = string.Empty;
        protected string ModuleName = string.Empty;
        protected string PlayName = string.Empty;
        protected int BetType = 1;
        protected int IsuseCount = 1;
        protected string StartTime = string.Empty;
        protected string EndTime = string.Empty;
        protected long StopTypeWhenWinMoney = 1;
        protected string CreateTime = string.Empty;
        protected int SchemeStatus = -1;
        protected string LotteryNumber = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = QPRequest.GetQueryString("action");
            this.pageSize = GetPageSize(10); //每页数量
            if (!string.IsNullOrEmpty(_action) && _action == CaileEnums.ActionEnum.Edit.ToString())
            {
                this.action = CaileEnums.ActionEnum.Edit.ToString();//修改类型
                if (!long.TryParse(Request.QueryString["id"] as string, out this.SchemeID))
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new SchemesBLL().Exists(this.SchemeID))
                {
                    JscriptMsg("记录不存在或已被删除！", "back");
                    return;
                }
            }

            if (!Page.IsPostBack)
            {
                ChkAdminLevel("scheme_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                model = new udv_SchemeChaseTaskBLL().QueryEntity(SchemeID);
                if (model != null)
                {
                    LotteryName = model.LotteryName;
                    Multiple = model.Multiple;
                    BetNum = model.BetNum;
                    SchemeNumber = model.SchemeNumber;
                    BetNumber = model.BetNumber;
                    ModuleName = model.ModuleName;
                    PlayName = model.PlayName;
                    BetType = model.BetType;
                    IsuseCount = model.IsuseCount;
                    StartTime = model.StartTime.ToString();
                    EndTime = model.EndTime.ToString();
                    StopTypeWhenWinMoney = model.StopTypeWhenWinMoney;
                    CreateTime = model.CreateTime.ToString();
                    LotteryNumber = model.LotteryNumber;
                    SchemeStatus = Convert.ToInt32(model.SchemeStatus);
                    RptBind(model.SchemeID, "CreateTime desc");
                }
            }
        }


        #region 数据绑定
        private void RptBind(long _SchemeID, string _orderby)
        {
            this.page = QPRequest.GetQueryInt("page", 1);
            udv_SchemeChaseTaskDetailBLL bll = new udv_SchemeChaseTaskDetailBLL();
            this.rptList.DataSource = bll.QueryListByPage(_SchemeID, _orderby, this.page, this.pageSize, ref this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("schemeChaseTask_edit.aspx", "action={0}&id={1}&page={2}",
                CaileEnums.ActionEnum.Edit.ToString(), _SchemeID.ToString(), "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion



        #region 返回每页数量
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("schemeChaseTask_list_page_size", "QPcmsPage"), out _pagesize))
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
                    Utils.WriteCookie("schemeChaseTask_list_page_size", "QPcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("schemeChaseTask_edit.aspx", "id={0}", this.SchemeID.ToString()));
        }


        #region 处理内容

        //获取终止条件枚举中对应值或名的描述
        protected string SetStopTypeName(long StopTypeWhenWinMoney)
        {
            int isExisEnum = 1;
            Int32.TryParse(StopTypeWhenWinMoney.ToString(), out isExisEnum);
            if (!System.Enum.IsDefined(typeof(StopType), Convert.ToInt32(StopTypeWhenWinMoney)))
            {
                isExisEnum = 0;
            }
            if (isExisEnum == 0)
                return "超过" + StopTypeWhenWinMoney + "元奖金停止追加";
            else
                return Common.GetDescription((StopType)StopTypeWhenWinMoney);
        }

        protected string ConvertMoney(object val)
        {
            decimal money = 0;
            decimal.TryParse(val.ToString(), out money);
            return (money > 0 ? (money / 100) : 0) + " 元";
        }

        //获取撤销状态枚举中对应值或名的描述
        protected string SetTicketStatusName(long TicketStatus)
        {
            return Common.GetDescription((TicketStatusEnum)TicketStatus);
        }

        //获取电子票状态枚举中对应值或名的描述
        protected string SetQuashStatusName(long QuashStatus)
        {
            return Common.GetDescription((QuashStatusEnum)QuashStatus);
        }
        #endregion
    }
}