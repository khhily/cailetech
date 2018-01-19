using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Web.UI;

namespace CL.Admin.admin.dialog
{
    public partial class dialog_AddIsuse : UI.AdminPage
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
                    if (LotteryCode != 301)
                        dtLastDate = dtLastDate.AddDays(1);
                    txtDate.Text = dtLastDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
        }
    }
}