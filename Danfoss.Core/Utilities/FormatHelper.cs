using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text;
using System.IO;

namespace Danfoss.Core.Utilities
{
    /// <summary>
    /// 格式化帮助类
    /// </summary>
    public sealed class FormatHelper
    {
        /// <summary>
        /// 显示金额 eg:0.00
        /// </summary>
        /// <param name="val">decimal类型值</param>
        /// <returns>金额格式字符串</returns>
        public static string ShowMoney(decimal val, bool currency = false, bool place = false)
        {
            return val.ToString("f2");
        }

        /// <summary>
        /// 保留日期部分，设置时分秒为0
        /// </summary>
        /// <param name="date">时间</param>
        /// <returns>时分秒为0的时间</returns>
        public static DateTime ShortDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        /// <summary>
        /// xml格式化
        /// </summary>
        /// <param name="sUnformattedXml">未格式化的xml</param>
        /// <returns>格式化的xml</returns>
        public static string FormatXml(string sUnformattedXml)
        {
            if (sUnformattedXml == "")
                return null;
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(sUnformattedXml);
            }
            catch { return sUnformattedXml; }
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw);
                xtw.Formatting = Formatting.Indented;
                xtw.Indentation = 1;
                xtw.IndentChar = '\t';
                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
        }  
    }
}
