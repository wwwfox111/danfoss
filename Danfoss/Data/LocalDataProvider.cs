using Danfoss.Core.Utilities;
using Danfoss.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Danfoss.Data
{
    public class LocalDataProvider
    {
        private static LocalDataProvider curr;
        private static object obj = new object();
        public static LocalDataProvider Current
        {
            get
            {
                if (curr == null)
                {
                    lock (obj)
                    {
                        if (curr == null)
                            curr = new LocalDataProvider();
                    }
                }
                return curr;
            }
        }


        public virtual Solution FindSolutionById(int id)
        {
            return GetAll().Solutions.FirstOrDefault(c => c.Id == id);
        }

        public virtual string CacheKeyPrefix
        {
            get
            {
                return "Data_";
            }
        }

        /// <summary>
        /// 获取解决方案数据信息
        /// </summary>
        /// <returns></returns>
        public SolutionData GetAll()
        {
            SolutionData result = null;
            var type = typeof(SolutionData);
            var cacheKey = CacheKeyPrefix + type.FullName;
            if (CacheHelper.GetCache(cacheKey) == null)
            {
                string content = string.Empty;
                string filePath = HttpContext.Current.Server.MapPath("~/App_Data/Data.xml");
                if (File.Exists(filePath))
                    content = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(content))
                {
                    result = XmlHelper.DeserializeXML<SolutionData>(content);
                    CacheHelper.SetCache(cacheKey, result);
                }
            }
            else
            {
                result = (SolutionData)CacheHelper.GetCache(cacheKey);
            }
            return result;
        }

    }
}