using CL.Enum.Common;
using CL.Enum.Common.Lottery;
using CL.Enum.Common.Other;
using CL.Enum.Common.Status;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.news
{
    public partial class lookauditing : UI.AdminPage
    {
        protected static int id = 0;
        protected static NewsEntity Entity = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int.TryParse(Request.QueryString["id"], out id);
                string _action = QPRequest.GetQueryString("action");
                if (id > 0)
                {
                    if (_action == CaileEnums.ActionEnum.Audit.ToString())
                    {
                        BindData();
                    }
                    else
                    {
                        btnAdopt.Enabled = false;
                        btnRefuse.Enabled = false;
                        btnAdopt.BackColor = Color.Gray;
                        btnRefuse.BackColor = Color.Gray;
                    }
                }
                else
                {
                    btnAdopt.Enabled = false;
                    btnRefuse.Enabled = false;
                    btnAdopt.BackColor = Color.Gray;
                    btnRefuse.BackColor = Color.Gray;
                }
            }
        }
        protected void BindData()
        {
            btnRefuse.BackColor = Color.Red;
            try
            {
                Entity = new NewsBLL().QueryEntity(id);
                if (Entity != null)
                {
                    var TypeEntity = new NewsTypesBLL().QueryEntity(Entity.TypeID);
                    txtNewsType.Text = TypeEntity.TypeName;
                    txtTitle.Text = Entity.Title;
                    txtKeys.Text = Entity.Keys;
                    txtAuthor.Text = Entity.Author;
                    txtSource.Text = Entity.Source;
                    txtEquipment.Text = Common.GetDescription((EquipmentEnum)Entity.Equipment);
                    txtLottery.Text = Common.GetDescription((LotteryInfo)Entity.LotteryCode);
                    txtLotNumber.Text = Entity.LotNumber;
                    txtIsRecommend.Text = Entity.IsRecommend == true ? "推荐" : "不推荐";
                    txtPublish.Text = Entity.Publish;
                    txtPublishTime.Text = Entity.PublishTime.ToString();
                    txtModify.Text = Entity.Modify;
                    txtModifyTime.Text = Entity.ModifyTime.ToString();
                    txtSort.Text = Entity.Sort.ToString();
                    txtReadNum.Text = Entity.ReadNum.ToString();
                    txtSupportNum.Text = Entity.SupportNum.ToString();
                    txtOpposeNum.Text = Entity.OpposeNum.ToString();
                    txtContent.Value = Entity.RichText;
                }
            }
            catch
            {
                JscriptMsg("审核资源读取失败！", "lookauditing.aspx");
            }
        }
        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdopt_Click(object sender, EventArgs e)
        {
            if (id > 0 && Entity != null)
            {
                Entity.AuditingStatus = (int)AuditingStatusEnum.Adopt;
                int Rec = new NewsBLL().ModifyEntity(Entity);
                if (Rec > 0)
                {
                    AddAdminLog(CaileEnums.ActionEnum.Edit.ToString(), "新闻资讯审核通过:" + Entity.Title); //记录日志
                    JscriptMsg("新闻资讯审核完成！", "auditinglist.aspx");
                    return;
                }
            }
        }

        protected void btnRefuse_Click(object sender, EventArgs e)
        {
            if (id > 0 && Entity != null)
            {
                Entity.AuditingStatus = (int)AuditingStatusEnum.Refuse;
                int Rec = new NewsBLL().ModifyEntity(Entity);
                if (Rec > 0)
                {
                    AddAdminLog(CaileEnums.ActionEnum.Edit.ToString(), "新闻资讯审核拒绝:" + Entity.Title); //记录日志
                    JscriptMsg("新闻资讯审核拒绝发生错误！", string.Empty);
                    return;
                }
            }
        }
    }
}