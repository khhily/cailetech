namespace CL.Tools.TicketInterface.arithmetic
{
    /// <summary>
    /// 重庆时时彩算法
    /// </summary>
    public class CQSSCSF
    {
        /// <summary>
        /// 阶层
        /// </summary>
        /// <param name="m">层数</param>
        /// <returns>阶层结果</returns>
        public static int GetJC(int m)
        {
            if (m == 0 || m == 1)
            {
                return 1;
            }
            else
            {
                return m * GetJC(m - 1);
            }
        }

        /// <summary>
        /// 二星和值注数
        /// </summary>
        /// <param name="sum">和值</param>
        /// <param name="isZX">是否组选</param>
        /// <returns>注数</returns>
        public static int GetEXHZBet(int sum, bool isZX = false)
        {
            int result = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (sum == i + j)
                    {
                        result++;
                    }
                }
            }
            if (isZX == true)
            {
                if (result % 2 != 0)
                {
                    result++;
                }
                result = result / 2;
            }

            return result;
        }

        /// <summary>
        /// 获取注数
        /// </summary>
        /// <param name="sep">分隔符</param>
        /// <param name="sepNext">次分隔符</param>
        /// <param name="numStr">统计的号码</param>
        /// <param name="bet">默认注数</param>
        /// <returns>注数</returns>
        public static int GetBet(char sep, char sepNext, string numStr, int bet = 1)
        {
            var numArray = numStr.Split(sep);
            foreach (var num in numArray)
            {
                bet *= num.Split(sepNext).Length;
            }
            return bet;
        }

        /// <summary>
        /// 获取注数
        /// </summary>
        /// <param name="sep">分隔符</param>
        /// <param name="numStr">统计的号码</param>
        /// <param name="bet">默认注数</param>
        /// <returns>注数</returns>
        public static int GetBet(char sep, string numStr, int bet = 1)
        {
            var numArray = numStr.Split(sep);
            bet = numArray.Length * (numArray.Length - 1);
            return bet;
        }
    }
}
