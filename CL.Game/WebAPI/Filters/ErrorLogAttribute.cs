using CL.SystemInfo.BLL;
using log4net;
using System;
using System.Reflection;
using System.Threading;
using System.Web.Http.Filters;
using System.Collections.Generic;
using System.Web.Http.Controllers;

namespace CL.WebAPI.Filters
{
    public class ErrorLogAttribute : ExceptionFilterAttribute
    {

        private ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string controller = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            Dictionary<string, object> arguments = actionExecutedContext.ActionContext.ActionArguments;
            ThreadPool.QueueUserWorkItem(
                     new WaitCallback(delegate
                     {
                         try
                         {
                             var e = actionExecutedContext.Exception;
                             string error = string.Format("异常控制器: {0}/{1};异常信息: {2}", controller, actionName, e.Message);
                             log.Error(error, e);
                             #region 错误日志入库
                             var Entity = new SystemInfo.Entity.ErrorLogEntity()
                             {
                                 Message = e.Message,
                                 MethedDescribe = "记录错误日志",
                                 MethedName = "OnException",
                                 OperateParamters = Newtonsoft.Json.JsonConvert.SerializeObject(arguments),
                                 RecordTime = DateTime.Now,
                                 StackTrace = e.StackTrace
                             };
                             new ErrorLogBLL().InsertEntity(Entity);
                             #endregion
                         }
                         catch (Exception ex)
                         {
                             log.Debug(ex.Message);
                         }
                     }));
        }
    }
}