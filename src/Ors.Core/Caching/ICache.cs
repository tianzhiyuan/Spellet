using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Caching
{
    /// <summary>
    /// Caching
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Add or update object with key
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="cacheObj"></param>
        /// <param name="expiredAfter"></param>
        void AddOrUpdate(string Key, object cacheObj, TimeSpan? expiredAfter = null);
        /// <summary>
        /// delete cached object by key
        /// </summary>
        /// <param name="Key"></param>
        void Delete(string Key);
        /// <summary>
        /// Get cached object by key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        object Get(string Key);
        /// <summary>
        /// Get list of Keys
        /// </summary>
        IEnumerable<string> Keys { get; }
        /// <summary>
        /// Clear all cachedd objects
        /// </summary>
        void Clear();
    }
}
