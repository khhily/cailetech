using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CL.Admin.UI;
using CL.Game.BLL;

namespace CL.Admin.admin.report
{
    public partial class CheckingReport : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDay.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            //待加redis存储
            DateTime dt = DateTime.Now.AddMonths(-1);
            if (!string.IsNullOrEmpty(txtDay.Text.Trim()))
            {
                if (Convert.ToInt32(Convert.ToDateTime(txtDay.Text.Trim()).ToString("yyyyMM")) >= Convert.ToInt32(Convert.ToDateTime(DateTime.Now).ToString("yyyyMM")))
                {
                    Response.Write("<script>alert('不支持查询当前月或未来月份数据');</script>");
                    return;
                }
                dt = Convert.ToDateTime(txtDay.Text.Trim());
            }
            this.rptList.DataSource = new UsersRecordBLL().QueryCheckingReport(dt);
            this.rptList.DataBind();
        }
    }
}