using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.User.Queries.GetAllUser
{
    public class GetAllUserQHandler : IRequestHandler<GetAllUserQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetAllUserQHandler> _logger;

        public GetAllUserQHandler(IAuthUnitOfWork auow, ILogger<GetAllUserQHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetAllUserQuery query, CancellationToken token)
        {
            var list = await _auow.RUserRepository.GetAllAsync(token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}