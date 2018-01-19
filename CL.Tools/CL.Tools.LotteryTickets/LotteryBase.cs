using CL.Tools.Common;
using CL.Tools.LotteryTickets.Lottery._11x5;
using CL.Tools.LotteryTickets.Lottery.DP;
using CL.Tools.LotteryTickets.Lottery.K3;
using CL.View.Entity.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using CL.Tools.LotteryTickets.Lottery.SSC;

namespace CL.Tools.LotteryTickets
{
    public partial class LotteryBase
    {
        //玩法集合
        public List<WinType> wt = new List<WinType>();
        
        public LotteryBase this[int LotteryCode]
        {
            get
            {
                switch (LotteryCode)
                {
                    case 101:
                        return new JLK3();
                    case 102:
                        return new JXK3();
                    case 201:
                        return new HB11X5();
                    case 202:
                        return new SD11X5();
                    case 301:
                        return new CQSSC();
                    case 801:
                        return new SSQ();
                    case 901:
                        return new CJDLT();
                }
                return null;
            }
        }
        public LotteryBase this[XmlNode node]
        {
            get
            {
                if (node == null) return null;
                int lotterycode = Convert.ToInt32(node.SelectSingleNode("SystemLotteryCode").InnerText);
                switch (lotterycode)
                {
                    case 101:
                        return new JLK3(node);
                    case 102:
                        return new JXK3(node);
                    case 201:
                        return new HB11X5(node);
                    case 202:
                        return new SD11X5(node);
                    case 301:
                        return new CQSSC(node);
                    case 801:
                        return new SSQ(node);
                    case 901:
                        return new CJDLT(node);
                }
                return null;
            }
        }

        #region 采集服务
        /// <summary>
        /// 维护当天的所有期号信息，高频彩使用
        /// </summary>
        public virtual void ModifyIsuseInfo() { }
        /// <summary>
        /// 维护当天期号信息(国外彩种)
        /// </summary>
        public virtual void ModifyIsuseInfo_Foreign() { }

        /// <summary>
        /// 期号截止后撤销等待拆票的订单
        /// </summary>
        /// <param name="LotteryCode"></param>
        public virtual void IsuseStopRevokeSchemes(int LotteryCode) { }

        /// <summary>
        /// 采集彩种信息
        /// </summary>
        /// <returns></returns>
        public virtual LotteryResult GetValue() { return null; }

        /// <summary>
        /// 获取遗漏的号码
        /// </summary>
        public virtual void GetSuppNum() { }
        #endregion

        #region 拆票服务

        #endregion

        #region 出票服务
        /// <summary>
        /// 处理电子票出票
        /// </summary>
        /// <param name="LotteryCode"></param>
        public virtual void OutTicket(XmlNode xml)
        {

        }
        #endregion

        #region 算奖服务
        /// <summary>
        /// 算奖
        /// </summary>
        /// <param name="model">电子票实体</param>
        /// <param name="Description">开奖描述</param>
        /// <param name="WinMoneyNoWithTax">税后奖金</param>
        /// <returns></returns>
        public virtual void ComputeWin(XmlNode xml)
        {
        }
        /// <summary>
        /// 追号算奖
        /// </summary>
        public virtual void ComputeChaseTasksWin(XmlNode xml)
        {

        }
        public virtual long ComputeWin(udv_ComputeTicket model, ref string Description, ref long WinMoneyNoWithTax)
        {
            return 0;
        }
        #endregion

        #region 派奖服务
        /// <summary>
        /// 派奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        public virtual void AwardWin(XmlNode xml)
        {

        }
        #endregion

        #region 其他方法
        /// <summary>
        /// 方案分析
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="PlayTypeCode"></param>
        /// <returns></returns>
        public virtual string[] AnalyseScheme(string Content, int PlayTypeCode)
        {
            return null;
        }

        /// <summary>
        /// 验证开奖号码是否正确
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public virtual bool AnalyseWinNumber(string Number)
        {
            return true;
        }

        /// <summary>
        /// 分割电子票号码
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        protected string[] SplitLotteryNumber(string Number, char c)
        {
            string[] s = Number.Split(c);
            if (s.Length == 0) return null;
            for (int i = 0; i < s.Length; i++)
                s[i] = s[i].Trim();
            return s;
        }
        protected string[] SplitLotteryNumber(string Number)
        {
            char[] ch = Number.ToCharArray();
            if (ch.Length == 0) return null;
            string[] arr = new string[ch.Length / 2];
            for (int i = 0; i < ch.Length; i = i + 2)
            {
                arr[i / 2] = ch[i].ToString() + ch[i + 1].ToString();
            }
            return arr;
        }
        protected int[] SplitLotteryNumbers(string Number)
        {
            List<int> arr = new List<int>();
            Number = Number.Trim();
            for (int i = 0; i < Number.Length; i++)
            {
                arr.Add(Convert.ToInt32(Number.Substring(i, 1)));
            }
            return arr.ToArray();
        }

        /// <summary>
        /// 字符串排序
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected string Sort(string str)
        {
            char[] ch = str.ToCharArray();
            Array.Sort(ch, new CompareToAscii());
            return new string(ch);
        }
        protected class CompareToAscii : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                return (new CaseInsensitiveComparer()).Compare(x, y);
            }
        }
        #endregion

    }

    public class WinType
    {
        /// <summary>
        /// 奖等编号
        /// </summary>
        public int WinCode { get; set; }
        /// <summary>
        /// 玩法编号
        /// </summary>
        public int PlayCode { get; set; }
        /// <summary>
        /// 奖等名称
        /// </summary>
        public string WinName { get; set; }
        /// <summary>
        /// 默认奖金
        /// </summary>
        public int DefaultMoney { get; set; }
        /// <summary>
        /// 默认税后奖金
        /// </summary>
        public int DefaultMoneyNoWithTax { get; set; }
    }
    public class LotteryResult
    {
        /// <summary>
        /// 彩票期号
        /// </summary>
        public string IsuseName;
        /// <summary>
        /// 彩种
        /// </summary>
        public int LotteryCode;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime;
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime EndTime;
        /// <summary>
        /// 开奖时间
        /// </summary>
        public DateTime LotteryTime;
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string LotteryWinNum;
        /// <summary>
        /// 重载 ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = String.Format(" 彩种：{0} 期号：{1} 官方开奖时间：{2} 采集入库时间：{3} 赢奖号码：【{4}】", LotteryCode, IsuseName, LotteryTime, DateTime.Now.ToString("HH:mm:ss"), LotteryWinNum);
            return str;
        }
    }

    public class JNDAward
    {
        public int drawNbr { set; get; }

        public string drawDate { set; get; }

        public string drawTime { set; get; }

        public int[] drawNbrs { set; get; }

        public decimal drawBonus { set; get; }
    }
}
