using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Dapper.Repository;
using CL.Enum.Common;
using CL.Game.Entity;
using CL.View.Entity.Game;
using Dapper;

/// <summary>
/// 投注金额累计排名加奖详细规则
/// </summary>
namespace CL.Game.DAL
{
    public class RegularBetRankingDAL : DataRepositoryBase<RegularBetRankingEntity>
    {
        public RegularBetRankingDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        public List<RegularBetRankingEntity> QueryEntitys(int RegularID)
        {
            return base.GetList(new { RegularID = RegularID }, "RBetRanID DESC").ToList();
        }
        /// <summary>
        /// 查询加奖信息(投注金额名次玩法)
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityBetRanking> QueryRegularBetRankingAward(int LotteryCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@LotteryCode", LotteryCode, DbType.Int32);
            return base.db.Query<udv_IsAwardActivityBetRanking>("udp_QueryIsAwardActivityBetRanking", Parms, null, true, null, CommandType.StoredProcedure).ToList();
        }
        
        /// <summary>
        /// 投注金额累计名次加奖 
        /// </summary>
        public void BetRankingAward(int ActivityID, int RegularID, int PlayCode, long Placing, long AwardMoney)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@ActivityID", ActivityID, DbType.Int32);
            Parms.Add("@RegularID", RegularID, DbType.Int32);
            Parms.Add("@PlayCode", PlayCode, DbType.Int32);
            Parms.Add("@Placing", Placing, DbType.Int64);
            Parms.Add("@AwardMoney", AwardMoney, DbType.Int64);
            base.Execute("udp_AwardActivityBetRanking", Parms);
        }
    }
}
