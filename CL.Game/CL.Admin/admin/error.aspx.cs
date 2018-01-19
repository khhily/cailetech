using CL.Tools.Common;
using System;

namespace CL.Admin.admin
{
    public partial class error : System.Web.UI.Page
    {
        protected string msg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            msg = QPRequest.GetString("msg");
        }
    }
}