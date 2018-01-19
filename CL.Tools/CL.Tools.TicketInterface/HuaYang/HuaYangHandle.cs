using CL.Enum.Common.Lottery;
using CL.Tools.Common;
using CL.Tools.TicketInterface.arithmetic;
using CL.View.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CL.Tools.TicketInterface.HuaYang
{
    public class HuaYangHandle : InterfaceBase
    {
        private readonly Log log = new Log("HuaYangHandle");

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

        #region 常量定义
        /// <summary>
        /// 余额查询
        /// </summary>
        protected const string BalanceQuery = "13002";
        /// <summary>
        /// 奖期查询
        /// </summary>
        protected const string AwardPeriodQuery = "13007";
        /// <summary>
        /// 电子票投注
        /// </summary>
        protected const string Betting = "13005";
        /// <summary>
        /// 电子票查询
        /// </summary>
        protected const string TicketQuery = "13004";
        /// <summary>
        /// 中奖查询
        /// </summary>
        protected const string WinningQuery = "13011";
        /// <summary>
        /// 奖金等级明细查询
        /// </summary>
        protected const string DetailsQuery = "13009";

        #region 竞彩
        /// <summary>
        /// 竞彩电子票投注
        /// </summary>
        protected const string FootballBetting = "13010";
        /// <summary>
        /// 猜冠军投注电子票投注
        /// </summary>
        protected const string FirstBetting = "20003";

        #endregion
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="xml"></param>
        public HuaYangHandle(XmlNode xml)
        {
            try
            {
                BindInterfaceInfo(xml);
            }
            catch (Exception ex)
            {
                log.Write("华阳接口读取配置信息错误：" + ex.Message, true);
                throw;
            }
        }
        public HuaYangHandle(string _agenterid, string _agenterpwd, string _username, string _InterfaceAddress,
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

        /// <summary>
        /// 头部结构
        /// </summary>
        /// <param name="transactiontype">投注类型</param>
        /// <param name="digest">投注签名</param>
        /// <returns></returns>
        private string GetModelHead(string transactiontype, string digest)
        {
            Random rd = new Random();
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string messengerid = DateTime.Now.ToString("yyyyMMddHHmmss") + rd.Next(100000, 999999);

            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<message version=\"1.0\">");
            sb.Append("<header>");
            sb.Append("<messengerid>" + messengerid + "</messengerid>");
            sb.Append("<timestamp>" + timestamp + "</timestamp>");
            sb.Append("<transactiontype>" + transactiontype + "</transactiontype>");
            sb.Append("<digest>" + digest + "</digest>");
            sb.Append("<agenterid>" + agenterid + "</agenterid>");
            sb.Append("<username>" + username + "</username>");
            sb.Append("</header>");
            sb.Append("{0}</message>");
            return sb.ToString();
        }

        /// <summary>
        /// 返回签名数据
        /// </summary>
        /// <param name="strbody"></param>
        /// <returns></returns>
        private string GetDigest(string strbody)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            //string timestamp = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds.ToString();
            return Utils.MD5(timestamp + agenterpwd + strbody, true);
        }

        /// <summary>
        /// 返回玩法
        /// </summary>
        /// <param name="playid"></param>
        /// <param name="childtype"></param>
        /// <param name="saletype"></param>
        private void GetPlay(udv_ParaBettingTicker item, out int childtype, out int saletype, out int bet)
        {
            int playid = item.PlayType;
            childtype = 0;
            saletype = 0;
            bet = 1;
            switch (SystemLotteryCode)
            {
                case 101:   //吉林快3
                    #region
                    bet = 1;
                    switch (playid)
                    {
                        case 10101: childtype = 1; saletype = 0; break;
                        case 10102: childtype = 2; saletype = 0; break;
                        case 10103: childtype = 2; saletype = 1; break;
                        case 10104: childtype = 3; saletype = 0; break;
                        case 10105: childtype = 4; saletype = 0; break;
                        case 10106: childtype = 5; saletype = 1; break;
                        case 10107: childtype = 5; saletype = 0; break;
                        case 10108: childtype = 6; saletype = 0; break;
                    }
                    #endregion
                    break;
                case 102:   //江西快3
                    #region
                    bet = 1;
                    switch (playid)
                    {
                        case 10201: childtype = 1; saletype = 0; break;
                        case 10202: childtype = 2; saletype = 0; break;
                        case 10203: childtype = 2; saletype = 1; break;
                        case 10204: childtype = 3; saletype = 0; break;
                        case 10205: childtype = 4; saletype = 0; break;
                        case 10206: childtype = 5; saletype = 1; break;
                        case 10207: childtype = 5; saletype = 0; break;
                        case 10208: childtype = 6; saletype = 0; break;
                    }
                    #endregion
                    break;
                case 201:   //湖北11选5
                    #region
                    switch (playid)
                    {
                        case 20101: //任选二
                            #region
                            childtype = 2;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                bet = item.Number.Split('*')[1].Length / 2;
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 4) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 4) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) / 2);
                            }
                            #endregion
                            break;
                        case 20102: //任选三
                            #region
                            childtype = 3;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr_rxs = item.Number.Split('*');
                                if (arr_rxs[0].Length == 2)
                                {
                                    decimal n = arr_rxs[1].Length / 2;
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr_rxs[0].Length == 4)
                                {
                                    bet = arr_rxs[1].Length / 2;
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 6) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 6) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                            }
                            #endregion
                            break;
                        case 20103: //任选四
                            #region
                            childtype = 4;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                string[] arr_rxs = item.Number.Split('*');
                                saletype = 2;
                                decimal n = arr_rxs[1].Length / 2;
                                if (arr_rxs[0].Length == 2)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                                }
                                else if (arr_rxs[0].Length == 4)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr_rxs[0].Length == 6)
                                {
                                    bet = Convert.ToInt32(n);
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 8) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 8) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                            }
                            #endregion
                            break;
                        case 20104: //任选五
                            #region
                            childtype = 5;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr_rxw = item.Number.Split('*');
                                decimal n = arr_rxw[1].Length / 2;
                                if (arr_rxw[0].Length == 2)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                                }
                                else if (arr_rxw[0].Length == 4)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                                }
                                else if (arr_rxw[0].Length == 6)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr_rxw[0].Length == 8)
                                {
                                    bet = Convert.ToInt32(n);
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 10) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 10) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                            }
                            #endregion
                            break;
                        case 20105: //任选六
                            #region
                            childtype = 6;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr_rxl = item.Number.Split('*');
                                decimal n = arr_rxl[1].Length / 2;
                                if (arr_rxl[0].Length == 2)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                                }
                                else if (arr_rxl[0].Length == 4)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                                }
                                else if (arr_rxl[0].Length == 6)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                                }
                                else if (arr_rxl[0].Length == 8)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr_rxl[0].Length == 10)
                                {
                                    bet = Convert.ToInt32(n);
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 12) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 12) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) / 720);
                            }
                            #endregion
                            break;
                        case 20106: //任选七
                            #region
                            childtype = 7;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr_rxq = item.Number.Split('*');
                                decimal n = arr_rxq[1].Length / 2;
                                if (arr_rxq[0].Length == 2)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) / 720);
                                }
                                else if (arr_rxq[0].Length == 4)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                                }
                                else if (arr_rxq[0].Length == 6)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                                }
                                else if (arr_rxq[0].Length == 8)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                                }
                                else if (arr_rxq[0].Length == 10)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr_rxq[0].Length == 12)
                                {
                                    bet = Convert.ToInt32(n);
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 14) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 14) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) * (n - 6) / 5040);
                            }
                            #endregion
                            break;
                        case 20107: //任选八
                            #region
                            childtype = 8;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr_rxb = item.Number.Split('*');
                                decimal n = arr_rxb[1].Length / 2;
                                if (arr_rxb[0].Length == 2)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) * (n - 6) / 5040);
                                }
                                else if (arr_rxb[0].Length == 4)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) / 720);
                                }
                                else if (arr_rxb[0].Length == 6)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                                }
                                else if (arr_rxb[0].Length == 8)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                                }
                                else if (arr_rxb[0].Length == 10)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                                }
                                else if (arr_rxb[0].Length == 12)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr_rxb[0].Length == 14)
                                {
                                    bet = Convert.ToInt32(n);
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 16) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            //else if (item.Number.Length > 16) //复式  任选八只有单式投注
                            //    saletype = 1;
                            #endregion
                            break;
                        case 20108: //前一直选
                            #region
                            childtype = 1;
                            saletype = 0;
                            bet = item.Number.Split('^').Length;
                            #endregion
                            break;
                        case 20109: //前二直选
                            #region
                            childtype = 9;
                            if (item.Number.IndexOf('*') > -1) //定位复式
                            {
                                saletype = 3;
                                string[] arr = item.Number.Split('*');
                                char[] first_ch = arr[0].ToCharArray();
                                char[] second_ch = arr[1].ToCharArray();
                                string[] first_arr = new string[first_ch.Length / 2];
                                string[] second_arr = new string[second_ch.Length / 2];
                                for (int i = 0; i < first_ch.Length; i = i + 2)
                                {
                                    first_arr[i / 2] = first_ch[i].ToString() + first_ch[i + 1].ToString();
                                }
                                for (int i = 0; i < second_ch.Length; i = i + 2)
                                {
                                    second_arr[i / 2] = second_ch[i].ToString() + second_ch[i + 1].ToString();
                                }

                                List<int> listnum = new List<int>();
                                for (int i = 0; i < first_arr.Length; i++)
                                {
                                    for (int j = 0; j < second_arr.Length; j++)
                                    {
                                        if (first_arr[i] != second_arr[j])
                                        {
                                            listnum.Add(1);
                                        }
                                    }
                                }
                                bet = listnum.Count;
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 4) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            #endregion
                            break;
                        case 20110: //前二组选
                            #region
                            childtype = 11;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr = item.Number.Split('*');
                                bet = Convert.ToInt32(arr[1].Length / 2);
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 4) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 4) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) / 2);
                            }
                            #endregion
                            break;
                        case 20111: //前三直选
                            #region
                            childtype = 10;
                            if (item.Number.IndexOf('*') > -1) //定位复式
                            {
                                saletype = 3;
                                string[] arr = item.Number.Split('*');
                                char[] first_ch = arr[0].ToCharArray();
                                char[] second_ch = arr[1].ToCharArray();
                                char[] third_ch = arr[2].ToCharArray();

                                string[] first_arr = new string[first_ch.Length / 2];
                                string[] second_arr = new string[second_ch.Length / 2];
                                string[] third_arr = new string[third_ch.Length / 2];
                                for (int i = 0; i < first_ch.Length; i = i + 2)
                                {
                                    first_arr[i / 2] = first_ch[i].ToString() + first_ch[i + 1].ToString();
                                }
                                for (int i = 0; i < second_ch.Length; i = i + 2)
                                {
                                    second_arr[i / 2] = second_ch[i].ToString() + second_ch[i + 1].ToString();
                                }
                                for (int i = 0; i < third_ch.Length; i = i + 2)
                                {
                                    third_arr[i / 2] = third_ch[i].ToString() + third_ch[i + 1].ToString();
                                }

                                List<int> listnum = new List<int>();
                                for (int i = 0; i < first_arr.Length; i++)
                                {
                                    for (int j = 0; j < second_arr.Length; j++)
                                    {
                                        if (first_arr[i] != second_arr[j])
                                        {
                                            for (int k = 0; k < third_arr.Length; k++)
                                            {
                                                if (first_arr[i] != third_arr[k] && second_arr[j] != third_arr[k])
                                                    listnum.Add(1);
                                            }
                                        }
                                    }
                                }
                                bet = listnum.Count;
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 6) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            #endregion
                            break;
                        case 20112: //前三组选
                            #region
                            childtype = 12;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr = item.Number.Split('*');
                                if (arr[0].Length == 2)
                                {
                                    decimal n = arr[1].Length / 2;
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr[0].Length == 4)
                                {
                                    bet = Convert.ToInt32(arr[1].Length / 2);
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 6) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 6) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                            }
                            #endregion
                            break;
                    }
                    #endregion
                    break;
                case 202:   //山东11选5
                    #region
                    switch (playid)
                    {
                        case 20201: //任选二
                            #region
                            childtype = 2;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                bet = item.Number.Split('*')[1].Length / 2;
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 4) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 4) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) / 2);
                            }
                            #endregion
                            break;
                        case 20202: //任选三
                            #region
                            childtype = 3;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr_rxs = item.Number.Split('*');
                                if (arr_rxs[0].Length == 2)
                                {
                                    decimal n = arr_rxs[1].Length / 2;
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr_rxs[0].Length == 4)
                                {
                                    bet = arr_rxs[1].Length / 2;
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 6) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 6) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                            }
                            #endregion
                            break;
                        case 20203: //任选四
                            #region
                            childtype = 4;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                string[] arr_rxs = item.Number.Split('*');
                                saletype = 2;
                                decimal n = arr_rxs[1].Length / 2;
                                if (arr_rxs[0].Length == 2)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                                }
                                else if (arr_rxs[0].Length == 4)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr_rxs[0].Length == 6)
                                {
                                    bet = Convert.ToInt32(n);
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 8) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 8) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                            }
                            #endregion
                            break;
                        case 20204: //任选五
                            #region
                            childtype = 5;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr_rxw = item.Number.Split('*');
                                decimal n = arr_rxw[1].Length / 2;
                                if (arr_rxw[0].Length == 2)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                                }
                                else if (arr_rxw[0].Length == 4)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                                }
                                else if (arr_rxw[0].Length == 6)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr_rxw[0].Length == 8)
                                {
                                    bet = Convert.ToInt32(n);
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 10) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 10) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                            }
                            #endregion
                            break;
                        case 20205: //任选六
                            #region
                            childtype = 6;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr_rxl = item.Number.Split('*');
                                decimal n = arr_rxl[1].Length / 2;
                                if (arr_rxl[0].Length == 2)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                                }
                                else if (arr_rxl[0].Length == 4)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                                }
                                else if (arr_rxl[0].Length == 6)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                                }
                                else if (arr_rxl[0].Length == 8)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr_rxl[0].Length == 10)
                                {
                                    bet = Convert.ToInt32(n);
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 12) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 12) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) / 720);
                            }
                            #endregion
                            break;
                        case 20206: //任选七
                            #region
                            childtype = 7;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr_rxq = item.Number.Split('*');
                                decimal n = arr_rxq[1].Length / 2;
                                if (arr_rxq[0].Length == 2)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) / 720);
                                }
                                else if (arr_rxq[0].Length == 4)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                                }
                                else if (arr_rxq[0].Length == 6)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                                }
                                else if (arr_rxq[0].Length == 8)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                                }
                                else if (arr_rxq[0].Length == 10)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr_rxq[0].Length == 12)
                                {
                                    bet = Convert.ToInt32(n);
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 14) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 14) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) * (n - 6) / 5040);
                            }
                            #endregion
                            break;
                        case 20207: //任选八
                            #region
                            childtype = 8;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr_rxb = item.Number.Split('*');
                                decimal n = arr_rxb[1].Length / 2;
                                if (arr_rxb[0].Length == 2)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) * (n - 6) / 5040);
                                }
                                else if (arr_rxb[0].Length == 4)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) / 720);
                                }
                                else if (arr_rxb[0].Length == 6)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120);
                                }
                                else if (arr_rxb[0].Length == 8)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) * (n - 3) / 24);
                                }
                                else if (arr_rxb[0].Length == 10)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                                }
                                else if (arr_rxb[0].Length == 12)
                                {
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr_rxb[0].Length == 14)
                                {
                                    bet = Convert.ToInt32(n);
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 16) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            //else if (item.Number.Length > 16) //复式  任选八只有单式投注
                            //    saletype = 1;
                            #endregion
                            break;
                        case 20208: //前一直选
                            #region
                            childtype = 1;
                            saletype = 0;
                            bet = item.Number.Split('^').Length;
                            #endregion
                            break;
                        case 20209: //前二直选
                            #region
                            childtype = 9;
                            if (item.Number.IndexOf('*') > -1) //定位复式
                            {
                                saletype = 3;
                                string[] arr = item.Number.Split('*');
                                char[] first_ch = arr[0].ToCharArray();
                                char[] second_ch = arr[1].ToCharArray();
                                string[] first_arr = new string[first_ch.Length / 2];
                                string[] second_arr = new string[second_ch.Length / 2];
                                for (int i = 0; i < first_ch.Length; i = i + 2)
                                {
                                    first_arr[i / 2] = first_ch[i].ToString() + first_ch[i + 1].ToString();
                                }
                                for (int i = 0; i < second_ch.Length; i = i + 2)
                                {
                                    second_arr[i / 2] = second_ch[i].ToString() + second_ch[i + 1].ToString();
                                }

                                List<int> listnum = new List<int>();
                                for (int i = 0; i < first_arr.Length; i++)
                                {
                                    for (int j = 0; j < second_arr.Length; j++)
                                    {
                                        if (first_arr[i] != second_arr[j])
                                        {
                                            listnum.Add(1);
                                        }
                                    }
                                }
                                bet = listnum.Count;
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 4) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            #endregion
                            break;
                        case 20210: //前二组选
                            #region
                            childtype = 11;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr = item.Number.Split('*');
                                bet = Convert.ToInt32(arr[1].Length / 2);
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 4) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 4) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) / 2);
                            }
                            #endregion
                            break;
                        case 20211: //前三直选
                            #region
                            childtype = 10;
                            if (item.Number.IndexOf('*') > -1) //定位复式
                            {
                                saletype = 3;
                                string[] arr = item.Number.Split('*');
                                char[] first_ch = arr[0].ToCharArray();
                                char[] second_ch = arr[1].ToCharArray();
                                char[] third_ch = arr[2].ToCharArray();

                                string[] first_arr = new string[first_ch.Length / 2];
                                string[] second_arr = new string[second_ch.Length / 2];
                                string[] third_arr = new string[third_ch.Length / 2];
                                for (int i = 0; i < first_ch.Length; i = i + 2)
                                {
                                    first_arr[i / 2] = first_ch[i].ToString() + first_ch[i + 1].ToString();
                                }
                                for (int i = 0; i < second_ch.Length; i = i + 2)
                                {
                                    second_arr[i / 2] = second_ch[i].ToString() + second_ch[i + 1].ToString();
                                }
                                for (int i = 0; i < third_ch.Length; i = i + 2)
                                {
                                    third_arr[i / 2] = third_ch[i].ToString() + third_ch[i + 1].ToString();
                                }

                                List<int> listnum = new List<int>();
                                for (int i = 0; i < first_arr.Length; i++)
                                {
                                    for (int j = 0; j < second_arr.Length; j++)
                                    {
                                        if (first_arr[i] != second_arr[j])
                                        {
                                            for (int k = 0; k < third_arr.Length; k++)
                                            {
                                                if (first_arr[i] != third_arr[k] && second_arr[j] != third_arr[k])
                                                    listnum.Add(1);
                                            }
                                        }
                                    }
                                }
                                bet = listnum.Count;
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 6) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            #endregion
                            break;
                        case 20212: //前三组选
                            #region
                            childtype = 12;
                            if (item.Number.IndexOf('*') > -1) //胆拖
                            {
                                saletype = 2;
                                string[] arr = item.Number.Split('*');
                                if (arr[0].Length == 2)
                                {
                                    decimal n = arr[1].Length / 2;
                                    bet = Convert.ToInt32(n * (n - 1) / 2);
                                }
                                else if (arr[0].Length == 4)
                                {
                                    bet = Convert.ToInt32(arr[1].Length / 2);
                                }
                            }
                            else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 6) //单式
                            {
                                saletype = 0;
                                bet = item.Number.Split('^').Length;
                            }
                            else if (item.Number.Length > 6) //复式
                            {
                                saletype = 1;
                                decimal n = item.Number.Length / 2;
                                bet = Convert.ToInt32(n * (n - 1) * (n - 2) / 6);
                            }
                            #endregion
                            break;
                        case 20213: //乐选3
                            childtype = 13;
                            break;
                        case 20214: //乐选4
                            childtype = 14;
                            break;
                        case 20215: //乐选5
                            childtype = 15;
                            break;
                    }
                    #endregion
                    break;
                case 801:   //双色球
                    #region
                    childtype = 0;
                    if (item.Number.IndexOf('*') > -1)  //是否胆拖
                    {
                        saletype = 2;
                        #region
                        string[] arr = item.Number.Split('|');
                        string first_num = arr[0].Split('*')[0];
                        string second_num = arr[0].Split('*')[1];
                        decimal n = second_num.Length / 2;
                        decimal w = arr[1].Length / 2;

                        if (first_num.Length == 2)
                        {
                            bet = Convert.ToInt32((n * (n - 1) * (n - 2) * (n - 3) * (n - 4) / 120) * w);
                        }
                        else if (first_num.Length == 4)
                        {
                            bet = Convert.ToInt32((n * (n - 1) * (n - 2) * (n - 3) / 24) * w);
                        }
                        else if (first_num.Length == 6)
                        {
                            bet = Convert.ToInt32((n * (n - 1) * (n - 2) / 6) * w);
                        }
                        else if (first_num.Length == 8)
                        {
                            bet = Convert.ToInt32((n * (n - 1) / 2) * w);
                        }
                        else if (first_num.Length == 10)
                        {
                            bet = Convert.ToInt32(n * w);
                        }
                        #endregion
                    }
                    else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 15) //单式
                    {
                        string[] num = item.Number.Split('^');
                        saletype = 0;
                        bet = num.Length;
                    }
                    else
                    {
                        string[] num = item.Number.Split('|');
                        saletype = 1;
                        decimal n = num[0].Length / 2;
                        bet = Convert.ToInt32((n * (n - 1) * (n - 2) * (n - 3) * (n - 4) * (n - 5) / 720) * num[1].Length / 2);
                    }
                    #endregion
                    break;
                case 901:   //大乐透
                    #region
                    childtype = playid == 90101 ? 0 : 1;
                    if (item.Number.IndexOf('*') > -1)  //是否胆拖
                    {
                        saletype = 2;
                        #region
                        string[] arr = item.Number.Split('|');
                        string[] QianQu = arr[0].Split('*');
                        string[] HouQu = arr[1].Split('*');
                        decimal n = QianQu[1].Length / 2;
                        decimal w = HouQu[1].Length / 2;

                        if (QianQu[0].Length == 2)
                        {
                            bet = Convert.ToInt32((n * (n - 1) * (n - 2) * (n - 3) / 24) * w);
                        }
                        else if (QianQu[0].Length == 4)
                        {
                            bet = Convert.ToInt32((n * (n - 1) * (n - 2) / 6) * w);
                        }
                        else if (QianQu[0].Length == 6)
                        {
                            bet = Convert.ToInt32((n * (n - 1) / 2) * w);
                        }
                        else if (QianQu[0].Length == 8)
                        {
                            bet = Convert.ToInt32(n * w);
                        }
                        #endregion
                    }
                    else if (item.Number.IndexOf('^') > -1 || item.Number.Length == 15) //单式
                    {
                        string[] num = item.Number.Split('^');
                        saletype = 0;
                        bet = num.Length;
                    }
                    else
                    {
                        string[] num = item.Number.Split('|');
                        saletype = 1;
                        decimal m = num[0].Length / 2;
                        decimal qbet = m * (m - 1) * (m - 2) * (m - 3) * (m - 4) / 120;
                        decimal n = num[1].Length / 2;
                        decimal hbet = n * (n - 1) / 2;
                        bet = Convert.ToInt32(qbet * hbet);
                    }
                    #endregion
                    break;
                case 301:   //重庆时时彩
                    #region 
                    bet = 0;
                    switch (playid)
                    {
                        case (int)CQSSCPlayCode.ZX_1X:
                            childtype = 1;
                            saletype = 0;
                            bet = 1;
                            break;
                        case (int)CQSSCPlayCode.ZX_2X:
                            childtype = 2;
                            bet = item.Bet;
                            saletype = bet > 1 ? 1 : 0;
                            break;
                        case (int)CQSSCPlayCode.ZXHZ_2X:
                            childtype = 2;
                            saletype = 5;
                            bet = item.Bet;
                            break;
                        case (int)CQSSCPlayCode.ZU_2X:
                            childtype = 2;
                            bet = item.Bet;
                            saletype = bet > 1 ? 4 : 2;
                            break;
                        case (int)CQSSCPlayCode.ZUHZ_2X:
                            childtype = 2;
                            saletype = 6;
                            bet = item.Bet;
                            break;
                        case (int)CQSSCPlayCode.ZX_3X:
                            childtype = 3;
                            bet = item.Bet;
                            saletype = bet > 1 ? 1 : 0;
                            break;
                        case (int)CQSSCPlayCode.ZU3_3X:
                            childtype = 3;
                            bet = item.Bet;
                            saletype = bet > 1 ? 3 : 2;
                            break;
                        case (int)CQSSCPlayCode.ZU6_3X:
                            childtype = 3;
                            bet = item.Bet;
                            saletype = bet > 1 ? 5 : 4;
                            break;
                        case (int)CQSSCPlayCode.ZX_5X:
                            childtype = 5;
                            bet = item.Bet;
                            saletype = bet > 1 ? 1 : 0;
                            break;
                        case (int)CQSSCPlayCode.TX_5X:
                            bet = item.Bet;
                            childtype = 5;
                            saletype = 2;
                            break;
                        case (int)CQSSCPlayCode.DXDS:
                            bet = 1;
                            childtype = 8;
                            saletype = 0;
                            break;
                    }
                    #endregion
                    break;

            }
        }

        #region 接口方法
        /// <summary>
        /// 查询用户余额 
        /// </summary>
        public override udv_ResultBalance GetAccBalance(udv_ParaAccBalance para)
        {
            try
            {
                username = para.username;
                XmlDocument xmlDoc = new XmlDocument();

                udv_ResultBalance ResModel = new udv_ResultBalance();
                string Digest = GetDigest("<body></body>");
                string StrHead = GetModelHead(BalanceQuery, Digest);
                string XmlModel = string.Format(StrHead, "<body></body>");
                string retxml = Utils.HttpPost(InterfaceAddress, XmlModel);

                xmlDoc.LoadXml(retxml);
                XmlNode root = xmlDoc.SelectSingleNode("//body/oelement");
                ResModel.ErrorCode = root.SelectSingleNode("errorcode").InnerText;
                ResModel.ErrorMsg = root.SelectSingleNode("errormsg").InnerText;
                ResModel.ActMoney = root.SelectSingleNode("actmoney").InnerText;
                ResModel.WinMoney = root.SelectSingleNode("bonusmoney").InnerText;

                return ResModel;
            }
            catch (Exception ex)
            {
                log.Write("查询华阳账户余额：" + ex.StackTrace, true);
                return null;
            }
        }

        /// <summary>
        /// 获取期号信息
        /// </summary>
        public override udv_ResultIssue GetIsuseInfo(udv_ParaIsuse para)
        {
            string XmlLog = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                udv_ResultIssue ResModel = new udv_ResultIssue();
                string StrBody = string.Format("<body><elements ><element><lotteryid>{0}</lotteryid><issue>{1}</issue></element></elements></body>", InterfaceLotteryCode, para.Issue);

                string Digest = GetDigest(StrBody);
                string StrHead = GetModelHead(AwardPeriodQuery, Digest);
                string XmlModel = string.Format(StrHead, StrBody);
                XmlLog = XmlModel;
                string retxml = Utils.HttpPost(InterfaceAddress, XmlModel);

                xmlDoc.LoadXml(retxml);
                XmlNode root = xmlDoc.SelectSingleNode("//body/oelement");
                XmlNode rootDetails = xmlDoc.SelectSingleNode("//body/elements/element");

                ResModel.ErrorCode = root.SelectSingleNode("errorcode").InnerText;
                ResModel.ErrorMsg = root.SelectSingleNode("errormsg").InnerText;
                if (rootDetails != null)
                {
                    ResModel.LotteryCode = rootDetails.SelectSingleNode("lotteryid").InnerText;
                    ResModel.Issue = rootDetails.SelectSingleNode("issue").InnerText;
                    ResModel.StartTime = rootDetails.SelectSingleNode("starttime").InnerText;
                    ResModel.EndTime = rootDetails.SelectSingleNode("endtime").InnerText;
                    ResModel.Satus = rootDetails.SelectSingleNode("status").InnerText;
                    ResModel.SaleMoney = rootDetails.SelectSingleNode("salemoney").InnerText;
                    ResModel.BonusMoney = rootDetails.SelectSingleNode("bonusmoney").InnerText;
                    ResModel.OpenNumber = rootDetails.SelectSingleNode("bonuscode").InnerText;
                }
                return ResModel;
            }
            catch (Exception ex)
            {
                log.Write("获取期号信息：" + ex.StackTrace, true);
                return null;
            }
        }

        /// <summary>
        /// 电子票投注
        /// </summary>
        public override List<udv_ResultBetting> HandleTicketBetting(List<udv_ParaBettingTicker> para)
        {
            try
            {
                if (IsInternal)
                    return InternalTicketBetting(para);

                XmlDocument xmlDoc = new XmlDocument();
                StringBuilder StrBody = new StringBuilder();
                StringBuilder StrElement = new StringBuilder();
                List<string> XmlModel = new List<string>();

                int childtype = 0;
                int saletype = 0;
                int bet = 0;
                int i = 0;
                int len = 1;

                foreach (udv_ParaBettingTicker item in para)
                {
                    string IssueName = string.Empty;
                    if (SystemLotteryCode == 801)
                        IssueName = item.Issue;
                    else
                        IssueName = item.Issue.Substring(2, item.Issue.Length - 2);

                    GetPlay(item, out childtype, out saletype, out bet);
                    StrElement.Append("<element>");
                    StrElement.Append("<ticketuser>" + item.TicketUser + "</ticketuser>");
                    StrElement.Append("<identify>" + item.Identify + "</identify>");
                    StrElement.Append("<phone>" + item.Phone + "</phone>");
                    StrElement.Append("<email>" + item.Email + "</email>");
                    StrElement.Append("<id>new" + item.SchemeETicketID + "</id>");
                    StrElement.Append("<lotteryid>" + InterfaceLotteryCode + "</lotteryid>");
                    StrElement.Append("<issue>" + IssueName + "</issue>");
                    StrElement.Append("<childtype>" + childtype + "</childtype>");
                    StrElement.Append("<saletype>" + saletype + "</saletype>");
                    StrElement.Append("<lotterycode>" + item.Number + "</lotterycode>");
                    StrElement.Append("<appnumbers>" + item.Multiple + "</appnumbers>");
                    StrElement.Append("<lotterynumber>" + bet + "</lotterynumber>");
                    StrElement.Append("<lotteryvalue>" + item.Amount + "</lotteryvalue>");
                    StrElement.Append("</element>");

                    //每条指令最多可以带500票，实际测试中100张已经是极限，接口建议每次50张提交
                    i++;
                    if (len == 50 || i == para.Count)
                    {
                        StrBody.Append("<body><elements>");
                        StrBody.Append(StrElement);
                        StrBody.Append("</elements></body>");
                        string Digest = GetDigest(StrBody.ToString());
                        string StrHead = GetModelHead(Betting, Digest);
                        XmlModel.Add(string.Format(StrHead, StrBody));
                        len = 1;
                        StrElement.Clear();
                        StrBody.Clear();
                    }
                    else
                        len++;
                }

                List<udv_ResultBetting> ListModel = new List<udv_ResultBetting>();
                foreach (string xml in XmlModel)
                {
                    udv_ResultBetting ResModel = new udv_ResultBetting();
                    List<udv_BettingEntites> ListTicker = new List<udv_BettingEntites>();

                    string retxml = Utils.HttpPost(InterfaceAddress, xml);
                    xmlDoc.LoadXml(retxml);
                    XmlNode root = xmlDoc.SelectSingleNode("//body/oelement");
                    XmlNodeList rootDetails = xmlDoc.SelectNodes("//body/elements/element");
                    ResModel.ErrorCode = root.SelectSingleNode("errorcode").InnerText;
                    ResModel.ErrorMsg = root.SelectSingleNode("errormsg").InnerText;
                    foreach (XmlNode item in rootDetails)
                    {
                        udv_BettingEntites model = new udv_BettingEntites();
                        model.SchemeETicketID = item.SelectSingleNode("id").InnerText.Replace("new", "");
                        model.TicketID = item.SelectSingleNode("ltappid").InnerText;
                        model.ActMoney = item.SelectSingleNode("actvalue").InnerText;
                        model.State = Convert.ToInt32(item.SelectSingleNode("errorcode").InnerText) == 0 ? 0 : 1;
                        model.Msg = item.SelectSingleNode("errormsg").InnerText;
                        ListTicker.Add(model);
                    }
                    ResModel.ListTicker = ListTicker;
                    ListModel.Add(ResModel);
                    if (XmlModel.Count > 1)
                        System.Threading.Thread.Sleep(100);
                }
                return ListModel;
            }
            catch (Exception ex)
            {
                log.Write("电子票投注：" + ex.StackTrace, true);
                return null;
            }
        }

        /// <summary>
        /// 竞彩电子票投注
        /// </summary>
        public override List<udv_ResultBetting> HandleFootballBetting(List<udv_ParaBettingTicker> para)
        {
            try
            {
                if (IsInternal)
                    return InternalTicketBetting(para);

                XmlDocument xmlDoc = new XmlDocument();
                StringBuilder StrBody = new StringBuilder();
                StringBuilder StrElement = new StringBuilder();
                List<string> XmlModel = new List<string>();

                int childtype = 0;
                int saletype = 0;
                int bet = 0;
                int i = 0;
                int len = 1;

                foreach (udv_ParaBettingTicker item in para)
                {
                    string IssueName = string.Empty;
                    if (SystemLotteryCode == 801)
                        IssueName = item.Issue;
                    else
                        IssueName = item.Issue.Substring(2, item.Issue.Length - 2);

                    GetPlay(item, out childtype, out saletype, out bet);
                    StrElement.Append("<element>");
                    StrElement.Append("<ticketuser>" + item.TicketUser + "</ticketuser>");
                    StrElement.Append("<identify>" + item.Identify + "</identify>");
                    StrElement.Append("<phone>" + item.Phone + "</phone>");
                    StrElement.Append("<email>" + item.Email + "</email>");
                    StrElement.Append("<id>new" + item.SchemeETicketID + "</id>");
                    StrElement.Append("<lotteryid>" + InterfaceLotteryCode + "</lotteryid>");
                    StrElement.Append("<issue>" + IssueName + "</issue>");
                    StrElement.Append("<childtype>" + childtype + "</childtype>");
                    StrElement.Append("<saletype>" + saletype + "</saletype>");
                    StrElement.Append("<lotterycode>" + item.Number + "</lotterycode>");
                    StrElement.Append("<appnumbers>" + item.Multiple + "</appnumbers>");
                    StrElement.Append("<lotterynumber>" + bet + "</lotterynumber>");
                    StrElement.Append("<lotteryvalue>" + item.Amount + "</lotteryvalue>");
                    StrElement.Append("<betlotterymode>" + item + "</betlotterymode>");
                    StrElement.Append("</element>");

                    //每条指令最多可以带500票，实际测试中100张已经是极限，接口建议每次50张提交
                    i++;
                    if (len == 50 || i == para.Count)
                    {
                        StrBody.Append("<body><elements>");
                        StrBody.Append(StrElement);
                        StrBody.Append("</elements></body>");
                        string Digest = GetDigest(StrBody.ToString());
                        string StrHead = GetModelHead(Betting, Digest);
                        XmlModel.Add(string.Format(StrHead, StrBody));
                        len = 1;
                        StrElement.Clear();
                        StrBody.Clear();
                    }
                    else
                        len++;
                }

                List<udv_ResultBetting> ListModel = new List<udv_ResultBetting>();
                foreach (string xml in XmlModel)
                {
                    udv_ResultBetting ResModel = new udv_ResultBetting();
                    List<udv_BettingEntites> ListTicker = new List<udv_BettingEntites>();

                    string retxml = Utils.HttpPost(InterfaceAddress, xml);
                    xmlDoc.LoadXml(retxml);
                    XmlNode root = xmlDoc.SelectSingleNode("//body/oelement");
                    XmlNodeList rootDetails = xmlDoc.SelectNodes("//body/elements/element");
                    ResModel.ErrorCode = root.SelectSingleNode("errorcode").InnerText;
                    ResModel.ErrorMsg = root.SelectSingleNode("errormsg").InnerText;
                    foreach (XmlNode item in rootDetails)
                    {
                        udv_BettingEntites model = new udv_BettingEntites();
                        model.SchemeETicketID = item.SelectSingleNode("id").InnerText.Replace("new", "");
                        model.TicketID = item.SelectSingleNode("ltappid").InnerText;
                        model.ActMoney = item.SelectSingleNode("actvalue").InnerText;
                        model.State = Convert.ToInt32(item.SelectSingleNode("errorcode").InnerText) == 0 ? 0 : 1;
                        model.Msg = item.SelectSingleNode("errormsg").InnerText;
                        ListTicker.Add(model);
                    }
                    ResModel.ListTicker = ListTicker;
                    ListModel.Add(ResModel);
                    if (XmlModel.Count > 1)
                        System.Threading.Thread.Sleep(100);
                }
                return ListModel;
            }
            catch (Exception ex)
            {
                log.Write("电子票投注：" + ex.StackTrace, true);
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
                if (IsInternal)
                    return InternalOutTicket(para);

                XmlDocument xmlDoc = new XmlDocument();
                StringBuilder StrBody = new StringBuilder();
                StringBuilder StrElement = new StringBuilder();
                List<string> XmlModel = new List<string>();
                int i = 0;
                int len = 1;

                foreach (udv_ParaOutTicket item in para)
                {
                    StrElement.Append("<element>");
                    StrElement.Append("<id>new" + item.SchemeETicketID + "</id>");
                    StrElement.Append("</element>");

                    //一次最多不得超过100票的查询
                    i++;
                    if (len == 100 || i == para.Count)
                    {
                        StrBody.Append("<body><elements>");
                        StrBody.Append(StrElement);
                        StrBody.Append("</elements></body>");
                        string Digest = GetDigest(StrBody.ToString());
                        string StrHead = GetModelHead(TicketQuery, Digest);
                        XmlModel.Add(string.Format(StrHead, StrBody));
                        len = 1;
                        StrElement.Clear();
                        StrBody.Clear();
                    }
                    else
                        len++;
                }

                List<udv_ResultOutTicket> ListModel = new List<udv_ResultOutTicket>();
                foreach (string xml in XmlModel)
                {
                    udv_ResultOutTicket ResModel = new udv_ResultOutTicket();
                    List<udv_OutTicketEntites> ListOutTicker = new List<udv_OutTicketEntites>();

                    string retxml = Utils.HttpPost(InterfaceAddress, xml);
                    xmlDoc.LoadXml(retxml);
                    XmlNode root = xmlDoc.SelectSingleNode("//body/oelement");
                    XmlNodeList rootDetails = xmlDoc.SelectNodes("//body/elements/element");
                    ResModel.ErrorCode = root.SelectSingleNode("errorcode").InnerText;
                    ResModel.ErrorMsg = root.SelectSingleNode("errormsg").InnerText;
                    foreach (XmlNode item in rootDetails)
                    {
                        udv_OutTicketEntites model = new udv_OutTicketEntites();
                        model.SchemeETicketID = item.SelectSingleNode("id").InnerText.Replace("new", "");
                        model.Status = Convert.ToInt32(item.SelectSingleNode("status").InnerText);
                        model.TicketID = item.SelectSingleNode("ticketid").InnerText;
                        model.TicketTime = item.SelectSingleNode("tickettime").InnerText;
                        model.ExtendedValue = item.SelectSingleNode("spvalue").InnerText;
                        ListOutTicker.Add(model);
                    }
                    ResModel.ListOutTicker = ListOutTicker;
                    ListModel.Add(ResModel);
                }
                return ListModel;
            }
            catch (Exception ex)
            {
                log.Write("处理出票数据：" + ex.StackTrace, true);
                return null;
            }
        }

        /// <summary>
        /// 处理中奖信息
        /// </summary>
        /// <returns></returns>
        public override List<udv_ResultWinInfo> HandleWinInfo(List<udv_ParaWinInfo> para)
        {
            List<string> XmlModel = new List<string>();
            try
            {
                if (IsInternal)
                    return InternalWinInfo(para);

                XmlDocument xmlDoc = new XmlDocument();
                StringBuilder StrBody = new StringBuilder();
                StringBuilder StrElement = new StringBuilder();
                int i = 0;
                int len = 1;

                foreach (udv_ParaWinInfo item in para)
                {
                    StrElement.Append("<element>");
                    StrElement.Append("<id>new" + item.SchemeETicketID + "</id>");
                    StrElement.Append("</element>");

                    //一次最多不得超过100票的查询
                    i++;
                    if (len == 100 || i == para.Count)
                    {
                        StrBody.Append("<body><elements>");
                        StrBody.Append(StrElement);
                        StrBody.Append("</elements></body>");
                        string Digest = GetDigest(StrBody.ToString());
                        string StrHead = GetModelHead(WinningQuery, Digest);
                        XmlModel.Add(string.Format(StrHead, StrBody));
                        len = 1;
                        StrElement.Clear();
                        StrBody.Clear();
                    }
                    else
                        len++;
                }
                log.Write("处理中奖信息(XML)1：" + Newtonsoft.Json.JsonConvert.SerializeObject(XmlModel), true);
                List<udv_ResultWinInfo> ListModel = new List<udv_ResultWinInfo>();
                foreach (string xml in XmlModel)
                {
                    udv_ResultWinInfo ResModel = new udv_ResultWinInfo();
                    List<udv_WinInfoEntites> ListTicker = new List<udv_WinInfoEntites>();

                    string retxml = Utils.HttpPost(InterfaceAddress, xml);
                    xmlDoc.LoadXml(retxml);
                    XmlNode root = xmlDoc.SelectSingleNode("//body/oelement");
                    XmlNodeList rootDetails = xmlDoc.SelectNodes("//body/elements/element");
                    ResModel.ErrorCode = root.SelectSingleNode("errorcode").InnerText;
                    ResModel.ErrorMsg = root.SelectSingleNode("errormsg").InnerText;
                    foreach (XmlNode item in rootDetails)
                    {
                        udv_WinInfoEntites model = new udv_WinInfoEntites();
                        model.SchemeETicketID = item.SelectSingleNode("id").InnerText.Replace("new", "");
                        model.Status = Convert.ToInt32(item.SelectSingleNode("status").InnerText);
                        model.TicketID = item.SelectSingleNode("ticketid").InnerText;
                        model.PrebonusValue = item.SelectSingleNode("prebonusvalue").InnerText;
                        model.BonusValue = item.SelectSingleNode("bonusvalue").InnerText;
                        ListTicker.Add(model);
                    }
                    ResModel.ListWinInfo = ListTicker;
                    ListModel.Add(ResModel);
                }
                return ListModel;
            }
            catch (Exception ex)
            {
                log.Write("处理中奖信息：" + ex.StackTrace, true);
                log.Write("处理中奖信息(XML)2：" + Newtonsoft.Json.JsonConvert.SerializeObject(XmlModel), true);
                return null;
            }
        }

        /// <summary>
        /// 查询奖期信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public override udv_ResultWinIsuse GetWinIsuse(udv_ParaWinIsuse para)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string StrBody = string.Format("<body><elements ><element><lotteryid>{0}</lotteryid><issue>{1}</issue></element></elements></body>", InterfaceLotteryCode, para.Issue);

                string Digest = GetDigest(StrBody);
                string StrHead = GetModelHead(DetailsQuery, Digest);
                string XmlModel = string.Format(StrHead, StrBody);
                string retxml = Utils.HttpPost(InterfaceAddress, XmlModel);

                xmlDoc.LoadXml(retxml);
                XmlNode root = xmlDoc.SelectSingleNode("//body/oelement");
                XmlNodeList rootDetails = xmlDoc.SelectNodes("//body/elements/element");

                udv_ResultWinIsuse ResModel = new udv_ResultWinIsuse();
                List<udv_WinIsuseEntites> ListWinIsuse = new List<udv_WinIsuseEntites>();
                ResModel.ErrorCode = root.SelectSingleNode("errorcode").InnerText;
                ResModel.ErrorMsg = root.SelectSingleNode("errormsg").InnerText;
                ResModel.LotteryCode = root.SelectSingleNode("lotteryid").InnerText;
                ResModel.Issue = root.SelectSingleNode("issue").InnerText;
                ResModel.OpenNumber = root.SelectSingleNode("bonuscode").InnerText;

                foreach (XmlNode item in rootDetails)
                {
                    udv_WinIsuseEntites model = new udv_WinIsuseEntites();
                    model.BonusLevel = item.SelectSingleNode("bonuslevel").InnerText;
                    model.BonusValue = item.SelectSingleNode("bonusvalue").InnerText;
                    model.BonusCount = item.SelectSingleNode("bonuscount").InnerText;
                    ListWinIsuse.Add(model);
                }
                ResModel.ListWinIsuse = ListWinIsuse;
                return ResModel;
            }
            catch (Exception ex)
            {
                log.Write("查询奖期信息：" + ex.StackTrace, true);
                return null;
            }
        }
        #endregion

        #region  电子票内部投注、出票、算奖、派奖
        /// <summary>
        /// 电子票内部投注
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<udv_ResultBetting> InternalTicketBetting(List<udv_ParaBettingTicker> para)
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
                    model.SchemeETicketID = item.SchemeETicketID.ToString().Replace("new", "");
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

                log.Write("电子票内部投注：" + ex.StackTrace, true);
                return null;
            }
        }

        /// <summary>
        /// 内部处理出票数据
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<udv_ResultOutTicket> InternalOutTicket(List<udv_ParaOutTicket> para)
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
                    model.SchemeETicketID = item.SchemeETicketID.ToString().Replace("new", "");
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
                log.Write("内部处理出票数据：" + ex.StackTrace, true);
                return null;
            }
        }

        /// <summary>
        /// 内部处理中奖信息
        /// </summary>
        /// <returns></returns>
        public List<udv_ResultWinInfo> InternalWinInfo(List<udv_ParaWinInfo> para)
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
                    model.SchemeETicketID = item.SchemeETicketID.ToString().Replace("new", "");
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
                log.Write("内部处理中奖信息：" + ex.StackTrace, true);
                return null;
            }
        }
        #endregion
    }
}
