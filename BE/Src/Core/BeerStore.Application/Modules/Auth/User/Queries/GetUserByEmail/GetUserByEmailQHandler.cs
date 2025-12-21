using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.User.Queries.GetUserByEmail
{
    public class GetUserByEmailQHandler : IRequestHandler<GetUserByEmailQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetUserByEmailQHandler> _logger;

        public GetUserByEmailQHandler(IAuthUnitOfWork auow, ILogger<GetUserByEmailQHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByEmailQuery query, CancellationToken token)
        {
            var list = await _auow.RUserRepository.FindAsync(u => u.Email == query.Email);
            return list.Select(u => u.ToUserResponse());
        }
    }
}