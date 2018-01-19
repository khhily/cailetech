using CL.Dapper.Repository;
using CL.View.Entity.Game;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;

namespace CL.Game.DAL.View
{
    public class udv_ChaseListDAL : DataRepositoryBase<udv_ChaseList>
    {
        public udv_ChaseListDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="iType"></param>
        /// <param name="IsuseName">期号</param>
        /// <param name="UserName">用户名</param>
        /// <param name="SchemeNumber">方案号</param>
        /// <param name="LotteryCode">彩种</param>
        /// <param name="ChaseStatus">追号状态</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="pageSize">当页大小</param>
        /// <param name="pageIndex">当页</param>
        /// <param name="recordCount">共条</param>
        /// <param name="SumMoney">总金额</param>
        /// <returns></returns>
        public List<udv_ChaseList> QueryListByPage(int iType, string IsuseName, string UserName, string SchemeNumber, int LotteryCode, int ChaseStatus, string StartTime, string EndTime, int pageSize, int pageIndex,
            ref int recordCount, ref long SumMoney)
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" UserID>0 ");
            if (iType == 1)
                Where.AppendFormat(" and UserName like '%{0}%' ", UserName);
            else if (iType == 0)
            {
                if (LotteryCode > 0)
                    Where.AppendFormat(" and LotteryCode={0} ", LotteryCode);
                if (!string.IsNullOrEmpty(IsuseName))
                    Where.AppendFormat(" and IsuseName={0} ", IsuseName);
            }
            if (ChaseStatus == 1)
                Where.Append(" and SumIsuseNum > (BuyedIsuseNum + QuashedIsuseNum) ");
            if (ChaseStatus == 2)
                Where.Append(" and SumIsuseNum = (BuyedIsuseNum + QuashedIsuseNum) ");
            if (!string.IsNullOrEmpty(SchemeNumber))
                Where.AppendFormat(" and SchemeNumber={0} ", SchemeNumber);
            if (StartTime.Length > 0)
                Where.Append(" AND CreateTime >= '" + StartTime + "' ");
            if (EndTime.Length > 0)
                Where.Append(" AND CreateTime <= '" + EndTime + "' ");

            recordCount = base.GetIntSingle(string.Format("select count(1) from udv_ChaseList where {0}", Where.ToString()));

            var r = base.QueryMultiple("select ISNULL(sum(SchemeMoney),0) as SchemeMoney from udv_ChaseList");

            var obj = r.Read<udv_ChaseList>().FirstOrDefault();
            if (obj != null)
                SumMoney = obj.SchemeMoney;
            return base.GetListPaged(pageIndex, pageSize, Where.ToString(), "CreateTime DESC").ToList();
        }
    }
}
