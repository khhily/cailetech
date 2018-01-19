using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.dialog
{
    public partial class dialog_SetWinMoney : UI.AdminPage
    {
        protected int LotteryCode = 0;
        protected int SchemeID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            LotteryCode = QPRequest.GetQueryInt("LotteryCode");
            SchemeID = QPRequest.GetQueryInt("SchemeID");

            if (!Page.IsPostBack)
            {
                ChkAdminLevel("scheme_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
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
    }
}