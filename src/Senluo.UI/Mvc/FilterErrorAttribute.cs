using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ors.Core.Components;
using Ors.Core.Exceptions;
using Ors.Core.Logging;

namespace Senluo.UI.Mvc
{
    public class FilterErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public string ErrPage { get; set; }
        public void OnException(ExceptionContext filterContext)
        {
            var error = filterContext.Exception as RuleViolatedException;
            if (error == null)
            {
                var logger = ObjectContainer.Resolve<ILogger>();
                logger.SafeLog(filterContext.Exception);

                error = HttpContext.Current.IsDebuggingEnabled
                            ? new RuleViolatedException(filterContext.Exception.Message)
                            : new RuleViolatedException((int) RuleViolatedType.UnKnown, "未知错误", filterContext.Exception);
            }
            switch (error.Code)
            {
                case (int)RuleViolatedType.NotAuthenticated:
                    break;
                
            }
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult()
                {
                    Data = new { success = false, msg = error.Message, code = error.Code },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                if (string.IsNullOrWhiteSpace(ErrPage))
                {
                    filterContext.Result =
                        new RedirectToRouteResult(new RouteValueDictionary()
                            {
                                {"controller", "Home"},
                                {"action", "Error"},
                                {"message", error.Message}
                            });
                }
                else
                {
                    filterContext.Result = new RedirectResult(ErrPage);
                }
                
            }

            
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}
