﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Danfoss
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
             "SolutionDetail",
             "solution/detail/{id}",
             new { controller = "Home", action = "Detail", id = 0 },
             new { id = @"\d*" }
             );

            routes.MapRoute(
              "DownloadAll",
              "download",
              new { controller = "Home", action = "Download" }
              );
            routes.MapRoute(
                "GenerateQrCode",
                "GenerateQrCode",
                new { controller = "Home", action = "GenerateQrCode" }
                );
            routes.MapRoute(
              "Export",
              "Export",
              new { controller = "Home", action = "Test" }
              );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
