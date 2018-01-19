using CL.Game.Entity;
using Dapper;

namespace CL.View.Entity.Game
{
    [Table("udv_UserPay")]
    public class udv_UserPay : UsersPayDetailEntity
    {
        public string UserName { get; set; }
        public string UserMobile { get; set; }
    }
}
