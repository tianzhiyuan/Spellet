using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Senluo.Spellet.Areas.Admin.Controllers
{
    public class LoginController : AdminController
    {
        public LoginController()
        {
            this.DoAuth = false;
        }
        //
        // GET: /Admin/Login/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            return Serialize(new {success = true});
        }
    }
}
