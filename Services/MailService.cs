using Doleance.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.IO;
using System.Threading.Tasks;
namespace Doleance.Services
{
    public class MailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
           
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmail(string mailto , string callbackUrl, string subject, string text)
        {
            var client = new System.Net.Mail.SmtpClient("mail.univ-setif.dz");
            client.UseDefaultCredentials = false;

            client.EnableSsl = false;

            client.Credentials = new System.Net.NetworkCredential("doleances", "SafceD19**");

            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress("doleances@univ-setif.dz");
            mailMessage.To.Add(mailto);
            if (callbackUrl != null)
            {
                mailMessage.Body = string.Format(@"<a href=""{0}"">{1}</a>", callbackUrl, text);
            }
            else
            {
                mailMessage.Body = text;
            }

            mailMessage.Subject = subject;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.IsBodyHtml = true;

           // OnSendEmail(mailMessage);

            await client.SendMailAsync(mailMessage);
        }

       
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.None);
            //smtp.Connect(_mailSettings.Host, _mailSettings.Port, false);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
