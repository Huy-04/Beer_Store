using BeerStore.Application.DTOs.Auth.RefreshToken.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.RefreshTokenMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Queries.GetActiveSessionsByUserId
{
    public class GetActiveSessionsByUserIdQHandler : IRequestHandler<GetActiveSessionsByUserIdQuery, IEnumerable<SessionResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetActiveSessionsByUserIdQHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public GetActiveSessionsByUserIdQHandler(IAuthUnitOfWork auow, ILogger<GetActiveSessionsByUserIdQHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<IEnumerable<SessionResponse>> Handle(GetActiveSessionsByUserIdQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadRefreshToken(query.UserId);

            try
            {
                var sessions = await _auow.RRefreshTokenRepository.GetActiveByUserIdAsync(query.UserId, token);

                return sessions.Select(s => s.ToSession());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get active sessions. UserId: {UserId}", query.UserId);
                throw;
            }
        }
    }
}
