using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Danfoss.Models
{
    /// <summary>
    /// 解决方案项目
    /// </summary>
    public class ProductSolution
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 解决方案名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 大图（详情页使用）
        /// </summary>
        public string BigPicUrl { get; set; }
        /// <summary>
        /// 小图（首页使用）
        /// </summary>
        public string SmallPicUrl { get; set; }

        /// <summary>
        /// 解决方案产品资料
        /// </summary>
        public List<ProductItem> Items { get; set; }

    }

    public class ProductItem
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 资料名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 小图
        /// </summary>
        public string SmallPicUrl { get; set; }

        /// <summary>
        /// 大图
        /// </summary>
        public string BigPicUrl { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadUrl { get; set; }
    }
}