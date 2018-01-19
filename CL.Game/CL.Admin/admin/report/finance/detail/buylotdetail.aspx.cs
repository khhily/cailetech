using CL.Enum.Common;
using CL.Enum.Common.Lottery;
using CL.Game.BLL;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.report.finance.detail
{
    public partial class buylotdetail : UI.AdminPage
    {
        protected static long SchemeID = 0;
        protected static List<PlayTypesEntity> PlayTypes = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                long.TryParse(Request.QueryString["id"], out SchemeID);

                if (SchemeID > 0)
                    BindData();
            }
        }
        protected void BindData()
        {
            var SchemeEntity = new SchemesBLL().QueryEntity(SchemeID);
            if (SchemeEntity != null)
            {
                var UsersEntity = new UsersBLL().QueryEntityByUserCode(SchemeEntity.InitiateUserID);
                txtLottery.Text = Common.GetDescription((LotteryInfo)Convert.ToInt32(SchemeEntity.LotteryCode));
                txtSchemesNumber.Text = SchemeEntity.SchemeNumber;
                txtSchemeMoney.Text = (SchemeEntity.SchemeMoney / 100).ToString("N2");
                txtCreateTime.Text = SchemeEntity.CreateTime.ToString("yyyy年MM月dd日 HH时mm分ss秒");
                txtSoure.Text = SchemeEntity.PrintOutType == 1 ? "彩乐电子票" : SchemeEntity.PrintOutType == 2 ? "华阳电子票" : "--";
                txtSchemeStatus.Text = SetStatus(SchemeEntity.SchemeStatus);
                txtUserID.Text = SchemeEntity.InitiateUserID.ToString();
                txtUserName.Text = UsersEntity.UserName;
                #region 支付方式 
                UsersRecordEntity RecordEntity1 = new UsersRecordBLL().QueryEntitys(SchemeEntity.InitiateUserID, SchemeEntity.SchemeID, 1);
                UsersRecordEntity RecordEntity22 = new UsersRecordBLL().QueryEntitys(SchemeEntity.InitiateUserID, SchemeEntity.SchemeID, 22);
                string rec = string.Empty;
                //余额|彩券|彩券+余额
                if (RecordEntity1 != null && RecordEntity1.TradeAmount > 0 && RecordEntity22 != null)
                    rec = "彩券+余额";
                else
                {
                    if (RecordEntity22 != null)
                        rec = "彩券";
                    else
                        rec = "余额";
                }
                txtModePay.Text = rec;
                #endregion

                PlayTypes = new PlayTypesBLL().QueryEntitysByLotteryCode(SchemeEntity.LotteryCode);

                var SchemeTicketEntitys = new SchemeETicketsBLL().QueryEntitysBySchemeID(SchemeID);
                this.rptList.DataSource = SchemeTicketEntitys;
                this.rptList.DataBind();
            }
        }
        /// <summary>
        /// 出票方式 1本地出票 2华阳电子票
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string SetOutType(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            switch (Convert.ToInt32(obj))
            {
                case 0: rec = "彩乐电子票"; break;
                case 1: rec = "华阳电子票"; break;
                default: rec = "--"; break;
            }
            return rec;
        }
        /// <summary>
        /// 状态,0.待出票，1.投注成功，2.出票完成，3.投注失败，4.出票失败，8.兑奖中，10.中奖，11.不中
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string SetTicketStatus(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            switch (Convert.ToInt32(obj))
            {
                case 0: rec = "待出票"; break;
                case 1: rec = "投注成功"; break;
                case 2: rec = "出票完成"; break;
                case 3: rec = "投注失败"; break;
                case 4: rec = "出票失败"; break;
                case 8: rec = "兑奖中"; break;
                case 10: rec = "中奖"; break;
                case 11: rec = "不中"; break;
                default: rec = "--"; break;
            }
            return rec;
        }
        protected string SetStatus(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            switch (Convert.ToInt32(obj))
            {
                case 0: rec = "待付款"; break;
                case 2: rec = "订单过期"; break;
                case 4: rec = "下单成功"; break;
                case 6: rec = "出票成功"; break;
                case 8: rec = "部分出票成功"; break;
                case 10: rec = "下单失败（限号）"; break;
                case 12: rec = "订单撤销"; break;
                case 14: rec = "中奖"; break;
                case 15: rec = "派奖中"; break;
                case 16: rec = "派奖完成"; break;
                case 18: rec = "未中奖"; break;
                case 19: rec = "追号中"; break;
                case 20: rec = "追号完成"; break;
                default: rec = "--"; break;
            }
            return rec;
        }
        protected string SetPlayCode(object obj)
        {
            string rec = string.Empty;
            if (obj == null)
                rec = "--";
            else if (PlayTypes == null)
                rec = "--";
            else
                rec = PlayTypes.Where(w => w.PlayCode == Convert.ToInt32(obj)).Select(s => s.PlayName).FirstOrDefault();
            return rec;
        }
    }
}