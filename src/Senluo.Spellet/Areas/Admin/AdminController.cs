using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ors.Core.Data;
using Ors.Framework.Data;
using Senluo.Spellet.Models;
using Senluo.UI.Mvc;

namespace Senluo.Spellet.Areas.Admin
{
    public class AdminController : BaseController
    {
        protected override string UserKey { get { return "TeacherID"; } }
        protected override string LoginUrl { get { return "/admin/login"; } }

        protected Teacher Teacher
        {
            get
            {
                var svc = Service;
                var userid = UserID;
                var teacher = svc.FindByID<Teacher, TeacherQuery>(userid);
                return teacher;
            }
        }
    }

    public class AdminController<TModel, TQuery> : BaseController<TModel, TQuery>
        where TModel : class, IModel, new()
        where TQuery : IQuery<TModel>, new()
    {
        protected override string UserKey { get { return "TeacherID"; } }
        protected override string LoginUrl { get { return "/admin/login"; } }

        protected Teacher Teacher
        {
            get
            {
                var svc = Service;
                var userid = UserID;
                var teacher = svc.FindByID<Teacher, TeacherQuery>(userid);
                return teacher;
            }
        }
    }
}