using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Enum.Common;
using CL.Tools.Common;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Enum.Common.Lottery;
using CL.Entity.Json.Omission;

namespace CL.Tools.LotteryTrendChart.SYYDJ
{
    /// <summary>
    /// 十一运夺金基础遗漏数据
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
        Log log = new Log("Lotomission_SYYDJ");
        #endregion

        public LotOmission(int Code)
        {
            LotCode = Code;
        }

        #region 基础遗漏
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
                    #region 任选玩法和乐选4乐选5玩法
                    case (int)SD11X5PlayCode.R2: //任选2
                    case (int)HB11X5PlayCode.R2: //任选2
                    case (int)SD11X5PlayCode.R3: //任选3
                    case (int)HB11X5PlayCode.R3: //任选3
                    case (int)SD11X5PlayCode.R4: //任选4
                    case (int)HB11X5PlayCode.R4: //任选4
                    case (int)SD11X5PlayCode.R5: //任选5
                    case (int)HB11X5PlayCode.R5: //任选5
                    case (int)SD11X5PlayCode.R6: //任选6
                    case (int)HB11X5PlayCode.R6: //任选6
                    case (int)SD11X5PlayCode.R7: //任选7
                    case (int)HB11X5PlayCode.R7: //任选7
                    case (int)SD11X5PlayCode.R8: //任选8
                    case (int)HB11X5PlayCode.R8: //任选8
                    case (int)SD11X5PlayCode.LX4: //乐选4
                    case (int)SD11X5PlayCode.LX5: //乐选5
                        Result = this.RX_Omission(IsuseEntitys);
                        break;
                    #endregion
                    #region 前一玩法
                    case (int)SD11X5PlayCode.Q1: //前一
                    case (int)HB11X5PlayCode.Q1: //前一
                        Result = this.Q1_Omission(IsuseEntitys);
                        break;
                    #endregion
                    #region 前二直选组选
                    case (int)SD11X5PlayCode.Q2_ZX: //前二直选
                    case (int)HB11X5PlayCode.Q2_ZX: //前二直选
                        Result = this.Q2_ZHI_Omission(IsuseEntitys);
                        break;
                    case (int)SD11X5PlayCode.Q2_ZUX: //前二组选
                    case (int)HB11X5PlayCode.Q2_ZUX: //前二组选
                        Result = this.Q2_ZU_Omission(IsuseEntitys);
                        break;
                    #endregion
                    #region 前三直选组选
                    case (int)SD11X5PlayCode.Q3_ZUX: //前三组选
                    case (int)HB11X5PlayCode.Q3_ZUX: //前三组选
                        Result = this.Q3_ZU_Omission(IsuseEntitys);
                        break;
                    case (int)SD11X5PlayCode.Q3_ZX: //前三直选
                    case (int)SD11X5PlayCode.LX3: //前三直选
                    case (int)HB11X5PlayCode.Q3_ZX: //前三直选
                        Result = this.Q3_ZHI_Omission(IsuseEntitys);
                        break;
                    #endregion
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
                log.Write(string.Format("实现基础遗漏错误[omission]：", ex.StackTrace), true);
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
        /// 任选玩法和乐选4乐选5玩法基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string RX_Omission(List<IsusesEntity> IsuseEntitys)
        {
            int[] omissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            IsuseEntitys.ForEach((item) =>
            {
                int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                omissionNumbers = omissionNumbers.Select(s => s = s + 1).ToArray();
                foreach (var val in Numbers)
                    omissionNumbers[val - 1] = 0;
            });
            var obj = new
            {
                i = IssueName,
                o = omissionNumbers
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 前一玩法基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string Q1_Omission(List<IsusesEntity> IsuseEntitys)
        {
            int[] omissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            IsuseEntitys.ForEach((item) =>
            {
                int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                omissionNumbers = omissionNumbers.Select(s => s = s + 1).ToArray();
                omissionNumbers[Numbers[0] - 1] = 0;
            });
            var obj = new
            {
                i = IssueName,
                o = omissionNumbers
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 前二直选基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string Q2_ZHI_Omission(List<IsusesEntity> IsuseEntitys)
        {
            int[] W_omissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] Q_omissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<int[]> omissions = new List<int[]>();
            IsuseEntitys.ForEach((item) =>
            {
                int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                //万位
                W_omissionNumbers = W_omissionNumbers.Select(s => s = s + 1).ToArray();
                W_omissionNumbers[Numbers[0] - 1] = 0;
                //千位
                Q_omissionNumbers = Q_omissionNumbers.Select(s => s = s + 1).ToArray();
                Q_omissionNumbers[Numbers[1] - 1] = 0;
            });
            omissions.Add(W_omissionNumbers);
            omissions.Add(Q_omissionNumbers);
            var obj = new
            {
                i = IssueName,
                o = omissions
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 前二组选基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string Q2_ZU_Omission(List<IsusesEntity> IsuseEntitys)
        {
            int[] omissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            IsuseEntitys.ForEach((item) =>
            {
                int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                omissionNumbers = omissionNumbers.Select(s => s = s + 1).ToArray();
                for (int i = 0; i < 2; i++)
                    omissionNumbers[Numbers[i] - 1] = 0;
            });
            var obj = new
            {
                i = IssueName,
                o = omissionNumbers
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 前三组选基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string Q3_ZU_Omission(List<IsusesEntity> IsuseEntitys)
        {
            int[] omissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            IsuseEntitys.ForEach((item) =>
            {
                int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                omissionNumbers = omissionNumbers.Select(s => s = s + 1).ToArray();
                for (int i = 0; i < 3; i++)
                    omissionNumbers[Numbers[i] - 1] = 0;
            });
            var obj = new
            {
                i = IssueName,
                o = omissionNumbers
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 前三直选基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string Q3_ZHI_Omission(List<IsusesEntity> IsuseEntitys)
        {
            int[] W_omissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] Q_omissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] B_omissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<int[]> omissions = omissions = new List<int[]>();
            IsuseEntitys.ForEach((item) =>
            {
                int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                //万位
                W_omissionNumbers = W_omissionNumbers.Select(s => s = s + 1).ToArray();
                W_omissionNumbers[Numbers[0] - 1] = 0;
                //千位
                Q_omissionNumbers = Q_omissionNumbers.Select(s => s = s + 1).ToArray();
                Q_omissionNumbers[Numbers[1] - 1] = 0;
                //百位
                B_omissionNumbers = B_omissionNumbers.Select(s => s = s + 1).ToArray();
                B_omissionNumbers[Numbers[2] - 1] = 0;
            });
            omissions.Add(W_omissionNumbers);
            omissions.Add(Q_omissionNumbers);
            omissions.Add(B_omissionNumbers);
            var obj = new
            {
                i = IssueName,
                o = omissions
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        #endregion
        #endregion

        #region 基础走势图
        /// <summary>
        /// 实现基础走势图
        /// </summary>
        /// <param name="IsuseName"></param>
        /// <param name="PlayCode"></param>
        /// <param name="TopCount"></param>
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
                    #region 任选玩法和乐选4乐选5玩法
                    case (int)SD11X5PlayCode.R2: //任选2
                    case (int)HB11X5PlayCode.R2: //任选2
                    case (int)SD11X5PlayCode.R3: //任选3
                    case (int)HB11X5PlayCode.R3: //任选3
                    case (int)SD11X5PlayCode.R4: //任选4
                    case (int)HB11X5PlayCode.R4: //任选4
                    case (int)SD11X5PlayCode.R5: //任选5
                    case (int)HB11X5PlayCode.R5: //任选5
                    case (int)SD11X5PlayCode.R6: //任选6
                    case (int)HB11X5PlayCode.R6: //任选6
                    case (int)SD11X5PlayCode.R7: //任选7
                    case (int)HB11X5PlayCode.R7: //任选7
                    case (int)SD11X5PlayCode.R8: //任选8
                    case (int)HB11X5PlayCode.R8: //任选8
                    case (int)SD11X5PlayCode.LX4: //乐选4
                    case (int)SD11X5PlayCode.LX5: //乐选5
                        Result = this.TrendChart_RenXuan(IsuseEntitys, TopCount);
                        break;
                    #endregion
                    #region 前一玩法
                    case (int)SD11X5PlayCode.Q1: //前一
                    case (int)HB11X5PlayCode.Q1: //前一
                        Result = this.TrendChart_Q1(IsuseEntitys, TopCount);
                        break;
                    #endregion
                    #region 前二直选组选
                    case (int)SD11X5PlayCode.Q2_ZX: //前二直选
                    case (int)HB11X5PlayCode.Q2_ZX: //前二直选
                        Result = this.TrendChart_Q2_ZHIXUAN(IsuseEntitys, TopCount);
                        break;
                    case (int)SD11X5PlayCode.Q2_ZUX: //前二组选
                    case (int)HB11X5PlayCode.Q2_ZUX: //前二组选
                        Result = this.TrendChart_Q2_ZUXUAN(IsuseEntitys, TopCount);
                        break;
                    #endregion
                    #region 前三直选组选
                    case (int)SD11X5PlayCode.Q3_ZUX: //前三组选
                    case (int)HB11X5PlayCode.Q3_ZUX: //前三组选
                        Result = this.TrendChart_Q3_ZUXUAN(IsuseEntitys, TopCount);
                        break;
                    case (int)SD11X5PlayCode.Q3_ZX: //前三直选
                    case (int)SD11X5PlayCode.LX3: //前三直选
                    case (int)HB11X5PlayCode.Q3_ZX: //前三直选
                        Result = this.TrendChart_Q3_ZHIXUAN(IsuseEntitys, TopCount);
                        break;
                    #endregion
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
                log.Write(string.Format("实现基础走势图错误[omission]：", ex.StackTrace), true);
                JsonRec = new OmissionResult()
                {
                    Code = (int)ResultCode.SystemBusy,
                    Msg = Enum.Common.Common.GetDescription(ResultCode.SystemBusy)
                };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(JsonRec);
        }

        #region 玩法详细处理
        /// <summary>
        /// 前一基础走势图
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_Q1(List<IsusesEntity> IsuseEntitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //质 1,2,3,5,7,11
                //int[] z = new int[] { 1, 2, 3, 5, 7, 11 };
                //合 4,6,8,9,10
                int[] h = new int[] { 4, 6, 8, 9, 10 };
                //形态：奇 偶 质 合 0路 1路 2路
                int[] xt = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //上期开奖号码
                int[] lastissue = new int[] { 0, 0, 0, 0, 0 };
                //冷热30
                int[] omission_30 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热50
                int[] omission_50 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热100
                int[] omission_100 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //遗漏
                int[] omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                #region 遗漏统计

                //基础平均遗漏
                int[] base_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //基础最大遗漏
                int[] base_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] base_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //基础最大连出
                int[] base_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] base_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //形态走势
                int[] omission_xt = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //形态平均遗漏
                int[] xt_average = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //形态最大遗漏
                int[] xt_max = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] xt_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                //形态最大连出
                int[] xt_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int[] xt_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                #endregion

                //上一次开奖号码
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int i = 0;
                IsuseEntitys.ForEach((Entity) =>
                {
                    i++;
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //前一
                    int q = numbers[0];
                    //排序后的开奖号码
                    int[] nums = numbers.OrderBy(o => o).ToArray();
                    //和值
                    int sum = numbers.Sum(s => s);
                    //跨度
                    int span = nums[4] - nums[0];
                    //重号个数
                    int double_sign = 0;
                    foreach (var item in numbers)
                        if (lastissue.Contains(item))
                            double_sign++;
                    //冷热
                    if (i <= 30)
                        omission_30[q - 1] += 1;
                    if (i <= 50)
                        omission_50[q - 1] += 1;
                    if (i <= 100)
                        omission_100[q - 1] += 1;
                    //遗漏
                    omission = omission.Select(s => s = s + 1).ToArray();
                    omission[q - 1] = 0;
                    //形态走势
                    //奇 偶 质 合 0路 1路 2路
                    xt = xt.Select(s => s = s + 1).ToArray();

                    xt_max_ext = xt_max_ext.Select(s => s = s + 1).ToArray();
                    //奇 偶
                    if ((q % 2) == 0)
                    {
                        xt[1] = 0;
                        omission_xt[1] += 1;

                        #region 基础最大遗漏
                        var rm_ext_1 = xt_max_ext[1] - 1;
                        var rm_1 = xt_max[1];
                        if (rm_ext_1 > rm_1)
                            xt_max[1] = rm_ext_1;
                        xt_max_ext[1] = 0;
                        #endregion

                        #region 基础最大连出
                        if (q == last_numbers[0])
                        {
                            xt_max_series_ext[1] += 1;
                            if (base_max_series_ext[1] > base_max_series[1])
                                base_max_series[1] = base_max_series_ext[1];
                        }
                        else
                            base_max_series_ext[1] = 0;
                        #endregion
                    }
                    else
                    {
                        xt[0] = 0;
                        omission_xt[0] += 1;

                        #region 基础最大遗漏
                        var rm_ext_0 = xt_max_ext[0] - 1;
                        var rm_0 = xt_max[0];
                        if (rm_ext_0 > rm_0)
                            xt_max[0] = rm_ext_0;
                        xt_max_ext[0] = 0;
                        #endregion

                        #region 基础最大连出
                        if (q == last_numbers[0])
                        {
                            xt_max_series_ext[0] += 1;
                            if (base_max_series_ext[0] > base_max_series[0])
                                base_max_series[0] = base_max_series_ext[0];
                        }
                        else
                            base_max_series_ext[0] = 0;
                        #endregion
                    }
                    if (h.Contains(q)) //合
                    {
                        xt[3] = 0;
                        omission_xt[3] += 1;

                        #region 基础最大遗漏
                        var rm_ext_3 = xt_max_ext[3] - 1;
                        var rm_3 = xt_max[3];
                        if (rm_ext_3 > rm_3)
                            xt_max[3] = rm_ext_3;
                        xt_max_ext[3] = 0;
                        #endregion

                        #region 基础最大连出
                        if (q == last_numbers[0])
                        {
                            xt_max_series_ext[3] += 1;
                            if (base_max_series_ext[3] > base_max_series[3])
                                base_max_series[3] = base_max_series_ext[3];
                        }
                        else
                            base_max_series_ext[3] = 0;
                        #endregion
                    }
                    else //质
                    {
                        xt[2] = 0;
                        omission_xt[2] += 1;

                        #region 基础最大遗漏
                        var rm_ext_2 = xt_max_ext[2] - 1;
                        var rm_2 = xt_max[2];
                        if (rm_ext_2 > rm_2)
                            xt_max[2] = rm_ext_2;
                        xt_max_ext[2] = 0;
                        #endregion

                        #region 基础最大连出
                        if (q == last_numbers[0])
                        {
                            xt_max_series_ext[2] += 1;
                            if (base_max_series_ext[2] > base_max_series[2])
                                base_max_series[2] = base_max_series_ext[2];
                        }
                        else
                            base_max_series_ext[2] = 0;
                        #endregion
                    }
                    switch ((q % 3))
                    {
                        case 0:
                            xt[4] = 0;
                            omission_xt[4] += 1;

                            #region 基础最大遗漏
                            var rm_ext_4 = xt_max_ext[4] - 1;
                            var rm_4 = xt_max[4];
                            if (rm_ext_4 > rm_4)
                                xt_max[4] = rm_ext_4;
                            xt_max_ext[4] = 0;
                            #endregion

                            #region 基础最大连出
                            if (q == last_numbers[0])
                            {
                                xt_max_series_ext[4] += 1;
                                if (base_max_series_ext[4] > base_max_series[4])
                                    base_max_series[4] = base_max_series_ext[4];
                            }
                            else
                                base_max_series_ext[4] = 0;
                            #endregion
                            break;
                        case 1:
                            xt[5] = 0;
                            omission_xt[5] += 1;

                            #region 基础最大遗漏
                            var rm_ext_5 = xt_max_ext[5] - 1;
                            var rm_5 = xt_max[5];
                            if (rm_ext_5 > rm_5)
                                xt_max[5] = rm_ext_5;
                            xt_max_ext[5] = 0;
                            #endregion

                            #region 基础最大连出
                            if (q == last_numbers[0])
                            {
                                xt_max_series_ext[5] += 1;
                                if (base_max_series_ext[5] > base_max_series[5])
                                    base_max_series[5] = base_max_series_ext[5];
                            }
                            else
                                base_max_series_ext[5] = 0;
                            #endregion
                            break;
                        case 2:
                            xt[6] = 0;
                            omission_xt[6] += 1;

                            #region 基础最大遗漏
                            var rm_ext_6 = xt_max_ext[6] - 1;
                            var rm_6 = xt_max[6];
                            if (rm_ext_6 > rm_6)
                                xt_max[6] = rm_ext_6;
                            xt_max_ext[6] = 0;
                            #endregion

                            #region 基础最大连出
                            if (q == last_numbers[0])
                            {
                                xt_max_series_ext[6] += 1;
                                if (base_max_series_ext[6] > base_max_series[6])
                                    base_max_series[6] = base_max_series_ext[6];
                            }
                            else
                                base_max_series_ext[6] = 0;
                            #endregion
                            break;
                    }

                    #region 基础最大遗漏
                    base_max_ext = base_max_ext.Select(s => s = s + 1).ToArray();

                    var rm_ext = base_max_ext[q - 1] - 1;
                    var rm = base_max[q - 1];
                    if (rm_ext > rm)
                        base_max[q - 1] = rm_ext;
                    base_max_ext[q - 1] = 0;
                    #endregion

                    #region 基础最大连出
                    if (q == last_numbers[0])
                    {
                        base_max_series_ext[q - 1] += 1;
                        if (base_max_series_ext[q - 1] > base_max_series[q - 1])
                            base_max_series[q - 1] = base_max_series_ext[q - 1];
                    }
                    else
                    {
                        base_max_series_ext[q - 1] = 0;
                    }
                    #endregion


                    last_numbers = numbers;
                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        x = xt,                  //形态走势
                        s = sum,                 //和值
                        d = double_sign,         //重号
                        t = omission             //走势
                    });
                    lastissue = numbers;
                });
                #region 计算
                //基础平均遗漏
                for (int j = 0; j < omission_100.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - omission_100[j]) / (omission_100[j] == 0 ? 1 : omission_100[j]));
                    base_average[j] = num;
                }
                //形态平均遗漏
                for (int j = 0; j < omission_xt.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - omission_xt[j]) / (omission_xt[j] == 0 ? 1 : omission_xt[j]));
                    xt_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,
                    o = omission,
                    o3 = omission_30,
                    o5 = omission_50,
                    o0 = omission_100,

                    bn = omission_100,      //基础出现次数
                    ba = base_average,      //基础平均遗漏
                    bm = base_max,          //基础最大遗漏
                    bs = base_max_series,   //基础最大连出

                    sn = omission_xt,     //形态出现次数
                    sa = xt_average,      //形态平均遗漏
                    sm = xt_max,          //形态最大遗漏
                    ss = xt_max_series    //形态最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础走势图[前一]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 前二直选基础走势图
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_Q2_ZHIXUAN(List<IsusesEntity> IsuseEntitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //万位走势
                int[] omission_w = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //千位走势
                int[] omission_q = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                #region 遗漏统计

                //万位走势
                int[] w_omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //万位平均遗漏
                int[] w_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //万位最大遗漏
                int[] w_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] w_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //万位最大连出
                int[] w_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] w_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //千位走势
                int[] q_omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //千位平均遗漏
                int[] q_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //千位最大遗漏
                int[] q_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] q_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //千位最大连出
                int[] q_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] q_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                #endregion



                //上期开奖
                int[] lastissue = new int[] { 0, 0, 0, 0, 0 };
                IsuseEntitys.ForEach((Entity) =>
                {
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //排序后的开奖号码
                    int[] nums = (new int[] { numbers[0], numbers[1] }).OrderBy(o => o).ToArray();
                    //和值
                    int sum = nums.Sum(s => s);
                    //跨度
                    int span = nums[1] - nums[0];
                    //重号个数
                    int double_sign = 0;
                    if (lastissue[0] == numbers[0])
                        double_sign += 1;
                    if (lastissue[1] == numbers[1])
                        double_sign += 1;
                    //万位走势
                    omission_w = omission_w.Select(s => s = s + 1).ToArray();
                    omission_w[numbers[0] - 1] = 0;
                    //千位走势
                    omission_q = omission_q.Select(s => s = s + 1).ToArray();
                    omission_q[numbers[1] - 1] = 0;

                    //开始计算
                    w_omission[numbers[0] - 1] += 1;
                    q_omission[numbers[1] - 1] += 1;

                    #region 基础最大遗漏
                    w_max_ext = w_max_ext.Select(s => s = s + 1).ToArray();
                    q_max_ext = q_max_ext.Select(s => s = s + 1).ToArray();

                    var wm_ext = w_max_ext[numbers[0] - 1] - 1;
                    var wm = w_max[numbers[0] - 1];
                    if (wm_ext > wm)
                        w_max[numbers[0] - 1] = wm_ext;
                    w_max_ext[numbers[0] - 1] = 0;

                    var qm_ext = q_max_ext[numbers[1] - 1] - 1;
                    var qm = q_max[numbers[1] - 1];
                    if (qm_ext > qm)
                        q_max[numbers[1] - 1] = qm_ext;
                    q_max_ext[numbers[1] - 1] = 0;
                    #endregion

                    #region 基础最大连出
                    if (numbers[0] == lastissue[0])
                    {
                        w_max_series_ext[numbers[0] - 1] += 1;
                        if (w_max_series_ext[numbers[0] - 1] > w_max_series[numbers[0] - 1])
                            w_max_series[numbers[0] - 1] = w_max_series_ext[numbers[0] - 1];
                    }
                    else
                        w_max_series_ext[numbers[0] - 1] = 0;

                    if (numbers[1] == lastissue[1])
                    {
                        q_max_series_ext[numbers[1] - 1] += 1;
                        if (q_max_series_ext[numbers[1] - 1] > q_max_series[numbers[1] - 1])
                            q_max_series[numbers[1] - 1] = q_max_series_ext[numbers[1] - 1];
                    }
                    else
                        q_max_series_ext[numbers[1] - 1] = 0;
                    #endregion

                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        s = sum,                 //和值
                        d = double_sign,         //重号
                        zw = omission_w,         //万位走势
                        zq = omission_q          //千位走势
                    });
                    lastissue = new int[] { numbers[0], numbers[1] };
                });
                #region 计算
                //万位平均遗漏
                for (int j = 0; j < w_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - w_omission[j]) / (w_omission[j] == 0 ? 1 : w_omission[j]));
                    w_average[j] = num;
                }
                //千位平均遗漏
                for (int j = 0; j < q_omission.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - q_omission[j]) / (q_omission[j] == 0 ? 1 : q_omission[j]));
                    q_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,

                    bn = w_omission,     //万位出现次数
                    ba = w_average,      //万位平均遗漏
                    bm = w_max,          //万位最大遗漏
                    bs = w_max_series,   //万位最大连出

                    sn = q_omission,     //千位出现次数
                    sa = q_average,      //千位平均遗漏
                    sm = q_max,          //千位最大遗漏
                    ss = q_max_series    //千位最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础走势图[前二直选]：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 前二组选基础走势图
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_Q2_ZUXUAN(List<IsusesEntity> IsuseEntitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //上期开奖
                int[] lastissue = new int[] { 0, 0, 0, 0, 0 };
                //走势
                int[] trend_ectype = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //遗漏
                int[] omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热30
                int[] omission_30 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热50
                int[] omission_50 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热100
                int[] omission_100 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //合 4,6,8,9,10
                int[] h_ectype = new int[] { 4, 6, 8, 9, 10 };


                //走势平均遗漏
                int[] xt_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //走势最大遗漏
                int[] xt_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] xt_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //走势最大连出
                int[] xt_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] xt_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


                int i = 0;
                IsuseEntitys.ForEach((Entity) =>
                {
                    i++;
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //排序后的开奖号码
                    int[] nums = (new int[] { numbers[0], numbers[1] }).OrderBy(b => b).ToArray();
                    //和值
                    int sum = nums.Sum(s => s);
                    //跨度
                    int span = nums[1] - nums[0];
                    //重号个数
                    int double_sign = 0;
                    foreach (var item in nums)
                        if (lastissue.Contains(item))
                            double_sign += 1;
                    //走势
                    trend_ectype = trend_ectype.Select(s => s + 1).ToArray();
                    trend_ectype[numbers[0] - 1] = 0;
                    trend_ectype[numbers[1] - 1] = 0;
                    //遗漏
                    omission = omission.Select(s => s + 1).ToArray();

                    xt_max_ext = xt_max_ext.Select(s => s = s + 1).ToArray();
                    for (int n = 0; n < nums.Length; n++)
                    {
                        var item = nums[n];
                        omission[item - 1] = 0;

                        #region 最大遗漏
                        var wm_ext = xt_max_ext[item - 1] - 1;
                        var wm = xt_max[item - 1];
                        if (wm_ext > wm)
                            xt_max[item - 1] = wm_ext;
                        xt_max_ext[item - 1] = 0;
                        #endregion

                        #region 基础最大连出
                        if (item == lastissue[n])
                        {
                            xt_max_series_ext[item - 1] += 1;
                            if (xt_max_series_ext[item - 1] > xt_max_series[item - 1])
                                xt_max_series[item - 1] = xt_max_series_ext[item - 1];
                        }
                        else
                            xt_max_series_ext[item - 1] = 0;
                        #endregion

                        if (i <= 30)
                            omission_30[item - 1] += 1;
                        if (i <= 50)
                            omission_50[item - 1] += 1;
                        if (i <= 100)
                            omission_100[item - 1] += 1;
                    }

                    //冷热

                    #region 形态
                    //大小
                    int d = 0, x = 0;
                    //奇偶
                    int j = 0, o = 0;
                    //质合
                    int z = 0, h = 0;
                    foreach (var item in nums)
                    {
                        //大小
                        if (item > 5)
                            d++;
                        else
                            x++;
                        //奇偶
                        if ((item % 2) == 0)
                            o++;
                        else
                            j++;
                        //质合
                        if (h_ectype.Contains(item))
                            h++;
                        else
                            z++;
                    }
                    string[] xt = new string[] { string.Format("{0}:{1}", d, x), string.Format("{0}:{1}", j, o), string.Format("{0}:{1}", z, h) };
                    #endregion
                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        s = sum,                 //和值
                        d = double_sign,         //重号
                        x = xt,                  //形态
                        t = trend_ectype         //走势
                    });
                    lastissue = new int[] { numbers[0], numbers[1] };
                });
                #region 计算
                //基础平均遗漏
                for (int j = 0; j < omission_100.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - omission_100[j]) / (omission_100[j] == 0 ? 1 : omission_100[j]));
                    xt_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,
                    o = omission,
                    o3 = omission_30,
                    o5 = omission_50,
                    o0 = omission_100,

                    sn = omission_100,    //走势出现次数
                    sa = xt_average,      //走势平均遗漏
                    sm = xt_max,          //走势最大遗漏
                    ss = xt_max_series    //走势最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础走势图[前二组选]：{0}", ex.Message));
            }

        }
        /// <summary>
        /// 前三直选基础走势图
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_Q3_ZHIXUAN(List<IsusesEntity> IsuseEntitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //万位走势
                int[] omission_w = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //千位走势
                int[] omission_q = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //百威走势
                int[] omission_b = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                #region 遗漏统计

                //万位走势
                int[] w_omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //万位平均遗漏
                int[] w_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //万位最大遗漏
                int[] w_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] w_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //万位最大连出
                int[] w_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] w_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //万位走势
                int[] q_omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //千位平均遗漏
                int[] q_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //千位最大遗漏
                int[] q_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] q_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //千位最大连出
                int[] q_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] q_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //万位走势
                int[] b_omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //百位平均遗漏
                int[] b_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //百位最大遗漏
                int[] b_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] b_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //百位最大连出
                int[] b_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] b_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                #endregion



                //上期开奖
                int[] lastissue = new int[] { 0, 0, 0, 0, 0 };
                int i = 0;
                IsuseEntitys.ForEach((Entity) =>
                {
                    i++;
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //排序后的开奖号码
                    int[] nums = (new int[] { numbers[0], numbers[1], numbers[2] }).OrderBy(b => b).ToArray();
                    int sum = nums.Sum(s => s);
                    //跨度
                    int span = nums[2] - nums[0];
                    //重号个数
                    int double_sign = 0;
                    if (lastissue[0] == numbers[0])
                        double_sign++;
                    if (lastissue[1] == numbers[1])
                        double_sign++;
                    if (lastissue[2] == numbers[2])
                        double_sign++;
                    //走势 万位
                    omission_w = omission_w.Select(s => s + 1).ToArray();
                    omission_w[numbers[0] - 1] = 0;
                    //千位
                    omission_q = omission_q.Select(s => s + 1).ToArray();
                    omission_q[numbers[1] - 1] = 0;
                    //百位
                    omission_b = omission_b.Select(s => s + 1).ToArray();
                    omission_b[numbers[2] - 1] = 0;

                    #region 走势图统计
                    w_omission[numbers[0] - 1] += 1;
                    q_omission[numbers[1] - 1] += 1;
                    b_omission[numbers[2] - 1] += 1;

                    #region 基础最大遗漏
                    w_max_ext = w_max_ext.Select(s => s = s + 1).ToArray();
                    q_max_ext = q_max_ext.Select(s => s = s + 1).ToArray();
                    b_max_ext = q_max_ext.Select(s => s = s + 1).ToArray();
                    //万位
                    var wm_ext = w_max_ext[numbers[0] - 1] - 1;
                    var wm = w_max[numbers[0] - 1];
                    if (wm_ext > wm)
                        w_max[numbers[0] - 1] = wm_ext;
                    w_max_ext[numbers[0] - 1] = 0;
                    //千位
                    var qm_ext = q_max_ext[numbers[1] - 1] - 1;
                    var qm = q_max[numbers[1] - 1];
                    if (qm_ext > qm)
                        q_max[numbers[1] - 1] = qm_ext;
                    q_max_ext[numbers[1] - 1] = 0;
                    //百位
                    var bm_ext = b_max_ext[numbers[2] - 1] - 1;
                    var bm = b_max[numbers[2] - 1];
                    if (bm_ext > bm)
                        b_max[numbers[2] - 1] = bm_ext;
                    b_max_ext[numbers[2] - 1] = 0;
                    #endregion

                    #region 基础最大连出
                    //万位
                    if (numbers[0] == lastissue[0])
                    {
                        w_max_series_ext[numbers[0] - 1] += 1;
                        if (w_max_series_ext[numbers[0] - 1] > w_max_series[numbers[0] - 1])
                            w_max_series[numbers[0] - 1] = w_max_series_ext[numbers[0] - 1];
                    }
                    else
                        w_max_series_ext[numbers[0] - 1] = 0;
                    //千位
                    if (numbers[1] == lastissue[1])
                    {
                        q_max_series_ext[numbers[1] - 1] += 1;
                        if (q_max_series_ext[numbers[1] - 1] > q_max_series[numbers[1] - 1])
                            q_max_series[numbers[1] - 1] = q_max_series_ext[numbers[1] - 1];
                    }
                    else
                        q_max_series_ext[numbers[1] - 1] = 0;
                    //百位
                    if (numbers[2] == lastissue[2])
                    {
                        b_max_series_ext[numbers[2] - 1] += 1;
                        if (b_max_series_ext[numbers[2] - 1] > b_max_series[numbers[2] - 1])
                            b_max_series[numbers[2] - 1] = b_max_series_ext[numbers[2] - 1];
                    }
                    else
                        b_max_series_ext[numbers[2] - 1] = 0;
                    #endregion
                    #endregion

                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        s = sum,                 //和值
                        d = double_sign,         //重号
                        zw = omission_w,         //走势图万位
                        zq = omission_q,         //走势图千位
                        zb = omission_b          //走势图百位
                    });
                    lastissue = new int[] { numbers[0], numbers[1], numbers[2] };
                });
                #region 计算
                //万位平均遗漏
                for (int j = 0; j < omission_w.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - omission_w[j]) / (omission_w[j] == 0 ? 1 : omission_w[j]));
                    w_average[j] = num;
                }
                //千位平均遗漏
                for (int j = 0; j < omission_q.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - omission_q[j]) / (omission_q[j] == 0 ? 1 : omission_q[j]));
                    q_average[j] = num;
                }
                //百位平均遗漏
                for (int j = 0; j < omission_b.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - omission_b[j]) / (omission_b[j] == 0 ? 1 : omission_b[j]));
                    b_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,

                    wn = w_omission,     //万位出现次数
                    wa = w_average,      //万位平均遗漏
                    wm = w_max,          //万位最大遗漏
                    ws = w_max_series,   //万位最大连出

                    qn = q_omission,     //千位出现次数
                    qa = q_average,      //千位平均遗漏
                    qm = q_max,          //千位最大遗漏
                    qs = q_max_series,   //千位最大连出

                    bn = b_omission,     //百位出现次数
                    ba = b_average,      //百位平均遗漏
                    bm = b_max,          //百位最大遗漏
                    bs = b_max_series,   //百位最大连出
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础走势图[前三直选]：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 前三组选基础走势图
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_Q3_ZUXUAN(List<IsusesEntity> IsuseEntitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //上期开奖
                int[] lastissue = new int[] { 0, 0, 0, 0, 0 };
                //合 4,6,8,9,10
                int[] h_ectype = new int[] { 4, 6, 8, 9, 10 };
                //遗漏
                int[] omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热30
                int[] omission_30 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热50
                int[] omission_50 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热100
                int[] omission_100 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //走势
                int[] trend_ectype = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //走势平均遗漏
                int[] xt_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //走势最大遗漏
                int[] xt_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] xt_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //走势最大连出
                int[] xt_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] xt_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                int i = 0;
                IsuseEntitys.ForEach((Entity) =>
                {
                    i++;
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //排序后的开奖号码
                    int[] nums = (new int[] { numbers[0], numbers[1], numbers[2] }).OrderBy(b => b).ToArray();
                    int sum = nums.Sum(s => s);
                    //跨度
                    int span = nums[2] - nums[0];
                    //重号个数
                    int double_sign = 0;
                    //大小
                    int d = 0, x = 0;
                    //奇偶
                    int j = 0, o = 0;
                    //质合
                    int z = 0, h = 0;

                    xt_max_ext = xt_max_ext.Select(s => s = s + 1).ToArray();
                    //走势
                    trend_ectype = trend_ectype.Select(s => s + 1).ToArray();
                    //遗漏
                    omission = omission.Select(s => s = s + 1).ToArray();

                    for (int n = 0; n < nums.Length; n++)
                    {
                        var item = nums[n];
                        if (lastissue.Contains(item))
                            double_sign++;

                        #region 最大遗漏
                        var wm_ext = xt_max_ext[item - 1] - 1;
                        var wm = xt_max[item - 1];
                        if (wm_ext > wm)
                            xt_max[item - 1] = wm_ext;
                        xt_max_ext[item - 1] = 0;
                        #endregion

                        #region 基础最大连出
                        if (item == lastissue[n])
                        {
                            xt_max_series_ext[item - 1] += 1;
                            if (xt_max_series_ext[item - 1] > xt_max_series[item - 1])
                                xt_max_series[item - 1] = xt_max_series_ext[item - 1];
                        }
                        else
                            xt_max_series_ext[item - 1] = 0;
                        #endregion

                        //走势
                        trend_ectype[item - 1] = 0;
                        //大小
                        if (item > 5)
                            d++;
                        else
                            x++;
                        //奇偶
                        if ((item % 2) == 0)
                            o++;
                        else
                            j++;
                        //质合
                        if (h_ectype.Contains(item))
                            h++;
                        else
                            z++;
                        //冷热 遗漏
                        omission[item - 1] = 0;
                        if (i <= 30)
                            omission_30[item - 1] += 1;
                        if (i <= 50)
                            omission_50[item - 1] += 1;
                        if (i <= 100)
                            omission_100[item - 1] += 1;

                    }
                    string[] xt = new string[] { string.Format("{0}:{1}", d, x), string.Format("{0}:{1}", j, o), string.Format("{0}:{1}", z, h) };


                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        s = sum,                 //和值
                        d = double_sign,         //重号
                        z = trend_ectype,        //走势
                        x = xt                   //形态
                    });
                    lastissue = new int[] { numbers[0], numbers[1], numbers[2] };
                });
                #region 计算
                //基础平均遗漏
                for (int j = 0; j < omission_100.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - omission_100[j]) / (omission_100[j] == 0 ? 1 : omission_100[j]));
                    xt_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,
                    o = omission,
                    o3 = omission_30,
                    o5 = omission_50,
                    o0 = omission_100,

                    sn = omission_100,    //走势出现次数
                    sa = xt_average,      //走势平均遗漏
                    sm = xt_max,          //走势最大遗漏
                    ss = xt_max_series    //走势最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础走势图[前三组选]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 任选基础走势图
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_RenXuan(List<IsusesEntity> IsuseEntitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //上期开奖
                int[] lastissue = new int[] { 0, 0, 0, 0, 0 };
                //合 4,6,8,9,10
                int[] h_ectype = new int[] { 4, 6, 8, 9, 10 };
                //遗漏
                int[] omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热30
                int[] omission_30 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热50
                int[] omission_50 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热100
                int[] omission_100 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //走势
                int[] trend_ectype = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //基础平均遗漏
                int[] base_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //基础最大遗漏
                int[] base_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] base_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //基础最大连出
                int[] base_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] base_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                int i = 0;
                IsuseEntitys.ForEach((Entity) =>
                {
                    i++;
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //排序后的开奖号码
                    int[] nums = numbers.OrderBy(b => b).ToArray();
                    //和值
                    int sum = nums.Sum(s => s);
                    //跨度
                    int span = nums[4] - nums[0];
                    //重号个数
                    int double_sign = 0;
                    //走势
                    trend_ectype = trend_ectype.Select(s => s = s + 1).ToArray();
                    //形态 大小
                    int d = 0, x = 0;
                    //形态 奇偶
                    int j = 0, o = 0;
                    //形态 质合
                    int z = 0, h = 0;
                    //冷热 遗漏
                    omission = omission.Select(s => s = s + 1).ToArray();
                    foreach (var item in numbers)
                    {
                        //重号个数
                        if (lastissue.Contains(item))
                            double_sign++;
                        //走势
                        trend_ectype[item - 1] = 0;
                        //形态 大小
                        if (item > 5)
                            d++;
                        else
                            x++;
                        //形态 奇偶
                        if ((item % 2) == 0)
                            o++;
                        else
                            j++;
                        //形态 质合
                        if (h_ectype.Contains(item))
                            h++;
                        else
                            z++;
                        //遗漏
                        omission[item - 1] = 0;
                        //走势
                        if (i <= 30)
                            omission_30[item - 1] += 1;
                        if (i <= 50)
                            omission_50[item - 1] += 1;
                        if (i <= 100)
                            omission_100[item - 1] += 1;
                    }
                    #region 基础最大遗漏
                    base_max_ext = base_max_ext.Select(s => s = s + 1).ToArray();

                    for (int n = 0; n < numbers.Length; n++)
                    {
                        var rm_ext = base_max_ext[numbers[n] - 1] - 1;
                        var rm = base_max[numbers[n] - 1];
                        if (rm_ext > rm)
                            base_max[numbers[n] - 1] = rm_ext;
                        base_max_ext[numbers[n] - 1] = 0;
                        #endregion

                        #region 基础最大连出
                        if (numbers[n] == lastissue[n])
                        {
                            base_max_series_ext[numbers[n] - 1] += 1;
                            if (base_max_series_ext[numbers[n] - 1] > base_max_series[numbers[n] - 1])
                                base_max_series[numbers[n] - 1] = base_max_series_ext[numbers[n] - 1];
                        }
                        else
                            base_max_series_ext[numbers[n] - 1] = 0;
                    }
                    #endregion
                    string[] xt = new string[] { string.Format("{0}:{1}", d, x), string.Format("{0}:{1}", j, o), string.Format("{0}:{1}", z, h) };
                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        s = sum,                 //和值
                        d = double_sign,         //重号
                        t = trend_ectype,        //走势
                        x = xt                   //形态
                    });
                    lastissue = numbers;
                });
                #region 计算
                //基础平均遗漏
                for (int j = 0; j < omission_100.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - omission_100[j]) / (omission_100[j] == 0 ? 1 : omission_100[j]));
                    base_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,
                    o = omission,
                    o3 = omission_30,
                    o5 = omission_50,
                    o0 = omission_100,

                    bn = omission_100,      //基础出现次数
                    ba = base_average,      //基础平均遗漏
                    bm = base_max,          //基础最大遗漏
                    bs = base_max_series,   //基础最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础走势图[任选]：{0}", ex.Message));
            }
        }
        #endregion
        #endregion
    }
}
