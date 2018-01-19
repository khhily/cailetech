using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Other
{
    public class udv_WFTPayNotify
    {
        /// <summary>
        /// 版本号
        /// 
        /// 版本号，version 默认值是 2.0。
        /// </summary>
        public string version { set; get; }
        /// <summary>
        /// 字符集
        /// 
        /// 可选值 UTF-8 ，默认为 UTF-8。
        /// </summary>
        public string charset { set; get; }
        /// <summary>
        /// 签名方式
        /// 
        /// 签名类型，取值：MD5 默认：MD5
        /// </summary>
        public string sign_type { set; get; }
        /// <summary>
        /// 返回状态码
        /// 
        /// 0 表示成功非 0 表示失败
        /// 此字段是通信标识，非交易
        /// 标识，交易是否成功需要查
        /// 看 result_code 来判断
        /// </summary>
        public string status { set; get; }
        /// <summary>
        /// 返回信息
        /// 
        /// 返回信息，如非空，为错误原因签名失败参数
        /// 格式校验错误
        /// </summary>
        public string message { set; get; }

        #region 以下字段在 status 为 0 的时候有返回
        /// <summary>
        /// 业务结果
        /// 
        /// 0 表示成功非 0 表示失败
        /// </summary>
        public string result_code { set; get; }
        /// <summary>
        /// 商户号
        /// 
        /// 商户号，由威富通分配
        /// </summary>
        public string mch_id { set; get; }
        /// <summary>
        /// 设备号
        /// 
        /// 终端设备号
        /// </summary>
        public string device_info { set; get; }
        /// <summary>
        /// 随机字符串
        /// 
        /// 随机字符串，不长于 32 位
        /// </summary>
        public string nonce_str { set; get; }
        /// <summary>
        /// 错误代码
        /// 
        /// 参考错误码
        /// </summary>
        public string err_code { set; get; }
        /// <summary>
        /// 错误代码描述
        /// 
        /// 结果信息描述
        /// </summary>
        public string err_msg { set; get; }
        /// <summary>
        /// 签名
        /// 
        /// MD5 签名结果，详见“第 4 章 MD5 签名规则”
        /// </summary>
        public string sign { set; get; }
        #endregion

        #region 以下字段在 status 和 result_code 都为 0 的时候有返回
        /// <summary>
        /// 用户标识
        /// 
        /// 用户在服务商 appid 下的唯一标识
        /// </summary>
        public string openid { set; get; }
        /// <summary>
        /// 交易类型
        /// 
        /// pay.weixin.app
        /// </summary>
        public string trade_type { set; get; }
        /// <summary>
        /// 支付结果
        /// 
        /// 支付结果：0—成功；其它—失败
        /// </summary>
        public int pay_result { set; get; }
        /// <summary>
        /// 支付结果信息
        /// 
        /// 支付结果信息，支付成功时为空
        /// </summary>
        public string pay_info { set; get; }
        /// <summary>
        /// 威富通订单号
        /// 
        /// 威富通交易号
        /// </summary>
        public string transaction_id { set; get; }
        /// <summary>
        /// 第三方支付单
        /// 
        /// 如：微信支付单号，支付宝支付单号
        /// </summary>
        public string out_transaction_id { set; get; }
        /// <summary>
        /// 商户订单号
        /// 
        /// 商户系统内部的定单号，32 个字符内、可包含字母
        /// </summary>
        public string out_trade_no { set; get; }
        /// <summary>
        /// 总金额
        /// 
        /// 总金额，以分为单位，不允许包含任何字、符号
        /// </summary>
        public int total_fee { set; get; }
        /// <summary>
        /// 货币种类
        /// 
        /// 货币类型，符合 ISO 4217 标准的三位字母代码，默认人民币：CNY
        /// </summary>
        public string fee_type { set; get; }
        /// <summary>
        /// 附加信息
        /// 
        /// 商家数据包，原样返回预下单时自定义数据
        /// </summary>
        public string attach { set; get; }
        /// <summary>
        /// 付款银行
        /// 
        /// 银行类型
        /// </summary>
        public string bank_type { set; get; }
        /// <summary>
        /// 支付完成时间
        /// 
        /// 支付完成时间，格式为 yyyyMMddHHmmss，如
        /// 2009 年 12 月 27 日 9 点 10 分 10 秒表示为
        /// 20091227091010。时区为 GMT+8 beijing。该
        /// 时间取自威富通服务器
        /// </summary>
        public string time_end { set; get; }
        #endregion
    }
}
