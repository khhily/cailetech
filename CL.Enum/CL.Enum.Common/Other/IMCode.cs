using System.ComponentModel;

namespace CL.Enum.Common.Other
{
    public enum IMCode
    {

        [Description("操作成功")]
        Success = 200,

        [Description("客户端版本不对，需升级sdk")]
        UploadSDK = 201,

        [Description("被封禁")]
        BeBanned = 301,

        [Description("用户名或密码错误")]
        UserPwdError = 302,

        [Description("IP限制")]
        IPLimit = 315,

        [Description("非法操作或没有权限")]
        NoPermission = 403,

        [Description("对象不存在")]
        ObjectIsNull = 404,

        [Description("参数长度过长")]
        ParameterThroughTheLong = 405,

        [Description("对象只读")]
        ObjectIsReadOnly = 406,

        [Description("客户端请求超时")]
        HttpReqseu = 408,

        [Description("验证失败(短信服务)")]
        VerifyFailure = 413,

        [Description("参数错误")]
        ParameterError = 414,

        [Description("客户端网络问题")]
        ClientNetworkProblems = 415,

        [Description("频率控制")]
        FrequencyControl = 416,

        [Description("重复操作")]
        RepetitiveOperation = 417,

        [Description("通道不可用(短信服务)")]
        ChannelIsNotAvailable = 418,

        [Description("数量超过上限")]
        NumberTheCap = 419,

        [Description("账号被禁用")]
        AccountIsDisabled = 422,

        [Description("HTTP重复请求")]
        RepeatRequest = 431,

        [Description("服务器内部错误")]
        ServerError = 500,

        [Description("服务器繁忙")]
        ServerBusy = 503,

        [Description("服务不可用")]
        ServerUnavailable = 514,

        [Description("无效协议")]
        InvalidAgreement = 509,

        [Description("解包错误")]
        UnpackError = 998,

        [Description("打包错误")]
        PackagingError = 999,

        [Description("群人数达到上限")]
        CrowdMaxMember = 801,

        [Description("没有权限")]
        PermissionDenied = 802,

        [Description("群不存在")]
        CrowdNonentity = 803,

        //[Description("用户不在群")]
        //Success = 804,

        //[Description("群类型不匹配")]
        //Success = 805,

        //[Description("创建群数量达到限制")]
        //Success = 806,

        //[Description("群成员状态错误")]
        //Success = 807,

        //[Description("申请成功")]
        //Success = 808,

        //[Description("已经在群内")]
        //Success = 809,

        //[Description("邀请成功")]
        //Success = 810,

        //[Description("通道失效")]
        //Success = 9102,

        //[Description("已经在他端对这个呼叫响应过了")]
        //Success = 9103,

        //[Description("通话不可达，对方离线状态")]
        //Success = 11001,

        //[Description("IM主连接状态异常")]
        //Success = 13001,

        //[Description("聊天室状态异常")]
        //Success = 13002,

        //[Description("账号在黑名单中,不允许进入聊天室")]
        //Success = 13003,

        //[Description("在禁言列表中,不允许发言")]
        //Success = 13004,

        //[Description("输入email不是邮箱")]
        //Success = 10431,

        //[Description("输入mobile不是手机号码")]
        //Success = 10432,

        //[Description("注册输入的两次密码不相同")]
        //Success = 10433,

        //[Description("企业不存在")]
        //Success = 10434,

        //[Description("登陆密码或帐号不对")]
        //Success = 10435,

        //[Description("app不存在")]
        //Success = 10436,

        //[Description("email已注册")]
        //Success = 10437,

        //[Description("手机号已注册")]
        //Success = 10438,

        //[Description("app名字已经存在")]
        //Success = 10441

    }
}
