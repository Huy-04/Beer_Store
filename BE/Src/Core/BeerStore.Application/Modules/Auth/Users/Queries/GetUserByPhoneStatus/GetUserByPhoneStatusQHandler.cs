using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserMap;
using BeerStore.Domain.ValueObjects.Auth.User.Status;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByPhoneStatus
{
    public class GetUserByPhoneStatusQHandler : IRequestHandler<GetUserByPhoneStatusQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetUserByPhoneStatusQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByPhoneStatusQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadAllUsers();

            var phoneStatus = PhoneStatus.Create(query.PhoneStatus);
            var list = await _auow.RUserRepository.FindAsync(u => u.PhoneStatus == phoneStatus, token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}

