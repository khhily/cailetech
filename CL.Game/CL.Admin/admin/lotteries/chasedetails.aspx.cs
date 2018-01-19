using CL.Enum.Common;
using CL.Game.BLL;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.lotteries
{
    public partial class chasedetails : UI.AdminPage
    {
        private int id = 0;
        public udv_ChaseTasks model = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.id = QPRequest.GetQueryInt("id");
            if (this.id == 0)
            {
                JscriptMsg("传输参数不正确！", "back");
                return;
            }
            if (!new ChaseTasksBLL().Exists(this.id))
            {
                JscriptMsg("信息不存在或已被删除！", "back");
                return;
            }

            if (!Page.IsPostBack)
            {
                ChkAdminLevel("chase_List", CaileEnums.ActionEnum.View.ToString()); //检查权限
                ShowInfo(this.id);
            }
        }

        #region 赋值操作
        private void ShowInfo(int _id)
        {
            ChaseTasksBLL bll = new ChaseTasksBLL();
            model = bll.QueryChaseTaskTotal(_id);
            labtitle.Text = model.Title;
            lablotteryname.Text = model.LotteryName;
            labplayname.Text = "";
            labdescription.Text = model.Description;
            Label1.Text = "共<font color='red'>" + model.SumIsuseNum + "</font>期<font color='red'>" + model.SumMoney + "</font>元；已完成<font color='red'>" + model.BuyedIsuseNum
                + "</font>期<font color='red'>" + model.BuyedMoney + "</font>元；已取消<font color='red'>" + model.QuashedIsuseNum + "</font>期<font color='red'>" + model.QuashedMoney + "</font>元";

            btnSubmit.Enabled = (model.SumIsuseNum > (model.BuyedIsuseNum + model.QuashedIsuseNum));

            rptList.DataSource = bll.QueryChaseTaskDetails(_id);
            rptList.DataBind();
        }
        #endregion

        //设置操作
        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(((HiddenField)e.Item.FindControl("hidId")).Value);
            ChaseTasksBLL bll = new ChaseTasksBLL();
            string IsuseName = string.Empty;
            switch (e.CommandName)
            {
                case "lbtndStatus":
                    bll.QuashChaseTaskDetail(id, true, ref IsuseName);
                    AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "取消" + IsuseName + "期追号"); //记录日志
                    JscriptMsg("取消此期追号成功！", "chasedetails.aspx?id=" + id.ToString());
                    break;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("chase_List", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
            ChaseTasksBLL bll = new ChaseTasksBLL();
            string AllIsuseName = string.Empty;
            if (bll.QuashChaseTask(this.id, true, ref AllIsuseName) < 0)
            {
                JscriptMsg("保存过程中发生错误啦！", string.Empty);
                return;
            }
            AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "中止任务成功,期号为" + AllIsuseName); //记录日志
            JscriptMsg("中止任务成功！", "chasedetails.aspx?id=" + id.ToString());
        }

    }
}