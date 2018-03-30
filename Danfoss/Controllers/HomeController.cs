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
using System.Data;
using Danfoss.Core.Utility;

namespace Danfoss.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {

            #region 微信认证 
            Customer customer = null;
            var actionResult = WeOAuth(out customer, Url.Action("Index"));
            if (actionResult != null)
                return actionResult;
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
            var content = WebHelper.GetViewHtml(this.ControllerContext, "~/Views/Shared/EmailTemplate_New.cshtml", solutions);
            var result = EmailHelper.SendEmail(emailAddress, "丹佛斯资料下载", content);
            if (result)
            {
                #region  保存邮箱地址
                //using (var db = new DanfossDbEntities())
                //{
                //    var model = db.Customer.FirstOrDefault(o => o.OpenId == CurAccount);
                //    if (model != null)
                //    {
                //        model.Email = emailAddress;
                //        db.Customer.Attach(model);
                //        db.Entry(model).State = EntityState.Modified;
                //        db.SaveChanges();
                //    }
                //}
                var fileName = string.Empty;
                solutions.ForEach(o =>
                {
                    fileName += string.Join(",", o.Products.Select(t => t.FileUrl))+",";
                });
                fileName = fileName.Replace("/content/download/", "");
                CustomerService.AddSendEmailLog(new SendEmailLog
                {
                    Email = emailAddress,
                    CreateTime = DateTime.Now,
                    OpenId = CurAccount,
                    SendContent = fileName
                });
                #endregion 
            }
            return Json(new { IsSuccess = result });
        }

        public ActionResult Detail(int id)
        {
            ViewBag.Id = id;
            #region 微信认证 
            Customer customer = null;
            var actionResult = WeOAuth(out customer, Url.Action("Detail", new { id = id }));
            if (actionResult != null)
                return actionResult;
            #endregion
            var solution = LocalDataProvider.Current.FindSolutionById(id);
            return View(solution);
        }

        public ActionResult Download()
        {
            #region 微信认证 
            Customer customer = null;
            var actionResult = WeOAuth(out customer, Url.Action("Download"));
            if (actionResult != null)
                return actionResult;
            #endregion
            return View();
        }

        /// <summary>
        /// 生成二维码 
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateQrCode()
        {
            return View();
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public ActionResult Generate(int id, int pixels)
        {
            var data = LocalDataProvider.Current.FindSolutionById(id);
            var url = Url.Action("Detail", new { id = id });
            url = Request.Url.GetLeftPart(UriPartial.Authority) + url;
            var iconPath = Server.MapPath("/Content/images/logo.jpg");
            MemoryStream ms = QrCodeHelper.RenderQrCode(url, "H", pixels, iconPath);
            return File(ms.ToArray(), "image/jpeg", DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpeg");
        }

        /// <summary>
        /// 导出用户信息 execl 
        /// </summary>
        /// <returns></returns>
        public FileResult ExportUserInfo()
        {
            var list = CustomerService.GetList();
            if (list != null && list.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("昵称");
                dt.Columns.Add("OpenId");
                dt.Columns.Add("性别");
                dt.Columns.Add("国家");
                dt.Columns.Add("省份");
                dt.Columns.Add("城市");
                dt.Columns.Add("邮箱地址");
                foreach (var item in list)
                {
                    DataRow row = dt.NewRow();
                    row["昵称"] = item.NickName;
                    row["OpenId"] = item.OpenId;
                    row["性别"] = item.Sex.HasValue ? (item.Sex.Value == 1 ? "男" : item.Sex.Value == 2 ? "女" : "未知") : "未知";
                    row["国家"] = item.Country;
                    row["省份"] = item.Province;
                    row["城市"] = item.City;
                    row["邮箱地址"] = item.Email;
                    dt.Rows.Add(row);
                }
                var ms = ExcelHelper.ExportToExcel(dt);
                return File(ms, "application/vnd.ms-excel", "用户信息-" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
            }
            return null;
        }

        /// <summary>
        /// 导出发送邮件记录
        /// </summary>
        /// <returns></returns>
        public FileResult ExportSendEmailLog()
        {
            var list = CustomerService.GetSendEmailLog();
            if (list != null && list.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("昵称");
                dt.Columns.Add("OpenId");
                dt.Columns.Add("Email地址");
                dt.Columns.Add("发送内容");
                dt.Columns.Add("发送时间");
                foreach (var item in list)
                {
                    DataRow row = dt.NewRow();
                    row["昵称"] = item.NickName;
                    row["OpenId"] = item.OpenId;
                    row["Email地址"] = item.Email;
                    row["发送内容"] = item.SendContent;
                    row["发送时间"] = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    dt.Rows.Add(row);
                }
                var ms = ExcelHelper.ExportToExcel(dt);
                return File(ms, "application/vnd.ms-excel", "邮件发送记录-" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
            }
            return null;
        }

        /// <summary>
        /// 展示关注二维码页面
        /// </summary>
        /// <returns></returns>
        public ActionResult FollowQrCode()
        {
            return View();
        }


    }
}