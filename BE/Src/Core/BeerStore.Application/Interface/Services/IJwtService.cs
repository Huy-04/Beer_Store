using BeerStore.Domain.ValueObjects.Auth.User;

namespace BeerStore.Application.Interface.Services
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, Email email, List<string> roles);
        string GenerateRefreshToken();
        string HashRefreshToken(string token);
    }
}
