using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
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
                        IDList = model.Questions.Select(o => o.ContentID).OfType<int>().ToArray(),
                        Includes = new string[]{"Entry"}
                    });
            foreach (Question q in model.Questions)
            {
                var example = examples.FirstOrDefault(o => o.ID == q.ContentID);
                q.Example = example;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ModifyItems(int[] entryids, int examid)
        {
            var model = Service.FindByID<Exam, ExamQuery>(examid);
            model.Questions = Service.Select(new QuestionQuery() { ExamIDList = new int[] { examid } }).ToArray();
            var entrys = Service.Select(new EntryQuery() {IDList = entryids});
            var examples = Service.Select(new ExampleQuery() {EntryIDList = entryids});

            var questions = new List<Question>();
            foreach (var entry in entrys)
            {
                var first = examples.FirstOrDefault(o => o.EntryID == entry.ID);
                var q = new Question()
                    {
                        ContentID = first.ID,
                        Expect = entry.Word,
                        Score = 1,
                        ExamID = examid
                    };
                questions.Add(q);
            }
            
            using (var ts = new TransactionScope())
            {
                if (model.Questions.Any())
                {
                    Service.Delete(model.Questions);
                }
                if (questions.Any())
                {
                    Service.Create(questions.ToArray());
                }
                ts.Complete();
            }

            return Serialize(new {success = true});
        }
    }
}
