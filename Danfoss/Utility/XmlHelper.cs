using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Serialization;


namespace Danfoss.Utility
{
    /// <summary>
    /// Xml处理操作类（基于System.Xml）
    /// </summary>
    public sealed class XmlHelper
    {

        static System.Collections.Concurrent.ConcurrentDictionary<string, XmlSerializer> xmlSerializerDic = new System.Collections.Concurrent.ConcurrentDictionary<string, XmlSerializer>();

        static Timer clearTimer;


        public static string GenerateKey(Type type, string myRoot)
        {
            StringBuilder strb = new StringBuilder(type.FullName);
            if (!string.IsNullOrEmpty(myRoot))
            {
                strb.Append("_").Append(myRoot);
            }
            return strb.ToString();
        }

        public static XmlSerializer GetXmlSerializer<T>(string rootName = null)
        {
            var key = GenerateKey(typeof(T), rootName);
            if (xmlSerializerDic.TryGetValue(key, out XmlSerializer serializer) == false)
            {
                serializer = string.IsNullOrWhiteSpace(rootName) ?
                 new System.Xml.Serialization.XmlSerializer(typeof(T)) :
                 new System.Xml.Serialization.XmlSerializer(typeof(T), new XmlRootAttribute(rootName));
                xmlSerializerDic.TryAdd(key, serializer);
            }

            if (clearTimer == null)
            {
                clearTimer = new Timer
                {
                    Interval = 300000
                };
                clearTimer.Elapsed += delegate {
                    xmlSerializerDic.Clear();
                    clearTimer.Stop();
                };
                clearTimer.Start();
            }


            return serializer;

        }

        /// <summary>  
        /// 反序列化XML为类实例  
        /// </summary>  
        /// <typeparam name="T">实例类型</typeparam>  
        /// <param name="xml">Xml字符串</param>  
        /// <param name="rootName">指定Xml的根节点</param>  
        /// <returns>实例</returns>  
        public static T DeserializeXML<T>(string xml, string rootName = null)
        {
            var serializer = GetXmlSerializer<T>(rootName);
            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>  
        /// 序列化类实例为XML  
        /// </summary>  
        /// <typeparam name="T">实例类型</typeparam>  
        /// <param name="obj">实例</param>  
        /// <returns>Xml字符串</returns>  
        public static string SerializeXML<T>(T obj)
        {
            using (var writer = new StringWriter())
            {
                var serializer = GetXmlSerializer<T>();
                serializer.Serialize(writer, obj);
                return writer.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            }
        }

        /// <summary>  
        /// 序列化类实例为XML  
        /// </summary>  
        /// <typeparam name="T">实例类型</typeparam>  
        /// <param name="obj">实例</param>  
        /// <returns>Xml字符串</returns>  
        public static string SerializeXML_UTF8<T>(T obj)
        {
            using (var writer = new StringWriter())
            {
                var serializer = GetXmlSerializer<T>();
                serializer.Serialize(writer, obj);
                return writer.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version =\"1.0\" encoding=\"utf-8\"?>");
            }
        }

        public static string SerializeXML<T>(T obj, string rootName)
        {
            using (var writer = new StringWriter())
            {
                var xmlSerializer = GetXmlSerializer<T>(rootName);
                xmlSerializer.Serialize(writer, obj);
                return writer.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            }
        }


    }
}