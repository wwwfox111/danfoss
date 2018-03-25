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
                        appId = "wx6fb80f8e8ecd51a7";
                    else if (environment == SiteEnvironment.PRO.ToString()) // (正式环境)
                        appId = "wx6fb80f8e8ecd51a7";
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
                        appSecret = "d2601a8cc004cd4213b8af68206f6faf";
                    else if (environment == SiteEnvironment.PRO.ToString()) // (正式环境)
                        appSecret = "d2601a8cc004cd4213b8af68206f6faf";
                }
                return appSecret;
            }
        }

      

        #endregion
    }
}
