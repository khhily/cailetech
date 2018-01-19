using CL.Game.BLL;
using CL.Game.BLL.Tools;
using CL.Tools.Common;
using CL.Tools.LotteryTickets;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CL.Plugins.LotteryTickets.Lottery
{
    public class KL28 : LotteryBase
    {
        Log log = new Log("KL28");
        private IsusesBLL bllis = new IsusesBLL();
        public KL28()
        {

        }
        public KL28(XmlNode _xml)
        {

        }

        public override LotteryResult GetValue()
        {
            LotteryResult ent = new LotteryResult();
            bool IsInterface = true;
            string Config_Key = ConfigurationManager.AppSettings["IsInterface"];
            bool.TryParse(Config_Key, out IsInterface);
            try
            {
                if (IsInterface)
                {
                    ent = GetValue1();
                    if (ent != null)
                    {
                        #region 新的开奖方式
                        string ReturnDescription = "";
                        bool res = bllis.EnteringDrawResults(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.LotteryTime, ref ReturnDescription);
                        if (res)
                        {
                            if (ReturnDescription.Trim() == "添加开奖信息成功" && !string.IsNullOrEmpty(ent.LotteryWinNum))
                            {
                                new IsusesBLL().InsertIsuseInfoRedis(ent.LotteryCode, ent.IsuseName, ent.LotteryWinNum, ent.StartTime, ent.EndTime, ent.LotteryTime, 0, 0);
                            }
                            Console.WriteLine(string.Format("抓取开奖结果：{0}", ent.ToString()));
                        }
                        #endregion
                    }
                }
                return ent;
            }
            catch (Exception ex)
            {
                log.Write("读取XML数据失败,错误日志:" + ex.Message, true);
                return null;
            }
        }

        protected LotteryResult GetValue1()
        {
            try
            {
                string htmlContent = HttpProxy.HttpGetProxy("https://kaijiang.500.com/kl8.shtml");
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);
                if (htmlContent == "")
                    return null;
                HtmlNode collection = doc.DocumentNode.SelectSingleNode("//table[@class='kj_tablelist02']");
                if (collection != null)
                {




                    return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
