using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.View.Entity.Game
{
    public class udv_UserAccountDetail : UsersRecordEntity
    {
        /// <summary>
        /// 方案编号
        /// </summary>
        public long SchemeID { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public long MoneyAdd { get; set; }
        /// <summary>
        /// 冻结金额
        /// </summary>
        public long MoneySub { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
    }
}
