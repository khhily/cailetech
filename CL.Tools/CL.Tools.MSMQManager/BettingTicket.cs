using System;
using System.Xml;

namespace CL.Tools.MSMQManager
{
    [Serializable]
    public class BettingTicket
    {
        /// <summary>
        /// 方案号
        /// </summary>
        public long SchemeID { get; set; }
        public long ChaseTaskDetailsID { get; set; }
        public int Status { get; set; }
        /// <summary>
        /// 接口配置信息
        /// </summary>
        public XmlNode InterfaceConfig { get; set; }
    }
}
