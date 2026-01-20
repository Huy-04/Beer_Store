using BeerStore.Application.DTOs.Auth.RefreshToken.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.RefreshTokenMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Queries.GetAllActiveSession
{
    public class GetAllActiveSessionQHandler : IRequestHandler<GetAllActiveSessionQuery, IEnumerable<SessionResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetAllActiveSessionQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<SessionResponse>> Handle(GetAllActiveSessionQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadAllRefreshTokens();

            var sessions = await _auow.RRefreshTokenRepository.GetAllActiveAsync(token);

            return sessions.Select(s => s.ToSession());
        }
    }
}
