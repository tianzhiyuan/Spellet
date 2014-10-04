using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senluo.UI.Mvc;

namespace Senluo.Spellet.Areas.Student.Controllers
{
    public class HomeController : StudentController
    {
        //
        // GET: /Student/Home/
        
        public ActionResult Index()
        {
            return View();
        }

    }
}
