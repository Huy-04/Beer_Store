using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.User.Queries.GetUserByUserName
{
    public class GetUserByUserNameQHandler : IRequestHandler<GetUserByUserNameQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetUserByUserNameQHandler> _logger;

        public GetUserByUserNameQHandler(IAuthUnitOfWork auow, ILogger<GetUserByUserNameQHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByUserNameQuery query, CancellationToken token)
        {
            var list = await _auow.RUserRepository.FindAsync(u => u.UserName == query.UserName, token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}