using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserMap;
using BeerStore.Domain.ValueObjects.Auth.User.Status;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByUserStatus
{
    public class GetUserByUserStatusQHandler : IRequestHandler<GetUserByUserStatusQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetUserByUserStatusQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByUserStatusQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadAllUsers();

            var userStatus = UserStatus.Create(query.UserStatus);
            var list = await _auow.RUserRepository.FindAsync(u => u.UserStatus == userStatus, token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}

