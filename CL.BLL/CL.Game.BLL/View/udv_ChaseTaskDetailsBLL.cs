using CL.Enum.Common;
using CL.Game.DAL.View;
using CL.Game.Entity;
using CL.Tools.Common;
using CL.Tools.MSMQManager;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CL.Game.BLL.View
{
    public class udv_ChaseTaskDetailsBLL
    {
        Log log = new Log("udv_ChaseTaskDetailsBLL");

        udv_ChaseTaskDetailsDAL dal = new udv_ChaseTaskDetailsDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 查询当前彩种的追号任务
        /// </summary>
        /// <param name="LotteryCode">彩种编码</param>
        /// <returns></returns>
        public List<udv_ChaseTaskDetails> QueryModelList(int LotteryCode)
        {
            return dal.QueryModelList(LotteryCode);
        }

        #region 自定义方法

        /// <summary>
        /// 处理停止追号
        /// </summary>
        /// <param name="LotteryCode"></param>
        public void StopChaseTask(int LotteryCode)
        {
            try
            {
                #region 已结期还没追号撤单
                var ChaseRevokes = new udv_ChaseRevokeBLL().QueryRevoke(LotteryCode);
                foreach (var item in ChaseRevokes)
                {
                    #region DB操作
                    new ChaseTaskDetailsBLL().ChaseRevoke(item.SchemeID, item.ID, item.UserID, item.Amount);
                    #endregion

                    #region Redis操作
                    //new SchemesBLL().ModifyRedisSchemeStatus(item.SchemeID, item.ID, 12, 0);
                    new UsersBLL().ModifyUserBalanceRedis(item.UserID, item.Amount, true);
                    #endregion
                }
                #endregion
                #region 追号完成条件撤单
                var overEntitys = new udv_OverChaseTasksExamineBLL().QueryModelList(LotteryCode);
                if (overEntitys.Count > 0)
                {
                    foreach (var entity in overEntitys)
                    {
                        //判断追号规则
                        if (entity.StopTypeWhenWinMoney == -1)
                        {
                            //完成追号任务
                            var Nums = new ChaseTaskDetailsBLL().StopChaseTaskQuery(entity.ChaseTaskID, 1);
                            if (Nums == 0)
                            {
                                //停止追号内容
                                new ChaseTaskDetailsBLL().StopChaseTask(entity.SchemeID, 0, 0, 1);
                            }
                        }
                        else if (entity.StopTypeWhenWinMoney == 0)
                        {
                            //中奖停止追号
                            var win = new ChaseTaskDetailsBLL().StopChaseTaskQuery(entity.ChaseTaskID, 2);
                            if (win > 0)
                            {
                                //中奖停止追号
                                var ls = new ChaseTaskDetailsBLL().QueryModelListByChaseTaskID(entity.SchemeID, entity.ChaseTaskID);
                                foreach (ChaseTaskDetailsEntity item in ls)//停止追号内容
                                    new ChaseTaskDetailsBLL().StopChaseTask(entity.SchemeID, item.ID, item.Amount, 2);
                            }
                        }
                        else if (entity.StopTypeWhenWinMoney > 0)
                        {
                            //中奖金额累计达到停止追号
                            var win = new ChaseTaskDetailsBLL().StopChaseTaskQuery(entity.ChaseTaskID, 2);
                            if (win >= entity.StopTypeWhenWinMoney)
                            {
                                //累计中奖金额大于等于停止追号
                                var ls = new ChaseTaskDetailsBLL().QueryModelListByChaseTaskID(entity.SchemeID, entity.ChaseTaskID);
                                foreach (ChaseTaskDetailsEntity item in ls)//停止追号内容
                                    new ChaseTaskDetailsBLL().StopChaseTask(entity.SchemeID, item.ID, item.Amount, 2);

                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                log.Write("处理停止追号:" + ex.StackTrace, true);
            }
        }
        /// <summary>
        /// 追号任务(处理并发送队列)
        /// </summary>
        /// <param name="LotteryCode"></param>
        public void ChaseTaskOutTickets(int LotteryCode)
        {
            try
            {
                StopChaseTask(LotteryCode);
                List<udv_ChaseTaskDetails> ChaseTasks = this.QueryModelList(LotteryCode);
                foreach (udv_ChaseTaskDetails Chase in ChaseTasks)
                {
                    var SchemeDetails = new SchemesDetailBLL().QueryEntityBySchemeID(Chase.SchemeID).Select(s => new udv_Parameter()
                    {
                        Amount = Chase.Amount,
                        Bet = s.BetNum,
                        IsNorm = s.IsNorm == 1 ? true : false,
                        LotteryCode = Chase.LotteryCode,
                        Multiple = Chase.Multiple,
                        Number = s.BetNumber,
                        SchemeID = Chase.SchemeID,
                        PlayCode = s.PlayCode,
                        UserCode = Chase.UserID
                    }).ToList();
                    ElectronicTicketSender ticket = new ElectronicTicketSender();
                    ticket.SchemeTicket.SchemeID = Chase.SchemeID;
                    ticket.SchemeTicket.LotteryCode = Chase.LotteryCode;
                    ticket.SchemeTicket.StartTime = Chase.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ticket.SchemeTicket.EndTime = Chase.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ticket.SchemeTicket.IsuseName = Chase.IsuseName;
                    ticket.SchemeTicket.SchemeMoney = Chase.Amount;
                    ticket.SchemeTicket.TicketDetails = SchemeDetails;
                    ticket.SchemeTicket.ChaseTaskDetailsID = Chase.ChaseTaskDetailsID;
                    if (!ticket.Sender())
                    {
                        log.Write("追号电子票发送失败：追号标识[" + Chase.ChaseTaskDetailsID + "]");
                    }
                    else
                    {
                        var entity = new ChaseTaskDetailsBLL().QueryEntity(Chase.ChaseTaskDetailsID);
                        entity.IsExecuted = true;
                        new ChaseTaskDetailsBLL().ModifyEntity(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Write("追号任务:" + ex.StackTrace, true);
            }
        }

        #endregion

    }
}
