using CL.Entity.Json.Pay;
using CL.Enum.Common;
using CL.Game.BLL.View;
using CL.Game.Entity;
using CL.Json.Entity.WebAPI;
using CL.Redis.BLL;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CL.Game.BLL.PayInterface
{
    public class WFTPay
    {
        private Log log = new Log("WFTPay");
        WFTPayHelper resHandler = new WFTPayHelper();
        WFTPayHttpClient pay = new WFTPayHttpClient();
        WFTPayRequestHandler reqHandler = null;
        private Dictionary<string, string> cfg = new Dictionary<string, string>(1);
        public WFTPay()
        {

        }
        /// <summary>
        /// 提交支付信息
        /// </summary>
        /// <param name="userid">用户标识</param>
        /// <param name="amount">金额</param>
        /// <param name="IpAddress">本机IP</param>
        /// <param name="tbbody">商品描述</param>
        /// <param name="tbattach">附加信息</param>
        /// <returns>UserResult</returns>
        public PreparePaymentResult PostPayInfo(long userid, long amount, string IpAddress, string tbbody, string tbattach)
        {
            PreparePaymentResult result = null;
            try
            {
                if (!IsIP(IpAddress))
                    IpAddress = "127.0.0.1";

                string tbout_trade_no = GetNotbout_trade_no();
                this.reqHandler = new WFTPayRequestHandler(null);
                //加载配置数据
                this.cfg = WFTPayHelper.loadCfg();
                //初始化数据  
                this.reqHandler.setGateUrl(this.cfg["req_url"].ToString());
                this.reqHandler.setKey(this.cfg["key"].ToString());
                //商户订单号
                this.reqHandler.setParameter("out_trade_no", tbout_trade_no);
                //商品描述
                this.reqHandler.setParameter("body", tbbody);
                //附加信息
                this.reqHandler.setParameter("attach", tbattach);
                //总金额
                this.reqHandler.setParameter("total_fee", amount.ToString());
                //终端IP 
                this.reqHandler.setParameter("mch_create_ip", IpAddress);
                //接口类型：
                this.reqHandler.setParameter("service", "unified.trade.pay");
                //必填项，商户号，由威富通分配
                this.reqHandler.setParameter("mch_id", this.cfg["mch_id"].ToString());
                //接口版本号
                this.reqHandler.setParameter("version", this.cfg["version"].ToString());
                //通知地址，必填项，接收威富通通知的URL，需给绝对路径，255字符内;此URL要保证外网能访问
                this.reqHandler.setParameter("notify_url", this.cfg["notify_url"].ToString());
                //随机字符串，必填项，不长于 32 位
                this.reqHandler.setParameter("nonce_str", WFTPayHelper.random());
                //字符集
                this.reqHandler.setParameter("charset", "UTF-8");
                //签名方式
                this.reqHandler.setParameter("sign_type", "MD5");
                //创建签名
                this.reqHandler.createSign();
                //以上参数进行签名
                string data = WFTPayHelper.toXml(this.reqHandler.getAllParameters());//生成XML报文
                Dictionary<string, string> reqContent = new Dictionary<string, string>();
                reqContent.Add("url", this.reqHandler.getGateUrl());
                reqContent.Add("data", data);
                this.pay.setReqContent(reqContent);
                if (this.pay.call())
                {
                    this.resHandler.setContent(this.pay.getResContent());
                    this.resHandler.setKey(this.cfg["key"].ToString());
                    Hashtable param = this.resHandler.getAllParameters();
                    if (this.resHandler.isTenpaySign())
                    {
                        //当返回状态与业务结果都为0时才返回，其它结果请查看接口文档
                        //if (int.Parse(param["status"].ToString()) == 0 && int.Parse(param["result_code"].ToString()) == 0)//后续result_code会返回出来。
                        if (int.Parse(param["status"].ToString()) == 0)
                        {
                            //param["token_id"].ToString()
                            //param["services"].ToString()
                            //services是返回全部的支付类型，此处仅用pay.weixin.wappay
                            UsersPayDetailEntity entity = new UsersPayDetailEntity()
                            {
                                RechargeNo = "0",
                                Amount = amount,
                                CreateTime = DateTime.Now,
                                FormalitiesFees = 0,
                                OrderNo = tbout_trade_no,
                                PayType = "0",
                                Result = 0,
                                UserID = userid,
                                OutRechargeNo = "0"
                            };

                            long entityResult = new UsersPayDetailBLL().InsertEntity(entity);
                            if (entityResult > 0)
                            {
                                //支付成功
                                result = new PreparePaymentResult()
                                {
                                    Code = (int)ResultCode.Success,
                                    Msg = Common.GetDescription(ResultCode.Success),
                                    Data = new PreparePaymentEnttiy_WFT()
                                    {
                                        TokenID = param["token_id"].ToString(),
                                        OrderNo = tbout_trade_no
                                    }
                                };
                            }
                            else
                            {
                                //支付失败
                                result = new PreparePaymentResult()
                                {
                                    Code = (int)ResultCode.PayDetailsFailure,
                                    Msg = Common.GetDescription(ResultCode.PayDetailsFailure)
                                };
                            }
                        }
                        else
                        {
                            //支付失败
                            result = new PreparePaymentResult()
                            {
                                Code = (int)ResultCode.PayDetailsFailure,
                                Msg = Common.GetDescription(ResultCode.PayDetailsFailure)
                            };
                        }
                    }
                    else
                        result = new PreparePaymentResult()
                        {
                            Code = (int)ResultCode.PayDetailsFailure,
                            Msg = Common.GetDescription(ResultCode.PayDetailsFailure)
                        };
                }
                else
                    result = new PreparePaymentResult()
                    {
                        Code = (int)ResultCode.PayDetailsFailure,
                        Msg = Common.GetDescription(ResultCode.PayDetailsFailure)
                    };
            }
            catch (Exception ex)
            {
                result = new PreparePaymentResult()
                {
                    Code = (int)ResultCode.Error,
                    Msg = ex.Message
                };
                log.Write(string.Format("提交支付信息错误[PostPayInfo](userid:{0};amount:{1};IpAddress:{2};tbbody:{3};tbattach:{4}): {5}", userid, amount, IpAddress, tbbody, tbattach, ex.Message), true);
            }
            return result;
        }
        /// <summary>
        /// 验证订单号是否已存在数据库
        /// </summary>
        /// <returns></returns>
        private string GetNotbout_trade_no()
        {
            string tbout_trade_no = WFTPayHelper.Nmrandom();
            bool result = new UsersPayDetailBLL().IsTboutTradeNo(tbout_trade_no);
            if (result)
                return tbout_trade_no;
            else
                return GetNotbout_trade_no();
        }
        /// <summary>
        /// 验证退款单号是否已存在
        /// </summary>
        /// <returns></returns>
        private string GetOut_refund_no()
        {
            string Out_refund_no = WFTPayHelper.Nmrandom();
            bool result = new UsersPayDetailBLL().IsOutRefundNo(Out_refund_no);
            if (result)
                return Out_refund_no;
            else
                return GetOut_refund_no();
        }
        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>  
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 提交退款，只做全额退款，不做部分退款
        /// </summary>
        /// <param name="PayID">支付ID</param>
        /// <param name="RefundFee">退款金额</param>
        /// <returns>UserResult</returns>
        public WFTResult PostRefund(long PayID, long RefundFee)
        {
            WFTResult result = null;
            try
            {
                UsersPayDetailEntity PayModel = new UsersPayDetailBLL().QueryPayDetailsByPayID(PayID);

                if (RefundFee > PayModel.Amount)
                {
                    return new WFTResult() { Code = 5, Msg = "退款金额大于充值金额,退款失败!" };
                }
                string out_refund_no = GetOut_refund_no();
                this.reqHandler = new WFTPayRequestHandler(null);

                #region 参数组合
                //加载配置数据
                this.cfg = WFTPayHelper.loadCfg("xmlconfig");
                //初始化数据  
                this.reqHandler.setGateUrl(this.cfg["req_url"].ToString());
                this.reqHandler.setKey(this.cfg["key"].ToString());

                //接口类型：
                this.reqHandler.setParameter("service", "unified.trade.refund");
                //接口版本号
                this.reqHandler.setParameter("version", this.cfg["version"].ToString());
                //字符集
                this.reqHandler.setParameter("charset", "UTF-8");
                //签名方式
                this.reqHandler.setParameter("sign_type", "MD5");
                //必填项，商户号，由威富通分配
                this.reqHandler.setParameter("mch_id", this.cfg["mch_id"].ToString());
                //商户订单号
                this.reqHandler.setParameter("out_trade_no", PayModel.OrderNo);
                //商户退款单号
                this.reqHandler.setParameter("out_refund_no", out_refund_no);
                //总金额
                this.reqHandler.setParameter("total_fee", PayModel.Amount.ToString());
                //退款金额
                this.reqHandler.setParameter("refund_fee", RefundFee.ToString());
                //操作员
                this.reqHandler.setParameter("op_user_id", this.cfg["mch_id"].ToString());
                //退款渠道
                this.reqHandler.setParameter("refund_channel", "ORIGINAL");
                //随机字符串，必填项，不长于 32 位
                this.reqHandler.setParameter("nonce_str", WFTPayHelper.random());
                //创建签名
                this.reqHandler.createSign();
                #endregion

                #region 提交请求
                string data = WFTPayHelper.toXml(this.reqHandler.getAllParameters());//生成XML报文
                Dictionary<string, string> reqContent = new Dictionary<string, string>();
                reqContent.Add("url", this.reqHandler.getGateUrl());
                reqContent.Add("data", data);
                this.pay.setReqContent(reqContent);
                if (this.pay.call())
                {
                    this.resHandler.setContent(this.pay.getResContent());
                    this.resHandler.setKey(this.cfg["key"].ToString());
                    Hashtable param = this.resHandler.getAllParameters();
                    if (this.resHandler.isTenpaySign())
                    {
                        //当返回状态与业务结果都为0时才返回，其它结果请查看接口文档
                        if (param["status"].ToString() == "0")
                        {
                            string refund_id = "";
                            if (param["result_code"].ToString() == "0")
                            {
                                refund_id = param["refund_id"].ToString();
                                UsersPayRefundEntity entity = new UsersPayRefundEntity()
                                {
                                    PayID = PayID,
                                    RefundNo = out_refund_no,
                                    RechargeNo = refund_id,
                                    Amount = PayModel.Amount,
                                    FormalitiesFees = 0,
                                    Result = 0,
                                    CreateTime = DateTime.Now
                                };
                                long entityResult = new UsersPayRefundBLL().InsertEntity(entity);
                                if (entityResult > 0)
                                {
                                    //result = new WFTResult() { Code = 0, Msg = "提交退款成功,退款处理中", TokenId = refund_id };
                                    //查询退款是否到帐
                                    result = PostRefundQuery(entityResult);
                                }
                                else
                                {
                                    result = new WFTResult() { Code = 1, Msg = "提交退款入库失败" };
                                }
                            }
                            else    //重复提交的不处理
                            {
                                //string message = param["err_msg"].ToString();
                                result = new WFTResult() { Code = 2, Msg = "不能重复提交退款申请" };
                            }
                        }
                        else
                        {
                            string message = param["err_msg"].ToString();
                            result = new WFTResult() { Code = 2, Msg = message };
                        }
                    }
                    else
                        result = new WFTResult() { Code = 3, Msg = "签名根式错误" };
                }
                else
                    result = new WFTResult() { Code = 4, Msg = "提交退款请求失败" };
                #endregion
            }
            catch (Exception ex)
            {
                result = new WFTResult() { Code = (int)ResultCode.Error, Msg = ex.Message };
                log.Write(string.Format("提交退款信息错误[PostRefund](PayID:{0};RefundFee:{1}): {2}", PayID, RefundFee, ex.Message), true);
            }
            return result;
        }

        /// <summary>
        /// 查询退款
        /// </summary>
        /// <param name="ReID"></param>
        /// <returns></returns>
        public WFTResult PostRefundQuery(long ReID)
        {
            WFTResult result = null;
            try
            {
                udv_UserPayReRefund PayRefundModel = new udv_UserPayReRefundBLL().GetPayReRefund(ReID);
                this.reqHandler = new WFTPayRequestHandler(null);

                #region 参数组合
                //加载配置数据
                this.cfg = WFTPayHelper.loadCfg("xmlconfig");
                //初始化数据  
                this.reqHandler.setGateUrl(this.cfg["req_url"].ToString());
                this.reqHandler.setKey(this.cfg["key"].ToString());

                //接口类型：
                this.reqHandler.setParameter("service", "unified.trade.refundquery");
                //接口版本号
                this.reqHandler.setParameter("version", this.cfg["version"].ToString());
                //字符集
                this.reqHandler.setParameter("charset", "UTF-8");
                //签名方式
                this.reqHandler.setParameter("sign_type", "MD5");
                //必填项，商户号，由威富通分配
                this.reqHandler.setParameter("mch_id", this.cfg["mch_id"].ToString());
                //商户订单号
                this.reqHandler.setParameter("out_trade_no", PayRefundModel.OrderNo);
                //商户退款单号
                this.reqHandler.setParameter("out_refund_no", PayRefundModel.RefundNo);
                //随机字符串，必填项，不长于 32 位
                this.reqHandler.setParameter("nonce_str", WFTPayHelper.random());
                //创建签名
                this.reqHandler.createSign();
                #endregion

                #region 提交请求
                string data = WFTPayHelper.toXml(this.reqHandler.getAllParameters());//生成XML报文
                Dictionary<string, string> reqContent = new Dictionary<string, string>();
                reqContent.Add("url", this.reqHandler.getGateUrl());
                reqContent.Add("data", data);
                this.pay.setReqContent(reqContent);
                if (this.pay.call())
                {
                    this.resHandler.setContent(this.pay.getResContent());
                    this.resHandler.setKey(this.cfg["key"].ToString());
                    Hashtable param = this.resHandler.getAllParameters();
                    if (this.resHandler.isTenpaySign())
                    {
                        //当返回状态与业务结果都为0时才返回，其它结果请查看接口文档
                        if (param["status"].ToString() == "0")
                        {
                            //int refund_count = 0;
                            short sResult = 0;
                            if (param["result_code"].ToString() == "0")
                            {
                                //refund_count = Convert.ToInt32(param["refund_count"]);  //退款记录数
                                //退款状态：
                                //SUCCESS—退款成功
                                //FAIL—退款失败
                                //PROCESSING—退款处理中
                                //NOTSURE—未确定， 需要商户原退款单号重新发起
                                //CHANGE—转入代发，退款到银行发现用户的卡作废或者冻结了，导致原路退款银行卡失败，资金回流到商户的现金帐号，需要商户人工干预，通过线下或者威富通转账的方式进行退款。
                                switch (param["refund_status_0"].ToString())
                                {
                                    case "SUCCESS":
                                        sResult = 1;
                                        break;
                                    case "FAIL":
                                        sResult = 2;
                                        break;
                                    case "PROCESSING":
                                        sResult = 0;
                                        break;
                                    case "NOTSURE":
                                        sResult = 3;
                                        break;
                                    case "CHANGE":
                                        sResult = 4;
                                        break;
                                }
                            }
                            if (sResult > 0)
                            {
                                if (new UsersPayRefundBLL().ModifyPayRefund(ReID, sResult))
                                {
                                    result = new WFTResult() { Code = 0, Msg = "退款成功" };
                                }
                                else
                                {
                                    result = new WFTResult() { Code = 1, Msg = "更新退款状态失败" };
                                }
                            }
                            else
                            {
                                result = new WFTResult() { Code = 0, Msg = "提交退款成功,退款处理中" };
                            }
                        }
                        else
                        {
                            string message = param["err_msg"].ToString();
                            result = new WFTResult() { Code = 2, Msg = message };
                        }
                    }
                    else
                        result = new WFTResult() { Code = 3, Msg = "签名根式错误" };
                }
                else
                    result = new WFTResult() { Code = 4, Msg = "提交查询退款请求失败" };
                #endregion
            }
            catch (Exception ex)
            {
                result = new WFTResult() { Code = (int)ResultCode.Error, Msg = ex.Message };
                log.Write(string.Format("查询退款信息错误[PostRefund](ReID:{0}): {1}", ReID, ex.Message), true);
            }
            return result;
        }

        #region 自定义方法
        /// <summary>
        /// 威富通预支付
        /// 接口调用
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Amount"></param>
        /// <param name="IpAddress"></param>
        /// <param name="tbBody"></param>
        /// <param name="tbAttach"></param>
        /// <returns></returns>
        public PreparePaymentResult WFTPrepayment(long UserCode, long Amount, string tbBody, string tbAttach)
        {
            PreparePaymentResult result = null;
            try
            {
                if (Amount <= 0) //验证充值金额
                    result = new PreparePaymentResult()
                    {
                        Code = (int)ResultCode.PayAmountError,
                        Msg = Common.GetDescription(ResultCode.PayAmountError)
                    };
                else
                    result = this.PostPayInfo(UserCode, Amount, Utils.RequestIP, tbBody, tbAttach);
            }
            catch (Exception ex)
            {
                log.Write("威富通预支付异常：" + Utils.ReplaceStr(ex.Message) + "\r\n" + ex.StackTrace, true);
                result = new PreparePaymentResult()
                {
                    Code = (int)ResultCode.InterfaceException,
                    Msg = string.Format("{0}{1}", Common.GetDescription(ResultCode.InterfaceException), Utils.ReplaceStr(ex.Message))
                };
            }
            return result;
        }
        #endregion
    }
}
