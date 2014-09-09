using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.Areas.Admin.Controllers
{
    public class ExamController : AdminController<Exam, ExamQuery>
    {
        //
        // GET: /Admin/Exam/

        public ActionResult Index()
        {
            return View();
        }

    }
}
