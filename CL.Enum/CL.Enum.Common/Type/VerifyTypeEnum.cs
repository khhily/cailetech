using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Type
{
    /// <summary>
    /// 短信类型：
    /// 0.注册 1.找回密码 3.修改登录密码 5.修改支付密码 7.绑定修改手机号码 9.绑定银行卡 11.身份验证
    /// </summary>
    public enum VerifyTypeEnum
    {
        [Description("注册")]
        Register = 0,

        [Description("找回密码")]
        RetrievePwd = 1,

        [Description("修改登录密码")]
        UpdateLoginPwd = 3,

        [Description("修改支付密码")]
        UpdatePaymentPwd = 5,

        [Description("绑定修改手机号码")]
        UpdateMobile = 7,

        [Description("绑定银行卡")]
        BandBank = 9,

        [Description("身份验证")]
        Identity = 11

    }
}
