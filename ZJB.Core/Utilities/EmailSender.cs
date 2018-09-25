using System.Net;
using System.Net.Mail;
using System.Text;

namespace ZJB.Core.Utilities
{
    public class EmailSender
    {
        public string SmtpServer { get; set; }

        public int Port { get; set; }

        public string MailUser { get; set; }

        public string MailPassword { get; set; }

        public bool EnableSsl { get; set; }

        public string MailFrom { get; set; }

        public string MailTo { get; set; }

        public bool IsPlainText { get; set; }

        public string DisplayName { get; set; }

        public string ReplyFrom { get; set; }

        public string ReplyDisplayName { get; set; }

        private MailPriority mailPriority = MailPriority.Normal;

        public MailPriority MailPriority
        {
            get { return mailPriority; }
            set{ mailPriority = value;}
        }

        public void Send(string subject, string body)
        {
            MailMessage mailMessage = new MailMessage();

            if (string.IsNullOrEmpty(DisplayName))
                mailMessage.From = new MailAddress(MailFrom);
            else
                mailMessage.From = new MailAddress(MailFrom, DisplayName, Encoding.UTF8);
            mailMessage.Sender = new MailAddress(MailFrom);
            mailMessage.To.Add(MailTo);
            mailMessage.IsBodyHtml = !IsPlainText;
            mailMessage.Subject = subject;

            mailMessage.Body = body;

            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.Priority = mailPriority;
            //add by chenxl 添加邮件回复人
            if (!string.IsNullOrEmpty(ReplyFrom))
            {
                if (string.IsNullOrEmpty(ReplyDisplayName))
                {
                    mailMessage.ReplyTo = new MailAddress(ReplyFrom);
                }
                else {
                    mailMessage.ReplyTo = new MailAddress(ReplyFrom,ReplyDisplayName,Encoding.UTF8);
                }
            }


            SmtpClient smtpClient = new SmtpClient(SmtpServer, Port);
            smtpClient.EnableSsl = EnableSsl;
            smtpClient.Credentials = new NetworkCredential(MailUser, MailPassword);
            smtpClient.Send(mailMessage);
        }
    }
}
