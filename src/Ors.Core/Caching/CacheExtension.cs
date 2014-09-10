using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Caching
{
    public static class CacheExtension
    {
        public static void AddOrUpdate(this ICache cache, string Key, object cacheObj, int minites)
        {
            cache.AddOrUpdate(Key, cacheObj, new TimeSpan(0, minites, 0));
        }
        public static void AddOrUpdate<T>(this ICache cache, T obj, int minites = 30) where T : class
        {
            var key = typeof(T).FullName;
            cache.AddOrUpdate(key, obj, new TimeSpan(0, minites, 0));
        }
        public static T GetTypeCache<T>(this ICache cache) where T : class
        {
            var key = typeof(T).FullName;
            return (T)cache.Get(key);
        }
    }
}
