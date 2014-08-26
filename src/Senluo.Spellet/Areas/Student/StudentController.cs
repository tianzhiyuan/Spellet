using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ors.Core.Data;
using Ors.Framework.Data;
using Senluo.UI.Mvc;

namespace Senluo.Spellet.Areas.Student
{
    public class StudentController : BaseController
    {
        protected override string UserKey { get { return "StudentID"; } }
        protected override string LoginUrl { get { return "/student/login"; } }

        protected Models.Student Student
        {
            get
            {
                var svc = Service;
                return svc.FindByID<Models.Student, Models.StudentQuery>(UserID);
            }
        }
    }

    public class StudentController<TModel, TQuery> : BaseController<TModel, TQuery>
        where TModel : class, IModel, new()
        where TQuery : IQuery<TModel>
    {
        protected override string UserKey { get { return "StudentID"; } }
        protected override string LoginUrl { get { return "/student/login"; } }

        protected Models.Student Student
        {
            get
            {
                var svc = Service;
                return svc.FindByID<Models.Student, Models.StudentQuery>(UserID);
            }
        }
    }
}