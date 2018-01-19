using CL.Enum.Common;
using CL.SystemInfo.BLL;
using CL.SystemInfo.Entity;
using CL.Tools.Common;
using System;
using System.Web.UI;
using CL.View.Entity.SystemInfo;
using CL.Redis.BLL;

namespace CL.Admin.admin
{
    public partial class login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //txtUserName.Text = Utils.GetCookie("CLRememberName");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string userPwd = txtPassword.Text.Trim();

            if (userName.Equals("") || userPwd.Equals(""))
            {
                msgtip.InnerHtml = "请输入用户名或密码";
                return;
            }
            udv_LoginCount LoginEntity = new SystemRedis().QueryLoginErrorCount(userName);
            if (LoginEntity != null && LoginEntity.LoginCount > 3)
            {
                msgtip.InnerHtml = "<span style=\"color:red;\">密码错误超3次，账号冻结两小时！</span>";
                return;
            }
            ManagerBLL bll = new ManagerBLL();
            ManagerEntity model = bll.QueryEntity(userName, userPwd, true);
            if (model == null)
            {
                if (LoginEntity == null)
                {
                    LoginEntity = new udv_LoginCount()
                    {
                        LoginCount = 1,
                        LoginName = userName,
                        Time = DateTime.Now
                    };
                }
                else
                    LoginEntity.LoginCount += 1;
                new SystemRedis().LoginErrorCount(LoginEntity);
                msgtip.InnerHtml = "<span style=\"color:red;\">用户名或密码有误，请重试！</span>";
                return;
            }
            if (model.IsLock == 1)
            {
                msgtip.InnerHtml = "<span style=\"color:red;\">当前用户已经停用！</span>";
                return;
            }

            Session[CLKeys.SESSION_ADMIN_INFO] = model;
            Session.Timeout = 45;
            //写入登录日志
            new ManagerLogBLL().InsertEntity(new ManagerLogEntity()
            {
                UserID = model.id,
                UserName = model.UserName,
                ActionType = CaileEnums.ActionEnum.Login.ToString(),
                Remark = "管理员登录",
                UserIP = QPRequest.GetIP(),
                AddTime = DateTime.Now
            });
            //写入Cookies
            //Utils.WriteCookie("CLRememberName", model.UserName, 14400);
            //Utils.WriteCookie("UserName", "CLAdmin", model.UserName);
            //Utils.WriteCookie("UserPwd", "CLAdmin", model.PassWord);
            Response.Redirect("index.aspx");
            return;
        }
    }
}