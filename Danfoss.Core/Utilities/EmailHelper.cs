using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;

namespace Danfoss.Core.Utilities
{
    /// <summary>
    /// 邮件帮助类
    /// </summary>
   public static class EmailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="mailSubject">邮箱主题</param>
        /// <param name="mailContent">内容</param>
        /// <returns></returns>
        public static bool SendEmail(string mailTo, string mailSubject, string mailContent)
        {
            // 设置发送方的邮件信息,例如使用网易的smtp
            string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];// SMTP服务器
            string mailFrom = ConfigurationManager.AppSettings["MailFrom"];// 登陆用户名
            string userPassword = ConfigurationManager.AppSettings["MailPassword"];//登陆密码

            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器
            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码

            // 发送邮件设置       
            MailMessage mailMessage = new MailMessage(mailFrom, mailTo); // 发送人和收件人
            mailMessage.Subject = mailSubject;//主题
            mailMessage.Body = mailContent;//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.Low;//优先级

            // 添加附件
            string[] files = new string[] {
                HttpContext.Current.Server.MapPath("~/Content/images/logo@2x.png"),
                HttpContext.Current.Server.MapPath("/Content/images/bg2.png"),
                HttpContext.Current.Server.MapPath("~/Content/images/f_bg.png"),
                HttpContext.Current.Server.MapPath("~/Content/images/erweima.png") };
            for (int i = 0; i < files.Length; i++)
            {
                mailMessage.Attachments.Add(new Attachment(files[i]));
                mailMessage.Attachments[i].ContentType.Name = "image/png";
                mailMessage.Attachments[i].ContentId = "pic" + i;
                mailMessage.Attachments[i].ContentDisposition.Inline = true;
                mailMessage.Attachments[i].TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            }            

            try
            {
                smtpClient.Send(mailMessage); // 发送邮件
                return true;
            }
            catch (SmtpException ex)
            {
                return false;
            }
            finally
            {
                for (int i = 0; i < mailMessage.Attachments.Count; i++) //释放资源
                {
                    mailMessage.Attachments[i].Dispose();
                }
            }
        }
    }
}
