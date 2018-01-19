using CL.Tools.Common;
using CL.View.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Xml;

namespace CL.Tools.TicketInterface.CaiLe
{
    public class CaiLeHandle : InterfaceBase
    {
        private readonly Log log = new Log("CaiLeHandle");

        #region 接口数据
        /// <summary>
        /// 代理编号
        /// </summary>
        private string agenterid = string.Empty;
        /// <summary>
        /// 代理密码
        /// </summary>
        private string agenterpwd = string.Empty;
        /// <summary>
        /// 用户登陆号
        /// </summary>
        private string username = string.Empty;
        /// <summary>
        /// 接口彩种编号
        /// </summary>
        private int InterfaceLotteryCode = 0;
        /// <summary>
        /// 接口地址
        /// </summary>
        private string InterfaceAddress = string.Empty;
        /// <summary>
        /// 当前系统彩种编号
        /// </summary>
        private int SystemLotteryCode = 0;
        /// <summary>
        /// 是否使用内部接口
        /// </summary>
        private bool IsInternal = false;
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="xml"></param>
        public CaiLeHandle(XmlNode xml)
        {
            try
            {
                BindInterfaceInfo(xml);
            }
            catch (Exception ex)
            {
                log.Write("彩乐内部接口读取配置信息错误：" + ex.Message, true);
                throw;
            }
        }
        public CaiLeHandle(string _agenterid, string _agenterpwd, string _username, string _InterfaceAddress,
            int _InterfaceLotteryCode, int _SystemLotteryCode, bool _IsInternal)
        {
            agenterid = _agenterid;
            agenterpwd = _agenterpwd;
            username = _username;
            InterfaceAddress = _InterfaceAddress;
            InterfaceLotteryCode = _InterfaceLotteryCode;
            SystemLotteryCode = _SystemLotteryCode;
            IsInternal = _IsInternal;
        }

        private void BindInterfaceInfo(XmlNode xml)
        {
            agenterid = xml.SelectSingleNode("AgenterId").InnerText;
            agenterpwd = xml.SelectSingleNode("AgenterPwd").InnerText;
            username = xml.SelectSingleNode("UserName").InnerText;
            InterfaceAddress = xml.SelectSingleNode("InterfaceAddress").InnerText;
            InterfaceLotteryCode = Convert.ToInt32(xml.SelectSingleNode("InterfaceLotteryCode").InnerText);
            SystemLotteryCode = Convert.ToInt32(xml.SelectSingleNode("SystemLotteryCode").InnerText);
            IsInternal = Convert.ToInt32(xml.SelectSingleNode("InternalOutTicket").InnerText) == 1;
        }

        #region 接口方法
        /// <summary>
        /// 电子票投注
        /// </summary>
        public override List<udv_ResultBetting> HandleTicketBetting(List<udv_ParaBettingTicker> para)
        {
            try
            {
                List<udv_ResultBetting> ListModel = new List<udv_ResultBetting>();

                udv_ResultBetting ResModel = new udv_ResultBetting();
                List<udv_BettingEntites> ListTicker = new List<udv_BettingEntites>();
                ResModel.ErrorCode = "0";
                ResModel.ErrorMsg = "内部投注成功";
                foreach (udv_ParaBettingTicker item in para)
                {
                    udv_BettingEntites model = new udv_BettingEntites();
                    model.SchemeETicketID = item.SchemeETicketID.ToString();
                    model.TicketID = Guid.NewGuid().ToString("N");
                    model.ActMoney = "0";
                    model.State = 0;
                    model.Msg = "内部投注成功";
                    ListTicker.Add(model);
                }
                ResModel.ListTicker = ListTicker;
                ListModel.Add(ResModel);
                return ListModel;
            }
            catch (Exception ex)
            {
                log.Write("电子票投注:" + ex.StackTrace, true);
                return null;
            }
        }

        /// <summary>
        /// 处理出票数据
        /// </summary>
        /// <returns></returns>
        public override List<udv_ResultOutTicket> HandleOutTicket(List<udv_ParaOutTicket> para)
        {
            try
            {
                List<udv_ResultOutTicket> ListModel = new List<udv_ResultOutTicket>();
                udv_ResultOutTicket ResModel = new udv_ResultOutTicket();
                List<udv_OutTicketEntites> ListOutTicker = new List<udv_OutTicketEntites>();
                ResModel.ErrorCode = "0";
                ResModel.ErrorMsg = "内部出票成功";
                foreach (udv_ParaOutTicket item in para)
                {
                    udv_OutTicketEntites model = new udv_OutTicketEntites();
                    model.SchemeETicketID = item.SchemeETicketID.ToString();
                    model.Status = 2;
                    model.TicketID = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    model.TicketTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    model.ExtendedValue = "内部出票成功";
                    ListOutTicker.Add(model);
                }
                ResModel.ListOutTicker = ListOutTicker;
                ListModel.Add(ResModel);

                return ListModel;
            }
            catch (Exception ex)
            {
                log.Write("处理出票数据:" + ex.StackTrace, true);
                return null;
            }
        }

        /// <summary>
        /// 处理中奖信息
        /// </summary>
        /// <returns></returns>
        public override List<udv_ResultWinInfo> HandleWinInfo(List<udv_ParaWinInfo> para)
        {
            try
            {
                List<udv_ResultWinInfo> ListModel = new List<udv_ResultWinInfo>();

                udv_ResultWinInfo ResModel = new udv_ResultWinInfo();
                List<udv_WinInfoEntites> ListTicker = new List<udv_WinInfoEntites>();

                ResModel.ErrorCode = "0";
                ResModel.ErrorMsg = "内部处理中奖信息成功";
                foreach (udv_ParaWinInfo item in para)
                {
                    udv_WinInfoEntites model = new udv_WinInfoEntites();
                    model.SchemeETicketID = item.SchemeETicketID.ToString();
                    model.Status = 2;
                    model.TicketID = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    model.PrebonusValue = item.WinMoney.ToString();
                    model.BonusValue = item.WinMoneyNoWithTax.ToString();
                    ListTicker.Add(model);
                }
                ResModel.ListWinInfo = ListTicker;
                ListModel.Add(ResModel);

                return ListModel;
            }
            catch (Exception ex)
            {
                log.Write("处理中奖信息:" + ex.StackTrace, true);
                return null;
            }
        }
        #endregion
    }
}
