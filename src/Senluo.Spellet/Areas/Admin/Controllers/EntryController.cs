using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using Ors.Core.Data;
using Ors.Framework.Data;
using Senluo.Spellet.Models;
using Senluo.UI.Mvc;

namespace Senluo.Spellet.Areas.Admin.Controllers
{
    public class EntryController : AdminController<Entry, EntryQuery>
    {
        
        //
        // GET: /Admin/Entry/
        private int take = 15;
        public ActionResult Index( string key, int skip = 0, bool startWith = false)
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
                string sql = string.Format("SELECT * FROM ENTRY WHERE Word like @key ORDER BY Word ", skip ,take);
                var param = new MySqlParameter("@key", MySqlDbType.String, 200) {Value =  key + "%"};
                entries = svc.SqlQuery<Entry>(sql, param);
                count = entries.Count();
                entries = entries.Skip(skip).Take(take).ToArray();

            }
            else
            {
                var query = new EntryQuery() {Take = take, Skip = skip, OrderField = "Word", WordPattern = key};
                entries = svc.Select(query);
                count = query.Count ?? 0;
            }
            ViewBag.Key = key;
            ViewBag.Skip = skip;
            ViewBag.Total = (int)Math.Round(((double) count)/take + 0.5);
            ViewBag.Current = (int)Math.Round(((double)take) / take + 0.5);
            ViewBag.Take = take;
            ViewBag.StartsWith = startWith.ToString().ToLower();
            ViewBag.Pagination = new Pagination(take, skip, count);
            var entryids = entries.Select(o => o.ID).OfType<int>().ToArray();
            var trans = svc.Select(new TranslationQuery() {EntryIDList = entryids});
            var examples = svc.Select(new ExampleQuery() {EntryIDList = entryids});
            foreach (var entry in entries)
            {
                entry.Translations = trans.Where(o => o.EntryID == entry.ID).ToArray();
                entry.Examples = examples.Where(o => o.EntryID == entry.ID).ToArray();
            }
            return View(entries);
        }


        public ActionResult Modify(int? id = null)
        {
            var model = (id == null)
                            ? new Entry() {Examples = new Example[0], Translations = new Translation[0]}
                            : Service.FindByID<Entry, EntryQuery>(id.Value);
            
            return View(model);
        }
        [HttpPost]
        public ActionResult Modify()
        {
            return null;
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
                    }
                    Service.Create(entry.Examples.ToArray());
                }
                ts.Complete();
            }
            return Serialize(new {success = true});
        }
    }
}
