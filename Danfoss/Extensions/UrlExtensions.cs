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
            return string.Format("solution/detail/{0}", id);
        }

        public static string DownloadUrl(this UrlHelper helper, string file)
        {
            return string.Format("/download/{0}", helper.RequestContext.HttpContext.Server.UrlEncode(file));
        }
    }
}