using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Pors.Application.Common.Interfaces;

namespace Pors.Website.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string DisplayName => GetClaimValue(ClaimTypes.Name);
        public string UserId => GetClaimValue(ClaimTypes.NameIdentifier);
        public string ProfilePicture => GetClaimValue("ProfilePicture");

        private string GetClaimValue(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType);
        }
    }
}
