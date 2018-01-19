using CL.Dapper.Repository;
using CL.Game.Entity;
using CL.Enum.Common;
using System.Data;
using CL.View.Entity.Game;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Text;
using CL.Enum.Common.Type;
using System;

namespace CL.Game.DAL
{
    public class UsersRecordDAL : DataRepositoryBase<UsersRecordEntity>
    {
        public UsersRecordDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 账户金币变化明细记录
        /// </summary>
        /// <returns></returns>
        public List<udv_UserAccountDetail> QueryUserAccountDetail(long UserID, int tradeType, string startTime, string endTime, int pageSize, int pageIndex, ref int recordCount, ref long SumMoneyAdd, ref long SumMoneySub, ref long SumReward)
        {
            var para = new DynamicParameters();
            para.Add("@UserID", UserID, DbType.Int64, null, 8);
            para.Add("@tradeType", tradeType, DbType.Int32, null, 4);
            para.Add("@startTime", startTime, DbType.String, null, 20);
            para.Add("@endTime", endTime, DbType.String, null, 20);
            para.Add("@pageSize", pageSize, DbType.Int32, null, 4);
            para.Add("@pageIndex", pageIndex, DbType.Int32, null, 4);
            para.Add("@recordCount", null, DbType.Int32, ParameterDirection.Output, 4);
            para.Add("@SumMoneyAdd", null, DbType.Int64, ParameterDirection.Output, 8);
            para.Add("@SumMoneySub", null, DbType.Int64, ParameterDirection.Output, 8);
            para.Add("@SumReward", null, DbType.Int64, ParameterDirection.Output, 8);

            SqlMapper.GridReader grid = base.QueryMultiple("udp_AccountDetail", para, CommandType.StoredProcedure);

            List<udv_UserAccountDetail> list = grid.Read<udv_UserAccountDetail>().ToList();
            recordCount = para.Get<int>("@recordCount");
            SumMoneyAdd = para.Get<long>("@SumMoneyAdd");
            SumMoneySub = para.Get<long>("@SumMoneySub");
            SumReward = para.Get<long>("@SumReward");
            grid.Dispose();
            return list;
        }
        /// <summary>
        /// 充值金额
        /// </summary>
        /// <param name="accountEntity"></param>
        /// <param name="payEntity"></param>
        /// <returns></returns>
        public bool PayInfo(UsersRecordEntity accountEntity, UsersPayDetailEntity payEntity, UsersEntity userEntity)
        {
            long WithdrawMoney = payEntity.Amount / 2;
            var Parms = new DynamicParameters();
            Parms.Add("@RechargeNo", payEntity.RechargeNo, DbType.String, null, 200);
            Parms.Add("@OutRechargeNo", payEntity.OutRechargeNo, DbType.String, null, 200);
            Parms.Add("@PayType", payEntity.PayType, DbType.String, null, 50);
            Parms.Add("@Result", payEntity.Result, DbType.Int16);
            Parms.Add("@PayID", payEntity.PayID, DbType.Int64);
            Parms.Add("@Balance", userEntity.Balance, DbType.Int64);
            Parms.Add("@UserID", userEntity.UserID, DbType.Int64);
            Parms.Add("@RelationID", accountEntity.RelationID, DbType.String, null, 200);
            Parms.Add("@WithdrawMoney", WithdrawMoney, DbType.Int64);
            Parms.Add("@Rec", null, DbType.Int32, ParameterDirection.Output, 4);
            base.Execute("udp_PayInfo", Parms);
            int iResult = Parms.Get<int>("@Rec");
            return iResult == 0 ? true : false;
        }

        /// <summary>
        /// 统计报表
        /// 操作类型,0.充值 1.购彩消费 2.提现冻结 3.提现失败解冻 4.金豆兑换
        /// 5.中奖 11.用户撤单 12.系统撤单 13.追号撤单 14.投注失败退款 
        /// 15.出票失败退款 16.充值退款冻结 17.退款失败返回金额
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<udv_CheckingReport> QueryCheckingReport(string day)
        {
            var para = new DynamicParameters();
            para.Add("@Day", day, DbType.String, null, 20);
            SqlMapper.GridReader grid = base.QueryMultiple("udp_CheckingReport", para, CommandType.StoredProcedure);
            return grid.Read<udv_CheckingReport>().ToList();
        }

        /// <summary>
        /// 查询账户流水
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<udv_UsersRecord> QueryUsersRecordList(long UserID, int PageIndex, int PageSize, ref int RecordCount, ref int RecordWithdraw, ref int RecordBalance)
        {
            var para = new DynamicParameters();
            para.Add("@UserID", UserID);
            para.Add("@PageIndex", PageIndex);
            para.Add("@PageSize", PageSize);
            para.Add("@RecordCount", 0, DbType.Int32, ParameterDirection.Output);
            para.Add("@RecordWithdraw", 0, DbType.Int32, ParameterDirection.Output);
            para.Add("@RecordBalance", 0, DbType.Int32, ParameterDirection.Output);
            List<udv_UsersRecord> ls = new DataRepositoryBase<udv_UsersRecord>(DbConnectionEnum.CaileGame).QueryList("udp_UsersRecord", para, CommandType.StoredProcedure).ToList();
            RecordCount = para.Get<int>("@RecordCount");
            RecordWithdraw = para.Get<int>("@RecordWithdraw");
            RecordBalance = para.Get<int>("@RecordBalance");
            return ls;
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <param name="SchemeID">订单编号</param>
        /// <param name="TradeType">操作类型,0.充值 1.购彩消费 2.提现冻结 3.提现失败解冻 4.金豆兑换 5.中奖 11.用户撤单 
        /// 12.系统撤单 13.追号撤单 14.投注失败退款 15.出票失败退款 16.充值退款冻结 17.退款失败返回金额
        /// </param>
        /// <returns></returns>
        public UsersRecordEntity QueryEntitys(long UserID, long SchemeID, int TradeType)
        {
            return base.Get(new { UserID = UserID, TradeType = TradeType, RelationID = SchemeID }, "UserID desc");
        }

        /// <summary>
        /// 查询回水记录
        /// </summary>
        /// <returns></returns>
        public List<cv_UserRebate> QueryRebate(long UserID, int PageIndex, int PageSize, ref int RecordCount)
        {
            var para = new DynamicParameters();
            para.Add("@UserID", UserID);
            para.Add("@PageIndex", PageIndex);
            para.Add("@PageSize", PageSize);
            para.Add("@RecordCount", RecordCount, DbType.Int32, ParameterDirection.Output);
            List<cv_UserRebate> ls = new DataRepositoryBase<cv_UserRebate>(DbConnectionEnum.CaileGame).QueryList("CP_UserRebate", para, CommandType.StoredProcedure).ToList();
            RecordCount = para.Get<int>("@RecordCount");
            return ls;
        }
    }
}
