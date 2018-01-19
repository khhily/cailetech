using CL.Enum.Common;
using CL.Tools.Common;
using System;
using System.Web.UI;

namespace CL.Admin.admin.dialog
{
    public partial class dialog_SetHomeOwners : UI.AdminPage
    {
        protected int id = 0;
        protected string username = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            id = QPRequest.GetQueryInt("id");
            username = QPRequest.GetQueryString("UserName");

            if (!Page.IsPostBack)
            {
                ChkAdminLevel("room_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                txtUserName.Text = username;
            }
        }
    }
}