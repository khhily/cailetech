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
using CL.Json.Entity;
using CL.Redis.BLL;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Game.BLL
{

    /// <summary>
    ///UsersPayDetail info
    /// </summary>
    public class UsersPayDetailBLL
    {

        Log log = new Log("UsersPayDetailBLL");
        UsersPayDetailDAL dal = new UsersPayDetailDAL(DbConnectionEnum.CaileGame);


        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long InsertEntity(UsersPayDetailEntity entity)
        {
            return dal.InsertEntity(entity);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="PayID"></param>
        /// <returns></returns>
        public UsersPayDetailEntity QueryPayDetailsByPayID(long PayID)
        {
            return dal.GetList(new { PayID = PayID }).FirstOrDefault();
        }
        /// <summary>
        /// 充值补单
        /// </summary>
        /// <param name="PayID"></param>
        /// <param name="RechargeNo">接口交易号</param>
        /// <returns></returns>
        public int Replenishment(long PayID, string RechargeNo, string OutRechargeNo)
        {
            return dal.Replenishment(PayID, RechargeNo, OutRechargeNo);
        }
        /// <summary>
        /// 退款单号是否存在
        /// </summary>
        /// <param name="IsOutRefundNo"></param>
        /// <returns></returns>
        public bool IsOutRefundNo(string RefundNo)
        {
            return dal.IsOutRefundNo(RefundNo);
        }
        /// <summary>
        /// 订单号是否存在
        /// 存在返回false
        /// </summary>
        /// <param name="tbout_trade_no"></param>
        /// <returns></returns>
        public bool IsTboutTradeNo(string tbout_trade_no)
        {
            return dal.IsTboutTradeNo(tbout_trade_no);
        }
        public UsersPayDetailEntity QueryPayDetailsByOrderNo(string OrderNo)
        {
            return dal.GetList(new { OrderNo = OrderNo }).FirstOrDefault();
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<udv_UserPay> QueryListByPage(string keywords, int iType, int pageSize, int pageIndex, ref int recordCount)
        {
            return dal.QueryListByPage(keywords, iType, pageSize, pageIndex, ref recordCount);
        }
        /// <summary>
        /// 报表:充值查询统计
        /// </summary>
        /// <param name="StartTime">起止时间</param>
        /// <param name="EndTime">起止时间</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="OrderNo">平台订单编号</param>
        /// <param name="RechargeNo">第三方订单编号</param>
        /// <param name="PayType">充值方式</param>
        /// <param name="Result">状态</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="RecordPayAmount">充值总额</param>
        /// <param name="RecordCount">查询总记录</param>
        /// <returns></returns>
        public List<udv_ReportPayDetail> QuertPayDetailReport(DateTime StartTime, DateTime EndTime, long UserID, string OrderNo, string RechargeNo, string PayType, int Result, int PageIndex, int PageSize, ref long RecordPayAmount, ref int RecordCount)
        {
            return dal.QuertPayDetailReport(StartTime, EndTime, UserID, OrderNo, RechargeNo, PayType, Result, PageIndex, PageSize, ref RecordPayAmount, ref RecordCount);
        }

        #region 自定义方法
        /// <summary>
        /// 快捷支付验证
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="UserCode"></param>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public JsonResult VerifyPaymentShortcut(long UserCode, string OrderNo)
        {
            JsonResult result = null;
            try
            {
                int RecCode = (int)ResultCode.Success;
                var PayDetailEntity = QueryPayDetailsByOrderNo(OrderNo);
                if (PayDetailEntity == null || PayDetailEntity.Result != 1)
                    RecCode = (int)ResultCode.WaitPayment;
                result = new JsonResult()
                {
                    Code = RecCode,
                    Msg = Common.GetDescription((ResultCode)RecCode)
                };
            }
            catch (Exception ex)
            {
                log.Write("快捷支付验证错误[VerifyPaymentShortcut]：" + ex.StackTrace, true);
                result = new JsonResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return result;
        }

        #endregion
    }
}