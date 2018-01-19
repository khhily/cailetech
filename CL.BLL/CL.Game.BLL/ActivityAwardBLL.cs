using CL.Entity.Json.WebAPI;
using CL.Enum.Common;
using CL.Game.DAL;
using CL.Game.Entity;
using CL.Redis.BLL;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.BLL
{
    public class ActivityAwardBLL
    {
        Log log = new Log("ActivityAwardBLL");
        ActivityAwardDAL dal = new ActivityAwardDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public int InsertEntity(ActivityAwardEntity Entity)
        {
            return dal.InsertEntity(Entity);
        }
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public int UpdateEntity(ActivityAwardEntity Entity)
        {
            return dal.UpdateEntity(Entity);
        }
        /// <summary>
        /// 根据标识查询对象
        /// </summary>
        /// <param name="RegularID"></param>
        /// <returns></returns>
        public ActivityAwardEntity QueryEntity(int RegularID)
        {
            return dal.QueryEntity(RegularID);
        }
        /// <summary>
        /// 根据活动标识查询对象集
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public List<ActivityAwardEntity> QueryEntitys(int ActivityID)
        {
            return dal.QueryEntitys(ActivityID);
        }
        /// <summary>
        /// 查询加奖彩种
        /// </summary>
        /// <param name="ActivityType">0 官方活动，1 彩乐平台活动</param>
        /// <param name="LotteryCode">彩种编码</param>
        /// <returns></returns>
        public List<int> QueryAwardLotteryCode(int ActivityType, int LotteryCode)
        {
            return dal.QueryAwardLotteryCode(ActivityType, LotteryCode);
        }
        // <summary>
        /// 查询加奖彩种详细玩法
        /// </summary>
        /// <param name="ActivityType">0 官方活动，1 彩乐平台活动</param>
        /// <param name="LotteryCode">彩种编码</param>
        /// <returns></returns>
        public List<udv_AwardPlayCode> QueryAwardPlayCode(int LotteryCode)
        {
            return dal.QueryAwardPlayCode(LotteryCode);
        }


        #region 自定义方法

        /// <summary>
        /// 查询彩种加奖玩法
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public AwarPlayCodeResult QueryAwardPlayCode(string Token, int LotteryCode)
        {
            AwarPlayCodeResult result = null;
            try
            {
                var data = this.QueryAwardPlayCode(LotteryCode);
                ResultCode code = ResultCode.Success;
                if (data == null || data.Count == 0)
                    code = ResultCode.NullData;
                result = new AwarPlayCodeResult()
                {
                    Code = (int)code,
                    Msg = Common.GetDescription(code),
                    Data = code != ResultCode.Success ? null : data.Select(s => new AwardPlayCode()
                    {
                        PlayCode = s.PlayCode,
                        AwardMoney = s.AwardMoney
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                log.Write("查询彩种加奖玩法错误[QueryAwardPlayCode]：" + ex.StackTrace, true);
                result = new AwarPlayCodeResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return result;
        }

        public bool ActivityAuditing(int ActivityID, bool IsActivity)
        {
            bool rec = false;
            try
            {
                var ActivityEntity = new ActivityBLL().QueryEntity(ActivityID);
                var RegularEntitys = this.QueryEntitys(ActivityID);
                if (IsActivity)
                {
                    ActivityEntity.ActivityApply = 1;
                    new ActivityBLL().UpdateEntity(ActivityEntity);
                    foreach (var Entity in RegularEntitys)
                    {
                        Entity.RegularStatus = 2;
                        this.UpdateEntity(Entity);
                    }
                }
                else
                {
                    ActivityEntity.ActivityApply = 2;
                    new ActivityBLL().UpdateEntity(ActivityEntity);
                    foreach (var Entity in RegularEntitys)
                    {
                        Entity.RegularStatus = 1;
                        this.UpdateEntity(Entity);
                    }
                }
                rec = true;
            }
            catch
            {
                rec = false;
            }
            return rec;
        }
        #endregion

    }
}
