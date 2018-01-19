using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Entity.Json.Omission;
using CL.Enum.Common;
using CL.Enum.Common.Lottery;
using CL.Game.Entity;
using CL.Tools.Common;
using CL.Game.BLL;

namespace CL.Tools.LotteryTrendChart.SSC
{
    /// <summary>
    /// 时时彩基础遗漏数据
    /// Joan
    /// </summary>
    public class LotOmission : LotBase
    {
        #region 分隔符
        private const char SEP_NUM_A = ' ';
        #endregion
        #region 公共
        protected int LotCode = 0;
        protected static string IssueName = string.Empty;
        Log log = new Log("LotOmission_SSC");
        #endregion

        public LotOmission(int Code)
        {
            LotCode = Code;
        }
        /// <summary>
        /// 实现基础遗漏
        /// </summary>
        /// <param name="PlayCode"></param>
        /// <param name="TopNumber"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public override string Omission(string IsuseName, int PlayCode, int TopCount)
        {
            OmissionResult JsonRec = new OmissionResult();
            try
            {
                IssueName = IsuseName;
                List<IsusesEntity> IsuseEntitys = new IsusesBLL().QueryLotTrendChart(LotCode, TopCount, IsuseName).OrderBy(o => o.IsuseName).ToList();
                string Result = string.Empty;
                switch (PlayCode)
                {
                    case (int)CQSSCPlayCode.DXDS:  //标准
                        Result = this.DXDS_Omission(IsuseEntitys);
                        break;
                    case (int)CQSSCPlayCode.ZX_1X:  //一星
                        Result = this.YX_Omission(IsuseEntitys);
                        break;
                    case (int)CQSSCPlayCode.ZX_2X:  //二星直选
                        Result = this.EXZHIX_Omission(IsuseEntitys);
                        break;
                    case (int)CQSSCPlayCode.ZU_2X:  //二星直选
                        Result = this.EXZUX_Omission(IsuseEntitys);
                        break;
                    case (int)CQSSCPlayCode.ZX_3X:  //三星直选
                        Result = this.SXZHIX_Omission(IsuseEntitys);
                        break;
                    case (int)CQSSCPlayCode.ZU3_3X:  //三星组选 组三
                    case (int)CQSSCPlayCode.ZU6_3X:  //三星组选 组六
                        Result = this.SXZUX_Omission(IsuseEntitys);
                        break;
                    case (int)CQSSCPlayCode.ZX_5X:  //五星直选
                    case (int)CQSSCPlayCode.TX_5X:  //五星通选
                        Result = this.WX_Omission(IsuseEntitys);
                        break;
                    default:
                        break;
                }
                JsonRec = new OmissionResult()
                {
                    Code = (int)ResultCode.Success,
                    Msg = Enum.Common.Common.GetDescription(ResultCode.Success),
                    Data = Result
                };
            }
            catch (Exception ex)
            {
                log.Write(string.Format("实现基础遗漏错误[Omission]：", ex.StackTrace), true);
                JsonRec = new OmissionResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Enum.Common.Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(JsonRec);
        }
        #region 不同玩法详细处理
        /// <summary>
        /// 大小单双基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string DXDS_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                //大：5-9；小：0-4；单：13579；双：02468；
                int[] S_OmissionNumbers = new int[] { 0, 0, 0, 0 }; //十位 w位置：大小单双
                int[] G_OmissionNumbers = new int[] { 0, 0, 0, 0 }; //个位
                List<int[]> Omissions = new List<int[]>();
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    S_OmissionNumbers = S_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    G_OmissionNumbers = G_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    int S_Numbers = Numbers[Numbers.Length - 2]; //十位
                    //十位计算
                    if (S_Numbers <= 4) //小
                        S_OmissionNumbers[1] = 0;
                    else //大
                        S_OmissionNumbers[0] = 0;
                    if ((S_Numbers % 2) > 0)
                        S_OmissionNumbers[2] = 0;
                    else
                        S_OmissionNumbers[3] = 0;

                    //个位计算
                    int G_Numbers = Numbers[Numbers.Length - 1]; //个位
                    if (G_Numbers <= 4) //小
                        G_OmissionNumbers[1] = 0;
                    else //大
                        G_OmissionNumbers[0] = 0;
                    if ((G_Numbers % 2) > 0)
                        G_OmissionNumbers[2] = 0;
                    else
                        G_OmissionNumbers[3] = 0;
                });
                Omissions.Add(S_OmissionNumbers);
                Omissions.Add(G_OmissionNumbers);
                var obj = new
                {
                    i = IssueName,
                    o = Omissions
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[大小单双]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 一星基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string YX_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                //开奖号码最后一位
                int[] OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //一星
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    OmissionNumbers = OmissionNumbers.Select(s => s = s + 1).ToArray();
                    int YX_Number = Numbers[Numbers.Length - 1];
                    OmissionNumbers[YX_Number] = 0;
                });
                var obj = new
                {
                    i = IssueName,
                    o = OmissionNumbers
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[一星]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 二星组选基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string EXZUX_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                //开奖号码最后一位
                int[] OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //二星
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    OmissionNumbers = OmissionNumbers.Select(s => s = s + 1).ToArray();
                    for (int i = 3; i < 5; i++)
                        OmissionNumbers[Numbers[i]] = 0;
                });
                var obj = new
                {
                    i = IssueName,
                    o = OmissionNumbers
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[二星组选]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 二星直选基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string EXZHIX_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                int[] S_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //十位
                int[] G_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //个位
                List<int[]> Omissions = new List<int[]>();
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    S_OmissionNumbers = S_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    G_OmissionNumbers = G_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    S_OmissionNumbers[Numbers[Numbers.Length - 2]] = 0;//十位
                    G_OmissionNumbers[Numbers[Numbers.Length - 1]] = 0;//个位
                });
                Omissions.Add(S_OmissionNumbers);
                Omissions.Add(G_OmissionNumbers);
                var obj = new
                {
                    i = IssueName,
                    o = Omissions
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[二星直选]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 三星组选基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string SXZUX_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                //开奖号码最后一位
                int[] OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //二星
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    OmissionNumbers = OmissionNumbers.Select(s => s = s + 1).ToArray();
                    for (int i = 2; i < 5; i++)
                        OmissionNumbers[Numbers[i]] = 0;
                });
                var obj = new
                {
                    i = IssueName,
                    o = OmissionNumbers
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[三星组选]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 三星直选基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string SXZHIX_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                int[] B_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //十位
                int[] S_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //十位
                int[] G_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //个位
                List<int[]> Omissions = new List<int[]>();
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    B_OmissionNumbers = B_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    S_OmissionNumbers = S_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    G_OmissionNumbers = G_OmissionNumbers.Select(s => s = s + 1).ToArray();

                    B_OmissionNumbers[Numbers[Numbers.Length - 3]] = 0;//百位
                    S_OmissionNumbers[Numbers[Numbers.Length - 2]] = 0;//十位
                    G_OmissionNumbers[Numbers[Numbers.Length - 1]] = 0;//个位
                });
                Omissions.Add(B_OmissionNumbers);
                Omissions.Add(S_OmissionNumbers);
                Omissions.Add(G_OmissionNumbers);
                var obj = new
                {
                    i = IssueName,
                    o = Omissions
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[三星直选]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 五星基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string WX_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                int[] W_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //万位
                int[] Q_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //千位
                int[] B_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //白位
                int[] S_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //十位
                int[] G_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //个位
                List<int[]> Omissions = new List<int[]>();
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    W_OmissionNumbers = W_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    Q_OmissionNumbers = Q_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    B_OmissionNumbers = B_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    S_OmissionNumbers = S_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    G_OmissionNumbers = G_OmissionNumbers.Select(s => s = s + 1).ToArray();

                    W_OmissionNumbers[Numbers[0]] = 0;//万位
                    Q_OmissionNumbers[Numbers[1]] = 0;//千位
                    B_OmissionNumbers[Numbers[2]] = 0;//百位
                    S_OmissionNumbers[Numbers[3]] = 0;//十位
                    G_OmissionNumbers[Numbers[4]] = 0;//个位
                });
                Omissions.Add(W_OmissionNumbers);
                Omissions.Add(Q_OmissionNumbers);
                Omissions.Add(B_OmissionNumbers);
                Omissions.Add(S_OmissionNumbers);
                Omissions.Add(G_OmissionNumbers);
                var obj = new
                {
                    i = IssueName,
                    o = Omissions
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[五星]：{0}", ex.Message));
            }
        }
        #endregion

        #region 基本走势图
        /// <summary>
        /// 基础走势图
        /// </summary>
        /// <param name="IsuseName"></param>
        /// <param name="PlayCode"></param>
        /// <param name="TopNumber"></param>
        /// <returns></returns>
        public override string Basics_TrendChart(string IsuseName, int PlayCode, int TopCount)
        {
            OmissionResult JsonRec = new OmissionResult();
            try
            {
                IssueName = IsuseName;
                List<IsusesEntity> IsuseEntitys = new IsusesBLL().QueryLotTrendChart(LotCode, TopCount, IsuseName).OrderBy(o => o.IsuseName).ToList();
                string Result = string.Empty;
                switch (PlayCode)
                {
                    case (int)CQSSCPlayCode.ZX_1X:  //一星
                        Result = this.TrendChart_YIXING(IsuseEntitys, TopCount);
                        break;
                    default:
                        break;
                }
                JsonRec = new OmissionResult()
                {
                    Code = (int)ResultCode.Success,
                    Msg = Enum.Common.Common.GetDescription(ResultCode.Success),
                    Data = Result
                };
            }
            catch (Exception ex)
            {
                log.Write(string.Format("实现基础遗漏错误[Omission]：", ex.StackTrace), true);
                JsonRec = new OmissionResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Enum.Common.Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(JsonRec);
        }
        #region 不同玩法详细处理
        /// <summary>
        /// 一星基础走势图
        /// </summary>
        /// <param name="Entitys"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public string TrendChart_YIXING(List<IsusesEntity> Entitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //形态走势(奇，偶，质，合，0路，1路，2路)
                int[] xts_trend = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //形态遗漏
                int[] xts_omission = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //形态平均遗漏
                int[] xts_average = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //形态最大遗漏
                int[] xts_max = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] xts_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //形态最大连出
                int[] xts_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] xts_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };

                //个位走势
                int[] g_trend = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //个位遗漏
                int[] g_omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //个位平均遗漏
                int[] g_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //个位最大遗漏
                int[] g_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] g_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //个位最大连出
                int[] g_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] g_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //合 4,6,8,9,10
                int[] h_ectype = new int[] { 0, 4, 6, 8, 9, 10 };
                //上一次开奖号码
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0 };

                Entitys.ForEach((Entity) =>
                {
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //一星
                    int yx_num = numbers[numbers.Length - 1];
                    //和值
                    int sum = numbers.Sum(s => s);
                    //跨度
                    int max = numbers.OrderByDescending(o => o).FirstOrDefault();
                    int min = numbers.OrderBy(o => o).FirstOrDefault();
                    int span = max - min;
                    //重号个数
                    int double_sign = 0;
                    if (yx_num == last_numbers[last_numbers.Length - 1])
                        double_sign++;
                    //形态 奇，偶，质，合，0路，1路，2路
                    xts_trend = xts_trend.Select(s => s = s + 1).ToArray();
                    xts_max_ext = xts_max_ext.Select(s => s = s + 1).ToArray();
                    //奇偶
                    if ((yx_num % 2) == 0)
                    {
                        //偶数
                        xts_trend[1] = 0;
                        xts_omission[1] += 1;

                        #region 一星 最大遗漏,基本最大连出
                        var xts_ext = xts_max_ext[1] - 1;
                        var xts = xts_max[1];
                        if (xts_ext > xts)
                            xts_max[1] = xts_ext;
                        xts_max_ext[1] = 0;

                        var last_item = last_numbers[last_numbers.Length - 1];
                        if (yx_num == last_item)
                        {
                            xts_max_series_ext[1] += 1;
                            if (xts_max_series_ext[1] > xts_max_series[1])
                                xts_max_series[1] = xts_max_series_ext[1];
                        }
                        else
                            xts_max_series_ext[1] = 0;
                        #endregion
                    }
                    else
                    {
                        //奇数
                        xts_trend[0] = 0;
                        xts_omission[0] += 1;

                        #region 一星 最大遗漏,基本最大连出
                        var xts_ext = xts_max_ext[0] - 1;
                        var xts = xts_max[0];
                        if (xts_ext > xts)
                            xts_max[0] = xts_ext;
                        xts_max_ext[0] = 0;

                        var last_item = last_numbers[last_numbers.Length - 1];
                        if (yx_num == last_item)
                        {
                            xts_max_series_ext[0] += 1;
                            if (xts_max_series_ext[0] > xts_max_series[0])
                                xts_max_series[0] = xts_max_series_ext[0];
                        }
                        else
                            xts_max_series_ext[0] = 0;
                        #endregion
                    }
                    //质合
                    if (h_ectype.Contains(yx_num))
                    {
                        //合数
                        xts_trend[3] = 0;
                        xts_omission[3] += 1;

                        #region 一星 最大遗漏,基本最大连出
                        var xts_ext = xts_max_ext[3] - 1;
                        var xts = xts_max[3];
                        if (xts_ext > xts)
                            xts_max[3] = xts_ext;
                        xts_max_ext[3] = 0;

                        var last_item = last_numbers[last_numbers.Length - 1];
                        if (yx_num == last_item)
                        {
                            xts_max_series_ext[3] += 1;
                            if (xts_max_series_ext[3] > xts_max_series[3])
                                xts_max_series[3] = xts_max_series_ext[3];
                        }
                        else
                            xts_max_series_ext[3] = 0;
                        #endregion
                    }
                    else
                    {
                        //质数
                        xts_trend[2] = 0;
                        xts_omission[2] += 1;

                        #region 一星 最大遗漏,基本最大连出
                        var xts_ext = xts_max_ext[2] - 1;
                        var xts = xts_max[2];
                        if (xts_ext > xts)
                            xts_max[2] = xts_ext;
                        xts_max_ext[2] = 0;

                        var last_item = last_numbers[last_numbers.Length - 1];
                        if (yx_num == last_item)
                        {
                            xts_max_series_ext[2] += 1;
                            if (xts_max_series_ext[2] > xts_max_series[2])
                                xts_max_series[2] = xts_max_series_ext[2];
                        }
                        else
                            xts_max_series_ext[2] = 0;
                        #endregion
                    }
                    //0 1 2路
                    switch ((yx_num % 3))
                    {
                        case 0:
                            xts_trend[4] = 0;
                            xts_omission[4] += 1;

                            #region 一星 最大遗漏,基本最大连出
                            var xts_ext = xts_max_ext[4] - 1;
                            var xts = xts_max[4];
                            if (xts_ext > xts)
                                xts_max[4] = xts_ext;
                            xts_max_ext[4] = 0;

                            var last_item = last_numbers[last_numbers.Length - 1];
                            if (yx_num == last_item)
                            {
                                xts_max_series_ext[4] += 1;
                                if (xts_max_series_ext[4] > xts_max_series[4])
                                    xts_max_series[4] = xts_max_series_ext[4];
                            }
                            else
                                xts_max_series_ext[4] = 0;
                            #endregion
                            break;
                        case 1:
                            xts_trend[5] = 0;
                            xts_omission[5] += 1;

                            #region 一星 最大遗漏,基本最大连出
                            var xts5_ext = xts_max_ext[5] - 1;
                            var xts5 = xts_max[5];
                            if (xts5_ext > xts5)
                                xts_max[5] = xts5_ext;
                            xts_max_ext[5] = 0;

                            var last5_item = last_numbers[last_numbers.Length - 1];
                            if (yx_num == last5_item)
                            {
                                xts_max_series_ext[5] += 1;
                                if (xts_max_series_ext[5] > xts_max_series[5])
                                    xts_max_series[5] = xts_max_series_ext[5];
                            }
                            else
                                xts_max_series_ext[5] = 0;
                            #endregion
                            break;
                        case 2:
                            xts_trend[6] = 0;
                            xts_omission[6] += 1;

                            #region 一星 最大遗漏,基本最大连出
                            var xts6_ext = xts_max_ext[6] - 1;
                            var xts6 = xts_max[6];
                            if (xts6_ext > xts6)
                                xts_max[6] = xts6_ext;
                            xts_max_ext[6] = 0;

                            var last6_item = last_numbers[last_numbers.Length - 1];
                            if (yx_num == last6_item)
                            {
                                xts_max_series_ext[6] += 1;
                                if (xts_max_series_ext[6] > xts_max_series[6])
                                    xts_max_series[6] = xts_max_series_ext[6];
                            }
                            else
                                xts_max_series_ext[6] = 0;
                            #endregion
                            break;
                    }
                    //个位
                    g_trend = g_trend.Select(s => s = s + 1).ToArray();
                    g_trend[yx_num] = 0;
                    g_omission[yx_num] += 1;
                    #region 一星最大遗漏
                    g_max_ext = g_max_ext.Select(s => s = s + 1).ToArray();
                    var rm_ext = g_max_ext[yx_num] - 1;
                    var rm = g_max[yx_num];
                    if (rm_ext > rm)
                        g_max[yx_num] = rm_ext;
                    g_max_ext[yx_num] = 0;
                    #endregion
                    #region 基本最大连出
                    if (yx_num == last_numbers[last_numbers.Length - 1])
                    {
                        var last_item = last_numbers[last_numbers.Length - 1];
                        if (yx_num == last_item)
                        {
                            g_max_series_ext[last_item] += 1;
                            if (g_max_series_ext[yx_num] > g_max_series[yx_num])
                                g_max_series[yx_num] = g_max_series_ext[yx_num];
                        }
                        else
                            g_max_series_ext[yx_num] = 0;
                    }
                    #endregion
                    last_numbers = numbers;

                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        s = sum,                 //和值
                        d = double_sign,         //重号
                        x = xts_trend,           //形态走势
                        t = g_trend              //个位走势
                    });
                });
                #region 计算
                //个平均遗漏
                for (int j = 0; j < g_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - g_omission[j]) / (g_omission[j] == 0 ? 1 : g_omission[j]));
                    g_average[j] = num;
                }
                //走势平均遗漏
                for (int j = 0; j < xts_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - xts_omission[j]) / (xts_omission[j] == 0 ? 1 : xts_omission[j]));
                    xts_average[j] = num;
                }
                var obj = new
                {
                    tr = trend,

                    gn = g_omission,     //个位出现次数
                    ga = g_average,      //个位平均遗漏
                    gm = g_max,          //个位最大遗漏
                    gs = g_max_series,   //个位最大连出

                    xn = xts_omission,     //走势出现次数
                    xa = xts_average,      //走势平均遗漏
                    xm = xts_max,          //走势最大遗漏
                    xs = xts_max_series,   //走势最大连出
                };
                #endregion

                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("一星基础走势图错误：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 大小单双基础走势图
        /// </summary>
        /// <param name="Entitys"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public string TrendChart_DXDS(List<IsusesEntity> Entitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //个位形态走势(大,小,单,双)
                int[] g_trend = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态遗漏
                int[] g_omission = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态平均遗漏
                int[] g_average = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态最大遗漏
                int[] g_max = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] g_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态最大连出
                int[] g_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] g_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };

                //个位形态走势(大,小,单,双)
                int[] s_trend = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态遗漏
                int[] s_omission = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态平均遗漏
                int[] s_average = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态最大遗漏
                int[] s_max = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] s_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态最大连出
                int[] s_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] s_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };

                //上期开奖
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0 };
                //大小号：5～9属大号，0～4属小号。
                //单双号：13579为单号，02468为双号。
                Entitys.ForEach((Entity) =>
                {
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //个位
                    int g_num = numbers[numbers.Length - 1];
                    //十位
                    int s_num = numbers[numbers.Length - 2];

                    //个位
                    #region 个位 最大遗漏,基本最大连出
                    g_trend = g_trend.Select(s => s = s + 1).ToArray();
                    g_trend[g_num] = 0;
                    g_omission[g_num] += 1;

                    g_max_ext = g_max_ext.Select(s => s = s + 1).ToArray();
                    var rm_ext = g_max_ext[g_num] - 1;
                    var rm = g_max[g_num];
                    if (rm_ext > rm)
                        g_max[g_num] = rm_ext;
                    g_max_ext[g_num] = 0;
                    var last_item = last_numbers[last_numbers.Length - 1];
                    if (g_num == last_item)
                    {
                        g_max_series_ext[g_num] += 1;
                        if (g_max_series_ext[g_num] > g_max_series[g_num])
                            g_max_series[g_num] = g_max_series_ext[g_num];
                    }
                    else
                        g_max_series_ext[g_num] = 0;
                    #endregion
                    #region 十位 最大遗漏,基本最大连出
                    s_trend = s_trend.Select(s => s = s + 1).ToArray();
                    s_trend[g_num] = 0;
                    s_omission[g_num] += 1;

                    s_max_ext = s_max_ext.Select(s => s = s + 1).ToArray();
                    var rs_ext = s_max_ext[s_num] - 1;
                    var rs = s_max[s_num];
                    if (rs_ext > rs)
                        s_max[s_num] = rs_ext;
                    s_max_ext[s_num] = 0;
                    var last_item_s = last_numbers[last_numbers.Length - 2];
                    if (s_num == last_item_s)
                    {
                        s_max_series_ext[s_num] += 1;
                        if (s_max_series_ext[s_num] > s_max_series[s_num])
                            s_max_series[s_num] = s_max_series_ext[s_num];
                    }
                    else
                        s_max_series_ext[s_num] = 0;
                    #endregion
                    last_numbers = numbers;

                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        t = g_trend,             //个位走势
                        s = s_trend              //十位走势
                    });
                });
                #region 计算
                //个平均遗漏
                for (int j = 0; j < g_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - g_omission[j]) / (g_omission[j] == 0 ? 1 : g_omission[j]));
                    g_average[j] = num;
                }
                //走势平均遗漏
                for (int j = 0; j < s_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - s_omission[j]) / (s_omission[j] == 0 ? 1 : s_omission[j]));
                    s_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,

                    gn = g_omission,     //个位出现次数
                    ga = g_average,      //个位平均遗漏
                    gm = g_max,          //个位最大遗漏
                    gs = g_max_series,   //个位最大连出

                    sn = s_omission,     //十位出现次数
                    sa = s_average,      //十位平均遗漏
                    sm = s_max,          //十位最大遗漏
                    ss = s_max_series,   //十位最大连出
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("大小单双基础走势图错误：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 二星直选基础走势图
        /// </summary>
        /// <param name="Entitys"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public string TrendChart_ERXINGZHIXUAN(List<IsusesEntity> Entitys, int TopCount)
        {
            try
            {

                List<object> trend = new List<object>();
                //个位形态走势(大,小,单,双)
                int[] g_trend = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态遗漏
                int[] g_omission = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态平均遗漏
                int[] g_average = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态最大遗漏
                int[] g_max = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] g_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态最大连出
                int[] g_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] g_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };

                //个位形态走势(大,小,单,双)
                int[] s_trend = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态遗漏
                int[] s_omission = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态平均遗漏
                int[] s_average = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态最大遗漏
                int[] s_max = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] s_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //个位形态最大连出
                int[] s_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] s_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };

                //上期开奖
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0 };
                //大小号：5～9属大号，0～4属小号。
                //单双号：13579为单号，02468为双号。
                Entitys.ForEach((Entity) =>
                {
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //和值
                    int sum = numbers[3] + numbers[4];
                    //跨度
                    int max = (new int[] { numbers[3], numbers[4] }).OrderByDescending(o => o).FirstOrDefault();
                    int min = (new int[] { numbers[3], numbers[4] }).OrderBy(o => o).FirstOrDefault();
                    int span = max - min;
                    //重号个数
                    int double_sign = 0;
                    for (int i = 3; i < numbers.Length; i++)
                    {
                        if (numbers[i] == last_numbers[i])
                            double_sign++;
                    }

                    //个位
                    int g_num = numbers[numbers.Length - 1];
                    //十位
                    int s_num = numbers[numbers.Length - 2];

                    //个位
                    #region 个位 最大遗漏,基本最大连出
                    g_trend = g_trend.Select(s => s = s + 1).ToArray();
                    g_trend[g_num] = 0;
                    g_omission[g_num] += 1;

                    g_max_ext = g_max_ext.Select(s => s = s + 1).ToArray();
                    var rm_ext = g_max_ext[g_num] - 1;
                    var rm = g_max[g_num];
                    if (rm_ext > rm)
                        g_max[g_num] = rm_ext;
                    g_max_ext[g_num] = 0;
                    var last_item = last_numbers[last_numbers.Length - 1];
                    if (g_num == last_item)
                    {
                        g_max_series_ext[g_num] += 1;
                        if (g_max_series_ext[g_num] > g_max_series[g_num])
                            g_max_series[g_num] = g_max_series_ext[g_num];
                    }
                    else
                        g_max_series_ext[g_num] = 0;
                    #endregion
                    #region 十位 最大遗漏,基本最大连出
                    s_trend = s_trend.Select(s => s = s + 1).ToArray();
                    s_trend[g_num] = 0;
                    s_omission[g_num] += 1;

                    s_max_ext = s_max_ext.Select(s => s = s + 1).ToArray();
                    var rs_ext = s_max_ext[s_num] - 1;
                    var rs = s_max[s_num];
                    if (rs_ext > rs)
                        s_max[s_num] = rs_ext;
                    s_max_ext[s_num] = 0;
                    var last_item_s = last_numbers[last_numbers.Length - 2];
                    if (s_num == last_item_s)
                    {
                        s_max_series_ext[s_num] += 1;
                        if (s_max_series_ext[s_num] > s_max_series[s_num])
                            s_max_series[s_num] = s_max_series_ext[s_num];
                    }
                    else
                        s_max_series_ext[s_num] = 0;
                    #endregion
                    last_numbers = numbers;

                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        s = sum,                 //和值
                        d = double_sign,         //重号
                        gt = g_trend,             //个位走势
                        st = s_trend              //十位走势
                    });
                });
                #region 计算
                //个平均遗漏
                for (int j = 0; j < g_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - g_omission[j]) / (g_omission[j] == 0 ? 1 : g_omission[j]));
                    g_average[j] = num;
                }
                //走势平均遗漏
                for (int j = 0; j < s_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - s_omission[j]) / (s_omission[j] == 0 ? 1 : s_omission[j]));
                    s_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,

                    gn = g_omission,     //个位出现次数
                    ga = g_average,      //个位平均遗漏
                    gm = g_max,          //个位最大遗漏
                    gs = g_max_series,   //个位最大连出

                    sn = s_omission,     //十位出现次数
                    sa = s_average,      //十位平均遗漏
                    sm = s_max,          //十位最大遗漏
                    ss = s_max_series,   //十位最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("二星直选基础走势图错误：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 二星组选基础走势图
        /// </summary>
        /// <param name="Entitys"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public string TrendChart_ERXINGZUXUAN(List<IsusesEntity> Entitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //分布走势
                int[] xts_trend = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //分布遗漏
                int[] xts_omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //分布平均遗漏
                int[] xts_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //分布最大遗漏
                int[] xts_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] xts_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //分布最大连出
                int[] xts_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] xts_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


                //形态走势 (奇，偶，质，合, 2:0, 1:1, 0:1)
                int[] xt_trend = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //形态遗漏
                int[] xt_omission = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //形态平均遗漏
                int[] xt_average = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //形态最大遗漏
                int[] xt_max = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] xt_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //形态最大连出
                int[] xt_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] xt_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };


                //合 0, 4, 6, 8, 9, 10, 12, 14, 15, 16, 18, 20
                int[] h_ectype = new int[] { 0, 4, 6, 8, 9, 10, 12, 14, 15, 16, 18, 20 };
                //上期开奖
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0 };
                Entitys.ForEach((Entity) =>
                {
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //二星组选
                    int[] nums = { numbers[3], numbers[4] };
                    //形态走势 (奇，偶，质，合, 2:0, 1:1, 0:1)
                    int j_num = 0; //奇数
                    int o_num = 0; //偶数
                    for (int i = 0; i < nums.Length; i++)
                        if ((nums[i] / 2) == 0)
                            o_num++;
                        else
                            j_num++;
                    //和值
                    int sum = nums.Sum(s => s);
                    //跨度
                    nums = numbers.GroupBy(g => g).Select(s => s.Key).ToArray();


                    int max = nums.OrderByDescending(o => o).FirstOrDefault();
                    int min = nums.OrderBy(o => o).FirstOrDefault();
                    int span = max - min;

                    xts_trend = xts_trend.Select(s => s = s + 1).ToArray();
                    xts_max_ext = xts_max_ext.Select(s => s = s + 1).ToArray();

                    xt_trend = xts_trend.Select(s => s = s + 1).ToArray();
                    xt_max_ext = xts_max_ext.Select(s => s = s + 1).ToArray();

                    //重号个数
                    int double_sign = 0;
                    for (int i = 0; i < nums.Length; i++)
                    {
                        if (nums[i] == last_numbers[i])
                            double_sign++;
                        xts_trend[nums[i]] = 0;
                        xts_omission[nums[i]] += 1;

                        #region 最大遗漏
                        var xts_ext = xts_max_ext[nums[i]] - 1;
                        var xts = xts_max[nums[i]];
                        if (xts_ext > xts)
                            xts_max[nums[i]] = xts_ext;
                        xts_max_ext[nums[i]] = 0;
                        #endregion

                        #region 基本最大连出
                        if (nums.Length == 1)
                        {
                            if (nums[i] == last_numbers[3] || nums[i] == last_numbers[4])
                            {
                                xts_max_series_ext[nums[i]] += 1;
                                if (xts_max_series_ext[nums[i]] > xts_max_series[nums[i]])
                                    xts_max_series[nums[i]] = xts_max_series_ext[nums[i]];
                            }
                            else
                                xts_max_series_ext[nums[i]] = 0;
                        }
                        else if (nums.Length == 2)
                        {
                            if (nums[i] == last_numbers[i + 3])
                            {
                                xts_max_series_ext[nums[i]] += 1;
                                if (xts_max_series_ext[nums[i]] > xts_max_series[nums[i]])
                                    xts_max_series[nums[i]] = xts_max_series_ext[nums[i]];
                            }
                            else
                                xts_max_series_ext[nums[i]] = 0;
                        }
                        #endregion
                    }
                    #region 形态走势 (奇，偶，质，合, 2:0, 1:1, 0:1)
                    int last_sum = (new int[] { last_numbers[3], last_numbers[4] }).Sum(s => s);
                    int[] last_sums = new int[] { last_numbers[3], last_numbers[4] };
                    int last_j_num = 0; //奇数
                    int last_o_num = 0; //偶数
                    for (int i = 0; i < last_sums.Length; i++)
                        if ((last_sums[i] / 2) == 0)
                            last_o_num++;
                        else
                            last_j_num++;
                    if ((sum / 2) == 0)
                    {
                        xt_trend[1] = 0;
                        xt_omission[1] += 1;

                        #region 最大遗漏 连出
                        var xt_ext = xt_max_ext[1] - 1;
                        var xt = xt_max[1];
                        if (xt_ext > xt)
                            xt_max[1] = xt_ext;
                        xt_max_ext[1] = 0;

                        if (sum == last_sum)
                        {
                            xt_max_series_ext[1] += 1;
                            if (xt_max_series_ext[1] > xt_max_series[1])
                                xt_max_series[1] = xt_max_series_ext[1];
                        }
                        #endregion
                    }
                    else
                    {
                        xt_trend[0] = 0;
                        xt_omission[0] += 1;

                        #region 最大遗漏 连出
                        var xt_ext = xt_max_ext[0] - 1;
                        var xt = xt_max[0];
                        if (xt_ext > xt)
                            xt_max[0] = xt_ext;
                        xt_max_ext[0] = 0;

                        if (sum == last_sum)
                        {
                            xt_max_series_ext[0] += 1;
                            if (xt_max_series_ext[0] > xt_max_series[0])
                                xt_max_series[0] = xt_max_series_ext[0];
                        }
                        #endregion
                    }
                    if (h_ectype.Contains(sum))
                    {
                        xt_trend[3] = 0;
                        xt_omission[3] += 1;

                        #region 最大遗漏 连出
                        var xt_ext = xt_max_ext[3] - 1;
                        var xt = xt_max[3];
                        if (xt_ext > xt)
                            xt_max[3] = xt_ext;
                        xt_max_ext[3] = 0;

                        if (sum == last_sum)
                        {
                            xt_max_series_ext[3] += 1;
                            if (xt_max_series_ext[3] > xt_max_series[3])
                                xt_max_series[3] = xt_max_series_ext[3];
                        }
                        #endregion
                    }
                    else
                    {
                        xt_trend[2] = 0;
                        xt_omission[2] += 1;

                        #region 最大遗漏 连出
                        var xt_ext = xt_max_ext[2] - 1;
                        var xt = xt_max[2];
                        if (xt_ext > xt)
                            xt_max[2] = xt_ext;
                        xt_max_ext[2] = 0;

                        if (sum == last_sum)
                        {
                            xt_max_series_ext[2] += 1;
                            if (xt_max_series_ext[2] > xt_max_series[2])
                                xt_max_series[2] = xt_max_series_ext[2];
                        }
                        #endregion
                    }
                    if (j_num == 2)
                    {
                        xt_trend[4] = 0;
                        xt_omission[4] += 1;
                        #region 最大遗漏 连出
                        var xt_ext = xt_max_ext[4] - 1;
                        var xt = xt_max[4];
                        if (xt_ext > xt)
                            xt_max[4] = xt_ext;
                        xt_max_ext[4] = 0;

                        if (j_num == last_j_num)
                        {
                            xt_max_series_ext[4] += 1;
                            if (xt_max_series_ext[4] > xt_max_series[4])
                                xt_max_series[4] = xt_max_series_ext[4];
                        }
                        #endregion
                    }
                    else if (j_num == 1)
                    {
                        xt_trend[5] = 0;
                        xt_omission[5] += 1;

                        #region 最大遗漏 连出
                        var xt_ext = xt_max_ext[5] - 1;
                        var xt = xt_max[5];
                        if (xt_ext > xt)
                            xt_max[5] = xt_ext;
                        xt_max_ext[5] = 0;

                        if (j_num == last_j_num)
                        {
                            xt_max_series_ext[5] += 1;
                            if (xt_max_series_ext[5] > xt_max_series[5])
                                xt_max_series[5] = xt_max_series_ext[5];
                        }
                        #endregion
                    }
                    else
                    {
                        xt_trend[6] = 0;
                        xt_omission[6] += 1;

                        #region 最大遗漏 连出
                        var xt_ext = xt_max_ext[6] - 1;
                        var xt = xt_max[6];
                        if (xt_ext > xt)
                            xt_max[6] = xt_ext;
                        xt_max_ext[6] = 0;

                        if (j_num == last_j_num)
                        {
                            xt_max_series_ext[6] += 1;
                            if (xt_max_series_ext[6] > xt_max_series[6])
                                xt_max_series[6] = xt_max_series_ext[6];
                        }
                        #endregion
                    }
                    #endregion
                    last_numbers = numbers;
                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        s = sum,                 //和值
                        d = double_sign,         //重号
                        f = xts_trend,           //分布走势
                        x = xt_trend             //形态走势
                    });

                });

                #region 计算
                //分布平均遗漏
                for (int j = 0; j < xts_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - xts_omission[j]) / (xts_omission[j] == 0 ? 1 : xts_omission[j]));
                    xts_average[j] = num;
                }
                //形态平均遗漏
                for (int j = 0; j < xt_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - xt_omission[j]) / (xt_omission[j] == 0 ? 1 : xt_omission[j]));
                    xt_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,

                    fn = xts_omission,     //分布出现次数
                    fa = xts_average,      //分布平均遗漏
                    fm = xts_max,          //分布最大遗漏
                    fs = xts_max_series,   //分布最大连出

                    xn = xt_omission,     //形态出现次数
                    xa = xt_average,      //形态平均遗漏
                    xm = xt_max,          //形态最大遗漏
                    xs = xt_max_series,   //形态最大连出
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("二星组选基础走势图错误：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 三星直选基础走势图
        /// </summary>
        /// <param name="Entitys"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public string TrendChart_SANXINGZHIXUAN(List<IsusesEntity> Entitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //百位走势
                int[] b_trend = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //百位遗漏
                int[] b_omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //百位平均遗漏
                int[] b_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //百位最大遗漏
                int[] b_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] b_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //百位最大连出
                int[] b_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] b_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //十位走势
                int[] s_trend = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //十位遗漏
                int[] s_omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //十位平均遗漏
                int[] s_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //十位最大遗漏
                int[] s_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] s_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //十位最大连出
                int[] s_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] s_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //个位走势
                int[] g_trend = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //个位遗漏
                int[] g_omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //个位平均遗漏
                int[] g_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //个位最大遗漏
                int[] g_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] g_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //个位最大连出
                int[] g_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] g_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //上期开奖
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0 };
                Entitys.ForEach((Entity) =>
                {
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(v => Convert.ToInt32(v)).ToArray();
                    //三星直选
                    int[] nums = { numbers[2], numbers[3], numbers[4] };
                    int _b_num = numbers[2];
                    int _s_num = numbers[3];
                    int _g_num = numbers[4];
                    //和值
                    int sum = nums.Sum(v => v);
                    //跨度
                    int max = nums.OrderByDescending(o => o).FirstOrDefault();
                    int min = nums.OrderBy(o => o).FirstOrDefault();
                    int span = max - min;
                    //重号个数
                    int double_sign = 0;
                    //走势 百位 十位 个位
                    b_trend = b_trend.Select(v => v = v + 1).ToArray();
                    s_trend = s_trend.Select(v => v = v + 1).ToArray();
                    g_trend = g_trend.Select(v => v = v + 1).ToArray();
                    b_max_ext = b_max_ext.Select(v => v = v + 1).ToArray();
                    s_max_ext = s_max_ext.Select(v => v = v + 1).ToArray();
                    g_max_ext = g_max_ext.Select(v => v = v + 1).ToArray();
                    b_trend[_b_num] = 0;
                    s_trend[_s_num] = 0;
                    g_trend[_g_num] = 0;
                    b_omission[_b_num] += 1;
                    s_omission[_s_num] += 1;
                    g_omission[_g_num] += 1;
                    for (int i = 2; i < numbers.Length; i++)
                        if (numbers[i] == last_numbers[i])
                            double_sign++;
                    #region 最大遗漏
                    //百位
                    var b_ext = b_max_ext[_b_num] - 1;
                    var b = b_max[_b_num];
                    if (b_ext > b)
                        b_max[_b_num] = b_ext;
                    b_max_ext[_b_num] = 0;
                    //十位
                    var s_ext = s_max_ext[_s_num] - 1;
                    var s = s_max[_s_num];
                    if (s_ext > s)
                        s_max[_s_num] = s_ext;
                    s_max_ext[_s_num] = 0;
                    //个位
                    var g_ext = g_max_ext[_g_num] - 1;
                    var g = g_max[_g_num];
                    if (g_ext > g)
                        g_max[_g_num] = g_ext;
                    g_max_ext[_g_num] = 0;
                    #endregion
                    #region 最大连出
                    //百位
                    if (_b_num == last_numbers[2])
                    {
                        b_max_series_ext[_b_num] += 1;
                        if (b_max_series_ext[_b_num] > b_max_series[_b_num])
                            b_max_series[_b_num] = b_max_series_ext[_b_num];
                    }
                    else
                        b_max_series_ext[_b_num] = 0;
                    //十位
                    if (_s_num == last_numbers[3])
                    {
                        s_max_series_ext[_s_num] += 1;
                        if (s_max_series_ext[_s_num] > s_max_series[_s_num])
                            s_max_series[_s_num] = s_max_series_ext[_s_num];
                    }
                    else
                        s_max_series_ext[_s_num] = 0;
                    //个位
                    if (_g_num == last_numbers[4])
                    {
                        g_max_series_ext[_g_num] += 1;
                        if (g_max_series_ext[_g_num] > g_max_series[_g_num])
                            g_max_series[_g_num] = g_max_series_ext[_g_num];
                    }
                    else
                        g_max_series_ext[_g_num] = 0;
                    #endregion
                    last_numbers = numbers;
                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        s = sum,                 //和值
                        d = double_sign,         //重号
                        bw = b_trend,            //百位走势
                        sw = s_trend,            //十位走势
                        gw = g_trend             //个位走势
                    });
                });
                #region 计算
                //百位平均遗漏
                for (int j = 0; j < b_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - b_omission[j]) / (b_omission[j] == 0 ? 1 : b_omission[j]));
                    b_average[j] = num;
                }
                //十位平均遗漏
                for (int j = 0; j < s_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - s_omission[j]) / (s_omission[j] == 0 ? 1 : s_omission[j]));
                    s_average[j] = num;
                }
                //个位平均遗漏
                for (int j = 0; j < g_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - g_omission[j]) / (g_omission[j] == 0 ? 1 : g_omission[j]));
                    g_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,
                    bn = b_omission,     //百位出现次数
                    ba = b_average,      //百位平均遗漏
                    bm = b_max,          //百位最大遗漏
                    bs = b_max_series,   //百位最大连出

                    sn = s_omission,     //十位出现次数
                    sa = s_average,      //十位平均遗漏
                    sm = s_max,          //十位最大遗漏
                    ss = s_max_series,   //十位最大连出

                    gn = g_omission,     //个位出现次数
                    ga = g_average,      //个位平均遗漏
                    gm = g_max,          //个位最大遗漏
                    gs = g_max_series,   //个位最大连出
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("三星直选基础走势图错误：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 三星组选基础走势图
        /// </summary>
        /// <param name="Entitys"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        private string TrendChart_SANXINGZUXUAN(List<IsusesEntity> Entitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //分布走势
                int[] xts_trend = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //分布遗漏
                int[] xts_omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //分布平均遗漏
                int[] xts_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //分布最大遗漏
                int[] xts_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] xts_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //分布最大连出
                int[] xts_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] xts_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //合 0, 4, 6, 8, 9, 10, 12, 14, 15, 16, 18, 20
                int[] h_ectype = new int[] { 0, 4, 6, 8, 9, 10, 12, 14, 15, 16, 18, 20 };
                //上期开奖
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0 };
                Entitys.ForEach((Entity) =>
                {
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(v => Convert.ToInt32(v)).ToArray();
                    //三星组选
                    int[] nums = { numbers[2], numbers[3], numbers[4] };
                    //和值
                    int sum = nums.Sum(v => v);
                    //跨度
                    nums = numbers.GroupBy(v => v).Select(v => v.Key).ToArray();
                    int max = nums.OrderByDescending(o => o).FirstOrDefault();
                    int min = nums.OrderBy(o => o).FirstOrDefault();
                    int span = max - min;
                    //重号个数
                    int double_sign = 0;
                    xts_trend = xts_trend.Select(v => v = v + 1).ToArray();

                    //三星大数
                    int d_data = 0;
                    //三星偶数
                    int o_data = 0;
                    //三星合数
                    int h_data = 0;
                    //大小号：5～9属大号，0～4属小号。
                    //单双号：13579为单号，02468为双号。
                    for (int i = 2; i < numbers.Length; i++)
                    {
                        if (numbers[i] == last_numbers[i])
                        {
                            double_sign++;
                            //最大连出
                            xts_max_series_ext[numbers[i]] += 1;
                            if (xts_max_series_ext[numbers[i]] > xts_max_series[numbers[i]])
                                xts_max_series[numbers[i]] = xts_max_series_ext[numbers[i]];
                        }
                        xts_trend[numbers[i]] = 0;
                        xts_omission[numbers[i]] += 1;
                        //最大遗漏
                        xts_max_ext = xts_max_ext.Select(v => v = v + 1).ToArray();
                        var xts_ext = xts_max_ext[numbers[i]] - 1;
                        var xts = xts_max[numbers[i]];
                        if (xts_ext > xts)
                            xts_max[numbers[i]] = xts_ext;
                        xts_max_ext[numbers[i]] = 0;
                        if (numbers[i] >= 5)//大号
                            d_data++;
                        if ((numbers[i] / 2) != 0) //双号
                            o_data++;
                        if (!h_ectype.Contains(numbers[i]))
                            h_data++;
                    }
                    //形态走势
                    string xt = nums.Length == 1 ? "豹子" : nums.Length == 2 ? "组三" : "组六";
                    string[] xt_trend = new string[] { string.Format("{0}:{1}", d_data, 3 - d_data),
                                        string.Format("{0}:{1}", o_data, 3 - o_data), string.Format("{0}:{1}", h_data, 3 - h_data),
                                        xt };
                    last_numbers = numbers;
                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        s = sum,                 //和值
                        d = double_sign,         //重号
                        f = xts_trend,           //分布走势
                        x = xt_trend             //形态走势
                    });
                });
                #region 计算
                //分布平均遗漏
                for (int j = 0; j < xts_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - xts_omission[j]) / (xts_omission[j] == 0 ? 1 : xts_omission[j]));
                    xts_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,
                    fn = xts_omission,     //分布出现次数
                    fa = xts_average,      //分布平均遗漏
                    fm = xts_max,          //分布最大遗漏
                    fs = xts_max_series,   //分布最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("三星组选基础走势图错误：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 五星基础走势
        /// </summary>
        /// <param name="Entitys"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        private string TrendChart_WUXING(List<IsusesEntity> Entitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //和值走势 0-13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35-45
                int[] sum_trend = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //和值遗漏
                int[] sum_omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //和值平均遗漏
                int[] sum_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //和值最大遗漏
                int[] sum_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] sum_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //和值最大连出
                int[] sum_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] sum_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //上期开奖
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0 };
                Entitys.ForEach((Entity) =>
                {
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(v => Convert.ToInt32(v)).ToArray();
                    //和值
                    int sum = numbers.Sum(v => v);
                    //跨度
                    int max = numbers.OrderByDescending(o => o).FirstOrDefault();
                    int min = numbers.OrderBy(o => o).FirstOrDefault();
                    int span = max - min;
                    //重号个数
                    int double_sign = 0;
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        int item = numbers[i];
                        int last_item = last_numbers[i];
                        if (item == last_item)
                            double_sign++;
                    }
                    sum_trend = sum_trend.Select(v => v = v + 1).ToArray();
                    sum_max_ext = sum_max_ext.Select(v => v = v + 1).ToArray();
                    if (sum < 14)
                    {
                        sum_trend[0] = 0;
                        sum_omission[0] += 1;
                        if (last_numbers.Sum(v => v) == sum)
                        {
                            sum_max_series_ext[0] += 1;
                            if (sum_max_series_ext[0] > sum_max_series[0])
                                sum_max_series[0] = sum_max_series_ext[0];
                        }
                        var xts_ext = sum_max_ext[0] - 1;
                        var xts = sum_max[0];
                        if (xts_ext > xts)
                            sum_max[0] = xts_ext;
                        sum_max_ext[0] = 0;
                    }
                    else if (sum > 34)
                    {
                        sum_trend[sum_trend.Length - 1] = 0;
                        sum_omission[sum_trend.Length - 1] += 1;
                        if (last_numbers.Sum(v => v) == sum)
                        {
                            sum_max_series_ext[sum_trend.Length - 1] += 1;
                            if (sum_max_series_ext[sum_trend.Length - 1] > sum_max_series[sum_trend.Length - 1])
                                sum_max_series[sum_trend.Length - 1] = sum_max_series_ext[sum_trend.Length - 1];
                        }
                        var xts_ext = sum_max_ext[sum_trend.Length - 1] - 1;
                        var xts = sum_max[sum_trend.Length - 1];
                        if (xts_ext > xts)
                            sum_max[sum_trend.Length - 1] = xts_ext;
                        sum_max_ext[sum_trend.Length - 1] = 0;
                    }
                    else
                    {
                        sum_trend[sum - 13] = 0;
                        sum_omission[sum - 13] += 1;
                        if (last_numbers.Sum(v => v) == sum)
                        {
                            sum_max_series_ext[sum - 13] += 1;
                            if (sum_max_series_ext[sum - 13] > sum_max_series[sum - 13])
                                sum_max_series[sum - 13] = sum_max_series_ext[sum - 13];
                        }
                        var xts_ext = sum_max_ext[sum - 13] - 1;
                        var xts = sum_max[sum - 13];
                        if (xts_ext > xts)
                            sum_max[sum - 13] = xts_ext;
                        sum_max_ext[sum - 13] = 0;
                    }
                    last_numbers = numbers;
                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        s = sum,                 //和值
                        d = double_sign,         //重号
                        st = sum_trend           //分布走势
                    });
                });
                var obj = new
                {
                    tr = trend,
                    sn = sum_omission,     //和值出现次数
                    sa = sum_average,      //和值平均遗漏
                    sm = sum_max,          //和值最大遗漏
                    ss = sum_max_series,   //和值最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("五星基础走势错误：{0}", ex.Message));
            }
        }
        #endregion
        #endregion
    }
}
