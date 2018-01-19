using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Json.Entity.WebAPI;
using CL.Redis.BLL;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using CL.Enum.Common.Lottery;

namespace CL.Admin.admin.lotteries
{
    public partial class openlottery_edit : UI.AdminPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("inputopenlottery", CaileEnums.ActionEnum.View.ToString()); //检查权限
                LotteryBind();
            }
        }

        #region 彩种类型
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
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("inputopenlottery", CaileEnums.ActionEnum.View.ToString()); //检查权限
            int LotteryCode = Convert.ToInt32(ddlLotteryCode.SelectedValue); //彩种编号
            string IsuseName = txtIsuseName.Text.Trim(); //期号
            string OpenNumber = hidWinNumer.Value.Trim(); // 开奖号码
            var EntityAdmin = base.GetAdminInfo();//当前登录管理员
            
            string[] Numbers = null;
            #region 彩种、期号、开奖号码非空验证
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
            #endregion
            if (LotteryCode == (int)LotteryInfo.SSQ || LotteryCode == (int)LotteryInfo.CJDLT)
            {
                long WinRollover = Convert.ToInt64(txtWinRollover.Text.Trim()) + 00; //奖池滚存
                long TotalSales = Convert.ToInt64(txtTotalSales.Text.Trim()) + 00; //本期销售
                string Isusebonuses = this.isusebonuses.Value; //奖等拼接的字符串
                #region 低频彩开奖号码验证
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
                #endregion
                #region 低频彩处理
                try
                {
                    #region 获得奖等数据集
                    List<IsuseBonusesEntity> Entitys = new List<IsuseBonusesEntity>();
                    List<DataAwardEntity> DataAward = new List<DataAwardEntity>();
                    var IsuseEntity = new IsusesBLL().QueryEntitysByLotteryCode(Convert.ToInt32(LotteryCode), IsuseName);
                    if (IsuseEntity == null)
                    {
                        lbMsg.Text = "开奖期号有误，请确认。";
                        return;
                    }
                    else if (IsuseEntity.EndTime > DateTime.Now)
                    {
                        //当前期次未结期
                        lbMsg.Text = "当前期次未结期，请稍后开奖";
                        return;
                    }
                    if (IsuseEntity != null)
                        Entitys = new IsuseBonusesBLL().QueryEntitysByIsuseID(IsuseEntity.IsuseID);
                    if (Entitys != null && Entitys.Count > 0)
                        Entitys = Entitys.OrderByDescending(o => o.defaultMoney).ToList();
                    foreach (var Entity in Entitys)
                        DataAward.Add(new DataAwardEntity()
                        {
                            WinLevel = Entity.WinLevel,
                            DefaultMoney = Entity.DefaultMoneyNoWithTax,
                            WinBet = Convert.ToInt32(Entity.WinBet)
                        }); 
                    #endregion
                    OfLotteryDetailEntity EntityRedis = new BusinessRedis().OfLotteryDetailRedis(Convert.ToInt32(LotteryCode), IsuseName);
                    if (EntityRedis != null)
                    {
                        //第二次开奖
                        if (EntityRedis.OpenLotUser == EntityAdmin.id)
                        {
                            //同一个账号开同一彩种期号的数据为第一次开奖
                            EntityRedis.OpenNumber = OpenNumber;
                            EntityRedis.WinRollover = WinRollover;
                            EntityRedis.TotalSales = TotalSales;
                            EntityRedis.DataAward = DataAward;
                            EntityRedis.Isusebonuses = Isusebonuses;
                            new Redis.BLL.BusinessRedis().OfLotteryDetailRedis(EntityRedis);
                            lbMsg.Text = "同一个账号同一期次开奖为更新，不做第二次确认开奖条件。";
                        }
                        else
                        {
                            if (EntityRedis.OpenNumber != OpenNumber || EntityRedis.Isusebonuses != Isusebonuses)
                            {
                                //两次开奖号码不一致，开奖失败 重新开奖
                                new Redis.BLL.BusinessRedis().RemoveOfLotteryDetailRedis(LotteryCode, IsuseName);
                                lbMsg.Text = "两次开奖号码或奖等不一致，开奖失败，请重新开奖。";
                            }
                            else
                            {
                                string ReturnDescription = "";
                                string RetDes = string.Empty;
                                bool bResult = new IsusesBLL().AddLotteryOpenNumber(LotteryCode, IsuseName, OpenNumber, EntityRedis.StartTime, EntityRedis.EndTime, DateTime.Now, ref ReturnDescription);
                                if (!bResult)
                                {
                                    JscriptMsg(RetDes, string.Empty);
                                    return;
                                }
                                int iResult = new IsuseBonusesBLL().InsertEntitys(Managerinfo.id, Convert.ToInt32(LotteryCode), IsuseName, WinRollover, txtBettingPrompt.Text.Trim(), TotalSales, Isusebonuses, ref RetDes);
                                if (iResult < 0)
                                {
                                    JscriptMsg(RetDes, string.Empty);
                                    return;
                                }
                                if (ReturnDescription.Trim() == "添加开奖信息成功" && !string.IsNullOrEmpty(OpenNumber))
                                {
                                    new IsusesBLL().InsertIsuseInfoRedis(LotteryCode, IsuseName, OpenNumber, EntityRedis.StartTime, EntityRedis.EndTime, DateTime.Now, 0, 0);
                                    new Redis.BLL.BusinessRedis().RemoveOfLotteryDetailRedis(LotteryCode, IsuseName);
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
                        
                            EntityRedis = new CL.Json.Entity.WebAPI.OfLotteryDetailEntity();
                            EntityRedis.OpenLotUser = EntityAdmin.id;
                            EntityRedis.LotteryCode = LotteryCode;
                            EntityRedis.IsuseNum = IsuseName;
                            EntityRedis.OpenNumber = OpenNumber;
                            EntityRedis.OpenTime = Convert.ToDateTime(IsuseEntity.OpenTime).ToString("yyyyMMddHHmmss");
                            EntityRedis.EndTime = IsuseEntity.StartTime;
                            EntityRedis.EndTime = IsuseEntity.EndTime;
                            new Redis.BLL.BusinessRedis().OfLotteryDetailRedis(EntityRedis);
                            lbMsg.Text = "第一次开奖完成，请在五分钟之内完成第二次开奖确认。";
                    }
                    //AddAdminLog(CaileEnums.ActionEnum.Add.ToString(), "添加奖等信息成功，彩种：" + ddlLotteryCode.Text + "期号：" + txtIsuseName.Text + ""); //记录日志
                    //JscriptMsg("添加奖等成功！", "openlottery_edit.aspx");
                }
                catch (Exception ex)
                {
                    JscriptMsg("添加奖等失败！" + ex.Message, "openlottery_edit.aspx");
                }
                #endregion
            }
            else
            {
                #region 高频彩开奖号码验证
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
                #endregion
                #region 高频彩处理
                try
                {
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
                }
                catch (Exception ex)
                {
                    JscriptMsg("手动开奖失败！" + ex.Message, "openlottery_edit.aspx");
                }
                #endregion
            }
        }

    }
}