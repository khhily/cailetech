using CL.Enum.Common;
using CL.Tools.Common;
using System;
using System.Web.UI;

namespace CL.Admin.admin.dialog
{
    public partial class dialog_AddIsuseFC : UI.AdminPage
    {
        protected int LotteryCode = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            LotteryCode = QPRequest.GetQueryInt("LotteryCode");
            if (LotteryCode == 0)
            {
                JscriptMsg("传输参数不正确！", "back");
                return;
            }

            if (!Page.IsPostBack)
            {
                ChkAdminLevel("lotteries_isuses", CaileEnums.ActionEnum.Add.ToString()); //检查权限
            }
        }
    }
}