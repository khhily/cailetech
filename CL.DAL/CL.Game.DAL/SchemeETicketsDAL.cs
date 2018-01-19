using CL.Dapper.Repository;
using CL.Game.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;
using Dapper;
using CL.View.Entity.Interface;
using CL.View.Entity.ElectronicTicket;
using CL.View.Entity.Game;
using System;

namespace CL.Game.DAL
{
    public class SchemeETicketsDAL : DataRepositoryBase<SchemeETicketsEntity>
    {
        public SchemeETicketsDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {

        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeETicketsID"></param>
        /// <returns></returns>
        public SchemeETicketsEntity QueryEntity(long SchemeETicketsID)
        {
            return base.Get(SchemeETicketsID);
        }
        /// <summary>
        /// 根据方案编号查询电子票
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public List<SchemeETicketsEntity> QueryEntitysBySchemeID(long SchemeID)
        {
            return base.GetList(new { SchemeID = SchemeID }, "SchemeETicketsID asc").ToList();
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="SchemeETicketsID"></param>
        /// <returns></returns>
        public List<SchemeETicketsEntity> QueryEntitysBySchemeIDSDID(long SchemeID, long ChaseTaskDetailsID)
        {
            return base.GetList(new { SchemeID = SchemeID, ChaseTaskDetailsID = ChaseTaskDetailsID }, "SchemeETicketsID asc").ToList();
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="SchemeETicketsID"></param>
        /// <returns></returns>
        public List<SchemeETicketsEntity> QueryEntitysBySchemeIDSDID(long SchemeID, long ChaseTaskDetailsID, int PageIndex, int PageSize)
        {
            return base.GetListPaged(PageIndex, PageSize, string.Format(" SchemeID={0} and ChaseTaskDetailsID={1} ", SchemeID, ChaseTaskDetailsID), "SchemeETicketsID asc").ToList();
        }
        /// <summary>
        /// 撤单处理
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="IsSystemQuash">是否系统撤单</param>
        /// <param name="ReturnValue"></param>
        /// <param name="ReturnDescription"></param>
        /// <returns></returns>
        public bool QuashScheme(long SchemeID, bool IsSystemQuash, ref string ReturnDescription)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@SchemeID", SchemeID, DbType.Int64, null, 8);
            Parms.Add("@IsSystemQuash", IsSystemQuash, DbType.Boolean, null, 1);
            Parms.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            Parms.Add("@ReturnDescription", null, DbType.String, ParameterDirection.Output, 100);

            int rows = base.Execute("udp_RevokeScheme", Parms);
            ReturnDescription = Parms.Get<string>("@ReturnDescription");
            return rows > 0;
        }

        /// <summary>
        /// 更新出票状态
        /// </summary>
        /// <returns></returns>
        public bool HandleOutTicket(List<udv_OutTicketEntites> ListOutTicker, List<long> SchemeIDList)
        {
            if (ListOutTicker.Count == 0) return true;

            StringBuilder SqlPara = new StringBuilder();
            foreach (udv_OutTicketEntites item in ListOutTicker)
            {
                if (item.Status == 2)
                    SqlPara.AppendFormat(" EXEC udp_DealOutTicket {0},{1},'{2}'; ", item.SchemeETicketID, 2, item.TicketID);
                else if (item.Status == 0 || item.Status == 6)
                    SqlPara.AppendFormat(string.Format(" EXEC udp_DealOutTicket {0},{1},'{2}'; ", item.SchemeETicketID, 4, item.TicketID));
            }
            //更新方案号出票状态
            foreach (long item in SchemeIDList)
            {
                SqlPara.AppendFormat(" EXEC udp_UpdateSchemesOutTicketStatus {0}; ", item);
            }
            return InsertBatch(SqlPara.ToString());
        }
        /// <summary>
        /// 自动算奖派奖
        /// </summary>
        public bool AutomaticComputeWin(List<udv_WinInfoEntites> SqlPara, List<long> SchemeIDList, string InterfaceType)
        {
            StringBuilder StrSql = new StringBuilder();
            foreach (udv_WinInfoEntites item in SqlPara)
            {
                StrSql.AppendFormat(" EXEC udp_AutoCalculatePrize {0},{1},{2},{3},'{4}'; ", item.SchemeETicketID, item.Status, item.PrebonusValue, item.BonusValue, InterfaceType);
            }
            //更新方案号中奖状态
            foreach (long item in SchemeIDList)
            {
                StrSql.AppendFormat(" EXEC udp_EditOrderStatus {0},{1}; ", item, 0);
            }
            return InsertBatch(StrSql.ToString());
        }

        /// <summary>
        /// 电子票入库
        /// </summary>
        public bool TicketsStorage(long SchemeID, long ChaseTaskDetailsID, List<udv_ts> ttb)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@SchemeID", SchemeID);
            Parms.Add("@ChaseTaskDetailsID", ChaseTaskDetailsID);
            Parms.Add("@IsRobot", false);
            Parms.Add("@TicketTable", Tools.Common.XmlHelper.Serializer(ttb.GetType(), ttb));
            int b = base.Execute("udp_TicketsStorage", Parms);
            return b > 0 ? true : false;
        }
        /// <summary>
        /// 电子票入库
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="ChaseTaskDetailsID"></param>
        /// <param name="SqlPara"></param>
        /// <returns></returns>
        public bool TicketsStorage_Robot(long SchemeID, long ChaseTaskDetailsID, List<udv_ts> ttb)
        {

            var Parms = new DynamicParameters();
            Parms.Add("@SchemeID", SchemeID);
            Parms.Add("@ChaseTaskDetailsID", ChaseTaskDetailsID);
            Parms.Add("@IsRobot", true);
            Parms.Add("@TicketTable", Tools.Common.XmlHelper.Serializer(ttb.GetType(), ttb));
            int b = base.Execute("udp_TicketsStorage", Parms);
            return b > 0 ? true : false;
        }
        /// <summary>
        /// 电子票投注
        /// </summary>
        /// <returns></returns>
        public bool TicketBetting(List<udv_tb> tb, long SchemeID, long ChaseTaskDetailsID)
        {
            int ReturnValue = 0;
            string ReturnDescription = string.Empty;
            var Parms = new DynamicParameters();
            Parms.Add("@SchemeID", SchemeID);
            Parms.Add("@ChaseTaskDetailsID", ChaseTaskDetailsID);
            Parms.Add("@TicketTable", Tools.Common.XmlHelper.Serializer(tb.GetType(), tb));
            Parms.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            Parms.Add("@ReturnDescription", null, DbType.String, ParameterDirection.Output, 100);
            int rows = base.Execute("udp_TicketBetting", Parms);
            ReturnValue = Parms.Get<int>("@ReturnValue");
            ReturnDescription = Parms.Get<string>("@ReturnDescription");
            return rows > 0;
        }

        /// <summary>
        /// 投注失败撤单
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="ErrorDescription"></param>
        /// <returns></returns>
        public bool SchemeETicketsWithdrawal(long SchemeID, string ErrorDescription)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@SchemeID", SchemeID, DbType.Int64, null, 8);
            Parms.Add("@HandleDescribe", ErrorDescription, DbType.String, null, 100);
            Parms.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            Parms.Add("@ReturnDescription", null, DbType.String, ParameterDirection.Output, 100);

            int rows = base.Execute("udp_BettingRevoke", Parms);
            return rows > 0;
        }

