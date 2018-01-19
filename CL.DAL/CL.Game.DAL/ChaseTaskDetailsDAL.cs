using CL.Dapper.Repository;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;
using Dapper;
using CL.View.Entity.Game;
using System.Text;

namespace CL.Game.DAL
{
    public class ChaseTaskDetailsDAL : DataRepositoryBase<ChaseTaskDetailsEntity>
    {
        public ChaseTaskDetailsDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        
        /// <summary>
        /// 更新追号详情对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long ModifyEntity(ChaseTaskDetailsEntity entity)
        {
            return base.Update(entity);
        }
        
        /// <summary>
        /// 查询追号详情单个对象
        /// </summary>
        /// <param name="ChaseDetailsID"></param>
        /// <returns></returns>
        public ChaseTaskDetailsEntity QueryEntity(long ChaseDetailsID)
        {
            return base.Get(ChaseDetailsID);
        }

        /// <summary>
        /// 根据方案编号查询对象集
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public List<ChaseTaskDetailsEntity> QueryEntitysBySchemeID(long SchemeID)
        {
            return base.GetList(new { SchemeID = SchemeID }, "ID DESC").ToList();
        }
        
        /// <summary>
        /// 更新发送状态
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public bool ModifySendOut(long ChaseTaskDetailsID)
        {
            using (var tran = base.db.BeginTransaction())
            {
                try
                {
                    var Entity = base.Get(ChaseTaskDetailsID, tran);
                    Entity.IsSendOut = true;
                    Entity.IsExecuted = true;
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
        /// 是否发送
        /// </summary>
        /// <param name="ChaseTaskDetailsID"></param>
        /// <returns></returns>
        public bool IsSendOut(long ChaseTaskDetailsID)
        {
            var entity = base.Get(ChaseTaskDetailsID);
            return entity == null ? true : entity.IsSendOut ?? false;
        }

        /// <summary>
        /// 停止追号查询
        /// </summary>
        /// <param name="ChaseTaskID"></param>
        /// <param name="QueryType">查询类型  1 追号完成停止、2 追号中奖或中奖奖金大于多少停止</param>
        /// <returns></returns>
        public long StopChaseTaskQuery(long ChaseTaskID, int QueryType)
        {
            var para = new DynamicParameters();
            para.Add("@ChaseTaskID", ChaseTaskID);
            para.Add("@QueryType", QueryType);
            object Numbs = base.ExecScalar("udp_StopChaseTaskQuery", para);
            return Convert.ToInt64(Numbs);
        }
        /// <summary>
        /// 未完成追号撤单
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="ChaseTaskDetailID"></param>
        /// <param name="UserID"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public int ChaseRevoke(long SchemeID, long ChaseTaskDetailID, long UserID, long Amount)
        {
            var para = new DynamicParameters();
            para.Add("@SchemeID", SchemeID);
            para.Add("@ChaseTaskDetailID", ChaseTaskDetailID);
            para.Add("@UserID", UserID);
            para.Add("@Amount", Amount);
            object Numbs = base.ExecScalar("udp_ChaseRevoke", para);
            return Convert.ToInt32(Numbs);
        }
        /// <summary>
        /// 停止追号
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="ChaseTaskDetailsID"></param>
        /// <param name="Amount"></param>
        /// <param name="StopType">1追号完成停止 ,2追号满足条件停止</param>
        /// <returns></returns>
        public long StopChaseTask(long SchemeID, long ChaseTaskDetailsID, long Amount, int StopType)
        {
            var para = new DynamicParameters();
            para.Add("@SchemeID", SchemeID);
            para.Add("@ChaseTaskDetailsID", ChaseTaskDetailsID);
            para.Add("@Amount", Amount);
            para.Add("@StopType", StopType);
            object Numbs = base.ExecScalar("udp_StopChaseTask", para);
            return Convert.ToInt64(Numbs);
        }
        /// <summary>
        /// 查询追号需要停止的任务
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="ChaseTaskID"></param>
        /// <returns></returns>
        public List<ChaseTaskDetailsEntity> QueryModelListByChaseTaskID(long SchemeID, long ChaseTaskID)
        {
            return base.GetList(new { ChaseTaskID = ChaseTaskID, SchemeID = SchemeID, IsExecuted = false }, "ID desc").ToList();
        }
        /// <summary>
        /// 查询追号内容
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public List<udv_ChaseDetailRecord> QueryChaseDetailRecord(long SchemeID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT c.ID,i.IsOpened,i.IsuseName,c.Amount,c.IsExecuted,c.IsSendOut,c.SchemeID,i.LotteryCode,i.IsuseID,c.QuashStatus ");
            strSql.Append(" ,SUM(d.WinMoneyNoWithTax) AS WinMoney FROM dbo.CT_ChaseTaskDetails AS c ");
            strSql.Append(" INNER JOIN dbo.CT_Isuses AS i ON i.IsuseID=c.IsuseID ");
            strSql.Append(" LEFT JOIN dbo.CT_SchemeETickets AS e ON e.ChaseTaskDetailsID=c.ID AND c.SchemeID=c.SchemeID ");
            strSql.Append(" LEFT JOIN dbo.CT_SchemesDetail AS d ON d.SDID = e.SDID AND d.SchemeID=c.SchemeID ");
            strSql.AppendFormat(" WHERE c.SchemeID={0} ", SchemeID);
            strSql.Append(" GROUP BY c.ID,i.IsOpened,i.IsuseName,c.Amount,c.IsExecuted,c.IsSendOut,c.SchemeID,i.LotteryCode,i.IsuseID,c.QuashStatus");

            return new DataRepositoryBase<udv_ChaseDetailRecord>(DbConnectionEnum.CaileGame).QueryList(strSql.ToString()).ToList();
        }

        /// <summary>
        /// 查询期号过期处理追号失败数据
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <returns></returns>
        public List<udv_ExpireRevokeChase> QueryExpireRevokeChase(int LotteryCode, string IsuseName)
        {
            var Param = new DynamicParameters();
            Param.Add("@LotteryCode", LotteryCode);
            Param.Add("@IsuseName", IsuseName);
            return new DataRepositoryBase<udv_ExpireRevokeChase>(DbConnectionEnum.CaileGame).QueryList("udp_ExpireRevokeChase", Param, CommandType.StoredProcedure).ToList();
        }

    }
}
