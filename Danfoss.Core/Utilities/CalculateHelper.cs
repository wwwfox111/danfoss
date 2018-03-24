using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Danfoss.Core.Utilities
{
    /// <summary>
    /// 计算帮助类
    /// </summary>
    public sealed class CalculateHelper
    {
        /// <summary>
        /// 根据入住时间和离店时间计算间夜数
        /// </summary>
        /// <param name="enterDate">入住时间</param>
        /// <param name="leaveDate">离店时间</param>
        /// <returns>间夜数</returns>
        public static int GetStayDays(DateTime enterDate, DateTime leaveDate)
        {
            TimeSpan ts = new TimeSpan(enterDate.Ticks);
            TimeSpan tslast = new TimeSpan(leaveDate.Ticks);
            TimeSpan tsDisparity = ts.Subtract(tslast).Duration();
            var days = (int)tsDisparity.TotalDays;
            return days;
        }
    }
}
