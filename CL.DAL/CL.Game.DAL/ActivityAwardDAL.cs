using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Game.Entity;
using CL.Dapper.Repository;
using CL.Enum.Common;
using System.Data;
using Dapper;
using CL.View.Entity.Game;
/// <summary>
/// 活动加奖规则
/// </summary>
namespace CL.Game.DAL
{
    public class ActivityAwardDAL : DataRepositoryBase<ActivityAwardEntity>
    {
        public ActivityAwardDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public int InsertEntity(ActivityAwardEntity Entity)
        {
            return base.Insert(Entity) ?? 0;
        }
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public int UpdateEntity(ActivityAwardEntity Entity)
        {
            return base.Update(Entity);
        }
        /// <summary>
        /// 根据标识查询对象
        /// </summary>
        /// <param name="RegularID"></param>
        /// <returns></returns>
        public ActivityAwardEntity QueryEntity(int RegularID)
        {
            return base.Get(RegularID);
        }
        /// <summary>
        /// 根据活动标识查询对象集
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public List<ActivityAwardEntity> QueryEntitys(int ActivityID)
        {
            return base.GetList(new { ActivityID = ActivityID }, "RegularID desc").ToList();
        }
        /// <summary>
        /// 查询加奖彩种
        /// </summary>
        /// <param name="ActivityType">0 官方活动，1 彩乐平台活动</param>
        /// <param name="LotteryCode">彩种编码</param>
        /// <returns></returns>
        public List<int> QueryAwardLotteryCode(int ActivityType, int LotteryCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@ActivityType", ActivityType, DbType.Int32);
            Parms.Add("@LotteryCode", LotteryCode, DbType.Int32);
            return base.db.Query<int>("udp_QueryAwardLot", Parms, null, true, null, CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 查询加奖彩种详细玩法
        /// </summary>
        /// <param name="ActivityType">0 官方活动，1 彩乐平台活动</param>
        /// <param name="LotteryCode">彩种编码</param>
        /// <returns></returns>
        public List<udv_AwardPlayCode> QueryAwardPlayCode(int LotteryCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@LotteryCode", LotteryCode, DbType.Int32);
            return base.db.Query<udv_AwardPlayCode>("udp_QueryAwardPlayCode", Parms, null, true, null, CommandType.StoredProcedure).ToList();
        }
        
    }
}
