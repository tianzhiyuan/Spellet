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
                if (filter.ExamID != null)
                {
                    objects.Add(string.Format("ExamIDList:{0}", filter.ExamID));
                }
                if (filter.StudentID != null)
                {
                    objects.Add(string.Format("StudentID:{0}", filter.StudentID));
                }
                ViewBag.Filter = string.Format("{{{0}}}", string.Join(",", objects));
            }
            return View();
        }

        public ActionResult Detail(int examid, int studentid)
        {
            var items = new Student.Controllers.TestController().BuildAnswerSheetVM(examid, studentid);
            return View("~/Areas/Student/Views/Test/Score.cshtml", items);
        }
    }
}
