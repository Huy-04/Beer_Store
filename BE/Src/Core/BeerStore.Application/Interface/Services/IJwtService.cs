using BeerStore.Domain.ValueObjects.Auth.User;
using Domain.Core.ValueObjects.Common;

namespace BeerStore.Application.Interface.Services
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, Email email, List<string> roles, List<string> permissions);
        string GenerateRefreshToken();
        string HashRefreshToken(string token);
    }
}
