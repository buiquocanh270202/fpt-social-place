using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.MailDTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Application.Services
{
    public class EmailServices
    {
        private readonly IConfiguration _configuration;

        public EmailServices( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string mailto, string subject, string body)
        {
            var _mailSettings = new MailSettings
            {
                Email = _configuration["MailSettings:Email"],
                DisplayName = _configuration["MailSettings:DisplayName"],
                Host = _configuration["MailSettings:Host"],
                Port = int.Parse(_configuration["MailSettings:Port"]),
                Password = _configuration["MailSettings:Password"],
            };
            var email = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject ?? string.Empty,
            };
            email.To.Add(MailboxAddress.Parse(mailto));
            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
