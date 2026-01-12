using BeerStore.Application.DTOs.Auth.RefreshToken.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.RefreshTokenMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Queries.GetAllActiveSession
{
    public class GetAllActiveSessionQHandler : IRequestHandler<GetAllActiveSessionQuery, IEnumerable<SessionResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetAllActiveSessionQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<SessionResponse>> Handle(GetAllActiveSessionQuery query, CancellationToken token)
        {
            var sessions = await _auow.RRefreshTokenRepository.GetAllActiveAsync(token);

            return sessions.Select(s => s.ToSession());
        }
    }
}