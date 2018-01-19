using CL.Dapper.Repository;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Enum.Common;
using System.Data;
using Dapper;
using CL.View.Entity.Game;
using System.Xml;

/// <summary>
/// 投注金额上限加奖详细规则
/// </summary>
namespace CL.Game.DAL
{
    public class RegularTopLimitDAL : DataRepositoryBase<RegularTopLimitEntity>
    {
        public RegularTopLimitDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 查询加奖信息(投注金额上限加奖玩法)
        /// 不包含阶梯加奖或等级加奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityTopLimit> QueryRegularTopLimitAward(int LotteryCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@LotteryCode", LotteryCode, DbType.Int32);
            return base.db.Query<udv_IsAwardActivityTopLimit>("udp_QueryIsAwardActivityTopLimit", Parms, null, true, null, CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 投注金额累计区间加奖
        /// </summary>
        public void TopLimitAward(int ActivityID, int RegularID, int PlayCode, long AwardMoney, long TotalMoney)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@ActivityID", ActivityID, DbType.Int32);
            Parms.Add("@RegularID", RegularID, DbType.Int32);
            Parms.Add("@PlayCode", PlayCode, DbType.Int32);
            Parms.Add("@AwardMoney", AwardMoney, DbType.Int64);
            Parms.Add("@TotalMoney", TotalMoney, DbType.Int64);
            base.Execute("udp_AwardActivityTopLimit", Parms);
        }

       
    }
}
