using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ors.Core.Data;
using Ors.Framework.Data;
using Senluo.Spellet.Models;
using Senluo.UI.Mvc;

namespace Senluo.Spellet.Areas.Student
{
    public class StudentController : BaseController
    {
        protected override string UserKey { get { return "StudentID"; } }
        protected override string LoginUrl { get { return "/student/login"; } }

        protected Senluo.Spellet.Models.Student Student
        {
            get
            {
                var svc = Service;
                return svc.FindByID<Senluo.Spellet.Models.Student, Senluo.Spellet.Models.StudentQuery>(UserID);
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var userObj = Session[UserKey];
            if (userObj != null)
            {
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    ViewBag.User = Service.Select(new StudentQuery() {ID = (int) userObj}).FirstOrDefault();
                }
            }

        }
    }

    public class StudentController<TModel, TQuery> : BaseController<TModel, TQuery>
        where TModel : class, IModel, new()
        where TQuery : IQuery<TModel>, new()
    {
        protected override string UserKey { get { return "StudentID"; } }
        protected override string LoginUrl { get { return "/student/login"; } }

        protected Senluo.Spellet.Models.Student Student
        {
            get
            {
                var svc = Service;
                return svc.FindByID<Senluo.Spellet.Models.Student, Senluo.Spellet.Models.StudentQuery>(UserID);
            }
        }
    }
}