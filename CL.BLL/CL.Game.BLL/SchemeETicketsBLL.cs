//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//     2017-04-26 11:36:00 Created by LEON
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using CL.Enum.Common;
using CL.Game.DAL;
using CL.Game.Entity;
using CL.View.Entity.ElectronicTicket;
using CL.View.Entity.Interface;
using System.Collections.Generic;
using System.Linq;
using System;
using CL.Entity.Json.WebAPI;
using CL.Tools.Common;
using CL.Redis.BLL;
using CL.View.Entity.Game;
using CL.Enum.Common.Lottery;
using System.Text;

namespace CL.Game.BLL
{

    /// <summary>
    ///SchemeETickets info
    /// </summary>
    public class SchemeETicketsBLL
    {
        Log log = new Log("SchemeETicketsBLL");
        SchemeETicketsDAL dal = new SchemeETicketsDAL(DbConnectionEnum.CaileGame);
        /// <summary>
        /// 撤单处理
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="IsSystemQuash">是否系统撤单</param>
        /// <param name="ReturnValue"></param>
        /// <param name="ReturnDescription"></param>
        /// <returns></returns>
        public bool QuashScheme(long SchemeID, bool IsSystemQuash, ref string ReturnDescription)
        {
            return dal.QuashScheme(SchemeID, IsSystemQuash, ref ReturnDescription);
        }
        /// <summary>
        /// 更新出票状态
        /// </summary>
        /// <returns></returns>
        public bool HandleOutTicket(List<udv_OutTicketEntites> ListOutTicker, List<long> SchemeIDList)
        {

            return dal.HandleOutTicket(ListOutTicker, SchemeIDList);
        }
        /// <summary>
        /// 自动算奖派奖
        /// </summary>
        public bool AutomaticComputeWin(List<udv_WinInfoEntites> SqlPara, List<long> SchemeIDList, string InterfaceType)
        {
            return dal.AutomaticComputeWin(SqlPara, SchemeIDList, InterfaceType);
        }
        /// <summary>
        /// 电子票入库
        /// </summary>
        public bool TicketsStorage(long SchemeID, long ChaseTaskDetailsID, List<udv_Ticket> SqlPara)
        {
            var ttb = SqlPara.Select(s => new CL.View.Entity.Game.udv_ts
            {
                pc = s.PlayTypeCode,
                at = s.Money * 100,
                mp = s.Multiple,
                n = s.Number,
                sdid = s.SDID,
                ts = 1,
                s = 0
            }).ToList();
            return dal.TicketsStorage(SchemeID, ChaseTaskDetailsID, ttb);
        }
        /// <summary>
        /// 电子票入库
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="ChaseTaskDetailsID"></param>
        /// <param name="SqlPara"></param>
        /// <returns></returns>
        public bool TicketsStorage_Robot(long SchemeID, long ChaseTaskDetailsID, List<udv_Ticket> SqlPara)
        {
            var ttb = SqlPara.Select(s => new CL.View.Entity.Game.udv_ts
            {
                pc = s.PlayTypeCode,
                at = s.Money * 100,
                mp = s.Multiple,
                n = s.Number,
                sdid = s.SDID,
                ts = 1,
                s = 2
            }).ToList();
            return dal.TicketsStorage_Robot(SchemeID, ChaseTaskDetailsID, ttb);
        }
        /// <summary>
        /// 电子票投注
        /// </summary>
        /// <returns></returns>
        public bool TicketBetting(List<udv_BettingEntites> TicketNoticeList, long SchemeID, long ChaseTaskDetailsID)
        {
            var tb = TicketNoticeList.Select(s => new CL.View.Entity.Game.udv_tb
            {
                idf = s.TicketID,
                seid = Convert.ToInt64(s.SchemeETicketID),
                msg = s.Msg,
                s = (byte)(s.State == 0 ? 1 : 3)
            }).ToList();
            return dal.TicketBetting(tb, SchemeID, ChaseTaskDetailsID);
        }
        /// <summary>
        /// 投注失败撤单
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="ErrorDescription"></param>
        /// <returns></returns>
        public bool SchemeETicketsWithdrawal(long SchemeID, string ErrorDescription)
        {
            return dal.SchemeETicketsWithdrawal(SchemeID, ErrorDescription);
        }
        /// <summary>
        /// 过滤投注号码
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <param name="Number"></param>
        /// <returns></returns>
        public string FilterBetNum(long OrderCode, string Number)
        {
            try
            {
                var Entity = new SchemesBLL().QuerySchemeEntity(OrderCode);
                if (Entity != null)
                {
                    StringBuilder Num = new StringBuilder();
                    if (Entity.LotteryCode == (int)LotteryInfo.CQSSC || Entity.LotteryCode == (int)LotteryInfo.JLK3 || Entity.LotteryCode == (int)LotteryInfo.JXK3)
                    {
                        int m = 1;
                        foreach (string item in Number.Split('|'))
                        {
                            string itemStr = item.Replace("*", "");
                            if (!string.IsNullOrEmpty(Num.ToString()))
                                Num.Append("| ");
                            for (int i = 0; i < itemStr.Length; i++)
                                Num.AppendFormat("{0} ", itemStr.Substring(i, m));
                        }
                    }
                    else
                    {
                        int m = 2;
                        foreach (string item in Number.Split('|'))
                        {
                            if (!string.IsNullOrEmpty(Num.ToString()))
                                Num.Append("| ");
                            for (int i = 0; i < item.Length / 2; i++)
                                Num.AppendFormat("{0} ", item.Substring(i * m, m));
                        }
                    }

                    return Num.ToString().Trim();
                }
                else
                    return Number;
            }
            catch (Exception ex)
            {
                log.Write("过滤投注号码错误:" + ex.StackTrace, true);
                return Number;
            }
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeETicketsID"></param>
        /// <returns></returns>
        public SchemeETicketsEntity QueryEntity(long SchemeETicketsID)
        {
            return dal.QueryEntity(SchemeETicketsID);
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="SchemeETicketsID"></param>
        /// <returns></returns>
        public List<SchemeETicketsEntity> QueryEntitysBySchemeIDSDID(long SchemeID, long ChaseTaskDetailsID)
        {
            return dal.QueryEntitysBySchemeIDSDID(SchemeID, ChaseTaskDetailsID);
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="SchemeETicketsID"></param>
        /// <returns></returns>
        public List<SchemeETicketsEntity> QueryEntitysBySchemeIDSDID(long SchemeID, long ChaseTaskDetailsID, int PageIndex, int PageSize)
        {
            return dal.QueryEntitysBySchemeIDSDID(SchemeID, ChaseTaskDetailsID, PageIndex, PageSize);
        }
        /// <summary>
        /// 根据方案编号查询电子票
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public List<SchemeETicketsEntity> QueryEntitysBySchemeID(long SchemeID)
        {
            return dal.QueryEntitysBySchemeID(SchemeID);
        }
        /// <summary>
        /// 出票查询统计
        /// </summary>
        /// <param name="StartTime">起止时间_起</param>
        /// <param name="EndTime">岂止时间_止</param>
        /// <param name="LotteryCode">彩种</param>
        /// <param name="SourceID">出票商</param>
        /// <param name="SchemeNumber">订单编号</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="RecordAmount">订单总额</param>
        /// <param name="RecordWin">中奖总额</param>
        /// <param name="RecordCount">查询总数</param>
        /// <returns></returns>
        public List<udv_OutTicketReport> QueryOutTicketReport(DateTime StartTime, DateTime EndTime, int LotteryCode, int SourceID, string SchemeNumber, int PageIndex, int PageSize, ref long RecordAmount, ref long RecordWin, ref int RecordCount)
        {
            return dal.QueryOutTicketReport(StartTime, EndTime, LotteryCode, SourceID, SchemeNumber, PageIndex, PageSize, ref RecordAmount, ref RecordWin, ref RecordCount);
        }
        /// <summary>
        /// 优化查询中奖电子票sql
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public List<cv_Ticket> QueryWinMoneyListBySchemeID(long SchemeID)
        {
            return dal.QueryWinMoneyListBySchemeID(SchemeID);
        }


        #region 管理后台查询某期重新派奖数据
        /// <summary>
        /// 查询手动返奖数据
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <returns></returns>
        public List<udv_SchemeETicketAward> QuerySchemeETicketAward(int LotteryCode, string IsuseName)
        {
            return dal.QuerySchemeETicketAward(LotteryCode, IsuseName);
        }
        /// <summary>
        /// 手动算奖
        /// </summary>
        /// <param name="Entitys"></param>
        /// <returns></returns>
        public int ReplenishAward(List<udv_Award> Entitys)
        {
            return dal.ReplenishAward(Entitys);
        }
        #endregion


        #region 自定义方法

        /// <summary>
        /// 电子票详情查询
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        /// <param name="OrderCode"></param>
        /// <param name="ChaseTaskDetailsID"></param>
        /// <returns></returns>
        public SchemeTicketsResult LookETickets(string Token, long UserCode, long OrderCode, long ChaseTaskDetailsID, int PageIndex, int PageSize)
        {
            SchemeTicketsResult result = null;
            try
            {
                int RecCode = (int)ResultCode.NullData;
                List<SchemeTicketsInfo> ticketData = new List<SchemeTicketsInfo>();
                var TicketEntitys = this.QueryEntitysBySchemeIDSDID(OrderCode, ChaseTaskDetailsID, PageIndex, PageSize);
                if (TicketEntitys != null && TicketEntitys.Count > 0)
                {
                    RecCode = (int)ResultCode.Success;
                    ticketData = TicketEntitys.Select(s => new SchemeTicketsInfo()
                    {
                        Number = FilterBetNum(OrderCode, s.Number),
                        PlayCode = s.PlayCode,
                        Tickets = GenerateTickets(s.Identifiers, s.TicketStatus, s.SchemeETicketsID),
                        TicketStatus = s.TicketStatus,
                        WinMoney = s.WinMoney,
                        Multiple = s.Multiple
                    }).ToList();
                }
                result = new SchemeTicketsResult()
                {
                    Code = RecCode,
                    Msg = Common.GetDescription((ResultCode)RecCode),
                    Data = ticketData
                };
            }
            catch (Exception ex)
            {
                log.Write("电子票详情查询[LookETickets]：" + ex.StackTrace, true);
                result = new SchemeTicketsResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return result;
        }

        /// <summary>
        /// 处理电子票票号
        /// 状态,0.待出票，1.投注成功，2.出票完成，3.投注失败，4.出票失败，8.兑奖中，10中奖，11.不中
        /// </summary>
        /// <param name="Identifiers"></param>
        /// <param name="TicketStatus"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GenerateTickets(string Identifiers, byte TicketStatus, long ID)
        {

            string Tickets = string.Empty;
            switch (TicketStatus)
            {
                case 0:
                case 1:
                case 3:
                case 4:
                    Tickets = string.IsNullOrEmpty(Identifiers.Trim()) == true ? "" : Identifiers;
                    break;
                case 2:
                case 8:
                case 10:
                case 11:
                    Tickets = string.IsNullOrEmpty(Identifiers.Trim()) == true ? EncryptAlgorithm.MD5(ID.ToString().ToLower()) : Identifiers;
                    break;
            }
            return Tickets;
        }
        #endregion
    }
}
