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

namespace CL.Tools.LotteryTrendChart.KS
{
    /// <summary>
    /// 快三基础遗漏数据
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
        Log log = new Log("LotOmission_SYYDJ");
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
                    case (int)JLXK3PlayCode.HZ: //和值
                    case (int)JXK3PlayCode.HZ:
                        Result = this.HZ_Omission(IsuseEntitys);
                        break;
                    case (int)JLXK3PlayCode.SBTH: //三不同号
                    case (int)JXK3PlayCode.SBTH:
                    case (int)JLXK3PlayCode.SLHTX: //三连号通选
                    case (int)JXK3PlayCode.SLHTX:
                        Result = this.SBTH_Omission(IsuseEntitys);
                        break;
                    case (int)JLXK3PlayCode.STHDX: //三同号单选
                    case (int)JXK3PlayCode.STHDX:
                    case (int)JLXK3PlayCode.STHTX: //三同号通选
                    case (int)JXK3PlayCode.STHTX:
                        Result = this.STH_Omission(IsuseEntitys);
                        break;
                    case (int)JLXK3PlayCode.ETHDX: //二同号单选
                    case (int)JXK3PlayCode.ETHDX:
                    case (int)JLXK3PlayCode.ETHFX: //二同号复选
                    case (int)JXK3PlayCode.ETHFX:
                        Result = this.ETH_Omission(IsuseEntitys);
                        break;
                    case (int)JLXK3PlayCode.EBTH: //二不同号
                    case (int)JXK3PlayCode.EBTH:
                        Result = this.EBTH_Omission(IsuseEntitys);
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
        /// 和值基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string HZ_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                int[] OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //和值 4-17
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    int val = Numbers.Sum(s => s);
                    if (val > 3 && val < 18)
                    {
                        OmissionNumbers = OmissionNumbers.Select(s => s = s + 1).ToArray();
                        OmissionNumbers[val - 4] = 0;
                    }
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
                throw new Exception(string.Format("基础遗漏错误[和值]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 三不同号,三连号基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string SBTH_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                int[] BTH_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0 }; //三不同号单选
                int[] LH_OmissionNumbers = new int[] { 0 }; //三连号通选
                List<int[]> Omissions = Omissions = new List<int[]>(); //集合
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    //三不同号
                    BTH_OmissionNumbers = BTH_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    foreach (int val in Numbers)
                        BTH_OmissionNumbers[val - 1] = 0;
                    //三连号
                    LH_OmissionNumbers = LH_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    if (Numbers[1] == (Numbers[0] + 1) && Numbers[2] == (Numbers[1] + 1))
                        LH_OmissionNumbers[0] = 0;
                });
                Omissions.Add(BTH_OmissionNumbers);
                Omissions.Add(LH_OmissionNumbers);
                var obj = new
                {
                    i = IssueName,
                    o = Omissions
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[三不同号三连号通选]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 三同号单选三同号通选基础遗漏
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string STH_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                int[] DX_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0 }; //同号单选
                int[] TX_OmissionNumbers = new int[] { 0 }; //同号通选
                List<int[]> Omissions = Omissions = new List<int[]>(); //集合
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    int[] Same = Numbers.GroupBy(g => g).Select(s => s.Key).ToArray();
                    DX_OmissionNumbers = DX_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    TX_OmissionNumbers = TX_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    if (Same.Length == 1) //三同号
                    {
                        DX_OmissionNumbers[Same[0] - 1] = 0;
                        TX_OmissionNumbers[0] = 0;
                    }
                });
                Omissions.Add(DX_OmissionNumbers);
                Omissions.Add(TX_OmissionNumbers);
                var obj = new
                {
                    i = IssueName,
                    o = Omissions
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[三同号单选三同号通选]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 二同号单选二同号复选
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string ETH_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                int[] DX_TH_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0 }; //同号单选 同号
                int[] DX_BTH_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0 }; //同号单选 不同号
                int[] FX_OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0 }; //同号单选
                List<int[]> Omissions = Omissions = new List<int[]>(); //集合
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    DX_TH_OmissionNumbers = DX_TH_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    DX_BTH_OmissionNumbers = DX_BTH_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    FX_OmissionNumbers = FX_OmissionNumbers.Select(s => s = s + 1).ToArray();
                    int[] Same = Numbers.GroupBy(g => g).Select(s => s.Key).ToArray();
                    if (Same.Length == 2) //二同号
                    {
                        if (Numbers.Where(w => w == Same[0]).ToArray().Length == 2)
                        {
                            DX_TH_OmissionNumbers[Same[0] - 1] = 0;//同号
                            FX_OmissionNumbers[Same[0] - 1] = 0;//同号
                            DX_BTH_OmissionNumbers[Same[1] - 1] = 0;//不同号
                        }
                        else
                        {
                            DX_TH_OmissionNumbers[Same[1] - 1] = 0;//同号
                            FX_OmissionNumbers[Same[1] - 1] = 0;//同号
                            DX_BTH_OmissionNumbers[Same[0] - 1] = 0;//不同号
                        }
                    }
                });
                Omissions.Add(DX_TH_OmissionNumbers);
                Omissions.Add(DX_BTH_OmissionNumbers);
                Omissions.Add(FX_OmissionNumbers);
                var obj = new
                {
                    i = IssueName,
                    o = Omissions
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[二同号单选二同号复选]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 二不同号
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string EBTH_Omission(List<IsusesEntity> IsuseEntitys)
        {
            try
            {
                int[] OmissionNumbers = new int[] { 0, 0, 0, 0, 0, 0 }; //二不同号
                IsuseEntitys.ForEach((item) =>
                {
                    int[] Numbers = item.OpenNumber.Split(SEP_NUM_A).Select(s => Convert.ToInt32(s)).ToArray();
                    OmissionNumbers = OmissionNumbers.Select(s => s = s + 1).ToArray();
                    foreach (var val in Numbers)
                        OmissionNumbers[val - 1] = 0;
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
                throw new Exception(string.Format("基础遗漏错误[二不同号]：{0}", ex.Message));
            }
        }
        #endregion
        #endregion

        #region 基础走势图
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
                string Result = string.Empty;
                List<IsusesEntity> IsuseEntitys = new IsusesBLL().QueryLotTrendChart(LotCode, TopCount, IsuseName).OrderBy(o => o.IsuseName).ToList();
                switch (PlayCode)
                {
                    case (int)JLXK3PlayCode.HZ: //和值
                    case (int)JXK3PlayCode.HZ:
                        Result = this.TrendChart_Sum(IsuseEntitys, TopCount);
                        break;
                    case (int)JLXK3PlayCode.SBTH: //三不同号
                    case (int)JXK3PlayCode.SBTH:
                    case (int)JLXK3PlayCode.SLHTX: //三连号通选
                    case (int)JXK3PlayCode.SLHTX:
                        Result = this.TrendChart_SBT(IsuseEntitys, TopCount);
                        break;
                    case (int)JLXK3PlayCode.STHDX: //三同号单选
                    case (int)JXK3PlayCode.STHDX:
                    case (int)JLXK3PlayCode.STHTX: //三同号通选
                    case (int)JXK3PlayCode.STHTX:
                        Result = this.TrendChart_STH(IsuseEntitys, TopCount);
                        break;
                    case (int)JLXK3PlayCode.ETHDX: //二同号单选
                    case (int)JXK3PlayCode.ETHDX:
                    case (int)JLXK3PlayCode.ETHFX: //二同号复选
                    case (int)JXK3PlayCode.ETHFX:
                        Result = this.TrendChart_ETH(IsuseEntitys, TopCount);
                        break;
                    case (int)JLXK3PlayCode.EBTH: //二不同号
                    case (int)JXK3PlayCode.EBTH:
                        Result = this.TrendChart_EBT(IsuseEntitys, TopCount);
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
                log.Write(string.Format("实现基础走势图[Basics_TrendChart]：", ex.StackTrace), true);
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
        /// 通用
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_Default(List<IsusesEntity> IsuseEntitys)
        {
            try
            {

                List<object> trend = new List<object>();
                int[] spacings_arr = new int[] { 0, 0, 0 };
                IsuseEntitys.ForEach((Entity) =>
                {
                    //开奖号码数组
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    #region 和值计算
                    int sum = numbers.Sum(s => s);
                    #endregion
                    #region 跨度计算
                    int max = numbers.OrderByDescending(o => o).FirstOrDefault();
                    int min = numbers.OrderBy(o => o).FirstOrDefault();
                    int span = max - min;
                    #endregion
                    #region 重号
                    int spacings = 0;
                    foreach (int item in numbers)
                        if (spacings_arr.Contains(item))
                            spacings = spacings + 1;
                    #endregion
                    #region 形态
                    int[] nums = numbers.GroupBy(g => g).Select(s => s.Key).ToArray();
                    string from = string.Empty;
                    switch (nums.Length)
                    {
                        case 1:
                            from = "三同号";
                            break;
                        case 2:
                            from = "二同号";
                            break;
                        case 3:
                            if ((nums[0] + 1 == nums[1]) && (nums[1] + 1 == nums[2]))
                                from = "三连号";
                            else
                                from = "三不同号";
                            break;
                    }
                    #endregion
                    trend.Add(new
                    {
                        i = Entity.IsuseName, //期号
                        nb = numbers,         //开奖号码
                        s = sum,              //和值
                        sp = span,            //跨度
                        sc = spacings,        //重号
                        f = from              //形态
                    });
                    spacings_arr = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray(); //用于下一期计算重号
                });
                return Newtonsoft.Json.JsonConvert.SerializeObject(trend);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础遗漏错误[和值]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 和值基础走势图
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_Sum(List<IsusesEntity> IsuseEntitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //冷热遗漏
                int[] Omission = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热30期
                int[] Omission_30 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热50期
                int[] Omission_50 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //冷热100期
                int[] Omission_100 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //和值平均遗漏
                int[] sum_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //和值最大遗漏
                int[] sum_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] sum_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //和值最大连出
                int[] sum_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] sum_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //基础走势
                int[] base_trend = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] Omission_base = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础平均遗漏
                int[] base_average = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础最大遗漏
                int[] base_max = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] base_max_ext = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础最大连出
                int[] base_max_series = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] base_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0 };

                int i = 0;
                //上一次开奖号码
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                IsuseEntitys.ForEach((Entity) =>
                {
                    i++;
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //和值
                    int sum = numbers.Sum(s => s);
                    //大小
                    string d_x = sum > 10 ? "大" : "小";
                    //单双
                    string d_s = (sum % 2) == 0 ? "双" : "单";
                    //跨度
                    int max = numbers.OrderByDescending(o => o).FirstOrDefault();
                    int min = numbers.OrderBy(o => o).FirstOrDefault();
                    int span = max - min;
                    //30期冷热
                    if (i <= 30)
                        Omission_30[sum - 3] += 1;

                    //50期冷热
                    if (i <= 50)
                        Omission_50[sum - 3] += 1;
                    //100期冷热
                    Omission_100[sum - 3] += 1;

                    //遗漏
                    Omission = Omission.Select(s => s = s + 1).ToArray();
                    Omission[sum - 3] = 0;


                    #region 计算和值走势
                    sum_max_ext = sum_max_ext.Select(s => s = s + 1).ToArray();


                    #region 基本最大遗漏
                    var rmsum_ext = sum_max_ext[sum - 3] - 1;
                    var rmsum = sum_max[sum - 3];
                    if (rmsum_ext > rmsum)
                        sum_max[sum - 3] = rmsum_ext;
                    sum_max_ext[sum - 3] = 0;
                    #endregion

                    #endregion

                    #region 计算基本走势
                    base_trend = base_trend.Select(s => s = s + 1).ToArray();
                    base_max_ext = base_max_ext.Select(s => s = s + 1).ToArray();
                    for (int j = 0; j < numbers.Length; j++)
                    {
                        Omission_base[numbers[j] - 1] += 1;
                        base_trend[numbers[j] - 1] = 0;

                        #region 基本最大遗漏
                        var rm_ext = base_max_ext[numbers[j] - 1] - 1;
                        var rm = base_max[numbers[j] - 1];
                        if (rm_ext > rm)
                            base_max[numbers[j] - 1] = rm_ext;
                        base_max_ext[numbers[j] - 1] = 0;
                        #endregion
                        #region 基本最大连出
                        var last_item = last_numbers[j];
                        if (numbers[j] == last_item)
                        {
                            base_max_series_ext[numbers[j] - 1] += 1;
                            if (base_max_series_ext[numbers[j] - 1] > base_max_series[numbers[j] - 1])
                                base_max_series[numbers[j] - 1] = base_max_series_ext[numbers[j] - 1];
                        }
                        else
                            base_max_series_ext[numbers[j] - 1] = 0;
                        #endregion
                    }

                    #endregion
                    last_numbers = numbers;
                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        s = sum,                 //和值
                        dx = d_x,                //大小
                        ds = d_s,                //单双
                        sp = span,               //跨度
                        t = Omission,            //走势
                        bt = base_trend,         //基础走势
                    });
                });
                #region 计算
                //基础平均遗漏
                for (int j = 0; j < Omission_base.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - Omission_base[j]) / (Omission_base[j] == 0 ? 1 : Omission_base[j]));
                    base_average[j] = num;
                }
                //和值平均遗漏
                for (int j = 0; j < Omission_100.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - Omission_100[j]) / (Omission_100[j] == 0 ? 1 : Omission_100[j]));
                    sum_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,             //基础信息
                    o = Omission,           //遗漏
                    o3 = Omission_30,       //冷热30期
                    o5 = Omission_50,       //冷热50期
                    o0 = Omission_100,      //冷热100期

                    bn = Omission_base,     //基础出现次数
                    ba = base_average,      //基础平均遗漏
                    bm = base_max,          //基础最大遗漏
                    bs = base_max_series,   //基础最大连出

                    sn = Omission_100,     //和值出现次数
                    sa = sum_average,      //和值平均遗漏
                    sm = sum_max,          //和值最大遗漏
                    ss = sum_max_series,   //和值最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础走势图[和值]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 三同号基础走势图
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_STH(List<IsusesEntity> IsuseEntitys, int TopCount)
        {
            try
            {
                int[] Omission = new int[] { 0, 0, 0, 0, 0, 0 };
                List<object> trend = new List<object>();
                int[] xts = new int[] { 0, 0, 0, 0 };// 形态走势 下标对应：三同号,三不同号,二同号,二不同号

                //基础走势
                int[] Omission_base = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础平均遗漏
                int[] base_average = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础最大遗漏
                int[] base_max = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] base_max_ext = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础最大连出
                int[] base_max_series = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] base_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0 };

                //形态走势
                int[] Omission_xts = new int[] { 0, 0, 0, 0 };
                //形态平均遗漏
                int[] xts_average = new int[] { 0, 0, 0, 0 };
                //形态最大遗漏
                int[] xts_max = new int[] { 0, 0, 0, 0 };
                int[] xts_max_ext = new int[] { 0, 0, 0, 0 };
                //形态最大连出
                int[] xts_max_series = new int[] { 0, 0, 0, 0 };
                int[] xts_max_series_ext = new int[] { 0, 0, 0, 0 };




                //上一次开奖号码
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                IsuseEntitys.ForEach((Entity) =>
                {
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //和值
                    int sum = numbers.Sum(s => s);
                    //跨度
                    int max = numbers.OrderByDescending(o => o).FirstOrDefault();
                    int min = numbers.OrderBy(o => o).FirstOrDefault();
                    int span = max - min;
                    //形态
                    string xt = string.Empty; //形态
                    xts = xts.Select(s => s = s + 1).ToArray();
                    int[] nums = numbers.GroupBy(g => g).Select(s => s.Key).ToArray();

                    //最大遗漏
                    xts_max_ext = xts_max_ext.Select(s => s = s + 1).ToArray();
                    base_max_ext = base_max_ext.Select(s => s = s + 1).ToArray();
                    switch (nums.Length)
                    {
                        case 1: //三同号
                            xts[0] = 0;
                            xts[2] = 0;
                            Omission_xts[0] += 1;
                            Omission_xts[2] += 1;
                            xt = "三同号";

                            #region 形态最大遗漏
                            var rm_ext_1 = base_max_ext[0] - 1;
                            var rm_1 = base_max[0];
                            if (rm_ext_1 > rm_1)
                                base_max[0] = rm_ext_1;
                            base_max_ext[0] = 0;

                            rm_ext_1 = base_max_ext[2] - 1;
                            rm_1 = base_max[2];
                            if (rm_ext_1 > rm_1)
                                base_max[2] = rm_ext_1;
                            base_max_ext[2] = 0;
                            #endregion

                            #region 形态最大连出
                            if (last_numbers.GroupBy(g => g).Select(s => s.Key).ToArray().Length == 1)
                            {
                                xts_max_series_ext[0] += 1;
                                if (xts_max_series_ext[0] > xts_max_series[0])
                                    xts_max_series[0] = xts_max_series_ext[0];

                                xts_max_series_ext[2] += 1;
                                if (xts_max_series_ext[2] > xts_max_series[2])
                                    xts_max_series[2] = xts_max_series_ext[2];
                            }
                            else
                            {
                                xts_max_series_ext[0] = 0;
                                xts_max_series_ext[2] = 0;
                            }
                            #endregion
                            break;
                        case 2: //二同号
                            xts[2] = 0;
                            xts[3] = 0;
                            Omission_xts[2] += 1;
                            Omission_xts[3] += 1;
                            xt = "二同号";

                            #region 形态最大遗漏
                            var rm_ext_2 = base_max_ext[3] - 1;
                            var rm_2 = base_max[3];
                            if (rm_ext_2 > rm_2)
                                base_max[3] = rm_ext_2;
                            base_max_ext[3] = 0;

                            rm_ext_2 = base_max_ext[2] - 1;
                            rm_2 = base_max[2];
                            if (rm_ext_2 > rm_2)
                                base_max[2] = rm_ext_2;
                            base_max_ext[2] = 0;
                            #endregion

                            #region 形态最大连出
                            if (last_numbers.GroupBy(g => g).Select(s => s.Key).ToArray().Length == 2)
                            {
                                xts_max_series_ext[3] += 1;
                                if (xts_max_series_ext[3] > xts_max_series[3])
                                    xts_max_series[3] = xts_max_series_ext[3];

                                xts_max_series_ext[2] += 1;
                                if (xts_max_series_ext[2] > xts_max_series[2])
                                    xts_max_series[2] = xts_max_series_ext[2];
                            }
                            else
                            {
                                xts_max_series_ext[3] = 0;
                                xts_max_series_ext[2] = 0;
                            }
                            #endregion
                            break;
                        case 3: //三不同号  二不同号  三连号
                            xts[1] = 0;
                            xts[3] = 0;
                            Omission_xts[1] += 1;
                            Omission_xts[3] += 1;
                            if ((nums[0] + 1) == nums[1] && (nums[1] + 1) == nums[2])
                                xt = "三连号";
                            else
                                xt = "三不同号";

                            #region 形态最大遗漏
                            var rm_ext_3 = base_max_ext[3] - 1;
                            var rm_3 = base_max[3];
                            if (rm_ext_3 > rm_3)
                                base_max[3] = rm_ext_3;
                            base_max_ext[3] = 0;

                            rm_ext_3 = base_max_ext[1] - 1;
                            rm_3 = base_max[1];
                            if (rm_ext_3 > rm_3)
                                base_max[1] = rm_ext_3;
                            base_max_ext[1] = 0;
                            #endregion

                            #region 形态最大连出
                            if (last_numbers.GroupBy(g => g).Select(s => s.Key).ToArray().Length == 2)
                            {
                                xts_max_series_ext[3] += 1;
                                if (xts_max_series_ext[3] > xts_max_series[3])
                                    xts_max_series[3] = xts_max_series_ext[3];

                                xts_max_series_ext[1] += 1;
                                if (xts_max_series_ext[1] > xts_max_series[1])
                                    xts_max_series[1] = xts_max_series_ext[1];
                            }
                            else
                            {
                                xts_max_series_ext[3] = 0;
                                xts_max_series_ext[1] = 0;
                            }
                            #endregion
                            break;
                    }
                    //基本走势
                    Omission = Omission.Select(s => s = s + 1).ToArray();
                    foreach (var item in nums)
                        Omission[item - 1] = 0;

                    #region 计算基本走势
                    base_max_ext = base_max_ext.Select(s => s = s + 1).ToArray();
                    last_numbers = last_numbers.GroupBy(g => g).Select(s => s.Key).ToArray();
                    for (int j = 0; j < nums.Length; j++)
                    {
                        Omission_base[nums[j] - 1] += 1;

                        #region 基本最大遗漏
                        var rm_ext = base_max_ext[nums[j] - 1] - 1;
                        var rm = base_max[nums[j] - 1];
                        if (rm_ext > rm)
                            base_max[nums[j] - 1] = rm_ext;
                        base_max_ext[nums[j] - 1] = 0;
                        #endregion
                        #region 基本最大连出
                        if (nums.Length == last_numbers.Length)
                        {
                            var last_item = last_numbers[j];
                            if (nums[j] == last_item)
                            {
                                base_max_series_ext[nums[j] - 1] += 1;
                                if (base_max_series_ext[nums[j] - 1] > base_max_series[nums[j] - 1])
                                    base_max_series[nums[j] - 1] = base_max_series_ext[nums[j] - 1];
                            }
                            else
                                base_max_series_ext[nums[j] - 1] = 0;
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
                        x = xt,                  //形态
                        s = sum,                 //和值
                        t = Omission,            //基本走势
                        z = xts,                 //形态走势 下标对应：三同号,三不同号,二同号,二不同号
                    });
                });
                #region 计算
                //基础平均遗漏
                for (int j = 0; j < Omission_base.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - Omission_base[j]) / (Omission_base[j] == 0 ? 1 : Omission_base[j]));
                    base_average[j] = num;
                }
                //形态平均遗漏
                for (int j = 0; j < Omission_xts.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - Omission_xts[j]) / (Omission_xts[j] == 0 ? 1 : Omission_xts[j]));
                    xts_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,             //基础信息

                    bn = Omission_base,     //基础出现次数
                    ba = base_average,      //基础平均遗漏
                    bm = base_max,          //基础最大遗漏
                    bs = base_max_series,   //基础最大连出

                    sn = Omission_xts,     //形态出现次数
                    sa = xts_average,      //形态平均遗漏
                    sm = xts_max,          //形态最大遗漏
                    ss = xts_max_series,   //形态最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础走势图[三同号]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 二同号基础走势图
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_ETH(List<IsusesEntity> IsuseEntitys, int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                int[] Omission = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] fb = new int[] { 0, 0, 0, 0, 0, 0 };//号码分布

                //基础走势
                int[] Omission_base = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础平均遗漏
                int[] base_average = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础最大遗漏
                int[] base_max = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] base_max_ext = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础最大连出
                int[] base_max_series = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] base_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0 };

                //形态走势
                int[] Omission_fb = new int[] { 0, 0, 0, 0, 0, 0 };
                //形态平均遗漏
                int[] fb_average = new int[] { 0, 0, 0, 0, 0, 0 };
                //形态最大遗漏
                int[] fb_max = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] fb_max_ext = new int[] { 0, 0, 0, 0, 0, 0 };
                //形态最大连出
                int[] fb_max_series = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] fb_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0 };

                //上一次开奖号码
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0, 0, 0 };

                IsuseEntitys.ForEach((Entity) =>
                {
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //和值
                    int sum = numbers.Sum(s => s);
                    //跨度
                    int max = numbers.OrderByDescending(o => o).FirstOrDefault();
                    int min = numbers.OrderBy(o => o).FirstOrDefault();
                    int span = max - min;
                    //形态
                    string xt = string.Empty;
                    int[] nums = numbers.GroupBy(g => g).Select(s => s.Key).ToArray();
                    //基本走势
                    Omission = Omission.Select(s => s = s + 1).ToArray();
                    foreach (var item in nums)
                        Omission[item - 1] = 0;

                    //最大遗漏
                    fb_max_ext = fb_max_ext.Select(s => s = s + 1).ToArray();
                    base_max_ext = base_max_ext.Select(s => s = s + 1).ToArray();
                    //基础
                    #region 计算基本走势
                    base_max_ext = base_max_ext.Select(s => s = s + 1).ToArray();
                    last_numbers = last_numbers.GroupBy(g => g).Select(s => s.Key).ToArray();
                    for (int j = 0; j < nums.Length; j++)
                    {
                        Omission_base[nums[j] - 1] += 1;

                        #region 基本最大遗漏
                        var rm_ext = base_max_ext[nums[j] - 1] - 1;
                        var rm = base_max[nums[j] - 1];
                        if (rm_ext > rm)
                            base_max[nums[j] - 1] = rm_ext;
                        base_max_ext[nums[j] - 1] = 0;
                        #endregion
                        #region 基本最大连出
                        if (nums.Length == last_numbers.Length)
                        {
                            var last_item = last_numbers[j];
                            if (nums[j] == last_item)
                            {
                                base_max_series_ext[nums[j] - 1] += 1;
                                if (base_max_series_ext[nums[j] - 1] > base_max_series[nums[j] - 1])
                                    base_max_series[nums[j] - 1] = base_max_series_ext[nums[j] - 1];
                            }
                            else
                                base_max_series_ext[nums[j] - 1] = 0;
                        }
                        #endregion
                    }
                    #endregion


                    //号码分布
                    fb = fb.Select(s => s = s + 1).ToArray();
                    switch (nums.Length)
                    {
                        case 1: //三同号
                            xt = "三同号";
                            break;
                        case 2: //二同号
                            xt = "二同号";
                            //计算号码分布
                            if ((numbers.Where(w => w == nums[0]).ToArray().Length) > 1)
                            {
                                fb[nums[0] - 1] = 0;
                                Omission_fb[nums[0] - 1] += 1;

                                #region 形态最大遗漏
                                var rm_ext = fb_max_ext[nums[0] - 1] - 1;
                                var rm = fb_max[nums[0] - 1];
                                if (rm_ext > rm)
                                    fb_max[nums[0] - 1] = rm_ext;
                                fb_max_ext[nums[0] - 1] = 0;
                                #endregion

                                #region 形态最大连出
                                if (last_numbers.GroupBy(g => g).Select(s => s.Key).ToArray().Length == 2 && (last_numbers.Where(w => w == nums[0]).ToArray().Length) > 1)
                                {
                                    fb_max_series_ext[nums[0] - 1] += 1;
                                    if (fb_max_series_ext[nums[0] - 1] > fb_max_series[nums[0] - 1])
                                        fb_max_series[nums[0] - 1] = fb_max_series_ext[nums[0] - 1];
                                }
                                else
                                    fb_max_series_ext[nums[0] - 1] = 0;
                                #endregion
                            }
                            else
                            {
                                fb[nums[1] - 1] = 0;
                                Omission_fb[nums[1] - 1] += 1;

                                #region 形态最大遗漏
                                var rm_ext = fb_max_ext[nums[1] - 1] - 1;
                                var rm = fb_max[nums[1] - 1];
                                if (rm_ext > rm)
                                    fb_max[nums[1] - 1] = rm_ext;
                                fb_max_ext[nums[1] - 1] = 0;
                                #endregion

                                #region 形态最大连出
                                if (last_numbers.GroupBy(g => g).Select(s => s.Key).ToArray().Length == 2 && (last_numbers.Where(w => w == nums[1]).ToArray().Length) > 1)
                                {
                                    fb_max_series_ext[nums[1] - 1] += 1;
                                    if (fb_max_series_ext[nums[1] - 1] > fb_max_series[nums[1] - 1])
                                        fb_max_series[nums[1] - 1] = fb_max_series_ext[nums[1] - 1];
                                }
                                else
                                    fb_max_series_ext[nums[1] - 1] = 0;
                                #endregion
                            }
                            break;
                        case 3: //三不同号  二不同号  三连号
                            if ((nums[0] + 1) == nums[1] && (nums[1] + 1) == nums[2])
                                xt = "三连号";
                            else
                                xt = "三不同号";
                            break;
                    }


                    last_numbers = numbers;
                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        x = xt,                  //形态
                        s = sum,                 //和值
                        t = Omission,            //基本走势
                        f = fb                   //号码分布
                    });
                });
                #region 计算
                //基础平均遗漏
                for (int j = 0; j < Omission_base.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - Omission_base[j]) / (Omission_base[j] == 0 ? 1 : Omission_base[j]));
                    base_average[j] = num;
                }
                //号码分布平均遗漏
                for (int j = 0; j < Omission_fb.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - Omission_fb[j]) / (Omission_fb[j] == 0 ? 1 : Omission_fb[j]));
                    fb_average[j] = num;
                }
                #endregion

                var obj = new
                {
                    tr = trend,             //基础信息

                    bn = Omission_base,     //基础出现次数
                    ba = base_average,      //基础平均遗漏
                    bm = base_max,          //基础最大遗漏
                    bs = base_max_series,   //基础最大连出

                    sn = Omission_fb,     //号码分布出现次数
                    sa = fb_average,      //号码分布形态平均遗漏
                    sm = fb_max,          //号码分布形态最大遗漏
                    ss = fb_max_series,   //号码分布形态最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础走势图[二同号]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 二不同号基础走势图
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_EBT(List<IsusesEntity> IsuseEntitys,int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                //号码分布下标：12,13,14,15,16,23,24,25,26,34,35,36,45,46,56
                int[] fb = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] fb_ectype = new int[] { 12, 13, 14, 15, 16, 23, 24, 25, 26, 34, 35, 36, 45, 46, 56 };
                //遗漏
                int[] Omission = new int[] { 0, 0, 0, 0, 0, 0 };
                //冷热30
                int[] Omission_30 = new int[] { 0, 0, 0, 0, 0, 0 };
                //冷热50
                int[] Omission_50 = new int[] { 0, 0, 0, 0, 0, 0 };
                //冷热100
                int[] Omission_100 = new int[] { 0, 0, 0, 0, 0, 0 };


                //基础平均遗漏
                int[] base_average = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础最大遗漏
                int[] base_max = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] base_max_ext = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础最大连出
                int[] base_max_series = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] base_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0 };

                //号码分布走势
                int[] Omission_fb = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //号码分布平均遗漏
                int[] fb_average = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //号码分布最大遗漏
                int[] fb_max = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] fb_max_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //号码分布最大连出
                int[] fb_max_series = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] fb_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


                //上一次开奖号码
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                int i = 0;
                IsuseEntitys.ForEach((Entity) =>
                {
                    i++;
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //和值
                    int sum = numbers.Sum(s => s);
                    //跨度
                    int max = numbers.OrderByDescending(o => o).FirstOrDefault();
                    int min = numbers.OrderBy(o => o).FirstOrDefault();
                    int span = max - min;
                    //形态
                    string xt = string.Empty;
                    fb = fb.Select(s => s = s + 1).ToArray();
                    int[] nums = numbers.GroupBy(g => g).Select(s => s.Key).OrderBy(o => o).ToArray();

                    fb_max_ext = fb_max_ext.Select(s => s = s + 1).ToArray();
                    base_max_ext = base_max_ext.Select(s => s = s + 1).ToArray();

                    if (nums.Length == 2)
                    {
                        var item = Array.IndexOf(fb_ectype, Convert.ToInt32(string.Format("{0}{1}", nums[0], nums[1])));
                        Omission_fb[item] += 1;
                        fb[item] = 0;

                        #region 形态最大遗漏
                        var rm_ext = fb_max_ext[item] - 1;
                        var rm = fb_max[item];
                        if (rm_ext > rm)
                            fb_max[item] = rm_ext;
                        fb_max_ext[item] = 0;
                        #endregion

                        #region 形态最大连出
                        int[] nums_ext = last_numbers.GroupBy(g => g).Select(s => s.Key).OrderBy(o => o).ToArray();
                        if (nums_ext.Length == 2)
                        {
                            var item_ext = Array.IndexOf(fb_ectype, Convert.ToInt32(string.Format("{0}{1}", nums_ext[0], nums_ext[1])));
                            if (item == item_ext)
                            {
                                fb_max_series_ext[item] += 1;
                                if (fb_max_series_ext[item] > fb_max_series[item])
                                    fb_max_series[item] = fb_max_series_ext[item];
                            }
                            else
                                fb_max_series_ext[item] = 0;
                        }
                        #endregion
                    }

                    switch (nums.Length)
                    {
                        case 1: //三同号
                            xt = "三同号";
                            break;
                        case 2: //二同号
                            xt = "二同号";

                            break;
                        case 3: //三不同号  二不同号  三连号
                            if ((nums[0] + 1) == nums[1] && (nums[1] + 1) == nums[2])
                                xt = "三连号";
                            else
                                xt = "三不同号";
                            break;
                    }
                    Omission = Omission.Select(s => s = s + 1).ToArray();

                    for (int j = 0; j < nums.Length; j++)
                    {
                        var item = nums[j];

                        //30期冷热
                        if (i <= 30)
                            Omission_30[item - 1] += 1;
                        //50期冷热
                        if (i <= 50)
                            Omission_50[item - 1] += 1;
                        //100期冷热
                        Omission_100[item - 1] += 1;
                        //遗漏
                        Omission[item - 1] = 0;

                        #region 形态最大遗漏
                        var rm_ext = base_max_ext[item - 1] - 1;
                        var rm = base_max[item - 1];
                        if (rm_ext > rm)
                            base_max[item - 1] = rm_ext;
                        base_max_ext[item - 1] = 0;
                        #endregion

                        #region 形态最大连出
                        var item_ext = last_numbers.GroupBy(g => g).Select(s => s.Key).OrderBy(o => o).ToArray();
                        if (nums.Length == item_ext.Length && item == item_ext[j])
                        {
                            base_max_series_ext[item-1] += 1;
                            if (base_max_series_ext[item-1] > base_max_series[item-1])
                                base_max_series[item-1] = base_max_series_ext[item-1];
                        }
                        else
                            base_max_series_ext[item-1] = 0;
                        #endregion
                    }


                    last_numbers = numbers;
                    trend.Add(new
                    {
                        i = Entity.IsuseName,    //期号
                        n = numbers,             //开奖号码
                        sp = span,               //跨度
                        x = xt,                  //形态
                        s = sum,                 //和值
                        t = Omission,            //基本走势
                        f = fb                   //号码分布下标：12,13,14,15,16,23,24,25,26,34,35,36,45,46,56
                    });
                });
                #region 计算
                //基础平均遗漏
                for (int j = 0; j < Omission_100.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - Omission_100[j]) / (Omission_100[j] == 0 ? 1 : Omission_100[j]));
                    base_average[j] = num;
                }
                //号码分布平均遗漏
                for (int j = 0; j < Omission_fb.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - Omission_fb[j]) / (Omission_fb[j] == 0 ? 1 : Omission_fb[j]));
                    fb_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,
                    o = Omission,
                    o3 = Omission_30,
                    o5 = Omission_50,
                    o0 = Omission_100,

                    bn = Omission_100,      //基础出现次数
                    ba = base_average,      //基础平均遗漏
                    bm = base_max,          //基础最大遗漏
                    bs = base_max_series,   //基础最大连出

                    sn = Omission_fb,     //号码分布出现次数
                    sa = fb_average,      //号码分布形态平均遗漏
                    sm = fb_max,          //号码分布形态最大遗漏
                    ss = fb_max_series    //号码分布形态最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础走势图[二不同号]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 三不同号基础走势图
        /// </summary>
        /// <param name="IsuseEntitys"></param>
        /// <returns></returns>
        public string TrendChart_SBT(List<IsusesEntity> IsuseEntitys,int TopCount)
        {
            try
            {
                List<object> trend = new List<object>();
                // 形态走势 下标对应：三同号,三不同号,二同号,二不同号
                int[] xts = new int[] { 0, 0, 0, 0 };
                //遗漏
                int[] Omission = new int[] { 0, 0, 0, 0, 0, 0 };
                //冷热30
                int[] Omission_30 = new int[] { 0, 0, 0, 0, 0, 0 };
                //冷热50
                int[] Omission_50 = new int[] { 0, 0, 0, 0, 0, 0 };
                //冷热100
                int[] Omission_100 = new int[] { 0, 0, 0, 0, 0, 0 };
                
                //基础平均遗漏
                int[] base_average = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础最大遗漏
                int[] base_max = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] base_max_ext = new int[] { 0, 0, 0, 0, 0, 0 };
                //基础最大连出
                int[] base_max_series = new int[] { 0, 0, 0, 0, 0, 0 };
                int[] base_max_series_ext = new int[] { 0, 0, 0, 0, 0, 0 };

                //形态走势
                int[] Omission_xts = new int[] { 0, 0, 0, 0 };
                //形态平均遗漏
                int[] xts_average = new int[] { 0, 0, 0, 0 };
                //形态最大遗漏
                int[] xts_max = new int[] { 0, 0, 0, 0 };
                int[] xts_max_ext = new int[] { 0, 0, 0, 0 };
                //形态最大连出
                int[] xts_max_series = new int[] { 0, 0, 0, 0 };
                int[] xts_max_series_ext = new int[] { 0, 0, 0, 0 };


                //上一次开奖号码
                int[] last_numbers = new int[] { 0, 0, 0, 0, 0, 0, 0 };

                int i = 0;
                IsuseEntitys.ForEach((Entity) =>
                {
                    i++;
                    //开奖号码
                    int[] numbers = Entity.OpenNumber.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                    //和值
                    int sum = numbers.Sum(s => s);
                    //跨度
                    int max = numbers.OrderByDescending(o => o).FirstOrDefault();
                    int min = numbers.OrderBy(o => o).FirstOrDefault();
                    int span = max - min;
                    //形态
                    string xt = string.Empty; //形态
                    xts = xts.Select(s => s = s + 1).ToArray();
                    int[] nums = numbers.GroupBy(g => g).Select(s => s.Key).ToArray();
                    base_max_ext = base_max_ext.Select(s => s = s + 1).ToArray();
                    switch (nums.Length)
                    {
                        case 1: //三同号
                            xts[0] = 0;
                            xts[2] = 0;
                            xt = "三同号";

                            #region 形态最大遗漏
                            var rm_ext_1 = base_max_ext[0] - 1;
                            var rm_1 = base_max[0];
                            if (rm_ext_1 > rm_1)
                                base_max[0] = rm_ext_1;
                            base_max_ext[0] = 0;

                            rm_ext_1 = base_max_ext[2] - 1;
                            rm_1 = base_max[2];
                            if (rm_ext_1 > rm_1)
                                base_max[2] = rm_ext_1;
                            base_max_ext[2] = 0;
                            #endregion

                            #region 形态最大连出
                            if (last_numbers.GroupBy(g => g).Select(s => s.Key).ToArray().Length == 1)
                            {
                                xts_max_series_ext[0] += 1;
                                if (xts_max_series_ext[0] > xts_max_series[0])
                                    xts_max_series[0] = xts_max_series_ext[0];

                                xts_max_series_ext[2] += 1;
                                if (xts_max_series_ext[2] > xts_max_series[2])
                                    xts_max_series[2] = xts_max_series_ext[2];
                            }
                            else
                            {
                                xts_max_series_ext[0] = 0;
                                xts_max_series_ext[2] = 0;
                            }
                            #endregion
                            break;
                        case 2: //二同号
                            xts[2] = 0;
                            xts[3] = 0;
                            xt = "二同号";

                            #region 形态最大遗漏
                            var rm_ext_2 = base_max_ext[3] - 1;
                            var rm_2 = base_max[3];
                            if (rm_ext_2 > rm_2)
                                base_max[3] = rm_ext_2;
                            base_max_ext[3] = 0;

                            rm_ext_2 = base_max_ext[2] - 1;
                            rm_2 = base_max[2];
                            if (rm_ext_2 > rm_2)
                                base_max[2] = rm_ext_2;
                            base_max_ext[2] = 0;
                            #endregion

                            #region 形态最大连出
                            if (last_numbers.GroupBy(g => g).Select(s => s.Key).ToArray().Length == 2)
                            {
                                xts_max_series_ext[3] += 1;
                                if (xts_max_series_ext[3] > xts_max_series[3])
                                    xts_max_series[3] = xts_max_series_ext[3];

                                xts_max_series_ext[2] += 1;
                                if (xts_max_series_ext[2] > xts_max_series[2])
                                    xts_max_series[2] = xts_max_series_ext[2];
                            }
                            else
                            {
                                xts_max_series_ext[3] = 0;
                                xts_max_series_ext[2] = 0;
                            }
                            #endregion
                            break;
                        case 3: //三不同号  二不同号  三连号
                            xts[1] = 0;
                            xts[3] = 0;
                            if ((nums[0] + 1) == nums[1] && (nums[1] + 1) == nums[2])
                                xt = "三连号";
                            else
                                xt = "三不同号";

                            #region 形态最大遗漏
                            var rm_ext_3 = base_max_ext[3] - 1;
                            var rm_3 = base_max[3];
                            if (rm_ext_3 > rm_3)
                                base_max[3] = rm_ext_3;
                            base_max_ext[3] = 0;

                            rm_ext_3 = base_max_ext[1] - 1;
                            rm_3 = base_max[1];
                            if (rm_ext_3 > rm_3)
                                base_max[1] = rm_ext_3;
                            base_max_ext[1] = 0;
                            #endregion

                            #region 形态最大连出
                            if (last_numbers.GroupBy(g => g).Select(s => s.Key).ToArray().Length == 2)
                            {
                                xts_max_series_ext[3] += 1;
                                if (xts_max_series_ext[3] > xts_max_series[3])
                                    xts_max_series[3] = xts_max_series_ext[3];

                                xts_max_series_ext[1] += 1;
                                if (xts_max_series_ext[1] > xts_max_series[1])
                                    xts_max_series[1] = xts_max_series_ext[1];
                            }
                            else
                            {
                                xts_max_series_ext[3] = 0;
                                xts_max_series_ext[1] = 0;
                            }
                            #endregion
                            break;
                    }
                    //冷热
                    Omission = Omission.Select(s => s = s + 1).ToArray();
                    foreach (var item in nums)
                    {
                        //30期冷热
                        if (i <= 30)
                            Omission_30[item - 1] += 1;
                        //50期冷热
                        if (i <= 50)
                            Omission_50[item - 1] += 1;
                        //100期冷热
                        Omission_100[item - 1] += 1;
                        //遗漏
                        Omission[item - 1] = 0;
                    }
                    #region 计算基本走势
                    last_numbers = last_numbers.GroupBy(g => g).Select(s => s.Key).ToArray();
                    for (int j = 0; j < nums.Length; j++)
                    {
                        #region 基本最大遗漏
                        var rm_ext = base_max_ext[nums[j] - 1] - 1;
                        var rm = base_max[nums[j] - 1];
                        if (rm_ext > rm)
                            base_max[nums[j] - 1] = rm_ext;
                        base_max_ext[nums[j] - 1] = 0;
                        #endregion
                        #region 基本最大连出
                        if (nums.Length == last_numbers.Length)
                        {
                            var last_item = last_numbers[j];
                            if (nums[j] == last_item)
                            {
                                base_max_series_ext[nums[j] - 1] += 1;
                                if (base_max_series_ext[nums[j] - 1] > base_max_series[nums[j] - 1])
                                    base_max_series[nums[j] - 1] = base_max_series_ext[nums[j] - 1];
                            }
                            else
                                base_max_series_ext[nums[j] - 1] = 0;
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
                        x = xt,                  //形态
                        s = sum,                 //和值
                        t = Omission,            //基本走势
                        z = xts                  //形态走势 下标对应：三同号,三不同号,二同号,二不同号
                    });
                });
                #region 计算
                //基础平均遗漏
                for (int j = 0; j < Omission_100.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - Omission_100[j]) / (Omission_100[j] == 0 ? 1 : Omission_100[j]));
                    base_average[j] = num;
                }
                //形态平均遗漏
                for (int j = 0; j < Omission_xts.Length; j++)
                {
                    int num = Convert.ToInt32((TopCount - Omission_xts[j]) / (Omission_xts[j] == 0 ? 1 : Omission_xts[j]));
                    xts_average[j] = num;
                }
                #endregion
                var obj = new
                {
                    tr = trend,
                    o = Omission,
                    o3 = Omission_30,
                    o5 = Omission_50,
                    o0 = Omission_100,

                    bn = Omission_100,      //基础出现次数
                    ba = base_average,      //基础平均遗漏
                    bm = base_max,          //基础最大遗漏
                    bs = base_max_series,   //基础最大连出

                    sn = Omission_xts,     //形态出现次数
                    sa = xts_average,      //形态平均遗漏
                    sm = xts_max,          //形态最大遗漏
                    ss = xts_max_series    //形态最大连出
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基础走势图[三不同号]：{0}", ex.Message));
            }
        }
        #endregion
        #endregion


    }
}
