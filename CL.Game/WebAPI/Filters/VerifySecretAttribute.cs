using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using CL.Tools.Common;
using WebAPI.Models;
using CL.Enum.Common;
using CL.Enum.Common.Other;

namespace CL.WebAPI.Filters
{
    public class VerifySecretAttribute : ActionFilterAttribute
    {
        private ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string methed = actionContext.Request.Method.Method;
            long UnixTime = 0;
            string Secret = string.Empty;
            string actionName = actionContext.ActionDescriptor.ActionName;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            if (methed == HttpMethedEnum.GET.ToString())
                parameters = RequestInputStream.SetParameters(actionContext.RequestContext.Url.Request.RequestUri.Query.Replace("?", ""), ref UnixTime, ref Secret);
            else if (methed == HttpMethedEnum.POST.ToString() || methed == HttpMethedEnum.PUT.ToString())
                parameters = RequestInputStream.SetParameters(actionContext.Request.Content.ReadAsStreamAsync().Result, ref UnixTime, ref Secret);
            new DataSecret().VerifySecret(UnixTime, Secret, parameters, methed);
            if (DataSecret.VerifyRec == (int)ResultCode.Success && actionName != "ApplyInitiate")
                new DataSecret().VerifyParameters(parameters);
        }
    }
}