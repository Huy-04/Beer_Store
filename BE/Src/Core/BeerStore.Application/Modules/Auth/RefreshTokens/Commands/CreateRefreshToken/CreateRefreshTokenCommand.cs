using BeerStore.Application.DTOs.Auth.RefreshToken.Requests;
using BeerStore.Application.DTOs.Auth.RefreshToken.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Commands.CreateRefreshToken
{
    public record CreateRefreshTokenCommand(Guid CreatedBy, Guid UpdatedBy, RefreshTokenRequest Request) : IRequest<RefreshTokenResponse>;
}