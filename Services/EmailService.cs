using BlogMVC.ConfigurationModels;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BlogMVC.Services
{
    public class EmailService: IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string emailTo, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSettings.SmtpUserName);
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.SmtpHost, _emailSettings.SmtpTlsPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.SmtpUserName, _emailSettings.SmtpPassword);

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

    }

}
