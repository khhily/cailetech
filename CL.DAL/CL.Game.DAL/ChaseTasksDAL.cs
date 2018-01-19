using CL.Dapper.Repository;
using CL.Game.Entity;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;
using CL.View.Entity.Game;
using Dapper;

namespace CL.Game.DAL
{
    public class ChaseTasksDAL : DataRepositoryBase<ChaseTasksEntity>
    {
        public ChaseTasksDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {

        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            int iResult = base.RecordCount(new { ID = ID });
            if (iResult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
       
        /// <summary>
        /// 根据方案编号查询追号对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public ChaseTasksEntity QueryEntityBySchemeID(long SchemeID)
        {
            return base.Get(new { SchemeID = SchemeID }, "ChaseTaskID DESC");
        }

        /// <summary>
        /// 获取方案统计数据实体
        /// 待优化方法
        /// </summary>
        public udv_ChaseTasks QueryChaseTaskTotal(int ChaseTaskID)
        {
            string strSql = @" 
                            SELECT top(1) a.ChaseTaskID, a.UserID, a.Title, a.Description, a.LotteryCode, c.LotteryName,
                                    a.QuashStatus, a.StopTypeWhenWin, a.StopTypeWhenWinMoney, 
		                            d.SumMoney , --总金额
		                            ISNULL(d.SumIsuseNum, 0) AS SumIsuseNum , --总期数
                                    ISNULL(d.BuyedIsuseNum, 0) AS BuyedIsuseNum , --完成期数
                                    ISNULL(d.QuashedIsuseNum, 0) AS QuashedIsuseNum ,--取消期数
                                    ISNULL(d.BuyedMoney, 0) AS BuyedMoney ,--完成期数总金额
                                    ISNULL(d.QuashedMoney, 0) AS QuashedMoney --取消期数总金额
                            FROM dbo.CT_ChaseTasks a
	                            LEFT JOIN dbo.CT_Users b ON a.UserID = b.UserID 
	                            LEFT JOIN dbo.CT_Lotteries c ON a.LotteryCode = c.LotteryCode
	                            LEFT JOIN (
		                            SELECT ChaseTaskID, COUNT(*) AS SumIsuseNum, SUM(Amount + RedPacketMoney) AS SumMoney, 
			                            COUNT(CASE WHEN IsExecuted=1 AND QuashStatus=0 THEN 1 END) AS BuyedIsuseNum,
			                            COUNT(CASE WHEN QuashStatus <> 0 THEN 1 END) AS QuashedIsuseNum,
			                            ISNULL(SUM(CASE WHEN IsExecuted=1 AND QuashStatus=0 THEN Amount END),0) AS BuyedMoney,
			                            ISNULL(SUM(CASE WHEN QuashStatus <> 0 THEN Amount END),0) AS QuashedMoney
		                            FROM dbo.CT_ChaseTaskDetails GROUP BY  ChaseTaskID
	                            ) d ON d.ChaseTaskID = a.ChaseTaskID
                            WHERE a.ChaseTaskID={0} 
                            ORDER BY a.ChaseTaskID ";
            strSql = string.Format(strSql, ChaseTaskID);

            udv_ChaseTasks model = new DataRepositoryBase<udv_ChaseTasks>(DbConnectionEnum.CaileGame).QuerySingleOrDefaultT(strSql);
            return model;
        }
        /// <summary>
        /// 获取方案数据明细
        /// 待优化方法
        /// </summary>
        public List<udv_ChaseTasksDetails> QueryChaseTaskDetails(long ChaseTaskID)
        {

            string strSql = @" 
                    SELECT a.ID, a.ChaseTaskID, a.CreateTime, a.IsuseID, a.PlayCode, c.PlayName, a.LotteryNumber, a.Multiple, a.Amount, a.RedPacketMoney, a.RedPacketId,
	                    a.QuashStatus, a.SchemeID, b.Title, b.Description, b.LotteryCode, d.IsuseName, e.SchemeNumber, e.WinMoney, e.WinMoneyNoWithTax
                    FROM dbo.CT_ChaseTaskDetails a
                    LEFT JOIN dbo.CT_ChaseTasks b ON b.ChaseTaskID=a.ChaseTaskID
                    LEFT JOIN dbo.CT_PlayTypes c ON c.PlayCode=a.PlayCode
                    LEFT JOIN dbo.CT_Isuses d ON d.IsuseID = a.IsuseID
                    LEFT JOIN dbo.CT_Schemes e ON e.SchemeID = a.SchemeID
                    WHERE a.ChaseTaskID={0} 
                    ORDER BY a.ID ";

            strSql = string.Format(strSql, ChaseTaskID);
            List<udv_ChaseTasksDetails> list = QueryMultiple(strSql).Read<udv_ChaseTasksDetails>().ToList();
            return list;
        }
        /// <summary>
        /// 撤销单期追号
        /// </summary>
        /// <param name="ChaseTaskDetailsID"></param>
        /// <param name="IsSystem"></param>
        /// <param name="ReturnDescription"></param>
        /// <returns></returns>
        public int QuashChaseTaskDetail(int ChaseTaskDetailsID, bool IsSystem, ref string ReturnDescription)
        {
            var para = new DynamicParameters();
            para.Add("@ChaseTaskDetailID", ChaseTaskDetailsID, DbType.Int32, null, 4);
            para.Add("@isSystemQuash", IsSystem, DbType.Boolean, null, 1);
            para.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            para.Add("@ReturnDescription", null, DbType.String, ParameterDirection.Output, 100);

            base.Execute("udp_RevokeChaseDetail", para);
            ReturnDescription = para.Get<string>("@ReturnDescription");
            int iResult = para.Get<int>("@ReturnValue");
            return iResult;
        }
        /// <summary>
        /// 撤消总追号任务
        /// </summary>
        /// <param name="ChaseTaskDetailsID"></param>
        /// <param name="IsSystem"></param>
        /// <param name="ReturnDescription"></param>
        /// <returns></returns>
        public int QuashChaseTask(int ChaseTaskID, bool IsSystem, ref string ReturnDescription)
        {
            var para = new DynamicParameters();
            para.Add("@ChaseTaskID", ChaseTaskID, DbType.Int32, null, 4);
            para.Add("@isSystemQuash", IsSystem, DbType.Boolean, null, 1);
            para.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            para.Add("@ReturnDescription", null, DbType.String, ParameterDirection.Output, 100);

            base.Execute("udp_RevokeChase", para);
            ReturnDescription = para.Get<string>("@ReturnDescription");
            int iResult = para.Get<int>("@ReturnValue");
            return iResult;
        }
    }
}
