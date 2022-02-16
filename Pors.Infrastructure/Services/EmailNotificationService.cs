using System;
using System.Net;
using Loby.Tools;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Pors.Application.Common.Interfaces;

namespace Pors.Infrastructure.Services
{
    public class EmailNotificationService : INotificationService
    {
        public class Settings
        {
            public int Port { get; set; }
            public string Host { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private readonly Mailer _mailer;
        private readonly Settings _settings;

        public EmailNotificationService(IOptions<Settings> options)
        {
            _settings = options.Value;

            _mailer = new Mailer(_settings.Host, _settings.Port, _settings.Username, _settings.Password);
        }

        public async Task SendAsync(string recipient, string subject, string body)
        {
           await _mailer.SendAsync(recipient, subject, body);
        }

        public async Task SendAsync(IEnumerable<string> recipients, string subject, string body)
        {
           await _mailer.SendAsync(recipients, subject, body);
        }
    }
}
