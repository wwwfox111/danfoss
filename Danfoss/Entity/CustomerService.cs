using Danfoss.Core.We;
using System;
using System.Collections.Generic;
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
                using (var db = new DanfossDbEntities()) {
                    db.Customer.Add(customer);
                    db.SaveChanges();
                }
                return customer;
            }
            return null;
        }
    }
}