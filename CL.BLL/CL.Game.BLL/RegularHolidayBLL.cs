using CL.Game.DAL;
using CL.Game.Entity;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.BLL
{
    public class RegularHolidayBLL
    {
        Log log = new Log("RegularHolidayBLL");
        RegularHolidayDAL dal = new RegularHolidayDAL(Enum.Common.DbConnectionEnum.CaileGame);

        public int ModifyEntity(RegularHolidayEntity Entity)
        {
            return dal.Update(Entity);
        }
        public int InsertEntity(RegularHolidayEntity Entity)
        {
            return dal.Insert(Entity) ?? 0;
        }
        public List<RegularHolidayEntity> QueryEntitys(int RegularID)
        {
            return dal.QueryEntitys(RegularID);
        }

        /// <summary>
        /// 查询加奖信息(节假日玩法)
        /// 不包含阶梯加奖或等级加奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityHoliday> QueryRegularHolidayAward(int LotteryCode)
        {
            return dal.QueryRegularHolidayAward(LotteryCode);
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
                DateTime dt_day = DateTime.Now;
                List<udv_IsAwardActivityHoliday> AwardActivityHolidayEntitys = this.QueryRegularHolidayAward(LotteryCode);
                AwardActivityHolidayEntitys.ForEach((HolidayEntity) =>
                {
                    //类型：1 周六加奖，2 周日加奖，3 周六日加奖，4 指定时间段加奖(按活动开始结束时间加奖)
                    ////0 周日, 6 周六
                    switch (HolidayEntity.HolidayType)
                    {
                        case 1: //周六加奖
                            if (((int)dt_day.DayOfWeek) == 6)
                            {
                                ComputeTickets = ComputeTickets.Where(w => w.PlayCode == HolidayEntity.PlayCode).ToList();
                                if (ComputeTickets != null && ComputeTickets.Count > 0)
                                {
                                    ComputeTickets.ForEach((TicketEntity) =>
                                    {
                                        Awards.Add(new udv_Awards()
                                        {
                                            tid = TicketEntity.SchemeETicketsID,   //方案电子票
                                            oid = TicketEntity.SchemeID,           //方案标识
                                            rid = HolidayEntity.RegularID,          //规则标识
                                            at = HolidayEntity.ActivityType,        //加奖类型
                                            am = HolidayEntity.AwardMoney           //加奖金额
                                        });
                                    });
                                }
                            }
                            break;
                        case 2: //周日加奖
                            if (((int)dt_day.DayOfWeek) == 0)
                            {
                                ComputeTickets = ComputeTickets.Where(w => w.PlayCode == HolidayEntity.PlayCode).ToList();
                                if (ComputeTickets != null && ComputeTickets.Count > 0)
                                {
                                    ComputeTickets.ForEach((TicketEntity) =>
                                    {
                                        Awards.Add(new udv_Awards()
                                        {
                                            tid = TicketEntity.SchemeETicketsID,   //方案电子票
                                            oid = TicketEntity.SchemeID,           //方案标识
                                            rid = HolidayEntity.RegularID,          //规则标识
                                            at = HolidayEntity.ActivityType,        //加奖类型
                                            am = HolidayEntity.AwardMoney           //加奖金额
                                        });
                                    });
                                }
                            }
                            break;
                        case 3: //周六日加奖
                            if (((int)dt_day.DayOfWeek) == 0 || ((int)dt_day.DayOfWeek) == 6)
                            {
                                ComputeTickets = ComputeTickets.Where(w => w.PlayCode == HolidayEntity.PlayCode).ToList();
                                if (ComputeTickets != null && ComputeTickets.Count > 0)
                                {
                                    ComputeTickets.ForEach((TicketEntity) =>
                                    {
                                        Awards.Add(new udv_Awards()
                                        {
                                            tid = TicketEntity.SchemeETicketsID,    //方案电子票
                                            oid = TicketEntity.SchemeID,            //方案标识
                                            rid = HolidayEntity.RegularID,          //规则标识
                                            at = HolidayEntity.ActivityType,        //加奖类型
                                            am = HolidayEntity.AwardMoney           //加奖金额
                                        });
                                    });
                                }
                            }
                            break;
                        case 4: //指定时间段加奖(按活动开始结束时间加奖)
                            ComputeTickets = ComputeTickets.Where(w => w.PlayCode == HolidayEntity.PlayCode).ToList();
                            if (ComputeTickets != null && ComputeTickets.Count > 0)
                            {
                                ComputeTickets.ForEach((TicketEntity) =>
                                {
                                    Awards.Add(new udv_Awards()
                                    {
                                        tid = TicketEntity.SchemeETicketsID,   //方案电子票
                                        oid = TicketEntity.SchemeID,           //方案标识
                                        rid = HolidayEntity.RegularID,          //规则标识
                                        at = HolidayEntity.ActivityType,        //加奖类型
                                        am = HolidayEntity.AwardMoney           //加奖金额
                                    });
                                });
                            }
                            break;
                    }
                    new RegularNormBLL().NormAward(Awards);
                });
                return true;
            }
            catch (Exception ex)
            {
                log.Write("加奖计算错误：" + ex.Message, true);
                return false;
            }
        }
        #endregion
    }
}
