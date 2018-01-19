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

namespace CL.Game.DAL
{
    public class IsusesDAL : DataRepositoryBase<IsusesEntity>
    {
        public IsusesDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long IsuseID)
        {
            return base.RecordCount(new { IsuseID = IsuseID }) == 0 ? false : true;
        }

        /// <summary>
        /// 获取彩种最近一次的期号
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public IsusesEntity QueryEntitysLastIsues(int LotteryCode)
        {
            return base.Get(new { LotteryCode = LotteryCode }, " IsuseID desc ");
        }
        /// <summary>
        /// 获取彩种最近一次的期号
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public IsusesEntity QueryEntity(long IsuseID)
        {
            return base.Get(IsuseID);
        }
        /// <summary>
        /// 根据彩种编号查询期号集
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public IsusesEntity QueryEntitysByLotteryCode(int LotteryCode, string IsuseName)
        {
            return base.Get(new { LotteryCode = LotteryCode, IsuseName = IsuseName }, " IsuseID desc ");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string IsuseName, int LotteryCode)
        {
            return base.RecordCount(new { IsuseName = IsuseName, LotteryCode = LotteryCode }) == 0 ? false : true;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(IsusesEntity entity)
        {
            return base.Update(entity);
        }
        /// <summary>
        /// 插入对象集
        /// </summary>
        /// <param name="Entitys"></param>
        /// <returns></returns>
        public bool InsertEntity(List<IsusesEntity> Entitys)
        {
            using (IDbTransaction tran = base.db.BeginTransaction())
            {
                try
                {
                    foreach (var item in Entitys)
                        base.Insert_Long(item, tran);
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return false;
                }
            }
        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<IsusesEntity> QueryListByPage(int LotteryCode, string date, string strName, string orderby, int pageSize, int pageIndex, ref int recordCount)
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" 1 = @Val ");
            object Paramters = new { Val = 1 };
            if (LotteryCode != 801 || LotteryCode != 901)
            {
                Where.Append(" AND StartTime >= @StartTime ");
                Where.Append(" AND EndTime <= @EndTime ");
                Paramters = new { Val = 1, StartTime = string.Format("{0} 00:00:00", date), EndTime = string.Format("{0} 23:59:59", date) };
                if (LotteryCode > 0)
                {
                    Where.Append(" AND LotteryCode = @LotteryCode ");
                    Paramters = new { Val = 1, StartTime = string.Format("{0} 00:00:00", date), EndTime = string.Format("{0} 23:59:59", date), LotteryCode = LotteryCode };
                }
                if (!string.IsNullOrEmpty(strName))
                {
                    Where.Append(" AND IsuseName = @IsuseName ");
                    if (LotteryCode > 0)
                        Paramters = new { Val = 1, StartTime = string.Format("{0} 00:00:00", date), EndTime = string.Format("{0} 23:59:59", date), LotteryCode = LotteryCode, IsuseName = strName };
                    else
                        Paramters = new { Val = 1, StartTime = string.Format("{0} 00:00:00", date), EndTime = string.Format("{0} 23:59:59", date), IsuseName = strName };

                }
            }
            else
            {
                if (LotteryCode > 0)
                {
                    Where.Append(" AND LotteryCode = @LotteryCode ");
                    Paramters = new { Val = 1, LotteryCode = LotteryCode };
                }
                if (!string.IsNullOrEmpty(strName))
                {
                    Where.Append(" AND IsuseName = @IsuseName ");
                    if (LotteryCode > 0)
                        Paramters = new { Val = 1, LotteryCode = LotteryCode, IsuseName = strName };
                    else
                        Paramters = new { Val = 1, IsuseName = strName };

                }
            }

            recordCount = base.GetIntSingle(string.Format("select count(1) from CT_Isuses where {0}", Where.ToString()), Paramters);
            return base.GetListPaged(pageIndex, pageSize, Where.ToString(), orderby, Paramters).ToList();

        }
        /// <summary>
        /// 获取最新开售期号
        /// </summary>
        /// <returns></returns>
        public IsusesEntity QueryNewSaleIsuses(int LotteryCode)
        {
            string strSql = @" SELECT TOP 1 * FROM dbo.CT_Isuses WHERE LotteryCode=@LotteryCode 
                AND DATEDIFF(d,StartTime,GETDATE())=0 AND EndTime < GETDATE() AND OpenNumber = ''
            ORDER BY IsuseID DESC ";

            var para = new DynamicParameters();
            para.Add("@LotteryCode", LotteryCode);
            return base.QuerySingleOrDefaultT(strSql.ToString(), para);
        }
        /// <summary>
        /// 获取封盘期号
        /// 只使用低频彩
        /// </summary>
        /// <returns></returns>
        public IsusesEntity QueryEntertainedIsuses(int LotteryCode)
        {
            //string strSql = string.Format("select IsuseName,EndTime from ct_isuses where lotterycode={0} and convert(date,'{1}')=convert(date,EndTime) ;", LotteryCode, DateTime.Now.ToString("yyyy-MM-dd"));
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select IsuseName,DATEADD(MINUTE,(-l.AdvanceEndTime),EndTime) AS EndTime from ct_isuses AS i ");
            strSql.Append(" INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = i.LotteryCode ");
            strSql.AppendFormat(" WHERE i.lotterycode={0} and convert(date,'{1}')=convert(date,DATEADD(MINUTE,(-l.AdvanceEndTime),EndTime)) ; ", LotteryCode, DateTime.Now.ToString("yyyy-MM-dd"));
            var para = new DynamicParameters();
            para.Add("@LotteryCode", LotteryCode);
            return base.QuerySingleOrDefaultT(strSql.ToString(), para);
        }


