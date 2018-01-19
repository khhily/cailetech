using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Tools.Common;
using System.Web;
using CL.Enum.Common.IM;

/// <summary>
/// 内部IM通讯机制
/// </summary>
namespace CL.Game.BLL.IM
{
    public class Communication
    {
        Log log = new Log("Communication");
        public static string IMUrl = ConfigHelper.GetConfigString("IMURL");

        /// <summary>
        /// 登录内部IM通讯
        /// </summary>
        /// <param name="uid">用户编号</param>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        public bool Login(long uid, string token, bool allowchat)
        {
            try
            {
                string Param = string.Format("msgtype=login&userid={0}&token={1}&allowchat={2}", uid, token, allowchat == true ? 1 : 0);
                log.Write(string.Format("登录IM：{0}{1}", IMUrl, Param), true);
                return this.CommonPush(IMUrl, Param);
            }
            catch (Exception ex)
            {
                log.Write(string.Format("登录IM通讯错误：{0}", ex.StackTrace), true);
                return false;
            }
        }

        /// <summary>
        /// 解除禁言
        /// </summary>
        /// <returns></returns>
        public bool UnBanToPost(long uid, string token, string reason)
        {
            try
            {
                string Param = string.Format("msgtype=setuserblack&userid={0}&token={1}&reason={2}&action=del", uid, token, reason);
                log.Write(string.Format("IM解除禁言：{0}{1}", IMUrl, Param), true);
                return this.CommonPush(IMUrl, Param);
            }
            catch (Exception ex)
            {
                log.Write(string.Format("登录IM通讯错误：{0}", ex.StackTrace), true);
                return false;
            }
        }
        /// <summary>
        /// 发送内部IM通讯
        /// </summary>
        /// <param name="to">消息发送目标:1为全服   2为房间   3为个人</param>
        /// <param name="rc">消息接收者:为1时不需提供此参数 to为1时 此参数指定房间, to为2时 此参数为接收者token</param>
        /// <param name="msg">消息内容</param>
        /// <param name="isrecord">1代表 记录聊天数据，非1不记录</param>
        /// <returns></returns>
        public bool Sender(int to, string rc, string msg, int isrecord)
        {
            try
            {
                string Param = string.Format("msgtype=publish&to={0}&rc={1}&msg={2}&isrecord={3}", to, rc, msg, isrecord);
                log.Write(string.Format("IM消息发送：{0}", Param));
                return this.CommonPush(IMUrl, Param);
            }
            catch (Exception ex)
            {
                log.Write(string.Format("IM消息通讯错误：{0}", ex.StackTrace), true);
                return false;
            }
        }

        /// <summary>
        /// 设置内部IM房间
        /// </summary>
        /// <param name="roomid">房间id, 如果此参数非空为修改 否则为增加</param>
        /// <param name="roomname">房间名称</param>
        /// <param name="managerid">管理员id</param>
        /// <param name="allowchat">房间是否允许聊天 0禁止 1允许</param>
        /// <param name="lotteryid">彩种id</param>
        /// <param name="roomstate">房间状态 0关闭  1开启</param>
        /// <returns></returns>
        public bool SetRoom(string roomid, string roomname, string managerid, int allowchat, int lotteryid, int roomstate)
        {
            try
            {
                string Param = string.Format("msgtype=setroomconfig&roomname={0}&managerid={1}&allowchat={2}&lotteryid={3}&roomstate={4}&action=1&roomid={5}",
                    roomname,
                    managerid,
                    allowchat,
                    lotteryid,
                    roomstate,
                    roomid);
                log.Write(string.Format("IM房间设置：{0}", Param));
                return this.CommonPush(IMUrl, Param);
            }
            catch (Exception ex)
            {
                log.Write(string.Format("IM房间通讯错误：{0}", ex.StackTrace), true);
                return false;
            }
        }
        /// <summary>
        /// 删除房间信息
        /// </summary>
        /// <param name="roomid"></param>
        /// <returns></returns>
        public bool DelRoom(string roomid)
        {
            try
            {
                string Param = string.Format("msgtype=delroom&roomid={0}", roomid);
                return this.CommonPush(IMUrl, Param);
            }
            catch (Exception ex)
            {
                log.Write(string.Format("删除IM房间错误：{0}", ex.StackTrace), true);
                return false;
            }
        }

