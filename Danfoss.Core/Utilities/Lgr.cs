using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Danfoss.Core.Utilities
{
    public class Lgr
    {
        /// <summary>
        /// 获取在配置文件中的log4net配置对象
        /// </summary>
        public static ILog Log
        {
            get
            {
                return LogManager.GetLogger("log4netMainLogger");
            }
        }
        /// <summary>
        /// 初始化log4net
        /// </summary>
        static Lgr()
        {
            InitLog4Net();
        }
        /// <summary>
        /// 初始化log4net
        /// </summary>
        public static void InitLog4Net()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
