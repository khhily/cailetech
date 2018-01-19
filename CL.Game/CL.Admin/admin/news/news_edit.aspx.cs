using CL.Enum.Common;
using CL.Enum.Common.Lottery;
using CL.Enum.Common.Status;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.news
{
    public partial class news_edit : UI.AdminPage
    {
        private string action = CaileEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = Request.QueryString["action"];

            //如果是编辑则检查信息是否存在
            if (_action == CaileEnums.ActionEnum.Edit.ToString())
            {
                this.action = _action;//修改类型
                int.TryParse(Request.QueryString["id"], out this.id);
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new NewsBLL().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("news_list", CaileEnums.ActionEnum.View.ToString()); //检查权限
                NewsTypeBind();
                BindLottery();
                if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }

        #region 新闻类型
        private void NewsTypeBind()
        {
            NewsTypesBLL bll = new NewsTypesBLL();
            List<NewsTypesEntity> list = bll.QueryEntitys();

            ddlNewsType.Items.Clear();
            ddlNewsType.Items.Add(new ListItem("请选择类型..", ""));
            foreach (NewsTypesEntity item in list)
            {
                string Id = item.TypeID.ToString();
                string Title = item.TypeName;
                this.ddlNewsType.Items.Add(new ListItem(Title, Id));
            }
        }
        #endregion

        #region 彩种
        protected void BindLottery()
        {
            ddlLottery.DataSource = new LotteriesBLL().QueryEntitys(string.Empty);
            ddlLottery.DataTextField = "LotteryName";
            ddlLottery.DataValueField = "LotteryCode";
            ddlLottery.DataBind();
            ddlLottery.Items.Insert(0, new ListItem()
            {
                Value = "-1",
                Text = "请选择",
                Selected = true
            });
        }
        #endregion

        #region 赋值操作
        private void ShowInfo(int _id)
        {
            NewsBLL bll = new NewsBLL();
            NewsEntity model = bll.QueryEntity(_id);
            ddlNewsType.SelectedValue = model.TypeID.ToString();
            txtTitle.Text = model.Title;
            txtAuthor.Text = model.Author;
            txtKeys.Text = model.Keys;
            ddlEquipment.SelectedValue = model.Equipment.ToString();
            ddlIsRecommend.SelectedValue = Convert.ToString(model.IsRecommend);
            string[] Ball = model.LotNumber.Split('|');
            if (Ball.Length == 1)
                txtRedBall.Text = Ball[0];
            if (Ball.Length == 2)
            {
                txtRedBall.Text = Ball[0];
                txtBlueBall.Text = Ball[1];
            }
            ddlLottery.SelectedValue = model.LotteryCode.ToString();
            txtContent.Value = model.RichText;
            txtSource.Text = model.Source;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {

            string msg = string.Empty;
            if (!this.Verify(ref msg))
                return false;
            NewsEntity model = new NewsEntity();
            NewsBLL bll = new NewsBLL();
            var AdminInfo = this.GetAdminInfo();
            model.TypeID = Convert.ToInt32(ddlNewsType.SelectedValue);
            model.Title = txtTitle.Text.Trim();
            model.Author = txtAuthor.Text.Trim();
            model.Equipment = Convert.ToInt32(ddlEquipment.SelectedValue.Trim());
            model.IsRecommend = Convert.ToBoolean(ddlIsRecommend.SelectedValue.Trim());
            model.Keys = txtKeys.Text.Trim();
            model.LotNumber = string.Format("{0}|{1}", txtRedBall.Text.Trim(), txtBlueBall.Text.Trim());
            model.LotteryCode = Convert.ToInt32(ddlLottery.SelectedValue.Trim());
            model.PlainText = Utils.DropHTML(txtContent.Value);
            model.RichText = txtContent.Value;
            model.Publish = AdminInfo.NickName;
            model.PublishID = AdminInfo.id;
            model.PublishTime = DateTime.Now;
            model.Source = txtSource.Text.Trim();
            bool result = false;
            if (bll.InsertEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加新闻:" + model.Title); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            NewsBLL bll = new NewsBLL();
            NewsEntity model = bll.QueryEntity(_id);
            var AdminInfo = this.GetAdminInfo();
            model.TypeID = Convert.ToInt32(ddlNewsType.SelectedValue);
            model.Title = txtTitle.Text.Trim();
            model.Author = txtAuthor.Text.Trim();
            model.Equipment = Convert.ToInt32(ddlEquipment.SelectedValue.Trim());
            model.IsRecommend = Convert.ToBoolean(ddlIsRecommend.SelectedValue.Trim());
            model.Keys = txtKeys.Text.Trim();
            model.LotNumber = string.Format("{0}|{1}", txtRedBall.Text.Trim(), txtBlueBall.Text.Trim());
            model.LotteryCode = Convert.ToInt32(ddlLottery.SelectedValue.Trim());
            model.PlainText = Utils.DropHTML(txtContent.Value);
            model.RichText = txtContent.Value;
            model.Publish = AdminInfo.NickName;
            model.PublishID = AdminInfo.id;
            model.PublishTime = DateTime.Now;
            model.Auditing = string.Empty;
            model.AuditingID = 0;
            model.AuditingTime = null;
            model.AuditingStatus = (int)AuditingStatusEnum.Wait;
            model.Source = txtSource.Text.Trim();
            if (bll.ModifyEntity(model) > 0)
            {
                AddAdminLog(CaileEnums.ActionEnum.Edit.ToString(), "修改新闻:" + model.Title); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        #region  验证操作=================================
        string[] SSQ_Red = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16",
                                          "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32","33"};
        string[] SSQ_Blue = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16" };

        string[] DLT_Red = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16",
                                          "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32","33","34","35"};
        string[] DLT_Blue = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
        protected bool Verify(ref string msg)
        {

            int LotteryCode = 0;
            int.TryParse(ddlLottery.SelectedValue, out LotteryCode);

            if (LotteryCode > 0)
            {
                string[] Red = txtRedBall.Text.Split(',');
                string[] Blue = txtBlueBall.Text.Split(',');
                string MaxRed = Red.OrderByDescending(o => 0).FirstOrDefault();
                string MinRed = Red.OrderBy(o => 0).FirstOrDefault();
                string MaxBlue = Blue.OrderByDescending(o => 0).FirstOrDefault();
                string MinBlue = Blue.OrderBy(o => 0).FirstOrDefault();

                if (LotteryCode == (int)LotteryInfo.SSQ)
                {
                    if (!SSQ_Red.Contains(MinRed) || !SSQ_Red.Contains(MaxRed))
                    {
                        msg = "推荐红球有误";
                        return false;
                    }
                    if (!SSQ_Blue.Contains(MinBlue) || !SSQ_Blue.Contains(MaxBlue))
                    {
                        msg = "推荐蓝球有误";
                        return false;
                    }
                }
                if (LotteryCode == (int)LotteryInfo.CJDLT)
                {
                    if (!DLT_Red.Contains(MinRed) || !DLT_Red.Contains(MaxRed))
                    {
                        msg = "推荐红球有误";
                        return false;
                    }
                    if (!DLT_Blue.Contains(MinBlue) || !DLT_Blue.Contains(MaxBlue))
                    {
                        msg = "推荐蓝球有误";
                        return false;
                    }
                }
            }


            return true;
        }
        #endregion


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == CaileEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("news_list", CaileEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", string.Empty);
                    return;
                }
                JscriptMsg("修改新闻成功！", "news_list.aspx");
            }
            else //添加
            {
                ChkAdminLevel("news_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", string.Empty);
                    return;
                }
                JscriptMsg("添加新闻成功！", "news_list.aspx");
            }
        }

    }
}