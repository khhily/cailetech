using System.ComponentModel;

namespace CL.Enum.Common
{
    public enum ResultCode
    {
        [Description("成功")]
        Success = 0,

        [Description("手机格式不正确")]
        MobileFormatError = 1,

        [Description("用户编码格式不正确")]
        UserIdError = 2,

        [Description("旧密码为空")]
        OldPwdNull = 3,

        [Description("旧密码错误")]
        OldPwdError = 4,

        [Description("新密码为空")]
        NewPwdNull = 5,

        [Description("旧支付密码为空")]
        OldPayPwdNull = 6,

        [Description("旧支付密码错误")]
        OldPayPwdError = 7,

        [Description("新支付密码为空")]
        NewPayPwdNull = 8,

        [Description("新旧密码不能一致")]
        NewOldPwdNoEqual = 9,

        [Description("未设置支付密码")]
        NoSetPayPwd = 10,

        [Description("已设置支付密码")]
        SetPayPwd = 11,

        [Description("重复关注好友")]
        RepeatFans = 12,

        [Description("不是好友关系")]
        NoFans = 13,

        [Description("关注好友失败")]
        ConcernFailure = 14,

        [Description("取消失败")]
        CanclFailure = 15,

        [Description("体现申请失败")]
        PayOutApplyForFailure = 16,

        [Description("支付密码错误")]
        PayPwdFailure = 17,

        [Description("余额不足")]
        InsufficientBalance = 18,

        [Description("订单号错误")]
        OrderNoError = 19,

        [Description("交易号错误")]
        AlipayNoError = 20,

        [Description("支付类型错误")]
        PayTypeError = 21,

        [Description("充值金额错误")]
        PayAmountError = 22,

        [Description("昵称不能为空")]
        NickNameNull = 23,

        [Description("昵称编辑失败")]
        EditNickNameFailure = 24,

        [Description("旧手机号码格式不正确")]
        OldMobileFormatError = 25,

        [Description("新手机号码格式不正确")]
        NewMobileFormatError = 26,

        [Description("旧手机号码有误")]
        OldMobileFailure = 27,

        [Description("手机号码修改失败")]
        EditMobileFailure = 28,

        [Description("手机号已注册,请更换")]
        Mobiles = 29,

        [Description("昵称编辑成功，云信昵称编辑失败")]
        IMFailure = 30,

        [Description("IP地址错误")]
        IPError = 31,

        [Description("充值失败")]
        PayDetailsFailure = 32,

        [Description("参数verify值非法")]
        VerifyUnlawful = 33,

        [Description("同一号码每天最多只能发送10次，请隔天再试")]
        AlternateDays = 34,

        [Description("短信平台发送异常")]
        SendException = 35,

        [Description("验证码格式不正确")]
        CodeFormatError = 36,

        [Description("验证码无效")]
        CodeInvalidation = 37,

        [Description("验证码错误")]
        CodeError = 38,

        [Description("登录密码为空")]
        LoginPwdNull = 39,

        [Description("用户已存在")]
        UserAlreadyExist = 40,

        [Description("注册失败")]
        RegisterFailure = 41,

        [Description("终端类型为空")]
        TerminalNull = 42,

        [Description("登录密码错误")]
        LoginPwdError = 43,

        [Description("用户登录被禁用")]
        UserIsCanLogin = 44,

        [Description("用户为机器人，不允许登录")]
        UserIsRobot = 45,

        [Description("用户不存在")]
        UserNotExist = 46,

        [Description("索引格式不正确")]
        IndexFormatError = 47,

        [Description("页记录格式不正确")]
        SizeFormatError = 48,

        [Description("没有数据")]
        NullData = 49,

        [Description("姓名格式不正确")]
        FullNameFormatError = 50,

        [Description("身份证号码格式不正确")]
        IdentifyFormatError = 51,

        [Description("身份已验证")]
        IdentifyVerify = 52,

        [Description("银行卡格式不正确")]
        BankCardFormatError = 53,

        [Description("支付密码格式不正确")]
        PayPwdFormatError = 54,

        [Description("支付金额格式不正确")]
        PayAmountFormatError = 55,

        [Description("此卡与用户不存在绑定关系")]
        BankCardRelationError = 56,

        [Description("银行名称格式不正确")]
        BankNameFormatError = 57,

        [Description("归属地市格式不正确")]
        AreaFormatError = 58,

        [Description("该银行卡已存在")]
        IsBankCardExist = 59,

