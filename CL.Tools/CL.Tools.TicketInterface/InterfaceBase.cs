using CL.Tools.Common;
using CL.View.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Xml;

namespace CL.Tools.TicketInterface
{
    public partial class InterfaceBase
    {
        Log log = new Log("InterfaceBase");
        public InterfaceBase this[XmlNode node]
        {
            get
            {
                try
                {
                    switch (node.SelectSingleNode("InterfaceType").InnerText.ToLower())
                    {
                        case "huayang":
                            return new HuaYang.HuaYangHandle(node);
                        case "caile":
                            return new CaiLe.CaiLeHandle(node);
                    }
                }
                catch (Exception ex)
                {
                    log.Write("接口错误：" + ex.StackTrace, true);
                }
                return null;
            }
        }

        #region 接口方法虚方法
        /// <summary>
        /// 查询接口余额
        /// </summary>
        /// <returns></returns>
        public virtual udv_ResultBalance GetAccBalance(udv_ParaAccBalance para)
        {
            return null;
        }

        /// <summary>
        /// 获取期号信息
        /// </summary>
        /// <returns></returns>
        public virtual udv_ResultIssue GetIsuseInfo(udv_ParaIsuse para)
        {
            return null;
        }

        /// <summary>
        /// 电子票投注
        /// </summary>
        /// <returns></returns>
        public virtual List<udv_ResultBetting> HandleTicketBetting(List<udv_ParaBettingTicker> para)
        {
            return null;
        }
        /// <summary>
        /// 竞彩电子票投注
        /// </summary>
        /// <returns></returns>
        public virtual List<udv_ResultBetting> HandleFootballBetting(List<udv_ParaBettingTicker> para)
        {
            return null;
        }
        /// <summary>
        /// 处理出票数据
        /// </summary>
        /// <returns></returns>
        public virtual List<udv_ResultOutTicket> HandleOutTicket(List<udv_ParaOutTicket> para)
        {
            return null;
        }

        /// <summary>
        /// 处理中奖信息
        /// </summary>
        /// <returns></returns>
        public virtual List<udv_ResultWinInfo> HandleWinInfo(List<udv_ParaWinInfo> para)
        {
            return null;
        }

        /// <summary>
        /// 查询奖期信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public virtual udv_ResultWinIsuse GetWinIsuse(udv_ParaWinIsuse para)
        {
            return null;
        }
        #endregion

    }
}
