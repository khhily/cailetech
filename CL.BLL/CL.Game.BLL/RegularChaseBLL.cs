using CL.Game.DAL;
using CL.Game.Entity;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.BLL
{
    public class RegularChaseBLL
    {
        RegularChaseDAL dal = new RegularChaseDAL(Enum.Common.DbConnectionEnum.CaileGame);

        public int InsertEntity(RegularChaseEntity Entity)
        {
            return dal.Insert(Entity) ?? 0;
        }
        public int ModifyEntity(RegularChaseEntity Entity)
        {
            return dal.Update(Entity);
        }
        public List<RegularChaseEntity> QueryEntitys(int RegularID)
        {
            return dal.QueryEntitys(RegularID);
        }
        /// <summary>
        /// 查询加奖信息(胆拖玩法)
        /// 不包含阶梯加奖或等级加奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityChase> QueryRegularChaseAward(int LotteryCode)
        {
            return dal.QueryRegularChaseAward(LotteryCode);
        }

        /// <summary>
        /// 追号玩法加奖
        /// </summary>
        /// <param name="Awards"></param>
        /// <returns></returns>
        public bool ChaseAward(int RegularID, int AwardType, long AwardMoney, DateTime StartTime, DateTime EndTime, int RChaseType, long Unit, int PlayCode)
        {
            return dal.ChaseAward(RegularID, AwardType, AwardMoney, StartTime, EndTime, RChaseType, Unit, PlayCode);
        }

        #region 计算
        /// <summary>
        /// 加奖计算
        /// </summary>
        /// <param name="Entitys"></param>
        /// <param name="Chase_Entitys"></param>
        /// <returns></returns>
        public bool CalculateAward(int LotteryCode)
        {
            try
            {
                List<udv_IsAwardActivityChase> AwardActivitys = this.QueryRegularChaseAward(LotteryCode);
                if (AwardActivitys != null && AwardActivitys.Count > 0)
                {
                    AwardActivitys.ForEach((Entity) =>
                    {
                        //规则状态：0 初始化规则，1 规则作废(活动审核失败)，2 规则开始并生效(活动审核通过)，
                        //3 活动截止并开始加奖(这里加奖针对活动期间累计加奖规则)，4 活动结束并销毁(所有加奖派发完成后结束和销毁活动，销毁后的活动规则无法直接启用，启用销毁的规则需要重置及走审核流程)
                        if (Entity.RegularStatus == 2 || Entity.RegularStatus == 3)
                        {
                            this.ChaseAward(Entity.RegularID, Entity.ActivityType, Entity.AwardMoney, Entity.StartTime, Entity.EndTime, Entity.RChaseType, Entity.Unit, Entity.PlayCode);
                        }
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

    }
}
