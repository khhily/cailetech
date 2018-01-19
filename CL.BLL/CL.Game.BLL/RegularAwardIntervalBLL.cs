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
    public class RegularAwardIntervalBLL
    {
        RegularAwardIntervalDAL dal = new RegularAwardIntervalDAL(Enum.Common.DbConnectionEnum.CaileGame);

        public int InsertEntity(RegularAwardIntervalEntity Entity)
        {
            return dal.Insert(Entity) ?? 0;
        }
        public int ModifyEntity(RegularAwardIntervalEntity Entity)
        {
            return dal.Update(Entity);
        }

        public List<RegularAwardIntervalEntity> QueryEntitys(int RegularID)
        {
            return dal.QueryEntitys(RegularID);
        }

        /// <summary>
        /// 查询加奖信息(中奖金额区间玩法)
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityAwardInterval> QueryRegularAwardIntervalAward(int LotteryCode)
        {
            return dal.QueryRegularAwardIntervalAward(LotteryCode);
        }
        /// <summary>
        /// 中奖金额累计区间加奖
        /// </summary>
        public void AwardIntervalAward(int ActivityID, int RegularID, int PlayCode, long Min, long Max, long AwardMoney)
        {
            dal.AwardIntervalAward(ActivityID, RegularID, PlayCode, Min, Max, AwardMoney);
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
                List<udv_IsAwardActivityAwardInterval> AwardActivitys = this.QueryRegularAwardIntervalAward(LotteryCode);
                if (AwardActivitys != null && AwardActivitys.Count > 0)
                {
                    AwardActivitys.ForEach((Entity) =>
                    {
                        if (!string.IsNullOrEmpty(Entity.AwardInterval.Trim()))
                        {
                            //规则状态：0 初始化规则，1 规则作废(活动审核失败)，2 规则开始并生效(活动审核通过)，
                            //3 活动截止并开始加奖(这里加奖针对活动期间累计加奖规则)，4 活动结束并销毁(所有加奖派发完成后结束和销毁活动，销毁后的活动规则无法直接启用，启用销毁的规则需要重置及走审核流程)
                            if (Entity.RegularStatus == 2 || Entity.RegularStatus == 3)
                            {
                                XmlDocument doc = new XmlDocument();
                                try
                                {
                                    doc.LoadXml(Entity.AwardInterval);
                                    XmlNodeList ItemList = doc.SelectNodes("root/item");
                                    foreach (XmlNode item in ItemList)
                                    {
                                        long min = Convert.ToInt64(item.SelectSingleNode("min").InnerText);
                                        long max = Convert.ToInt64(item.SelectSingleNode("max").InnerText);
                                        long award = Convert.ToInt64(item.SelectSingleNode("award").InnerText);
                                        //加奖派发
                                        this.AwardIntervalAward(Entity.ActivityID, Entity.RegularID, Entity.PlayCode, min, max, award);
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
