using System.Collections.Generic;

namespace CL.View.Entity.Interface
{
    public class udv_ResultWinIsuse
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// 具体信息
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 彩种编号
        /// </summary>
        public string LotteryCode { get; set; }
        /// <summary>
        /// 期号
        /// </summary>
        public string Issue { get; set; }
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string OpenNumber { get; set; }

        public List<udv_WinIsuseEntites> ListWinIsuse { get; set; }
    }
    public class udv_WinIsuseEntites
    {
        /// <summary>
        /// 奖金等级
        /// </summary>
        public string BonusLevel { get; set; }
        /// <summary>
        /// 每注奖金金额，以分为单位
        /// </summary>
        public string BonusValue { get; set; }
        /// <summary>
        /// 中奖总注数
        /// </summary>
        public string BonusCount { get; set; }
    }
}
