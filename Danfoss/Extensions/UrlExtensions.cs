using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Danfoss.Extensions
{
    public static class UrlExtensions
    {
        public static string Img(this UrlHelper helper, string url, string folder = null)
        {
            if (string.IsNullOrEmpty(folder))
                return helper.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + string.Format("/content/images/{0}", url);
            return helper.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + string.Format("/content/{1}/{0}", url, folder);
        }

        public static string Solution(this UrlHelper helper, int id)
        {
            return helper.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + string.Format("/solution/detail/{0}", id);
        }

        public static string Download(this UrlHelper helper, string file)
        {
            return helper.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + string.Format("/content/download/{0}", file);

            //return string.Format("/content/download/{0}", helper.RequestContext.HttpContext.Server.UrlEncode(file));
        }

        public static string GetRootPath(this UrlHelper helper, string path)
        {
            return helper.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + path;
        }
    }
}