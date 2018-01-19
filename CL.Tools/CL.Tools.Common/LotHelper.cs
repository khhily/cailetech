using CL.Enum.Common.Lottery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Tools.Common
{
    /// <summary>
    /// 彩种公共调用类
    /// Json
    /// </summary>
    public class LotHelper
    {
        #region  ---------- 分隔符 ---------- 
        protected const char SEP_NUM_A = ',';
        protected const char SEP_NUM_B = '#';
        protected const char SEP_NUM_C = '-';
        protected const char SEP_NUM_D = '@';
        protected const char SEP_NUM_E = ';';
        protected const char SEP_NUM_F = '^';
        protected const char SEP_NUM_G = '|';
        #endregion

        #region  ---------- 判断符 ---------- 
        protected const string IF_NUM_A = "#";
        #endregion

        #region ---------- 变量 ---------- 
        int bet = 0;
        int th = 0;
        int bth = 0;
        int place_w = 0;
        int place_q = 0;
        int place_b = 0;
        int red_ball = 0;
        int blue_ball = 0;
        int length = 0;
        string[] num = null;
        string[] red_num = null;
        string[] blue_num = null;
        //string[] red_num_dm = null;
        //string[] red_num_tm = null;
        //string[] blue_num_dm = null;
        //string[] blue_num_tm = null;
        #endregion 

        #region  ---------- 对象 ---------- 
        Log log = new Log("LotHelper");
        #endregion

        #region ---------- 彩票注数计算方程式(普通和胆拖) ---------- 
        /// <summary>
        /// 校验投注号码注数
        /// </summary>
        /// <param name="PlayCode">彩种玩法编号</param>
        /// <param name="Bet">注数</param>
        /// <param name="Number">选号</param>
        /// <param name="CorrectBet">正确的注数：验证失败的时候才返回</param>
        /// <returns>Bool:True or False</returns>
        public bool VerifyBet(int PlayCode, int Bet, string Number, ref int CorrectBet)
        {
            try
            {
                var CalculateBet = CalculateLot(PlayCode, Number);
                if (CalculateBet == Bet)
                    return true;
                else
                {
                    CorrectBet = CalculateBet;
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Write("验证投注注数错误[VerifyBet]：" + ex.StackTrace, true);
                CorrectBet = -1;
                return false;
            }
        }
        /// <summary>
        /// 计算选号注数
        /// 红快3,赢快3,华11选5,老11选5,双色球,超级大乐透
        /// </summary>
        /// <param name="PlayCode">玩法</param>
        /// <param name="Number">选号</param>
        /// <returns>Int32</returns>
        public int CalculateLot(int PlayCode, string Number)
        {

            bet = 0;
            th = 0;
            bth = 0;
            place_w = 0;
            place_q = 0;
            place_b = 0;
            red_ball = 0;
            blue_ball = 0;
            num = null;
            red_num = null;
            blue_num = null;
            //red_num_dm = null;
            //red_num_tm = null;
            //blue_num_dm = null;
            //blue_num_tm = null;

            length = Number.Split(SEP_NUM_A).Length;
            Number = Number.Trim().TrimEnd(SEP_NUM_A);
            switch (PlayCode)
            {
                #region 快三 吉林(红) 江西(赢) 
                case (int)JLXK3PlayCode.HZ: //和值
                case (int)JXK3PlayCode.HZ: //和值
                    bet = length;
                    break;
                case (int)JLXK3PlayCode.STHTX: //三同号通选
                case (int)JXK3PlayCode.STHTX: //三同号通选
                    bet = 1;
                    break;
                case (int)JLXK3PlayCode.STHDX: //三同号单选
                case (int)JXK3PlayCode.STHDX: //三同号单选
                    bet = length;
                    break;
                case (int)JLXK3PlayCode.SBTH: //三不同号
                case (int)JXK3PlayCode.SBTH: //三不同号
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 3 - th);
                    }
                    else
                        bet = Strata(length, 3);
                    break;
                case (int)JLXK3PlayCode.SLHTX: //三连号通选
                case (int)JXK3PlayCode.SLHTX: //三连号通选
                    bet = 1;
                    break;
                case (int)JLXK3PlayCode.ETHFX: //二同号复选
                case (int)JXK3PlayCode.ETHFX: //二同号复选
                    bet = length;
                    break;
                case (int)JLXK3PlayCode.ETHDX: //二同号单选
                case (int)JXK3PlayCode.ETHDX: //二同号单选
                    num = Number.Split(SEP_NUM_G);
                    th = num[0].Split(SEP_NUM_A).Length;
                    bth = num[1].Split(SEP_NUM_A).Length;
                    bet = th * bth;
                    break;
                case (int)JLXK3PlayCode.EBTH: //二不同号
                case (int)JXK3PlayCode.EBTH: //二不同号
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 2 - th);
                    }
                    else
                        bet = Strata(length, 2);
                    break;
                #endregion
                #region 十一云夺金 湖北(华) 山东(老)
                case (int)HB11X5PlayCode.R2: //任选2
                case (int)SD11X5PlayCode.R2: //任选2
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 2 - th);
                    }
                    else
                        bet = Strata(length, 2);
                    break;
                case (int)HB11X5PlayCode.R3: //任选3
                case (int)SD11X5PlayCode.R3: //任选3
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 3 - th);
                    }
                    else
                        bet = Strata(length, 3);
                    break;
                case (int)HB11X5PlayCode.R4: //任选4
                case (int)SD11X5PlayCode.R4: //任选4
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 4 - th);
                    }
                    else
                        bet = Strata(length, 4);
                    break;
                case (int)HB11X5PlayCode.R5: //任选5
                case (int)SD11X5PlayCode.R5: //任选5
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 5 - th);
                    }
                    else
                        bet = Strata(length, 5);
                    break;
                case (int)HB11X5PlayCode.R6: //任选6
                case (int)SD11X5PlayCode.R6: //任选6
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 6 - th);
                    }
                    else
                        bet = Strata(length, 6);
                    break;
                case (int)HB11X5PlayCode.R7: //任选7
                case (int)SD11X5PlayCode.R7: //任选7
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 7 - th);
                    }
                    else
                        bet = Strata(length, 7);
                    break;
                case (int)HB11X5PlayCode.R8: //任选8
                case (int)SD11X5PlayCode.R8: //任选8
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 8 - th);
                    }
                    else
                        bet = Strata(length, 8);
                    break;
                case (int)HB11X5PlayCode.Q1: //前一直选
                case (int)SD11X5PlayCode.Q1: //前一直选
                    bet = length;
                    break;
                case (int)HB11X5PlayCode.Q2_ZX: //前二直选
                case (int)SD11X5PlayCode.Q2_ZX: //前二直选
                    num = Number.Split(SEP_NUM_G);
                    place_w = num[0].Split(SEP_NUM_A).Length;
                    place_q = num[1].Split(SEP_NUM_A).Length;
                    #region 按位重复选号算法
                    //if (place_w >= place_q)
                    //    bet = (place_w - 1) * place_q;
                    //else
                    //    bet = place_w * (place_q - 1);
                    #endregion
                    #region 按位排斥重复选号算法
                    bet = place_w * place_q;
                    #endregion
                    break;
                case (int)HB11X5PlayCode.Q2_ZUX: //前二组选
                case (int)SD11X5PlayCode.Q2_ZUX: //前二组选
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 2 - th);
                    }
                    else
                        bet = Strata(length, 2);
                    break;
                case (int)HB11X5PlayCode.Q3_ZX: //前三直选
                case (int)SD11X5PlayCode.Q3_ZX: //前三直选
                    num = Number.Split(SEP_NUM_G);
                    place_w = num[0].Split(SEP_NUM_A).Length;
                    place_q = num[1].Split(SEP_NUM_A).Length;
                    place_b = num[2].Split(SEP_NUM_A).Length;
                    #region 按位排斥重复选号算法
                    bet = place_w * place_q * place_b;
                    #endregion
                    break;
                case (int)HB11X5PlayCode.Q3_ZUX: //前三组选
                case (int)SD11X5PlayCode.Q3_ZUX: //前三组选
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 3 - th);
                    }
                    else
                        bet = Strata(length, 3);
                    break;
                case (int)SD11X5PlayCode.LX3: //乐选3
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 3 - th);
                    }
                    else
                        bet = Strata(length, 3);
                    break;
                case (int)SD11X5PlayCode.LX4: //乐选4
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 4 - th);
                    }
                    else
                        bet = Strata(length, 4);
                    break;
                case (int)SD11X5PlayCode.LX5: //乐选5
                    if (Number.Contains(IF_NUM_A))
                    {
                        num = Number.Split(SEP_NUM_B);
                        th = num[0].Split(SEP_NUM_A).Length;
                        bth = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(bth, 5 - th);
                    }
                    else
                        bet = Strata(length, 5);
                    break;
                #endregion
                #region 双色球 超级大乐透
                case (int)SSQPlayCode.Norm: //普通玩法 双色球蓝球不支持胆拖功能
                    if (Number.Contains(IF_NUM_A)) //胆拖
                    {
                        num = Number.Split(SEP_NUM_G);
                        red_num = num[0].Split(SEP_NUM_B);
                        th = red_num[0].Split(SEP_NUM_A).Length;  //胆码
                        bth = red_num[1].Split(SEP_NUM_A).Length; //拖码
                        bet = Strata(bth, 6 - th) * red_num[1].Split(SEP_NUM_A).Length;
                    }
                    else
                    {
                        num = Number.Split(SEP_NUM_G);
                        red_ball = num[0].Split(SEP_NUM_A).Length;
                        blue_ball = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(red_ball, 6) * blue_ball;
                    }
                    break;
                case (int)CJDLTPlayCode.Norm:  //普通玩法
                case (int)CJDLTPlayCode.AddTo: //追加玩法
                    if (Number.Contains(IF_NUM_A)) //胆拖
                    {
                        num = Number.Split(SEP_NUM_G);
                        red_num = num[0].Split(SEP_NUM_B);
                        blue_num = num[1].Split(SEP_NUM_B);
                        if (blue_num.Length == 1)
                        {
                            th = red_num[0].Split(SEP_NUM_A).Length;  //胆码
                            bth = red_num[1].Split(SEP_NUM_A).Length; //拖码
                            bet = Strata(bth, 5 - th) * Strata(blue_ball, 2);
                        }
                        else
                        {
                            //计算红球组成
                            th = red_num[0].Split(SEP_NUM_A).Length;  //胆码
                            bth = red_num[1].Split(SEP_NUM_A).Length; //拖码
                            bet = Strata(bth, 5 - th);
                            //计算蓝球组成
                            th = blue_num[0].Split(SEP_NUM_A).Length;  //胆码
                            bth = blue_num[1].Split(SEP_NUM_A).Length; //拖码
                            bet = bet * Strata(bth, 2 - th);
                        }
                    }
                    else
                    {
                        num = Number.Split(SEP_NUM_G);
                        red_ball = num[0].Split(SEP_NUM_A).Length;
                        blue_ball = num[1].Split(SEP_NUM_A).Length;
                        bet = Strata(red_ball, 5) * Strata(blue_ball, 2);
                    }
                    break;
                #endregion 
                default:
                    bet = 1;
                    break;
            }
            return bet;
        }
        /// <summary>
        /// 计算公式
        /// 组合算法(阶乘)
        /// </summary>
        /// <param name="m">选号</param>
        /// <param name="n">标准</param>
        /// <returns>Int32</returns>
        private int Strata(int m, int n)
        {
            int res = 1;
            int jc_mn_val = 1;
            int jc_m_val = m;
            if (m > n)
            {
                for (int i = (m - 1); i > n; i--)
                    jc_m_val = jc_m_val * i;
                for (int i = 1; i <= (m - n); i++)
                    jc_mn_val = jc_mn_val * i;
                res = jc_m_val / jc_mn_val;
            }
            return res;
        }
        #endregion

        #region ---------- 彩票通用拆票计算方程式(普通和胆拖) ----------
        public string SplitLotTicket(string Number, char SplitSymbol, bool IsSingle)
        {
            try
            {
                string res = string.Empty;
                if (Number.Contains(IF_NUM_A))
                {
                    #region 胆拖拆票
                    if (IsSingle)
                    {
                        #region 拆单式

                        #endregion
                    }
                    else
                    {
                        #region 拆复试

                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region 标准拆票
                    if (IsSingle)
                    {
                        #region 拆单式

                        #endregion
                    }
                    else
                    {
                        #region 拆复试

                        #endregion
                    }
                    #endregion
                }
                return res;
            }
            catch (Exception ex)
            {
                log.Write("公共拆分电子票错误[SplitLotTicket]：" + ex.StackTrace, true);
                return string.Empty;
            }
        }
        #endregion
    }
}
