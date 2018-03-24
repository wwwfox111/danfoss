using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Danfoss.Controllers
{
    public class BaseController: Controller
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
#if DEBUG
                return "orbpLt2PcsB8gIbkkR7f4exf1Hb8";  //"orbpLt2PcsB8gIbkkR7f4exf1Hb8 666";
#else
            return Session[_ACCOUNT_KEY] != null ? Session[_ACCOUNT_KEY].ToString() : null; 
#endif
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
    }
}