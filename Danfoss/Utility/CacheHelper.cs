using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace Danfoss.Utility
{
    /// <summary>
    /// 本地缓存帮助类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 本地缓存获取
        /// </summary>
        /// <param name="name">key</param>
        /// <returns></returns>
        public static object Get(string name)
        {
#if DEBUG
            return null;
#else
           return HttpRuntime.Cache.Get(name);
#endif
        }

        /// <summary>
        /// 本地缓存获取
        /// </summary>
        public static T Get<T>(string name)
        {
            return (T)HttpRuntime.Cache.Get(name);
        }

        /// <summary>
        /// 本地缓存移除
        /// </summary>
        /// <param name="name">key</param>
        public static void Remove(string name)
        {
            if (HttpRuntime.Cache[name] != null)
                HttpRuntime.Cache.Remove(name);
        }

        /// <summary>
        /// 本地缓存写入（默认缓存20min）
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        public static void Set(string name, object value)
        {
            Set(name, value, null);
        }

        /// <summary>
        /// 本地缓存写入（默认缓存20min）,依赖项
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        /// <param name="cacheDependency">依赖项</param>
        public static void Set(string name, object value, CacheDependency cacheDependency)
        {
            HttpRuntime.Cache.Insert(name, value, cacheDependency, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 本地缓存写入,依赖文件路径
        /// </summary>
        public static void Set(string name, object value, int minutes, string cacheDependencyFilePath)
        {
#if DEBUG

#else
            HttpRuntime.Cache.Insert(name, value, string.IsNullOrEmpty(cacheDependencyFilePath) ? null : new CacheDependency(cacheDependencyFilePath), DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration);
#endif
        }

        /// <summary>
        /// 本地缓存写入
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        /// <param name="minutes">缓存分钟</param>
        public static void Set(string name, object value, int minutes, int seconds = 0)
        {
            HttpRuntime.Cache.Insert(name, value, null, DateTime.Now.AddMinutes(minutes).AddSeconds(seconds), Cache.NoSlidingExpiration);
        }

        ///// <summary>
        ///// 本地缓存写入，包括分钟，是否绝对过期及缓存过期的回调
        ///// </summary>
        ///// <param name="name">key</param>
        ///// <param name="value">value</param>
        ///// <param name="minutes">缓存分钟</param>
        ///// <param name="isAbsoluteExpiration">是否绝对过期</param>
        ///// <param name="onRemoveCallback">缓存过期回调</param>
        //public static void Set(string name, object value, int minutes, bool isAbsoluteExpiration, CacheItemRemovedCallback onRemoveCallback)
        //{
        //    if (isAbsoluteExpiration)
        //        HttpRuntime.Cache.Insert(name, value, null, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration, CacheItemPriority.Normal, onRemoveCallback);
        //    else
        //        HttpRuntime.Cache.Insert(name, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes), CacheItemPriority.Normal, onRemoveCallback);
        //}

        /// <summary>
        /// 本地缓存写入，包括分钟，是否绝对过期及缓存过期的回调
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        /// <param name="minutes">缓存分钟</param>
        /// <param name="isAbsoluteExpiration">是否绝对过期</param>
        /// <param name="onUpdateCallback">缓存更新时的回调</param>
        public static void Set(string name, object value, int minutes, bool isAbsoluteExpiration, CacheItemUpdateCallback onUpdateCallback)
        {
            if (isAbsoluteExpiration)
            {
                if (onUpdateCallback != null)
                {
                    HttpRuntime.Cache.Insert(name, value, null, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration, onUpdateCallback);
                }
                else
                {
                    HttpRuntime.Cache.Insert(name, value, null, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration);
                }
            }
            else
            {
                if (onUpdateCallback != null)
                {
                    HttpRuntime.Cache.Insert(name, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes), onUpdateCallback);
                }
                else
                {
                    HttpRuntime.Cache.Insert(name, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes));
                }
            }
        }

        /// <summary>
        /// 根据正则匹配键值清除相关的本地缓存
        /// </summary>
        /// <param name="keyRegex">如"Config_.*"</param>
        public static void Clear(string keyRegex)
        {
            var keys = new List<string>();
            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var key = enumerator.Key.ToString();
                if (Regex.IsMatch(key, keyRegex, RegexOptions.IgnoreCase))
                    keys.Add(key);
            }

            for (int i = 0; i < keys.Count; i++)
            {
                HttpRuntime.Cache.Remove(keys[i]);
            }
        }
    }
}