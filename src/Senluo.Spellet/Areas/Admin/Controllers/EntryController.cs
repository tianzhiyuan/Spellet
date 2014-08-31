using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using Ors.Core.Data;
using Ors.Framework.Data;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.Areas.Admin.Controllers
{
    public class EntryController : AdminController<Entry, EntryQuery>
    {
        
        //
        // GET: /Admin/Entry/
        private int take = 15;
        public ActionResult Index(string key, int skip = 0)
        {
            var svc = Service as DataService;
            IEnumerable<Entry> entries;
            if (!string.IsNullOrWhiteSpace(key))
            {
                string sql = string.Format("SELECT * FROM ENTRY WHERE Word like @key ORDER BY Word LIMIT {0},{1}", skip ,take);
                var param = new MySqlParameter("@key", MySqlDbType.String, 200) {Value =  key + "%"};
                entries = svc.SqlQuery<Entry>(sql, param);

                entries = entries.Skip(skip).Take(take).ToArray();
            }
            else
            {
                entries = svc.Select(new EntryQuery() {Take = take, Skip = skip, OrderField = "Word"});
            }
            ViewBag.Key = key;
            ViewBag.Skip = skip;
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

    }
}
