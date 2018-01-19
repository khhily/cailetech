using CL.Coupons.BLL;
using CL.View.Entity.Coupons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.coupons.report
{
    public partial class reportcoupons : UI.AdminPage
    {
        protected static string dtTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = dtTime;
                BindData();
            }
        }
        protected void BindData()
        {
            if (string.IsNullOrEmpty(txtDate.Text))
            {
                dtTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");
                txtDate.Text = dtTime;
            }
            else
                dtTime = txtDate.Text;
            DateTime dt = Convert.ToDateTime(dtTime);
            //开始时间
            DateTime StartTime = Convert.ToDateTime(dt.ToString("yyyy-MM-01 00:00:00"));
            //结束时间
            DateTime EndTime = Convert.ToDateTime(Convert.ToDateTime(dt.AddMonths(1).ToString("yyyy-MM-01 00:00:00")).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            int TimeType = Convert.ToInt32(ddlTimeType.SelectedValue);
            List<udv_ReportCouponsList> array = new CouponsBLL().QueryReportCoupons(TimeType, StartTime, EndTime);
            List<udv_ReportCouponsList> arrayList = this.GenerateDateArray(StartTime, EndTime, array);

            #region 统计数据
            var Entity = new udv_ReportCouponsList();
            Entity.DTime = "统计";
            Entity.Amount = arrayList.Sum(s => s.Amount);
            Entity.ExpireAmount = arrayList.Sum(s => s.Amount);
            Entity.ExpireCount = arrayList.Sum(s => s.ExpireCount);
            Entity.GDAmount = arrayList.Sum(s => s.GDAmount);
            Entity.GDCount = arrayList.Sum(s => s.GDCount);
            Entity.GDTimeAmount = arrayList.Sum(s => s.GDTimeAmount);
            Entity.GDTimeCount = arrayList.Sum(s => s.GDTimeCount);
            Entity.MJAmount = arrayList.Sum(s => s.MJAmount);
            Entity.MJCount = arrayList.Sum(s => s.MJCount);
            Entity.NoExpireAmount = arrayList.Sum(s => s.NoExpireAmount);
            Entity.NoExpireCount = arrayList.Sum(s => s.NoExpireCount);
            Entity.ReleaseCount = arrayList.Sum(s => s.ReleaseCount);
            Entity.ReleaseMoney = arrayList.Sum(s => s.ReleaseMoney);
            arrayList.Add(Entity);
            #endregion
            this.rptList.DataSource = arrayList;
            this.rptList.DataBind();
        }

        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected List<udv_ReportCouponsList> GenerateDateArray(DateTime StartTime, DateTime EndTime, List<udv_ReportCouponsList> array)
        {
            List<udv_ReportCouponsList> arrayList = new List<udv_ReportCouponsList>();
            int StartIndex = Convert.ToInt32(StartTime.ToString("yyyyMMdd"));
            int EndIndex = Convert.ToInt32(EndTime.ToString("yyyyMMdd"));
            int index = 0;
            while (StartIndex <= EndIndex)
            {
                var Entity = new udv_ReportCouponsList();
                string DTime = Convert.ToDateTime(StartTime).AddDays(index).ToString("yyyy-MM-dd");
                Entity.DTime = DTime;
                var Entity_Ext = array.Where(w => Convert.ToDateTime(w.DTime).ToString("yyyy-MM-dd") == DTime).FirstOrDefault();
                if (Entity_Ext != null)
                {
                    Entity.Amount = Entity_Ext.Amount;
                    Entity.ExpireAmount = Entity_Ext.ExpireAmount;
                    Entity.ExpireCount = Entity_Ext.ExpireCount;
                    Entity.GDAmount = Entity_Ext.GDAmount;
                    Entity.GDCount = Entity_Ext.GDCount;
                    Entity.GDTimeAmount = Entity_Ext.GDTimeAmount;
                    Entity.GDTimeCount = Entity_Ext.GDTimeCount;
                    Entity.MJAmount = Entity_Ext.MJAmount;
                    Entity.MJCount = Entity_Ext.MJCount;
                    Entity.NoExpireAmount = Entity_Ext.NoExpireAmount;
                    Entity.NoExpireCount = Entity_Ext.NoExpireCount;
                    Entity.ReleaseCount = Entity_Ext.ReleaseCount;
                    Entity.ReleaseMoney = Entity_Ext.ReleaseMoney;
                }
                arrayList.Add(Entity);
                StartIndex += 1;
                index += 1;
            }
            return arrayList;
        }
    }
}