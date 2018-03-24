using System;
using System.Web;
using System.Collections;

namespace Danfoss.Core.Utilities
{
    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public static object GetCache(string cacheKey)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            return cache[cacheKey];
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string cacheKey, object obj)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            cache.Insert(cacheKey, obj);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="cacheSeconds">缓存多少秒</param>
        public static void SetCache(string cacheKey, object obj, int cacheSeconds)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            cache.Insert(cacheKey, obj, null, DateTime.Now.AddSeconds(cacheSeconds), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string cacheKey, object obj, TimeSpan timeout)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            cache.Insert(cacheKey, obj, null, DateTime.MaxValue, timeout,
                System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string cacheKey, object obj, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            cache.Insert(cacheKey, obj, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveCache(string cacheKey)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            cache.Remove(cacheKey);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                cache.Remove(CacheEnum.Key.ToString());
            }
        }
    }
}
