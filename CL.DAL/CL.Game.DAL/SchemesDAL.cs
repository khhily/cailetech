using CL.Dapper.Repository;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;
using Dapper;
using CL.View.Entity.Game;
using CL.Tools.Common;
using CL.Json.Entity.WebAPI;
using CL.View.Entity.Coupons;
using CL.Enum.Common.Type;

namespace CL.Game.DAL
{
    public class SchemesDAL : DataRepositoryBase<SchemesEntity>
    {
        public SchemesDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(SchemesEntity entity)
        {
            return base.Update(entity);
        }

        /// <summary>
        /// 更新发送状态
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public bool ModifySendOut(long SchemeID)
        {
            using (var tran = base.db.BeginTransaction())
            {
                try
                {
                    var Entity = base.Get(SchemeID, tran);
                    Entity.IsSendOut = true;
                    base.Update(Entity, tran);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public SchemesEntity QueryEntity(long SchemeID)
        {
            return base.Get(SchemeID);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public bool Exists(long SchemeID)
        {
            return base.RecordCount(new { SchemeID = SchemeID }) > 0;
        }
        /// <summary>
        /// 设置中奖方案
        /// </summary>
        /// <returns></returns>
        public bool SetWinMoney(long SchemeID, string AllValues, ref string ReturnDescription)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@SchemeID", SchemeID, DbType.Int64, null, 8);
            Parms.Add("@AllValues", AllValues, DbType.String, null, 512);
            Parms.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            Parms.Add("@ReturnDescription", null, DbType.String, ParameterDirection.Output, 100);

            int rows = base.Execute("udp_SetWinPrize", Parms);
            ReturnDescription = Parms.Get<string>("@ReturnDescription");

            return rows == 0;
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<udv_SchemesMain> QueryListByPage(int LotteryCode, int iState, int iWinState, string StartTime, string EndTime, string IsuseName, string SchemeNumber, string UserName, string orderby, int pageSize, int pageIndex,
            ref int recordCount, ref long SumMoney, ref long WinSumMoney)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" where SchemeID>0 ");

            if (LotteryCode > 0)
                strWhere.Append(" and LotteryCode = " + LotteryCode);
            if (IsuseName.Trim() != "")
                strWhere.Append(" and IsuseName = '" + IsuseName.Trim() + "' ");
            if (StartTime.Trim() != "")
                strWhere.Append(" and CreateTime >= '" + StartTime.Trim() + "' ");

            if (EndTime.Trim() != "")
                strWhere.Append(" and CreateTime <= '" + EndTime.Trim() + "' ");

            if (SchemeNumber.Trim() != "")
                strWhere.Append(" and SchemeNumber = '" + SchemeNumber.Trim() + "' ");
            if (UserName.Trim() != "")
                strWhere.Append(" and UserName = '" + UserName.Trim() + "' ");

            switch (iState) //出票状态
            {
                case 2:
                    strWhere.Append(" and SchemeStatus=0 ");
                    break;
                case 3:
                    strWhere.Append(" and SchemeStatus in () ");
                    break;
            }
            switch (iWinState)  //开奖状态
            {
                case 1:
                    strWhere.Append(" and SchemeStatus in(0, 4, 6, 8) ");
                    break;
                case 2:
                    strWhere.Append(" and SchemeStatus = 18 ");
                    break;
                case 3:
                    strWhere.Append(" and WinMoney > 0 and SchemeStatus=14 ");
                    break;
                case 4:
                    strWhere.Append(" and SchemeStatus in(2, 10, 12) ");
                    break;
            }

            string strSql = " SELECT * FROM dbo.udv_Schemes " + strWhere.ToString();
            string strSql1 = " SELECT COUNT(1) AS RecordCount FROM dbo.udv_Schemes " + strWhere.ToString();
            string strSql2 = " SELECT ISNULL(SUM(SchemeMoney),0) as SchemeMoney FROM dbo.udv_Schemes " + strWhere.ToString();
            string strSql3 = " SELECT ISNULL(SUM(WinMoneyNoWithTax),0) as WinMoneyNoWithTax FROM dbo.udv_Schemes " + strWhere.ToString();

            SqlMapper.GridReader grid = base.QueryMultiple(strSql1 + strSql2 + strSql3);
            recordCount = grid.Read<int>().FirstOrDefault();
            SumMoney = grid.Read<long>().FirstOrDefault();
            WinSumMoney = grid.Read<long>().FirstOrDefault();
            grid.Dispose();

            SqlMapper.GridReader grid1 = base.QueryMultiple(Tools.Common.PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql, orderby));
            List<udv_SchemesMain> list = grid1.Read<udv_SchemesMain>().ToList();
            grid1.Dispose();
            return list;
        }
        /// <summary>
        /// 投注
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        /// <param name="Tasksls"></param>
        /// <param name="ChaseTaskDetailsID"></param>
        /// <returns></returns>
        public long SubmitOrder(SchemesEntity model, List<udv_Parameter> list, udv_Tasks Tasksls, List<udv_Coupons> Coupons, int PaymentType, long Gold, ref long ChaseTaskDetailsID)
        {
            var para = new DynamicParameters();
            para.Add("@UserID", model.InitiateUserID);
            para.Add("@LotteryCode", model.LotteryCode);
            para.Add("@IsuseName", model.IsuseName);
            para.Add("@SchemeNumber", model.SchemeNumber);
            para.Add("@SchemeMoney", model.SchemeMoney);
            para.Add("@LotteryNumber", model.LotteryNumber);
            para.Add("@IsSplit", model.IsSplit);
            para.Add("@BuyType", model.BuyType);
            para.Add("@RoomCode", model.RoomCode);
            para.Add("@SchemesDetailTables", XmlHelper.Serializer(list.GetType(), list));
            //追号信息
            para.Add("@TasksBeginTime", Tasksls == null ? "" : DateTime.ParseExact(Tasksls.BeginTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm:ss"));
            para.Add("@TasksEndTime", Tasksls == null ? "" : DateTime.ParseExact(Tasksls.EndTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm:ss"));
            para.Add("@IsuseCount", Tasksls == null ? 0 : Tasksls.IsuseCount);
            para.Add("@Stops", Tasksls == null ? -1 : Tasksls.Stops);
            para.Add("@TasksDetailsTables", Tasksls == null ? null : XmlHelper.Serializer(Tasksls.Data.GetType(), Tasksls.Data));

            para.Add("@CouponsTables", Coupons == null ? null : XmlHelper.Serializer(Coupons.GetType(), Coupons));
            para.Add("@PaymentType", PaymentType);
            para.Add("@Gold", Gold);
            para.Add("@ChaseTaskDetailsID", null, DbType.Int64, ParameterDirection.Output, 8);
            object aa = base.ExecScalar("udp_SubmitBetting", para);
            ChaseTaskDetailsID = para.Get<long>("@ChaseTaskDetailsID");
            return Convert.ToInt64(aa);
        }

        /// <summary>
        /// 方案跟单
        /// </summary>
        /// <returns></returns>
        public long FollowBetting(long UserID, long SchemeID, string IsuseName, string SchemeNumber)
        {
            var para = new DynamicParameters();
            para.Add("@UserID", UserID);
            para.Add("@SchemeID", SchemeID);
            para.Add("@SchemeNumber", SchemeNumber);
            para.Add("@IsuseName", IsuseName);
            object aa = base.ExecScalar("udp_FollowBetting", para);
            return Convert.ToInt64(aa);
        }

        /// <summary>
        /// 处理期号截止没有拆票投注的方案
        /// 撤单发短信
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public List<udv_RevokeSchemes> IsuseStopRevokeSchemes(int LotteryCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@LotteryCode", LotteryCode, DbType.Int32, null, 4);
            return new DataRepositoryBase<udv_RevokeSchemes>(DbConnectionEnum.CaileGame).QueryList("udp_IsuseStopRevokeSchemes", Parms, CommandType.StoredProcedure).ToList();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <returns></returns>
        public List<SchemesEntity> QueryEntity(int LotteryCode, string IsuseName)
        {
            return this.GetList(new { LotteryCode = LotteryCode, IsuseName = IsuseName }, "SchemeID desc").ToList();
        }
        //存储过程
        /// <summary>
        /// 查询方案记录列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="IsWin"></param>
        /// <param name="IsChase"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<SchemeRecordEntity> QuerySchemeRecord(long UserCode, int IsWin, int IsChase, int PageIndex, int PageSize)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@UserID", UserCode, DbType.Int64, null, 4);
            Parms.Add("@IsWin", IsWin, DbType.Int32, null, 4);
            Parms.Add("@IsChase", IsChase, DbType.Int32, null, 4);
            Parms.Add("@PageIndex", PageIndex, DbType.Int32, null, 4);
            Parms.Add("@PageSize", PageSize, DbType.Int32, null, 4);
            return base.db.Query<udv_SchemeRecordEntity>("udp_QuerySchemeRecord", Parms, null, true, null, CommandType.StoredProcedure).Select(s => new SchemeRecordEntity()
            {
                Amount = s.Amount,
                BuyType = s.BuyType,
                LotteryName = s.LotteryName,
                OrderCode = s.OrderCode,
                OrderStatus = s.OrderStatus,
                WinMoney = s.WinMoney,
                OrderTime = s.OrderTime.ToString("yyyyMMddHHmmss"),
                IsuseNum = s.BuyType == (byte)BuyTypeEnum.BuyChase ? string.Empty : s.IsuseNum
            }).ToList();
        }

        /// <summary>
        /// 报表：彩票投注查询
        /// </summary>
        /// <param name="StartTime">起止时间</param>
        /// <param name="EndTime">起止时间</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="UserName">用户账号</param>
        /// <param name="LotteryCode">彩种</param>
        /// <param name="SchemeStatus">状态</param>
        /// <param name="SchemeNumber">订单编号</param>
        /// <param name="PrintOutType">出票商</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="RecordAmount">投注金额</param>
        /// <param name="RecordCount">查询条数</param>
        /// <returns></returns>
        public List<udv_BuyLotReport> QueryBuyLotReport(DateTime StartTime, DateTime EndTime, long UserID, string UserName, int LotteryCode, int SchemeStatus, string SchemeNumber, int PrintOutType, int PageIndex, int PageSize, ref long RecordAmount, ref int RecordCount)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@StartTime", StartTime);
            Parms.Add("@EndTime", EndTime);
            Parms.Add("@UserID", UserID);
            Parms.Add("@UserName", UserName);
            Parms.Add("@LotteryCode", LotteryCode);
            Parms.Add("@SchemeStatus", SchemeStatus);
            Parms.Add("@SchemeNumber", SchemeNumber);
            Parms.Add("@PrintOutType", PrintOutType);
            Parms.Add("@PageIndex", PageIndex);
            Parms.Add("@PageSize", PageSize);
            Parms.Add("@RecordAmount", RecordAmount, DbType.Int64, ParameterDirection.Output);
            Parms.Add("@RecordCount", RecordCount, DbType.Int32, ParameterDirection.Output);
            var Entitys = new DataRepositoryBase<udv_BuyLotReport>(DbConnectionEnum.CaileGame).QueryList("udp_QueryBuyLotReport", Parms, CommandType.StoredProcedure).ToList();
            RecordAmount = Parms.Get<long>("@RecordAmount");
            RecordCount = Parms.Get<int>("@RecordCount");
            return Entitys;
        }
    }
}
