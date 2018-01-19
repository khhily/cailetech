using CL.Enum.Common;
using CL.Game.BLL;
using CL.SystemInfo.BLL;
using CL.SystemInfo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin
{
    public partial class center : UI.AdminPage
    {
        protected string NotBaMoney = "";
        protected string ToAgencyEnrich = "";
        protected string ToPlayerEnrich = "";
        protected int Count = 0;
        protected int BingCount = 0;
        protected string InterfaceTag = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //登录信息
                ManagerLogBLL bll = new ManagerLogBLL();
                ManagerLogEntity model1 = bll.QueryEntity(Managerinfo.UserName, 1, CaileEnums.ActionEnum.Login.ToString());
                if (model1 != null)
                {
                    //本次登录
                    litIP.Text = model1.UserIP;
                }
                ManagerLogEntity model2 = bll.QueryEntity(Managerinfo.UserName, 2, CaileEnums.ActionEnum.Login.ToString());
                if (model2 != null)
                {
                    //上一次登录
                    litBackIP.Text = model2.UserIP;
                    litBackTime.Text = model2.AddTime.ToString();
                }
                BindData();
            }
        }
        #region 全局变量
        //总数
        protected long RecordBuy = 0;
        protected long RecordWin = 0;
        protected long RecordUsers = 0;
        protected long RecordLargess = 0;
        protected long RecordRecharge = 0;
        protected long RecordWithdraw = 0;

        //每日
        protected long Day_RecordBuy = 0;
        protected long Day_RecordWin = 0;
        protected long Day_RecordUsers = 0;
        protected long Day_RecordLargess = 0;
        protected long Day_RecordRecharge = 0;
        protected long Day_RecordWithdraw = 0;

        //每周
        protected long Week_RecordBuy = 0;
        protected long Week_RecordWin = 0;
        protected long Week_RecordUsers = 0;
        protected long Week_RecordLargess = 0;
        protected long Week_RecordRecharge = 0;
        protected long Week_RecordWithdraw = 0;

        //每月
        protected long Month_RecordBuy = 0;
        protected long Month_RecordWin = 0;
        protected long Month_RecordUsers = 0;
        protected long Month_RecordLargess = 0;
        protected long Month_RecordRecharge = 0;
        protected long Month_RecordWithdraw = 0;
        #endregion
        private void BindData()
        {
            DateTime dt = DateTime.Now;
            string Month = dt.ToString("yyyy-MM");
            string Day = dt.ToString("yyyy-MM-dd");
            var Entitys = new SystemStaticdataBLL().QueryEntitys(Month, ref RecordBuy, ref RecordWin, ref RecordUsers, ref RecordRecharge, ref RecordLargess, ref RecordWithdraw);
            #region 总计
            RecordBuy = RecordBuy / 100;
            RecordWin = RecordWin / 100;
            RecordRecharge = RecordRecharge / 100;
            RecordLargess = RecordLargess / 100;
            RecordWithdraw = RecordWithdraw / 100;
            #endregion


            if (Entitys != null && Entitys.Count > 0)
            {
                #region 日统计
                var Entity_Day = Entitys.Where(w => w.dateday == Day).FirstOrDefault();
                if (Entity_Day != null)
                {
                    Day_RecordBuy = (Entity_Day.buy_cqssc + Entity_Day.buy_dlt + Entity_Day.buy_hbsyydj + Entity_Day.buy_jlk + Entity_Day.buy_jxk + Entity_Day.buy_sdsyydj + Entity_Day.buy_ssq) / 100;
                    Day_RecordWin = (Entity_Day.win_cqssc + Entity_Day.win_dlt + Entity_Day.win_hbsyydj + Entity_Day.win_jlk + Entity_Day.win_jxk + Entity_Day.win_sdsyydj + Entity_Day.win_ssq) / 100;
                    Day_RecordUsers = Entity_Day.users;
                    Day_RecordLargess = Entity_Day.largess / 100;
                    Day_RecordRecharge = Entity_Day.recharge / 100;
                    Day_RecordWithdraw = Entity_Day.withdraw / 100;
                }
                #endregion
                #region 周统计
                int week = 0;
                //周日是0
                week = (int)dt.DayOfWeek;
                DateTime WeekTime = dt.AddDays(-week);
                for (int i = 0; i <= week; i++)
                {
                    string Week_Day = WeekTime.AddDays(i).ToString("yyyy-MM-dd");
                    var Entity_Week = Entitys.Where(w => w.dateday == Week_Day).FirstOrDefault();
                    if (Entity_Week != null)
                    {
                        Week_RecordBuy += (Entity_Week.buy_cqssc + Entity_Week.buy_dlt + Entity_Week.buy_hbsyydj + Entity_Week.buy_jlk + Entity_Week.buy_jxk + Entity_Week.buy_sdsyydj + Entity_Week.buy_ssq) / 100;
                        Week_RecordWin += (Entity_Week.win_cqssc + Entity_Week.win_dlt + Entity_Week.win_hbsyydj + Entity_Week.win_jlk + Entity_Week.win_jxk + Entity_Week.win_sdsyydj + Entity_Week.win_ssq) / 100;
                        Week_RecordUsers += Entity_Week.users;
                        Week_RecordLargess += Entity_Week.largess / 100;
                        Week_RecordRecharge += Entity_Week.recharge / 100;
                        Week_RecordWithdraw += Entity_Week.withdraw / 100;
                    }
                }
                #endregion
                #region 月统计
                Month_RecordBuy = (Entitys.Sum(s => s.buy_cqssc) + Entitys.Sum(s => s.buy_dlt) + Entitys.Sum(s => s.buy_hbsyydj) + Entitys.Sum(s => s.buy_jlk) + Entitys.Sum(s => s.buy_jxk) + Entitys.Sum(s => s.buy_sdsyydj) + Entitys.Sum(s => s.buy_ssq)) / 100;
                Month_RecordWin = (Entitys.Sum(s => s.win_cqssc) + Entitys.Sum(s => s.win_dlt) + Entitys.Sum(s => s.win_hbsyydj) + Entitys.Sum(s => s.win_jlk) + Entitys.Sum(s => s.win_jxk) + Entitys.Sum(s => s.win_sdsyydj) + Entitys.Sum(s => s.win_ssq)) / 100;
                Month_RecordUsers = Entitys.Sum(s => s.users);
                Month_RecordLargess = Entitys.Sum(s => s.largess) / 100;
                Month_RecordRecharge = Entitys.Sum(s => s.recharge) / 100;
                Month_RecordWithdraw = Entitys.Sum(s => s.withdraw) / 100;

                #endregion

            }
        }
    }
}