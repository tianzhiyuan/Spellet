using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senluo.Spellet.EF;

namespace Senluo.Spellet.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home", new {area = "Student"});
        }

        
    }
}
