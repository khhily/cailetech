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
/// 数字彩中球加奖详细规则
/// </summary>
namespace CL.Game.DAL
{
    public class RegularBallDAL : DataRepositoryBase<RegularBallEntity>
    {
        public RegularBallDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        public List<RegularBallEntity> QueryEntitys(int RegularID)
        {
            return base.GetList(new { RegularID = RegularID }, "RBallID DESC").ToList();
        }

        /// <summary>
        /// 查询加奖信息(数字彩中球玩法)
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityBall> QueryRegularBetIntervalAward(int LotteryCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@LotteryCode", LotteryCode, DbType.Int32);
            return base.db.Query<udv_IsAwardActivityBall>("udp_QueryIsAwardActivityBall", Parms, null, true, null, CommandType.StoredProcedure).ToList();
        }

    }
}
