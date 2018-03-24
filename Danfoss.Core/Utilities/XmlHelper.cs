using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Danfoss.Core.Utilities
{
    /// <summary>
    /// Xml处理操作类（基于System.Xml）
    /// </summary>
    public sealed class XmlHelper
    {
        /// <summary>  
        /// 反序列化XML为类实例  
        /// </summary>  
        /// <typeparam name="T">实例类型</typeparam>  
        /// <param name="xml">Xml字符串</param>  
        /// <param name="rootName">指定Xml的根节点</param>  
        /// <returns>实例</returns>  
        public static T DeserializeXML<T>(string xml, string rootName = null)
        {
            var serializer = string.IsNullOrWhiteSpace(rootName) ?
                        new System.Xml.Serialization.XmlSerializer(typeof(T)) :
                        new System.Xml.Serialization.XmlSerializer(typeof(T), new XmlRootAttribute(rootName));
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
                new XmlSerializer(obj.GetType()).Serialize((TextWriter)writer, obj);
                return writer.ToString();
            }


            //XmlSerializer serializer = new XmlSerializer(obj.GetType());
            //XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //ns.Add("", "");
            //MemoryStream ms = new MemoryStream();
            //XmlWriterSettings settings = new XmlWriterSettings();
            //settings.OmitXmlDeclaration = true;
            //settings.Encoding = new UnicodeEncoding(bigEndian: false, byteOrderMark: false);
            //XmlWriter writer = XmlWriter.Create(ms, settings);
            //serializer.Serialize(writer, obj, ns);

            //return Encoding.Unicode.GetString(ms.ToArray());
        }
    }
}