        /// <summary>
        /// 出票查询统计
        /// </summary>
        /// <param name="StartTime">起止时间_起</param>
        /// <param name="EndTime">岂止时间_止</param>
        /// <param name="LotteryCode">彩种</param>
        /// <param name="SourceID">出票商</param>
        /// <param name="SchemeNumber">订单编号</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="RecordAmount">订单总额</param>
        /// <param name="RecordWin">中奖总额</param>
        /// <param name="RecordCount">查询总数</param>
        /// <returns></returns>
        public List<udv_OutTicketReport> QueryOutTicketReport(DateTime StartTime, DateTime EndTime, int LotteryCode, int SourceID, string SchemeNumber, int PageIndex, int PageSize, ref long RecordAmount, ref long RecordWin, ref int RecordCount)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@StartTime", StartTime);
            Parms.Add("@EndTime", EndTime);
            Parms.Add("@LotteryCode", LotteryCode);
            Parms.Add("@SourceID", SourceID);
            Parms.Add("@SchemeNumber", SchemeNumber);
            Parms.Add("@PageIndex", PageIndex);
            Parms.Add("@PageSize", PageSize);
            Parms.Add("@RecordAmount", RecordAmount, DbType.Int64, ParameterDirection.Output);
            Parms.Add("@RecordWin", RecordWin, DbType.Int64, ParameterDirection.Output);
            Parms.Add("@RecordCount", RecordCount, DbType.Int32, ParameterDirection.Output);
            var Entitys = new DataRepositoryBase<udv_OutTicketReport>(DbConnectionEnum.CaileGame).QueryList("udp_QueryOutTicketReport", Parms, CommandType.StoredProcedure).ToList();
            RecordAmount = Parms.Get<long>("@RecordAmount");
            RecordWin = Parms.Get<long>("@RecordWin");
            RecordCount = Parms.Get<int>("@RecordCount");
            return Entitys;
        }

