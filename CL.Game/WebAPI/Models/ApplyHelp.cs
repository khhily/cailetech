using CL.Enum.Common;
using CL.Json.Entity.WebAPI;
using CL.Tools.Common;
using System;
using System.Configuration;
using System.Xml;

namespace WebAPI.Models
{
    public class ApplyHelp
    {
        public static ApplyStartResult ApplyStartConfig(string Equipment, string Version)
        {
            try
            {
                string xmlpath = ConfigurationManager.AppSettings["APPLY"];
                int IsUpdate = -1;
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlpath);
                XmlNode Node = doc.SelectSingleNode(string.Format("Apply/{0}", Equipment.ToUpper()));

                bool HotModify = false;
                bool Modify = false;

                //四位
                var VersionInfo = Version.Split('.');
                var NodeVersion = Node.SelectSingleNode("Version").InnerText.Split('.');

                if (Convert.ToInt32(NodeVersion[0]) != Convert.ToInt32(VersionInfo[0]))
                    Modify = true;
                else if (Convert.ToInt32(NodeVersion[1]) != Convert.ToInt32(VersionInfo[1]))
                    Modify = true;
                else if (Convert.ToInt32(NodeVersion[2]) != Convert.ToInt32(VersionInfo[2]))
                    HotModify = true;


                if (Modify)
                    IsUpdate = 1; //强更
                else if (HotModify)
                    IsUpdate = 0;  //热更
                else
                    IsUpdate = -1; //不更
                //Web端只热更(客户端开发人员强调)
                if (Equipment.ToLower() == "web")
                    IsUpdate = 0;  //热更

                return new ApplyStartResult()
                {
                    Code = 0,
                    Msg = "成功",
                    GatewayUrl = Convert.ToString(Node.SelectSingleNode("GatewayUrl").InnerText),
                    HotUpdateUrl = Convert.ToString(Node.SelectSingleNode("HotUpdateUrl").InnerText),
                    UpdateUrl = Convert.ToString(Node.SelectSingleNode("UpdateUrl").InnerText),
                    WebAPIUrl = Convert.ToString(Node.SelectSingleNode("WebAPIUrl").InnerText),
                    SocketURL = Convert.ToString(Node.SelectSingleNode("SocketURL").InnerText),
                    IsUpdate = IsUpdate,
                    Status = Convert.ToInt32(Node.SelectSingleNode("Status").InnerText)
                };
            }
            catch (Exception ex)
            {
                new Log("ApplyHelp").Write("应用程序启动错误[ApplyStartConfig]：" + ex.StackTrace, true);
                return new ApplyStartResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Common.GetDescription(ResultCode.SystemBusy)
                };
            }
        }
    }
}