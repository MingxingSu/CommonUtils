using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace MaxSu.Framework.Common.Web
{
    public class CacheLibrary
    {
        /// <summary>
        ///     获取数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public static object GetCache(string cacheKey)
        {
            Cache objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
        }

        /// <summary>
        ///     设置数据缓存
        /// </summary>
        public static void SetCache(string cacheKey, object objObject)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject);
        }

        /// <summary>
        ///     设置数据缓存
        /// </summary>
        public static void SetCache(string cacheKey, object objObject, TimeSpan Timeout)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, DateTime.MaxValue, Timeout, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        ///     设置数据缓存
        /// </summary>
        public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration,
                                    TimeSpan slidingExpiration)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        ///     移除指定数据缓存
        /// </summary>
        public static void RemoveAllCache(string cacheKey)
        {
            Cache _cache = HttpRuntime.Cache;
            _cache.Remove(cacheKey);
        }

        /// <summary>
        ///     移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }
    }
}