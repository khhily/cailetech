using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Redis.BLL;
using CL.Tools;
using CL.Tools.Common;
using System;
using System.Web.UI;

namespace CL.Admin.admin.lotteries
{
    public partial class lotteries_edit : UI.AdminPage
    {
        private string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = QPRequest.GetQueryString("action");

            //如果是编辑则检查信息是否存在
            if (_action == CaileEnums.ActionEnum.Edit.ToString())
            {
                this.action = _action;//修改类型
                this.id = QPRequest.GetQueryInt("id");
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new LotteriesBLL().Exists(this.id))
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
            LotteriesBLL bll = new LotteriesBLL();
            LotteriesEntity model = bll.QueryLotterys(_id);

            txtLotterryName.Text = model.LotteryName;
            txtShorthand.Text = model.Shorthand;
            txtLotteryCode.Text = model.LotteryCode.ToString();
            txtLotteryCode.ReadOnly = true;
            txtLotteryCode.Attributes.Remove("ajaxurl");

            rblTypeID.SelectedValue = model.TypeID.ToString();
            txtSubhead.Text = model.Subhead;
            txtMaxChaseIsuse.Text = model.MaxChaseIsuse;
            txtWinNumberExemple.Text = model.WinNumberExemple;
            txtIntervalType.Text = model.IntervalType;
            ddlPrintOutType.Text = model.PrintOutType.ToString();
            txtPrice.Text = model.Price.ToString();
            txtMaxMultiple.Text = model.MaxMultiple.ToString();
            txtOffTime.Text = model.OffTime.ToString();
            txtChaseExecuteDeferMinute.Text = model.ChaseDeferTime.ToString();
            txtQuashExecuteDeferMinute.Text = model.QuashChaseTime.ToString();
            txtKinformation.Value = model.Kinformation;
            rblIsEmphasis.SelectedValue = (model.IsEmphasis == null ? 0 : model.IsEmphasis == true ? 1 : 0).ToString();
            rblIsAddaward.SelectedValue = model.IsAddaward ? "1" : "0";
            rblIsEnable.SelectedValue = model.IsEnable ? "1" : "0";
            txtModuleVersion.Text = model.ModuleVersion.ToString();
            txtSortId.Text = model.Sort.ToString();
            txtAdvanceEndTime.Text = model.AdvanceEndTime.ToString();
            txtPresellTime.Text = model.PresellTime.ToString();
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = false;
            LotteriesEntity model = new LotteriesEntity();
            LotteriesBLL bll = new LotteriesBLL();

            model.LotteryName = txtLotterryName.Text.Trim();
            model.Shorthand = txtShorthand.Text.Trim();
            model.LotteryCode = Convert.ToInt32(txtLotteryCode.Text.Trim());
            model.TypeID = (byte)Convert.ToInt16(rblTypeID.SelectedValue);
            model.Subhead = txtSubhead.Text.Trim();
            model.MaxChaseIsuse = txtMaxChaseIsuse.Text.Trim();
            model.WinNumberExemple = txtWinNumberExemple.Text.Trim();
            model.IntervalType = txtIntervalType.Text;
            model.PrintOutType = Convert.ToByte(ddlPrintOutType.SelectedValue);
            model.Price = Convert.ToInt32(txtPrice.Text.Trim());
            model.MaxMultiple = Convert.ToInt32(txtMaxMultiple.Text.Trim());
            model.OffTime = Convert.ToInt32(txtOffTime.Text);
            model.ChaseDeferTime = int.Parse(txtChaseExecuteDeferMinute.Text.Trim());
            model.QuashChaseTime = int.Parse(txtQuashExecuteDeferMinute.Text.Trim());
            model.Kinformation = txtKinformation.Value;
            model.IsEmphasis = rblIsEmphasis.SelectedValue == "1";
            model.IsAddaward = rblIsAddaward.SelectedValue == "1";
            model.IsEnable = rblIsEnable.SelectedValue == "1";
            model.ModuleVersion = string.IsNullOrEmpty(txtModuleVersion.Text) == false ? 0 : Convert.ToInt32(txtModuleVersion.Text);
            model.Sort = Convert.ToInt32(txtSortId.Text);
            model.AdvanceEndTime = Convert.ToInt32(txtAdvanceEndTime.Text);
            model.PresellTime = Convert.ToInt32(txtPresellTime.Text);

            if (bll.InsertEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加彩种:" + model.LotteryName); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            LotteriesBLL bll = new LotteriesBLL();
            LotteriesEntity model = bll.QueryLotterys(_id);

            model.LotteryName = txtLotterryName.Text.Trim();
            model.Shorthand = txtShorthand.Text.Trim();
            model.LotteryCode = Convert.ToInt32(txtLotteryCode.Text.Trim());
            model.TypeID = (byte)Convert.ToInt16(rblTypeID.SelectedValue);
            model.Subhead = txtSubhead.Text.Trim();
            model.MaxChaseIsuse = txtMaxChaseIsuse.Text.Trim();
            model.WinNumberExemple = txtWinNumberExemple.Text.Trim();
            model.IntervalType = txtIntervalType.Text;
            model.PrintOutType = Convert.ToByte(ddlPrintOutType.SelectedValue);
            model.Price = Convert.ToInt32(txtPrice.Text.Trim());
            model.MaxMultiple = Convert.ToInt32(txtMaxMultiple.Text.Trim());
            model.OffTime = Convert.ToInt32(txtOffTime.Text);
            model.ChaseDeferTime = Convert.ToInt32(txtChaseExecuteDeferMinute.Text.Trim());
            model.QuashChaseTime = Convert.ToInt32(txtQuashExecuteDeferMinute.Text.Trim());
            model.Kinformation = txtKinformation.Value;
            model.IsEmphasis = rblIsEmphasis.SelectedValue == "1";
            model.IsAddaward = rblIsAddaward.SelectedValue == "1";
            model.IsEnable = rblIsEnable.SelectedValue == "1";
            model.ModuleVersion = Convert.ToInt32(txtModuleVersion.Text);
            model.Sort = Convert.ToInt32(txtSortId.Text);
            model.AdvanceEndTime = Convert.ToInt32(txtAdvanceEndTime.Text);
            model.PresellTime = Convert.ToInt32(txtPresellTime.Text);

            if (bll.ModifyEntity(model))
            {
                AddAdminLog(CaileEnums.ActionEnum.Edit.ToString(), "修改彩种:" + model.LotteryName); //记录日志
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
                #region 更新redis彩种数据
                new SystemRedis().RemoveApplyLotteryDataRedis();
                #endregion
                JscriptMsg("修改彩种成功！", "lotteries_list.aspx");
            }
            else //添加
            {
                ChkAdminLevel("lotteries_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", string.Empty);
                    return;
                }
                #region 更新redis彩种数据
                new SystemRedis().RemoveApplyLotteryDataRedis();
                #endregion
                JscriptMsg("添加彩种成功！", "lotteries_list.aspx");
            }
        }

    }
}