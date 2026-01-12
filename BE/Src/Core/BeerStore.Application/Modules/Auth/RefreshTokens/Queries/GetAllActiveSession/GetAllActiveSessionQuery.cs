using BeerStore.Application.DTOs.Auth.RefreshToken.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Queries.GetAllActiveSession
{
    public record class GetAllActiveSessionQuery : IRequest<IEnumerable<SessionResponse>>
    {
    }
}