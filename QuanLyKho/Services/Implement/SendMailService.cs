using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using QuanLyKho.Models;

namespace QuanLyKho.Services.Implement
{
    public class SendMailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        public SendMailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public class MailContent
        {
            public string To { get; set; }
            public string Subject { get; set; }
            public string Content { get; set; }
        }


        public async Task SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);

            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));

            email.To.Add(new MailboxAddress(mailContent.To, mailContent.To));
            email.Subject = mailContent.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Content;

            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.PassWord);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("gửi mail thất bai !!!!!!!");
                Console.WriteLine(ex.Message);
                return;
            }
            smtp.Disconnect(true);
            Console.WriteLine("gửi mail thành công !!!");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mess = new MimeMessage();
            mess.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            mess.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));

            mess.To.Add(MailboxAddress.Parse(email));
            mess.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;

            mess.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.PassWord);
                await smtp.SendAsync(mess);
            }
            catch (Exception ex)
            {
                Console.WriteLine("gửi mail thất bai !!!!!!!");
                Console.WriteLine(ex.Message);
            }
            smtp.Disconnect(true);
            Console.WriteLine("gửi mail thành công !!!");
        }
    }
}