        /// <summary>
        /// 优化查询中奖电子票sql
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public List<cv_Ticket> QueryWinMoneyListBySchemeID(long SchemeID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(WinMoney) AS WinMoney,ChaseTaskDetailsID,TicketStatus FROM CT_SchemeETickets WHERE SchemeID=@SchemeID GROUP BY ChaseTaskDetailsID,TicketStatus  ;");
            var Parms = new DynamicParameters();
            Parms.Add("@SchemeID", SchemeID);
            return new DataRepositoryBase<cv_Ticket>(DbConnectionEnum.CaileGame).QueryList(strSql.ToString(), Parms).ToList();
        }
        #region 管理后台查询某期重新派奖数据
        /// <summary>
        /// 查询手动返奖数据
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <returns></returns>
        public List<udv_SchemeETicketAward> QuerySchemeETicketAward(int LotteryCode, string IsuseName)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(" WITH tab AS ( ");
            sqlStr.Append(" 	SELECT u.UserID,se.TicketStatus,i.LotteryCode,i.IsuseName,se.SchemeETicketsID,s.SchemeNumber,u.UserName,u.UserMobile,se.TicketMoney,se.Multiple,se.Number,se.Ticket,se.Identifiers  ");
            sqlStr.Append(" 	FROM dbo.CT_SchemeETickets AS se ");
            sqlStr.Append(" 	INNER JOIN dbo.CT_Schemes AS s ON s.SchemeID = se.SchemeID AND s.BuyType != 1 ");
            sqlStr.Append(" 	INNER JOIN dbo.CT_Isuses AS i ON i.IsuseID = s.IsuseID ");
            sqlStr.Append(" 	INNER JOIN dbo.CT_Users AS u ON u.UserID = s.InitiateUserID ");
            sqlStr.AppendFormat(" 	WHERE i.LotteryCode={0} AND i.IsuseName='{1}' ", LotteryCode, IsuseName);
            sqlStr.Append(" 	UNION ALL ");
            sqlStr.Append("     SELECT u.UserID,se.TicketStatus,i.LotteryCode,i.IsuseName,se.SchemeETicketsID,s.SchemeNumber,u.UserName,u.UserMobile,se.TicketMoney,se.Multiple,se.Number,se.Ticket,se.Identifiers  ");
            sqlStr.Append(" 	FROM dbo.CT_SchemeETickets AS se  ");
            sqlStr.Append(" 	INNER JOIN dbo.CT_Schemes AS s ON s.SchemeID = se.SchemeID AND s.BuyType = 1  ");
            sqlStr.Append(" 	INNER JOIN dbo.CT_ChaseTaskDetails AS cd ON cd.SchemeID = se.SchemeID AND cd.ID = se.ChaseTaskDetailsID ");
            sqlStr.Append(" 	INNER JOIN dbo.CT_Isuses AS i ON i.IsuseID = cd.IsuseID ");
            sqlStr.Append(" 	INNER JOIN dbo.CT_Users AS u ON u.UserID = s.InitiateUserID ");
            sqlStr.AppendFormat(" 	WHERE i.LotteryCode={0} AND i.IsuseName='{1}' ", LotteryCode, IsuseName);
            sqlStr.Append(" ) SELECT tab.* FROM tab WHERE tab.SchemeETicketsID NOT IN(SELECT CONVERT(BIGINT, RelationID) FROM dbo.CT_UsersRecord WHERE UserID = tab.UserID AND TradeType = 5) ");
            return new DataRepositoryBase<udv_SchemeETicketAward>(DbConnectionEnum.CaileGame).QueryList(sqlStr.ToString()).ToList();
        }
        /// <summary>
        /// 手动算奖
        /// </summary>
        /// <param name="Entitys"></param>
        /// <returns></returns>
        public int ReplenishAward(List<udv_Award> Entitys)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@AwardTable", Entitys, DbType.Xml);
            return base.Execute("udp_ReplenishAward", Parms);
        }
        #endregion

    }
}
