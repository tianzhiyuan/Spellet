using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Ors.Core.Components;
using Ors.Framework.Components;
using Ors.Framework.Data;

namespace Senluo.Spellet.Models
{
    public class EntryManager
    {
        
        static EntryManager()
        {
            var provider = ObjectContainer.Resolve<CacheProvider>();
            foreach (var firstletter in "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray())
            {
                var key = "entrycache_" + firstletter;
                char local = firstletter;
                Func<object> updater = () => UpdateEntryCache(local);
                provider.SetUpdatePolicy(key, new TimeSpan(0, 30, 0), updater);
            }
        }
        static object UpdateEntryCache(char firstletter)
        {
            var svc = ObjectContainer.Resolve<IModelService>() as DataService;
            string sql = string.Format("SELECT * FROM ENTRY WHERE Word like @key ORDER BY Word ");
            var param = new MySqlParameter("@key", MySqlDbType.String, 200) { Value = firstletter + "%" };
            var entries = svc.SqlQuery<Entry>(sql, param);
            return entries.ToArray();
        }

        public Entry[] Get(string firstletter)
        {
            var provider = ObjectContainer.Resolve<CacheProvider>();
            return (Entry[])provider.Get("entrycache_" + firstletter);
        }
        public Entry[] Refresh(string firstletter)
        {
            var provider = ObjectContainer.Resolve<CacheProvider>();
            return (Entry[]) provider.Refresh("entrycache_" + firstletter);
        }
    }
}