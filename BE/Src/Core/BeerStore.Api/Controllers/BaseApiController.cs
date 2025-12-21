using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeerStore.Api.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Lấy UserId từ JWT claims của user đã đăng nhập
        /// </summary>
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

        /// <summary>
        /// Lấy Email từ JWT claims
        /// </summary>
        protected string? CurrentUserEmail
        {
            get => User.FindFirst(ClaimTypes.Email)?.Value;
        }

        /// <summary>
        /// Lấy danh sách Roles từ JWT claims
        /// </summary>
        protected List<string> CurrentUserRoles
        {
            get => User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        }
    }
}
