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
/// 中奖累计排名加奖详细规则
/// </summary>
namespace CL.Game.DAL
{
    public class RegularAwadRankingDAL : DataRepositoryBase<RegularAwardRankingEntity>
    {
        public RegularAwadRankingDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        public List<RegularAwardRankingEntity> QueryEntitys(int RegularID)
        {
            return base.GetList(new { RegularID = RegularID }, "RAwardRanID DESC").ToList();
        }
        /// <summary>
        /// 查询加奖信息(中奖金额名次玩法) 
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityAwardRanking> QueryRegularAwardRankingAward(int LotteryCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@LotteryCode", LotteryCode, DbType.Int32);
            return base.db.Query<udv_IsAwardActivityAwardRanking>("udp_QueryIsAwardActivityAwardRanking", Parms, null, true, null, CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 中奖金额累计名次加奖
        /// </summary>
        public void AwardRankingAward(int ActivityID, int RegularID, int PlayCode, long Placing, long AwardMoney)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@ActivityID", ActivityID, DbType.Int32);
            Parms.Add("@RegularID", RegularID, DbType.Int32);
            Parms.Add("@PlayCode", PlayCode, DbType.Int32);
            Parms.Add("@Placing", Placing, DbType.Int64);
            Parms.Add("@AwardMoney", AwardMoney, DbType.Int64);
            base.Execute("udp_AwardActivityAwardRanking", Parms);
        }



    }
}
