using CL.SystemInfo.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin
{
    public partial class index : UI.AdminPage
    {
        //public ManagerEntity Managerinfos; //管理员信息
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Managerinfo = GetAdminInfo();
            }
        }

        //安全退出
        protected void lbtnExit_Click(object sender, EventArgs e)
        {
            Session[CLKeys.SESSION_ADMIN_INFO] = null;
            //Utils.WriteCookie("UserName", "CLAdmin", -14400);
            //Utils.WriteCookie("UserPwd", "CLAdmin", -14400);
            Response.Redirect("login.aspx");
        }
    }
}