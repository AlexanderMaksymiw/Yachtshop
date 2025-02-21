using Hangfire;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using AlexAPI.Library.Mail;
using AlexAPI.Services.Interfaces;

namespace AlexAPI.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public void SendEmailNow(Mail mail)
        {
            BackgroundJob.Enqueue(() => SendEmailAsync(mail));
        }

        public void SendEmailDelayMinutes(Mail mail, int delay)
        {
            BackgroundJob.Schedule(() => SendEmailAsync(mail), TimeSpan.FromMinutes(delay));
        }

        public void SendEmailDelayHours(Mail mail, int delay)
        {
            BackgroundJob.Schedule(() => SendEmailAsync(mail), TimeSpan.FromHours(delay));
        }

        public void SendEmailDelayDays(Mail mail, int delay)
        {
            BackgroundJob.Schedule(() => SendEmailAsync(mail), TimeSpan.FromDays(delay));
        }

        public void SendEmailAtDateTime(Mail mail, DateTime dateTime)
        {
            BackgroundJob.Schedule(() => SendEmailAsync(mail), dateTime - DateTime.Now);
        }

        public async Task SendEmailAsync(Mail mail)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));

            email.To.Add(MailboxAddress.Parse(mail.ToEmail));


            email.Subject = mail.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mail.Body;
            if (mail.Attachments != null)
            {
                foreach (var file in mail.Attachments)
                {
                    if (file.Length > 0)
                    {
                        builder.Attachments.Add(file);
                    }
                }
            }

            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
