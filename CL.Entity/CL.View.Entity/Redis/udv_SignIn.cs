using CL.Json.Entity.WebAPI;
using System.Collections.Generic;

namespace CL.View.Entity.Redis
{
    public class udv_SignIn
    {
        public List<LotteryData> LotteryData { set; get; }
        public LoginData LoginData { set; get; }
    }
    
}