        /// <summary>
        /// IM公共调用
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool CommonPush(string url, string param)
        {
            #region 开关
            bool IMLoginSwitch = true;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IMLOGINSWITCH"] ?? "true", out IMLoginSwitch);
            if (!IMLoginSwitch)
                return true;
            #endregion
            string Rec = Utils.HttpPost(url, param);
            if (string.IsNullOrEmpty(Rec))
                return false;
            var RecEntity = Newtonsoft.Json.JsonConvert.DeserializeObject<IMResult>(Rec);
            if (RecEntity == null)
                return false;
            if (RecEntity.code == 1 && RecEntity.data.ToLower() == "sucess")
                return true;
            else
                return false;
        }

        /// <summary>
        /// 投注消息
        /// </summary>
        /// <param name="target_nick"></param>
        /// <param name="chatroom_id"></param>
        /// <param name="IsuseName"></param>
        /// <param name="LotteryName"></param>
        /// <param name="OrderCode"></param>
        /// <param name="BuyType"></param>
        /// <param name="Amount"></param>
        /// <param name="AvatarUrl"></param>
        /// <param name="LotNumber"></param>
        /// <param name="errorinfo"></param>
        /// <returns></returns>
        public bool Send_Api_Chatrooms_Bet(string target_nick, string chatroom_id, string IsuseName, string LotteryName, long OrderCode, byte BuyType, long Amount, string AvatarUrl, string LotNumber, ref string errorinfo)
        {
            string Parameter = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", target_nick, chatroom_id, IsuseName, LotteryName, OrderCode, BuyType, Amount, AvatarUrl, LotNumber);
            try
            {
                Bet_Detail ext_manager_bet_detail = new Bet_Detail();
                ext_manager_bet_detail.AvatarUrl = AvatarUrl;
                ext_manager_bet_detail.Name = target_nick;
                ext_manager_bet_detail.LotNumber = LotNumber;
                Bet ext_manager_bet = new Bet();
                ext_manager_bet.CustomMsgType = 4;
                ext_manager_bet.MsgType = 1;
                ext_manager_bet.Amount = Amount;
                ext_manager_bet.BuyType = BuyType;
                ext_manager_bet.IsuseName = IsuseName;
                ext_manager_bet.LotteryName = LotteryName;
                ext_manager_bet.OrderCode = OrderCode;
                ext_manager_bet.Data = ext_manager_bet_detail;
                bool Rec = this.Sender((int)SendToEnums.Room, chatroom_id, Newtonsoft.Json.JsonConvert.SerializeObject(ext_manager_bet), (int)IMRecordEnums.Yes);
                if (Rec)
                    errorinfo = "发送成功";
                else
                    errorinfo = "发送失败";
                return true;
            }
            catch (Exception ex)
            {
                errorinfo = string.Format("[{0}]聊天室发送投注消息失败：{1}{2}|{3}", target_nick, ex.Message, ex.Source, Parameter);
                log.Write(errorinfo, true);
                return false;
            }
        }
        /// <summary>
        /// 发送中奖信息
        /// </summary>
        /// <param name="chatroom_id"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public bool Send_Api_Chatrooms_Award(string chatroom_id, string Content)
        {
            try
            {
                Award ext_manager_award = new Award();
                ext_manager_award.CustomMsgType = 6;
                ext_manager_award.MsgType = 1;
                ext_manager_award.Content = Content.Trim();
                this.Sender((int)SendToEnums.Room, chatroom_id, Newtonsoft.Json.JsonConvert.SerializeObject(ext_manager_award), (int)IMRecordEnums.No);
                return true;
            }
            catch (Exception ex)
            {
                log.Write(string.Format("发送中奖信息失败：{0}", ex.StackTrace), true);
                return false;
            }
        }

        /// <summary>
        /// 发送公告信息
        /// </summary>
        /// <param name="chatroom_id"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public bool Send_Api_Chatrooms_Notice(string chatroom_id, string Content)
        {
            try
            {
                Notice ext_manager_notice = new Notice();
                ext_manager_notice.CustomMsgType = 5;
                ext_manager_notice.MsgType = 1;
                ext_manager_notice.Content = Content.Trim();
                this.Sender((int)SendToEnums.Room, chatroom_id, Newtonsoft.Json.JsonConvert.SerializeObject(ext_manager_notice), (int)IMRecordEnums.No);
                return true;
            }
            catch (Exception ex)
            {
                log.Write(string.Format("发送公告信息失败：{0}", ex.StackTrace), true);
                return false;
            }
        }
        /// <summary>
        /// 开奖信息发送
        /// </summary>
        /// <param name="chatroom_id"></param>
        /// <param name="lotid"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool Sendd_Api_Issue(string chatroom_id, int lotid, string issue, string num)
        {
            try
            {
                Issue ext_manager_issue = new Issue();
                ext_manager_issue.MsgType = 4;
                ext_manager_issue.CustomMsgType = 5;
                ext_manager_issue.LotteryCode = lotid;
                ext_manager_issue.Number = num.Trim();
                ext_manager_issue.IsuseNum = issue.Trim();
                ext_manager_issue.OpenTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                this.Sender((int)SendToEnums.All, chatroom_id, Newtonsoft.Json.JsonConvert.SerializeObject(ext_manager_issue), (int)IMRecordEnums.No);
                return true;
            }
            catch (Exception ex)
            {
                log.Write(string.Format("发送开奖信息失败：{0}", ex.StackTrace), true);
                return false;
            }
        }

