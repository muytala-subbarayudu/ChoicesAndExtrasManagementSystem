using ChoicesExtrasManagement.Interfaces;
using System.Net.Mail;
using System.Net;

namespace ChoicesExtrasManagement.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var emailAccount = _configuration["EmailAccount"];
            var emailPassword = _configuration["EmailPassword"];

            var client = new SmtpClient("smtp-mail.outlook.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailAccount,emailPassword)
                };

                return client.SendMailAsync(
                    new MailMessage(from: emailAccount,
                                    to: email,
                                    subject,
                                    message
                                    ));
            
        }

        public Task SendEmailSelfAsync(string subject, string message)
        {
            var emailAccount = _configuration["EmailAccount"];
            var emailPassword = _configuration["EmailPassword"];

            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("modernhomeslimited@outlook.com", "ModernHomes2023#")
            };

            return client.SendMailAsync(
                new MailMessage(from: emailAccount,
                                to: emailAccount,
                                subject,
                                message
                                ));
        }
    }
}
