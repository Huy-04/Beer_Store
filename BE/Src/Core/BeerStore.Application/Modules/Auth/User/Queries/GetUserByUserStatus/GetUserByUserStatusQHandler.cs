using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using BeerStore.Domain.ValueObjects.Auth.User.Status;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.User.Queries.GetUserByUserStatus
{
    public class GetUserByUserStatusQHandler : IRequestHandler<GetUserByUserStatusQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetUserByUserStatusQHandler> _logger;

        public GetUserByUserStatusQHandler(IAuthUnitOfWork auow, ILogger<GetUserByUserStatusQHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByUserStatusQuery query, CancellationToken token)
        {
            var userStatus = UserStatus.Create(query.UserStatus);
            var list = await _auow.RUserRepository.FindAsync(u => u.UserStatus == userStatus, token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}