using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Senluo.Spellet.Areas.Student.Controllers
{
    public class LogoutController : StudentController
    {
        //
        // GET: /Student/Logout/

        public LogoutController()
        {
            this.DoAuth = false;
        }
        public ActionResult Index()
        {
            this.LogOut();
            return RedirectToAction("Index", "Login");
        }

    }
}
