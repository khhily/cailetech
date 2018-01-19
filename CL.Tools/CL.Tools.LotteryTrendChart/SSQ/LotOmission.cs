using CL.Entity.Json.Omission;
using CL.Enum.Common;
using CL.Enum.Common.Lottery;
using CL.Game.BLL;
using CL.Game.Entity;
using CL.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Tools.LotteryTrendChart.SSQ
{
    /// <summary>
    /// 双色球基础遗漏数据
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
        Log log = new Log("LotOmission_SSQ");
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
                    case (int)SSQPlayCode.Norm:  //标准
                        Result = this.Norm_Omission(IsuseEntitys);
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
        /// <summary>
        /// 实现冷热遗漏
        /// </summary>
        /// <param name="IsuseName"></param>
        /// <param name="PlayCode"></param>
        /// <param name="TopNumber"></param>
        /// <returns></returns>
        public override string OmissionColdHot(string IsuseName, int PlayCode, int TopCount)
        {
            OmissionResult JsonRec = new OmissionResult();
            try
            {
                List<IsusesEntity> IsuseEntitys = new IsusesBLL().QueryLotTrendChart(LotCode, TopCount, IsuseName).OrderByDescending(o => o.EndTime).ToList();
                //冷热 出现速
                int[] Red_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //红球
                int[] Blue_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //蓝球

                Dictionary<string, List<int[]>> dic = new Dictionary<string, List<int[]>>();
                List<int[]> Omissions = new List<int[]>();

                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    for (int i = 0; i < 6; i++)
                        Red_OmissionNumbers[Numbers[i] - 1] += 1;
                    Blue_OmissionNumbers[Numbers[6] - 1] += 1;
                });
                Omissions.Add(Red_OmissionNumbers);
                Omissions.Add(Blue_OmissionNumbers);
                dic.Add(IssueName, Omissions);
                JsonRec = new OmissionResult()
                {
                    Code = (int)ResultCode.Success,
                    Msg = Enum.Common.Common.GetDescription(ResultCode.Success),
                    Data = Newtonsoft.Json.JsonConvert.SerializeObject(dic)
                };
            }
            catch (Exception ex)
            {
                log.Write(string.Format("实现冷热遗漏错误[Omission]：", ex.StackTrace), true);
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
        /// 标准玩法基础遗漏
        /// 选号：33 + 16
        /// 开奖：6 + 1
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string Norm_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                int[] Red_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //红球
                int[] Blue_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //蓝球
                List<int[]> Omissions = new List<int[]>();
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    int[] Red_Ball = new int[] { Numbers[0], Numbers[1], Numbers[2], Numbers[3], Numbers[4], Numbers[5] };
                    Red_OmissionNumbers = Red_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    Blue_OmissionNumbers = Blue_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    //红球
                    foreach (var val in Red_Ball)
                        Red_OmissionNumbers[val - 1] = 0;
                    //蓝球
                    Blue_OmissionNumbers[Numbers[6] - 1] = 0;
                });
                Omissions.Add(Red_OmissionNumbers);
                Omissions.Add(Blue_OmissionNumbers);
                var obj = new
                {
                    i = IssueName,
                    o = Omissions
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[标准]：{0}", ex.Message));
            }
        }
        #endregion
        #endregion
        #region 基本走势图
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
                    case (int)SSQPlayCode.Norm:  //标准
                        Result = this.TrendChart_Norm(IsuseEntitys, TopCount);
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

        /// <summary>
        /// 基础走势图
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_Norm(List<IsusesEntity> IsuseEntitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //红球走势
                int[] red_trend = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //篮球走势
                int[] blue_trend = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //红球冷热30
                int[] red_30 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //红球冷热50
                int[] red_50 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //红球冷热100
                int[] red_100 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //红球遗漏
                int[] red = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //篮球冷热30
                int[] blue_30 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //篮球冷热50
                int[] blue_50 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //篮球冷热100
                int[] blue_100 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //篮球遗漏
                int[] blue = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //新增输出
                //红球平均遗漏
                int[] red_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] red_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //红球最大遗漏
                int[] red_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //红球最大连出
                int[] red_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] red_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //蓝球最大遗漏
                int[] blue_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] blue_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //蓝球平均遗漏
                int[] blue_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //蓝球最大连出
                int[] blue_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] blue_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


                //上一次开奖号码
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int i = 0;
                IsuseEntitys.ForEach((Entity) =>
                {
                    i++;
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    if (numbers.Length != 7)
                        return;
                    int[] red_num = new int[] { numbers[0], numbers[1], numbers[2], numbers[3], numbers[4], numbers[5] };
                    int blue_num = numbers[6];
                    //走势
                    red_trend = red_trend.Select(s => s = s + 1).ToArray();
                    blue_trend = blue_trend.Select(s => s = s + 1).ToArray();
                    //遗漏
                    red = red.Select(s => s = s + 1).ToArray();
                    blue = blue.Select(s => s = s + 1).ToArray();
                    //最大遗漏
                    red_max_ext = red_max_ext.Select(s => s = s + 1).ToArray();
                    blue_max_ext = blue_max_ext.Select(s => s = s + 1).ToArray();
                    //红球开奖号码
                    int x = 0;
                    foreach (int item in red_num)
                    {
                        //走势
                        red_trend[item - 1] = 0;
                        //遗漏
                        red[item - 1] = 0;
                        //冷热
                        if (i <= 30)
                            red_30[item - 1] += 1;
                        if (i <= 50)
                            red_50[item - 1] += 1;
                        if (i <= 100)
                            red_100[item - 1] += 1;
                        #region 红球最大遗漏
                        var rm_ext = red_max_ext[item - 1] - 1;
                        var rm = red_max[item - 1];
                        if (rm_ext > rm)
                            red_max[item - 1] = rm_ext;
                        red_max_ext[item - 1] = 0;
                        #endregion
                        #region 最大连出
                        var last_item = last_numbers[x];
                        if (item == last_item)
                        {
                            red_max_series_ext[item - 1] += 1;
                            if (red_max_series_ext[item - 1] > red_max_series[item - 1])
                                red_max_series[item - 1] = red_max_series_ext[item - 1];
                        }
                        else
                            red_max_series_ext[item - 1] = 0;
                        #endregion
                        x++;
                    }
                    //蓝球开奖号码 走势
                    blue_trend[blue_num - 1] = 0;
                    //遗漏
                    blue[blue_num - 1] = 0;


                    #region 蓝球最大遗漏
                    var bm_ext = blue_max_ext[blue_num - 1] - 1;
                    var bm = blue_max[blue_num - 1];
                    if (bm_ext > bm)
                        blue_max[blue_num - 1] = bm_ext;
                    blue_max_ext[blue_num - 1] = 0;
                    #endregion


                    #region 最大连出
                    if (blue_num == last_numbers[x])
                    {
                        blue_max_series_ext[blue_num - 1] += 1;
                        if (blue_max_series_ext[blue_num - 1] > blue_max_series[blue_num - 1])
                            blue_max_series[blue_num - 1] = blue_max_series_ext[blue_num - 1];
                    }
                    else
                        blue_max_series_ext[blue_num - 1] = 0;
                    #endregion

                    //冷热
                    if (i <= 30)
                        blue_30[blue_num - 1] += 1;
                    if (i <= 50)
                        blue_50[blue_num - 1] += 1;
                    if (i <= 100)
                        blue_100[blue_num - 1] += 1;
                    trend.Add(new
                    {
                        i = Entity.IsuseName,   //期号
                        rn = red_num,           //开奖号码 红球
                        bn = blue_num,          //开奖号码 篮球
                        rt = red_trend,         //红球走势
                        bt = blue_trend         //蓝球走势
                    });
                    last_numbers = numbers;
                });
                #region 计算
                //红球平均遗漏
                for (int j = 0; j < red_100.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - red_100[j]) / (red_100[j] == 0 ? 1 : red_100[j]));
                    red_average[j] = num;
                }
                //蓝球平均遗漏
                for (int j = 0; j < blue_100.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - blue_100[j]) / (blue_100[j] == 0 ? 1 : blue_100[j]));
                    blue_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,
                    r = red,         //红球遗漏
                    r3 = red_30,     //红球冷热30
                    r5 = red_50,     //红球冷热50
                    r0 = red_100,    //红球冷热100
                    b = blue,        //蓝球遗漏
                    b3 = blue_30,    //蓝球冷热30
                    b5 = blue_50,    //篮球冷热50
                    b0 = blue_100,   //蓝球冷热100

                    rn = red_100,    //红球出现次数
                    ra = red_average,//红球平均遗漏
                    rm = red_max,    //红球最大遗漏
                    rs = red_max_series, //红球最大连出

                    bn = blue_100,   //蓝球出现次数
                    ba = blue_average,//蓝球平均遗漏
                    bm = blue_max,    //蓝球最大遗漏
                    bs = blue_max_series //蓝球最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[标准]：{0}", ex.Message));
            }
        }
        #endregion
    }
}
