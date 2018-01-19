using System.Collections.Generic;

namespace CL.View.Entity.BasicTable
{
    /// <summary>
    /// 和值基础表
    /// </summary>
    public class ValueTable
    {
        /// <summary>
        /// 和值
        /// </summary>
        public string SumValue { set; get; }
        /// <summary>
        /// 和值对应的注数
        /// </summary>
        public int Bet { set; get; }
        /// <summary>
        /// 和值对应的选号
        /// </summary>
        public List<string> Numbers { set; get; }
    }
}
