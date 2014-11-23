using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using Ors.Core.Components;
using Ors.Core.Data;
using Ors.Core.Exceptions;
using Ors.Core.Serialization;
using Ors.Framework.Data;
using Senluo.Spellet.Models;
using Senluo.UI.Mvc;
using Senluo.VocaSpider.Parser;

namespace Senluo.Spellet.Areas.Admin.Controllers
{
    public class EntryController : AdminController<Entry, EntryQuery>
    {

        //
        // GET: /Admin/Entry/
        private int take = 15;
        public ActionResult Index(string key, int skip = 0, bool startWith = false)
        {
            var svc = Service as DataService;
            IEnumerable<Entry> entries;
            int count = 0;
            if (string.IsNullOrWhiteSpace(key))
            {
                key = null;
            }
            if (startWith && !string.IsNullOrWhiteSpace(key))
            {
                string sql = string.Format("SELECT * FROM ENTRY WHERE Word like @key ORDER BY Word ", skip, take);
                var param = new MySqlParameter("@key", MySqlDbType.String, 200) { Value = key + "%" };
                entries = svc.SqlQuery<Entry>(sql, param);
                count = entries.Count();
                entries = entries.Skip(skip).Take(take).ToArray();

            }
            else
            {
                var query = new EntryQuery() { Take = take, Skip = skip, OrderField = "Word", WordPattern = key };
                entries = svc.Select(query);
                count = query.Count ?? 0;
            }
            ViewBag.Key = key;
            ViewBag.Skip = skip;
            ViewBag.Total = (int)Math.Round(((double)count) / take + 0.5);
            ViewBag.Current = (int)Math.Round(((double)take) / take + 0.5);
            ViewBag.Take = take;
            ViewBag.StartsWith = startWith.ToString().ToLower();
            ViewBag.Pagination = new Pagination(take, skip, count);
            var entryids = entries.Select(o => o.ID).OfType<int>().ToArray();
            var trans = svc.Select(new TranslationQuery() { EntryIDList = entryids });
            var examples = svc.Select(new ExampleQuery() { EntryIDList = entryids });
            foreach (var entry in entries)
            {
                entry.Translations = trans.Where(o => o.EntryID == entry.ID).ToArray();
                entry.Examples = examples.Where(o => o.EntryID == entry.ID).ToArray();
            }
            return View(entries);
        }


        public ActionResult Modify(int id)
        {
            var model = Service.FindByID<Entry, EntryQuery>(id);
            model.Translations = Service.Select(new TranslationQuery() {EntryIDList = new[] {id}}).ToArray();
            model.Examples = Service.Select(new ExampleQuery() {EntryIDList = new[] {id}}).ToArray();
            return View(model);
        }
        [HttpPost]
        public ActionResult Modify(Entry entry)
        {
            using (var ts = new TransactionScope())
            {
                Service.Patch<Entry, EntryQuery>(entry);
                if (entry.Translations!= null && entry.Translations.Any())
                {
                    Service.Patch<Translation,TranslationQuery >(entry.Translations.ToArray());
                }
                if (entry.Examples != null && entry.Examples.Any())
                {
                    foreach (var example in entry.Examples)
                    {
                        if (string.IsNullOrWhiteSpace(example.Keyword))
                        {
                            example.Keyword = entry.Word;
                        }
                    }
                    Service.Patch<Example, ExampleQuery>(entry.Examples.ToArray());
                }
                ts.Complete();
            }
            new EntryManager().Refresh(entry.Word[0]);
            return Serialize(new {success = true});
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Entry entry)
        {
            using (var ts = new TransactionScope())
            {
                Service.Create(entry);
                if (entry.Translations != null && entry.Translations.Any())
                {
                    foreach (var t in entry.Translations)
                    {
                        t.EntryID = entry.ID;
                    }
                    Service.Create(entry.Translations.ToArray());
                }
                if (entry.Examples != null && entry.Examples.Any())
                {
                    foreach (var ex in entry.Examples)
                    {
                        ex.EntryID = entry.ID;
                        if (string.IsNullOrWhiteSpace(ex.Keyword))
                        {
                            ex.Keyword = entry.Word;
                        }
                    }
                    Service.Create(entry.Examples.ToArray());
                }
                ts.Complete();
            }
            new EntryManager().Refresh(entry.Word[0]);
            return Serialize(new { success = true, item = entry });
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Put | HttpVerbs.Delete)]
        public override ActionResult Index(Entry item)
        {
            var svc = Service;
            if (item == null) return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            PartialFiller.Fill<Entry, EntryQuery>(item);
            var verb = Request.HttpMethod;
            switch (verb)
            {
                case "POST":
                    svc.Create(item);
                    return Serialize(new { success = true, item = item });
                case "PUT":
                    svc.Patch<Entry, EntryQuery>(item);
                    break;
                case "DELETE":
                    if (svc.Any(new CourseContentQuery() {ContentIDList = new int[] {item.ID.Value}}))
                    {
                        throw new RuleViolatedException("有相关的课程，不可以删除");
                    }
                    
                    svc.Delete(item);
                    break;
                default:
                    return new HttpStatusCodeResult((int)HttpStatusCode.MethodNotAllowed);
            }
            new EntryManager().Refresh(item.Word[0]);
            return Serialize(new { success = true });
        }
        [HttpPost]
        public ActionResult Test()
        {
            var count = Service.GetCount(new EntryQuery());
            var parser = ObjectContainer.Resolve<IParser>();
            
            for (var index = 0; index < count; index++)
            {
                var query = new EntryQuery() { Take = 1, Skip = index };
                var entry = Service.Select(query).FirstOrDefault();
                if (entry == null) break;
                try
                {
                    var res = parser.Search(entry.Word);
                    entry.Phonetic_US = res.Phonetic_US;
                    Service.Update(entry);
                }
                catch
                {
                }
            }
            
            return null;
        }

        public ActionResult WordPicker(int examid)
        {
            var model = Service.FindByID<Exam, ExamQuery>(examid);
            model.Questions = Service.Select(new QuestionQuery() { ExamIDList = new int[] { examid } }).ToArray();
            var examples =
                Service.Select(new ExampleQuery()
                {
                    IDList = model.Questions.Select(o => o.ContentID).OfType<int>().ToArray(),
                    Includes = new string[]{"Entry"}
                });
            ViewBag.EntryIDs = examples.Select(o => o.EntryID).ToArray();
            ViewBag.Entries = examples.Select(o => o.Entry).ToArray();
            
            ViewBag.Max = model.Count;
            return View();
        }

        public ActionResult GetWords(string word)
        {
            var list = new EntryManager().Get(word);
            return Serialize(new {success = true, items = list, count = list.Count()});
        }
    }
}
