using System.IO;
using System.Net;
using System.Text;

namespace Danfoss.Core.Utilities
{
    /// <summary>
    /// Http相关帮助类
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">目标Url</param>
        /// <returns>字符串</returns>
        public static string Get(string url)
        {
            var client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            var data = client.DownloadString(url);
            return data;
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">目标Url</param>
        /// <returns>字符串</returns>
        public static string Post(string url, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            //request.ContentLength = Encoding.UTF8.GetByteCount(data);    
            var myRequestStream = request.GetRequestStream();
            //StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            var myStreamWriter = new StreamWriter(myRequestStream);
            myStreamWriter.Write(data);
            myStreamWriter.Close();

            var response = (HttpWebResponse)request.GetResponse();

            var myResponseStream = response.GetResponseStream();
            var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            var retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
    }
}
