using Danfoss.Core;
using Danfoss.Core.Utilities;
using Danfoss.Core.We;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;

namespace Danfoss.Controllers
{
    public class WeController : BaseController
    {
        // GET: We
        public ActionResult Index()
        {
            return View();
        }



        #region 微信交互

        /// <summary>
        /// 公众号平台对接入口
        /// </summary>
        /// <returns></returns>
        public ActionResult Interface()
        {
            string result = string.Empty;
            try
            {
                if (CheckSignature())
                {
                    if (Request.HttpMethod == WebRequestMethods.Http.Get)
                        result = HandleGet();
                    else if (Request.HttpMethod == WebRequestMethods.Http.Post)
                        result = HandlePost();
                }
                else
                {
                    Lgr.Log.Debug("校验消息失败");
                    //_logService.RecordLog("", "", LogLevel.Debug, "微信事件", "校验消息失败。\r\n地址：" + Request.RawUrl);
                }
            }
            catch (Exception ex)
            {
                Lgr.Log.Error(ex.Message, ex);
               // _logService.RecordLog("", "", LogLevel.Debug, "微信事件", ex.Message + "\r\n详细错误：" + ex.StackTrace);
            }
            return Content(result);
        }

        public ActionResult Test()
        {
            WeHelper.GetUserInfo("orbpLt2PcsB8gIbkkR7f4exf1Hb8");
            return View();

            //var ret1 = WeHelper.AddKfAccount("jckf1", "客服人员", accessToken);
            //var ret2 = WeHelper.SendMessage("orbpLt2PcsB8gIbkkR7f4exf1Hb8", "取消订单 50165465446 成功！", accessToken);
            //return Content("");
        }

        /// <summary>
        /// 处理Post请求
        /// </summary>
        /// <returns>响应电文</returns>
        private string HandlePost()
        {
            //读取发过来的信息到inputXml变量中
            var sin = Request.InputStream;
            var readBytes = new byte[sin.Length];
            sin.Read(readBytes, 0, readBytes.Length);
            var inputXml = Encoding.UTF8.GetString(readBytes);

            Lgr.Log.Debug("微信事件："+inputXml);

            //使用XMLDocument加载信息结构
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(inputXml);

            //解析XML数据到字典
            var fields = new Dictionary<string, string>();
            foreach (XmlNode x in xmlDoc.SelectSingleNode("/xml").ChildNodes)
                fields.Add(x.Name, x.InnerText);

            var returnXml = string.Empty;

            #region 根据不同的参数处理相应的微信事件
            //说明：目前需求暂时只处理关注事件

            //新用户关注公众号或扫描酒店二维码
            if (fields.ContainsKey("MsgType") && fields.ContainsKey("Event")
                && fields["MsgType"] == "event" && fields["Event"] == "subscribe")
                returnXml = HandleSubscribe(fields);
            //老用户扫描酒店二维码
            else if (fields.ContainsKey("MsgType") && fields.ContainsKey("Event")
                && fields["MsgType"] == "event" && fields["Event"] == "SCAN")
                returnXml = HandleScan(fields);
            //点击公众号链接菜单
            else if (fields.ContainsKey("MsgType") && fields.ContainsKey("Event")
                && fields["MsgType"] == "event" && fields["Event"] == "VIEW")
                returnXml = HandleEventView(fields);
            //点击公众号事件菜单
            else if (fields.ContainsKey("MsgType") && fields.ContainsKey("Event")
                && fields["MsgType"] == "event" && fields["Event"] == "CLICK")
                returnXml = HandleEventClick(fields);
            //用户回复文本消息
            else if (fields.ContainsKey("MsgType") && fields["MsgType"] == "text")
                returnXml = HandleText(fields);
            #endregion

            return returnXml;
        }

        /// <summary>
        /// 处理微信的GET请求，校验签名
        /// </summary>
        /// <param name="context"></param>
        /// <returns>响应电文</returns>
        private string HandleGet()
        {
            return Request["echostr"];
        }

        /// <summary>
        /// 处理关注事件
        /// </summary>
        /// <param name="fields"></param>
        /// <returns>响应电文</returns>
        private string HandleSubscribe(Dictionary<string, string> fields)
        {
            var returnXml = string.Empty;          

            return returnXml;
        }

