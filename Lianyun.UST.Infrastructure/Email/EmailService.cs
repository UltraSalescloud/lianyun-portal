using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Lianyun.UST.Infrastructure.Config;
using Lianyun.UST.Infrastructure.Logging;

namespace Lianyun.UST.Infrastructure.Email
{
    public class EmailService : IEmail
    {
        private IConfiguration configuration;
        private ILogger logger;
        public EmailService(IConfiguration configuration,Logging.ILogger logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        /// <summary>
        /// 邮件发送(收件者、抄送者、密送者、附件多个的情况采用";"进行分隔)
        /// </summary>
        /// <param name="To">收件者</param>
        /// <param name="Subject">标题</param>
        /// <param name="Body">内容</param>
        /// <returns></returns>
        public bool SendEmail(string To, string Subject, string Message)
        {
            //检查邮件配置是否已经加载
            if (string.IsNullOrEmpty(configuration.SmtpServer))
            {
                return false;
            }

            string strSmtpServer = configuration.SmtpServer;
            string strSmtpUser = configuration.EmailFromAddress;
            string strSmtpUserDisplayName = configuration.EmailFromDisplayName;
            string strSmtpPassword = configuration.EmailFromPassword;

            bool bResult = SendSmtpMail(strSmtpServer, strSmtpUser, strSmtpUserDisplayName, strSmtpPassword, To, Subject, Message, null, null, null);

            return bResult;
        }

        public bool SendEmail(string to, string cc, string bcc, string subject, string message)
        {
            //检查邮件配置是否已经加载
            if (string.IsNullOrEmpty(configuration.SmtpServer))
            {
                return false;
            }

            string strSmtpServer = configuration.SmtpServer;
            string strSmtpUser = configuration.EmailFromAddress;
            string strSmtpUserDisplayName = configuration.EmailFromDisplayName;
            string strSmtpPassword = configuration.EmailFromPassword;

            bool bResult = SendSmtpMail(strSmtpServer, strSmtpUser, strSmtpUserDisplayName, strSmtpPassword, to, subject, message, cc, bcc, null);

            return bResult;
        }

        public bool SendEmail(string[] to, string[] cc, string[] bcc, string subject, string message)
        {
            string strTos = string.Join(";", to);
            string strCcs = string.Join(";", cc);
            string strBccs = string.Join(";", bcc);
            return SendEmail(strTos, strCcs, strBccs, subject, message);
        }


        /// <summary>
        /// 邮件发送(收件者、抄送者、密送者、附件多个的情况采用";"进行分隔)
        /// </summary>
        /// <param name="SmtpServer">SMTP邮件服务器</param>
        /// <param name="From">发件人邮件地址</param>
        /// <param name="FromDisplayName">发件人显示名称</param>
        /// <param name="FromPwd">发件人邮件密码</param>
        /// <param name="To">收件者</param>
        /// <param name="Subject">标题</param>
        /// <param name="Body">内容</param>
        /// <param name="Cc">抄送者</param>
        /// <param name="Bcc">密送者</param>
        /// <param name="Attachment">附件</param>
        /// <returns></returns>
        private static bool SendSmtpMail(string SmtpServer, string From, string FromDisplayName, string FromPwd, string To, string Subject, string Body, string Cc, string Bcc, string Attachment)
        {
            try
            {
                // 创建邮件服务器对象
                SmtpClient mSmtpServer = new SmtpClient(SmtpServer);
                //使用的端口   
                mSmtpServer.Port = 587;
                mSmtpServer.UseDefaultCredentials = false;
                mSmtpServer.Credentials = new System.Net.NetworkCredential(From, FromPwd);
                mSmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                // 创建邮件消息对象
                MailMessage mMessage = new MailMessage();
                mMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mMessage.IsBodyHtml = true;

                // 发件人
                mMessage.From = new MailAddress(From, FromDisplayName, Encoding.UTF8);
                // 收件人
                if (!string.IsNullOrEmpty(To))
                {
                    To = To.Replace(",", ";");
                    string[] mTo = To.Split(';');
                    foreach (string kTo in mTo)
                    {
                        mMessage.To.Add(new MailAddress(kTo, kTo, Encoding.UTF8));
                    }
                }
                // 抄送人
                if (!string.IsNullOrEmpty(Cc))
                {
                    Cc = Cc.Replace(",", ";");
                    string[] mCc = Cc.Split(';');
                    foreach (string kCc in mCc)
                    {
                        if (!To.Contains(kCc))
                        {
                            mMessage.CC.Add(new MailAddress(kCc, kCc, Encoding.UTF8));
                        }
                    }
                }
                // 抄送人
                if (!string.IsNullOrEmpty(Bcc))
                {
                    Bcc = Bcc.Replace(",", ";");
                    string[] mBcc = Bcc.Split(';');
                    foreach (string kBcc in mBcc)
                    {
                        if (!To.Contains(kBcc) || !Cc.Contains(kBcc))
                        {
                            mMessage.Bcc.Add(new MailAddress(kBcc, kBcc, Encoding.UTF8));
                        }
                    }
                }
                // 附件
                if (!string.IsNullOrEmpty(Attachment))
                {
                    Attachment = Attachment.Replace(",", ";");
                    string[] mAttachment = Attachment.Split(';');
                    foreach (string kAttach in mAttachment)
                    {
                        mMessage.Attachments.Add(new Attachment(kAttach));
                    }
                }
                // 主题
                mMessage.Subject = Subject;
                // 内容
                mMessage.Body = Body;

                // 发送邮件
                mSmtpServer.Send(mMessage);
            }
            catch (Exception ex)
            {
                //记录错误日志
                //LogHelper.WriteLog(ex.ToString());

                return false;
            }

            return true;
        }


    }
}
