using Danfoss.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Danfoss
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }


        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            string code = HttpContext.Current.Request["code"];
            string state = HttpContext.Current.Request["state"];
            Lgr.Log.Debug(string.Format("用户IP：{0}，请求地址：{1},code:{2},state:{3}", Request.UserHostAddress, Request.Url.ToString(), code, state));
            // 微信请求时，上次请求的URL地址信息没值，暂时不做这个过滤，2015-11-11
            //string urlReferrer = HttpContext.Current.Request.UrlReferrer != null ? HttpContext.Current.Request.UrlReferrer.ToString() : "";
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(state) && state == "1")
            {
                //Session["code"] = code;
                HttpCookie cookie = new HttpCookie("code", code);
                Lgr.Log.Debug(string.Format("Cookie在Remove之前的code：{0}", HttpContext.Current.Response.Cookies["code"] != null ? HttpContext.Current.Response.Cookies["code"].Value : ""));
                HttpContext.Current.Response.Cookies.Remove("code");//清除 
                HttpContext.Current.Response.SetCookie(cookie);
                Lgr.Log.Debug(string.Format("code:{0},Cookie在更新以后中的code：{1}", code, HttpContext.Current.Response.Cookies["code"].Value));
            }
        }


    }
}
