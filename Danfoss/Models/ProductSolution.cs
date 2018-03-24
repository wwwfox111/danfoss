using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Danfoss.Models
{
    [XmlRoot(ElementName = "Data")]
    public class SolutionData
    {
        public List<Solution> Solutions { get; set; } = new List<Solution>();
    }


    /// <summary>
    /// 解决方案项目
    /// </summary>
    public class Solution
    {
        /// <summary>
        /// ID
        /// </summary>
        [XmlAttribute("Id")]
        public int Id { get; set; }
        /// <summary>
        /// 解决方案名称
        /// </summary>
        [XmlAttribute("Title")]
        public string Title { get; set; }
        /// <summary>
        /// 大图（详情页使用）
        /// </summary>
        [XmlAttribute("BigPicUrl")]
        public string BigPicUrl { get; set; }
        /// <summary>
        /// 小图（首页使用）
        /// </summary>
        [XmlAttribute("SmallPicUrl")]
        public string SmallPicUrl { get; set; }

        [XmlIgnore]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 解决方案产品资料
        /// </summary>
        public List<ProductItem> Products { get; set; } = new List<ProductItem>();

    }

    public class ProductItem
    {
        /// <summary>
        /// ID
        /// </summary>
        [XmlAttribute("Id")]
        public int Id { get; set; }
        /// <summary>
        /// 资料名称
        /// </summary>
        [XmlAttribute("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        [XmlAttribute("SubTitle")]
        public string SubTitle { get; set; }
        /// <summary>
        /// 小图
        /// </summary>
        [XmlAttribute("SmallPicUrl")]
        public string SmallPicUrl { get; set; }

        /// <summary>
        /// 大图
        /// </summary>
        [XmlAttribute("BigPicUrl")]
        public string BigPicUrl { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        [XmlAttribute("FileUrl")]
        public string FileUrl { get; set; }


    }
}