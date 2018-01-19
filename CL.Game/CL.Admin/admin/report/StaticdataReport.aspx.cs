using CL.Game.BLL;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Admin.admin.report
{
    public partial class StaticdataReport : UI.AdminPage
    {
        protected static long RecordBuy = 0;  //购彩总额
        protected static long RecordWin = 0;  //中奖总额
        protected static long RecordUsers = 0;  //总注册用户
        protected static long RecordRecharge = 0;  //总充值
        protected static long RecordLargess = 0;  //总赠送
        protected static long RecordWithdraw = 0;  //总提现

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDay.Text = DateTime.Now.ToString("yyyy-MM");
                BindData();
            }
        }

        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }
        public void BindData()
        {
            if (string.IsNullOrEmpty(txtDay.Text.Trim()))
                return;

            string Month = txtDay.Text.Trim();
            List<SystemStaticdataEntity> array = new List<SystemStaticdataEntity>();
            var Entitys = new SystemStaticdataBLL().QueryEntitys(Month, ref RecordBuy, ref RecordWin, ref RecordUsers, ref RecordRecharge, ref RecordLargess, ref RecordWithdraw);
            array.Add(MonthStatistics(Entitys));
            array = array.OrderBy(o => o.dateday).ToList();
            array.AddRange(Entitys);
            this.rptList.DataSource = array;
            this.rptList.DataBind();
        }
        public SystemStaticdataEntity MonthStatistics(List<SystemStaticdataEntity> Entitys)
        {
            try
            {
                SystemStaticdataEntity Entity = new SystemStaticdataEntity();
                Entity.dateday = "<span style='color:red;'>当月统计</span>";
                Entity.buy_cqssc = Entitys.Sum(s => s.buy_cqssc);
                Entity.buy_dlt = Entitys.Sum(s => s.buy_dlt);
                Entity.buy_hbsyydj = Entitys.Sum(s => s.buy_hbsyydj);
                Entity.buy_jlk = Entitys.Sum(s => s.buy_jlk);
                Entity.buy_jxk = Entitys.Sum(s => s.buy_jxk);
                Entity.buy_sdsyydj = Entitys.Sum(s => s.buy_sdsyydj);
                Entity.buy_ssq = Entitys.Sum(s => s.buy_ssq);
                Entity.largess = Entitys.Sum(s => s.largess);
                Entity.offline_recharge = Entitys.Sum(s => s.offline_recharge);
                Entity.online_recharge = Entitys.Sum(s => s.online_recharge);
                Entity.recharge = Entitys.Sum(s => s.recharge);
                Entity.users = Entitys.Sum(s => s.users);
                Entity.win_cqssc = Entitys.Sum(s => s.win_cqssc);
                Entity.win_dlt = Entitys.Sum(s => s.win_dlt);
                Entity.win_hbsyydj = Entitys.Sum(s => s.win_hbsyydj);
                Entity.win_jlk = Entitys.Sum(s => s.win_jlk);
                Entity.win_jxk = Entitys.Sum(s => s.win_jxk);
                Entity.win_sdsyydj = Entitys.Sum(s => s.win_sdsyydj);
                Entity.win_ssq = Entitys.Sum(s => s.win_ssq);
                Entity.withdraw = Entitys.Sum(s => s.withdraw);
                return Entity;
            }
            catch
            {
                return null;
            }
        }
    }
}