using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace CL.Admin.admin.settings
{
    public partial class systemconfig_edit : UI.AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("sys_config", CaileEnums.ActionEnum.View.ToString()); //检查权限
                ShowInfo();
            }
        }

        #region 赋值操作
        private void ShowInfo()
        {
            SystemSetInfoBLL bll = new SystemSetInfoBLL();
            List<SystemSetInfoEntity> list = bll.QueryEntitys();
            list.Sort((x, y) =>
            {
                return x.Sort.CompareTo(y.Sort);
            });

            Configjson.Value = JsonHelper.ObjectToJSON(list);
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("sys_config", CaileEnums.ActionEnum.Add.ToString()); //检查权限

            if (string.IsNullOrEmpty(SetKeyHidden.Value.Trim()))
            {
                JscriptMsg("没有修改的配置项目", "systemconfig_edit.aspx", "parent.loadMenuTree");
            }

            string[] ArrSetKey = SetKeyHidden.Value.Split('*');
            string[] ArrSetValue = SetValueHidden.Value.Split('*');
            SystemSetInfoBLL bll = new SystemSetInfoBLL();
            List<SystemSetInfoEntity> list = bll.QueryEntitys();
            for(int i = 0; i < ArrSetKey.Length; i++)
            {
                string skey = ArrSetKey[i].Replace("setvalue_", "");
                string svalue = ArrSetValue[i];
                SystemSetInfoEntity mode = list.FirstOrDefault(s => s.SetKey == skey);

                if (mode.SetValue != svalue)
                {
                    mode.SetValue = svalue;
                    bll.ModifyEntity(mode);
                }
            }
            JscriptMsg("维护系统设置成功！", "systemconfig_edit.aspx", "parent.loadMenuTree");
        }

    }
}