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

namespace CL.Game.DAL
{
    public class RegularHolidayDAL : DataRepositoryBase<RegularHolidayEntity>
    {
        public RegularHolidayDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        public List<RegularHolidayEntity> QueryEntitys(int RegularID)
        {
            return base.GetList(new { RegularID = RegularID }, "RHolidayID DESC").ToList();
        }
        /// <summary>
        /// 查询加奖信息(节假日玩法)
        /// 不包含阶梯加奖或等级加奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityHoliday> QueryRegularHolidayAward(int LotteryCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@LotteryCode", LotteryCode, DbType.Int32);
            return base.db.Query<udv_IsAwardActivityHoliday>("udp_QueryIsAwardActivityHoliday", Parms, null, true, null, CommandType.StoredProcedure).ToList();
        }
    }
}
