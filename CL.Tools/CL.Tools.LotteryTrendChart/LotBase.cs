using CL.Enum.Common;
using CL.Enum.Common.Lottery;
using CL.Enum.Common.LotteryTrendChart;
using CL.Tools.Common;

namespace CL.Tools.LotteryTrendChart
{
    /// <summary>
    /// 彩种走势图基础构造
    /// Joan
    /// </summary>
    public class LotBase
    {
        Log log = new Log("LotBase");
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="LotteryCode">彩种</param>
        /// <param name="BaseType">基本类型</param>
        /// <returns></returns>
        public LotBase this[int LotteryCode, LotBaseType BaseType]
        {
            get
            {
                switch (BaseType)
                {
                    case LotBaseType.BaseOmission:
                        switch (LotteryCode)
                        {
                            case (int)LotteryInfo.HB11X5:
                            case (int)LotteryInfo.SD11X5:
                                return new SYYDJ.LotOmission(LotteryCode);
                            case (int)LotteryInfo.JLK3:
                            case (int)LotteryInfo.JXK3:
                                return new KS.LotOmission(LotteryCode);
                            case (int)LotteryInfo.CJDLT:
                                return new CJDLT.LotOmission(LotteryCode);
                            case (int)LotteryInfo.SSQ:
                                return new SSQ.LotOmission(LotteryCode);
                            case (int)LotteryInfo.CQSSC:
                                return new SSC.LotOmission(LotteryCode);
                        }
                        break;
                }
                return null;
            }
        }

        #region 走势图基础遗漏虚方法
        /// <summary>
        /// 基础遗漏虚方法
        /// </summary>
        /// <param name="PlayCode">玩法编号</param>
        /// <param name="TopNumber">多少条遗漏</param>
        /// <param name="Code">处理编码</param>
        /// <returns></returns>
        public virtual string Omission(string IsuseName, int PlayCode, int TopNumber)
        {
            return string.Empty;
        }

        /// <summary>
        /// 基础遗漏冷热虚方法
        /// </summary>
        /// <param name="PlayCode">玩法编号</param>
        /// <param name="TopNumber">多少条遗漏</param>
        /// <param name="Code">处理编码</param>
        /// <returns></returns>
        public virtual string OmissionColdHot(string IsuseName, int PlayCode, int TopNumber)
        {
            return string.Empty;
        }

        /// <summary>
        /// 基础走势图
        /// </summary>
        /// <param name="IsuseName">当前期号</param>
        /// <param name="PlayCode">彩种玩法</param>
        /// <param name="TopNumber">记录数</param>
        /// <returns></returns>
        public virtual string Basics_TrendChart(string IsuseName, int PlayCode, int TopNumber)
        {
            return string.Empty;
        }
        #endregion
    }
}
