using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Danfoss.Core
{
    public class Constants
    {
        #region 网站部署环境

        /// <summary>
        /// 站点部署域名（末尾不带斜杠）
        /// </summary>
        public static string SiteDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteDomain"];
            }
        }

        #endregion

        #region 分页参数

        /// <summary>
        /// 每页显示多少条
        /// </summary>
        public const int PageSize = 9;

        /// <summary>
        /// 上一页文本
        /// </summary>
        public const string LastPageText = "上一页";

        /// <summary>
        /// 下一页文本
        /// </summary>
        public const string NextPageText = "下一页";

        /// <summary>
        /// 最多显示多少个分页连接
        /// </summary>
        public const int MaxNrOfPages = 12;

        #endregion

        #region 公众号信息

        /// <summary>
        /// AppId
        /// </summary>
        public static string AppId
        {
            get
            {
                var appId = string.Empty;
                var environment = ConfigurationManager.AppSettings["SiteEnvironment"];
                if (environment != null && environment != "")
                {
                    if (environment == SiteEnvironment.UAT.ToString()) // (测试环境)
                        appId = "wxfdd692381aacf136";
                    else if (environment == SiteEnvironment.PRO.ToString()) // (正式环境)
                        appId = "wxeafc2c148a9db384";
                }
                return appId;
            }
        }

        /// <summary>
        /// AppSecret
        /// </summary>
        public static string AppSecret
        {
            get
            {
                var appSecret = string.Empty;
                var environment = ConfigurationManager.AppSettings["SiteEnvironment"];
                if (environment != null && environment != "")
                {
                    if (environment == SiteEnvironment.UAT.ToString()) // (测试环境)
                        appSecret = "91c8d3c5e02627a6409a7775263ecc8a";
                    else if (environment == SiteEnvironment.PRO.ToString()) // (正式环境)
                        appSecret = "2b712e86f8aabe9640fe6a72576effa9";
                }
                return appSecret;
            }
        }

      

        #endregion
    }
}
