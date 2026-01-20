using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByPhone
{
    public class GetUserByPhoneQHandler : IRequestHandler<GetUserByPhoneQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetUserByPhoneQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByPhoneQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadAllUsers();

            var list = await _auow.RUserRepository.FindAsync(u => u.Phone == query.Phone, token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}

