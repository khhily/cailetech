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
    public partial class reportcdkey : UI.AdminPage
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
            string PartnerCode = ddlPartnerCode.SelectedValue;
            int TimeType = Convert.ToInt32(ddlTimeType.SelectedValue);
            List<udv_ReportCDKeyList> array = new CouponsCDKeyBLL().QueryReportCDKey(PartnerCode, TimeType, StartTime, EndTime);
            List<udv_ReportCDKeyList> arrayList = this.GenerateDateArray(StartTime, EndTime, array);

            #region 统计数据
            var Entity = new udv_ReportCDKeyList();
            Entity.DTime = "统计";
            Entity.GenerateCount = arrayList.Sum(s => s.GenerateCount);
            Entity.ExpireCount = arrayList.Sum(s => s.ExpireCount);
            Entity.ExchangerCount = arrayList.Sum(s => s.ExchangerCount);
            arrayList.Add(Entity);
            #endregion
            this.rptList.DataSource = arrayList;
            this.rptList.DataBind();
        }

        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected List<udv_ReportCDKeyList> GenerateDateArray(DateTime StartTime, DateTime EndTime, List<udv_ReportCDKeyList> array)
        {
            List<udv_ReportCDKeyList> arrayList = new List<udv_ReportCDKeyList>();
            int StartIndex = Convert.ToInt32(StartTime.ToString("yyyyMMdd"));
            int EndIndex = Convert.ToInt32(EndTime.ToString("yyyyMMdd"));
            int index = 0;
            while (StartIndex <= EndIndex)
            {
                var Entity = new udv_ReportCDKeyList();
                string DTime = Convert.ToDateTime(StartTime).AddDays(index).ToString("yyyy-MM-dd");
                Entity.DTime = DTime;
                var Entity_Ext = array.Where(w => Convert.ToDateTime(w.DTime).ToString("yyyy-MM-dd") == DTime).FirstOrDefault();
                if (Entity_Ext != null)
                {
                    Entity.GenerateCount = Entity_Ext.GenerateCount;
                    Entity.ExpireCount = Entity_Ext.ExpireCount;
                    Entity.ExchangerCount = Entity_Ext.ExchangerCount;
                }
                arrayList.Add(Entity);
                StartIndex += 1;
                index += 1;
            }
            return arrayList;
        }
    }
}