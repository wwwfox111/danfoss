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

namespace Danfoss.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (DanfossDbEntities db = new DanfossDbEntities())
            {
                var list = db.Customer.AsNoTracking().ToList();
                Lgr.Log.Info(JsonConvert.SerializeObject(list));
            }


            return View();
        }

        public ActionResult Test()
        {
            var data = LocalDataProvider.Current.GetDataSource();
            var str = WebHelper.GetViewHtml(this.ControllerContext, "~/Views/Shared/EmailTemplate.cshtml", data.Solutions);
            return Content(str);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SendEmail(string emailAddress,string content)
        {
         var result=   EmailHelper.SendEmail(emailAddress, "邮件测试", content);
            return Json(new { IsSuccess=result});
        }


        public ActionResult DataSource()
        {
            //var data = new SolutionData();
            //data.Solutions.Add(new Solution
            //{
            //    Id = 1,
            //    Name = "丹佛斯冷水机组解决方案",
            //    BigPicUrl = "/content/images/product-1-3x.jpg",
            //    SmallPicUrl = "/content/images/product-1-3x.jpg",
            //    Items = new List<ProductItem> {
            //       new ProductItem{
            //           Id=1,
            //           Name="冷水机组",
            //           Desc="由内至外，面面俱到",
            //           BigPicUrl="/content/images/product-1-3x.jpg",
            //           SmallPicUrl="/content/images/product-1-3x.jpg",
            //           DownloadUrl="http://www.baidu.com/download.pdf"
            //       },
            //       new ProductItem{
            //               Id=2,
            //           Name="冷水机组",
            //           Desc="由内至外，面面俱到",
            //           BigPicUrl="/content/images/product-1-3x.jpg",
            //           SmallPicUrl="/content/images/product-1-3x.jpg",
            //           DownloadUrl="http://www.baidu.com/download.pdf"
            //       }
            //     }
            //});
            //data.Solutions.Add(new Solution
            //{
            //    Id = 1,
            //    Name = "丹佛斯冷水机组解决方案",
            //    BigPicUrl = "/content/images/product-1-3x.jpg",
            //    SmallPicUrl = "/content/images/product-1-3x.jpg",
            //    Items = new List<ProductItem> {
            //       new ProductItem{
            //           Id=1,
            //           Name="冷水机组",
            //           Desc="由内至外，面面俱到",
            //           BigPicUrl="/content/images/product-1-3x.jpg",
            //           SmallPicUrl="/content/images/product-1-3x.jpg"
            //       },
            //       new ProductItem{
            //               Id=2,
            //           Name="冷水机组",
            //           Desc="由内至外，面面俱到",
            //           BigPicUrl="/content/images/product-1-3x.jpg",
            //           SmallPicUrl="/content/images/product-1-3x.jpg"
            //       }
            //     }
            //}); ;
            var data = LocalDataProvider.Current.GetDataSource();
            return this.Content(XmlHelper.SerializeXML(data));
        }

    }
}