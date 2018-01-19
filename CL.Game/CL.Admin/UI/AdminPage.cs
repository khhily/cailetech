using CL.SystemInfo.BLL;
using CL.SystemInfo.Entity;
using CL.Tools.Common;
using CL.View.Entity.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CL.Admin.UI
{
    public class AdminPage : System.Web.UI.Page
    {
        protected internal SiteConfig siteConfig;
        protected internal ManagerEntity Managerinfo = null;

        public AdminPage()
        {
            this.Load += new EventHandler(AdminPage_Load);
            siteConfig = new SiteConfigBLL().loadConfig();
        }

        private void AdminPage_Load(object sender, EventArgs e)
        {
            //判断管理员是否登录
            if (!IsAdminLogin())
            {
                Response.Write("<script>parent.location.href='/admin/login.aspx'</script>");
                Response.End();
            }
            Managerinfo = GetAdminInfo();
        }

        #region 管理员============================================
        /// <summary>
        /// 判断管理员是否已经登录(解决Session超时问题)
        /// </summary>
        public bool IsAdminLogin()
        {
            //如果Session为Null
            if (Session[CLKeys.SESSION_ADMIN_INFO] != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 取得管理员信息
        /// </summary>
        public ManagerEntity GetAdminInfo()
        {
            if (IsAdminLogin())
            {
                ManagerEntity model = Session[CLKeys.SESSION_ADMIN_INFO] as ManagerEntity;
                if (model != null)
                {
                    #region 防止操作过程中超时
                    Session[CLKeys.SESSION_ADMIN_INFO] = model;
                    Session.Timeout = 45;
                    #endregion
                    return model;
                }
            }
            return null;
        }

        /// <summary>
        /// 检查管理员权限
        /// </summary>
        /// <param name="nav_name">菜单名称</param>
        /// <param name="action_type">操作类型</param>
        public void ChkAdminLevel(string nav_name, string action_type)
        {
            ManagerEntity model = GetAdminInfo();
            RosleBLL bll = new RosleBLL();
            bool result = bll.Exists(model.RoleID, nav_name, action_type);

            if (!result)
            {
                string msgbox = "parent.jsdialog(\"错误提示\", \"您没有管理该页面的权限，请勿非法进入！\", \"back\")";
                Response.Write("<script type=\"text/javascript\">" + msgbox + "</script>");
                Response.End();
            }
        }
        
        /// <summary>
        /// 写入管理日志
        /// </summary>
        /// <param name="action_type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool AddAdminLog(string action_type, string remark)
        {
            ManagerEntity model = GetAdminInfo();
            int newId = new ManagerLogBLL().InsertEntity(new ManagerLogEntity()
            {
                UserID = model.id,
                UserName = model.UserName,
                ActionType = action_type,
                Remark = remark,
                UserIP = QPRequest.GetIP(),
                AddTime = DateTime.Now
            });
            if (newId > 0)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region JS提示============================================
        /// <summary>
        /// 添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        protected void JscriptMsg(string msgtitle, string url)
        {
            string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\")";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
        }
        /// <summary>
        /// 带回传函数的添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        /// <param name="callback">JS回调函数</param>
        protected void JscriptMsg(string msgtitle, string url, string callback)
        {
            string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\", " + callback + ")";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
        }
        #endregion

    }
}