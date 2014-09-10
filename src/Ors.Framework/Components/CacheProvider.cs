using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Caching;
using Ors.Core.Components;
using Ors.Core.Exceptions;

namespace Ors.Framework.Components
{
    internal class CacheItem
    {
        public Func<object> Updater;
        public TimeSpan ExpireAfter;
    }
    [Component]
    public class CacheProvider
    {
        private readonly IDictionary<string, CacheItem> _updaters = new Dictionary<string, CacheItem>();
        private readonly ICache _innerCache;
        private readonly object _lock = new object();
        public CacheProvider(ICache cache)
        {
            _innerCache = cache;
        }
        public object Get(string key)
        {
            var obj = _innerCache.Get(key);
            if (obj == null)
            {
                CacheItem cacheItem;
                if (_updaters.TryGetValue(key, out cacheItem))
                {
                    obj = cacheItem.Updater.Invoke();
                    _innerCache.AddOrUpdate(key, obj);
                }
                else
                {
                    throw new RuleViolatedException("no policy registered");
                }
            }
            return obj;
        }
        public object Refresh(string key)
        {
            CacheItem cacheItem;
            object obj;
            if (_updaters.TryGetValue(key, out cacheItem))
            {
                obj = cacheItem.Updater.Invoke();
                _innerCache.AddOrUpdate(key, obj);
            }
            else
            {
                throw new RuleViolatedException("no policy registered");
            }
            return obj;
        }
        public void SetUpdatePolicy(string key, TimeSpan expireAfter, Func<object> updater)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new RuleViolatedException("key不能为空");
            }
            if (updater == null)
            {
                throw new RuleViolatedException("updater cannot be null");
            }
            lock (_lock)
            {
                if (_updaters.ContainsKey(key))
                {
                    throw new RuleViolatedException("key 不可以重复注册");
                }
                _updaters.Add(key, new CacheItem() {ExpireAfter = expireAfter, Updater = updater});
            }
        }
    }
}
