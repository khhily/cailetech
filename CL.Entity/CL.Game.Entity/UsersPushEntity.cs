using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.Entity
{
    /// <summary>
    /// 会员推送(暂时支持百度)
    /// </summary>
    [Table("CT_UsersPush")]
    public class UsersPushEntity
    {
        [Key]
        public long UserId { set; get; }
        public string PushIdentify { set; get; }
    }
}
