using System.ComponentModel;

namespace CL.Enum.Common
{
    public enum RedisKeysEnum
    {
        [Description("令牌(加Token值)")]
        Token,

        [Description("大厅彩种")]
        HallLottery,

        [Description("大厅累计中奖")]
        HallTotalWin,

        [Description("app应用底部栏目")]
        ApplyShowModel,

        [Description("登陆的用户信息(加UserCode)")]
        SignInUser,

        [Description("用户登录日志")]
        UserLoginRecord,

        [Description("验证码(加VerifyType和Mobile)")]
        VerifyCode,

        [Description("身份认证(加UserCode)")]
        Verification,

        [Description("绑定银行卡(加UserCode)")]
        BankCard,

        [Description("提现申请(加UserCode)")]
        WithdrawDeposit,

        [Description("充值记录(加UserCode)")]
        RechargeRecord,

        [Description("交易记录(加UserCode)")]
        TradingRecord,

        [Description("方案总记录(加UserCode)")]
        SchemeRecord,

        [Description("方案中奖记录(加UserCode)")]
        SchemeRecordWinMoney,

        [Description("方案待开奖记录(加UserCode)")]
        SchemeRecordWait,

        [Description("方案追号记录(加UserCode)")]
        SchemeRecordChase,

        [Description("方案详情(加SchemeID)")]
        SchemeDetail,

        [Description("当期开期(加LotteryCode)")]
        CurrentIsuse,

        [Description("开奖列表(加LotteryCode)")]
        OfLottery,

        [Description("开奖详情(加LotteryCode和IsuseNum)")]
        OfLotteryDetail,

        [Description("追号记录(加UserCode和SchemeID)")]
        ChaseRecord,

        [Description("追号详情(加ChaseDetailID)")]
        ChaseRecordDetail,

        [Description("奖等类型(加LotteryCode和WinCode)")]
        WinTypes,

        [Description("基础表")]
        BasicTable,

        [Description("和值")]
        Value,

        [Description("手动开奖")]
        ManualOpenLottery,

        [Description("彩种")]
        Lot,

        [Description("彩种")]
        IsuseName,

        [Description("临时方案列表")]
        Schemes,

        [Description("临时方案详情")]
        SchemesDetail,

        [Description("临时方案电子票")]
        SchemeETickets,

        [Description("临时追号列表")]
        ChaseTasks,

        [Description("临时追号详情")]
        ChaseTaskDetails,

        [Description("临时中奖列表")]
        SchemesWin,

        [Description("期号")]
        IsusesName,

        [Description("模版")]
        TemplateConfig,

        [Description("模版")]
        LoginError,

        [Description("方案实体对象")]
        Entitys,

        [Description("方案总记录(加UserCode)")]
        Scheme,

        [Description("方案中奖记录(加UserCode)")]
        SchemeWin,

        [Description("方案待开奖记录(加UserCode)")]
        SchemeWait,

        [Description("方案追号记录(加UserCode)")]
        SchemeChase,

        [Description("提现次数限制")]
        WithdrawCount,

        [Description("开奖大厅")]
        OpenAwardHall,

        [Description("代理")]
        Proxy,

        [Description("用户推送")]
        UsersPush,

        [Description("定时器使用")]
        Scheduler,

        [Description("小游戏")]
        MiniGame,

        [Description("游戏批次")]
        GameBatch,

        [Description("彩券批次")]
        BatchCirculation,

        [Description("小游戏记录")]
        GameRecord,

        [Description("小游戏记录")]
        GameRecordList,

        [Description("分析资讯")]
        NewAnalysis,

        [Description("客户端分析资讯")]
        ClientNews,

        [Description("分享")]
        Sharing,

        [Description("房间")]
        Rooms
    }
}
