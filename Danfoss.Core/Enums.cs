using System;

namespace Danfoss.Core
{
    #region 枚举

    /// <summary>
    /// 通用枚举（应用于只有true或fasle两种状态的类型）
    /// </summary>
    public enum CommonStatus
    {
        /// <summary>
        /// 是，未激活，失效，禁用
        /// </summary>
        False = 0,
        /// <summary>
        /// 否，激活，有效，启用
        /// </summary>
        True = 1
    }

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
    {
        All = 0,
        Debug = 10,
        Info = 20,
        Warning = 30,
        Error = 40,
        Fatal = 50,
        Off = 100
    }

    /// <summary>
    /// 站点环境
    /// </summary>
    public enum SiteEnvironment
    {
        /// <summary>
        /// 开发
        /// </summary>
        DEV = 0,
        /// <summary>
        /// 测试（默认）
        /// </summary>
        UAT = 1,
        /// <summary>
        /// 正式
        /// </summary>
        PRO = 2
    }

    #endregion

    #region 枚举扩展

    public static class EnumExtensions
    {
        public static string ToName(this CommonStatus commonStatus)
        {
            var name = string.Empty;
            switch (commonStatus)
            {
                case CommonStatus.False:
                    name = "否"; break;
                case CommonStatus.True:
                    name = "是"; break;
            }
            return name;
        }

        public static int ToValue(this CommonStatus commonStatus)
        {
            return (int)commonStatus;
        }

        public static string ToName(this LogLevel logLevel)
        {
            var name = string.Empty;
            switch (logLevel)
            {
                case LogLevel.All:
                    name = "所有"; break;
                case LogLevel.Debug:
                    name = "调试"; break;
                case LogLevel.Info:
                    name = "信息"; break;
                case LogLevel.Warning:
                    name = "警告"; break;
                case LogLevel.Error:
                    name = "错误"; break;
                case LogLevel.Fatal:
                    name = "严重"; break;
                case LogLevel.Off:
                    name = "关闭"; break;
            }
            return name;
        }

        public static int ToValue(this LogLevel logLevel)
        {
            return (int)logLevel;
        }
    }

    #endregion
}
