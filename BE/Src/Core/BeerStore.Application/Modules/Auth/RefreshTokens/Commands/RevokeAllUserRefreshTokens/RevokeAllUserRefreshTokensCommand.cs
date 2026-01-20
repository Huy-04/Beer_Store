using MediatR;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Commands.RevokeAllUserRefreshTokens
{
    public record RevokeAllUserRefreshTokensCommand(Guid UserId, Guid UpdatedBy) : IRequest<bool>;
}