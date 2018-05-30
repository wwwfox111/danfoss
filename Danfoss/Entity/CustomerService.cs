using Danfoss.Core.Utilities;
using Danfoss.Core.We;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Danfoss.Entity
{
    public static class CustomerService
    {
        /// <summary>
        /// 获取所有用户资料
        /// </summary>
        /// <returns></returns>
        public static List<Customer> GetList()
        {
            using (var db = new DanfossDbEntities())
            {
                return db.Customer.ToList();
            }

        }

        /// <summary>
        /// 根据openid 获取 用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static Customer GetByopenId(string openId)
        {
            using (var db = new DanfossDbEntities())
            {
                return db.Customer.FirstOrDefault(o => o.OpenId == openId);
            }
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model"></param>
        public static void AddCustomer(Customer model)
        {
            using (var db = new DanfossDbEntities())
            {
                db.Customer.Add(model);
                db.SaveChanges();
            }
        }


        public static Customer CheckNewWeAccount(UserInfo userInfo)
        {
            var existCustomer = GetByopenId(userInfo.OpenId);
            //如果用户不存在，则在第一次检查的时候保存用户信息
            if (existCustomer == null)
            {
                var customer = new Customer
                {
                    OpenId = userInfo.OpenId,
                    NickName = userInfo.NickName,
                    HeadImgUrl = userInfo.HeadImgUrl,
                    Sex = int.Parse(userInfo.Sex),
                    CreatorTime = DateTime.Now,
                    Country = userInfo.Country,
                    City = userInfo.City,
                    Province = userInfo.Province
                };
                using (var db = new DanfossDbEntities())
                {
                    db.Customer.Add(customer);
                    db.SaveChanges();
                }
                return customer;
            }
            return null;
        }

        /// <summary>
        ///  增加邮件发送记录 
        /// </summary>
        /// <param name="log"></param>
        public static void AddSendEmailLog(SendEmailLog log)
        {
            try
            {
                using (var db = new DanfossDbEntities())
                {

                    var model = db.Customer.FirstOrDefault(o => o.OpenId == log.OpenId);
                    if (model != null)
                    {
                        model.Email = log.Email;
                        db.Customer.Attach(model);
                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SendEmailLog.Add(log);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Lgr.Log.Error(ex.Message, ex);
            }
        }
        /// <summary>
        /// 获取发送邮件列表
        /// </summary>
        public static List<SendEmailLogDto> GetSendEmailLog()
        {
            List<SendEmailLogDto> result = new List<SendEmailLogDto>();
            try
            {
                using (var db = new DanfossDbEntities())
                {
                    var query = from a in db.SendEmailLog
                                join b in db.Customer on a.OpenId equals b.OpenId
                                select new SendEmailLogDto
                                {
                                    CreateTime = a.CreateTime,
                                    Email = a.Email,
                                    NickName = b.NickName,
                                    OpenId = a.OpenId,
                                    SendContent = a.SendContent
                                };
                    result = query.ToList();
                }
            }
            catch (Exception ex)
            {
                Lgr.Log.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 增加分享次数
        /// </summary>
        /// <param name="log"></param>
        public static void AddSharesLog(SharesLog log)
        {
            using (var db = new DanfossDbEntities())
            {
                var model = db.SharesLog.FirstOrDefault(o => o.OpenId == log.OpenId);
                if (model != null)
                {
                    model.Shares += 1;
                    db.SharesLog.Attach(model);
                    db.Entry(model).State = EntityState.Modified;
                }
                else
                {
                    db.SharesLog.Add(log);
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        ///  获取分享列表
        /// </summary>
        /// <returns></returns>
        public static List<SharesLogDto> GetSharesLog()
        {
            List<SharesLogDto> result = new List<SharesLogDto>();
            try
            {
                using (var db = new DanfossDbEntities())
                {
                    var query = from a in db.SharesLog
                                join b in db.Customer on a.OpenId equals b.OpenId
                                select new SharesLogDto
                                {
                                    NickName = b.NickName,
                                    OpenId = a.OpenId,
                                    Shares = a.Shares
                                };
                    result = query.ToList();
                }
            }
            catch (Exception ex)
            {
                Lgr.Log.Error(ex.Message, ex);
            }
            return result;
        }

    }

        public class SharesLogDto
        {

            public string NickName { get; set; }
            public string OpenId { get; set; }

            public int Shares { get; set; }
        }

        public class SendEmailLogDto
        {

            public string NickName { get; set; }
            public string OpenId { get; set; }
            public string Email { get; set; }
            public string SendContent { get; set; }
            public System.DateTime CreateTime { get; set; }
        }
    }