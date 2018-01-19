namespace CL.Json.Entity.WebAPI
{
    public class ApplyStartResult : JsonResult
    {
        /// <summary>
        /// 网关地址
        /// </summary>
        public string GatewayUrl { set; get; }
        
        /// <summary>
        /// 热更地址
        /// </summary>
        public string HotUpdateUrl { set; get; }
        
        /// <summary>
        /// 强制更新地址
        /// </summary>
        public string UpdateUrl { set; get; }

        /// <summary>
        /// API地址
        /// </summary>
        public string WebAPIUrl { set; get; }

        /// <summary>
        /// Socket地址
        /// </summary>
        public string SocketURL { set; get; }
        
        /// <summary>
        /// 更新状态
        /// </summary>
        public int IsUpdate { set; get; }
        
        /// <summary>
        /// 包状态：0 审核 1正常
        /// </summary>
        public int Status { set; get; }
    }
}
