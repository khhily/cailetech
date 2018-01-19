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

/// <summary>
/// 活动主题
/// </summary>
namespace CL.Game.DAL
{
    public class ActivityDAL : DataRepositoryBase<ActivityEntity>
    {
        public ActivityDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public int InsertEntity(ActivityEntity Entity)
        {
            return base.Insert(Entity) ?? 0;
        }
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public int UpdateEntity(ActivityEntity Entity)
        {
            return base.Update(Entity);
        }
        /// <summary>
        /// 根据标识查询对象
        /// </summary>
        /// <param name="RegularID"></param>
        /// <returns></returns>
        public ActivityEntity QueryEntity(int ActivityID)
        {
            return base.Get(ActivityID);
        }
        /// <summary>
        /// 获取活动对象集
        /// </summary>
        /// <returns></returns>
        public List<ActivityEntity> QueryEntitys()
        {
            return base.GetList(new { ActivityApply = 1 }, " ActivityID desc").ToList();
        }

        /// <summary>
        /// 查询活动列表
        /// </summary>
        /// <param name="Keys"></param>
        /// <param name="ActivityType"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="IsModify"></param>
        /// <param name="ActivityApply"></param>
        /// <param name="CurrencyUnit"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RecordCount"></param>
        /// <returns></returns>
        public List<ActivityEntity> QueryActivityList(string Keys, int ActivityType, DateTime StartTime, DateTime EndTime, int IsModify, int ActivityApply, int CurrencyUnit, int PageIndex, int PageSize, ref int RecordCount)
        {
            var para = new DynamicParameters();
            para.Add("@Keys", Keys);
            para.Add("@ActivityType", ActivityType);
            para.Add("@StartTime", StartTime);
            para.Add("@EndTime", EndTime);
            para.Add("@IsModify", IsModify);
            para.Add("@ActivityApply", ActivityApply);
            para.Add("@CurrencyUnit", CurrencyUnit);
            para.Add("@PageIndex", PageIndex);
            para.Add("@PageSize", PageSize);
            para.Add("@RecordCount", RecordCount, DbType.Int32, ParameterDirection.Output);
            var Entitys = base.db.Query<ActivityEntity>("udp_QueryActivityList", para, null, true, null, CommandType.StoredProcedure).ToList();
            RecordCount = para.Get<int>("@RecordCount");
            return Entitys;
        }
    }
}
