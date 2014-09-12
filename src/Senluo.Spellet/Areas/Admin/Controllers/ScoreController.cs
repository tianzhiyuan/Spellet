using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ors.Core.Components;
using Ors.Core.Serialization;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.Areas.Admin.Controllers
{
    public class ScoreController : AdminController<AnswerSheet, AnswerSheetQuery>
    {
        //
        // GET: /Admin/Score/

        public ActionResult Index(AnswerSheetQuery filter)
        {
            ViewBag.Filter = "{}";
            if (filter != null)
            {
                var objects = new List<string>();
                if (filter.ExamIDList != null)
                {
                    objects.Add(string.Format("ExamIDList:[{0}]", string.Join(",", filter.ExamIDList)));
                }
                if (filter.StudentIDList != null)
                {
                    objects.Add(string.Format("StudentIDList:[{0}]", string.Join(",", filter.StudentIDList)));
                }
                ViewBag.Filter = string.Format("{{{0}}}", string.Join(",", objects));
            }
            return View();
        }

    }
}
