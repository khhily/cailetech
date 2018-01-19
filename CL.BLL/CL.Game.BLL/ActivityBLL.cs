using CL.Entity.Json.WebAPI;
using CL.Enum.Common;
using CL.Game.DAL;
using CL.Game.Entity;
using CL.Json.Entity.WebAPI;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.BLL
{
    public class ActivityBLL
    {
        ActivityDAL dal = new ActivityDAL(Enum.Common.DbConnectionEnum.CaileGame);

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public int InsertEntity(ActivityEntity Entity)
        {
            return dal.InsertEntity(Entity);
        }
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public int UpdateEntity(ActivityEntity Entity)
        {
            return dal.UpdateEntity(Entity);
        }
        /// <summary>
        /// 根据标识查询对象
        /// </summary>
        /// <param name="RegularID"></param>
        /// <returns></returns>
        public ActivityEntity QueryEntity(int ActivityID)
        {
            return dal.QueryEntity(ActivityID);
        }
        /// <summary>
        /// 获取活动对象集
        /// </summary>
        /// <returns></returns>
        public List<ActivityEntity> QueryEntitys()
        {
            return dal.QueryEntitys();
        }
        /// <summary>
        /// 活动广告
        /// 局部使用本地缓存 初始化加载提升效率
        /// </summary>
        /// <returns></returns>
        public List<ApplyAd> QueryEntitysAd()
        {
            string CacheKey = "Cache_QueryEntitysAd";
            List<ApplyAd> array = CacheHelper.Get<List<ApplyAd>>(CacheKey);
            if (array == null || array.Count == 0)
            {
                var Entitys = dal.QueryEntitys();
                if (Entitys != null && Entitys.Count > 0)
                    array = Entitys.Select(s => new ApplyAd()
                    {
                        ad = s.ADUrl,
                        page = s.LandingPage
                    }).ToList();
                //本地缓存
                CacheHelper.Insert(CacheKey, array, 10);
            }
            return array;
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
            return dal.QueryActivityList(Keys, ActivityType, StartTime, EndTime, IsModify, ActivityApply, CurrencyUnit, PageIndex, PageSize, ref RecordCount);
        }

    }
}
