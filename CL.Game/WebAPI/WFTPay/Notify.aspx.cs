using CL.Game.BLL;
using CL.Tools.Common;
using CL.View.Entity.Other;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace WebAPI.WFTPay
{
    public partial class Notify : System.Web.UI.Page
    {
        private Log log = new Log("WFTPayNotify");
        private WFTPayHelper resHandler = new WFTPayHelper();
        private Dictionary<string, string> cfg = new Dictionary<string, string>(1);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestType == "POST")
            {
                CallBack();
            }
        }
        public void CallBack()
        {
            try
            {
                //加载配置数据
                this.cfg = WFTPayHelper.loadCfg();
                //初始化数据
                using (StreamReader sr = new StreamReader(Request.InputStream))
                {
                    string ReadEnd = sr.ReadToEnd().Trim().Replace(" ", "");
                    log.Write("威富通支付服务端回调数据：" + ReadEnd);
                    this.resHandler.setContent(ReadEnd);
                    this.resHandler.setKey(this.cfg["key"]);

                    Hashtable resParam = this.resHandler.getAllParameters();
                    if (this.resHandler.isTenpaySign())
                    {
                        udv_WFTPayNotify entityWFT = new udv_WFTPayNotify();

                        if (int.Parse(resParam["status"].ToString()) == 0 && int.Parse(resParam["result_code"].ToString()) == 0)
                        {
                            bool ResultHelper = false;
                            #region 开始处理程序
                            
                            #region 对象赋值
                            entityWFT.version = resParam["version"].ToString();
                            entityWFT.charset = resParam["charset"].ToString();
                            entityWFT.sign_type = resParam["sign_type"].ToString();
                            entityWFT.status = resParam["status"].ToString();
                            //entityWFT.message = resParam["message"].ToString();

                            #region 以下字段在 status 为 0 的时候有返回
                            entityWFT.result_code = resParam["result_code"].ToString();
                            entityWFT.mch_id = resParam["mch_id"].ToString();
                            //entityWFT.device_info = resParam["device_info"].ToString();
                            entityWFT.nonce_str = resParam["nonce_str"].ToString();
                            //entityWFT.err_code = resParam["err_code"].ToString();
                            //entityWFT.err_msg = resParam["err_msg"].ToString();
                            entityWFT.sign = resParam["sign"].ToString();
                            #endregion

                            #region 以下字段在 status 和 result_code 都为 0 的时候有返回
                            entityWFT.openid = resParam["openid"].ToString();
                            entityWFT.trade_type = resParam["trade_type"].ToString();
                            entityWFT.pay_result = Convert.ToInt32(resParam["pay_result"].ToString());
                            //entityWFT.pay_info = resParam["pay_info"].ToString();
                            entityWFT.transaction_id = resParam["transaction_id"].ToString();
                            entityWFT.out_transaction_id = resParam["out_transaction_id"].ToString();
                            entityWFT.out_trade_no = resParam["out_trade_no"].ToString();
                            entityWFT.total_fee = Convert.ToInt32(resParam["total_fee"].ToString());
                            entityWFT.fee_type = resParam["fee_type"].ToString();
                            entityWFT.attach = resParam["attach"].ToString();
                            entityWFT.bank_type = resParam["bank_type"].ToString();
                            entityWFT.time_end = resParam["time_end"].ToString();
                            #endregion
                            #endregion
                            if (resHandler.isVerfiySign().Equals(entityWFT.sign.ToLower()) && entityWFT.pay_result.Equals(0))
                                ResultHelper = new UsersRecordBLL().PayInfo(entityWFT.out_trade_no, entityWFT.transaction_id, entityWFT.bank_type, entityWFT.total_fee, entityWFT.out_transaction_id);
                            #endregion
                            if (ResultHelper)
                            {
                                //此处可以在添加相关处理业务，校验通知参数中的商户订单号out_trade_no和金额total_fee是否和商户业务系统的单号和金额是否一致，一致后方可更新数据库表中的记录。  
                                Response.Write("success");
                            }
                            else
                            {
                                Response.Write("failure");
                            }
                        }
                        else
                        {
                            Response.Write("failure1");
                        }
                    }
                    else
                    {
                        Response.Write("failure2");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Write("威富通支付服务端回调错误：" + ex.Message + "\r\n" + ex.StackTrace, true);
                Response.Write("failure3");
            }
        }
    }
}