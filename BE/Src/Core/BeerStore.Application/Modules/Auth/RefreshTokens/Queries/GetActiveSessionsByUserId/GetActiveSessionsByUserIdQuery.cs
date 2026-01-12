using BeerStore.Application.DTOs.Auth.RefreshToken.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Queries.GetActiveSessionsByUserId
{
    public record GetActiveSessionsByUserIdQuery(Guid UserId) : IRequest<IEnumerable<SessionResponse>>;
}