using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string IpAddress { get; }
        string ProfilePicture { get; }
        public string DisplayName { get; }
    }
}