using Danfoss.Core;
using Danfoss.Core.Utilities;
using Danfoss.Core.We;
using Danfoss.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Danfoss.Controllers
{
    public class BaseController : Controller
    {


        /// <summary>
        /// 微信用户帐号KEY
        /// </summary>
        public const string _ACCOUNT_KEY = "WE_ACCOUNT";


        /// <summary>
        /// 获取当前访问的微信用户帐号
        /// </summary>
        /// <returns></returns>
        public string CurAccount
        {
            get
            {
                //return "o9NzmstXQ-Wbbl75iGuUUh5hll5I"; // "orbpLt2PcsB8gIbkkR7f4exf1Hb8";
                //#if DEBUG
                //                return "orbpLt2PcsB8gIbkkR7f4exf1Hb8";  //"orbpLt2PcsB8gIbkkR7f4exf1Hb8 666";
                //#else
                return Session[_ACCOUNT_KEY]?.ToString();
                //#endif
            }
        }


        /// <summary>
        /// 设置当前用户帐号
        /// </summary>
        /// <param name="account"></param>
        public void SetCurAccount(string account)
        {
            Session[_ACCOUNT_KEY] = account;
        }

        /// <summary>
        /// 清掉当前用户帐号
        /// </summary>
        public void ClearCurAccount()
        {
            Session[_ACCOUNT_KEY] = null;
        }

        /// <summary>
        /// 用户是否已经验证（登录）
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return this.CurAccount != null;
            }
        }

        /// <summary>
        /// 微信用户oauth方式认证
        /// </summary>
        public ActionResult WeOAuth(out Customer customer, string actionUrl)
        {
            customer = null;
            if (!this.IsAuthenticated)
            {
                //var code = Request["code"];
                Lgr.Log.Info(string.Format("用户IP:{0} HttpContext.Request.Cookies.Count:{1},UserAgent:{2},UserHostName:{3}", HttpContext.Request.UserHostAddress, HttpContext.Request.Cookies.Count, HttpContext.Request.UserAgent, HttpContext.Request.UserHostName));
                string code = HttpContext.Request["code"];
                Lgr.Log.Info(string.Format("WeOAuth认证时 HttpContext.Request.Cookie:{0},用户IP:{1}", code, HttpContext.Request.UserHostAddress));
                if (string.IsNullOrEmpty(code))
                    return Redirect(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=1#wechat_redirect",
                        Constants.AppId, Constants.SiteDomain + actionUrl));

                #region 获取微信用户信息
                try
                {
                    //获取微信用户openid
                    Lgr.Log.Info(string.Format("WeOAuth获取微信用户信息时，code：{0}", code));
                    var openId = WeHelper.GetOpenId(code);
                    if (string.IsNullOrEmpty(openId))
                        throw new Exception("微信用户认证失败");

                    //检查openid是否已经在数据库中存在
                    customer = CustomerService.GetByopenId(openId);
                    if (customer == null)
                    {
                        #region 如果关注的用户未保存到数据库，重新获取用户信息并保存
                        //获取用户详细信息 
                        var userInfo = WeHelper.GetUserInfo(openId);
                        if (userInfo.Subscribe) //只有用户关注了公众号才能获取到用户其他信息
                        {
                            //数据库中检查微信用户
                            customer = CustomerService.CheckNewWeAccount(userInfo);
                        }
                        else
                        {
                            return RedirectToAction("FollowQrCode");
                        }
                        #endregion
                    }

                    //设置当前登录的微信用户
                    SetCurAccount(openId);
                }
                #region 2015-11-16 遇到不合法的oauth_code 再次请求获取Code
                catch (KeyNotFoundException ex)
                {
                    Lgr.Log.Error(ex.Message, ex);
                    Lgr.Log.Info(string.Format("URL:{0}", HttpContext.Request.Url));
                    return Redirect(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=1#wechat_redirect",
                        Constants.AppId, Constants.SiteDomain + actionUrl));
                }
                #endregion
                catch (Exception ex)
                {
                    Lgr.Log.Error(ex.Message, ex);
                }
                #endregion
            }
            else
            {
                Lgr.Log.Debug(string.Format("Session[WE_ACCOUNT]:{0},actionUrl:{1}", this.CurAccount, actionUrl));
                customer = CustomerService.GetByopenId(this.CurAccount);
            }

            if (!this.IsAuthenticated)
            {
                Lgr.Log.Debug("BaseController:IsAuthenticated:" + this.IsAuthenticated);
                throw new Exception("用户认证失败");
            }
            if (customer == null)
            {
                Lgr.Log.Debug("BaseController:customer == null" + customer == null);
                throw new Exception("用户认证失败");
            }

            return null;
        }

    }
}