        [Description("余额不足")]
        NotSufficientFunds = 60,

        [Description("配置的起止发售时间参数不正确")]
        StartStopIncorrectParameter = 61,

        [Description("隔天首期预售投注参数不正确")]
        OtherDayIncorrectParameter = 62,

        [Description("当天预售下期投注参数不正确")]
        DayIncorrectParameter = 63,

        [Description("当期投注参数不正确")]
        IssueIncorrectParameter = 64,

        [Description("订单不存在")]
        OrderNull = 65,

        [Description("订单状态格式不正确")]
        OrderStatusFormatError = 66,

        [Description("彩种id格式不正确")]
        LotteryCodeError = 67,

        [Description("没有最新期数信息")]
        NoClausesIssue = 68,

        [Description("投注信息为空")]
        BettingInfoNull = 69,

        [Description("方案订单金额格式不正确")]
        SchemeMoneyError = 70,

        [Description("期数ID格式不正确")]
        IssueNumError = 71,

        [Description("预售状态格式不正确")]
        PresellFormatError = 72,

        [Description("是否付款状态格式不正确")]
        IsPayFormatError = 73,

        [Description("当期的销售起止时间格式不正确")]
        StartStopParameterError = 74,

        [Description("订单投注金额与订单总金额不一致")]
        BettingMoneyOrderMoneyNoEqual = 75,

        [Description("订单投注信息包含非法的注数或倍数")]
        BettingInfoIllegitmacy = 76,

        [Description("订单提交成功，待充值后付款")]
        OrderSuccessObligations = 77,

        [Description("手机唯一标识码为空")]
        MobileUniqueCodeNull = 78,

        [Description("未绑定QQ账号")]
        NotBindQQ = 79,

        [Description("未绑定微信账号")]
        NotBindWechat = 101,

        [Description("绑定失败")]
        BindFailure = 80,

        [Description("未付款订单已结期")]
        NoPayOrderEndIssue = 81,

        [Description("没有未付款订单")]
        NotNoPayOrder = 82,

        [Description("用户与订单号不匹配")]
        UserOrderNoNotMatching = 83,

        [Description("交易中，请稍后")]
        WaitPayment = 84,

        [Description("游客")]
        Sightseer = 85,

        [Description("会员")]
        User = 86,

        [Description("提现金额不足")]
        WithdrawAmount = 87,

        [Description("该彩种已停售")]
        LotStop = 88,

        [Description("手机号码有误")]
        MobileFailure = 89,

        [Description("预售中")]
        IsPresell = 90,

        [Description("本期已封盘")]
        Entertained = 91,

        [Description("当前手机号码未绑定任何账号")]
        NoMobile = 92,

        [Description("该银行卡不属于")]
        NoBankCard = 93,

        [Description("当日提现次数已达上线")]
        WithdrawCount = 94,

        [Description("保存推送标识失败")]
        SavePushFailure = 95,

        [Description("已结期")]
        EndIsuseName = 96,

        [Description("退出登录失败")]
        OutLoginFailure = 97,

        [Description("兑换码错误")]
        CDKeyFailure = 98,

        [Description("兑换码已被使用，兑换失败")]
        ExchangerAbate = 99,

        [Description("兑换码已过期，兑换失败")]
        ExchangerExpire = 102,

        [Description("IM登录失败")]
        IMLoginFailure = 100,

        [Description("彩豆不足")]
        BeanDeficiency = 200,

        [Description("暂无可玩游戏")]
        NotGame = 300,

        [Description("身份验证参数不存在")]
        AuthParamterNonexistence = 992,

        [Description("时间戳参数不存在")]
        TimeStampParamterNonexistence = 993,

        [Description("时间戳参数格式不正确")]
        TimeStampFormatError = 994,

        [Description("时间戳错误")]
        TimeStampError = 991,

        [Description("签名错误")]
        SecretError = 990,

        [Description("请求已失效")]
        RequestHasFailed = 995,

        [Description("非法操作")]
        IllegalOperation = 996,

        [Description("系统繁忙")]
        SystemBusy = 997,

        [Description("接口异常")]
        InterfaceException = 998,

        [Description("系统错误")]
        Error = 999,

        [Description("令牌无效")]
        TokenError = 1000,

        [Description("不满足最小金额")]
        MinAmount = 8001,

        [Description("超出最大限制金额")]
        MaxAmount = 8002
    }
}
