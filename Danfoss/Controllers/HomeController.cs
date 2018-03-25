using Danfoss.Core.Utilities;
using Danfoss.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Danfoss.Models;
using Danfoss.Data;
using System.Data.Entity;
using System.IO;

namespace Danfoss.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            
            #region 微信认证 
            //Customer customer = null;
            //var actionResult = WeOAuth(out customer, Url.Action("Index"));
            //if (actionResult != null)
            //    return actionResult;
            #endregion 
            //using (DanfossDbEntities db = new DanfossDbEntities())
            //{
            //    var list = db.Customer.AsNoTracking().ToList();
            //    Lgr.Log.Info(JsonConvert.SerializeObject(list));
            //}
            // MemoryStream ms = QrCodeHelper.RenderQrCode("sdfsadfsdfsa", "H", 20);
            //return File(ms.ToArray(), "image/jpeg");
            return View();
        }

        public ActionResult Test()
        {
            //var data = LocalDataProvider.Current.GetAll();
            //var str = WebHelper.GetViewHtml(this.ControllerContext, "~/Views/Shared/EmailTemplate.cshtml", data.Solutions);
            //return Content(str);
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SendEmail(string emailAddress, List<Solution> solutions)
        {
            var content = WebHelper.GetViewHtml(this.ControllerContext, "~/Views/Shared/EmailTemplate.cshtml", solutions);
            var result = EmailHelper.SendEmail(emailAddress, "丹佛斯资料下载", content);
            if (result)
            {
                #region  保存邮箱地址
                using (var db = new DanfossDbEntities())
                {
                    var model = db.Customer.FirstOrDefault(o => o.OpenId == CurAccount);
                    if (model != null)
                    {
                        model.Email = emailAddress;
                        db.Customer.Attach(model);
                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                #endregion 
            }
            return Json(new { IsSuccess = result });
        }

        public ActionResult Detail(int id)
        {
            var solution = LocalDataProvider.Current.FindSolutionById(id);
            return View(solution);
        }

        public ActionResult Download()
        {
            return View();
        }

    }
}