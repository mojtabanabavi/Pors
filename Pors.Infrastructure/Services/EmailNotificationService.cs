using System;
using Loby.Tools;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Pors.Application.Common.Interfaces;

namespace Pors.Infrastructure.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly Mailer _mailer;

        public EmailNotificationService(IOptions<MailerSettings> options)
        {
            _mailer = new Mailer(options.Value);
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
