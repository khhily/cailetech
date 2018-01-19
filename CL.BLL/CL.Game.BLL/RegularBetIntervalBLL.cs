using CL.Game.DAL;
using CL.Game.Entity;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CL.Game.BLL
{
    public class RegularBetIntervalBLL
    {
        RegularBetIntervalDAL dal = new RegularBetIntervalDAL(Enum.Common.DbConnectionEnum.CaileGame);

        public int InsertEntity(RegularBetIntervalEntity Entity)
        {
            return dal.Insert(Entity) ?? 0;
        }
        public int ModifyEntity(RegularBetIntervalEntity Entity)
        {
            return dal.Update(Entity);
        }

        public List<RegularBetIntervalEntity> QueryEntitys(int RegularID)
        {
            return dal.QueryEntitys(RegularID);
        }
        /// <summary>
        /// 查询加奖信息(投注金额区间玩法)
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityBetInterval> QueryRegularBetIntervalAward(int LotteryCode)
        {
            return dal.QueryRegularBetIntervalAward(LotteryCode);
        }
        /// <summary>
        /// 投注金额累计区间加奖
        /// </summary>
        public void BetIntervalAward(int ActivityID, int RegularID, int PlayCode, long Min, long Max, long AwardMoney)
        {
            dal.BetIntervalAward(ActivityID, RegularID, PlayCode, Min, Max, AwardMoney);
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
                List<udv_IsAwardActivityBetInterval> AwardActivitys = this.QueryRegularBetIntervalAward(LotteryCode);
                if (AwardActivitys != null && AwardActivitys.Count > 0)
                {
                    AwardActivitys.ForEach((Entity) =>
                    {
                        if (!string.IsNullOrEmpty(Entity.BetInterval.Trim()))
                        {
                            //规则状态：0 初始化规则，1 规则作废(活动审核失败)，2 规则开始并生效(活动审核通过)，
                            //3 活动截止并开始加奖(这里加奖针对活动期间累计加奖规则)，4 活动结束并销毁(所有加奖派发完成后结束和销毁活动，销毁后的活动规则无法直接启用，启用销毁的规则需要重置及走审核流程)
                            if (Entity.RegularStatus == 2 || Entity.RegularStatus == 3)
                            {
                                XmlDocument doc = new XmlDocument();
                                try
                                {
                                    doc.LoadXml(Entity.BetInterval);
                                    XmlNodeList ItemList = doc.SelectNodes("root/item");
                                    foreach (XmlNode item in ItemList)
                                    {
                                        long min = Convert.ToInt64(item.SelectSingleNode("min").InnerText);
                                        long max = Convert.ToInt64(item.SelectSingleNode("max").InnerText);
                                        long award = Convert.ToInt64(item.SelectSingleNode("award").InnerText);
                                        //加奖派发
                                        this.BetIntervalAward(Entity.ActivityID, Entity.RegularID, Entity.PlayCode, min, max, award);
                                    }
                                }
                                catch
                                {
                                    throw;
                                }
                            }
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
