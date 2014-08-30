﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senluo.UI.Mvc;

namespace Senluo.Spellet.Areas.Student.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Student/Home/
        public HomeController()
        {
            this.DoAuth = false;
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}
