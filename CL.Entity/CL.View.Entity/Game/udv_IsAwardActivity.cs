using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    /// <summary>
    /// 查询是否存在加奖活动(标准)
    /// </summary>
    public class udv_IsAwardActivityNorm
    {
        /// <summary>
        /// 活动标识
        /// </summary>
        public int ActivityID { set; get; }

        /// <summary>
        /// 活动类型
        /// 0 官方活动动 1 彩乐活动
        /// </summary>
        public int ActivityType { set; get; }

        /// <summary>
        /// 活动总金额
        /// </summary>
        public long ActivityMoney { set; get; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public long ModifyMoney { set; get; }

        /// <summary>
        /// 是否变动活动
        /// </summary>
        public bool IsModify { set; get; }

        /// <summary>
        /// 规则标识
        /// </summary>
        public int RegularID { set; get; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 累计加奖金额
        /// </summary>
        public long TotalAwardMoney { set; get; }

        /// <summary>
        /// 规则类型：0 标准玩法加奖，1 追号加奖，2 胆拖玩法加奖，
        /// 3 球队加奖，4 串关加奖，5 投注金额累计区间加奖，6 中奖金额区间加奖，
        /// 7 活动期间投注金额累计名次加奖，8 活动期间中奖金额累计名次加奖，
        /// 9 数字彩中球加奖，10 节假日加奖
        /// </summary>
        public int RegularStatus { set; get; }

        /// <summary>
        /// 标准活动规则详细标识
        /// </summary>
        public int RNormID { set; get; }

        /// <summary>
        /// 加奖玩法
        /// </summary>
        public int PlayCode { set; get; }

        /// <summary>
        /// 单次加奖金额
        /// </summary>
        public long AwardMoney { set; get; }

        /// <summary>
        /// 单用户累计中奖上线金额：0 表示没有限制，大于0表示累计上限多少停止单用户加奖
        /// </summary>
        public long TopLimit { set; get; }
    }

    /// <summary>
    /// 查询是否存在加奖活动(胆拖)
    /// </summary>
    public class udv_IsAwardActivityDanTuo
    {
        /// <summary>
        /// 活动标识
        /// </summary>
        public int ActivityID { set; get; }

        /// <summary>
        /// 活动类型
        /// 0 官方活动动 1 彩乐活动
        /// </summary>
        public int ActivityType { set; get; }

        /// <summary>
        /// 活动总金额
        /// </summary>
        public long ActivityMoney { set; get; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public long ModifyMoney { set; get; }

        /// <summary>
        /// 是否变动活动
        /// </summary>
        public bool IsModify { set; get; }

        /// <summary>
        /// 规则标识
        /// </summary>
        public int RegularID { set; get; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 累计加奖金额
        /// </summary>
        public long TotalAwardMoney { set; get; }

        /// <summary>
        /// 规则类型：0 标准玩法加奖，1 追号加奖，2 胆拖玩法加奖，
        /// 3 球队加奖，4 串关加奖，5 投注金额累计区间加奖，6 中奖金额区间加奖，
        /// 7 活动期间投注金额累计名次加奖，8 活动期间中奖金额累计名次加奖，
        /// 9 数字彩中球加奖，10 节假日加奖
        /// </summary>
        public int RegularStatus { set; get; }

        /// <summary>
        /// 标准活动规则详细标识
        /// </summary>
        public int RDanTuoID { set; get; }

        /// <summary>
        /// 加奖玩法
        /// </summary>
        public int PlayCode { set; get; }

        /// <summary>
        /// 单次加奖金额
        /// </summary>
        public long AwardMoney { set; get; }

        /// <summary>
        /// 单用户累计中奖上线金额：0 表示没有限制，大于0表示累计上限多少停止单用户加奖
        /// </summary>
        public long TopLimit { set; get; }

        /// <summary>
        /// 胆码数量
        /// </summary>
        public int DanNums { set; get; }

        /// <summary>
        /// 拖码数量
        /// </summary>
        public int TuoNums { set; get; }
    }
    /// <summary>
    /// 查询是否存在加奖活动(追号)
    /// </summary>
    public class udv_IsAwardActivityChase
    {
        /// <summary>
        /// 活动标识
        /// </summary>
        public int ActivityID { set; get; }

        /// <summary>
        /// 活动类型
        /// 0 官方活动动 1 彩乐活动
        /// </summary>
        public int ActivityType { set; get; }

        /// <summary>
        /// 活动总金额
        /// </summary>
        public long ActivityMoney { set; get; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public long ModifyMoney { set; get; }

        /// <summary>
        /// 是否变动活动
        /// </summary>
        public bool IsModify { set; get; }

        /// <summary>
        /// 规则标识
        /// </summary>
        public int RegularID { set; get; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 累计加奖金额
        /// </summary>
        public long TotalAwardMoney { set; get; }

        /// <summary>
        /// 规则类型：0 标准玩法加奖，1 追号加奖，2 胆拖玩法加奖，
        /// 3 球队加奖，4 串关加奖，5 投注金额累计区间加奖，6 中奖金额区间加奖，
        /// 7 活动期间投注金额累计名次加奖，8 活动期间中奖金额累计名次加奖，
        /// 9 数字彩中球加奖，10 节假日加奖
        /// </summary>
        public int RegularStatus { set; get; }

        /// <summary>
        /// 追号活动规则详细标识
        /// </summary>
        public int RChaseID { set; get; }

        /// <summary>
        /// 加奖玩法
        /// </summary>
        public int PlayCode { set; get; }

        /// <summary>
        /// 单次加奖金额
        /// </summary>
        public long AwardMoney { set; get; }

        /// <summary>
        /// 单用户累计中奖上线金额：0 表示没有限制，大于0表示累计上限多少停止单用户加奖
        /// </summary>
        public long TopLimit { set; get; }

        /// <summary>
        /// 中奖类型：1 累计中奖次数，2 累计中奖金额
        /// </summary>
        public int RChaseType { set; get; }

        /// <summary>
        /// 单位： 次数或金额
        /// </summary>
        public int Unit { set; get; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { set; get; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { set; get; }
    }

    /// <summary>
    /// 节假日
    /// </summary>
    public class udv_IsAwardActivityHoliday
    {
        /// <summary>
        /// 活动标识
        /// </summary>
        public int ActivityID { set; get; }

        /// <summary>
        /// 活动类型
        /// 0 官方活动动 1 彩乐活动
        /// </summary>
        public int ActivityType { set; get; }

        /// <summary>
        /// 活动总金额
        /// </summary>
        public long ActivityMoney { set; get; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public long ModifyMoney { set; get; }

        /// <summary>
        /// 是否变动活动
        /// </summary>
        public bool IsModify { set; get; }

        /// <summary>
        /// 规则标识
        /// </summary>
        public int RegularID { set; get; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 累计加奖金额
        /// </summary>
        public long TotalAwardMoney { set; get; }

        /// <summary>
        /// 规则类型：0 标准玩法加奖，1 追号加奖，2 胆拖玩法加奖，
        /// 3 球队加奖，4 串关加奖，5 投注金额累计区间加奖，6 中奖金额区间加奖，
        /// 7 活动期间投注金额累计名次加奖，8 活动期间中奖金额累计名次加奖，
        /// 9 数字彩中球加奖，10 节假日加奖
        /// </summary>
        public int RegularStatus { set; get; }

        /// <summary>
        /// 节假日活动规则详细标识
        /// </summary>
        public int RHolidayID { set; get; }

        /// <summary>
        /// 加奖玩法
        /// </summary>
        public int PlayCode { set; get; }

        /// <summary>
        /// 单次加奖金额
        /// </summary>
        public long AwardMoney { set; get; }

        /// <summary>
        /// 单用户累计中奖上线金额：0 表示没有限制，大于0表示累计上限多少停止单用户加奖
        /// </summary>
        public long TopLimitDay { set; get; }

        /// <summary>
        /// 类型：1 周六加奖，2 周日加奖，3 周六日加奖，4 指定时间段加奖(按活动开始结束时间加奖)
        /// </summary>
        public int HolidayType { set; get; }

    }

    /// <summary>
    /// 投注金额累计区间
    /// </summary>
    public class udv_IsAwardActivityBetInterval
    {
        /// <summary>
        /// 活动标识
        /// </summary>
        public int ActivityID { set; get; }

        /// <summary>
        /// 活动类型
        /// 0 官方活动动 1 彩乐活动
        /// </summary>
        public int ActivityType { set; get; }

        /// <summary>
        /// 活动总金额
        /// </summary>
        public long ActivityMoney { set; get; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public long ModifyMoney { set; get; }

        /// <summary>
        /// 是否变动活动
        /// </summary>
        public bool IsModify { set; get; }

        /// <summary>
        /// 规则标识
        /// </summary>
        public int RegularID { set; get; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 累计加奖金额
        /// </summary>
        public long TotalAwardMoney { set; get; }

        /// <summary>
        /// 规则类型：0 标准玩法加奖，1 追号加奖，2 胆拖玩法加奖，
        /// 3 球队加奖，4 串关加奖，5 投注金额累计区间加奖，6 中奖金额区间加奖，
        /// 7 活动期间投注金额累计名次加奖，8 活动期间中奖金额累计名次加奖，
        /// 9 数字彩中球加奖，10 节假日加奖
        /// </summary>
        public int RegularStatus { set; get; }

        /// <summary>
        /// 投注累计区间活动规则详细标识
        /// </summary>
        public int RBetIntervalID { set; get; }

        /// <summary>
        /// 加奖玩法
        /// </summary>
        public int PlayCode { set; get; }

        /// <summary>
        /// 排名规则：root 顶级节点，item 项目节点，pk 序号(整型)，award 加奖金额(长整型，分为单位)，min 最小金额，max 最大金额(当最大金额为0时，则中奖金额累计大于等于最小金额时即可加奖)；最大支持十个区间加奖
        /// 实例 如：
        /// <?xml version = "1.0" encoding="utf-8"?>
        /// <root> 
        ///   <item> 
        ///     <pk>1</pk>  
        ///     <min>20000</min>  
        ///     <max>0</max>  
        ///     <award>10000</award> 
        ///   </item>  
        ///   <item> 
        ///     <pk>2</pk>  
        ///     <min>10000</min>  
        ///     <max>20000</max>  
        ///     <award>10000</award> 
        ///   </item> 
        /// </root>
        /// </summary>
        public string BetInterval { set; get; }

    }

    /// <summary>
    /// 投注金额累计名称
    /// </summary>
    public class udv_IsAwardActivityBetRanking
    {
        /// <summary>
        /// 活动标识
        /// </summary>
        public int ActivityID { set; get; }

        /// <summary>
        /// 活动类型
        /// 0 官方活动动 1 彩乐活动
        /// </summary>
        public int ActivityType { set; get; }

        /// <summary>
        /// 活动总金额
        /// </summary>
        public long ActivityMoney { set; get; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public long ModifyMoney { set; get; }

        /// <summary>
        /// 是否变动活动
        /// </summary>
        public bool IsModify { set; get; }

        /// <summary>
        /// 规则标识
        /// </summary>
        public int RegularID { set; get; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 累计加奖金额
        /// </summary>
        public long TotalAwardMoney { set; get; }

        /// <summary>
        /// 规则类型：0 标准玩法加奖，1 追号加奖，2 胆拖玩法加奖，
        /// 3 球队加奖，4 串关加奖，5 投注金额累计区间加奖，6 中奖金额区间加奖，
        /// 7 活动期间投注金额累计名次加奖，8 活动期间中奖金额累计名次加奖，
        /// 9 数字彩中球加奖，10 节假日加奖
        /// </summary>
        public int RegularStatus { set; get; }

        /// <summary>
        /// 投注累计区间活动规则详细标识
        /// </summary>
        public int RBetRanID { set; get; }

        /// <summary>
        /// 加奖玩法
        /// </summary>
        public int PlayCode { set; get; }

        /// <summary>
        /// 排名规则：root 顶级节点，item 项目节点，placing 名次(整型)，award 加奖金额(长整型，分为单位)；最大支持前十名或后十名加奖(倒数名称请按负数计算 如：倒数第一名  -1 ， 倒数第二 -2 等等)
        /// 实例 如：
        /// <?xml version = "1.0" encoding="utf-8"?>
        /// <root> 
        ///   <item> 
        ///     <placing>1</placing>  
        ///     <award>20000</award> 
        ///   </item>  
        ///   <item> 
        ///     <placing>2</placing>  
        ///     <award>8000</award> 
        ///   </item> 
        /// </root>
        /// </summary>
        public string BetRanking { set; get; }

    }

    /// <summary>
    /// 中奖金额累计区间
    /// </summary>
    public class udv_IsAwardActivityAwardInterval
    {
        /// <summary>
        /// 活动标识
        /// </summary>
        public int ActivityID { set; get; }

        /// <summary>
        /// 活动类型
        /// 0 官方活动动 1 彩乐活动
        /// </summary>
        public int ActivityType { set; get; }

        /// <summary>
        /// 活动总金额
        /// </summary>
        public long ActivityMoney { set; get; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public long ModifyMoney { set; get; }

        /// <summary>
        /// 是否变动活动
        /// </summary>
        public bool IsModify { set; get; }

        /// <summary>
        /// 规则标识
        /// </summary>
        public int RegularID { set; get; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 累计加奖金额
        /// </summary>
        public long TotalAwardMoney { set; get; }

        /// <summary>
        /// 规则类型：0 标准玩法加奖，1 追号加奖，2 胆拖玩法加奖，
        /// 3 球队加奖，4 串关加奖，5 投注金额累计区间加奖，6 中奖金额区间加奖，
        /// 7 活动期间投注金额累计名次加奖，8 活动期间中奖金额累计名次加奖，
        /// 9 数字彩中球加奖，10 节假日加奖
        /// </summary>
        public int RegularStatus { set; get; }

        /// <summary>
        /// 投注累计区间活动规则详细标识
        /// </summary>
        public int RAwardIntervalID { set; get; }

        /// <summary>
        /// 加奖玩法
        /// </summary>
        public int PlayCode { set; get; }

        /// <summary>
        /// 排名规则：root 顶级节点，item 项目节点，placing 名次(整型)，award 加奖金额(长整型，分为单位)；最大支持前十名或后十名加奖(倒数名称请按负数计算 如：倒数第一名  -1 ， 倒数第二 -2 等等)
        /// 实例 如：
        /// <?xml version = "1.0" encoding="utf-8"?>
        /// <root> 
        ///    <item> 
        ///         <pk>1</pk>  
        ///         <min>20000</min>  
        ///         <max>0</max>  
        ///         <award>10000</award> 
        ///    </item>  
        ///    <item> 
        ///         <pk>2</pk>  
        ///         <min>10000</min>  
        ///         <max>20000</max>  
        ///         <award>10000</award> 
        ///     </item> 
        /// </root>
        /// </summary>
        public string AwardInterval { set; get; }

    }

    /// <summary>
    /// 中奖金额累计名称
    /// </summary>
    public class udv_IsAwardActivityAwardRanking
    {
        /// <summary>
        /// 活动标识
        /// </summary>
        public int ActivityID { set; get; }

        /// <summary>
        /// 活动类型
        /// 0 官方活动动 1 彩乐活动
        /// </summary>
        public int ActivityType { set; get; }

        /// <summary>
        /// 活动总金额
        /// </summary>
        public long ActivityMoney { set; get; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public long ModifyMoney { set; get; }

        /// <summary>
        /// 是否变动活动
        /// </summary>
        public bool IsModify { set; get; }

        /// <summary>
        /// 规则标识
        /// </summary>
        public int RegularID { set; get; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 累计加奖金额
        /// </summary>
        public long TotalAwardMoney { set; get; }

        /// <summary>
        /// 规则类型：0 标准玩法加奖，1 追号加奖，2 胆拖玩法加奖，
        /// 3 球队加奖，4 串关加奖，5 投注金额累计区间加奖，6 中奖金额区间加奖，
        /// 7 活动期间投注金额累计名次加奖，8 活动期间中奖金额累计名次加奖，
        /// 9 数字彩中球加奖，10 节假日加奖
        /// </summary>
        public int RegularStatus { set; get; }

        /// <summary>
        /// 投注累计区间活动规则详细标识
        /// </summary>
        public int RAwardRanID { set; get; }

        /// <summary>
        /// 加奖玩法
        /// </summary>
        public int PlayCode { set; get; }

        /// <summary>
        /// 排名规则：root 顶级节点，item 项目节点，placing 名次(整型)，award 加奖金额(长整型，分为单位)；最大支持前十名或后十名加奖(倒数名称请按负数计算 如：倒数第一名  -1 ， 倒数第二 -2 等等)
        /// 实例 如：
        /// <?xml version = "1.0" encoding="utf-8"?>
        /// <root> 
        ///   <item> 
        ///     <placing>1</placing>  
        ///     <award>20000</award> 
        ///   </item>  
        ///   <item> 
        ///     <placing>2</placing>  
        ///     <award>8000</award> 
        ///   </item> 
        /// </root>
        /// </summary>
        public string AwardRanking { set; get; }

    }

    /// <summary>
    /// 中奖金额累计名称
    /// </summary>
    public class udv_IsAwardActivityBall
    {
        /// <summary>
        /// 活动标识
        /// </summary>
        public int ActivityID { set; get; }

        /// <summary>
        /// 活动类型
        /// 0 官方活动动 1 彩乐活动
        /// </summary>
        public int ActivityType { set; get; }

        /// <summary>
        /// 活动总金额
        /// </summary>
        public long ActivityMoney { set; get; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public long ModifyMoney { set; get; }

        /// <summary>
        /// 是否变动活动
        /// </summary>
        public bool IsModify { set; get; }

        /// <summary>
        /// 规则标识
        /// </summary>
        public int RegularID { set; get; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 累计加奖金额
        /// </summary>
        public long TotalAwardMoney { set; get; }

        /// <summary>
        /// 规则类型：0 标准玩法加奖，1 追号加奖，2 胆拖玩法加奖，
        /// 3 球队加奖，4 串关加奖，5 投注金额累计区间加奖，6 中奖金额区间加奖，
        /// 7 活动期间投注金额累计名次加奖，8 活动期间中奖金额累计名次加奖，
        /// 9 数字彩中球加奖，10 节假日加奖
        /// </summary>
        public int RegularStatus { set; get; }

        /// <summary>
        /// 数字彩中球活动规则详细标识
        /// </summary>
        public int RBallID { set; get; }

        /// <summary>
        /// 加奖玩法
        /// </summary>
        public int PlayCode { set; get; }

        /// <summary>
        /// 加奖金额
        /// </summary>
        public long AwardMoney { set; get; }

        /// <summary>
        /// 上限金额
        /// </summary>
        public long TopLimit { set; get; }

        /// <summary>
        /// 类型：1 指定红球数字，2 指定蓝球数字(高频彩没有蓝球)
        /// </summary>
        public int BallType { set; get; }

        /// <summary>
        /// 球：彩种选号，数字超过9的 请以两位字符表示 如：双色球 01，02  多个球请已英文字符逗号隔开
        /// </summary>
        public string Ball { set; get; }

    }

    /// <summary>
    /// 投注金额上限
    /// </summary>
    public class udv_IsAwardActivityTopLimit
    {
        /// <summary>
        /// 活动标识
        /// </summary>
        public int ActivityID { set; get; }

        /// <summary>
        /// 活动类型
        /// 0 官方活动动 1 彩乐活动
        /// </summary>
        public int ActivityType { set; get; }

        /// <summary>
        /// 活动总金额
        /// </summary>
        public long ActivityMoney { set; get; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public long ModifyMoney { set; get; }

        /// <summary>
        /// 是否变动活动
        /// </summary>
        public bool IsModify { set; get; }

        /// <summary>
        /// 规则标识
        /// </summary>
        public int RegularID { set; get; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 累计加奖金额
        /// </summary>
        public long TotalAwardMoney { set; get; }

        /// <summary>
        /// 规则类型：0 标准玩法加奖，1 追号加奖，2 胆拖玩法加奖，
        /// 3 球队加奖，4 串关加奖，5 投注金额累计区间加奖，6 中奖金额区间加奖，
        /// 7 活动期间投注金额累计名次加奖，8 活动期间中奖金额累计名次加奖，
        /// 9 数字彩中球加奖，10 节假日加奖
        /// </summary>
        public int RegularStatus { set; get; }

        /// <summary>
        /// 投注金额上限活动规则详细标识
        /// </summary>
        public int RTopLimitID { set; get; }

        /// <summary>
        /// 加奖玩法
        /// </summary>
        public int PlayCode { set; get; }

        /// <summary>
        /// 加奖金额
        /// </summary>
        public long AwardMoney { set; get; }

        /// <summary>
        /// 上限金额
        /// </summary>
        public long TotalMoney { set; get; }

    }
}
