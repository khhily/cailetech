using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.push
{
    public partial class Marquee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string Content = txtContent.Text.Trim();
            int Number = 3;
            int.TryParse(txtNumber.Text.Trim(), out Number);
            bool Rec = new CL.Game.BLL.IM.Communication().Sendd_Api_Marquee(Number, Content);
            if (Rec)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('发送成功');", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('发送失败');", true);
        }
    }
}