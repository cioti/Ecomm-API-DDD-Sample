using Ecomm.Domain.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Ecomm.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Username => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        //public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");
        public string UserId => Guid.NewGuid().ToString();
    }
}
