using CL.Dapper.Repository;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Enum.Common;
using System.Data;
using CL.View.Entity.Game;
using Dapper;

/// <summary>
/// 追号玩法加奖详细规则
/// </summary>
namespace CL.Game.DAL
{
    public class RegularChaseDAL : DataRepositoryBase<RegularChaseEntity>
    {
        public RegularChaseDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {

        }

        public List<RegularChaseEntity> QueryEntitys(int RegularID)
        {
            return base.GetList(new { RegularID = RegularID }, "RChaseID DESC").ToList();
        }
        /// <summary>
        /// 查询加奖信息(胆拖玩法)
        /// 不包含阶梯加奖或等级加奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityChase> QueryRegularChaseAward(int LotteryCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@LotteryCode", LotteryCode, DbType.Int32);
            return base.db.Query<udv_IsAwardActivityChase>("udp_QueryIsAwardActivityChase", Parms, null, true, null, CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 追号玩法加奖 
        /// </summary>
        /// <param name="Awards"></param>
        /// <returns></returns>
        public bool ChaseAward(int RegularID, int AwardType, long AwardMoney, DateTime StartTime, DateTime EndTime, int RChaseType, long Unit, int PlayCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@RegularID", RegularID, DbType.Int32);
            Parms.Add("@AwardType", AwardType, DbType.Int32);
            Parms.Add("@AwardMoney", AwardMoney, DbType.Int64);
            Parms.Add("@StartTime", StartTime, DbType.DateTime);
            Parms.Add("@EndTime", EndTime, DbType.DateTime);
            Parms.Add("@RChaseType", RChaseType, DbType.Int32);
            Parms.Add("@Unit", Unit, DbType.Int64);
            Parms.Add("@PlayCode", PlayCode, DbType.Int32);
            var i = base.Execute("udp_AwardActivityChase", Parms);
            return i > 0;
        }
    }
}
