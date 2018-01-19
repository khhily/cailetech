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
    public partial class dialog_AddIsuseXYEB : UI.AdminPage
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
                IsusesEntity model = new IsusesBLL().QueryEntitysLastIsues(LotteryCode);
                if (model != null)
                {
                    DateTime dtLastDate = DateTime.Parse(model.EndTime.ToString());
                    txtDate.Text = dtLastDate.ToString("yyyy-MM-dd");
                    txtIsuseName.Text = model.IsuseName;
                }
                else
                {
                    txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }

        }
    }
}