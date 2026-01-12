using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Core
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected Guid CurrentUserId
        {
            get
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    throw new UnauthorizedAccessException("Invalid or missing user ID in token");
                }

                return userId;
            }
        }

        protected string? CurrentUserEmail
        {
            get => User.FindFirst(ClaimTypes.Email)?.Value;
        }

        protected List<string> CurrentUserRoles
        {
            get => User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        }
    }
}