using Danfoss.Core.Utilities;
using Danfoss.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
namespace Danfoss.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (DanfossDbEntities db = new DanfossDbEntities())
            {
              var list =  db.Customer.AsNoTracking().ToList();
                Lgr.Log.Info(JsonConvert.SerializeObject(list));
            }


                return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SendEmail(string emailAddress,string content)
        {
         var result=   EmailHelper.SendEmail(emailAddress, "邮件测试", content);
            return Json(new { IsSuccess=result});
        }
    }
}