using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Dapper.Repository;
using CL.Enum.Common;
using CL.Game.Entity;
using Dapper;
using CL.View.Entity.Game;
/// <summary>
/// 中奖金额区间加奖详细规则
/// </summary>
namespace CL.Game.DAL
{
    public class RegularAwardIntervalDAL : DataRepositoryBase<RegularAwardIntervalEntity>
    {
        public RegularAwardIntervalDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        public List<RegularAwardIntervalEntity> QueryEntitys(int RegularID)
        {
            return base.GetList(new { RegularID = RegularID }, "RAwardIntervalID desc").ToList();
        }
        /// <summary>
        /// 查询加奖信息(中奖金额区间玩法)
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityAwardInterval> QueryRegularAwardIntervalAward(int LotteryCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@LotteryCode", LotteryCode, DbType.Int32);
            return base.db.Query<udv_IsAwardActivityAwardInterval>("udp_QueryIsAwardActivityAwardInterval", Parms, null, true, null, CommandType.StoredProcedure).ToList();
        }
        /// <summary>
        /// 中奖金额累计区间加奖 
        /// </summary>
        public void AwardIntervalAward(int ActivityID, int RegularID, int PlayCode, long Min, long Max, long AwardMoney)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@ActivityID", ActivityID, DbType.Int32);
            Parms.Add("@RegularID", RegularID, DbType.Int32);
            Parms.Add("@PlayCode", PlayCode, DbType.Int32);
            Parms.Add("@Min", Min, DbType.Int64);
            Parms.Add("@Max", Max, DbType.Int64);
            Parms.Add("@AwardMoney", AwardMoney, DbType.Int64);
            base.Execute("udp_AwardActivityAwardInterval", Parms);
        }
    }
}
