using CL.Game.BLL;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Plugins.Award
{
    public class AwardEntrance
    {
        #region 实例化对象
        /// <summary>
        /// 中奖累计名次玩法加奖
        /// </summary>
        protected RegularAwadRankingBLL AwadRankingBLL = new RegularAwadRankingBLL();
        /// <summary>
        /// 中奖累计区间玩法加奖
        /// </summary>
        protected RegularAwardIntervalBLL AwardIntervalBLL = new RegularAwardIntervalBLL();
        /// <summary>
        /// 中球玩法加奖
        /// </summary>
        protected RegularBallBLL BallBLL = new RegularBallBLL();
        /// <summary>
        /// 投注累计区间玩法加奖
        /// </summary>
        protected RegularBetIntervalBLL BetIntervalBLL = new RegularBetIntervalBLL();
        /// <summary>
        /// 投注累计名次玩法加奖
        /// </summary>
        protected RegularBetRankingBLL BetRankingBLL = new RegularBetRankingBLL();
        /// <summary>
        /// 追号玩法加奖
        /// </summary>
        protected RegularChaseBLL ChaseBLL = new RegularChaseBLL();
        /// <summary>
        /// 胆拖玩法加奖
        /// </summary>
        protected RegularDanTuoBLL DanTuoBLL = new RegularDanTuoBLL();
        /// <summary>
        /// 节假日加奖
        /// </summary>
        protected RegularHolidayBLL HolidayBLL = new RegularHolidayBLL();
        /// <summary>
        /// 标准玩法加奖
        /// </summary>
        protected RegularNormBLL NormBLL = new RegularNormBLL();
        /// <summary>
        /// 投注金额上限加奖
        /// </summary>
        protected RegularTopLimitBLL TopLimitBLL = new RegularTopLimitBLL();

        #endregion
        #region 参数
        protected int Award_LotteryCode = 0;
        protected List<udv_ComputeTicket> Award_Ticket = null;
        #endregion
        public AwardEntrance(int LotteryCode, List<udv_ComputeTicket> Ticket)
        {
            Award_LotteryCode = LotteryCode;
            Award_Ticket = Ticket;
        }

        /// <summary>
        /// 入口
        /// </summary>
        public void Main()
        {
            Task.Factory.StartNew(() =>
            {
                //中奖累计名次玩法加奖
                AwadRankingBLL.CalculateAward(Award_LotteryCode);
                //中奖累计区间玩法加奖
                AwardIntervalBLL.CalculateAward(Award_LotteryCode);
                //中球玩法加奖
                BallBLL.CalculateAward(Award_Ticket, Award_LotteryCode);
                //投注累计区间玩法加奖
                BetIntervalBLL.CalculateAward(Award_LotteryCode);
                //投注累计名次玩法加奖
                BetRankingBLL.CalculateAward(Award_LotteryCode);
                //追号加奖玩法
                ChaseBLL.CalculateAward(Award_LotteryCode);
                //胆拖玩法
                DanTuoBLL.CalculateAward(Award_Ticket, Award_LotteryCode);
                //节假日玩法加奖
                HolidayBLL.CalculateAward(Award_Ticket, Award_LotteryCode);
                //标准玩法加奖
                NormBLL.CalculateAward(Award_Ticket, Award_LotteryCode);
                // 投注金额上限加奖 
                TopLimitBLL.CalculateAward(Award_LotteryCode);
            });
        }
    }
}
