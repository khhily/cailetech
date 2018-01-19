using System.Collections.Generic;

namespace CL.Json.Entity.WebAPI
{
    public class ChaseRecordResult : JsonResult
    {
        public ChaseRecordEntity Data { set; get; }
    }
    public class ChaseRecordEntity
    {
        public string LotteryName { set; get; }
        public int LotteryCode { set; get; }
        public long Amount { set; get; }
        public long WinMoney { set; get; }
        public int IsuseCount { set; get; }
        public int OrderStatus { set; get; }
        public string Number { set; get; }
        public int Already { set; get; }
        public int Surplus { set; get; }
        /// <summary>
        /// 停止条件(0表示中奖停止;大于0表示累计中奖达到停止)
        /// </summary>
        public long Stops { set; get; }
        public List<AlreadyEntity> AlreadyData { set; get; }
        public List<SurplusEntity> SurplusData { set; get; }
    }
    /// <summary>
    /// 已追号列表
    /// </summary>
    public class AlreadyEntity
    {
        public long ChaseDetailCode { set; get; }
        public string ChaseStatus { set; get; }
        public string IsuseNum { set; get; }
        public long Amount { set; get; }
        public long WinMoney { set; get; }

    }
    /// <summary>
    /// 剩余追号列表
    /// </summary>
    public class SurplusEntity
    {
        public long ChaseDetailCode { set; get; }
        public string ChaseStatus { set; get; }
        public string IsuseNum { set; get; }
        public long Amount { set; get; }
    }
}
