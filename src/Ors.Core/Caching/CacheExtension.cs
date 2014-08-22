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
    }
}
