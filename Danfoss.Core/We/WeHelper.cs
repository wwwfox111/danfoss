using Danfoss.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Danfoss.Core.We
{
   public sealed class WeHelper
    {

        //微信ACCESS_TOKEN缓存Key
        public const string KEY_WX_ACCESS_TOKEN = "KEY_WX_ACCESS_TOKEN";
        //微信JSAPI_TICKET缓存key
        public const string KEY_WX_JSAPI_TICKET = "KEY_WX_JSAPI_TICKET";

        #region 认证基础

        /// <summary>
        /// 获得全局access_token
        /// </summary>
        /// <returns>access_token</returns>
        public static string GetAccessToken()
        {
            //先从缓存中获取access_token
            var accessToken = CacheHelper.GetCache(KEY_WX_ACCESS_TOKEN);
            if (accessToken != null)
                return accessToken.ToString();

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}"
                , Constants.AppId, Constants.AppSecret);
            var json = HttpHelper.Get(url);
            var dic = JsonHelper.ConvertToDictionary(json);
            if (dic.ContainsKey("errcode")) // 获取access_token失败 eg:{"errcode":40013,"errmsg":"invalid appid"}
                return "";

            //缓存access_token
            //微信获取access_token接口每日调用限额2000次，所以我们缓存60s
            //微信官方建议缓存1分钟 参考：http://mp.weixin.qq.com/wiki/6/01405db0092f76bb96b12a9f954cd866.html#.E6.8A.A5.E8.AD.A6.E5.86.85.E5.AE.B9.E8.AF.B4.E6.98.8E
            CacheHelper.SetCache(KEY_WX_ACCESS_TOKEN, dic["access_token"], 60);

            return dic["access_token"];
        }

        /// <summary>
        /// 获取OAuth2方式access_token
        /// </summary>
        /// <param name="code">code</param>
        /// <returns>AccessToken</returns>
        public static string GetOAuth2AccessToken(string code)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code"
                , Constants.AppId, Constants.AppSecret, code);
            var json = HttpHelper.Get(url);
            var dic = JsonHelper.ConvertToDictionary(json);
            return dic["access_token"];
        }

        /// <summary>
        /// 获取全局jsapi_ticket
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static string GetJsApiTicket(string accessToken)
        {
            var ticket = CacheHelper.GetCache(KEY_WX_JSAPI_TICKET);
            if (ticket != null)
                return ticket.ToString();

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi"
                , accessToken);
            var resultJson = HttpHelper.Get(url);
            var dic = JsonHelper.ConvertToDictionary(resultJson);
            //缓存jsapi_ticket 
            //过期时间7200秒，提前600秒重新获取
            CacheHelper.SetCache(KEY_WX_JSAPI_TICKET, dic["ticket"], 7200 - 600);
            if (dic["errcode"].ToString() != "0")
            {
                return "";
            }
            return dic["ticket"].ToString();
        }

        /// <summary>
        /// 获得AppId字符串
        /// </summary>
        /// <returns></returns>
        private static string GetAppId()
        {
            return Danfoss.Core.Constants.AppId;
        }

        /// <summary>
        /// 获取生成jsapi_ticket所用的时间戳（秒数）
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            return Math.Round(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds).ToString();
        }

        /// <summary>
        /// 生成JS-SDK使用权限签名
        /// </summary>
        /// <param name="timestamp">签名时间戳</param>
        /// <param name="noncestr">签名随机串</param>
        /// <returns></returns>
        public static string GenSignature(string timestamp, string noncestr, string jsapi_ticket, string url)
        {
            var tmpStr = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapi_ticket, noncestr, timestamp, url);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            return tmpStr;
        }

        /// <summary>
        /// 返回JS-SDK使用时需要配置的内容信息
        /// </summary>
        /// <param name="url">调用JS接口页面的完整URL(不需要#和之后的信息)</param>
        /// <returns></returns>
        public static WxJsSdkConfigData GetWxConfigObject(string url)
        {
            var appId = GetAppId();
            var timestamp = GetTimeStamp();
            var nonceStr = Guid.NewGuid().ToString().Replace("-", "");
            var accessToken = GetAccessToken();
            var jsapi_ticket = GetJsApiTicket(accessToken);
            var signature = GenSignature(timestamp, nonceStr, jsapi_ticket, url);
            return new WxJsSdkConfigData()
            {
                appId = appId,
                timestamp = timestamp,
                nonceStr = nonceStr,
                signature = signature
            };
        }

        /// <summary>
        /// 微信JSSDK配置数据结构
        /// </summary>
        public class WxJsSdkConfigData
        {
            public string appId { set; get; }
            public string timestamp { set; get; }
            public string nonceStr { set; get; }
            public string signature { set; get; }
        }
        #endregion

        #region 微信用户信息

        /// <summary>
        /// 根据微信OAuth2.0接口返回的code获取用户openid
        /// </summary>
        /// <param name="code">code</param>
        /// <returns>openid</returns>
        public static string GetOpenId(string code)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", Constants.AppId, Constants.AppSecret, code);
            var json = HttpHelper.Get(url);
            Lgr.Log.Info(string.Format("用户IP：{0}，Code：{2}，GetOpenId返回JSON值：{1}", HttpContext.Current.Request.UserHostAddress, json, code));
            var dic = JsonHelper.ConvertToDictionary(json);
            var openId = dic["openid"];
            //var access_token = dic["access_token"]; //oauth2方式token，不同于基础接口的token
            return openId;
        }

        /// <summary>
        /// 根据openid获取用户详细信息
        /// </summary>
        /// <param name="openId">openId</param>
        /// <returns>UserInfo</returns>
        public static UserInfo GetUserInfo(string openId)
        {
            //获取全局accessToken
            var accessToken = WeHelper.GetAccessToken();

            //获取用户详细信息 参看：http://mp.weixin.qq.com/wiki/14/bb5031008f1494a59c6f71fa0f319c66.html
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", accessToken, openId);
            var json = HttpHelper.Get(url);
            var dic = JsonHelper.ConvertToDictionary(json);

            //获取失败将返回错误码 {"errcode":40013,"errmsg":"invalid appid"}
            if (dic.ContainsKey("errmsg"))
                throw new Exception("获取微信用户信息失败。接口返回：" + json);

            var userInfo = new UserInfo();
            userInfo.Subscribe = dic["subscribe"] == "1";
            if (userInfo.Subscribe) //用户必须关注公众号才能获取到用户其他信息
            {
                userInfo.OpenId = dic["openid"];
                userInfo.NickName = dic["nickname"];
                userInfo.Sex = dic["sex"];
                userInfo.Language = dic["language"];
                userInfo.City = dic["city"];
                userInfo.Province = dic["province"];
                userInfo.Country = dic["country"];
                userInfo.HeadImgUrl = dic["headimgurl"];
                userInfo.SubscribeTime = dic["subscribe_time"];
                //userInfo.UnionId = dic["unionid"];
                userInfo.Remark = dic["remark"];
                userInfo.GroupId = dic["groupid"];

                if (userInfo.Sex == "1")
                    userInfo.Sex = "男";
                else if (userInfo.Sex == "2")
                    userInfo.Sex = "女";
                else
                    userInfo.Sex = "未知";
            }
            return userInfo;
        }

        #endregion

        #region 客服帐号

        /// <summary>
        /// 获得全局ACCESS_TOKEN
        /// </summary>
        /// <returns></returns>
        public static string AddKfAccount(string account, string nickname, string accessToken = null)
        {
            //var data = "{\"kf_account\": \"" + account + "\", \"nickname\": \"" + nickname + "\", \"password\": \"pswmd5\",}";
            var data = "{\"kf_account\" : \"test1@test\",\"nickname\" : \"客服1\",\"password\" : \"pswmd5\",}";

            if (string.IsNullOrEmpty(accessToken))
                accessToken = GetAccessToken();

            var url = "https://api.weixin.qq.com/customservice/kfaccount/add?access_token=" + accessToken;
            var resultJson = HttpHelper.Post(url, data);
            var dic = JsonHelper.ConvertToDictionary(resultJson);
            return dic["errmsg"].ToString();
        }

        /// <summary>
        /// 发送消息(不抛出异常)
        /// </summary>
        /// <returns></returns>
        public static bool SendMessage(string openId, string content, string accessToken = null)
        {
            var success = false;
            try
            {
                var data = "{\"touser\": \"" + openId + "\", \"msgtype\": \"text\", \"text\": {\"content\" : \"" + content + "\"}}";

                if (string.IsNullOrEmpty(accessToken))
                    accessToken = GetAccessToken();

                var url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + accessToken;
                var resultJson = HttpHelper.Post(url, data);
                var dic = JsonHelper.ConvertToDictionary(resultJson);
                success = dic["errmsg"].ToString() == "ok";
            }
            catch { }
            return success;
        }

        #endregion


    }
}
