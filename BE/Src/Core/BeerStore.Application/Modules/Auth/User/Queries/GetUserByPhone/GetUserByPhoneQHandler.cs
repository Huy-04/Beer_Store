using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.User.Queries.GetUserByPhone
{
    public class GetUserByPhoneQHandler : IRequestHandler<GetUserByPhoneQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetUserByPhoneQHandler> _logger;

        public GetUserByPhoneQHandler(IAuthUnitOfWork auow, ILogger<GetUserByPhoneQHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByPhoneQuery query, CancellationToken token)
        {
            var list = await _auow.RUserRepository.FindAsync(u => u.Phone == query.Phone, token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}