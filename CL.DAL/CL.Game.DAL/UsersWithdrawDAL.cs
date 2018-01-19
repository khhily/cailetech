using CL.Dapper.Repository;
using CL.Game.Entity;
using CL.Enum.Common;
using System.Data;
using Dapper;
using System.Collections.Generic;
using System;
using System.Text;
using CL.Game.DAL;
using CL.View.Entity.Game;

namespace CL.Game.DAL
{
    public class UsersWithdrawDAL : DataRepositoryBase<UsersWithdrawEntity>
    {
        public UsersWithdrawDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="PayOutStatus"></param>
        /// <returns></returns>
        public bool AuditPayOutStatus(List<long> Ids, int PayOutStatus, int OperaterID)
        {
            using (IDbTransaction tran = base.db.BeginTransaction())
            {
                try
                {
                    foreach (var item in Ids)
                    {
                        var entity = base.Get(item, tran);
                        entity.PayOutStatus = (byte)PayOutStatus;
                        entity.OperaterID = OperaterID;
                        base.Update(entity, tran);
                    }

                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                }
                finally
                {
                    base.db.Dispose();
                    base.db.Close();
                }
            }
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="PayOutStatus"></param>
        /// <returns></returns>
        public bool AuditPayOutStatus(long Id, int PayOutStatus, int OperaterID)
        {
            using (IDbTransaction tran = base.db.BeginTransaction())
            {
                try
                {
                    var entity = base.Get(Id, tran);
                    entity.PayOutStatus = (byte)PayOutStatus;
                    entity.OperaterID = OperaterID;
                    base.Update(entity, tran);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                }
                finally
                {
                    base.db.Dispose();
                    base.db.Close();
                }
            }
        }
        /// <summary>
        /// 提现成功
        /// </summary>
        /// <param name="Model.Entities.UserPayOut"></param>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        public bool AuditPayOutSuccess(long PayOutID, int OperaterID)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@PayOutID", PayOutID, DbType.Int64);
            Parms.Add("@OperaterID", OperaterID, DbType.Int32);
            Parms.Add("@Result", null, DbType.Int32, ParameterDirection.Output, 4);
            base.Execute("udp_AuditPaySuccess", Parms);
            int iResult = Parms.Get<int>("@Result");
            return iResult == 0 ? true : false;
        }
        /// <summary>
        /// 提现失败
        /// </summary>
        /// <param name="Model.Entities.UserPayOut"></param>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        public bool AuditPayOutFailure(long PayOutID, int OperaterID, string Remark)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@PayOutID", PayOutID, DbType.Int64);
            Parms.Add("@OperaterID", OperaterID, DbType.Int32);
            Parms.Add("@Remark", Remark, DbType.String);
            Parms.Add("@Result", null, DbType.Int32, ParameterDirection.Output, 4);
            base.Execute("udp_AuditPayFailure", Parms);
            int iResult = Parms.Get<int>("@Result");
            return iResult == 0 ? true : false;
        }

        /// <summary>
        /// 提现申请并冻结金额
        /// 资金流动需要审核则需要冻结金额
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public int ApplyForWithdraw(long UserCode, long BankCode, long Amount, string PayPwd)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@UserCode", UserCode, DbType.Int64, null, 8);
            Parms.Add("@BankCode", BankCode, DbType.Int64, null, 8);
            Parms.Add("@Amount", Amount, DbType.Int64, null, 8);
            Parms.Add("@PayPwd", PayPwd, DbType.String, null, 100);
            Parms.Add("@Result", null, DbType.Int32, ParameterDirection.Output, 4);
            base.Execute("udp_ApplyForWithdraw", Parms);
            int iResult = Parms.Get<int>("@Result");
            return iResult;
        }
        /// <summary>
        /// 报表：提现查询
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="UserName"></param>
        /// <param name="UserID"></param>
        /// <param name="PayOutStatus"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RecordWithdrawAmount"></param>
        /// <param name="RecordCount"></param>
        /// <returns></returns>
        public List<udv_WithdrawReport> QuertWithdrawReport(DateTime StartTime, DateTime EndTime, string UserName, long UserID, int PayOutStatus, int PageIndex, int PageSize, ref long RecordWithdrawAmount, ref int RecordCount)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@StartTime", StartTime);
            Parms.Add("@EndTime", EndTime);
            Parms.Add("@UserName", UserName);
            Parms.Add("@UserID", UserID);
            Parms.Add("@PayOutStatus", PayOutStatus);
            Parms.Add("@PageIndex", PageIndex);
            Parms.Add("@PageSize", PageSize);
            Parms.Add("@RecordWithdrawAmount", RecordWithdrawAmount, DbType.Int64, ParameterDirection.Output);
            Parms.Add("@RecordCount", RecordCount, DbType.Int32, ParameterDirection.Output);
            var Entitys = new DataRepositoryBase<udv_WithdrawReport>(DbConnectionEnum.CaileGame).QueryList("udp_QuertWithdrawReport", Parms, CommandType.StoredProcedure);
            RecordWithdrawAmount = Parms.Get<long>("@RecordWithdrawAmount");
            RecordCount = Parms.Get<int>("@RecordCount");
            return Entitys.AsList();
        }
    }
}
