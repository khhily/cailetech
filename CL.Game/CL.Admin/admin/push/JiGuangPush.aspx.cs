using CL.Tools.JiGuangPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.push
{
    public partial class JiGuangPush : UI.AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string Content = TaContent.Value.Trim();
            string Registration = hidRegistration.Value.Trim(',');
            string PushType = ddlPush.SelectedValue;  //1 广播  2单推
            cn.jpush.api.push.MessageResult result = null;
            if (PushType == "1")
            {
                new PushHelper().BroadcastNotice(Content, Content, ref result);
            }
            else
            {
                string[] RegIds = Registration.Split(',');
                new PushHelper().PushPersonal(RegIds, Content, Content, string.Empty, ref result);
            }

        }
    }
}