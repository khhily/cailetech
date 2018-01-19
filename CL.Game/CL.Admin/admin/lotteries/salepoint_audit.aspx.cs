using CL.Enum.Common;
using CL.Enum.Common.Lottery;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Redis.BLL;
using CL.Tools;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace CL.Admin.admin.lotteries
{
    public partial class salepoint_audit : UI.AdminPage
    {
        private string action = string.Empty; //操作类型
        private int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = QPRequest.GetQueryString("action");

            //如果是编辑则检查信息是否存在
            if (_action == CaileEnums.ActionEnum.Audit.ToString())
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
                ChkAdminLevel("salepoint_list", CaileEnums.ActionEnum.Audit.ToString()); //检查权限
                if (action == CaileEnums.ActionEnum.Audit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("salepoint_list", CaileEnums.ActionEnum.Audit.ToString()); //检查权限
            SalePointBLL bll = new SalePointBLL();
            SalePointEntity model = new SalePointBLL().QueryEntity(this.id);
            model.AuditorID = GetAdminInfo().id;
            model.AuditorName = GetAdminInfo().NickName;
            model.AuditTime = DateTime.Now;
            model.SalePointStatus = 2;
            //这个更新和下面的循环插入明细后面会优化存储过程，目前功能是实现的。
            if (bll.UpadteEntity(model) < 0)
            {
                JscriptMsg("审核过程中发生错误啦！", string.Empty);
                return;
            }
            string ReturnDescription = string.Empty;
            int ReturnValue = 0;
            new SalePointRecordBLL().AddSalePointRecord(model.TicketSourceID, model.LotteryCode, model.SalesRebate, model.StartTime.ToString(), ref ReturnValue, ref ReturnDescription);
            if (!ReturnDescription.Equals("更新点位变更明细记录成功"))
            {
                JscriptMsg("保存过程中发生错误！", string.Empty);
                return;
            }
            AddAdminLog(CaileEnums.ActionEnum.Audit.ToString(), "审核点位变更完成,点位ID为" + this.id); //记录日志
            JscriptMsg("审核成功！", "salepoint_list.aspx");
        }


        #region 赋值操作
        private void ShowInfo(int _id)
        {
            SalePointBLL bll = new SalePointBLL();
            SalePointFileBLL fbll = new SalePointFileBLL();
            SalePointEntity model = bll.QueryEntity(_id);
            labTicketSourceID.Text = model.TicketSourceID == 0 ? "彩乐" : "华阳";
            labLotteryName.Text = model.LotteryName;
            labStartTime.Text = model.StartTime.ToString("yyyy-MM-dd");
            labDescribe.Text = model.Describe;

            string html = string.Empty;
            int num = 1;
            string[] objArr = model.SalesRebate.Split(',');
            foreach (string obj in objArr)
            {
                html += "<dl>"
                        + "<dt>销售阶梯" + num + "</dt>"
                        + "<dd>"
                            + "<label>大于" + obj.Split('#')[0] + "元</label>"
                        + "</dd>"
                    + "</dl>"
                    + "<dl>"
                        + "<dt>销售点位" + num + "</dt>"
                        + "<dd>"
                           + "<label>" + obj.Split('#')[1] + "%</label>"
                        + "</dd>"
                    + "</dl>";
                num++;
            }
            SaleDIV.InnerHtml = html;

            rptList.DataSource = fbll.QueryEntitys(" FileSign = '" + model.FileSign + "' ");
            rptList.DataBind();
        }
        #endregion


    }
}