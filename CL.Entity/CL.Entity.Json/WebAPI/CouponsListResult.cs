using CL.Json.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Entity.Json.WebAPI
{
    public class CouponsListResult : JsonResult
    {
        public int Counts { set; get; }
        public List<CouponsListData> Data { set; get; }
    }

    public class CouponsListData
    {
        /// <summary>
        /// 彩券标识
        /// </summary>
        public long CouponsID { set; get; }

        /// <summary>
        /// 面值
        /// </summary>
        public long FaceValue { set; get; }

        /// <summary>
        /// 余额
        /// </summary>
        public long Balance { set; get; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public string StartTime { set; get; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public string ExpireTime { set; get; }

        /// <summary>
        /// 所属彩种
        /// </summary>
        public int LotteryCode { set; get; }

        /// <summary>
        /// 是否允许叠加使用
        /// </summary>
        public bool IsSuperposition { set; get; }

        /// <summary>
        /// 是否允许追号
        /// </summary>
        public bool IsChaseTask { set; get; }
        /// <summary>
        /// 满减
        /// </summary>
        public long SatisfiedMoney { set; get; }
        /// <summary>
        /// 彩券类型：0固定时间段，1固定时长，2满减，3永不过期
        /// </summary>
        public int CouponsType { set; get; }
    }
}
