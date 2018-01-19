
namespace CL.Tools.Common
{
    public class DataCheck
    {
        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isSymbol">是否有符号，false表示正整数</param>
        /// <param name="digit">位数，最小位数</param>
        /// <param name="digitend">最大位数</param>
        /// <returns></returns>
        public static bool IsNum(string value, bool isSymbol = false, int digit = 0, int digitend = 0)
        {
            string regex = @"\d+";
            if (digit > 0)
                if (digitend > 0)
                    regex = @"\d{" + digit + "," + digitend + "}";
                else
                    regex = @"\d{" + digit + "}";
            if (isSymbol)
                regex = @"[-]?" + regex;

            regex = @"^" + regex + "$";

            return System.Text.RegularExpressions.Regex.IsMatch(value, regex);
        }
        /// <summary>
        /// 是否手机号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMobile(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[1]+[3-8]+\d{9}$");
        }
        /// <summary>
        /// 是否身份证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIDCard(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"(^\d{18}$)|(^\d{17}[x|X]$)|(^\d{15}$)");
        }
        /// <summary>
        /// 是否邮箱
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
        /// <summary>
        /// 是否中文
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool WhetherChinese(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[\u4e00-\u9fa5]+$");
        }
    }
}
