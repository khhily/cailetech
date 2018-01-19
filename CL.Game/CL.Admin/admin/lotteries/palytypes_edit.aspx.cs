using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools;
using CL.Tools.Common;
using System;
using System.Web.UI;

namespace CL.Admin.admin.lotteries
{
    public partial class palytypes_edit : UI.AdminPage
    {
        private string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;
        protected int LotteryCode = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            LotteryCode = QPRequest.GetQueryInt("LotteryCode");
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
                if (!new PlayTypesBLL().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("lotteries_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }

        #region 赋值操作
        private void ShowInfo(int _id)
        {
            PlayTypesBLL bll = new PlayTypesBLL();
            PlayTypesEntity model = bll.QueryEntity(_id);

            txtPlayName.Text = model.PlayName;
            txtPlayCode.Text = model.PlayCode.ToString();
            txtPlayCode.ReadOnly = true;
            txtPlayCode.Attributes.Remove("ajaxurl");

            txtModuleName.Text = model.ModuleName;
            txtSortId.Text = model.Sort.ToString();
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = false;
            PlayTypesEntity model = new PlayTypesEntity();
            PlayTypesBLL bll = new PlayTypesBLL();

            model.PlayName = txtPlayName.Text.Trim();
            model.LotteryCode = LotteryCode;
            model.PlayCode = Convert.ToInt32(txtPlayCode.Text);
            model.Price = 0;
            model.MaxMultiple = 0;
            model.ModuleName = txtModuleName.Text.Trim();
            model.Sort = Convert.ToInt32(txtSortId.Text);

            if (bll.InsertEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加玩法:" + model.PlayName); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            PlayTypesBLL bll = new PlayTypesBLL();
            PlayTypesEntity model = bll.QueryEntity(_id);

            model.PlayName = txtPlayName.Text.Trim();
            model.LotteryCode = LotteryCode;
            model.PlayCode = Convert.ToInt32(txtPlayCode.Text);
            model.ModuleName = txtModuleName.Text.Trim();
            model.Sort = Convert.ToInt32(txtSortId.Text);

            if (bll.ModifyEntity(model))
            {
                AddAdminLog(CaileEnums.ActionEnum.Edit.ToString(), "修改玩法:" + model.PlayName); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("lotteries_list", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", string.Empty);
                    return;
                }
                JscriptMsg("修改玩法成功！", "palytypes_list.aspx?LotteryCode=" + LotteryCode);
            }
            else //添加
            {
                ChkAdminLevel("lotteries_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", string.Empty);
                    return;
                }
                JscriptMsg("添加玩法成功！", "palytypes_list.aspx?LotteryCode=" + LotteryCode);
            }
        }

    }
}