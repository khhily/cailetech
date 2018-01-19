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

namespace CL.Admin.admin.lotteries
{
    public partial class scheme_edit : UI.AdminPage
    {
        private string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型
        protected long SchemeID = 0;
        protected udv_Schemes model = new udv_Schemes();
        protected string LotteryName = string.Empty;
        protected string IsuseName = string.Empty;
        protected string OpenNumber = string.Empty;
        protected string SchemeNumber = string.Empty;
        protected string CreateTime = string.Empty;
        protected int SchemeStatus = -1;
        protected string LotteryNumber = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = QPRequest.GetQueryString("action");
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
                model = new udv_SchemesBLL().QueryEntity(SchemeID);
                if (model != null)
                {
                    LotteryName = model.LotteryName;
                    IsuseName = model.IsuseName;
                    OpenNumber = model.OpenNumber;
                    SchemeNumber = model.SchemeNumber;
                    CreateTime = model.CreateTime.ToString();
                    LotteryNumber = model.LotteryNumber;
                    SchemeStatus = Convert.ToInt32(model.SchemeStatus);

                    List<udv_SchemesDetail> list = new udv_SchemesDetailBLL().QueryEntitysBySchemeID(model.SchemeID);
                    rptList.DataSource = list;
                    rptList.DataBind();
                }
            }
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DropDownList ddlWintypes;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int LotteryCode = Convert.ToInt32(((HiddenField)e.Item.FindControl("hidlcode")).Value);
                ddlWintypes = (DropDownList)e.Item.FindControl("ddlWintypes");
                WinTypesBLL bll = new WinTypesBLL();
                List<WinTypesEntity> list = bll.QueryEntitysByLotteryCode(LotteryCode);
                ddlWintypes.Items.Clear();
                ddlWintypes.Items.Add(new ListItem("请选择奖等..", ""));
                foreach (WinTypesEntity item in list)
                {
                    ddlWintypes.Items.Add(new ListItem(item.WinName, item.WinID.ToString()));
                }
            }
        }

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("scheme_list", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
                string AllValues = string.Empty;
                SchemesBLL bll = new SchemesBLL();
                for (int i = 0; i < rptList.Items.Count; i++)
                {
                    string id = ((HiddenField)rptList.Items[i].FindControl("hidId")).Value;
                    DropDownList cb = (DropDownList)rptList.Items[i].FindControl("ddlWintypes");
                    if (!string.IsNullOrEmpty(cb.SelectedValue))
                    {
                        AllValues += id + "," + cb.SelectedValue + "#";
                    }
                }

                string ReturnDescription = string.Empty;
                if (bll.SetWinMoney(SchemeID, AllValues, ref ReturnDescription))
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }

                string Msg = string.Format("后台设置中奖信息成功【彩种:{0},期号:{1},方案号{2},用户{3}】", model.LotteryName, model.IsuseName, model.SchemeNumber, model.UserName);
                AddAdminLog(CaileEnums.ActionEnum.Edit.ToString(), Msg); //记录日志
                JscriptMsg("设置中奖信息成功！", "scheme_list.aspx");
            }
        }

    }
}