using CL.Json.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Entity.Json.WebAPI
{
    public class NewsTitleResult : JsonResult
    {
        public List<NewsTitle> Data { set; get; }
    }
    public class NewsTitle
    {
        /// <summary>
        /// 新闻ID
        /// </summary>
        public int ID { set; get; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend { set; get; }

        /// <summary>
        /// 内容连接
        /// </summary>
        public string LookUrl { set; get; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public string Time { set; get; }
    }
}
