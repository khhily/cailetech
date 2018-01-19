using CL.SystemInfo.BLL;
using log4net;
using System;
using System.Reflection;
using System.Threading;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Collections.Generic;

namespace CL.WebAPI.Filters
{
    public class DataLogAttribute : ActionFilterAttribute
    {
        private ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            Dictionary<string, object> arguments = filterContext.ActionArguments;
            ThreadPool.QueueUserWorkItem(
                new WaitCallback(delegate
                {
                    try
                    {
                        string Info = string.Format("执行控制器: {0}/{1}", controller, actionName);
                        log.Info(Info);
                        #region 操作日志入库
                        var Entity = new SystemInfo.Entity.DataLogEntity()
                        {
                            DataType = 1,
                            MethedDescribe = "记录操作日志",
                            MethedName = "OnActionExecuting",
                            OperateContent = Newtonsoft.Json.JsonConvert.SerializeObject(arguments),
                            OperateType = 1,
                            RecordTime = DateTime.Now
                        };
                        new DataLogBLL().InertEntity(Entity);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        log.Debug(ex.Message);
                    }
                }));
            
        }
        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }


    public class NoExecuteAttribute : Attribute
    {
        public NoExecuteAttribute() { }
    }
}