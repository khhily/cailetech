using CL.Json.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Entity.Json.WebAPI
{
    public class LotteryRoomResult : JsonResult
    {
        public List<Room> Data { set; get; }
    }

    /// <summary>
    /// 房间
    /// </summary>
    public class Room
    {
        /// <summary>
        /// 房间标识
        /// </summary>
        public long RoomID { set; get; }

        /// <summary>
        /// 房间编号
        /// </summary>
        public string RoomCode { set; get; }

        /// <summary>
        /// 房间限制人数
        /// </summary>
        public int RoomNums { set; get; }

        /// <summary>
        /// 房间名称
        /// </summary>
        public string RoomName { set; get; }

        /// <summary>
        /// 房间公告
        /// </summary>
        public string RoomNotice { set; get; }

        /// <summary>
        /// 发言频率
        /// </summary>
        public long Frequency { set; get; }

        /// <summary>
        /// 允许聊天设置：all,full,mobile,bank,order
        /// all：无限制条件
        /// full：实名认证
        /// mobile：绑定手机号码
        /// bank：绑定银行卡
        /// </summary>
        public string ChatSet { set; get; }

        /// <summary>
        /// 房主
        /// </summary>
        public long RoomAuthor { set; get; }

        /// <summary>
        /// 管理员表
        /// </summary>
        public List<Manager> ManagerData { set; get; }

        /// <summary>
        /// 回水表
        /// </summary>
        public List<Rebate> RebateData { set; get; }
    }

    /// <summary>
    /// 管理员
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// 房间管理员标识
        /// </summary>
        public long ManagerID { set; get; }
        /// <summary>
        /// 管理员编号
        /// </summary>
        public long UserCode { set; get; }

        /// <summary>
        /// 管理员房间昵称
        /// </summary>
        public string UserNick { set; get; }
    }

    /// <summary>
    /// 回水
    /// </summary>
    public class Rebate
    {
        /// <summary>
        /// 回水标识
        /// </summary>
        public long RebateID { set; get; }
        /// <summary>
        /// 最小金额
        /// </summary>
        public long MinAmount { set; get; }
        /// <summary>
        /// 最大金额
        /// </summary>
        public long MaxAmount { set; get; }
        /// <summary>
        /// 回水百分比
        /// </summary>
        public string Rebates { set; get; }

    }
}