        /// <summary>
        /// 处理扫描二维码事件
        /// </summary>
        /// <param name="fields"></param>
        /// <returns>响应电文</returns>
        private string HandleScan(Dictionary<string, string> fields)
        {
            var returnXml = "success";
            //if (fields.ContainsKey("EventKey")) // && fields["EventKey"].StartsWith("H")
            //{
            //    //老用户扫描酒店二维码
            //    var eventKey = fields["EventKey"];
            //    var wxScene = _wxSceneService.GetBySceneValue(eventKey);
            //    returnXml = ScanningHotelImage(wxScene, fields["FromUserName"], fields["ToUserName"]);
            //}

            return returnXml;
        }

        /// <summary>
        /// 处理用户公众号VIEW事件
        /// </summary>
        /// <param name="fields"></param>
        /// <returns>响应电文</returns>
        private string HandleEventView(Dictionary<string, string> fields)
        {
            try
            {
                //var openid = fields["FromUserName"];
                //var customer = _customerService.GetByWeAccount(openid);
                //if (customer != null)
                //{
                //    #region 给没有可用券、且订单都完成了的用户发送优惠券
                //    if (!_couponService.ExistsUsableCoupon(customer)
                //        && _bookingOrderService.IsAllOrderFinished(customer))
                //    {
                //        var coupon = _couponService.SendCoupon(customer, CouponKey.COUPON_2, CouponType.Coupon);
                //        if (coupon != null) //发券成功后，给微信用户发送消息
                //            WeHelper.SendMessage(customer.WeAccount, string.Format("您获得了{0}{1}元，可以在预定酒店的时候使用!", coupon.CouponName, FormatHelper.ShowMoney(coupon.CouponAmount)));
                //    }
                    //#endregion
                //}
            }
            catch (Exception ex)
            {
                //_logService.Error("公众号处理VIEW事件出错", ex);
            }

            return "success"; //微信推荐的回复方式 success
        }

