using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Pors.Application.Common.Interfaces;

namespace Pors.Website.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string DisplayName => GetClaimValue(ClaimTypes.Name);
        public string ProfilePicture => GetClaimValue("ProfilePicture") ?? "~/img/avatars/default.jpg";
        public string UserId => GetClaimValue(ClaimTypes.NameIdentifier);

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetClaimValue(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType);
        }
    }
}
