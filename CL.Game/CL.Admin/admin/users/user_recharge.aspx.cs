using CL.Enum.Common;
using CL.Game.BLL;
using System;
using System.Web.UI;

namespace CL.Admin.admin.users
{
    public partial class user_recharge : UI.AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("user_recharge", CaileEnums.ActionEnum.View.ToString()); //检查权限
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("user_recharge", CaileEnums.ActionEnum.View.ToString()); //检查权限
            long Amount = Convert.ToInt64(txtMoney.Text) * 100;
            string RetDes = string.Empty;
            long iResult = new UsersBLL().AddUserBalanceManual(Managerinfo.id, txtUserName.Text.Trim(), Amount, "后台手工充值，" + txtMemo.Text.Trim(), ref RetDes);
            if (iResult < 0)
            {
                JscriptMsg(RetDes, string.Empty);
                return;
            }
            if (iResult > 0 && RetDes.Trim() == "后台手工充值成功")
            {
                if (Amount > 0)
                    new UsersBLL().ModifyUserBalanceRedis(iResult, Amount, true);
                else
                {
                    Amount = Convert.ToInt64(txtMoney.Text.Trim().Substring(1, txtMoney.Text.Trim().Length - 1)) * 100;
                    new UsersBLL().ModifyUserBalanceRedis(iResult, Amount, false);
                }
            }

            JscriptMsg("充值成功！", "user_recharge.aspx");
        }

    }
}