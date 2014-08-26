using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senluo.UI.Mvc;

namespace Senluo.Spellet.Controllers
{
    public class LoginController : BaseController
    {
        //
        // GET: /Login/
        public LoginController()
        {
            this.DoAuth = false;
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}
