using MediatR;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Commands.RevokeRefreshToken
{
    public record RevokeRefreshTokenCommand(Guid UpdatedBy, string TokenHash) : IRequest<bool>;
}