        /// <summary>
        /// 获取遗漏的期号记录
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<IsusesEntity> QueryNumEmpty(int LotteryCode, string DateDay)
        {
            var para = new DynamicParameters();
            para.Add("@LotteryCode", LotteryCode);
            para.Add("@DateDay", DateDay);
            List<IsusesEntity> list = new DataRepositoryBase<IsusesEntity>(DbConnectionEnum.CaileGame).QueryList("udp_OmissionRecord", para, CommandType.StoredProcedure).ToList();
            return list;
        }
        /// <summary>
        /// 查询当期开期数据
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public udv_IsuseInfo QueryLotteryCurrIsuses(int LotteryCode)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(" SELECT a.*,b.IntervalType,b.AdvanceEndTime,b.PresellTime FROM dbo.CT_Isuses a");
            //sb.Append(" JOIN [dbo].[CT_Lotteries] b ON a.LotteryCode = b.LotteryCode");
            //sb.AppendFormat(" WHERE a.LotteryCode = {0} AND DATEADD(MINUTE,(-b.PresellTime),StartTime) <= GETDATE() AND DATEADD(MINUTE,(-b.AdvanceEndTime),EndTime) >= GETDATE();", LotteryCode);
            sb.Append(" SELECT TOP 1 a.*,b.IntervalType,b.AdvanceEndTime,b.PresellTime FROM dbo.CT_Isuses a");
            sb.Append(" JOIN [dbo].[CT_Lotteries] b ON a.LotteryCode = b.LotteryCode");
            sb.AppendFormat(" WHERE a.LotteryCode = {0} AND DATEADD(MINUTE,(-b.PresellTime),StartTime) <= GETDATE() AND DATEADD(MINUTE,(-b.AdvanceEndTime),EndTime) >= GETDATE() ORDER BY a.IsuseName ASC ;", LotteryCode);
            SqlMapper.GridReader grid = new DataRepositoryBase<udv_IsuseInfo>(DbConnectionEnum.CaileGame).QueryMultiple(sb.ToString());
            return grid.Read<udv_IsuseInfo>().FirstOrDefault();
        }
        /// <summary>
        /// 查询最近一期开奖数据(预售)
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public udv_IsuseInfo QueryLotteryCurrIsuses(int LotteryCode, string IsuseName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT top 1 a.*,b.IntervalType,b.AdvanceEndTime,b.PresellTime FROM dbo.CT_Isuses a ");
            sb.Append(" JOIN [dbo].[CT_Lotteries] b ON a.LotteryCode = b.LotteryCode ");
            sb.AppendFormat(" WHERE a.LotteryCode = {0} AND StartTime > GETDATE()  ", LotteryCode);
            sb.Append(" order by IsuseName asc  ");
            SqlMapper.GridReader grid = new DataRepositoryBase<udv_IsuseInfo>(DbConnectionEnum.CaileGame).QueryMultiple(sb.ToString());
            return grid.Read<udv_IsuseInfo>().FirstOrDefault();
        }
        /// <summary>
        /// 追号时查询期号信息
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<IsusesEntity> QueryChaseTasksIsuses(int TopCount, int LotteryCode)
        {
            StringBuilder SqlStr = new StringBuilder();
            SqlStr.AppendFormat(" SELECT top {0} i.IsuseID,i.IsuseName,i.StartTime,i.EndTime FROM CT_Isuses AS i ", TopCount);
            SqlStr.Append(" INNER JOIN dbo.CT_Lotteries AS l ");
            SqlStr.Append(" ON l.LotteryCode=i.LotteryCode  ");
            SqlStr.Append(" AND DATEADD(MINUTE,-l.AdvanceEndTime,i.EndTime)>GETDATE() ");
            SqlStr.Append(" WHERE i.LotteryCode=@LotteryCode ORDER BY i.StartTime ASC ; ");

            var Paramter = new DynamicParameters();
            Paramter.Add("@LotteryCode", LotteryCode, DbType.Int32, null, 4);

            return base.QueryList(SqlStr.ToString(), Paramter).ToList();
        }

        /// <summary>
        /// 追号时查询期号信息
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<IsusesEntity> QueryLotTrendChart(int LotteryCode, int TopCount)
        {
            StringBuilder SqlStr = new StringBuilder();
            SqlStr.AppendFormat(" SELECT TOP {0} IsuseName,OpenNumber,EndTime FROM dbo.CT_Isuses  ", TopCount);
            SqlStr.Append(" WHERE LotteryCode=@LotteryCode AND EndTime < GETDATE() AND IsOpened=1 AND OpenNumber<>'' ");
            SqlStr.Append(" ORDER BY IsuseName DESC ");

            var Paramter = new DynamicParameters();
            Paramter.Add("@LotteryCode", LotteryCode, DbType.Int32, null, 4);

            return base.QueryList(SqlStr.ToString(), Paramter).ToList();
        }

        /// <summary>
        /// 生产彩种期号
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="Date"></param>
        /// <param name="Days"></param>
        /// <returns></returns>
        public int AddIsuseAdd(int LotteryCode, string sDate, int Days)
        {
            var para = new DynamicParameters();
            para.Add("@LotteryCode", LotteryCode);
            para.Add("@Date", Convert.ToDateTime(sDate));
            para.Add("@Days", Days);
            para.Add("@ReturnValue", null);
            base.Execute("udp_AddIssue", para);
            object iResult = para.Get<object>("@ReturnValue");
            return iResult == null ? 0 : Convert.ToInt32(iResult);
        }
        /// <summary>
        /// 生产彩种期号
        /// </summary>
        /// <param name="IsseuName">开始期号</param>
        /// <param name="LotteryCode">彩种</param>
        /// <param name="Date">日期</param>
        /// <param name="Days">生产天数</param>
        /// <returns></returns>
        public int AddIssueLuck(int IsseuName, int LotteryCode, string sDate, int Days)
        {
            var para = new DynamicParameters();
            para.Add("@IsseuName", IsseuName);
            para.Add("@LotteryCode", LotteryCode);
            para.Add("@Date", Convert.ToDateTime(sDate));
            para.Add("@Days", Days);
            para.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output);
            base.Execute("CP_AddIssueLuck", para);
            object iResult = para.Get<object>("@ReturnValue");
            return iResult == null ? 0 : Convert.ToInt32(iResult);
        }
        /// <summary>
        /// 生产彩种期号
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="Date"></param>
        /// <param name="Days"></param>
        /// <returns></returns>
        public int GenerateIssue(int LotteryCode, string sDate, int Days)
        {
            var para = new DynamicParameters();
            para.Add("@LotteryCode", LotteryCode, DbType.Int32, null, 4);
            para.Add("@Date", sDate, DbType.DateTime, null, 10);
            para.Add("@Days", Days, DbType.Int32, null, 4);
            para.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            base.Execute("udp_GenerateIssue", para);
            object iResult = para.Get<object>("@ReturnValue");
            return iResult == null ? 0 : Convert.ToInt32(iResult);
        }
        /// <summary>
        /// 生产彩种期号福利彩如：双色球、大乐透
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="sDate"></param>
        /// <returns></returns>
        public int AddIsuseAddFC(int LotteryCode, string sDate)
        {
            var para = new DynamicParameters();
            para.Add("@NewYear", sDate, DbType.DateTime, null, 10);
            para.Add("@LotteryCode", LotteryCode, DbType.Int32, null, 4);
            para.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            base.Execute("udp_AddIssueFC", para);
            object iResult = para.Get<object>("@ReturnValue");
            return iResult == null ? 0 : Convert.ToInt32(iResult);
        }
        /// <summary>
        /// 添加采集的数据记录
        /// </summary>
        /// <returns></returns>
        public bool AddLotteryOpenNumber(int LotteryCode, string IsuseName, string OpenNumber, DateTime StartTime, DateTime EndTime, DateTime OpenTime, ref string ReturnDescription)
        {
            var para = new DynamicParameters();
            para.Add("@LotteryCode", LotteryCode, DbType.Int32, null, 4);
            para.Add("@IsuseName", IsuseName, DbType.String, null, 20);
            para.Add("@OpenNumber", OpenNumber, DbType.String, null, 100);
            para.Add("@StartTime", StartTime, DbType.DateTime, null, 20);
            para.Add("@EndTime", EndTime, DbType.DateTime, null, 20);
            para.Add("@OpenTime", OpenTime, DbType.DateTime, null, 20);
            para.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            para.Add("@ReturnDescription", null, DbType.String, ParameterDirection.Output, 100);

            int rows = base.Execute("udp_CollectOpenPrizeRecord", para);
            ReturnDescription = para.Get<string>("@ReturnDescription");
            return rows > 0;
        }
        /// <summary>
        /// 录入开奖结果
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <param name="OpenNumber"></param>
        /// <param name="OpenTime"></param>
        /// <param name="ReturnDescription"></param>
        /// <returns></returns>
        public bool EnteringDrawResults(int LotteryCode, string IsuseName, string OpenNumber, DateTime OpenTime, ref string ReturnDescription)
        {
            long ReturnValue = 0;
            var para = new DynamicParameters();
            para.Add("@LotteryCode", LotteryCode, DbType.Int32, null, 4);
            para.Add("@IsuseName", IsuseName, DbType.String, null, 20);
            para.Add("@OpenNumber", OpenNumber, DbType.String, null, 100);
            para.Add("@OpenTime", OpenTime, DbType.DateTime, null, 20);
            para.Add("@ReturnValue", null, DbType.Int64, ParameterDirection.Output, 4);
            para.Add("@ReturnDescription", null, DbType.String, ParameterDirection.Output, 100);
            base.Execute("udp_EnteringDrawResults", para);
            ReturnDescription = para.Get<string>("@ReturnDescription");
            ReturnValue = para.Get<long>("@ReturnValue");
            return ReturnValue > 0;
        }
        /// <summary>
        /// 开奖大厅数据查询
        /// </summary>
        /// <returns></returns>
        public List<IsusesEntity> OpenAwardHall()
        {
            return base.db.Query<IsusesEntity>("udp_OpenAwardHall", null, null, true, null, CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 查询当前期的上一期是否开奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public IsusesEntity QueryLastIsuseInfo(int LotteryCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 a.* FROM dbo.CT_Isuses a ");
            strSql.Append(" JOIN [dbo].[CT_Lotteries] b ON a.LotteryCode = b.LotteryCode ");
            strSql.AppendFormat(" WHERE a.LotteryCode = {0} AND  DATEADD(MINUTE,(-b.AdvanceEndTime),EndTime) < GETDATE() ", LotteryCode);
            strSql.Append(" ORDER BY a.IsuseName DESC ");
            return base.QuerySingleOrDefaultT(strSql.ToString());
        }
        /// <summary>
        /// 查询上一期的投注用户
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<UsersEntity> QueryLastOpenUsers(int LotteryCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT s.InitiateUserID AS UserID FROM dbo.CT_Schemes  AS s ");
            strSql.AppendFormat(" INNER JOIN (SELECT TOP 1 * FROM dbo.CT_Isuses WHERE LotteryCode={0} AND OpenNumber<>'' ORDER BY IsuseName DESC) AS tb ", LotteryCode);
            strSql.Append(" ON tb.IsuseID = s.IsuseID ");
            strSql.Append(" GROUP BY s.InitiateUserID ");
            return new DataRepositoryBase<UsersEntity>(DbConnectionEnum.CaileGame).QueryList(strSql.ToString()).ToList();
        }

        /// <summary>
        /// 查询开奖记录
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<IsusesEntity> QueryOfLotteryRecord(int LotteryCode, int pageIndex, int pageSize)
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" LotteryCode = @LotteryCode AND EndTime < GETDATE() ");
            object Paramters = new { LotteryCode = LotteryCode };
            return base.GetListPaged(pageIndex, pageSize, Where.ToString(), " IsuseName desc", Paramters).ToList();
        }


    }
}
