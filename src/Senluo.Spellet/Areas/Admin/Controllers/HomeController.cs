using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senluo.Spellet.Models;
using Senluo.UI.Mvc;

namespace Senluo.Spellet.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Admin/Home/

        public ActionResult Index()
        {
            var svc = Service;
            var entry = svc.Select(new EntryQuery() {Take = 1}).FirstOrDefault();
            return View();
        }

    }
}
