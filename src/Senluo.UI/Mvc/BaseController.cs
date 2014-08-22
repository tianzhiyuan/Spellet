using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Ors.Core.Components;
using Ors.Core.Exceptions;
using Ors.Core.Logging;
using Ors.Core.Serialization;
using Ors.Framework.Data;

namespace Senluo.UI.Mvc
{
    [FilterError]
    public class BaseController : Controller
    {
        protected IModelService Service { get { return ObjectContainer.Resolve<IModelService>(); } }
        protected void Log(object obj, LogLevel level = LogLevel.Info, Exception ex = null)
        {
            try
            {
                var logger = ObjectContainer.Resolve<ILogger>();
                logger.Log(level, obj, ex);
            }catch{}
        }
        public BaseController()
        {
            bool.TryParse(ConfigurationManager.AppSettings["DoAuth"], out DoAuth);
        }
        protected bool DoAuth;
        protected int UserID
        {
            get { return (int)Session["UserID"]; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userObj = Session["UserID"];
            if (DoAuth)
            {
                if (userObj == null)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult { Data = new { success = false, msg = "logged out", code = (int)RuleViolatedType.NotAuthenticated}, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        filterContext.Result = Request.RawUrl == "/"
                                                   ? new RedirectResult("/login")
                                                   : new RedirectResult("/login?url=" +
                                                                        HttpUtility.UrlEncode(Request.RawUrl));

                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 返回序列化的json
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected internal ActionResult Serialize(object data)
        {
            return this.Serialize(data, "application/json");
        }
        protected internal ActionResult Serialize(object data, string contentType)
        {
            Response.ContentType = contentType;
            Response.Charset = "utf-8";

            var json = ObjectContainer.Resolve<IJsonSerializer>();
            var content = json.Serialize(data);
            
            return Content(content);
        }
    }
}