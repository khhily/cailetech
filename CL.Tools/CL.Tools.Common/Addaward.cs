using System;

namespace CL.Tools.Common
{
    public class Addaward
    {
        /// <summary>
        /// 是否今日开奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="LotteryDay"></param>
        /// <returns></returns>
        public static bool IsToDayLottery(int LotteryCode, string[] LotteryDay)
        {
            var weeks = (int)DateTime.Now.DayOfWeek;
            bool ToDayLottery = false;
            if (LotteryCode == 801 || LotteryCode == 901)
            {
                if (weeks == 0)
                    weeks = 7;
                foreach (var item in LotteryDay)
                    if (Convert.ToInt32(item) == weeks)
                    {
                        ToDayLottery = true;
                        continue;
                    }
            }
            return ToDayLottery;
        }
    }
}
