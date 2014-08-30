using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ors.Core.Components;
using Ors.Core.Data;
using Ors.Core.Exceptions;
using Ors.Core.Logging;
using Ors.Core.Serialization;
using Ors.Framework.Data;

namespace Senluo.UI.Mvc
{
    [FilterError]
    public class BaseController : Controller
    {
        protected readonly IModelService Service = ObjectContainer.Resolve<IModelService>();

        protected virtual string UserKey
        {
            get { return "UserID"; }
        }
        protected virtual void LogOut()
        {
            Session[UserKey] = null;
        }
        protected virtual void LogIn(object logObj)
        {
            Session[UserKey] = logObj;
        }
        protected void Log(object obj, LogLevel level = LogLevel.Info, Exception ex = null)
        {
            try
            {
                var logger = ObjectContainer.Resolve<ILogger>();
                logger.Log(level, obj, ex);
            }
            catch { }
        }
        public BaseController()
        {
            bool.TryParse(ConfigurationManager.AppSettings["DoAuth"], out DoAuth);
        }
        protected bool DoAuth;
        protected int UserID
        {
            get { return (int)Session[UserKey]; }
        }
        protected virtual string LoginUrl { get { return "/login"; } }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userObj = Session[UserKey];
            if (DoAuth)
            {
                if (userObj == null)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult { Data = new { success = false, msg = "logged out", code = (int)RuleViolatedType.NotAuthenticated }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        filterContext.Result = Request.RawUrl == "/"
                                                   ? new RedirectResult(LoginUrl)
                                                   : new RedirectResult(LoginUrl + "?url=" +
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

    public class BaseController<TModel, TQuery> : BaseController
        where TModel : class, IModel, new()
        where TQuery : IQuery<TModel>
    {
        public virtual ActionResult List(TQuery query)
        {
            var svc = Service;
            var models = svc.Select(query);
            return Serialize(new {success = true, items = models, count = query.Count});
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Put | HttpVerbs.Delete)]
        public virtual ActionResult Index(TModel item)
        {
            var svc = Service;
            if (item == null) return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);

            var verb = Request.HttpMethod;
            switch (verb)
            {
                case "POST":
                    svc.Create(item);
                    return Serialize(new {success = true, item = item});
                case "PUT":
                    svc.Update(item);
                    break;
                case "DELETE":
                    svc.Delete(item);
                    break;
                default:
                    return new HttpStatusCodeResult((int)HttpStatusCode.MethodNotAllowed);
            }

            return Serialize(new { success = true });
        }
    }
}