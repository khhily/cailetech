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

namespace CL.Admin.admin.users
{
    public partial class user_edit : UI.AdminPage
    {
        protected string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型
        private long id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = QPRequest.GetQueryString("action");

            if (_action == CaileEnums.ActionEnum.Edit.ToString())
            {
                this.action = _action;//修改类型
                this.id = QPRequest.GetQueryInt("id");
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new UsersBLL().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("user_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }

        #region 赋值操作
        private void ShowInfo(long _id)
        {
            var usersEntity = new UsersBLL().QueryEntityByUserCode(_id);
            var usersExtendEntity = new UsersExtendBLL().QueryEntityByUserCode(_id);
            labUserName.Text = usersEntity.UserName;
            labNickName.Text = usersExtendEntity.NickName;
            labFullName.Text = usersExtendEntity.FullName;
            labIDNumber.Text = usersExtendEntity.IDNumber;
            labUserMobile.Text = usersEntity.UserMobile;
            labBalance.Text = Utils.StrToDouble(usersEntity.Balance).ToString();
            labFreeze.Text = Utils.StrToDouble(usersEntity.Freeze).ToString();
            labCreateTime.Text = usersExtendEntity.CreateTime.ToString();
            labIdols.Text = usersExtendEntity.Idols.ToString();
            //labFans.Text = usersExtendEntity.Fans.ToString();
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            UsersEntity model = new UsersEntity();
            UsersBLL bll = new UsersBLL();

            //检测用户名是否重复
            if (bll.Exists(txtUserName.Text.Trim()))
            {
                return false;
            }
            model.UserName = txtUserName.Text.Trim();
            //以随机生成的6位字符串做为密钥加密
            model.UserPassword = Utils.MD5(txtPassword.Text.Trim());
            model.UserMobile = txtUserMobile.Text;
            model.Balance = Convert.ToInt64(txtBalance.Text);
            model.IsCanLogin = rblIsCanLogin.SelectedValue == "0" ? false : true;

            if (bll.InsertEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加用户:" + model.UserName); //记录日志
                return true;
            }
            return false;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(long _id)
        {
            bool result = false;
            return result;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("user_list", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("修改用户信息成功！", "user_list.aspx");
            }
            else //添加
            {
                ChkAdminLevel("user_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("添加用户信息成功！", "user_list.aspx");
            }
        }

    }
}