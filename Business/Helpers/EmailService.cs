using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Business.Helpers
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        // Inject IConfiguration to read from appsettings.json
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Generic method to send an email
        public async Task<bool> SendEmail(string to, string subject, string body)
        {
            try
            {
                // SMTP settings from configuration
                string userName = _configuration["EmailSettings:UserName"];
                string password = _configuration["EmailSettings:Password"];
                string smtpHost = _configuration["EmailSettings:Host"];
                int smtpPort = int.Parse(_configuration["EmailSettings:Port"]);

                // Create MailMessage
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(userName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(new MailAddress(to));

                // Configure SMTP client
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = smtpHost,
                    Port = smtpPort,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(userName, password),
                    EnableSsl = true
                };


                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
                return true; // Email sent successfully
            }
            catch (Exception)
            {
                return false; // Email failed
            }
        }

        // Generic method to read template and replace placeholders
        public string PrepareEmailBody(string templatePath, Dictionary<string, string> placeholders)
        {
            try
            {
                // Read the HTML template
                string body = File.ReadAllText(templatePath);

                // Replace placeholders dynamically
                foreach (var placeholder in placeholders)
                {
                    body = body.Replace(placeholder.Key, placeholder.Value);
                }

                return body;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error preparing email body: {ex.Message}");
                return string.Empty;
            }
        }




    }
}
