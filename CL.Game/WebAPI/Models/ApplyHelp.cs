using CL.Enum.Common;
using CL.Json.Entity.WebAPI;
using CL.Tools.Common;
using System;
using System.Xml;

namespace WebAPI.Models
{
    public class ApplyHelp
    {
        public static ApplyStartResult ApplyStartConfig(string Equipment, string Version)
        {
            try
            {
                XmlNode Node = Utils.QueryConfigNode(string.Format("root/apply/{0}", Equipment.ToLower()));
                int IsUpdate = -1;
                bool HotModify = false;
                bool Modify = false;

                //四位
                var VersionInfo = Version.Split('.');
                var NodeVersion = Node.SelectSingleNode("version").InnerText.Split('.');

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
                    GatewayUrl = Convert.ToString(Node.SelectSingleNode("gateway").InnerText),
                    HotUpdateUrl = Convert.ToString(Node.SelectSingleNode("hotupdate").InnerText),
                    UpdateUrl = Convert.ToString(Node.SelectSingleNode("update").InnerText),
                    WebAPIUrl = Convert.ToString(Node.SelectSingleNode("webapi").InnerText),
                    SocketURL = Convert.ToString(Node.SelectSingleNode("socket").InnerText),
                    IsUpdate = IsUpdate,
                    Status = Convert.ToInt32(Node.SelectSingleNode("status").InnerText)
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