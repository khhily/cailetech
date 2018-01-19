using CL.Json.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Entity.Json.WebAPI
{
    public class ClientNewsResult : JsonResult
    {
        public ClientNews Data { set; get; }
    }
    public class ClientNews
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public string Time { set; get; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { set; get; }

        /// <summary>
        /// 阅读次数
        /// </summary>
        public int ReadNum { set; get; }

        /// <summary>
        /// 支持数
        /// </summary>
        public int SupportNum { set; get; }

        /// <summary>
        /// 反对数
        /// </summary>
        public int OpposeNum { set; get; }

        /// <summary>
        /// 推荐投注号码
        /// </summary>
        public string LotNumber { set; get; }

        public List<NewsTitle> NewsData { set; get; }

    }
}
