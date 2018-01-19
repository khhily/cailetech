using CL.Enum.Common.Lottery;
using CL.Game.BLL;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.lotteries
{
    public partial class manualopenlottery : UI.AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LotteryBind();
            }
        }
        private void LotteryBind()
        {
            LotteriesBLL bll = new LotteriesBLL();
            List<LotteriesEntity> list = bll.QueryEntitys("");

            ddlLotteryCode.Items.Clear();
            ddlLotteryCode.Items.Add(new ListItem("请选择彩种..", ""));
            foreach (LotteriesEntity item in list)
            {
                ddlLotteryCode.Items.Add(new ListItem(item.LotteryName, item.LotteryCode.ToString()));
            }
        }
        /// <summary>
        /// 开奖对比
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int LotteryCode = Convert.ToInt32(ddlLotteryCode.SelectedValue); //彩种编号
            string IsuseName = txtIsuseName.Text.Trim(); //期号
            string OpenNumber = txtOpenNumber.Text.Trim(); // 开奖号码
            if (LotteryCode <= 0)
            {
                lbMsg.Text = "请选择彩种。";
                return;
            }
            if (string.IsNullOrEmpty(IsuseName))
            {
                lbMsg.Text = "请录入期号。";
                return;
            }
            if (string.IsNullOrEmpty(OpenNumber))
            {
                lbMsg.Text = "请录入开奖号码。";
                return;
            }
            var EntityAdmin = base.GetAdminInfo();
            string[] Numbers = null;
            #region 验证
            //验证格式
            if (LotteryCode == (int)LotteryInfo.JLK3 || LotteryCode == (int)LotteryInfo.JLK3)
            {
                //快三开奖号码格式
                Numbers = OpenNumber.Split(' ');
                if (Numbers.Length != 3)
                {
                    lbMsg.Text = "开奖号码格式错误(参考格式如：1 2 3)。";
                    return;
                }
                if (OpenNumber.Replace(" ", "").Length != 3)
                {
                    lbMsg.Text = "开奖号码格式错误(参考格式如：1 2 3)。";
                    return;
                }
            }
            if (LotteryCode == (int)LotteryInfo.SD11X5 || LotteryCode == (int)LotteryInfo.HB11X5)
            {
                //快三开奖号码格式
                Numbers = OpenNumber.Split(' ');
                if (Numbers.Length != 5)
                {
                    lbMsg.Text = "开奖号码格式错误(参考格式如：01 02 03 04 05)。";
                    return;
                }
                if (OpenNumber.Replace(" ", "").Length != 10)
                {
                    lbMsg.Text = "开奖号码格式错误(参考格式如：01 02 03 04 05)。";
                    return;
                }
            }
            if (LotteryCode == (int)LotteryInfo.SSQ || LotteryCode == (int)LotteryInfo.CJDLT)
            {
                Numbers = OpenNumber.Split(' ');
                if (Numbers.Length != 7)
                {
                    lbMsg.Text = "开奖号码格式错误(参考格式如：01 02 03 04 05 06 07 红球与蓝球之间用空格区分)。";
                    return;
                }
                if (OpenNumber.Replace(" ", "").Length != 14)
                {
                    lbMsg.Text = "开奖号码格式错误(参考格式如：01 02 03 04 05 06 07 红球与蓝球之间用空格区分)。";
                    return;
                }
            }
            if (LotteryCode == (int)LotteryInfo.CQSSC || LotteryCode == (int)LotteryInfo.JXSSC)
            {
                Numbers = OpenNumber.Split(' ');
                if (Numbers.Length != 5)
                {
                    lbMsg.Text = "开奖号码格式错误(参考格式如：1 2 3 3 2)。";
                    return;
                }
                if (OpenNumber.Replace(" ", "").Length != 5)
                {
                    lbMsg.Text = "开奖号码格式错误(参考格式如：1 2 3 3 2)。";
                    return;
                }
            }

            var EntityRedis = new Redis.BLL.BusinessRedis().ManualOpenLotteryRedis(LotteryCode, IsuseName);
            if (EntityRedis != null)
            {
                //第二次开奖
                if (EntityRedis.OpenLotUser == EntityAdmin.id)
                {
                    //同一个账号开同一彩种期号的数据为第一次开奖
                    EntityRedis.OpenNumber = OpenNumber;
                    new Redis.BLL.BusinessRedis().ManualOpenLotteryRedis(EntityRedis);
                    lbMsg.Text = "同一个账号同一期次开奖为更新，不做第二次确认开奖条件。";
                }
                else
                {
                    if (EntityRedis.OpenNumber != OpenNumber)
                    {
                        //两次开奖号码不一致，开奖失败 重新开奖
                        new Redis.BLL.BusinessRedis().RemoveManualOpenLotteryRedis(LotteryCode, IsuseName);
                        lbMsg.Text = "两次开奖号码不一致，开奖失败，请重新开奖。";
                    }
                    else
                    {
                        string ReturnDescription = "";
                        new IsusesBLL().AddLotteryOpenNumber(LotteryCode, IsuseName, OpenNumber, EntityRedis.StartTime, EntityRedis.EndTime, DateTime.Now, ref ReturnDescription);
                        if (ReturnDescription.Trim() == "添加开奖信息成功" && !string.IsNullOrEmpty(OpenNumber))
                        {
                            new IsusesBLL().InsertIsuseInfoRedis(LotteryCode, IsuseName, OpenNumber, EntityRedis.StartTime, EntityRedis.EndTime, DateTime.Now, 0, 0);
                            new Redis.BLL.BusinessRedis().RemoveManualOpenLotteryRedis(LotteryCode, IsuseName);
                            lbMsg.Text = "第二次确认开奖完成，" + IsuseName + "期开奖成功.";
                        }
                        else
                        {
                            lbMsg.Text = "第二次确认开奖失败，请稍后再试。";
                        }
                    }
                }
            }
            else
            {
                //第一次开奖
                var IsuseEntity = new IsusesBLL().QueryEntitysByLotteryCode(LotteryCode, IsuseName);
                if (IsuseEntity == null)
                {
                    lbMsg.Text = "开奖期号有误，请确认。";
                }
                else if (IsuseEntity.EndTime > DateTime.Now)
                {
                    //当前期次未结期
                    lbMsg.Text = "当前期次未结期，请稍后开奖";
                }
                else
                {
                    EntityRedis = new View.Entity.Game.udv_ManualOpenLottery();
                    EntityRedis.OpenLotUser = EntityAdmin.id;
                    EntityRedis.LotteryCode = LotteryCode;
                    EntityRedis.IsuseName = IsuseName;
                    EntityRedis.OpenNumber = OpenNumber;
                    EntityRedis.StartTime = IsuseEntity.StartTime;
                    EntityRedis.EndTime = IsuseEntity.EndTime;
                    new Redis.BLL.BusinessRedis().ManualOpenLotteryRedis(EntityRedis);

                    lbMsg.Text = "第一次开奖完成，请在五分钟之内完成第二次开奖确认。";
                }
            }

            #endregion
        }
    }
}