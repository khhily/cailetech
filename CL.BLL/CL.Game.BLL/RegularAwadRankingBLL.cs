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
    public class RegularAwadRankingBLL
    {
        RegularAwadRankingDAL dal = new RegularAwadRankingDAL(Enum.Common.DbConnectionEnum.CaileGame);

        public int InsertEntity(RegularAwardRankingEntity Entity)
        {
            return dal.Insert(Entity) ?? 0;
        }

        public int ModifyEntity(RegularAwardRankingEntity Entity)
        {
            return dal.Update(Entity);
        }
        public List<RegularAwardRankingEntity> QueryEntitys(int RegularID)
        {
            return dal.QueryEntitys(RegularID);
        }

        /// <summary>
        /// 查询加奖信息(中奖金额名次玩法)
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityAwardRanking> QueryRegularAwardRankingAward(int LotteryCode)
        {
            return dal.QueryRegularAwardRankingAward(LotteryCode);
        }

        /// <summary>
        /// 中奖金额累计名次加奖
        /// </summary>
        public void AwardRankingAward(int ActivityID, int RegularID, int PlayCode, long Placing, long AwardMoney)
        {
            dal.AwardRankingAward(ActivityID, RegularID, PlayCode, Placing, AwardMoney);
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
                List<udv_IsAwardActivityAwardRanking> AwardActivitys = this.QueryRegularAwardRankingAward(LotteryCode);
                if (AwardActivitys != null && AwardActivitys.Count > 0)
                {
                    AwardActivitys.ForEach((Entity) =>
                    {
                        if (!string.IsNullOrEmpty(Entity.AwardRanking.Trim()))
                        {
                            //规则状态：0 初始化规则，1 规则作废(活动审核失败)，2 规则开始并生效(活动审核通过)，
                            //3 活动截止并开始加奖(这里加奖针对活动期间累计加奖规则)，4 活动结束并销毁(所有加奖派发完成后结束和销毁活动，销毁后的活动规则无法直接启用，启用销毁的规则需要重置及走审核流程)
                            if (Entity.RegularStatus == 2 || Entity.RegularStatus == 3)
                            {
                                XmlDocument doc = new XmlDocument();
                                try
                                {
                                    doc.LoadXml(Entity.AwardRanking);
                                    XmlNodeList ItemList = doc.SelectNodes("root/item");
                                    foreach (XmlNode item in ItemList)
                                    {
                                        int placing = Convert.ToInt32(item.SelectSingleNode("placing").InnerText);
                                        long award = Convert.ToInt64(item.SelectSingleNode("award").InnerText);
                                        //加奖派发
                                        this.AwardRankingAward(Entity.ActivityID, Entity.RegularID, Entity.PlayCode, placing, award);
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
