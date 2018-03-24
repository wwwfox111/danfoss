using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danfoss.Core.We
{
    /// <summary>
    /// 微信用户详细信息 参看：http://mp.weixin.qq.com/wiki/14/bb5031008f1494a59c6f71fa0f319c66.html
    /// </summary>
    public class UserInfo
    {

        public bool Subscribe { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string Sex { get; set; }
        public string Language { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string HeadImgUrl { get; set; }
        public string SubscribeTime { get; set; }
        public string UnionId { get; set; }
        public string Remark { get; set; }
        public string GroupId { get; set; }
    }
}