        /// <summary>
        /// 处理用户发送文本消息事件
        /// </summary>
        /// <param name="fields"></param>
        /// <returns>响应电文</returns>
        private string HandleText(Dictionary<string, string> fields)
        {
            var returnXml = string.Empty;
            try
            {
                var fromUserName = fields["FromUserName"];
                var toUserName = fields["ToUserName"];
                var content = fields["Content"];

                #region 呼叫酒店服务
                returnXml = CallHotelService(fromUserName, toUserName, content);
                #endregion

                #region 非呼叫呼叫酒店服务的信息，转给客服处理
                if (string.IsNullOrEmpty(returnXml))
                {
                    //如果用户输入的内容不为呼叫服务，则转给在线客户处理
                    var xmlFormat = @"<xml>
                                        <ToUserName><![CDATA[{0}]]></ToUserName>
                                        <FromUserName><![CDATA[{1}]]></FromUserName>
                                        <CreateTime>{2}</CreateTime>
                                        <MsgType><![CDATA[transfer_customer_service]]></MsgType>                  
                                     </xml>";
                    returnXml = string.Format(xmlFormat,
                        fromUserName,
                        toUserName,
                        DateTime.Now.Subtract(new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds.ToString());
                }
                #endregion
            }
            catch (Exception ex)
            {
               // _logService.Error("处理用户发送文本消息事件出错", ex);
            }

            if (string.IsNullOrEmpty(returnXml))
                returnXml = "success"; //微信推荐的回复方式 success

            //_logService.Info("呼叫服务回复微信消息：" + returnXml);

            return returnXml;
        }

        /// <summary>
        /// 客户呼叫酒店服务
        /// </summary>
        /// <param name="fromUserName"></param>
        /// <param name="toUserName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private string CallHotelService(string fromUserName, string toUserName, string content)
        {
            //var message = string.Empty;
            //var customer = _customerService.GetByWeAccount(fromUserName);
            //if (customer == null)
            //    return null;

            ////判断当前用户是否入住
            //var checkinInfo = _checkinInfoService.GetCheckinInfo(customer);
            //if (checkinInfo == null)
            //    return null;

            ////如果入住了，根据用户和入住酒店判断Content是否是获取服务
            //var serviceSetting = _serviceSettingService.GetServiceSetting(checkinInfo.HotelId, content);
            //if (serviceSetting == null)
            //    return null;

            //if (serviceSetting.AutoReply)
            //{
            //    //自动回复，取服务设置的自动回复内容
            //    message = serviceSetting.AutoReplyContent;
            //}
            //else
            //{
            //    message = "您呼叫的服务已经通知柜台，柜台将会尽快联系您！";
            //    //根据不同服务返回内容(推送到PMS系统的消息)
            //    var messageToPms = _serviceSettingService.GetServiceMessage(serviceSetting, content);
            //    //把服务信息推送给PMS
            //    SwitchServiceAgent.ApplyForService(checkinInfo, messageToPms, 7); //Switch约定：7 呼叫服务 8 购物消息
            //}

            var xmlFormat = @"<xml>
                                <ToUserName><![CDATA[{0}]]></ToUserName>
                                <FromUserName><![CDATA[{1}]]></FromUserName>
                                <CreateTime>{2}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>                  
                                <Content><![CDATA[{3}]]></Content>                  
                             </xml>";

            ////回复微信用户信息
            //var returnXml = string.Format(xmlFormat,
            //      fromUserName,
            //      toUserName,
            //      DateTime.Now.Subtract(new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds.ToString(),
            //      message);

            return xmlFormat;
        }

        /// <summary>
        /// 处理点击菜单事件
        /// </summary>
        /// <returns></returns>
        private string HandleEventClick(Dictionary<string, string> fields)
        {
            var returnXml = "success";
            //try
            //{
            //    var fromUserName = fields["FromUserName"];
            //    var toUserName = fields["ToUserName"];
            //    var eventKey = fields["EventKey"];

            //    switch (eventKey)
            //    {
            //        #region 联系客服
            //        case "Event_Contact_Service":
            //            {
            //                //如果用户输入的内容不为呼叫服务，则转给在线客户处理
            //                var xmlFormat = @"<xml>
            //                            <ToUserName><![CDATA[{0}]]></ToUserName>
            //                            <FromUserName><![CDATA[{1}]]></FromUserName>
            //                            <CreateTime>{2}</CreateTime>
            //                            <MsgType><![CDATA[text]]></MsgType>                  
            //                            <Content><![CDATA[{3}]]></Content>                  
            //                         </xml>";
            //                returnXml = string.Format(xmlFormat,
            //                    fields["FromUserName"],
            //                    fields["ToUserName"],
            //                    DateTime.Now.Subtract(new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds.ToString(),
            //                    "您好！请问有什么可以帮到您？\n您可直接在本聊天窗口输入对话进行咨询\n或拨打客服电话：400-0731-831。\n我们会尽快回复您~谢谢您的关注！");
            //            }

            //            break;
            //            #endregion
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logService.Error("公众号处理点击菜单事件出错", ex);
            //}

            return returnXml;
        }

        /// <summary>
        /// 扫描酒店图片返回图文消息
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="fromUserName"></param>
        /// <param name="toUserName"></param>
        /// <returns></returns>
        //private string ScanningHotelImage(WxScene wxScene, string fromUserName, string toUserName)
        //{
        //    var returnXml = string.Empty;

        //    //微信场景值配置的处理事件
        //    if (wxScene != null && wxScene.SceneValue.Length != 6)
        //    {
        //        var title = string.Empty;
        //        var description = string.Empty;
        //        var picUrl = string.Empty;
        //        var url = string.Empty;

        //        //场景里配置的酒店Ids
        //        var hotelIds = wxScene.MappingData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //                        .Select(id => int.Parse(id)).ToArray();
        //        #region 单个酒店
        //        if (hotelIds.Count() == 1)
        //        {
        //            var hotel = _hotelService.GetById(hotelIds[0]);
        //            title = hotel.HotelName;
        //            description = hotel.Description;
        //            picUrl = hotel.CoverUrl;
        //            url = Constants.SiteDomain + "/hotel/detail?id=" + hotel.Id;
        //        }
        //        #endregion
        //        #region 多个酒店
        //        else if (hotelIds.Count() > 1)
        //        {
        //            var hotels = _hotelService.GetByIds(hotelIds);
        //            title = wxScene.Title;
        //            picUrl = hotels[0].CoverUrl;
        //            url = Constants.SiteDomain + "/hotel/HotelList?WxSceneId=" + wxScene.Id; //酒店列表根据WxSceneId显示对应的酒店

        //            for (var i = 0; i < hotels.Count(); i++)
        //            {
        //                var hotel = hotels[i];
        //                description = description + hotel.HotelName + Environment.NewLine
        //                    + "地址：" + hotel.Address + Environment.NewLine
        //                    + "电话：" + hotel.ContactPhone;
        //                if (i < hotels.Count - 1)
        //                    description = description + Environment.NewLine + Environment.NewLine;
        //            }
        //        }
        //        #endregion

        //        #region 返回微信电文
        //        var xmlFormat = @"<xml>
        //                            <ToUserName><![CDATA[{0}]]></ToUserName>
        //                            <FromUserName><![CDATA[{1}]]></FromUserName>
        //                            <CreateTime>{2}</CreateTime>
        //                            <MsgType><![CDATA[news]]></MsgType>
        //                            <ArticleCount>1</ArticleCount>
        //                            <Articles>                   
        //                                <item>
        //                                    <Title><![CDATA[{3}]]></Title> 
        //                                    <Description>{4}</Description>
        //                                    <PicUrl>{5}</PicUrl>
        //                                    <Url><![CDATA[{6}]]></Url>
        //                                </item>                         
        //                            </Articles>
        //                         </xml>";

        //        returnXml = string.Format(xmlFormat,
        //                fromUserName,
        //                toUserName,
        //                DateTime.Now.Subtract(new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds.ToString(),
        //                title,
        //                description,
        //                picUrl,
        //                url);
        //        #endregion
        //    }
        //    else if (wxScene != null && wxScene.SceneValue.Length == 6)
        //    {
        //        var title = "简单点智慧酒店商城";
        //        var picUrl = Constants.SiteDomain + "/Content/shop/images/zc.jpg";
        //        var url = Constants.SiteDomain + "/product/zclist";
        //        var description = string.Empty;

        //        //酒店配置的附带信息（比如WIFI信息）
        //        var hotelId = 0;
        //        int.TryParse(wxScene.MappingData, out hotelId);
        //        if (hotelId > 0)
        //        {
        //            var hotelMessage = _shoppingHotelMessageService.GetMany(m => m.HotelId == hotelId).FirstOrDefault();
        //            if (hotelMessage != null && !string.IsNullOrEmpty(hotelMessage.Message))
        //                description = hotelMessage.Message;
        //        }

        //        #region 返回微信电文
        //        var xmlFormat = @"<xml>
        //                            <ToUserName><![CDATA[{0}]]></ToUserName>
        //                            <FromUserName><![CDATA[{1}]]></FromUserName>
        //                            <CreateTime>{2}</CreateTime>
        //                            <MsgType><![CDATA[news]]></MsgType>
        //                            <ArticleCount>1</ArticleCount>
        //                            <Articles>                   
        //                                <item>
        //                                    <Title><![CDATA[{3}]]></Title> 
        //                                    <Description>{4}</Description>
        //                                    <PicUrl>{5}</PicUrl>
        //                                    <Url><![CDATA[{6}]]></Url>
        //                                </item>                         
        //                            </Articles>
        //                         </xml>";

        //        returnXml = string.Format(xmlFormat,
        //                fromUserName,
        //                toUserName,
        //                DateTime.Now.Subtract(new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds.ToString(),
        //                title,
        //                description,
        //                picUrl,
        //                url);
        //        #endregion
        //    }

        //    return returnXml;
        //}

        /// <summary>
        /// 生成酒店二维码
        /// </summary>
        /// <returns></returns>
        public ActionResult HotelImg()
        {
            //var key = "003000";
            //if (Request["key"] != null)
            //    key = Request["key"];
            //var token = WeHelper.GetAccessToken();

            ////临时二维码
            ////var data = "{\"expire_seconds\": 604800, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + key + "}}}";
            ////永久二维码
            ////var data = "{\"action_name\":\"QR_LIMIT_SCENE\",\"action_info\":{\"scene\":{\"scene_id\":" + key + "}}}";
            //var data = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": \"" + key + "\"}}}";

            //var retTicketJson = HttpHelper.Post(string.Format("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}", token), data);
            //var retTicket = JsonHelper.ConvertToDictionary(retTicketJson);
            //var ticket = retTicket["ticket"].ToString();
            //ViewBag.PicUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticket;
            return View();
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckSignature()
        {
            var signature = Request["signature"];
            var timestamp = Request["timestamp"];
            var nonce = Request["nonce"];
            var arrTmp = new string[] { "weixin", timestamp, nonce };
            Array.Sort(arrTmp);　//字典排序　
            var tmpStr = string.Join("", arrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            return (tmpStr == signature);
        }

        #endregion
    }
}