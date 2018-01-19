using CL.Game.DAL;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.BLL
{
    public class RegularTopLimitBLL
    {
        RegularTopLimitDAL dal = new RegularTopLimitDAL(Enum.Common.DbConnectionEnum.CaileGame);

        /// <summary>
        /// 查询加奖信息(投注金额上限加奖玩法)
        /// 不包含阶梯加奖或等级加奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityTopLimit> QueryRegularTopLimitAward(int LotteryCode)
        {
            return dal.QueryRegularTopLimitAward(LotteryCode);
        }

        /// <summary>
        /// 投注金额累计区间加奖
        /// </summary>
        public void TopLimitAward(int ActivityID, int RegularID, int PlayCode, long AwardMoney, long TotalAwardMoney)
        {
            dal.TopLimitAward(ActivityID, RegularID, PlayCode, AwardMoney, TotalAwardMoney);
        }

        #region  计算
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
                List<udv_IsAwardActivityTopLimit> AwardActivitys = this.QueryRegularTopLimitAward(LotteryCode);
                if (AwardActivitys != null && AwardActivitys.Count > 0)
                {
                    AwardActivitys.ForEach((Entity) =>
                    {
                        //规则状态：0 初始化规则，1 规则作废(活动审核失败)，2 规则开始并生效(活动审核通过)，
                        //3 活动截止并开始加奖(这里加奖针对活动期间累计加奖规则)，4 活动结束并销毁(所有加奖派发完成后结束和销毁活动，销毁后的活动规则无法直接启用，启用销毁的规则需要重置及走审核流程)
                        if (Entity.RegularStatus == 2 || Entity.RegularStatus == 3)
                        {
                            this.TopLimitAward(Entity.ActivityID, Entity.RegularID, Entity.PlayCode, Entity.AwardMoney, Entity.TotalAwardMoney);
                        }
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("投注金额累计区间加奖计算错误：" + ex.Message);
            }
        }
        #endregion

    }
}
