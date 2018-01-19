using CL.WebAPI.Filters;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web.Http;

namespace CL.WebAPI.Controllers
{
    [DataLog, ErrorLog]
    public class BaseController : ApiController
    {
        public BaseController()
        {

        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<string> TResultAsync(object obj)
        {
            return await Task.Run(() =>
            {
                return JsonConvert.SerializeObject(obj);
            });
        }
    }
}
