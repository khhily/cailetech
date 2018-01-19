using CL.Game.DAL;
using CL.Game.Entity;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 标准玩法加奖
/// </summary>
namespace CL.Game.BLL
{
    public class RegularNormBLL
    {
        Log log = new Log("RegularNormBLL");

        RegularNormDAL dal = new RegularNormDAL(Enum.Common.DbConnectionEnum.CaileGame);

        
        public int InsertEntity(RegularNormEntity Entity)
        {
            return dal.Insert(Entity) ?? 0;
        }
        public int ModifyEntity(RegularNormEntity Entity)
        {
            return dal.Update(Entity);
        }
        /// <summary>
        /// 查询加奖信息(标准玩法)
        /// 不包含阶梯加奖或等级加奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityNorm> QueryRegularNormAward(int LotteryCode)
        {
            return dal.QueryRegularNormAward(LotteryCode);
        }
        /// <summary>
        /// 标准玩法加奖
        /// </summary>
        /// <param name="Awards"></param>
        /// <returns></returns>
        public bool NormAward(List<udv_Awards> Awards)
        {
            return dal.NormAward(Awards);
        }
        /// <summary>
        /// 根据规则编号查询详细规则
        /// </summary>
        /// <param name="RegularID"></param>
        /// <returns></returns>
        public List<RegularNormEntity> QueryEntitys(int RegularID)
        {
            return dal.QueryEntitys(RegularID);
        }


        #region 计算加奖
        /// <summary>
        /// 加奖计算
        /// </summary>
        /// <param name="Entitys"></param>
        /// <param name="Chase_Entitys"></param>
        /// <returns></returns>
        public bool CalculateAward(List<udv_ComputeTicket> ComputeTickets, int LotteryCode)
        {
            try
            {
                //加奖内容
                List<udv_Awards> Awards = new List<udv_Awards>();
                if (ComputeTickets != null && ComputeTickets.Count > 0)
                {
                    List<udv_IsAwardActivityNorm> AwardActivitys = this.QueryRegularNormAward(LotteryCode);
                    if (AwardActivitys != null && AwardActivitys.Count > 0)
                    {
                        AwardActivitys.ForEach((Entity) =>
                        {
                            var Tickets = ComputeTickets.Where(w => w.PlayCode == Entity.PlayCode).ToList();
                            if (Tickets != null && Tickets.Count > 0)
                            {
                                Tickets.ForEach((Ticket) =>
                                {
                                    Awards.Add(new udv_Awards()
                                    {
                                        tid = Ticket.SchemeETicketsID,   //方案电子票
                                        oid = Ticket.SchemeID,           //方案标识
                                        rid = Entity.RegularID,          //规则标识
                                        at = Entity.ActivityType,        //加奖类型
                                        am = Entity.AwardMoney           //加奖金额
                                    });
                                });
                            }
                        });
                        //执行加奖操作
                        this.NormAward(Awards);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Write(string.Format("加奖计算错误：{0}", ex.Message), true);
                return false;
            }
        }


        #endregion
    }
}
