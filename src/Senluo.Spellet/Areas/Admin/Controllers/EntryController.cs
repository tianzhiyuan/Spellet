using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.Areas.Admin.Controllers
{
    public class EntryController : AdminController<Entry, EntryQuery>
    {
        //
        // GET: /Admin/Entry/

        public ActionResult Index()
        {
            return View();
        }

    }
}
