using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MC = System.Runtime.Caching.MemoryCache;
namespace Ors.Core.Caching
{
    public class MemoryCache : ICache
    {
        private readonly ObjectCache CacheService = MC.Default;
        private int _expire;
        public int DefaultExpireMinite
        {
            get
            {
                return _expire <= 0 ? 30 : _expire;
            }
            set { _expire = value; }
        }
        public void AddOrUpdate(string Key, object cacheObj, TimeSpan? expiredAfter)
        {
            CacheService.Set(Key, cacheObj, DateTime.Now.Add(expiredAfter ?? new TimeSpan(0, this.DefaultExpireMinite, 0)));
            var keys = Keys;
            if (!keys.Contains(Key))
            {
                lock (Keys)
                {
                    if (!Keys.Contains(Key))
                    {
                        Keys.Add(Key);
                    }
                }
            }
        }
        public void Delete(string Key)
        {
            CacheService.Remove(Key);
            Keys.Remove(Key);
        }

        public object Get(string Key)
        {
            return CacheService.Get(Key);
        }
        public MemoryCache()
        {
            Keys = new List<string>();
        }
        public IList<string> Keys { get; private set; }

        public void Clear()
        {
            foreach (var key in Keys)
            {
                this.Delete(key);
            }
        }
        #region ICache Members
        

        void ICache.AddOrUpdate(string Key, object cacheObj, TimeSpan? expiredAfter)
        {
            this.AddOrUpdate(Key, cacheObj, expiredAfter);
        }
        void ICache.Delete(string Key)
        {
            this.Delete(Key);
        }

        object ICache.Get(string Key)
        {
            return this.Get(Key);
        }

        IEnumerable<string> ICache.Keys
        {
            get { return this.Keys; }
        }

        void ICache.Clear()
        {
            this.Clear();
        }
        #endregion
    }
}
