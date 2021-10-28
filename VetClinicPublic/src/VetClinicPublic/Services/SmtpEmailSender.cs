using System.Net.Mail;
using Microsoft.Extensions.Options;
using VetClinicPublic.Configuration;
using VetClinicPublic.Interfaces;

namespace VetClinicPublic.Services
{
    public class SmtpEmailSender : ISendEmail
    {
        private readonly MailConfiguration _mailConfiguration;

        public SmtpEmailSender(IOptions<MailConfiguration> mailConfiguration)
        {
            _mailConfiguration = mailConfiguration.Value;
        }

        public void SendEmail(string to, string from, string subject, string body)
        {
            using var smtp = new SmtpClient(_mailConfiguration.Host, _mailConfiguration.Port);
            var email = new MailMessage();

            email.To.Add(to);
            email.From = new MailAddress(from);
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;

            smtp.Send(email);
        }
    }
}
