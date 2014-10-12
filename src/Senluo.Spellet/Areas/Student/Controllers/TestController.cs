using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Senluo.Spellet.Areas.Student.Controllers
{
    public class TestController : StudentController
    {
        //
        // GET: /Student/Test/

        public ActionResult Self()
        {

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}
