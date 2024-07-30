using Business_Logic_Layer.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using Business_Logic_Layer.Models;
using Microsoft.Extensions.Options;

namespace Business_Logic_Layer.Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfig _emailConfig;

        public EmailSender(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmailAsync(string receiverEmail, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_emailConfig.SMTPServer, _emailConfig.SMTPPort)
            {
                Credentials = new NetworkCredential(_emailConfig.FromEmail, _emailConfig.FromPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfig.FromEmail),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(receiverEmail);

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send email", ex);
            }
        }
    }
}