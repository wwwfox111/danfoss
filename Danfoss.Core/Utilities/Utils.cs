using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Danfoss.Core.Utilities
{
	/// <summary>
	/// 一些比较零散的工具方法
	/// </summary>
	public sealed class Utils
	{
		/// <summary>
		/// 获取客户端IP地址
		/// </summary>
		/// <returns>IP地址</returns>
		public static string GetClientIP()
		{
			var ip = string.Empty;
			try
			{
				if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null) // 服务器， using proxy
				{
					//得到真实的客户端地址
					ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();  // Return real client IP.
				}
				else//如果没有使用代理服务器或者得不到客户端的ip  not using proxy or can't get the Client IP
				{             //得到服务端的地址
					ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString(); //While it can't get the Client IP, it will return proxy IP.
				}
			}
			catch { }
			return ip;
		}
	}
}
