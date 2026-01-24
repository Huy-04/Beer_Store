using Application.Core.Constants;
using Application.Core.Interface.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Core.Services
{
    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Guid.TryParse(userIdClaim, out var id) ? id : Guid.Empty;
            }
        }

        public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;

        public IReadOnlyList<string> Roles => User?.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList() ?? new List<string>();

        public IReadOnlyList<string> Permissions => User?.Claims
            .Where(c => c.Type == CustomClaimTypes.Permission)
            .Select(c => c.Value)
            .ToList() ?? new List<string>();

        public bool HasPermission(string permission)
            => Permissions.Contains(permission);

        public bool HasAnyPermission(params string[] permissions)
            => permissions.Any(p => Permissions.Contains(p));
    }
}