        /// <summary>
        /// 彩种状态
        /// </summary>
        /// <param name="chatroom_id"></param>
        /// <param name="lotid"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool Sendd_Api_Lot(int lotid, int state)
        {
            try
            {
                Lot ext_manager_lot = new Lot();
                ext_manager_lot.MsgType = 5;
                ext_manager_lot.CustomMsgType = 5;
                ext_manager_lot.LotteryCode = lotid;
                ext_manager_lot.State = state;
                this.Sender((int)SendToEnums.All, "-1", Newtonsoft.Json.JsonConvert.SerializeObject(ext_manager_lot), (int)IMRecordEnums.No);
                return true;
            }
            catch (Exception ex)
            {
                log.Write(string.Format("发送开奖信息失败：{0}", ex.StackTrace), true);
                return false;
            }
        }
        /// <summary>
        /// 跑马灯
        /// </summary>
        /// <param name="chatroom_id"></param>
        /// <param name="lotid"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool Sendd_Api_Marquee(int Number, string Content)
        {
            try
            {
                Marquee ext_manager_Marquee = new Marquee();
                ext_manager_Marquee.MsgType = 3;
                ext_manager_Marquee.CustomMsgType = 5;
                ext_manager_Marquee.Number = Number;
                ext_manager_Marquee.Content = Content.Trim();
                this.Sender((int)SendToEnums.All, "-1", Newtonsoft.Json.JsonConvert.SerializeObject(ext_manager_Marquee), (int)IMRecordEnums.No);
                return true;
            }
            catch (Exception ex)
            {
                log.Write(string.Format("发送开奖信息失败：{0}", ex.StackTrace), true);
                return false;
            }
        }

    }
    public class IMResult
    {
        public int code { set; get; }
        public string data { set; get; }
    }
    public class IMManager
    {
        /// <summary>
        /// CHAT_PUBLIC_MemberJoin:1加入
        /// CHAT_PUBLIC_MuteList:2禁言
        /// CHAT_PUBLIC_Removed:3移除
        /// CHAT_PUBLIC_Bet:4投注
        /// CHAT_PUBLIC_Notice:5公告
        /// CHAT_PUBLIC_Award:6中奖
        /// </summary>
        public int CustomMsgType { set; get; }

        /// <summary>
        /// 1系统 
        /// 2玩家消息 
        /// 3跑马灯 
        /// 4当期开奖
        /// 5彩种停售
        /// </summary>
        public int MsgType { set; get; }
    }
    public class Bet_Detail
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 头像
        /// </summary>
        public string AvatarUrl { set; get; }
        /// <summary>
        /// 投注内容
        /// </summary>
        public string LotNumber { set; get; }
    }
    public class Bet : IMManager
    {
        /// <summary>
        /// 期号
        /// </summary>
        public string IsuseName { set; get; }
        /// <summary>
        /// 彩种
        /// </summary>
        public string LotteryName { set; get; }
        /// <summary>
        /// 方案编号
        /// </summary>
        public long OrderCode { set; get; }
        /// <summ, ary>
        /// 投注类型 0普通投注 1追号投注 2跟单投注
        /// </summary>
        public byte BuyType { set; get; }
        /// <summary>
        /// 投注金额
        /// </summary>
        public long Amount { set; get; }
        /// <summary>
        /// 投注详情
        /// </summary>
        public Bet_Detail Data { set; get; }

    }
    public class Award : IMManager
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { set; get; }

    }
    public class Notice : IMManager
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { set; get; }

    }
    public class Issue : IMManager
    {
        /// <summary>
        /// 彩种
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 期号
        /// </summary>
        public string IsuseNum { set; get; }

        /// <summary>
        /// 开奖号码
        /// </summary>
        public string Number { set; get; }
        public string OpenTime { set; get; }

    }
    public class Lot : IMManager
    {
        /// <summary>
        /// 彩种
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        public int State { set; get; }
    }
    public class Marquee : IMManager
    {
        /// <summary>
        /// 轮播次数
        /// </summary>
        public int Number { set; get; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { set; get; }
    }
}
