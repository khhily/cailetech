using System;
using System.Web.UI;
using System.Threading;
using CL.Tools.Common;
using CL.Enum.Common;

namespace CL.Admin.admin.users
{
    public partial class useracc_edit : UI.AdminPage
    {
        protected string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("useracc_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
            }
        }

        #region 增加操作=================================
        private bool DoAdd()
        {
            int num = Convert.ToInt32(txtCount.Text);
            if (num > 0)
            {
                //UserAcc model = new UserAcc();
                //BLL.UserAccBll bll = new BLL.UserAccBll();
                //NetEaseIM im = new NetEaseIM(siteConfig.neteaseappkey, siteConfig.neteaseappsecret);

                //for (int i = 0; i < num; i++)
                //{
                //    string AccID = txtAccID.Text + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                //    NetEaseInfo acc = im.CreateUser(siteConfig.createuser, AccID, "");
                //    if (acc != null)
                //    {
                //        model.AccID = acc.accid;
                //        model.ToKen = acc.token;
                //        if (!bll.Exists(model.AccID))
                //        {
                //            if (bll.Add(model) > 0)
                //            {
                //                AddAdminLog(CLEnums.ActionEnum.Add.ToString(), "添加预注册用户AccID:" + model.AccID); //记录日志
                //            }
                //        }
                //    }
                //    Thread.Sleep(100);
                //}
            }
            return true;
        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == CaileEnums.ActionEnum.Add.ToString())
            {
                ChkAdminLevel("useracc_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", string.Empty);
                    return;
                }
                JscriptMsg("添加预注册用户成功！", "useracc_list.aspx");
            }
        }


    }
}