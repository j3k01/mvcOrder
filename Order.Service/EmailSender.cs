using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Order.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                string smtpServer = _configuration["EmailSettings:SmtpServer"];
                int smtpPort = _configuration.GetValue<int>("EmailSettings:SmtpPort");
                string smtpUsername = _configuration["EmailSettings:SmtpUsername"];
                string smtpPassword = _configuration["EmailSettings:SmtpPassword"];

                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                mailMessage.From = new MailAddress(smtpUsername);
                mailMessage.To.Add(email);
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = htmlMessage;

                smtpClient.Port = smtpPort;
                smtpClient.Host = smtpServer;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}
