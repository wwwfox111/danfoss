using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Danfoss.Core.Utilities
{
    public class GenerateStaticFileHelper
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";//浏览器

        private static object _lockFile = new object();

        /// <summary>
        /// 生成酒店详细页面
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="wePlatUrl"></param>
        /// <param name="SaveFilePath"></param>
        public static void GenerateHotelDetailPage(int hotelId, string wePlatUrl, string SaveFilePath)
        {
            string detailUrl = string.Format("{0}/Hotel/Detail/{1}.html", wePlatUrl, hotelId);
            string infoUrl = string.Format("{0}/Hotel/infor/{1}.html", wePlatUrl, hotelId);
            string path = string.Format(@"{0}\files\html\hotel\Detail\{1}.html", SaveFilePath, hotelId);
            string infopath = string.Format(@"{0}\files\html\hotel\infor\{1}.html", SaveFilePath, hotelId);
            string content = GetHttpResponseString(detailUrl);
            string infocontent = GetHttpResponseString(infoUrl);
            SaveFile(path, content);
            SaveFile(infopath, infocontent);
        }

        /// <summary>
        /// 商品列表页面
        /// </summary>
       /// <param name="categoryId">商品分类ID</param>
       /// <param name="wePlatUrl"></param>
       /// <param name="SaveFilePath"></param>
        public static void GenerateProductPage(int? categoryId, string wePlatUrl, string SaveFilePath)
        {
            string path = string.Empty;
            string infoUrl = string.Empty;
            if (categoryId != null)
            {
                path = string.Format(@"{0}\files\html\Product\Category\{1}.html", SaveFilePath, categoryId);
                infoUrl = string.Format("{0}/Product/Index?id={1}", wePlatUrl, categoryId);
            }
            else
            {
                path = string.Format(@"{0}\files\html\Product\Category\index.html", SaveFilePath);
                 infoUrl = string.Format("{0}/Product/Index", wePlatUrl);
            }
            string content = GetHttpResponseString(infoUrl);
            SaveFile(path, content);
        }

        
        /// <summary>
        /// 商品详情页面
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="wePlatUrl"></param>
        /// <param name="SaveFilePath"></param>
        public static void GenerateProductDetailPage(int productId, string wePlatUrl, string SaveFilePath)
        {
            string path = string.Format(@"{0}\files\html\Product\Detail\{1}.html", SaveFilePath, productId);
            string infoUrl = string.Format("{0}/Product/ProductDetail?id={1}", wePlatUrl, productId);
            string content = GetHttpResponseString(infoUrl);
            SaveFile(path, content);
        }

        /// <summary>
        /// 生成酒店列表页面
        /// </summary>
        /// <param name="wePlatUrl"></param>
        /// <param name="SaveFilePath"></param>
        public static void GenerateHotelListPage(string wePlatUrl, string SaveFilePath)
        {
            string strHotelListUrl = string.Format("{0}{1}", wePlatUrl, "/hotel/HotelList");
            string content = GetHttpResponseString(strHotelListUrl);
            SaveFile(string.Format(@"{0}\files\html\hotel\HotelList.html", SaveFilePath), content);
        }

        /// <summary>
        /// 某个城市的酒店列表页面
        /// </summary>
        /// <param name="listCity">城市列表</param>
        /// <param name="wePlatUrl">web访问地址</param>
        /// <param name="SaveFilePath"></param>
        public static void GenerateCityPage(List<string> listCity, string wePlatUrl, string SaveFilePath)
        {
            string strHotelListUrl = string.Format("{0}{1}", wePlatUrl, "/hotel/HotelList");
            foreach (var city in listCity)
            {
                string hotelListPath = string.Format(@"{0}\files\html\hotel\HotelList{1}.html", SaveFilePath, city);
                string hotelListContent = GetHttpResponseString(strHotelListUrl, city);
                SaveFile(hotelListPath, hotelListContent);
            }
        }



        /// <summary>
        /// 根据城市获取酒店列表页面字符串
        /// </summary>
        /// <param name="url"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public static string GetHttpResponseString(string url, string city)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", string.Format("\"{0}\"",url));
            Cookie cookie = new Cookie();
            cookie.Name = "SearchData";
            cookie.Domain = "localhost";
            cookie.Path = "/";
            SearchData model = new SearchData() { City = city, EnterDate = DateTime.Now.ToString("yyyy-MM-dd"), LeaveDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), HotelLevel = "", HotelName = "" };
            cookie.Value = Uri.EscapeUriString(JsonHelper.ToJson<SearchData>(model)).Replace(":", "%3A").Replace(",", "%2C");
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(cookie);
            var response = request.GetResponse() as HttpWebResponse;
            StreamReader sReader = null;
            Stream responseStream = response.GetResponseStream();
            // 对接响应流(以"utf-8"字符集)  
            sReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));

            // 开始读取数据  
            string value = sReader.ReadToEnd();
            //强制关闭  
            if (sReader != null)
            {
                sReader.Close();
            }
            if (responseStream != null)
            {
                responseStream.Close();
            }
            if (response != null)
            {
                response.Close();
            }
            return value;
        }

        /// <summary>
        /// 存储文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void SaveFile(string path, string content)
        {
            //Stream stream = 
            lock (_lockFile)
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                if (!File.Exists(path))
                {
                    FileStream sw = File.Create(path);                    
                    sw.Close();
                }
                using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.Write(content);                    
                    sw.Close();
                }
            }
        }

        /// <summary>
        ///  获取请求页面的HTML字符串
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHttpResponseString(string url)
        {
            var result = string.Empty;
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                result = client.DownloadString(url);
            }
            return result;
        }
    }

    public class SearchData
    {
        public string City { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string HotelLevel { get; set; }
        public string EnterDate { get; set; }
        public string LeaveDate { get; set; }
        public string HotelName { get; set; }
    }
}
