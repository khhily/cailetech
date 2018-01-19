using CL.Dapper.Repository;
using CL.Game.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;
using CL.View.Entity.Game;
using Dapper;
using System;

namespace CL.Game.DAL
{
    public class SchemesWinDAL : DataRepositoryBase<SchemesWinEntity>
    {
        public SchemesWinDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }


        /// <summary>
        /// 根据方案查询中奖数据
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public List<SchemesWinEntity> QueryEntitysBySchemeID(long SchemeID)
        {
            return base.GetList(new { SchemeID = SchemeID }, "SchemesDetailsWinID asc").ToList();
        }

        /// <summary>
        /// 系统累计中奖
        /// </summary>
        /// <returns></returns>
        public long QueryTotalWin()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(WinMoney) as WinMoney FROM dbo.CT_SchemesWin ;");
            var entity = base.QuerySingleOrDefaultT(strSql.ToString());
            return entity.WinMoney;
        }

        /// <summary>
        /// 记录中奖信息
        /// </summary>
        /// <param name="ListSql">处理的所有方案号</param>
        /// <param name="ListWinSID">中奖记录方案号</param>
        /// <param name="ListNoWinSID">没有中奖记录方案号</param>
        public void SchemesDetailsWin(List<udv_ComputeTicket> ListSql, List<long> ListWinSID, List<long> ListNoWinSID)
        {
            bool success = false;
            StringBuilder strSql = new StringBuilder();
            if (ListSql.Count <= 5000)
            {
                foreach (udv_ComputeTicket item in ListSql)
                {
                    strSql.AppendFormat(" EXEC udp_RecordWinPrize {0},{1},{2},{3},'{4}',{5},{6},'{7}',{8}; ", item.SchemeID, item.SchemeETicketsID, item.LotteryCode, item.WinCode, item.OpenNumber,
                       item.SumWinMoney, item.SumWinMoneyNoWithTax, item.Description, item.IsFirstPrize);
                }
                success = InsertBatch(strSql.ToString());
            }
            else
            {
                int i = 0;
                int len = 1;
                int num = 0;
                foreach (udv_ComputeTicket item in ListSql)
                {
                    i++;
                    strSql.AppendFormat(" EXEC udp_RecordWinPrize {0},{1},{2},{3},'{4}',{5},{6},'{7}',{8}; ", item.SchemeID, item.SchemeETicketsID, item.LotteryCode, item.WinCode, item.OpenNumber,
                       item.SumWinMoney, item.SumWinMoneyNoWithTax, item.Description, item.IsFirstPrize);

                    if (len == 500 || i == ListSql.Count)
                    {
                        if (!InsertBatch(strSql.ToString()))
                            num += 1;
                        len = 1;
                        strSql.Clear();
                    }
                    else
                        len++;
                }
                int remainder = (ListSql.Count % 5000) > 0 ? 1 : 0;
                if (num == ((ListSql.Count / 5000) + remainder))  //全部失败
                    success = false;
                else
                    success = true;
            }

            if (success)
            {
                strSql.Clear();
                //处理中奖的记录
                foreach (long item in ListWinSID)
                {
                    strSql.AppendFormat(" EXEC udp_EditOrderStatus {0},{1}; ", item, 1);
                }
                //处理没中奖的记录
                foreach (long item in ListNoWinSID)
                {
                    strSql.AppendFormat(" EXEC udp_EditOrderStatus {0},{1}; ", item, 0);
                }
                success = InsertBatch(strSql.ToString());
            }
        }
        /// <summary>
        /// 记录中奖信息
        /// </summary>
        /// <param name="ListSql">处理的所有方案号</param>
        /// <param name="ListWinSID">中奖记录方案号</param>
        /// <param name="ListNoWinSID">没有中奖记录方案号</param>
        public void SchemesDetailsWinByChaseTasks(List<udv_ComputeTicketChaseTasks> ListSql, List<long> ListWinSID, List<long> ListNoWinSID)
        {
            bool success = false;
            StringBuilder strSql = new StringBuilder();
            if (ListSql.Count <= 5000)
            {
                foreach (udv_ComputeTicketChaseTasks item in ListSql)
                {
                    strSql.AppendFormat(" EXEC udp_RecordWinPrize {0},{1},{2},{3},'{4}',{5},{6},'{7}',{8}; ", item.SchemeID, item.SchemeETicketsID, item.LotteryCode, item.WinCode, item.OpenNumber,
                       item.SumWinMoney, item.SumWinMoneyNoWithTax, item.Description, item.IsFirstPrize);
                }
                success = InsertBatch(strSql.ToString());
            }
            else
            {
                int i = 0;
                int len = 1;
                int num = 0;
                foreach (udv_ComputeTicketChaseTasks item in ListSql)
                {
                    i++;
                    strSql.AppendFormat(" EXEC udp_RecordWinPrize {0},{1},{2},{3},'{4}',{5},{6},'{7}',{8}; ", item.SchemeID, item.SchemeETicketsID, item.LotteryCode, item.WinCode, item.OpenNumber,
                       item.SumWinMoney, item.SumWinMoneyNoWithTax, item.Description, item.IsFirstPrize);

                    if (len == 500 || i == ListSql.Count)
                    {
                        if (!InsertBatch(strSql.ToString()))
                            num += 1;
                        len = 1;
                        strSql.Clear();
                    }
                    else
                        len++;
                }
                int remainder = (ListSql.Count % 5000) > 0 ? 1 : 0;
                if (num == ((ListSql.Count / 5000) + remainder))  //全部失败
                    success = false;
                else
                    success = true;
            }

            if (success)
            {
                strSql.Clear();
                //处理中奖的记录
                foreach (long item in ListWinSID)
                {
                    strSql.AppendFormat(" EXEC udp_EditOrderStatus {0},{1}; ", item, 1);
                }
                //处理没中奖的记录
                foreach (long item in ListNoWinSID)
                {
                    strSql.AppendFormat(" EXEC udp_EditOrderStatus {0},{1}; ", item, 0);
                }
                success = InsertBatch(strSql.ToString());
            }
        }
        /// <summary>
        /// 获取需要派奖的记录（双色球，大乐透等大奖不自动计算,需要手动派奖）
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_SchemesDetailsWin> QueryFirstPrizeAwardList(int LotteryCode)
        {
            string strSql = @" SELECT SchemesDetailsWinID, SchemeETicketsID, SchemeID, UserID, WinCode, WinMoney, WinMoneyNoWithTax FROM dbo.CT_SchemesWin 
                WHERE LotteryCode={0} AND isAward=0 and IsDel=0 and IsFirstPrize=0 ";
            List<udv_SchemesDetailsWin> list = new DataRepositoryBase<udv_SchemesDetailsWin>(DbConnectionEnum.CaileGame).QueryList(string.Format(strSql, LotteryCode)).ToList();
            return list;
        }
        /// <summary>
        /// 获取需要派奖的记录
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_SchemesDetailsWin> QueryAwardList(int LotteryCode)
        {
            string strSql = @" SELECT w.SchemesDetailsWinID, w.SchemeETicketsID, w.SchemeID, w.UserID, w.WinCode, w.WinMoney, w.WinMoneyNoWithTax FROM dbo.CT_SchemesWin AS w
                               INNER JOIN dbo.CT_Schemes AS s ON s.SchemeID=w.SchemeID AND s.SchemeStatus=15
                               INNER JOIN dbo.CT_Users AS u ON u.UserID=s.InitiateUserID AND u.IsRobot=0 
                               WHERE w.LotteryCode={0} AND w.isAward=0 and w.IsDel=0 ";
            List<udv_SchemesDetailsWin> list = new DataRepositoryBase<udv_SchemesDetailsWin>(DbConnectionEnum.CaileGame).QueryList(string.Format(strSql, LotteryCode)).ToList();
            return list;
        }
        /// <summary>
        /// 获取派奖信息(非追号数据)
        /// </summary>
        /// <param name="LotteryCode">彩种</param>
        /// <returns></returns>
        public List<udv_SchemesDetailsWin> QueryAwardListByNotChase(int LotteryCode)
        {
            string strSql = @" SELECT se.SchemeETicketsID,se.WinMoney,se.WinMoney AS WinMoneyNoWithTax,se.SchemeID FROM CT_SchemeETickets AS se 
                               INNER JOIN dbo.CT_Schemes AS s ON s.SchemeID=se.SchemeID AND s.BuyType!=1 AND se.TicketStatus=2 
                               INNER JOIN dbo.CT_Isuses AS i ON i.IsuseID = s.IsuseID AND i.IsOpened=1 AND i.IsuseState=4 AND i.OpenNumber<>'' 
                               WHERE s.LotteryCode={0} 
                               GROUP BY se.SchemeETicketsID,se.WinMoney,se.SchemeID ";
            List<udv_SchemesDetailsWin> list = new DataRepositoryBase<udv_SchemesDetailsWin>(DbConnectionEnum.CaileGame).QueryList(string.Format(strSql, LotteryCode)).ToList();
            return list;
        }

        /// <summary>
        /// 获取派奖信息(非追号数据)
        /// </summary>
        /// <param name="LotteryCode">彩种</param>
        /// <returns></returns>
        public List<udv_SchemesDetailsWin> QueryAwardListByChase(int LotteryCode)
        {
            string strSql = @" SELECT se.SchemeETicketsID,se.WinMoney,se.WinMoney AS WinMoneyNoWithTax,se.SchemeID,se.ChaseTaskDetailsID FROM CT_SchemeETickets AS se 
                               INNER JOIN CT_Schemes AS s ON s.SchemeID=se.SchemeID AND s.BuyType=1 AND se.TicketStatus=2 
                               INNER JOIN dbo.CT_ChaseTaskDetails AS cd ON cd.SchemeID = s.SchemeID AND cd.ID = se.ChaseTaskDetailsID AND cd.QuashStatus = 0 AND cd.IsExecuted = 1 
                               INNER JOIN dbo.CT_Isuses AS i ON i.IsuseID = cd.IsuseID AND i.IsOpened=1 AND i.IsuseState=4 AND i.OpenNumber<>''  
                               WHERE s.LotteryCode={0} 
                               GROUP BY se.SchemeETicketsID,se.WinMoney,se.SchemeID,se.ChaseTaskDetailsID ";
            List<udv_SchemesDetailsWin> list = new DataRepositoryBase<udv_SchemesDetailsWin>(DbConnectionEnum.CaileGame).QueryList(string.Format(strSql, LotteryCode)).ToList();
            return list;
        }

        /// <summary>
        /// 报表：中奖查询统计
        /// </summary>
        /// <param name="StartTime">起止时间</param>
        /// <param name="EndTime">岂止时间_止</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="UserName">用户账号</param>
        /// <param name="Mobile">用户手机</param>
        /// <param name="LotteryCode">彩种</param>
        /// <param name="SchemeNumber">订单编号</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="RecordAmount">投注总额</param>
        /// <param name="RecordCount">查询总数</param>
        /// <returns></returns>
        public List<udv_WinAwardReport> QueryWinAwardReport(DateTime StartTime, DateTime EndTime, long UserID, string UserName, string Mobile, int LotteryCode, string SchemeNumber, int PageIndex, int PageSize, ref long RecordAmount,ref long RecordWin, ref int RecordCount)
        {
            var para = new DynamicParameters();
            para.Add("@StartTime", StartTime);
            para.Add("@EndTime", EndTime);
            para.Add("@UserID", UserID);
            para.Add("@UserName", UserName);
            para.Add("@Mobile", Mobile);
            para.Add("@LotteryCode", LotteryCode);
            para.Add("@SchemeNumber", SchemeNumber);
            para.Add("@PageIndex", PageIndex);
            para.Add("@PageSize", PageSize);
            para.Add("@RecordAmount", RecordAmount, DbType.Int64, ParameterDirection.Output);
            para.Add("@RecordWin", RecordWin, DbType.Int64, ParameterDirection.Output);
            para.Add("@RecordCount", RecordCount, DbType.Int32, ParameterDirection.Output);
            var Entitys = new DataRepositoryBase<udv_WinAwardReport>(DbConnectionEnum.CaileGame).QueryList("udp_QueryWinAwardReport", para, CommandType.StoredProcedure).ToList();
            RecordAmount = para.Get<long>("@RecordAmount");
            RecordWin = para.Get<long>("@RecordWin");
            RecordCount = para.Get<int>("@RecordCount");
            return Entitys;
        }
    }
}
