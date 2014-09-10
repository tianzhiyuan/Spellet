using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ors.Framework.Data;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.Areas.Admin.Controllers
{
    public class ExamController : AdminController<Exam, ExamQuery>
    {
        //
        // GET: /Admin/Exam/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Modify(int id)
        {
            var model = Service.FindByID<Exam, ExamQuery>(id);
            model.Questions = Service.Select(new QuestionQuery() {ExamIDList = new int[] {id}}).ToArray();
            var examples =
                Service.Select(new ExampleQuery()
                    {
                        IDList = model.Questions.Select(o => o.ContentID).OfType<int>().ToArray()
                    });
            foreach (Question q in model.Questions)
            {
                var example = examples.FirstOrDefault(o => o.ID == q.ContentID);
                q.Example = example;
            }
            return View(model);
        }
    }
}
