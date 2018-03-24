using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Danfoss.Extensions
{
    public static class UrlExtensions
    {
        public static string Img(this UrlHelper helper, string url)
        {
            return string.Format("/content/images/{0}", url);
        }

        public static string SolutionUrl(this UrlHelper helper, int id)
        {
            return string.Format("/detail/{0}.html", id);
        }
    }
}