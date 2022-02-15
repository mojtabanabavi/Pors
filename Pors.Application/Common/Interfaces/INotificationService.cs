using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(string recipient, string subject, string body);
        Task SendAsync(IEnumerable<string> recipients, string subject, string body);
    }
}