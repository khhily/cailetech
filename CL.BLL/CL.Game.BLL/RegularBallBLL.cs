using CL.Enum.Common.Lottery;
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
    public class RegularBallBLL
    {
        Log log = new Log("RegularBallBLL");
        RegularBallDAL dal = new RegularBallDAL(Enum.Common.DbConnectionEnum.CaileGame);

        public int InsertEntity(RegularBallEntity Entity)
        {
            return dal.Insert(Entity) ?? 0;
        }
        public int ModifyEntity(RegularBallEntity Entity)
        {
            return dal.Update(Entity);
        }
        public List<RegularBallEntity> QueryEntitys(int RegularID)
        {
            return dal.QueryEntitys(RegularID);
        }

        /// <summary>
        /// 查询加奖信息(数字彩中球玩法)
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityBall> QueryRegularBetIntervalAward(int LotteryCode)
        {
            return dal.QueryRegularBetIntervalAward(LotteryCode);
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
                    List<udv_IsAwardActivityBall> AwardActivitys = this.QueryRegularBetIntervalAward(LotteryCode);
                    if (AwardActivitys != null && AwardActivitys.Count > 0)
                    {
                        AwardActivitys.ForEach((Entity) =>
                        {
                            var Tickets = ComputeTickets.Where(w => w.PlayCode == Entity.PlayCode).ToList();
                            if (Tickets != null && Tickets.Count > 0)
                            {
                                Tickets.ForEach((Ticket) =>
                                {
                                    string[] SpecifyAwardBall = Entity.Ball.Split(',');
                                    int AwardBallLength = 0;
                                    if (LotteryCode != (int)LotteryInfo.SSQ && LotteryCode != (int)LotteryInfo.CJDLT)
                                    {
                                        for (int i = 0; i < SpecifyAwardBall.Length; i++)
                                            if (Ticket.Number.Contains(SpecifyAwardBall[i]) && Ticket.OpenNumber.Contains(SpecifyAwardBall[i]))
                                                AwardBallLength++;
                                    }
                                    else
                                    {
                                        //类型：1 指定红球数字，2 指定蓝球数字(高频彩没有蓝球)
                                        if (LotteryCode == (int)LotteryInfo.SSQ)
                                        {
                                            if (Entity.BallType == 1)
                                            {
                                                string Red_Num = Ticket.Number.Split('|')[0];
                                                string[] Red_OpenNum = { Ticket.OpenNumber.Split(' ')[0], Ticket.OpenNumber.Split(' ')[1], Ticket.OpenNumber.Split(' ')[2], Ticket.OpenNumber.Split(' ')[3], Ticket.OpenNumber.Split(' ')[4], Ticket.OpenNumber.Split(' ')[5] };
                                                for (int i = 0; i < SpecifyAwardBall.Length; i++)
                                                    if (Red_Num.Contains(SpecifyAwardBall[i]) && Red_OpenNum.Contains(SpecifyAwardBall[i]))
                                                        AwardBallLength++;
                                            }
                                            else
                                            {
                                                string Red_Num = Ticket.Number.Split('|')[1];
                                                string Red_OpenNum = Ticket.OpenNumber.Split(' ')[6];
                                                for (int i = 0; i < SpecifyAwardBall.Length; i++)
                                                    if (Red_Num.Contains(SpecifyAwardBall[i]) && Red_OpenNum.Contains(SpecifyAwardBall[i]))
                                                        AwardBallLength++;
                                            }
                                        }
                                        else
                                        {
                                            if (Entity.BallType == 1)
                                            {
                                                string Red_Num = Ticket.Number.Split('|')[0];
                                                string[] Red_OpenNum = { Ticket.OpenNumber.Split(' ')[0], Ticket.OpenNumber.Split(' ')[1], Ticket.OpenNumber.Split(' ')[2], Ticket.OpenNumber.Split(' ')[3], Ticket.OpenNumber.Split(' ')[4] };
                                                for (int i = 0; i < SpecifyAwardBall.Length; i++)
                                                    if (Red_Num.Contains(SpecifyAwardBall[i]) && Red_OpenNum.Contains(SpecifyAwardBall[i]))
                                                        AwardBallLength++;
                                            }
                                            else
                                            {
                                                string Red_Num = Ticket.Number.Split('|')[1];
                                                string[] Red_OpenNum = { Ticket.OpenNumber.Split(' ')[5], Ticket.OpenNumber.Split(' ')[6] };
                                                for (int i = 0; i < SpecifyAwardBall.Length; i++)
                                                    if (Red_Num.Contains(SpecifyAwardBall[i]) && Red_OpenNum.Contains(SpecifyAwardBall[i]))
                                                        AwardBallLength++;
                                            }
                                        }
                                    }
                                    if (AwardBallLength == SpecifyAwardBall.Length)
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
                        new RegularNormBLL().NormAward(Awards);
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
